using DataAccess.Models;

namespace BusinessLogic.PresentationLayerDtos;

public class BookingDto
{
  public int Id { get; init; }

  public PassengerDto Passenger { get; init; }
  
  public FlightDto Flight { get; init; }
  
  public FlightClass BookingClass { get; init; }
  
  public DateTime BookingDate { get; init; }
  
  public override string ToString() =>
    $"""
     Id: {Id}
     For passenger: {Passenger}
     In flight: {Flight}
     Booking class: {BookingClass}
     Booking date: {BookingDate}
     """;
}