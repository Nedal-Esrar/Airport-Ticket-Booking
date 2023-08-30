using DataAccess.Csv.Dtos;

namespace DataAccess.Csv.Parsers;

public class PassengerCsvImportDtoParser : IParser<PassengerCsvImportDto>
{
  public PassengerCsvImportDto? Parse(string[] fields)
  {
    if (typeof(PassengerCsvImportDto).GetProperties().Length != fields.Length)
    {
      return null;
    }
    
    return new PassengerCsvImportDto
    {
      Id = int.TryParse(fields[0], out var id) ? id : null,
      Name = fields[1].Trim()
    };
  }
}