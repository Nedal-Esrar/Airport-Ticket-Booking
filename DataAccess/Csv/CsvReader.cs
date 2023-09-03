using DataAccess.Csv.Parsers;
using DataAccess.Exceptions;

namespace DataAccess.Csv;

public class CsvReader : ICsvReader
{
  public async Task<List<T?>> ReadAsync<T>(string pathToFile, IParser<T> parser)
  {
    ValidateCsvFile(pathToFile);
    
    var readObjects = new List<T?>();

    using var reader = new StreamReader(pathToFile);

    if (reader.EndOfStream) return readObjects;

    await reader.ReadLineAsync(); // skip the header

    while (!reader.EndOfStream)
    {
      var line = await reader.ReadLineAsync();

      var fields = line?.Split(',');

      var instance = parser.Parse(fields);

      readObjects.Add(instance);
    }
  
    return readObjects;
  }
  
  private void ValidateCsvFile(string pathToFile)
  {
    if (!File.Exists(pathToFile))
    {
      throw new FileNotFoundException();
    }

    if (Path.GetExtension(pathToFile) != ".csv")
    {
      throw new NotACsvFileException(pathToFile);
    }
  }
}