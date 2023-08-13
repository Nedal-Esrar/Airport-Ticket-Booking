namespace DataAccessLayer.Csv.CsvImportService;

public interface IImportFromCsvService<TInput, TOutput>
{
  DataImportResult<TOutput> ImportFromCsv(string filePath);
}