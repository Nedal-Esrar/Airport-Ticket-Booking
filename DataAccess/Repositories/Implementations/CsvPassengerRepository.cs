using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories.Implementations;

public class CsvPassengerRepository : IPassengerRepository
{
  private const string PathToCsv = "../../../../DataAccess/CsvFiles/passengers.csv";

  private readonly IImportFromCsvService<PassengerCsvImportDto, Passenger> _importFromCsvService;

  public CsvPassengerRepository(IImportFromCsvService<PassengerCsvImportDto, Passenger> importFromCsvService)
  {
    _importFromCsvService = importFromCsvService;
  }
  
  public Passenger? GetById(int id)
  {
    return _importFromCsvService
      .ImportFromCsv(PathToCsv)
      .ValidObjects
      .DistinctBy(passenger => passenger.Id)
      .FirstOrDefault(passenger => passenger.Id == id);
  }
}