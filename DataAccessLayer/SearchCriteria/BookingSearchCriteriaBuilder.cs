using DataAccessLayer.Models;

namespace DataAccessLayer.SearchCriteria;

public class BookingSearchCriteriaBuilder
{
  private BookingSearchCriteria _criteria;

  public BookingSearchCriteriaBuilder()
  {
    _criteria = new();
  }

  public BookingSearchCriteriaBuilder SetPassengerId(int id)
  {
    _criteria.PassengerId = id;

    return this;
  }
  
  public BookingSearchCriteriaBuilder SetFlightId(int id)
  {
    _criteria.FlightId = id;

    return this;
  }

  public BookingSearchCriteriaBuilder SetPrice(decimal price)
  {
    _criteria.Price = price;

    return this;
  }
  
  public BookingSearchCriteriaBuilder SetDepartureCountry(string departureCountry)
  {
    _criteria.DepartureCountry = departureCountry;

    return this;
  }
  
  public BookingSearchCriteriaBuilder SetDestinationCountry(string destinationCountry)
  {
    _criteria.DestinationCountry = destinationCountry;

    return this;
  }

  public BookingSearchCriteriaBuilder SetDepartureDate(DateTime date)
  {
    _criteria.DepartureDate = date;

    return this;
  }

  public BookingSearchCriteriaBuilder SetDepartureAirport(string departureAirport)
  {
    _criteria.DepartureAirport = departureAirport;
    
    return this;
  }
  
  public BookingSearchCriteriaBuilder SetArrivalAirport(string arrivalAirport)
  {
    _criteria.ArrivalAirport = arrivalAirport;
    
    return this;
  }
  
  public BookingSearchCriteriaBuilder SetClass(FlightClass flightClass)
  {
    _criteria.Class = flightClass;
    
    return this;
  }

  public BookingSearchCriteria Build()
  {
    return _criteria;
  }
}