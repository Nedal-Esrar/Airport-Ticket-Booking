namespace PresentationLayer.InputUtilities;

public static class ParseFunctionsWithSkip
{
  public static bool TryParseId(string input, out int? id)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      id = null;
      
      return true;
    }

    var doesParseSucceed = ParseFunctionsWithoutSkip.TryParseId(input, out var parseResult);

    id = parseResult;

    return doesParseSucceed;
  }

  public static bool TryParsePrice(string input, out decimal? price)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      price = null;
      
      return true;
    }
    
    var doesParseSucceed = ParseFunctionsWithoutSkip.TryParsePrice(input, out var parseResult);

    price = parseResult;

    return doesParseSucceed;
  }

  public static bool TryParseDate(string input, out DateTime? date)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      date = null;
      
      return true;
    }
    
    var doesParseSucceed = ParseFunctionsWithoutSkip.TryParseDate(input, out var parseResult);

    date = parseResult;

    return doesParseSucceed;
  }

  public static bool TryParseEnum<TEnum>(string input, out TEnum? flightClass) where TEnum : struct, Enum
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      flightClass = null;
      
      return true;
    }
    
    var doesParseSucceed = ParseFunctionsWithoutSkip.TryParseEnum<TEnum>(input, out var parseResult);

    flightClass = parseResult;

    return doesParseSucceed;
  }

  public static bool TryParseString(string input, out string? parsedString)
  {
    if (string.IsNullOrWhiteSpace(input))
    {
      parsedString = null;
      
      return true;
    }

    parsedString = input.Trim();

    return true;
  }
}