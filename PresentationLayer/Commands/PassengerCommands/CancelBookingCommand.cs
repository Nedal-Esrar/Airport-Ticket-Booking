using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.Commands.PassengerCommands;

public class CancelBookingCommand : ICommand
{
  private readonly IBookingService _bookingService;

  private readonly PassengerDto _passenger;

  public CancelBookingCommand(IBookingService bookingService, PassengerDto passenger)
  {
    _bookingService = bookingService;

    _passenger = passenger;
  }
  
  public async Task Execute()
  {
    Console.WriteLine(BookingMessages.EnterBookingId);
    
    var bookingId = InputParser.GetInput<int>(InputPrompts.BookingIdPromptWithoutSkip,
      ParseFunctionsWithoutSkip.TryParseId);

    var booking = await _bookingService.GetById(bookingId);

    if (booking == null)
    {
      Console.WriteLine(BookingMessages.BookingDoesNotExist);

      return;
    }

    var passengerBookings = await _bookingService.GetPassengerBookings(_passenger.Id);

    var doesBookingBelongToPassenger = passengerBookings.Any(booking => booking.Passenger.Id == _passenger.Id);

    if (!doesBookingBelongToPassenger)
    {
      Console.WriteLine(BookingMessages.BookingDoesNotBelongToPassenger);

      return;
    }
    
    await _bookingService.CancelBooking(bookingId);
    
    Console.WriteLine(BookingMessages.SuccessfulBookingCancellation);
  }
}