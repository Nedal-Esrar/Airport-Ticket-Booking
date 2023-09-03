using BusinessLogic.Mappers;
using BusinessLogic.PresentationLayerDtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;

namespace BusinessLogic.Tests.Mappers;

public class BookingToBookingDtoMapperTests
{
  private readonly BookingToBookingDtoMapper _sut;

  private readonly Mock<IMapper<Passenger, PassengerDto>> _passengerMapperMock;

  private readonly Mock<IMapper<Flight, FlightDto>> _flightMapperMock;

  public BookingToBookingDtoMapperTests()
  {
    _passengerMapperMock = new();

    _flightMapperMock = new();

    _sut = new(_flightMapperMock.Object, _passengerMapperMock.Object);
  }
  
  [Fact]
  public void Map_Null_ShouldReturnNull()
  {
    var actual = _sut.Map(null);

    actual.Should().BeNull();
  }

  [Fact]
  public void Map_Booking_ShouldReturnDtoWithExpectedBooking()
  {
    var fixture = new Fixture();

    var booking = fixture.Create<Booking>();

    _flightMapperMock
      .Setup(x => x.Map(booking.Flight))
      .Returns(new FlightDto
      {
        Id = booking.Flight.Id,
        ArrivalAirport = booking.Flight.ArrivalAirport,
        Classes = booking.Flight.Classes.Select(details => new FlightClassDetailsDto
        {
          Capacity = details.Capacity,
          Price = details.Price,
          Class = details.Class
        }).ToList(),
        DepartureCountry = booking.Flight.DepartureCountry,
        DepartureDate = booking.Flight.DepartureDate,
        DestinationCountry = booking.Flight.DestinationCountry,
        DepartureAirport = booking.Flight.DepartureAirport
      });

    _passengerMapperMock
      .Setup(x => x.Map(booking.Passenger))
      .Returns(new PassengerDto
      {
        Id = booking.Passenger.Id,
        Name = booking.Passenger.Name
      });

    var actual = _sut.Map(booking);

    actual.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(booking);
  }
}