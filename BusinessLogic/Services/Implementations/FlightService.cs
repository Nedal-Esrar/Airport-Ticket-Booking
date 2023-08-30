using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using DataAccessLayer.Csv.CsvImportService;
using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.SearchCriteria;

namespace BusinessLogic.Services.Implementations;

public class FlightService : IFlightService
{
  private readonly IFlightRepository _flightRepository;
  
  private readonly IBookingRepository _bookingRepository;

  private readonly IImportFromCsvService<FlightCsvImportDto, Flight> _importFromCsvService;

  public FlightService(
    IFlightRepository flightRepository, 
    IBookingRepository bookingRepository, 
    IImportFromCsvService<FlightCsvImportDto, Flight> importFromCsvService
  )
  {
    _flightRepository = flightRepository;

    _bookingRepository = bookingRepository;

    _importFromCsvService = importFromCsvService;
  }

  public FlightDto? GetById(int id)
  {
    var flight = _flightRepository.GetById(id);

    return flight == null ? null : new FlightDto(flight);
  }

  public IEnumerable<FlightDto> GetAvailableFlightsMatchingCriteria(FlightSearchCriteria criteria)
  {
    var flightMatchingCriteria = _flightRepository.GetMatchingCriteria(criteria);

    if (criteria.Class is null)
    {
      return flightMatchingCriteria
        .Where(flight => flight
          .Classes
          .Any(details => IsClassAvailableToBook(flight, details.Class)))
        .Select(flight => new FlightDto(flight));
    }

    return flightMatchingCriteria
      .Where(flight => IsClassAvailableToBook(flight, (FlightClass) criteria.Class))
      .Select(flight => new FlightDto(flight));
  }
  
  private bool IsClassAvailableToBook(Flight bookingFlight, FlightClass newClass)
  {
    var capacity = bookingFlight
      .Classes
      .First(details => details.Class == newClass)
      .Capacity;

    var bookedSeats = _bookingRepository
      .GetBookingsForFlightWithClass(bookingFlight.Id, newClass)
      .Count();

    return capacity - bookedSeats > 0;
  }

  public IList<string> ImportFromCsv(string filePath)
  {
    var importResult = _importFromCsvService.ImportFromCsv(filePath);
    
    _flightRepository.Add(importResult
      .ValidObjects
      .DistinctBy(flight => flight.Id)
    );

    return importResult.ValidationErrors;
  }
}