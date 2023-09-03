using DataAccess.Csv;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.SearchCriteria;

namespace DataAccess.Repositories.Implementations;

public class CsvBookingRepository : IBookingRepository
{
  public readonly string PathToCsv = "../../../../DataAccess/CsvFiles/bookings.csv";
  
  private readonly IImportFromCsvService<BookingCsvImportDto, Booking> _importFromCsvService;
  
  private readonly ICsvWriter _writer;
  
  public CsvBookingRepository(IImportFromCsvService<BookingCsvImportDto, Booking> importFromCsvService, ICsvWriter writer)
  {
    _importFromCsvService = importFromCsvService;

    _writer = writer;
  }
  
  public async Task<IEnumerable<Booking>> GetAll()
  {
    var allBookings = await _importFromCsvService.ImportFromCsv(PathToCsv);
    
    return allBookings
      .ValidObjects
      .DistinctBy(booking => booking.Id);
  }
  
  public async Task Add(Booking booking)
  {
    await _writer.AppendAsync(PathToCsv, booking);
  }
  
  public async Task Remove(Booking booking)
  {
    var allBookings = await GetAll();

    await _writer.WriteAsync(PathToCsv,  allBookings.Where(bookingToExamine => bookingToExamine.Id != booking.Id));
  }
  
  public async Task<Booking?> GetById(int id)
  {
    var allBookings = await GetAll();
      
    return allBookings
      .FirstOrDefault(booking => booking.Id == id);
  }
  
  public async Task Update(Booking booking)
  {
    var allBookings = await GetAll();

    await _writer.WriteAsync(PathToCsv,  
      allBookings.Select(bookingToSelect => bookingToSelect.Id == booking.Id ? booking : bookingToSelect));
  }
  
  public async Task<IEnumerable<Booking>> GetPassengerBookings(int passengerId)
  {
    var allBookings = await GetAll();
    
    return allBookings
      .Where(booking => booking.Passenger.Id == passengerId);
  }
  
  public async Task<IEnumerable<Booking>> GetBookingsForFlightWithClass(int flightId, FlightClass flightClass)
  {
    var allBookings = await GetAll();
    
    return allBookings
      .Where(booking => booking.Flight.Id == flightId && booking.BookingClass == flightClass);
  }
  
  public async Task<IEnumerable<Booking>> GetMatchingCriteria(BookingSearchCriteria criteria)
  {
    var allBookings = await GetAll();
    
    return allBookings.Where(criteria.Matches);
  }
}