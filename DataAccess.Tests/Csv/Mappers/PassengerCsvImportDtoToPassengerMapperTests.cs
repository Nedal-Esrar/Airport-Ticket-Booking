using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;
using FluentAssertions.Execution;

namespace DataAccess.Tests.Csv.Mappers;

public class PassengerCsvImportDtoToPassengerMapperTests
{
  [Fact]
  public void Map_ValidatedPassengerCsvImportDto_ShouldReturnTheExpectedPassenger()
  {
    var fixture = new Fixture();
    
    var dto = new PassengerCsvImportDto
    {
      Id = fixture.Freeze<int>(),
      Name = fixture.Freeze<string>()
    };

    var expected = new Passenger
    {
      Id = fixture.Create<int>(),
      Name = fixture.Create<string>()
    };

    var sut = new PassengerCsvImportDtoToPassengerMapper();

    var returnedPassenger = sut.Map(dto);

    returnedPassenger.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(expected);
  }
}