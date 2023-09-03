using BusinessLogic.Mappers;
using BusinessLogic.PresentationLayerDtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;

namespace BusinessLogic.Tests.Mappers;

public class FlightToFlightDtoMapperTests
{
  private readonly FlightToFlightDtoMapper _sut;

  private readonly Mock<IMapper<FlightClassDetails, FlightClassDetailsDto>> _classDetailsMapperMock;

  public FlightToFlightDtoMapperTests()
  {
    _classDetailsMapperMock = new();

    _sut = new(_classDetailsMapperMock.Object);
  }
  
  [Fact]
  public void Map_Null_ShouldReturnNull()
  {
    var actual = _sut.Map(null);

    actual.Should().BeNull();
  }

  [Fact]
  public void Map_Flight_ShouldReturnDtoWithExpectedFlight()
  {
    var fixture = new Fixture();

    var flight = fixture.Create<Flight>();

    foreach (var classDetails in flight.Classes)
    {
      _classDetailsMapperMock
        .Setup(x => x.Map(classDetails))
        .Returns(new FlightClassDetailsDto
        {
          Class = classDetails.Class,
          Capacity = classDetails.Capacity,
          Price = classDetails.Price
        });
    }

    var actual = _sut.Map(flight);

    actual.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(flight);
  }
}