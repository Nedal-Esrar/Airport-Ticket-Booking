namespace DataAccess.Csv.CsvImportService;

public class DataImportResult<TOutput>
{
  public IEnumerable<TOutput> ValidObjects { get; init; }

  public IList<string> ValidationErrors { get; init; }
}