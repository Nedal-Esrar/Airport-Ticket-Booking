using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Utilites;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.SearchCriteria;

namespace BusinessLogic.Services.Implementations;

public class FlightService : IFlightService
{
  private readonly IFlightRepository _flightRepository;
  
  private readonly IBookingRepository _bookingRepository;

  private readonly IImportFromCsvService<FlightCsvImportDto, Flight> _importFromCsvService;

  private readonly IMapper<Flight, FlightDto> _flightMapper;

  public FlightService(
    IFlightRepository flightRepository, 
    IBookingRepository bookingRepository, 
    IImportFromCsvService<FlightCsvImportDto, Flight> importFromCsvService,
    IMapper<Flight, FlightDto> flightMapper
  )
  {
    _flightRepository = flightRepository;

    _bookingRepository = bookingRepository;

    _importFromCsvService = importFromCsvService;

    _flightMapper = flightMapper;
  }

  public async Task<FlightDto?> GetById(int id)
  {
    var flight = await _flightRepository.GetById(id);

    return _flightMapper.Map(flight);
  }

  public async Task<IEnumerable<FlightDto>> GetAvailableFlightsMatchingCriteria(FlightSearchCriteria criteria)
  {
    var flightMatchingCriteria = await _flightRepository.GetMatchingCriteria(criteria);

    var availableFlightsMatchingCriteria = criteria.Class is null
      ? flightMatchingCriteria.Where(flight => flight.Classes
        .Any(details => FlightUtilities.IsClassAvailableToBook(flight, details.Class, _bookingRepository).Result))
      : flightMatchingCriteria.Where(flight => FlightUtilities.IsClassAvailableToBook(flight, criteria.Class.Value, _bookingRepository).Result);

    return availableFlightsMatchingCriteria.Select(flight => _flightMapper.Map(flight));
  }

  public async Task<IList<string>> ImportFromCsv(string filePath)
  {
    var importResult = await _importFromCsvService.ImportFromCsv(filePath);
    
    await _flightRepository.Add(importResult
      .ValidObjects
      .DistinctBy(flight => flight.Id)
    );
    
    return importResult.ValidationErrors;
  }
}