namespace PresentationLayer.DisplayUtilities;

public static class Menus
{
  public static void DisplayPassengerMenu()
  {
    Console.WriteLine("[1] Book a flight.");
    Console.WriteLine("[2] Show available flights.");
    Console.WriteLine("[3] Cancel a booking.");
    Console.WriteLine("[4] Modify a booking");
    Console.WriteLine("[5] View bookings");
    Console.WriteLine("[6] Exit");
  }

  public static void DisplayManagerMenu()
  {
    Console.WriteLine("[1] Filter bookings.");
    Console.WriteLine("[2] Import flights from a csv file.");
    Console.WriteLine("[3] Exit");
  }

  public static void DisplayMainMenu()
  {
    Console.WriteLine("What are you?");
    Console.WriteLine("[1] Passenger");
    Console.WriteLine("[2] Manager");
    Console.WriteLine("[3] Exit");
  }
}