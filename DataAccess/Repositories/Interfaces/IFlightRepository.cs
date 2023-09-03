using DataAccess.Models;
using DataAccess.SearchCriteria;

namespace DataAccess.Repositories.Interfaces;

public interface IFlightRepository
{
  Task<IEnumerable<Flight>> GetAll();

  Task<IEnumerable<Flight>> GetMatchingCriteria(FlightSearchCriteria criteria);

  Task<Flight?> GetById(int id);

  Task Add(IEnumerable<Flight> flights);
}