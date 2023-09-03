using DataAccess.Csv;
using DataAccess.Exceptions;
using DataAccess.Models;

namespace DataAccess.Tests.Csv.Writer;

public class WriterTests
{
  private readonly CsvWriter _sut = new();

  private readonly Fixture _fixture = new();
  
  [Fact]
  public async Task WriteAsync_FileThatDoesNotExist_ShouldThrowFileNotFoundException()
  {
    var path = "IDONTEXIST";

    var act = async () => await _sut.WriteAsync(path, _fixture.CreateMany<Passenger>());

    await act.Should().ThrowExactlyAsync<FileNotFoundException>();
  }
  
  [Fact]
  public async Task AppendAsync_SingleObject_FileThatDoesNotExist_ShouldThrowFileNotFoundException()
  {
    var path = "IDONTEXIST";

    var act = async () => await _sut.AppendAsync(path, _fixture.Create<Passenger>());

    await act.Should().ThrowExactlyAsync<FileNotFoundException>();
  }
  
  [Fact]
  public async Task AppendAsync_EnumerableOfObjects_FileThatDoesNotExist_ShouldThrowFileNotFoundException()
  {
    var path = "IDONTEXIST";

    var act = async () => await _sut.AppendAsync(path, _fixture.CreateMany<Passenger>());

    await act.Should().ThrowExactlyAsync<FileNotFoundException>();
  }
  
  [Fact]
  public async Task WriteAsync_NotACsvFile_ShouldThrowNotACsvFileException()
  {
    var path = "../../../Csv/Writer/TestCsvFiles/not_csv";

    var act = async () => await _sut.WriteAsync(path, _fixture.CreateMany<Passenger>());

    await act.Should().ThrowExactlyAsync<NotACsvFileException>();
  }
  
  [Fact]
  public async Task AppendAsync_SingleObject_NotACsvFile_ShouldThrowNotACsvFileException()
  {
    var path = "../../../Csv/Writer/TestCsvFiles/not_csv";

    var act = async () => await _sut.AppendAsync(path, _fixture.Create<Passenger>());

    await act.Should().ThrowExactlyAsync<NotACsvFileException>();
  }
  
  [Fact]
  public async Task AppendAsync_EnumerableOfObjects_NotACsvFile_ShouldThrowNotACsvFileException()
  {
    var path = "../../../Csv/Writer/TestCsvFiles/not_csv";

    var act = async () => await _sut.AppendAsync(path, _fixture.CreateMany<Passenger>());

    await act.Should().ThrowExactlyAsync<NotACsvFileException>();
  }

  [Fact]
  public async Task WriteAsync_CsvFile_ShouldWriteObjectsToTheFileWithTheCorrespondingHeader()
  {
    var path = "../../../Csv/Writer/TestCsvFiles/file.csv";
    
    var passengers = _fixture.CreateMany<Passenger>();

    await _sut.WriteAsync(path, passengers);

    var expected = $"""
                    {Passenger.GetHeader()}
                    {string.Join(Environment.NewLine, passengers.Select(p => p.GetCsvRecord()))}
                    """;

    var actual = string.Join(Environment.NewLine, await File.ReadAllLinesAsync(path));

    actual.Should().Be(expected);
  }
  
  [Fact]
  public async Task AppendAsync_SingleObject_CsvFile_ShouldAppendTheObjectToTheFile()
  {
    var path = "../../../Csv/Writer/TestCsvFiles/file.csv";
    
    var passenger = _fixture.Create<Passenger>();

    var oldContent = string.Join(Environment.NewLine, await File.ReadAllLinesAsync(path));

    await _sut.AppendAsync(path, passenger);

    var expected = $"""
                    {oldContent}
                    {passenger.GetCsvRecord()}
                    """;

    var actual = string.Join(Environment.NewLine, await File.ReadAllLinesAsync(path));

    actual.Should().Be(expected);
  }
  
  [Fact]
  public async Task AppendAsync_EnumerableOfObjects_CsvFile_ShouldAppendObjectsToTheFile()
  {
    var path = "../../../Csv/Writer/TestCsvFiles/file.csv";
    
    var passengers = _fixture.CreateMany<Passenger>();

    var oldContent = string.Join(Environment.NewLine, await File.ReadAllLinesAsync(path));

    await _sut.AppendAsync(path, passengers);

    var expected = $"""
                    {oldContent}
                    {string.Join(Environment.NewLine, passengers.Select(p => p.GetCsvRecord()))}
                    """;

    var actual = string.Join(Environment.NewLine, await File.ReadAllLinesAsync(path));

    actual.Should().Be(expected);
  }
}