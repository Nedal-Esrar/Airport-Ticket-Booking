using DataAccess.Csv;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.SearchCriteria;

namespace DataAccess.Repositories.Implementations;

public class CsvFlightRepository : IFlightRepository
{
  private const string PathToCsv = "../../../../DataAccess/CsvFiles/flights.csv";

  private readonly IImportFromCsvService<FlightCsvImportDto, Flight> _importFromCsvService;

  private readonly CsvWriter<Flight> _writer;

  public CsvFlightRepository(IImportFromCsvService<FlightCsvImportDto, Flight> importFromCsvService)
  {
    _importFromCsvService = importFromCsvService;
    
    _writer = new CsvWriter<Flight>(PathToCsv);
  }

  public IEnumerable<Flight> GetAll()
  {
    return _importFromCsvService
      .ImportFromCsv(PathToCsv)
      .ValidObjects
      .DistinctBy(flight => flight.Id);
  }

  public IEnumerable<Flight> GetMatchingCriteria(FlightSearchCriteria criteria)
  {
    return GetAll()
      .Where(criteria.Matches);
  }

  public Flight? GetById(int id)
  {
    return GetAll()
      .FirstOrDefault(flight => flight.Id == id);
  }

  public void Add(IEnumerable<Flight> flights)
  {
    _writer.Append(flights);
  }
}