using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Exceptions;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using FluentAssertions.Execution;

namespace DataAccess.Tests.Csv.Mappers;

public class BookingCsvImportDtoToBookingMapperTests
{
  private readonly BookingCsvImportDtoToBookingMapper _sut;

  private readonly Mock<IFlightRepository> _flightRepositoryMock;

  private readonly Mock<IPassengerRepository> _passengerRepositoryMock;

  public BookingCsvImportDtoToBookingMapperTests()
  {
    _flightRepositoryMock = new Mock<IFlightRepository>();

    _passengerRepositoryMock = new Mock<IPassengerRepository>();

    _sut = new BookingCsvImportDtoToBookingMapper(_flightRepositoryMock.Object, _passengerRepositoryMock.Object);
  }

  [Fact]
  public void Map_Null_ShouldReturnNull()
  {
    var actual = _sut.Map(null);

    actual.Should().BeNull();
  }

  [Fact]
  public void Map_ValidatedBookingCsvImportDtoObeysReferentialIntegrity_ShouldReturnTheIntendedBooking()
  {
    var passenger = new Passenger
    {
      Id = 1
    };

    var flight = new Flight
    {
      Id = 1
    };
    
    _flightRepositoryMock
      .Setup(x => x.GetById(1))
      .ReturnsAsync(flight);
    
    _passengerRepositoryMock
      .Setup(x => x.GetById(1))
      .ReturnsAsync(passenger);
    
    var dto = new BookingCsvImportDto
    {
      Id = 1,
      PassengerId = 1,
      FlightId = 1,
      FlightClass = FlightClass.Business,
      BookingDate = new DateTime(2020, 12, 30)
    };
  
    var expected = new Booking
    {
      Id = 1,
      Passenger = passenger,
      Flight = flight,
      BookingClass = FlightClass.Business,
      BookingDate = new DateTime(2020, 12, 30)
    };

    var returnedBooking = _sut.Map(dto);

    returnedBooking.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(expected);
  }

  [Fact]
  public void Map_ValidatedBookingCsvImportDtoDoesNotObeysReferentialIntegrity_ShouldThrowIntegrityException()
  {
    var dto = new BookingCsvImportDto
    {
      Id = 1,
      PassengerId = 2,
      FlightId = 10,
      FlightClass = FlightClass.Business,
      BookingDate = new DateTime(2020, 12, 30)
    };

    Action act = () => _sut.Map(dto);

    act.Should().Throw<IntegrityException>();
  }
}