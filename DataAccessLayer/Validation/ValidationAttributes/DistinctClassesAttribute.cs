using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Models;

namespace DataAccessLayer.Validation.ValidationAttributes;

public class DistinctClassesAttribute : ValidationAttribute
{
  public override bool IsValid(object? value)
  {
    if (value is not IEnumerable<FlightClassDetailsCsvImportDto> enumerable)
    {
      return false;
    }

    var set = new HashSet<FlightClass>();
    
    foreach (var item in enumerable)
    {
      if (item.Class != null)
      {
        set.Add((FlightClass)item.Class);
      }
    }

    if (set.Count == enumerable.Count())
    {
      return true;
    }
    
    ErrorMessage = ValidationMessages.DistinctClasses;
      
    return false;

  }
}