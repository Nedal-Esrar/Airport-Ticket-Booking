using DataAccessLayer.Csv.CsvImportService;
using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations;

public class CsvPassengerRepository : IPassengerRepository
{
  private const string PathToCsv = "../../../../DataAccessLayer/CsvFiles/passengers.csv";

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