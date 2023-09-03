using DataAccess.Models;

namespace DataAccess.SearchCriteria;

public class FlightSearchCriteriaBuilder
{
  private FlightSearchCriteria _criteria;

  public FlightSearchCriteriaBuilder()
  {
    _criteria = new FlightSearchCriteria();
  }

  public FlightSearchCriteriaBuilder SetPrice(decimal price)
  {
    _criteria.Price = price;

    return this;
  }
  
  public FlightSearchCriteriaBuilder SetDepartureCountry(string departureCountry)
  {
    _criteria.DepartureCountry = departureCountry;

    return this;
  }
  
  public FlightSearchCriteriaBuilder SetDestinationCountry(string destinationCountry)
  {
    _criteria.DestinationCountry = destinationCountry;

    return this;
  }

  public FlightSearchCriteriaBuilder SetDepartureDate(DateTime date)
  {
    _criteria.DepartureDate = date;

    return this;
  }

  public FlightSearchCriteriaBuilder SetDepartureAirport(string departureAirport)
  {
    _criteria.DepartureAirport = departureAirport;
    
    return this;
  }
  
  public FlightSearchCriteriaBuilder SetArrivalAirport(string arrivalAirport)
  {
    _criteria.ArrivalAirport = arrivalAirport;
    
    return this;
  }
  
  public FlightSearchCriteriaBuilder SetClass(FlightClass flightClass)
  {
    _criteria.Class = flightClass;
    
    return this;
  }

  public FlightSearchCriteria Build()
  {
    return _criteria;
  }
}