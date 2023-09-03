namespace DataAccess.Csv;

public interface ICsvWriter
{
  Task WriteAsync<T>(string pathToFile, IEnumerable<T> objects) where T : ICsvWritable;
  
  Task AppendAsync<T>(string pathToFile, IEnumerable<T> objects) where T : ICsvWritable;
  
  Task AppendAsync<T>(string pathToFile, T obj) where T : ICsvWritable;
}