using BusinessLogic.PresentationLayerDtos;
using DataAccess.SearchCriteria;

namespace BusinessLogic.Services.Interfaces;

public interface IFlightService
{
  Task<FlightDto?> GetById(int id);
  
  Task<IEnumerable<FlightDto>> GetAvailableFlightsMatchingCriteria(FlightSearchCriteria criteria);

  Task<IList<string>> ImportFromCsv(string pathToFile);
}