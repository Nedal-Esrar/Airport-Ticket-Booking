namespace DataAccessLayer.Csv;

public class CsvReader<T>
{
  private readonly string _pathToFile;

  public CsvReader(string pathToFile)
  {
    _pathToFile = pathToFile;
  }
  
  public async Task<List<T?>> ReadAsync(Func<string[], T> parseFunc)
  {
    var readObjects = new List<T?>();
    
    using var reader = new StreamReader(_pathToFile);

    if (reader.EndOfStream)
    {
      return readObjects;
    }

    await reader.ReadLineAsync(); // skip the header

    while (!reader.EndOfStream)
    {
      var line = await reader.ReadLineAsync();

      var fields = line?.Split(',');

      var instance = parseFunc(fields);
      
      readObjects.Add(instance);
    }

    return readObjects;
  }

  public List<T?> Read(Func<string[], T> parseFunc)
  {
    var readObjects = new List<T?>();
    
    using var reader = new StreamReader(_pathToFile);

    if (reader.EndOfStream)
    {
      return readObjects;
    }

    reader.ReadLine(); // skip the header

    while (!reader.EndOfStream)
    {
      var line = reader.ReadLine();

      var fields = line?.Split(',');

      var instance = parseFunc(fields);
      
      readObjects.Add(instance);
    }

    return readObjects;
  }
}