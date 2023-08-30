using PresentationLayer.CommandInvokers;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.Enums;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.UserInterfaces;

public class PassengerInterface : IUserInterface
{
  private readonly ICommandInvoker _commandInvoker;

  public PassengerInterface(ICommandInvoker commandInvoker)
  {
    _commandInvoker = commandInvoker;
  }

  public void Display()
  {
    var exit = false;

    while (!exit)
    {
      Menus.DisplayPassengerMenu();

      var choice = InputParser.GetInput<PassengerChoice>(InputPrompts.ChoicePrompt,
        ParseFunctionsWithoutSkip.TryParseEnum);

      if (choice == PassengerChoice.Exit)
      {
        exit = true;
      }
      else
      {
        _commandInvoker.Invoke((int) choice);
      }
    }
    
    Console.WriteLine(GeneralMessages.ExitMessage);
  }
}