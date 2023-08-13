using DataAccessLayer.Models;
using DataAccessLayer.SearchCriteria;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IBookingRepository
{
  IEnumerable<Booking> GetAll();

  void Add(Booking booking);

  void Remove(Booking booking);

  Booking? GetById(int id);

  void Update(Booking booking);

  IEnumerable<Booking> GetPassengerBookings(int passengerId);
  
  IEnumerable<Booking> GetBookingsForFlightWithClass(int flightId, FlightClass flightClass);

  IEnumerable<Booking> GetMatchingCriteria(BookingSearchCriteria criteria);
}