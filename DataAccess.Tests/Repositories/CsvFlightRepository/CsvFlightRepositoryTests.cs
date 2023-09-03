using DataAccess.Csv;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Models;
using DataAccess.Repositories.Implementations;
using DataAccess.SearchCriteria;

namespace DataAccess.Tests.Repositories;

public class CsvFlightRepositoryTests
{
  private readonly CsvFlightRepository _sut;

  private readonly Mock<IImportFromCsvService<FlightCsvImportDto, Flight>> _importFromCsvServiceMock;

  private readonly Mock<ICsvWriter> _csvWriterMock;

  private readonly List<Flight> _flights;

  private readonly Fixture _fixture;

  public CsvFlightRepositoryTests()
  {
    _flights = TestData.Flights;

    _importFromCsvServiceMock = new();

    _csvWriterMock = new();

    _sut = new CsvFlightRepository(_importFromCsvServiceMock.Object, _csvWriterMock.Object);
    
    _importFromCsvServiceMock
      .Setup(x => x.ImportFromCsv(_sut.PathToCsv))
      .ReturnsAsync(new DataImportResult<Flight>
      {
        ValidObjects = _flights
      });

    _fixture = new Fixture();
  }

  [Fact]
  public async Task GetAll_AllFlights_ShouldReturnAllFlightsWithFirstOccuringIds()
  {
    var expected = _flights.DistinctBy(flight => flight.Id);

    var returnedFlights = await _sut.GetAll();

    returnedFlights.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public async Task GetById_RequiredFlightExists_ShouldReturnTheFirstFlightWithRequestedId()
  {
    var expected = _flights.First();

    var returnedFlight = await _sut.GetById(expected.Id);

    returnedFlight.Should().BeSameAs(expected);
  }

  [Fact]
  public async Task GetById_RequiredFlightDoesNotExist_ShouldReturnNull()
  {
    var id = 1000;

    var returnedFlight = await _sut.GetById(id);

    returnedFlight.Should().BeNull();
  }

  [Theory]
  [MemberData(nameof(TestData.CriteriaTestData), MemberType = typeof(TestData))]
  public async Task GetMatchingCriteria_Criteria_ShouldReturnExpectedFlights(FlightSearchCriteria criteria, IEnumerable<Flight> expected)
  {
    var returnedFlights = await _sut.GetMatchingCriteria(criteria);

    returnedFlights.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public async Task Add_Flights_CsvWriterWriteAsyncShouldBeCalledOnceWithFlights()
  {
    var flightsToAdd = _fixture.CreateMany<Flight>();

    await _sut.Add(flightsToAdd);
    
    _csvWriterMock.Verify(x => x.AppendAsync(_sut.PathToCsv, flightsToAdd), Times.Once);
  }
}