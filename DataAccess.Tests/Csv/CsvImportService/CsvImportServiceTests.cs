using DataAccess.Csv;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Csv.Parsers;
using DataAccess.Csv.Validators;
using DataAccess.Exceptions;
using DataAccess.Models;

namespace DataAccess.Tests.Csv.CsvImportService;

public class CsvImportServiceTests
{
  private readonly Fixture _fixture;

  private readonly Mock<IMapper<BookingCsvImportDto, Booking>> _mapperMock;

  private readonly Mock<IParser<BookingCsvImportDto>> _parserMock;

  private readonly Mock<ICsvReader> _readerMock;
  
  private readonly ImportFromCsvService<BookingCsvImportDto, Booking> _sut;

  private readonly Mock<IValidator> _validatorMock;

  public CsvImportServiceTests()
  {
    _mapperMock = new Mock<IMapper<BookingCsvImportDto, Booking>>();

    _parserMock = new Mock<IParser<BookingCsvImportDto>>();

    _readerMock = new Mock<ICsvReader>();

    _validatorMock = new Mock<IValidator>();

    _fixture = new Fixture();

    _sut = new ImportFromCsvService<BookingCsvImportDto, Booking>(_parserMock.Object, _validatorMock.Object,
      _mapperMock.Object, _readerMock.Object);
  }

  [Fact]
  public void ImportFromCsv_FileDoesNotExists_ShouldThrowFileNotFoundException()
  {
    var path = "IDONTEXIST";

    _readerMock
      .Setup(x => x.ReadAsync(It.Is<string>(str => !File.Exists(str)), _parserMock.Object))
      .ThrowsAsync(new FileNotFoundException());

    var act = async () => await _sut.ImportFromCsv(path);

    act.Should().ThrowExactlyAsync<FileNotFoundException>();
  }

  [Fact]
  public void ImportFromCsv_NotACsvFile_ShouldNotACsvFileException()
  {
    var path = "bla/bla/bla/not_csv";

    _readerMock
      .Setup(x => x.ReadAsync(It.Is<string>(str => Path.GetExtension(str) != ".csv"), _parserMock.Object))
      .ThrowsAsync(new NotACsvFileException(path));

    var act = async () => await _sut.ImportFromCsv(path);

    act.Should().ThrowExactlyAsync<NotACsvFileException>();
  }

  [Fact]
  public async Task ImportFromCsv_InvalidObjects_ValidatorsGetErrorMessagesShouldBeCalledByTheNumberOfInvalidObject()
  {
    var objects = new List<BookingCsvImportDto?> { CreateTestBookingDto(-1), null };

    _readerMock
      .Setup(x => x.ReadAsync(It.IsAny<string>(), _parserMock.Object))
      .ReturnsAsync(objects);

    _validatorMock
      .Setup(x => x.GetErrorMessages(It.IsAny<object?>()))
      .Returns(new List<string>());

    await _sut.ImportFromCsv(_fixture.Create<string>());

    _validatorMock.Verify(x => x.GetErrorMessages(It.IsAny<object?>()), Times.Exactly(objects.Count));
  }

  private BookingCsvImportDto CreateTestBookingDto(int id)
  {
    return new()
    {
      Id = id,
      BookingDate = new DateTime(2020, 12, 30),
      FlightClass = FlightClass.Business,
      FlightId = 1,
      PassengerId = 1
    };
  } 

  [Fact]
  public async Task ImportFromCsv_InvalidObjects_ShouldReturnCorrespondingErrorMessages()
  {
    var objects = new List<BookingCsvImportDto?>
    {
      CreateTestBookingDto(-1),
      null
    };

    _readerMock
      .Setup(x => x.ReadAsync(It.IsAny<string>(), _parserMock.Object))
      .ReturnsAsync(objects);

    _validatorMock
      .Setup(x => x.GetErrorMessages(objects[0]))
      .Returns(new List<string>
      {
        "Id should be greater than 0"
      });

    _validatorMock
      .Setup(x => x.GetErrorMessages(objects[1]))
      .Returns(new List<string>
      {
        "Invalid Object: # of parameters is not identical."
      });

    var result = await _sut.ImportFromCsv(_fixture.Create<string>());

    var expected = new List<string>
    {
      "In line 2:",
      "Id should be greater than 0",
      "In line 3:",
      "Invalid Object: # of parameters is not identical."
    };

    result.ValidationErrors.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public async Task ImportFromCsv_ReferentialIntegrityFails_ShouldReturnAMessageIndicatingThat()
  {
    var objects = new List<BookingCsvImportDto> { CreateTestBookingDto(1) };

    _readerMock
      .Setup(x => x.ReadAsync(It.IsAny<string>(), _parserMock.Object))
      .ReturnsAsync(objects);

    _validatorMock
      .Setup(x => x.IsValid(It.IsAny<object?>()))
      .Returns(true);

    _mapperMock
      .Setup(x => x.Map(objects[0]))
      .Throws<IntegrityException>();
    
    var result = await _sut.ImportFromCsv(_fixture.Create<string>());
    
    var expected = new List<string>
    {
      "In line 2:",
      "Data integrity is not applicable to this object"
    };
    
    result.ValidationErrors.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public async Task ImportFromCsv_ReferentialIntegrityFails_MapperMapShouldBeCalledByTheNoOfFailingObjects()
  {
    var objects = new List<BookingCsvImportDto>
    {
      CreateTestBookingDto(-1)
    };

    _readerMock
      .Setup(x => x.ReadAsync(It.IsAny<string>(), _parserMock.Object))
      .ReturnsAsync(objects);

    _validatorMock
      .Setup(x => x.IsValid(It.IsAny<object?>()))
      .Returns(true);

    _mapperMock
      .Setup(x => x.Map(objects[0]))
      .Throws<IntegrityException>();

    await _sut.ImportFromCsv(It.IsAny<string>());
    
    _mapperMock.Verify(x => x.Map(It.IsAny<BookingCsvImportDto>()), Times.Exactly(objects.Count));
  }
  
  [Fact]
  public async Task ImportFromCsv_ValidObjects_MappersMapShouldBeCalledByTheNumberOfValidObject()
  {
    var objects = new List<BookingCsvImportDto?>
    {
      CreateTestBookingDto(1)
    };

    _readerMock
      .Setup(x => x.ReadAsync(It.IsAny<string>(), _parserMock.Object))
      .ReturnsAsync(objects);

    _validatorMock
      .Setup(x => x.IsValid(It.IsAny<object?>()))
      .Returns(true);

    await _sut.ImportFromCsv(_fixture.Create<string>());

    _mapperMock.Verify(x => x.Map(It.IsAny<BookingCsvImportDto>()), Times.Exactly(objects.Count));
  }
  
  [Fact]
  public async Task ImportFromCsv_ValidObjects_ExpectedObjectsShouldBeReturned()
  {
    var objects = new List<BookingCsvImportDto?>
    {
      CreateTestBookingDto(1)
    };

    _readerMock
      .Setup(x => x.ReadAsync(It.IsAny<string>(), _parserMock.Object))
      .ReturnsAsync(objects);

    _validatorMock
      .Setup(x => x.IsValid(It.IsAny<object?>()))
      .Returns(true);

    var expected = new List<Booking>();

    for (var i = 0; i < objects.Count; ++i)
    {
      expected.Add(new Booking
      {
        Id = objects[i].Id.Value,
        BookingDate = objects[i].BookingDate.Value,
        BookingClass = objects[i].FlightClass.Value,
        Flight = new Flight { Id = objects[i].FlightId.Value },
        Passenger = new Passenger { Id = objects[i].PassengerId.Value }
      });
    }

    for (var i = 0; i < objects.Count; ++i)
    {
      _mapperMock
        .Setup(x => x.Map(objects[i]))
        .Returns(expected[i]);
    }

    var result = await _sut.ImportFromCsv(_fixture.Create<string>());

    result.ValidObjects.Should().BeEquivalentTo(expected);
  }
}