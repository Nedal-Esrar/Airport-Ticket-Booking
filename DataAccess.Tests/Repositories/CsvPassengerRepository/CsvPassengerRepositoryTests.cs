using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Models;
using DataAccess.Repositories.Implementations;
using FluentAssertions.Execution;

namespace DataAccess.Tests.Repositories;

public class CsvPassengerRepositoryTests
{
  private readonly CsvPassengerRepository _sut;

  private readonly Mock<IImportFromCsvService<PassengerCsvImportDto, Passenger>> _importFromCsvServiceMock;

  private readonly List<Passenger> _passengers;

  public CsvPassengerRepositoryTests()
  {
    _passengers = TestsData.Passengers;
    
    _importFromCsvServiceMock = new();
    
    _sut = new CsvPassengerRepository(_importFromCsvServiceMock.Object);

    _importFromCsvServiceMock
      .Setup(x => x.ImportFromCsv(_sut.PathToCsv))
      .ReturnsAsync(new DataImportResult<Passenger>
      {
        ValidObjects = _passengers
      });
  }

  [Fact]
  public async Task GetById_RequiredPassengerExists_ShouldReturnFirstPassengerMatchingId()
  {
    var expectedPassenger = _passengers.First();

    var returnedPassenger = await _sut.GetById(expectedPassenger.Id);
    
    returnedPassenger.Should().BeSameAs(expectedPassenger);
  }
  
  [Fact]
  public async Task GetById_RequiredPassengerDoesNotExists_ShouldReturnNull()
  {
    var id = 1000;

    var returnedPassenger = await _sut.GetById(id);

    returnedPassenger.Should().BeNull();
  }
}