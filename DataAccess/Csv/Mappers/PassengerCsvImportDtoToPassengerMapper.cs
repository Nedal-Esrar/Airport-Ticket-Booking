using DataAccess.Csv.Dtos;
using DataAccess.Models;

namespace DataAccess.Csv.Mappers;

public class PassengerCsvImportDtoToPassengerMapper : IMapper<PassengerCsvImportDto, Passenger>
{
  public Passenger? Map(PassengerCsvImportDto? dto)
  {
    if (dto is null)
    {
      return null;
    }
    
    return new Passenger
    {
      Id = (int)dto.Id!,
      Name = dto.Name!
    };
  }
}