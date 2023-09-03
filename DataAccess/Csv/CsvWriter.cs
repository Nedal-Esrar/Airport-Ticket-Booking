using DataAccess.Exceptions;

namespace DataAccess.Csv;

public class CsvWriter : ICsvWriter
{
  public async Task WriteAsync<T>(string pathToFile, IEnumerable<T> objects) where T : ICsvWritable
  {
    ValidateCsvFile(pathToFile);
    
    await using var writer = new StreamWriter(pathToFile);

    await writer.WriteLineAsync(T.GetHeader());
    
    foreach (var obj in objects)
    {
      await writer.WriteLineAsync(obj.GetCsvRecord());
    }
  }

  public async Task AppendAsync<T>(string pathToFile, IEnumerable<T> objects) where T : ICsvWritable
  {
    ValidateCsvFile(pathToFile);
    
    await using var writer = new StreamWriter(pathToFile, true);
    
    foreach (var obj in objects)
    {
      await writer.WriteLineAsync(obj.GetCsvRecord());
    }
  }
  
  public async Task AppendAsync<T>(string pathToFile, T obj) where T : ICsvWritable
  {
    ValidateCsvFile(pathToFile);
    
    await using var writer = new StreamWriter(pathToFile, true);
    
    await writer.WriteLineAsync(obj.GetCsvRecord());
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