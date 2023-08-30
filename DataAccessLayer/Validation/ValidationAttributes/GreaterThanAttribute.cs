using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Validation.ValidationAttributes;

public class GreaterThanAttribute<T> : ValidationAttribute where T : IComparable
{
  private readonly T _limit;

  public GreaterThanAttribute(T limit)
  {
    _limit = limit;
  }

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    if (value == null)
    {
      return ValidationResult.Success;
    }

    return ((T) value).CompareTo(_limit) > 0
      ? ValidationResult.Success
      : new ValidationResult(ValidationMessages.GenerateNotGreaterThanMessage(validationContext.MemberName, _limit));
  }
}