using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories.Implementations;

public class CsvPassengerRepository : IPassengerRepository
{
  public readonly string PathToCsv = "../../../../DataAccess/CsvFiles/passengers.csv";
  
  private readonly IImportFromCsvService<PassengerCsvImportDto, Passenger> _importFromCsvService;
  
  public CsvPassengerRepository(IImportFromCsvService<PassengerCsvImportDto, Passenger> importFromCsvService)
  {
    _importFromCsvService = importFromCsvService;
  }
  
  public async Task<Passenger?> GetById(int id)
  {
    var importResult = await _importFromCsvService.ImportFromCsv(PathToCsv);

    return importResult
      .ValidObjects
      .DistinctBy(passenger => passenger.Id)
      .FirstOrDefault(passenger => passenger.Id == id);
  }
}