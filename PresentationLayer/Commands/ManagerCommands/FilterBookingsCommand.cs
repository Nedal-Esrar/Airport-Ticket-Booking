using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.SearchCriteria;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.Commands.ManagerCommands;

public class FilterBookingsCommand : ICommand
{
  private readonly IBookingService _bookingService;

  public FilterBookingsCommand(IBookingService bookingService)
  {
    _bookingService = bookingService;
  }
  
  public void Execute()
  {
    Console.WriteLine(BookingMessages.EnterBookingSearchCriteria);

    var criteria = BuildBookingSearchCriteria();

    var availableFlightsMatchingCriteria = _bookingService.GetBookingsMatchingCriteria(criteria);

    if (availableFlightsMatchingCriteria.Any())
    {
      DisplayBookings(availableFlightsMatchingCriteria);
    }
    else
    {
      Console.WriteLine(FlightMessages.NoAvailableFlightsMatchingCriteria);
    }
  }
  
  private BookingSearchCriteria BuildBookingSearchCriteria()
  {
    var criteriaBuilder = new BookingSearchCriteriaBuilder();
    
    var passengerId = InputParser.GetInput<int?>(InputPrompts.PassengerIdPromptWithSkip,
      ParseFunctionsWithSkip.TryParseId);

    if (passengerId is not null)
    {
      criteriaBuilder.SetPassengerId((int)passengerId);
    }
    
    var flightId = InputParser.GetInput<int?>(InputPrompts.FlightIdPromptWithSkip,
      ParseFunctionsWithSkip.TryParseId);
    
    if (flightId is not null)
    {
      criteriaBuilder.SetPassengerId((int)flightId);
    }

    var price = InputParser.GetInput<decimal?>(InputPrompts.PricePromptWithSkip,
      ParseFunctionsWithSkip.TryParsePrice);

    if (price is not null)
    {
      criteriaBuilder.SetPrice((decimal)price);
    }

    var departureCountry = InputParser.GetInput<string>(InputPrompts.DepartureCountryPromptWithSkip,
      ParseFunctionsWithSkip.TryParseString);

    if (departureCountry is not null)
    {
      criteriaBuilder.SetDepartureCountry(departureCountry);
    }

    var destinationCountry = InputParser.GetInput<string>(InputPrompts.DestinationCountryPromptWithSkip,
      ParseFunctionsWithSkip.TryParseString);

    if (destinationCountry is not null)
    {
      criteriaBuilder.SetDestinationCountry(destinationCountry);
    }

    var departureDate = InputParser.GetInput<DateTime?>(InputPrompts.DepartureDatePromptWithSkip,
      ParseFunctionsWithSkip.TryParseDate);

    if (departureDate is not null)
    {
      criteriaBuilder.SetDepartureDate((DateTime)departureDate);
    }

    var departureAirport = InputParser.GetInput<string>(InputPrompts.DepartureAirportPromptWithSkip,
      ParseFunctionsWithSkip.TryParseString);

    if (departureAirport is not null)
    {
      criteriaBuilder.SetDepartureAirport(departureAirport);
    }

    var arrivalAirport = InputParser.GetInput<string>(InputPrompts.ArrivalAirportPromptWithSkip,
      ParseFunctionsWithSkip.TryParseString);

    if (arrivalAirport is not null)
    {
      criteriaBuilder.SetDepartureAirport(arrivalAirport);
    }

    var flightClass = InputParser.GetInput<FlightClass?>(InputPrompts.ClassPromptWithSkip,
      ParseFunctionsWithSkip.TryParseEnum);

    if (flightClass is not null)
    {
      criteriaBuilder.SetClass((FlightClass)flightClass);
    }

    return criteriaBuilder.Build();
  }

  private void DisplayBookings(IEnumerable<BookingDto> bookings)
  {
    Console.WriteLine(BookingMessages.FoundBookings);

    var count = 1;
    
    foreach (var booking in bookings)
    {
      Console.WriteLine($"{count}. {booking}");

      ++count;
    }
  }
}