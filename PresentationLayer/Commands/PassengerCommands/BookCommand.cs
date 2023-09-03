using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using DataAccess.Models;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.Commands.PassengerCommands;

public class BookCommand : ICommand
{
  private readonly IFlightService _flightService;

  private readonly IBookingService _bookingService;
  
  private readonly PassengerDto _passenger;
  
  public BookCommand(IFlightService flightRepository, IBookingService bookingRepository, PassengerDto passenger)
  {
    _flightService = flightRepository;

    _bookingService = bookingRepository;
    
    _passenger = passenger;
  }
  
  public async Task Execute()
  {
    Console.WriteLine(FlightMessages.EnterFlightId);
    
    var flightId = InputParser.GetInput<int>(FlightMessages.FlightIdPromptWithoutSkip,
      ParseFunctionsWithoutSkip.TryParseId);

    var flight = await _flightService.GetById(flightId);

    if (flight == null)
    {
      Console.WriteLine(FlightMessages.FlightDoesNotExist);

      return;
    }

    Console.WriteLine(FlightMessages.EnterFlightClass);

    var flightClass = InputParser.GetInput<FlightClass>(FlightMessages.FlightClassPromptWithoutSkip,
      ParseFunctionsWithoutSkip.TryParseEnum);

    if (flight.Classes.All(details => details.Class != flightClass))
    {
      Console.WriteLine(FlightMessages.ClassDoesNotExist);

      return;
    }

    var isBookingSuccessful = await _bookingService.BookFlight(flight.Id, _passenger.Id, flightClass);

    Console.WriteLine(isBookingSuccessful
      ? BookingMessages.SuccessfulBooking
      : FlightMessages.NotAvailableFlight);
  }
}