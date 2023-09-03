using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Implementations;
using BusinessLogic.Tests.Services.TestData;
using BusinessLogic.Utilites;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Exceptions;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.SearchCriteria;

namespace BusinessLogic.Tests.Services;

public class FlightServiceTests
{
  private readonly FlightService _sut;
  
  private readonly Mock<IFlightRepository> _flightRepositoryMock;
  
  private readonly Mock<IBookingRepository> _bookingRepositoryMock;

  private readonly Mock<IImportFromCsvService<FlightCsvImportDto, Flight>> _importFromCsvServiceMock;

  private readonly Mock<IMapper<Flight, FlightDto>> _flightMapperMock;

  private readonly Fixture _fixture;

  public FlightServiceTests()
  {
    _flightRepositoryMock = new();

    _bookingRepositoryMock = new();

    _importFromCsvServiceMock = new();

    _flightMapperMock = new();
    
    _flightMapperMock
      .Setup(x => x.Map(It.IsAny<Flight>()))
      .Returns(new Func<Flight?, FlightDto?>(flight => flight is null ? null : new FlightDto { Id = flight.Id }));

    _sut = new(_flightRepositoryMock.Object, _bookingRepositoryMock.Object, _importFromCsvServiceMock.Object,
      _flightMapperMock.Object);

    _fixture = new();

    _flightRepositoryMock
      .Setup(x => x.GetMatchingCriteria(It.Is<FlightSearchCriteria>(criteria => criteria.Class == FlightClass.Economy)))
      .ReturnsAsync(new List<Flight> { FlightTestData.Flights[0] });
    
    _flightRepositoryMock
      .Setup(x => x.GetMatchingCriteria(It.Is<FlightSearchCriteria>(criteria => criteria.Class == FlightClass.Business || criteria.Class == FlightClass.FirstClass)))
      .ReturnsAsync(new List<Flight> { FlightTestData.Flights[1] });

    _flightRepositoryMock
      .Setup(x => x.GetMatchingCriteria(It.Is<FlightSearchCriteria>(criteria => criteria.Class == null)))
      .ReturnsAsync(FlightTestData.Flights);

    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(1, FlightClass.Economy))
      .ReturnsAsync(_fixture.CreateMany<Booking>(4));

    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(2, FlightClass.Business))
      .ReturnsAsync(_fixture.CreateMany<Booking>(5));
    
    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(2, FlightClass.FirstClass))
      .ReturnsAsync(_fixture.CreateMany<Booking>(5));
  }
  
  [Fact]
  public async Task GetById_FlightDoesNotExist_ShouldReturnNull()
  {
    var id = _fixture.Create<int>();

    var actual = await _sut.GetById(id);

    actual.Should().BeNull();
  }

  [Fact]
  public async Task GetById_FlightExists_ShouldReturnTheExpectedDto()
  {
    var id = 1;

    var flight = new Flight { Id = id };

    _flightRepositoryMock
      .Setup(x => x.GetById(id))
      .ReturnsAsync(flight);

    var flightDto = new FlightDto { Id = id };

    var actual = await _sut.GetById(id);

    actual.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(flightDto);
  }

  [Fact]
  public async Task ImportFromCsv_FileWithPathDoesNotExist_ShouldThrowFileNotFoundException()
  {
    var path = "IDONTEXIST";

    _importFromCsvServiceMock
      .Setup(x => x.ImportFromCsv(It.Is<string>(path => !File.Exists(path))))
      .ThrowsAsync(new FileNotFoundException());

    var act = async () => await _sut.ImportFromCsv(path);

    await act.Should().ThrowAsync<FileNotFoundException>();
  }

  [Fact]
  public async Task ImportFromCsv_FileWithPathIsNotCsv_ShouldThrowNotACsvFileException()
  {
    var path = "not_csv";
    
    _importFromCsvServiceMock
      .Setup(x => x.ImportFromCsv(It.Is<string>(path => Path.GetExtension(path) != ".csv")))
      .ThrowsAsync(new NotACsvFileException(_fixture.Create<string>()));
    
    var act = async () => await _sut.ImportFromCsv(path);

    await act.Should().ThrowAsync<NotACsvFileException>();
  }

  [Fact]
  public async Task ImportFromCsv_CsvFile_ShouldAddDistinctFlightsToTheRepository()
  {
    var flights = FlightTestData.ReadFromCsvFlights;
    
    _importFromCsvServiceMock
      .Setup(x => x.ImportFromCsv(It.IsAny<string>()))
      .ReturnsAsync(new DataImportResult<Flight>
      {
        ValidObjects = flights
      });

    await _sut.ImportFromCsv(_fixture.Create<string>());

    var expectedFlightsToAdd = flights.DistinctBy(flight => flight.Id);
    
    _flightRepositoryMock.Verify(x => x.Add(expectedFlightsToAdd), Times.Once);
  }

  [Fact]
  public async Task ImportFromCsv_CsvFile_ShouldReturnTheExpectedValidationErrors()
  {
    var validationErrors = _fixture.CreateMany<string>().ToList();
    
    _importFromCsvServiceMock
      .Setup(x => x.ImportFromCsv(It.IsAny<string>()))
      .ReturnsAsync(new DataImportResult<Flight>
      {
        ValidObjects = new List<Flight>(),
        ValidationErrors = validationErrors
      });

    var actual = await _sut.ImportFromCsv(_fixture.Create<string>());

    actual.Should().BeSameAs(validationErrors);
  }

  [Fact]
  public async Task GetAvailableFlightsMatchingCriteria_FlightClassIsNotAvailable_ShouldNotReturnTheFlight()
  {
    var criteria = new FlightSearchCriteria { Class = FlightClass.Economy };

    var actual = await _sut.GetAvailableFlightsMatchingCriteria(criteria);

    actual.Should().NotContain(flight => flight.Id == 1);
  }

  [Fact]
  public async Task
    GetAvailableFlightsMatchingCriteria_FlightIsAvailableButSpecifiedClassIsNot_ShouldNotReturnTheFlight()
  {
    var criteria = new FlightSearchCriteria { Class = FlightClass.Business };
    
    var actual = await _sut.GetAvailableFlightsMatchingCriteria(criteria);

    actual.Should().NotContain(dto => dto.Id == 2);
  }

  [Fact]
  public async Task GetAvailableFlightsMatchingCriteria_CriteriaDoesNotSpecifyClass_ShouldReturnAvailableFlights()
  {
    var criteria = new FlightSearchCriteria();

    var actual = await _sut.GetAvailableFlightsMatchingCriteria(criteria);

    actual.Should().BeEquivalentTo(new List<FlightDto> { new() { Id = 2 } });
  }
}