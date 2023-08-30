namespace PresentationLayer.InputUtilities;

public static class InputParser
{
  public delegate bool ParseFunc<TValue>(string input, out TValue? value);

  public static TInput? GetInput<TInput>(string prompt, ParseFunc<TInput> parseFunc)
  {
    string input;

    TInput? parsedInput;

    do
    {
      Console.Write($"{prompt}");

      input = Console.ReadLine();
    } while (!parseFunc(input, out parsedInput));

    return parsedInput;
  }
}