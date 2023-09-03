using System.ComponentModel.DataAnnotations;

namespace DataAccess.Validation.ValidationAttributes;

public class InEnumDomainAttribute<T> : ValidationAttribute where T : struct, Enum
{
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (value is not T)
    {
      return ValidationResult.Success;
    }
    
    return Enum.IsDefined((T) value) 
      ? ValidationResult.Success
      : new ValidationResult(ValidationMessages.GenerateNotInEnumDomainMessage<T>(
        validationContext.MemberName, 
        Enum.GetValues<T>()));
  }
}