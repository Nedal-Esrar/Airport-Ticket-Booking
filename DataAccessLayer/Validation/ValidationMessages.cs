namespace DataAccessLayer.Validation;

public static class ValidationMessages
{
  public const string InvalidObjectStateMessage = "Invalid Object: # of parameters is not identical.";

  public static string GenerateLineIndicatorMessage(int lineNumber) =>
    $"In line {lineNumber}:";

  public static string GenerateNotGreaterThanMessage<T>(string property, T limit) where T : IComparable =>
    $"{property} should be greater than {limit}";

  public static string GeneratePastDateMessage(string property) =>
    $"{property} must not be a past date.";

  public static string GenerateNotInEnumDomainMessage<TEnum>(string property, IEnumerable<TEnum> enumValues) where TEnum : struct, Enum
    => $"{property} must be in range {string.Join(',', enumValues.Select(value => (int)(object)value))}";

  public static string GenerateInvalidStateMessage(string property) =>
    $"{property} state is invalid.";

  public static string GenerateBelowMinimumLengthMessage(string? property, int length) =>
    $"{property} length should be at least {length} characters.";

  public static string GenerateClassDetailsIndicator(int count) =>
    $"For Flight Class #{count}";

  public const string DistinctClasses = "Classes should be distinct.";
}