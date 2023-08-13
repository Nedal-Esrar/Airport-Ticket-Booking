using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Validation.ValidationAttributes;

public class MinimumLengthAttribute : ValidationAttribute
{
  private readonly int _length;

  public MinimumLengthAttribute(int length)
  {
    _length = length;
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (value is not string str)
    {
      return ValidationResult.Success;
    }

    return str.Length < _length
      ? new ValidationResult(
        ValidationMessages.GenerateBelowMinimumLengthMessage(validationContext.MemberName, _length))
      : ValidationResult.Success;
  }
}