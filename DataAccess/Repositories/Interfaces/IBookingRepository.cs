using DataAccess.Models;
using DataAccess.SearchCriteria;

namespace DataAccess.Repositories.Interfaces;

public interface IBookingRepository
{
  Task<IEnumerable<Booking>> GetAll();

  Task Add(Booking booking);

  Task Remove(Booking booking);

  Task<Booking?> GetById(int id);

  Task Update(Booking booking);

  Task<IEnumerable<Booking>> GetPassengerBookings(int passengerId);
  
  Task<IEnumerable<Booking>> GetBookingsForFlightWithClass(int flightId, FlightClass flightClass);

  Task<IEnumerable<Booking>> GetMatchingCriteria(BookingSearchCriteria criteria);
}