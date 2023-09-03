using System.ComponentModel.DataAnnotations;

namespace DataAccess.Validation.ValidationAttributes;

public class ValidStateAttribute : ValidationAttribute
{
  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    return value is null
      ? new ValidationResult(ValidationMessages.GenerateInvalidStateMessage(validationContext.MemberName))
      : ValidationResult.Success;
  }
}