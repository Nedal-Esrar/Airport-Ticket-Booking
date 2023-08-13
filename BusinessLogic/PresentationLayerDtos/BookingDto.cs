using DataAccessLayer.Models;

namespace BusinessLogic.PresentationLayerDtos;

public class BookingDto
{
  public int Id { get; init; }

  public PassengerDto Passenger { get; init; }
  
  public FlightDto Flight { get; init; }
  
  public FlightClass BookingClass { get; init; }
  
  public DateTime BookingDate { get; init; }

  public BookingDto(Booking booking)
  {
    Id = booking.Id;

    Passenger = new PassengerDto(booking.Passenger);

    Flight = new FlightDto(booking.Flight);

    BookingClass = booking.BookingClass;

    BookingDate = booking.BookingDate;
  }
  
  public override string ToString() =>
    $"""
     Id: {Id}
     For passenger: {Passenger}
     In flight: {Flight}
     Booking class: {BookingClass}
     Booking date: {BookingDate}
     """;
}