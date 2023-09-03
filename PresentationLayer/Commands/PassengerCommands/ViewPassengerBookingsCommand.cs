using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using PresentationLayer.DisplayUtilities;

namespace PresentationLayer.Commands.PassengerCommands;

public class ViewPassengerBookingsCommand : ICommand
{
  private readonly IBookingService _bookingService;

  private readonly PassengerDto _passenger;

  public ViewPassengerBookingsCommand(IBookingService bookingService, PassengerDto passenger)
  {
    _bookingService = bookingService;

    _passenger = passenger;
  }
  
  public async Task Execute()
  {
    var passengerBookings = await _bookingService.GetPassengerBookings(_passenger.Id);

    if (passengerBookings.Any())
    {
      DisplayBookings(passengerBookings);
    }
    else
    {
      Console.WriteLine(BookingMessages.NoBookingsFound);
    }
  }

  private void DisplayBookings(IEnumerable<BookingDto> passengerBookings)
  {
    Console.WriteLine(BookingMessages.FoundBookingsForPassenger);
    
    var count = 1;

    foreach (var booking in passengerBookings)
    {
      Console.WriteLine($"{count}. {booking}");

      ++count;
    }
  }
}