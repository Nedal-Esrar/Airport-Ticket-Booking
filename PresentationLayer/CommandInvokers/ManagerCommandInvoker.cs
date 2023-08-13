using BusinessLogic.Services.Interfaces;
using PresentationLayer.Commands;
using PresentationLayer.Commands.ManagerCommands;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.Enums;

namespace PresentationLayer.CommandInvokers;

public class ManagerCommandInvoker : ICommandInvoker
{
  private readonly IDictionary<ManagerChoice, ICommand> _commands;
  
  public ManagerCommandInvoker(IBookingService bookingService, IFlightService flightService)
  {
    _commands = new Dictionary<ManagerChoice, ICommand>
    {
      { ManagerChoice.FilterBookingUsingCriteria, new FilterBookingsCommand(bookingService) },
      { ManagerChoice.ImportFlightsFromCsv, new ImportFlightsFromCsvCommand(flightService) }
    };
  }
  
  public void Invoke(int choice)
  {
    if (_commands.TryGetValue((ManagerChoice) choice, out var command))
    {
      command.Execute();
    }
    else
    {
      HandleInvalidChoice();
    }
  }

  private void HandleInvalidChoice()
  {
    Console.WriteLine(GeneralMessages.InvalidChoice);
  }
}