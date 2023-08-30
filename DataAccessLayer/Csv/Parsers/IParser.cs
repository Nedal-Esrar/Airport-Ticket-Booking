namespace DataAccessLayer.Csv.Parsers;

public interface IParser<TInput>
{
  TInput? Parse(string[] fields);
}