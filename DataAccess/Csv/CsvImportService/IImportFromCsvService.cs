namespace DataAccess.Csv.CsvImportService;

public interface IImportFromCsvService<TInput, TOutput>
{
  Task<DataImportResult<TOutput>> ImportFromCsv(string filePath);
}