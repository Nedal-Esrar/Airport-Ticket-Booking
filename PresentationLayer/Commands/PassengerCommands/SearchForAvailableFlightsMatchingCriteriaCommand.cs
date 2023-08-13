using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.SearchCriteria;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.Commands.PassengerCommands;

public class SearchForAvailableFlightsMatchingCriteriaCommand : ICommand
{
  private readonly IFlightService _flightService;

  public SearchForAvailableFlightsMatchingCriteriaCommand(IFlightService flightService)
  {
    _flightService = flightService;
  }
  
  public void Execute()
  {
    Console.WriteLine(FlightMessages.EnterFlightSearchCriteria);

    var criteria = BuildFlightSearchCriteria();

    var availableFlightsMatchingCriteria = _flightService.GetAvailableFlightsMatchingCriteria(criteria);

    if (availableFlightsMatchingCriteria.Any())
    {
      DisplayAvailableFlights(availableFlightsMatchingCriteria);
    }
    else
    {
      Console.WriteLine(FlightMessages.NoAvailableFlightsMatchingCriteria);
    }
  }

  private static void DisplayAvailableFlights(IEnumerable<FlightDto> flights)
  {
    Console.WriteLine(FlightMessages.FoundFlightMatchingCriteria);

    var count = 1;

    foreach (var flight in flights)
    {
      Console.WriteLine($"{count}. {flight}");

      ++count;
    }
  }

  private FlightSearchCriteria BuildFlightSearchCriteria()
  {
    var criteriaBuilder = new FlightSearchCriteriaBuilder();

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
}