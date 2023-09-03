using DataAccess.Csv.Dtos;

namespace DataAccess.Tests.Csv.Reader;

public class TestData
{
  public static (string[], PassengerCsvImportDto)[] PassengerRecordsAndCorrespondingObjects =
  {
    (new[] { "1", "said" }, new PassengerCsvImportDto { Id = 1, Name = "said" }),
    (new[] { "2", "mustafa" }, new PassengerCsvImportDto { Id = 2, Name = "mustaf" })
  };
}