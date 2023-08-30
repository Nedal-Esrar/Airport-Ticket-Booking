namespace DataAccess.Csv;

public class CsvWriter<T> where T : ICsvWritable
{
  private readonly string _pathToFile;

  public CsvWriter(string pathToFile)
  {
    _pathToFile = pathToFile;
  }

  public void Write(IEnumerable<T> objects)
  {
    using var writer = new StreamWriter(_pathToFile);
    
    writer.WriteLine(T.GetHeader());

    foreach (var obj in objects)
    {
      writer.WriteLine(obj.GetCsvRecord());
    }
  }

  public async Task WriteAsync(IEnumerable<T> objects)
  {
    await using var writer = new StreamWriter(_pathToFile);
    
    foreach (var obj in objects)
    {
      await writer.WriteLineAsync(obj.GetCsvRecord());
    }
  }

  public void Append(IEnumerable<T> objects)
  {
    using var writer = new StreamWriter(_pathToFile, true);

    foreach (var obj in objects)
    {
      writer.WriteLine(obj.GetCsvRecord());
    }
  }
  
  public void Append(T obj)
  {
    using var writer = new StreamWriter(_pathToFile, true);
    
    writer.WriteLine(obj.GetCsvRecord());
  }

  public async Task AppendAsync(IEnumerable<T> objects)
  {
    await using var writer = new StreamWriter(_pathToFile, true);
    
    foreach (var obj in objects)
    {
      await writer.WriteLineAsync(obj.GetCsvRecord());
    }
  }
  
  public async Task AppendAsync(T obj)
  {
    await using var writer = new StreamWriter(_pathToFile, true);
    
    await writer.WriteLineAsync(obj.GetCsvRecord());
  }
}