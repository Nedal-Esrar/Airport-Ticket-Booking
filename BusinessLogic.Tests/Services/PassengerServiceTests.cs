using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Implementations;
using DataAccess.Csv.Mappers;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Tests.Services;

public class PassengerServiceTests
{
  private readonly PassengerService _sut;

  private readonly Mock<IPassengerRepository> _passengerRepositoryMock;

  private readonly Mock<IMapper<Passenger, PassengerDto>> _passengerMapperMock;

  private readonly Fixture _fixture;

  public PassengerServiceTests()
  {
    _passengerRepositoryMock = new();

    _passengerMapperMock = new();

    _sut = new(_passengerRepositoryMock.Object, _passengerMapperMock.Object);

    _fixture = new Fixture();
  }

  [Fact]
  public async Task GetById_PassengerDoesNotExist_ShouldReturnNull()
  {
    var id = _fixture.Create<int>();

    var actual = await _sut.GetById(id);

    actual.Should().BeNull();
  }

  [Fact]
  public async Task GetById_PassengerExists_ShouldReturnTheExpectedDto()
  {
    var id = 1;

    var passenger = new Passenger { Id = id };

    _passengerRepositoryMock
      .Setup(x => x.GetById(id))
      .ReturnsAsync(passenger);

    var passengerDto = new PassengerDto { Id = id };

    _passengerMapperMock
      .Setup(x => x.Map(passenger))
      .Returns(passengerDto);

    var actual = await _sut.GetById(id);

    actual.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(passengerDto);
  }
}