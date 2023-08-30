using DataAccessLayer.Models;
using DataAccessLayer.Validation.ValidationAttributes;

namespace DataAccessLayer.Csv.Dtos;

public class FlightClassDetailsCsvImportDto
{
  [ValidState, InEnumDomain<FlightClass>]
  public FlightClass? Class { get; set; }
  
  [ValidState, GreaterThan<double>(0.0)]
  public double? Price { get; set; }
  
  [ValidState, GreaterThan<int>(0)]
  public int? Capacity { get; set; }
}