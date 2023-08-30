using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Validation.ValidationAttributes;

public class ListElementsValidationAttribute : ValidationAttribute
{
  public override bool IsValid(object? value)
  {
    if (value is not IEnumerable<object> enumerable)
    {
      return true;
    }

    var count = 0;

    var errorMessages = new List<string>();
    
    foreach (var item in enumerable)
    {
      var context = new ValidationContext(item);

      var results = new List<ValidationResult>();

      var isValid = Validator.TryValidateObject(item, context, results, true);

      ++count;

      if (isValid)
      {
        continue;
      }
      
      errorMessages.Add(ValidationMessages.GenerateClassDetailsIndicator(count));
      
      errorMessages.Add(string.Join(Environment.NewLine, results.Select(result => result.ErrorMessage)));
    }

    if (!errorMessages.Any())
    {
      return true;
    }
    
    ErrorMessage = string.Join(Environment.NewLine, errorMessages);

    return false;
  }
}