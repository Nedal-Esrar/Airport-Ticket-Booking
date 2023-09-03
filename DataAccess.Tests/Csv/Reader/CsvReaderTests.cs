using DataAccess.Csv;
using DataAccess.Csv.Dtos;
using DataAccess.Csv.Parsers;
using DataAccess.Exceptions;
using FluentAssertions.Execution;

namespace DataAccess.Tests.Csv.Reader;

public class CsvReaderTests
{
  private readonly CsvReader _sut = new();

  private readonly Mock<IParser<PassengerCsvImportDto>> _passengerCsvDtoParserMock = new();

  public CsvReaderTests()
  {
    foreach (var (record, parserObject) in TestData.PassengerRecordsAndCorrespondingObjects)
    {
      _passengerCsvDtoParserMock
        .Setup(x => x.Parse(record))
        .Returns(parserObject);
    }
  }
  
  [Fact]
  public async Task ReadAsync_FileThatDoesNotExist_ShouldThrowFileNotFoundException()
  {
    var path = "IDONTEXIST";

    var act = async () => await _sut.ReadAsync(path, _passengerCsvDtoParserMock.Object);

    await act.Should().ThrowExactlyAsync<FileNotFoundException>();
  }
  
  [Fact]
  public async Task ReadAsync_NotACsvFile_ShouldThrowNotACsvFileException()
  {
    var path = "../../../Csv/Reader/TestCsvFiles/not_csv";

    var act = async () => await _sut.ReadAsync(path, _passengerCsvDtoParserMock.Object);

    await act.Should().ThrowExactlyAsync<NotACsvFileException>();
  }
  
  [Fact]
  public async Task ReadAsync_EmptyCsvFile_ShouldReturnAnEmptyList()
  {
    var path = "../../../Csv/Reader/TestCsvFiles/empty.csv";

    var returnedObjects = await _sut.ReadAsync(path, _passengerCsvDtoParserMock.Object);

    returnedObjects.Should().BeEmpty();
  }
  
  [Fact]
  public async Task ReadAsync_NonEmptyValidCsvFiles_ShouldReturnExpectedParsedObjects()
  {
    var path = "../../../Csv/Reader/TestCsvFiles/valid.csv";

    var returnedObjects = await _sut.ReadAsync(path, _passengerCsvDtoParserMock.Object);

    returnedObjects.Should().HaveCount(2);

    using (new AssertionScope())
    {
      for (var i = 0; i < 2; ++i)
      {
        returnedObjects[i].Should().BeEquivalentTo(TestData.PassengerRecordsAndCorrespondingObjects[i].Item2);
      }
    }
  }
}