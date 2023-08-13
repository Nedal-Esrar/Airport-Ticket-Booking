using DataAccessLayer.Models;

namespace BusinessLogic.PresentationLayerDtos;

public class FlightClassDetailsDto
{
  public FlightClass Class { get; init; }

  public decimal Price { get; init; }
  
  public int Capacity { get; init; }

  public FlightClassDetailsDto(FlightClassDetails details)
  {
    Class = details.Class;

    Price = details.Price;

    Capacity = details.Capacity;
  }

  public override string ToString() =>
    $"Class: {Class}; Price: {Price}, Capacity: {Capacity}";
}