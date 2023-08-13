using DataAccessLayer.Models;

namespace BusinessLogic.PresentationLayerDtos;

public class PassengerDto
{
  public int Id { get; set; }
  
  public string Name { get; set; }

  public PassengerDto(Passenger passenger)
  {
    Id = passenger.Id;

    Name = passenger.Name;
  }

  public override string ToString() =>
    $"{Id}; {Name}";
}