using DataAccess.Models;

namespace BusinessLogic.PresentationLayerDtos;

public class FlightClassDetailsDto
{
  public FlightClass Class { get; init; }

  public decimal Price { get; init; }
  
  public int Capacity { get; init; }

  public override string ToString() =>
    $"Class: {Class}; Price: {Price}, Capacity: {Capacity}";
}