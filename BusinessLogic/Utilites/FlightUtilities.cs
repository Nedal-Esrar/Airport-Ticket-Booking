using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Utilites;

public static class FlightUtilities
{
  public static async Task<bool> IsClassAvailableToBook(Flight bookingFlight, FlightClass newClass, IBookingRepository bookingRepository)
  {
    var capacity = bookingFlight
      .Classes
      .First(details => details.Class == newClass)
      .Capacity;

    var bookingsForFlightWithClass = await bookingRepository.GetBookingsForFlightWithClass(bookingFlight.Id, newClass);

    var bookedSeats = bookingsForFlightWithClass.Count();

    return capacity - bookedSeats > 0;
  }
}