using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using PresentationLayer.Commands;
using PresentationLayer.Commands.PassengerCommands;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.Enums;

namespace PresentationLayer.CommandInvokers;

public class PassengerCommandInvoker : ICommandInvoker
{
  private readonly IDictionary<PassengerChoice, ICommand> _commands;

  public PassengerCommandInvoker(IFlightService flightService, IBookingService bookingService, PassengerDto passenger)
  {
    _commands = new Dictionary<PassengerChoice, ICommand>
    {
      { PassengerChoice.BookFlight, new BookCommand(flightService, bookingService, passenger) },
      { PassengerChoice.SearchForFlightsMatchingCriteria, new SearchForAvailableFlightsMatchingCriteriaCommand(flightService) },
      { PassengerChoice.CancelBooking, new CancelBookingCommand(bookingService, passenger) },
      { PassengerChoice.ModifyBooking, new ModifyBookingCommand(bookingService, passenger) },
      { PassengerChoice.ViewBookings, new ViewPassengerBookingsCommand(bookingService, passenger) }
    };
  }
  
  public void Invoke(int choice)
  {
    if (_commands.TryGetValue((PassengerChoice) choice, out var command))
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