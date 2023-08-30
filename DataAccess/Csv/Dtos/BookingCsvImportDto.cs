using DataAccess.Models;
using DataAccess.Validation.ValidationAttributes;

namespace DataAccess.Csv.Dtos;

public class BookingCsvImportDto
{
  [ValidState, GreaterThan<int>(0)]
  public int? Id { get; set; }
  
  [ValidState, GreaterThan<int>(0)]
  public int? PassengerId { get; set; }
  
  [ValidState, GreaterThan<int>(0)]
  public int? FlightId { get; set; }
  
  [ValidState, InEnumDomain<FlightClass>]
  public FlightClass? FlightClass { get; set; }
  
  [ValidState]
  public DateTime? BookingDate { get; set; }
}