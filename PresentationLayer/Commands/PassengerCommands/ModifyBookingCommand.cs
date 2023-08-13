using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using DataAccessLayer.Models;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.Commands.PassengerCommands;

public class ModifyBookingCommand : ICommand
{
  private readonly IBookingService _bookingService;

  private readonly PassengerDto _passenger;

  public ModifyBookingCommand(IBookingService bookingService, PassengerDto passenger)
  {
    _bookingService = bookingService;

    _passenger = passenger;
  }

  public void Execute()
  {
    Console.WriteLine(BookingMessages.EnterBookingId);
    
    var bookingId = InputParser.GetInput<int>(InputPrompts.BookingIdPromptWithoutSkip,
      ParseFunctionsWithoutSkip.TryParseId);

    var booking = _bookingService.GetById(bookingId);

    if (booking == null)
    {
      Console.WriteLine(BookingMessages.BookingDoesNotExist);
    }
    
    var doesBookingBelongToPassenger = _bookingService
      .GetPassengerBookings(_passenger.Id)
      .Any(booking => booking.Passenger.Id == _passenger.Id);

    if (!doesBookingBelongToPassenger)
    {
      Console.WriteLine(BookingMessages.BookingDoesNotBelongToPassenger);
    }
    
    Console.WriteLine(FlightMessages.EnterFlightClass);

    var flightClass = InputParser.GetInput<FlightClass>(FlightMessages.FlightClassPromptWithoutSkip,
      ParseFunctionsWithoutSkip.TryParseEnum);

    var bookingModifiedSuccessfully = _bookingService.ModifyBooking(bookingId, flightClass);

    Console.WriteLine(bookingModifiedSuccessfully
      ? BookingMessages.BookingModifiedSuccessfully
      : BookingMessages.FlightClassNotAvailable);
  }
}