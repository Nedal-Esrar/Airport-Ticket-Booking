using DataAccess.Models;

namespace BusinessLogic.PresentationLayerDtos;

public class FlightDto
{
  public int Id { get; init; }
  
  public string DepartureCountry { get; init; }
  
  public string DestinationCountry { get; init; }
  
  public DateTime DepartureDate { get; init; }
  
  public string DepartureAirport { get; init; }
  
  public string ArrivalAirport { get; init; }

  public IList<FlightClassDetailsDto> Classes { get; init; }
  
  public override string ToString() =>
    $"""
     Id: {Id}
     Departure country: {DepartureCountry}
     Destination country: {DestinationCountry}
     Departure date: {DepartureDate}
     Departure airport: {DepartureAirport}
     Arrival airport: {ArrivalAirport}
     Classes to book:
     {string.Join(Environment.NewLine, Classes)}
     """;
}