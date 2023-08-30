using BusinessLogic.PresentationLayerDtos;
using DataAccessLayer.SearchCriteria;

namespace BusinessLogic.Services.Interfaces;

public interface IFlightService
{
  FlightDto? GetById(int id);
  
  IEnumerable<FlightDto> GetAvailableFlightsMatchingCriteria(FlightSearchCriteria criteria);

  IList<string> ImportFromCsv(string pathToFile);
}