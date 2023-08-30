using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Models;

namespace DataAccessLayer.Csv.Mappers;

public class PassengerCsvImportDtoToPassengerMapper : IMapper<PassengerCsvImportDto, Passenger>
{
  public Passenger? Map(PassengerCsvImportDto dto)
  {
    return new Passenger
    {
      Id = (int)dto.Id!,
      Name = dto.Name!
    };
  }
}