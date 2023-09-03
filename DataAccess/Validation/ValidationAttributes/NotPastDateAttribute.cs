using System.ComponentModel.DataAnnotations;

namespace DataAccess.Validation.ValidationAttributes;

public class NotPastDateAttribute : ValidationAttribute
{
  public override bool IsValid(object? value)
  {
    if (value is not DateTime time)
    {
      return true;
    }
    
    return time < DateTime.Now;
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (value is not DateTime time)
    {
      return ValidationResult.Success;
    }
    
    var propertyName = validationContext.MemberName;
    
    return time < DateTime.Now
      ? new ValidationResult(ValidationMessages.GeneratePastDateMessage(propertyName))
      : ValidationResult.Success;
  }
}