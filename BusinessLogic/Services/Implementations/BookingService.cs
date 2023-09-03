using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.DateTimeProvider;
using BusinessLogic.Services.Interfaces;
using BusinessLogic.Utilites;
using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.SearchCriteria;

namespace BusinessLogic.Services.Implementations;

public class BookingService : IBookingService
{
  private readonly IBookingRepository _bookingRepository;

  private readonly IFlightRepository _flightRepository;

  private readonly IPassengerRepository _passengerRepository;

  private readonly IMapper<Booking, BookingDto> _bookingMapper;

  private readonly IDateTimeProvider _dateTimeProvider;

  public BookingService(
    IBookingRepository bookingRepository, 
    IFlightRepository flightRepository, 
    IPassengerRepository passengerRepository, 
    IMapper<Booking, BookingDto> bookingMapper,
    IDateTimeProvider dateTimeProvider)
  {
    _bookingRepository = bookingRepository;
    _flightRepository = flightRepository;
    _passengerRepository = passengerRepository;
    _bookingMapper = bookingMapper;
    _dateTimeProvider = dateTimeProvider;
  }
  

  public async Task<BookingDto?> GetById(int id)
  {
    var booking = await _bookingRepository.GetById(id);

    return _bookingMapper.Map(booking);
  }

  public async Task<bool> BookFlight(int flightId, int passengerId, FlightClass flightClass)
  {
    var flight = await _flightRepository.GetById(flightId);

    if (flight is null)
    {
      return false;
    }
    
    if (!flight.Classes.Any(details => details.Class == flightClass))
    {
      return false;
    }
    
    if (!await IsClassAvailableToBook(flight, flightClass))
    {
      return false;
    }

    var passenger = await _passengerRepository.GetById(passengerId);

    if (passenger is null)
    {
      return false;
    }
    
    var bookingToAdd = await CreateBooking(flight, passenger, flightClass);

    await _bookingRepository.Add(bookingToAdd);

    return true;
  }
  
  private async Task<bool> IsClassAvailableToBook(Flight flight, FlightClass flightClass)
  {
    return await FlightUtilities.IsClassAvailableToBook(flight, flightClass, _bookingRepository);
  }

  private async Task<Booking> CreateBooking(Flight flight, Passenger passenger, FlightClass flightClass)
  {
    var allBookings = await _bookingRepository.GetAll();

    var id = allBookings.Any()
      ? allBookings.Max(booking => booking.Id) + 1
      : 1;
      
    return new Booking
    {
      Id = id,
      Flight = flight,
      Passenger = passenger,
      BookingClass = flightClass,
      BookingDate = _dateTimeProvider.GetCurrentDateTime()
    };
  }

  public async Task CancelBooking(int bookingId)
  {
    var bookingToCancel = await _bookingRepository.GetById(bookingId);

    if (bookingToCancel is null)
    {
      return;
    }

    await _bookingRepository.Remove(bookingToCancel);
  }

  public async Task<bool> ModifyBooking(int bookingId, FlightClass newClass)
  {
    var booking = await _bookingRepository.GetById(bookingId);

    if (booking is null)
    {
      return false;
    }

    if (!booking.Flight.Classes.Any(details => details.Class == newClass))
    {
      return false;
    }

    if (!await IsClassAvailableToBook(booking.Flight, newClass))
    {
      return false;
    }
    
    booking.BookingClass = newClass;
      
    await _bookingRepository.Update(booking);

    return true;
  }

  public async Task<IEnumerable<BookingDto>> GetPassengerBookings(int passengerId)
  {
    var passengerBookings = await _bookingRepository.GetPassengerBookings(passengerId);
    
    return passengerBookings.Select(booking => _bookingMapper.Map(booking));
  }

  public async Task<IEnumerable<BookingDto>> GetBookingsMatchingCriteria(BookingSearchCriteria criteria)
  {
    var bookingsMatchingCriteria = await _bookingRepository.GetMatchingCriteria(criteria);
    
    return bookingsMatchingCriteria.Select(booking => _bookingMapper.Map(booking));
  }
}