using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Validation;

namespace DataAccessLayer.Csv.Validators;

public class Validator : IValidator
{
  public bool IsValid(object? obj)
  {
    return obj is not null && System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
      obj,
      new ValidationContext(obj),
      new List<ValidationResult>(),
      true
    );
  }

  public IList<string> GetErrorMessages(object? obj)
  {
    if (obj is null)
    {
      return new List<string>
      {
        ValidationMessages.InvalidObjectStateMessage
      };
    }
    
    var errorMessages = new List<ValidationResult>();
    
    System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
      obj,
      new ValidationContext(obj),
      errorMessages,
      true
    );

    return errorMessages
      .Select(result => result.ErrorMessage)
      .ToList()!;
  }
}