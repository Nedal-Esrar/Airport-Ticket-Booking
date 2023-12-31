using DataAccessLayer.Csv;
using DataAccessLayer.Csv.CsvImportService;
using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.SearchCriteria;

namespace DataAccessLayer.Repositories.Implementations;

public class CsvBookingRepository : IBookingRepository
{
  private const string PathToCsv = "../../../../DataAccessLayer/CsvFiles/bookings.csv";

  private readonly IImportFromCsvService<BookingCsvImportDto, Booking> _importFromCsvService;

  private readonly CsvWriter<Booking> _writer;

  public CsvBookingRepository(IImportFromCsvService<BookingCsvImportDto, Booking> importFromCsvService)
  {
    _importFromCsvService = importFromCsvService;

    _writer = new CsvWriter<Booking>(PathToCsv);
  }

  public IEnumerable<Booking> GetAll()
  {
    return _importFromCsvService
      .ImportFromCsv(PathToCsv)
      .ValidObjects
      .DistinctBy(booking => booking.Id);
  }

  public void Add(Booking booking)
  {
    _writer.Append(booking);
  }

  public void Remove(Booking booking)
  {
    _writer.Write(
      GetAll()
        .Where(bookingToExamine => bookingToExamine.Id != booking.Id)
    );
  }

  public Booking? GetById(int id)
  {
    return GetAll()
      .FirstOrDefault(booking => booking.Id == id);
  }

  public void Update(Booking booking)
  {
    _writer.Write(
      GetAll()
        .Select(bookingToSelect => bookingToSelect.Id == booking.Id ? booking : bookingToSelect)
    );
  }

  public IEnumerable<Booking> GetPassengerBookings(int passengerId)
  {
    return GetAll()
      .Where(booking => booking.Passenger.Id == passengerId);
  }

  public IEnumerable<Booking> GetBookingsForFlightWithClass(int flightId, FlightClass flightClass)
  {
    return GetAll()
      .Where(booking => booking.Flight.Id == flightId && booking.BookingClass == flightClass);
  }

  public IEnumerable<Booking> GetMatchingCriteria(BookingSearchCriteria criteria)
  {
    return GetAll()
      .Where(criteria.Matches);
  }
}