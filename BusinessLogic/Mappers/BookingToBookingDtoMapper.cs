using BusinessLogic.PresentationLayerDtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;

namespace BusinessLogic.Mappers;

public class BookingToBookingDtoMapper : IMapper<Booking, BookingDto>
{
  private readonly IMapper<Flight, FlightDto> _flightMapper;

  private readonly IMapper<Passenger, PassengerDto> _passengerMapper;

  public BookingToBookingDtoMapper(IMapper<Flight, FlightDto> flightMapper, IMapper<Passenger, PassengerDto> passengerMapper)
  {
    _flightMapper = flightMapper;
    
    _passengerMapper = passengerMapper;
  }

  public BookingDto? Map(Booking? input)
  {
    if (input is null)
    {
      return null;
    }
    
    return new BookingDto
    {
      Id = input.Id,
      Passenger = _passengerMapper.Map(input.Passenger),
      Flight = _flightMapper.Map(input.Flight),
      BookingClass = input.BookingClass,
      BookingDate = input.BookingDate
    };
  }
}