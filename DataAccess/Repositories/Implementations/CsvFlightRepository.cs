using DataAccess.Csv;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.SearchCriteria;

namespace DataAccess.Repositories.Implementations;

public class CsvFlightRepository : IFlightRepository
{
  public readonly string PathToCsv = "../../../../DataAccess/CsvFiles/flights.csv";
  
  private readonly IImportFromCsvService<FlightCsvImportDto, Flight> _importFromCsvService;
  
  private readonly ICsvWriter _writer;
  
  public CsvFlightRepository(IImportFromCsvService<FlightCsvImportDto, Flight> importFromCsvService, ICsvWriter writer)
  {
    _importFromCsvService = importFromCsvService;

    _writer = writer;
  }
  
  public async Task<IEnumerable<Flight>> GetAll()
  {
    var importResult = await _importFromCsvService.ImportFromCsv(PathToCsv);
    
    return importResult
      .ValidObjects
      .DistinctBy(flight => flight.Id);
  }
  
  public async Task<IEnumerable<Flight>> GetMatchingCriteria(FlightSearchCriteria criteria)
  {
    var allFlights = await GetAll();
    
    return allFlights
      .Where(criteria.Matches);
  }
  
  public async Task<Flight?> GetById(int id)
  {
    var allFlights = await GetAll();
    
    return allFlights.FirstOrDefault(flight => flight.Id == id);
  }
  
  public async Task Add(IEnumerable<Flight> flights)
  {  
    await _writer.AppendAsync(PathToCsv, flights);
  }
}