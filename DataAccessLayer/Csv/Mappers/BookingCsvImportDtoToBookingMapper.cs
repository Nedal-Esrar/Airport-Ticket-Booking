using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Csv.Mappers;

public class BookingCsvImportDtoToBookingMapper : IMapper<BookingCsvImportDto, Booking>
{
  private readonly IFlightRepository _flightRepository;

  private readonly IPassengerRepository _passengerRepository;

  public BookingCsvImportDtoToBookingMapper(IFlightRepository flightRepository, IPassengerRepository passengerRepository)
  {
    _flightRepository = flightRepository;

    _passengerRepository = passengerRepository;
  }

  public Booking? Map(BookingCsvImportDto toMap)
  {
    var bookingPassenger = _passengerRepository.GetById((int) toMap.PassengerId!);

    var bookingFlight = _flightRepository.GetById((int) toMap.FlightId!);

    if (bookingFlight is null || bookingPassenger is null)
    {
      return null;
    }

    return new Booking
    {
      Id = (int)toMap.Id!,
      Passenger = bookingPassenger,
      Flight = bookingFlight,
      BookingClass = (FlightClass)toMap.FlightClass!,
      BookingDate = (DateTime)toMap.BookingDate!
    };
  }
}