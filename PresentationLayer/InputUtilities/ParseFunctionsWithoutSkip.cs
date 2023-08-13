using System.Globalization;

namespace PresentationLayer.InputUtilities;

public static class ParseFunctionsWithoutSkip
{
  public static bool TryParseId(string input, out int id)
  {
    return int.TryParse(input, out id) && id > 0;
  }

  public static bool TryParsePrice(string input, out decimal price)
  {
    return decimal.TryParse(input, out price) && price > 0M;
  }

  public static bool TryParseDate(string input, out DateTime date)
  {
    return DateTime.TryParse(input, CultureInfo.InvariantCulture, out date);
  }

  public static bool TryParseEnum<TEnum>(string input, out TEnum enumValue) where TEnum : struct, Enum
  {
    return Enum.TryParse(input, out enumValue) && Enum.IsDefined(enumValue);
  }

  public static bool TryParseString(string input, out string? parsedString)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      parsedString = null;
      
      return false;
    }

    parsedString = input.Trim();

    return true;
  }
}