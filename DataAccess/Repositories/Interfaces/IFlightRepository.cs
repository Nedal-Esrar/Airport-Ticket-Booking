using DataAccess.Models;
using DataAccess.SearchCriteria;

namespace DataAccess.Repositories.Interfaces;

public interface IFlightRepository
{
  IEnumerable<Flight> GetAll();

  IEnumerable<Flight> GetMatchingCriteria(FlightSearchCriteria criteria);

  Flight? GetById(int id);

  void Add(IEnumerable<Flight> flights);
}