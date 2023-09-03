using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.DateTimeProvider;
using BusinessLogic.Services.Implementations;
using DataAccess.Csv.Mappers;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.SearchCriteria;

namespace BusinessLogic.Tests.Services;

public class BookingServiceTests
{
  private readonly BookingService _sut;
  
  private readonly Mock<IBookingRepository> _bookingRepositoryMock;

  private readonly Mock<IFlightRepository> _flightRepositoryMock;

  private readonly Mock<IPassengerRepository> _passengerRepositoryMock;

  private readonly Mock<IMapper<Booking, BookingDto>> _bookingMapperMock;

  private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

  private readonly Fixture _fixture;

  public BookingServiceTests()
  {
    _bookingRepositoryMock = new();

    _flightRepositoryMock = new();

    _passengerRepositoryMock = new();

    _bookingMapperMock = new();

    _dateTimeProviderMock = new();
    
    _bookingMapperMock
      .Setup(x => x.Map(It.IsAny<Booking>()))
      .Returns(new Func<Booking?, BookingDto?>(booking => booking is null ? null : new BookingDto { Id = booking.Id }));

    _fixture = new();

    _sut = new BookingService(_bookingRepositoryMock.Object, _flightRepositoryMock.Object,
      _passengerRepositoryMock.Object, _bookingMapperMock.Object, _dateTimeProviderMock.Object);
  }
  
  [Fact]
  public async Task GetById_BookingDoesNotExist_ShouldReturnNull()
  {
    var id = _fixture.Create<int>();

    var actual = await _sut.GetById(id);

    actual.Should().BeNull();
  }

  [Fact]
  public async Task GetById_BookingExists_ShouldReturnTheExpectedDto()
  {
    var id = 1;

    var booking = new Booking { Id = id };

    _bookingRepositoryMock
      .Setup(x => x.GetById(id))
      .ReturnsAsync(booking);

    var bookingDto = new BookingDto { Id = id };

    var actual = await _sut.GetById(id);

    actual.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(bookingDto);
  }

  [Fact]
  public async Task GetPassengerBookings_PassengerId_ShouldReturnCorrespondingDtosForPassengersBookings()
  {
    var id = _fixture.Create<int>();

    _fixture.Register(() => new Booking { Passenger = new Passenger { Id = id } });

    var passengerBookings = _fixture.CreateMany<Booking>();

    _bookingRepositoryMock
      .Setup(x => x.GetPassengerBookings(id))
      .ReturnsAsync(passengerBookings);

    var actual = await _sut.GetPassengerBookings(id);

    actual.Should().BeEquivalentTo(passengerBookings.Select(booking => new BookingDto { Id = booking.Id }));
  }

  [Fact]
  public async Task GetBookingsMatchingCriteria_Criteria_ShouldReturnCorrespondingDtosForFlightMatchingCriteria()
  {
    var criteria = _fixture.Create<BookingSearchCriteria>();

    var bookings = _fixture.CreateMany<Booking>();
    
    _bookingRepositoryMock
      .Setup(x => x.GetMatchingCriteria(criteria))
      .ReturnsAsync(bookings);

    var actual = await _sut.GetBookingsMatchingCriteria(criteria);

    actual.Should().BeEquivalentTo(bookings.Select(booking => new BookingDto { Id = booking.Id }));
  }

  [Fact]
  public async Task CancelBooking_BookingWithIdDoesNotExist_ShouldNotInvokeReposRemove()
  {
    var id = _fixture.Create<int>();

    await _sut.CancelBooking(id);
    
    _bookingRepositoryMock.Verify(x => x.Remove(It.Is<Booking>(booking => booking.Id == id)), Times.Never);
  }
  
  [Fact]
  public async Task CancelBooking_BookingWithIdExists_ShouldInvokeReposRemoveOnce()
  {
    var id = _fixture.Create<int>();

    var booking = new Booking { Id = id };

    _bookingRepositoryMock
      .Setup(x => x.GetById(id))
      .ReturnsAsync(booking);

    await _sut.CancelBooking(id);
    
    _bookingRepositoryMock.Verify(x => x.Remove(booking), Times.Once);
  }

  [Fact]
  public async Task BookFlight_FlightWithIdDoesNotExist_ShouldReturnFalse()
  {
    var actual = await _sut.BookFlight(_fixture.Create<int>(), _fixture.Create<int>(), _fixture.Create<FlightClass>());

    actual.Should().BeFalse();
  }

  [Fact]
  public async Task BookFlight_FlightExistsButClassDoesNot_ShouldReturnFalse()
  {
    var flightId = _fixture.Create<int>();

    _flightRepositoryMock
      .Setup(x => x.GetById(flightId))
      .ReturnsAsync(new Flight { Id = flightId, Classes = new List<FlightClassDetails>() });

    var actual = await _sut.BookFlight(flightId, _fixture.Create<int>(), _fixture.Create<FlightClass>());

    actual.Should().BeFalse();
  }

  [Fact]
  public async Task BookFlight_FlightAndClassExistsButClassIsNotAvailable_ShouldReturnFalse()
  {
    var flightId = _fixture.Create<int>();

    _flightRepositoryMock
      .Setup(x => x.GetById(flightId))
      .ReturnsAsync(new Flight { Id = flightId, Classes = new List<FlightClassDetails> { new() { Capacity = 2, Class = FlightClass.Economy }}});

    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(flightId, FlightClass.Economy))
      .ReturnsAsync(_fixture.CreateMany<Booking>());
    
    var actual = await _sut.BookFlight(flightId, _fixture.Create<int>(), FlightClass.Economy);
    
    actual.Should().BeFalse();
  }

  [Fact]
  public async Task BookFlight_FlightAndClassExistAndClassIsAvailableButPassengerDoesNotExist_ShouldReturnFalse()
  {
    var flightId = _fixture.Create<int>();

    _flightRepositoryMock
      .Setup(x => x.GetById(flightId))
      .ReturnsAsync(new Flight { Id = flightId, Classes = new List<FlightClassDetails> { new() { Capacity = 2, Class = FlightClass.Economy }}});

    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(flightId, FlightClass.Economy))
      .ReturnsAsync(_fixture.CreateMany<Booking>(1));
    
    var actual = await _sut.BookFlight(flightId, _fixture.Create<int>(), FlightClass.Economy);
    
    actual.Should().BeFalse();
  }

  [Fact]
  public async Task BookFlight_FlightAndClassAndPassengerExistAndClassIsAvailable_ShouldReturnTrue()
  {
    var flightId = _fixture.Create<int>();

    _flightRepositoryMock
      .Setup(x => x.GetById(flightId))
      .ReturnsAsync(new Flight { Id = flightId, Classes = new List<FlightClassDetails> { new() { Capacity = 2, Class = FlightClass.Economy }}});

    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(flightId, FlightClass.Economy))
      .ReturnsAsync(_fixture.CreateMany<Booking>(1));

    var passengerId = _fixture.Create<int>();

    _passengerRepositoryMock
      .Setup(x => x.GetById(passengerId))
      .ReturnsAsync(new Passenger());
    
    var actual = await _sut.BookFlight(flightId, passengerId, FlightClass.Economy);
    
    actual.Should().BeTrue();
  }
  
  [Fact]
  public async Task BookFlight_FlightAndClassAndPassengerExistAndClassIsAvailable_ShouldInvokeReposAddWithExpectedBooking()
  {
    var flightId = _fixture.Create<int>();
  
    var flight = new Flight
      { Id = 1, Classes = new List<FlightClassDetails> { new() { Capacity = 2, Class = FlightClass.Economy } } };
  
    _flightRepositoryMock
      .Setup(x => x.GetById(flightId))
      .ReturnsAsync(flight);
  
    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(flightId, FlightClass.Economy))
      .ReturnsAsync(_fixture.CreateMany<Booking>(1));
  
    var passengerId = _fixture.Create<int>();
  
    var passenger = new Passenger();
  
    _passengerRepositoryMock
      .Setup(x => x.GetById(passengerId))
      .ReturnsAsync(passenger);
  
    var bookingDate = _fixture.Create<DateTime>();
  
    _dateTimeProviderMock
      .Setup(x => x.GetCurrentDateTime())
      .Returns(bookingDate);
    
    await _sut.BookFlight(flightId, passengerId, FlightClass.Economy);
    
    _bookingRepositoryMock.Verify(x => x.Add(It.Is<Booking>(booking =>
      booking.Id == 1 && 
      booking.BookingClass == FlightClass.Economy && 
      booking.BookingDate == bookingDate && 
      booking.Flight == flight && 
      booking.Passenger == passenger
    )), Times.Once);
  }
  
  [Fact]
  public async Task ModifyBooking_BookingWithIdDoesNotExist_ShouldReturnFalse()
  {
    var actual = await _sut.ModifyBooking(_fixture.Create<int>(), _fixture.Create<FlightClass>());

    actual.Should().BeFalse();
  }

  [Fact]
  public async Task ModifyBooking_BookingExistsButClassDoesNot_ShouldReturnFalse()
  {
    var bookingId = _fixture.Create<int>();

    var flightId = _fixture.Create<int>();

    _bookingRepositoryMock
      .Setup(x => x.GetById(bookingId))
      .ReturnsAsync(new Booking
      {
        Id = bookingId,
        Flight = new Flight
          { Id = flightId, Classes = new List<FlightClassDetails>()}
      });

    var actual = await _sut.ModifyBooking(bookingId, _fixture.Create<FlightClass>());

    actual.Should().BeFalse();
  }

  [Fact]
  public async Task ModifyBooking_BookingAndClassExistsButClassIsNotAvailable_ShouldReturnFalse()
  {
    var bookingId = _fixture.Create<int>();

    var flightId = _fixture.Create<int>();
    
    _bookingRepositoryMock
      .Setup(x => x.GetById(bookingId))
      .ReturnsAsync(new Booking
      {
        Id = bookingId,
        Flight = new Flight
          { 
            Id = flightId, 
            Classes = new List<FlightClassDetails>
            {
              new() { Capacity = 2, Class = FlightClass.Economy },
              new() { Capacity = 2, Class = FlightClass.Business }
            } 
          }
      });

    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(flightId, FlightClass.Business))
      .ReturnsAsync(_fixture.CreateMany<Booking>());
    
    var actual = await _sut.ModifyBooking(bookingId, FlightClass.Business);
    
    actual.Should().BeFalse();
  }
  
  [Fact]
  public async Task ModifyBooking_BookingAndClassExistAndClassIsAvailable_ShouldReturnTrue()
  {
    var bookingId = _fixture.Create<int>();

    var flightId = _fixture.Create<int>();
    
    _bookingRepositoryMock
      .Setup(x => x.GetById(bookingId))
      .ReturnsAsync(new Booking
      {
        Id = bookingId,
        Flight = new Flight
        { 
          Id = flightId, 
          Classes = new List<FlightClassDetails>
          {
            new() { Capacity = 2, Class = FlightClass.Economy },
            new() { Capacity = 2, Class = FlightClass.Business }
          } 
        }
      });

    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(flightId, FlightClass.Business))
      .ReturnsAsync(_fixture.CreateMany<Booking>(1));
    
    var actual = await _sut.ModifyBooking(bookingId, FlightClass.Business);
    
    actual.Should().BeTrue();
  }

  [Fact]
  public async Task ModifyBooking_BookingAndClassExistAndClassIsAvailable_ShouldInvokeReposUpdateWithTheUpdatedBooking()
  {
    var bookingId = _fixture.Create<int>();

    var flightId = _fixture.Create<int>();

    var bookingToUpdate = new Booking
    {
      Id = bookingId,
      Flight = new Flight
      {
        Id = flightId,
        Classes = new List<FlightClassDetails>
        {
          new() { Capacity = 2, Class = FlightClass.Economy },
          new() { Capacity = 2, Class = FlightClass.Business }
        }
      }
    };
    
    _bookingRepositoryMock
      .Setup(x => x.GetById(bookingId))
      .ReturnsAsync(bookingToUpdate);

    _bookingRepositoryMock
      .Setup(x => x.GetBookingsForFlightWithClass(flightId, FlightClass.Business))
      .ReturnsAsync(_fixture.CreateMany<Booking>(1));
    
    await _sut.ModifyBooking(bookingId, FlightClass.Business);

    _bookingRepositoryMock
      .Verify(x => x.Update(
        It.Is<Booking>(booking => booking == bookingToUpdate && booking.BookingClass == FlightClass.Business)),
        Times.Once);
  }
}