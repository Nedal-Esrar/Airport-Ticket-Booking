using DataAccess.Csv.Parsers;

namespace DataAccess.Csv;

public interface ICsvReader
{
  Task<List<T?>> ReadAsync<T>(string pathToFile, IParser<T> parser);
}