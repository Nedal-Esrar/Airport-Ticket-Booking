using DataAccess.Validation.ValidationAttributes;

namespace DataAccess.Csv.Dtos;

public class PassengerCsvImportDto
{
  [ValidState, GreaterThan<int>(0)]
  public int? Id { get; set; }
  
  [ValidState, MinimumLength(4)]
  public string? Name { get; set; }
}