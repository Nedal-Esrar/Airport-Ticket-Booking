namespace PresentationLayer.DisplayUtilities;

public static class InputPrompts
{
  public const string BookingIdPromptWithoutSkip = "Booking id: ";
  private const string SkipMessage = "(Press return to skip)";
  public const string PricePromptWithSkip = $"Price: {SkipMessage}";
  public const string DepartureCountryPromptWithSkip = $"Departure country: {SkipMessage}";
  public const string DestinationCountryPromptWithSkip = $"Destination country: {SkipMessage}";
  public const string DepartureDatePromptWithSkip = $"Departure date: {SkipMessage}";
  public const string DepartureAirportPromptWithSkip = $"Departure airport: {SkipMessage}";
  public const string ArrivalAirportPromptWithSkip = $"Arrival airport: {SkipMessage}";
  public const string ClassPromptWithSkip = $"Flight class: {SkipMessage}";
  public const string PassengerIdPromptWithoutSkip = "Passenger id: ";
  public const string PassengerIdPromptWithSkip = $"{PassengerIdPromptWithoutSkip} {SkipMessage}";
  public const string FlightIdPromptWithSkip = $"{FlightMessages.FlightIdPromptWithoutSkip} {SkipMessage}";
  public const string FilePathPrompt = "File path: ";
  public const string ChoicePrompt = "Choice: ";
}