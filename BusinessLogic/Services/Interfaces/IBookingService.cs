using BusinessLogic.PresentationLayerDtos;
using DataAccessLayer.Models;
using DataAccessLayer.SearchCriteria;

namespace BusinessLogic.Services.Interfaces;

public interface IBookingService
{
  BookingDto? GetById(int id);
  
  bool BookFlight(int flightId, int passengerId, FlightClass flightClass);

  void CancelBooking(int bookingId);

  bool ModifyBooking(int bookingId, FlightClass newClass);

  IEnumerable<BookingDto> GetPassengerBookings(int passengerId);

  IEnumerable<BookingDto> GetBookingsMatchingCriteria(BookingSearchCriteria criteria);
}