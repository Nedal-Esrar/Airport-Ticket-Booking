using DataAccess.Csv.Dtos;
using DataAccess.Exceptions;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Csv.Mappers;

public class BookingCsvImportDtoToBookingMapper : IMapper<BookingCsvImportDto, Booking>
{
  private readonly IFlightRepository _flightRepository;

  private readonly IPassengerRepository _passengerRepository;

  public BookingCsvImportDtoToBookingMapper(IFlightRepository flightRepository, IPassengerRepository passengerRepository)
  {
    _flightRepository = flightRepository;

    _passengerRepository = passengerRepository;
  }

  public Booking? Map(BookingCsvImportDto? toMap)
  {
    if (toMap is null)
    {
      return null;
    }
    
    var bookingPassenger = _passengerRepository.GetById(toMap.PassengerId.Value).Result;

    var bookingFlight = _flightRepository.GetById(toMap.PassengerId.Value).Result;

    if (bookingFlight is null || bookingPassenger is null)
    {
      throw new IntegrityException();
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