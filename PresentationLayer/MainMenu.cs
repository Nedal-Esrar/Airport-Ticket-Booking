using BusinessLogic.Services.Interfaces;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.Enums;
using PresentationLayer.InputUtilities;
using PresentationLayer.UserInterfaces.Factories;

namespace PresentationLayer;

public class MainMenu
{
  private readonly IDictionary<MainMenuChoice, IUserInterfaceFactory> _factories;

  public MainMenu(
    IBookingService bookingService,
    IFlightService flightService,
    IPassengerService passengerService
  )
  {
    _factories = new Dictionary<MainMenuChoice, IUserInterfaceFactory>
    {
      {
        MainMenuChoice.PassengerUserInterface,
        new PassengerInterfaceFactory(bookingService, flightService, passengerService)
      },
      { MainMenuChoice.ManagerUserInterface, new ManagerInterfaceFactory(bookingService, flightService) }
    };
  }

  public async Task Run()
  {
    Console.WriteLine(GeneralMessages.Welcome);

    var exit = false;

    while (!exit)
    {
      Menus.DisplayMainMenu();

      var choice = InputParser.GetInput<MainMenuChoice>(InputPrompts.ChoicePrompt,
        ParseFunctionsWithoutSkip.TryParseEnum);

      if (choice == MainMenuChoice.Exit)
      {
        exit = true;

        continue;
      }

      if (!_factories.TryGetValue(choice, out var factory))
      {
        continue;
      }
      
      var requestedInterface = await factory.Create();
      
      requestedInterface?.Display();
    }
    
    Console.WriteLine(GeneralMessages.ExitMessage);
  }
}