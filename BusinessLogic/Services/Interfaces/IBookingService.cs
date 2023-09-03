using BusinessLogic.PresentationLayerDtos;
using DataAccess.Models;
using DataAccess.SearchCriteria;

namespace BusinessLogic.Services.Interfaces;

public interface IBookingService
{
  Task<BookingDto?> GetById(int id);
  
  Task<bool> BookFlight(int flightId, int passengerId, FlightClass flightClass);

  Task CancelBooking(int bookingId);

  Task<bool> ModifyBooking(int bookingId, FlightClass newClass);

  Task<IEnumerable<BookingDto>> GetPassengerBookings(int passengerId);

  Task<IEnumerable<BookingDto>> GetBookingsMatchingCriteria(BookingSearchCriteria criteria);
}