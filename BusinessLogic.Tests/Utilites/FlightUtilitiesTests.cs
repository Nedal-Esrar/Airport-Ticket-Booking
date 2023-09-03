using BusinessLogic.Utilites;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Tests.Utilites;

public class FlightUtilitiesTests
{
  private readonly Mock<IBookingRepository> _bookingRepositoryMock;

  private readonly Flight _flight;

  private readonly Fixture _fixture;

  public FlightUtilitiesTests()
  {
    _bookingRepositoryMock = new();

    _fixture = new Fixture();

    _flight = new Flight
    {
      Id = 1,
      Classes = new List<FlightClassDetails>
      {
        new() {
          Capacity = 40,
          Class = FlightClass.Economy
        }
      }
    };

    _fixture.Register<Booking>(() => new()
    {
      Flight = _flight,
      BookingClass = FlightClass.Economy
    });
  }
  
  [Fact]
  public async Task IsClassAvailableToBook_ClassAvailable_ReturnsTrue()
  {
    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(1, FlightClass.Economy))
      .ReturnsAsync(_fixture.CreateMany<Booking>(new Random().Next(40)));

    var actual = await FlightUtilities.IsClassAvailableToBook(_flight, FlightClass.Economy, _bookingRepositoryMock.Object);

    actual.Should().BeTrue();
  }

  [Fact]
  public async Task IsClassAvailableToBook_ClassNotAvailable_ReturnsFalse()
  {
    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(1, FlightClass.Economy))
      .ReturnsAsync(_fixture.CreateMany<Booking>(40));

    var actual = await FlightUtilities.IsClassAvailableToBook(_flight, FlightClass.Economy, _bookingRepositoryMock.Object);

    actual.Should().BeFalse();
  }
}