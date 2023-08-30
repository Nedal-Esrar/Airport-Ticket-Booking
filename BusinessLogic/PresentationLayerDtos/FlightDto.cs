using DataAccessLayer.Models;

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

  public FlightDto(Flight flight)
  {
    Id = flight.Id;

    DepartureCountry = flight.DepartureCountry;

    DestinationCountry = flight.DestinationCountry;

    DepartureDate = flight.DepartureDate;

    DepartureAirport = flight.DepartureAirport;

    ArrivalAirport = flight.ArrivalAirport;

    Classes = flight
      .Classes
      .Select(details => new FlightClassDetailsDto(details))
      .ToList();
  }
  
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