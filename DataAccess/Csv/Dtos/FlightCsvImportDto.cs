using DataAccess.Validation.ValidationAttributes;

namespace DataAccess.Csv.Dtos;

public class FlightCsvImportDto
{
  [ValidState, GreaterThan<int>(0)]
  public int? Id { get; set; }
  
  [ValidState]
  public string? DepartureCountry { get; set; }
  
  [ValidState, MinimumLength(4)]
  public string? DestinationCountry { get; set; }
  
  [ValidState, NotPastDate]
  public DateTime? DepartureDate { get; set; }
  
  [ValidState, MinimumLength(4)]
  public string? DepartureAirport { get; set; }
  
  [ValidState, MinimumLength(4)]
  public string? ArrivalAirport { get; set; }
  
  [ValidState, ListElementsValidation, DistinctClasses]
  public List<FlightClassDetailsCsvImportDto> Classes { get; set; }
}