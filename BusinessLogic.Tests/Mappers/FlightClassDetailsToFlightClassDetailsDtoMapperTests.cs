using BusinessLogic.Mappers;
using DataAccess.Models;

namespace BusinessLogic.Tests.Mappers;

public class FlightClassDetailsToFlightClassDetailsDtoMapperTests
{
  private readonly FlightClassDetailsToFlightClassDetailsDtoMapper _sut;

  public FlightClassDetailsToFlightClassDetailsDtoMapperTests()
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
  public void Map_FlightClassDetails_ShouldReturnDtoWithExpectedFlightClassDetails()
  {
    var fixture = new Fixture();

    var flightClassDetails = fixture.Create<FlightClassDetails>();

    var actual = _sut.Map(flightClassDetails);

    actual.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(flightClassDetails);
  }
}