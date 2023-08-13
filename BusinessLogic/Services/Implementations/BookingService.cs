using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.SearchCriteria;

namespace BusinessLogic.Services.Implementations;

public class BookingService : IBookingService
{
  private readonly IBookingRepository _bookingRepository;

  private readonly IFlightRepository _flightRepository;

  private readonly IPassengerRepository _passengerRepository;


  public BookingService(
    IBookingRepository bookingRepository, 
    IFlightRepository flightRepository, 
    IPassengerRepository passengerRepository)
  {
    _bookingRepository = bookingRepository;
    _flightRepository = flightRepository;
    _passengerRepository = passengerRepository;
  }
  

  public BookingDto? GetById(int id)
  {
    var booking = _bookingRepository.GetById(id);

    return booking == null ? null : new BookingDto(booking);
  }

  public bool BookFlight(int flightId, int passengerId, FlightClass flightClass)
  {
    var flight = _flightRepository.GetById(flightId);

    if (flight == null)
    {
      return false;
    }
    
    if (!IsClassAvailableToBook(flight, flightClass))
    {
      return false;
    }

    var passenger = _passengerRepository.GetById(passengerId);

    if (passenger == null)
    {
      return false;
    }
    
    var bookingToAdd = CreateBooking(flight, passenger, flightClass);

    _bookingRepository.Add(bookingToAdd);

    return true;
  }

  private Booking CreateBooking(Flight flight, Passenger passenger, FlightClass flightClass)
  {
    var bookings = _bookingRepository.GetAll();

    var id = bookings.Any()
      ? bookings.Max(booking => booking.Id) + 1
      : 1;

    return new Booking
    {
      Id = id,
      Flight = flight,
      Passenger = passenger,
      BookingClass = flightClass,
      BookingDate = DateTime.Now
    };
  }

  public void CancelBooking(int bookingId)
  {
    var bookingToCancel = _bookingRepository.GetById(bookingId);

    if (bookingToCancel == null)
    {
      return;
    }

    _bookingRepository.Remove(bookingToCancel);
  }

  public bool ModifyBooking(int bookingId, FlightClass newClass)
  {
    var booking = _bookingRepository.GetById(bookingId);

    if (booking is null)
    {
      return false;
    }

    if (booking.Flight.Classes.All(details => details.Class != newClass))
    {
      return false;
    }

    if (!IsClassAvailableToBook(booking.Flight, newClass))
    {
      return false;
    }
    
    booking.BookingClass = newClass;
      
    _bookingRepository.Update(booking);

    return true;

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

  public IEnumerable<BookingDto> GetPassengerBookings(int passengerId)
  {
    return _bookingRepository
      .GetPassengerBookings(passengerId)
      .Select(booking => new BookingDto(booking));
  }

  public IEnumerable<BookingDto> GetBookingsMatchingCriteria(BookingSearchCriteria criteria)
  {
    return _bookingRepository
      .GetMatchingCriteria(criteria)
      .Select(booking => new BookingDto(booking));;
  }
}