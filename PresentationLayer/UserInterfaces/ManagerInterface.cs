using PresentationLayer.CommandInvokers;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.Enums;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.UserInterfaces;

public class ManagerInterface : IUserInterface
{
  private readonly ICommandInvoker _commandInvoker;

  public ManagerInterface(ICommandInvoker commandInvoker)
  {
    _commandInvoker = commandInvoker;
  }

  public void Display()
  {
    var exit = false;

    while (!exit)
    {
      Menus.DisplayManagerMenu();

      var choice = InputParser.GetInput<ManagerChoice>(InputPrompts.ChoicePrompt,
        ParseFunctionsWithoutSkip.TryParseEnum);

      if (choice == ManagerChoice.Exit)
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