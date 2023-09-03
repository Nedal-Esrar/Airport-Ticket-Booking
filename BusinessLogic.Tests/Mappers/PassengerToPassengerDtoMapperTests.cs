using BusinessLogic.Mappers;
using DataAccess.Models;

namespace BusinessLogic.Tests.Mappers;

public class PassengerToPassengerDtoMapperTests
{
  private readonly PassengerToPassengerDtoMapper _sut;

  public PassengerToPassengerDtoMapperTests()
  {
    _sut = new();
  }
  
  [Fact]
  public void Map_Null_ShouldReturnNull()
  {
    var actual = _sut.Map(null);

    actual.Should().BeNull();
  }

  [Fact]
  public void Map_Passenger_ShouldReturnDtoWithExpectedPassenger()
  {
    var fixture = new Fixture();

    var passenger = fixture.Create<Passenger>();

    var actual = _sut.Map(passenger);

    actual.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(passenger);
  }
}