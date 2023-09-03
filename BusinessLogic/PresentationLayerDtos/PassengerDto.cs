using DataAccess.Models;

namespace BusinessLogic.PresentationLayerDtos;

public class PassengerDto
{
  public int Id { get; set; }
  
  public string Name { get; set; }

  public override string ToString() =>
    $"{Id}; {Name}";
}