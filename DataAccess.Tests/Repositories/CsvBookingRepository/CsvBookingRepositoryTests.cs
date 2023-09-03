using DataAccess.Csv;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Models;
using DataAccess.Repositories.Implementations;
using DataAccess.SearchCriteria;

namespace DataAccess.Tests.Repositories.BookingRepository;

public class CsvBookingRepositoryTests
{
  private readonly CsvBookingRepository _sut;

  private readonly Mock<ICsvWriter> _csvWriterMock;

  private readonly Mock<IImportFromCsvService<BookingCsvImportDto, Booking>> _importFromCsvServiceMock;

  private readonly List<Booking> _bookings;

  private readonly Fixture _fixture;

  public CsvBookingRepositoryTests()
  {
    _bookings = TestData.Bookings;
    
    _csvWriterMock = new();

    _importFromCsvServiceMock = new();

    _sut = new(_importFromCsvServiceMock.Object, _csvWriterMock.Object);

    _importFromCsvServiceMock
      .Setup(x => x.ImportFromCsv(_sut.PathToCsv))
      .ReturnsAsync(new DataImportResult<Booking>
      {
        ValidObjects = _bookings
      });

    _fixture = new();
  }
  
  [Fact]
  public async Task GetAll_AllBookings_ShouldReturnAllFlightsWithFirstOccuringIds()
  {
    var expected = _bookings.DistinctBy(booking => booking.Id);

    var returnedBookings = await _sut.GetAll();

    returnedBookings.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public async Task GetById_RequiredBookingExists_ShouldReturnTheFirstFlightWithRequestedId()
  {
    var expected = _bookings.First();

    var returnedBooking = await _sut.GetById(expected.Id);

    returnedBooking.Should().BeSameAs(expected);
  }

  [Fact]
  public async Task GetById_RequiredBookingDoesNotExist_ShouldReturnNull()
  {
    var id = 1000;

    var returnedBooking = await _sut.GetById(id);

    returnedBooking.Should().BeNull();
  }

  [Fact]
  public async Task Add_BookingObject_CsvWriterAppendAsyncShouldBeCalledOnceWithTheObject()
  {
    var booking = _fixture.Create<Booking>();

    await _sut.Add(booking);

    _csvWriterMock.Verify(x => x.AppendAsync(_sut.PathToCsv, booking), Times.Once);
  }

  [Fact]
  public async Task Remove_BookingObject_CsvWriterWriteAsyncShouldBeCalledOnceWithoutTheObject()
  {
    var distinctIdBookings = _bookings.DistinctBy(booking => booking.Id);
    
    var bookingToRemove = distinctIdBookings.First();

    var bookingsWithoutRemoved = distinctIdBookings.Where(booking => booking.Id != bookingToRemove.Id);

    await _sut.Remove(bookingToRemove);
    
    _csvWriterMock.Verify(x => x.WriteAsync(_sut.PathToCsv, bookingsWithoutRemoved), Times.Once);
  }

  [Fact]
  public async Task Update_BookingObject_CsvWriterWriteAsyncShouldBeCalledOnceWithTheUpdatedObject()
  {
    var distinctIdBookings = _bookings.DistinctBy(booking => booking.Id);

    var updatedBooking = new Booking
    {
      Id = distinctIdBookings.First().Id
    };

    var bookingsWithUpdatedBooking = distinctIdBookings
      .Select(bookingToSelect => bookingToSelect.Id == updatedBooking.Id ? updatedBooking : bookingToSelect);

    await _sut.Update(updatedBooking);
    
    _csvWriterMock.Verify(x => x.WriteAsync(_sut.PathToCsv, bookingsWithUpdatedBooking), Times.Once);
  }

  [Theory]
  [MemberData(nameof(TestData.GetPassengerBookingsTestData), MemberType = typeof(TestData))]
  public async Task GetPassengerBookings_PassengerId_ShouldReturnExpectedBookings(int passengerId,
    IEnumerable<Booking> expected)
  {
    var returnedBookings = await _sut.GetPassengerBookings(passengerId);

    returnedBookings.Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.GetBookingsForFlightWithClassTestData), MemberType = typeof(TestData))]
  public async Task GetBookingsForFlightWithClass_FlightIdAndClass_ShouldReturnExpectedBookings(int flightId,
    FlightClass flightClass, IEnumerable<Booking> expected)
  {
    var returnedBookings = await _sut.GetBookingsForFlightWithClass(flightId, flightClass);

    returnedBookings.Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.GetMatchingCriteriaTestData), MemberType = typeof(TestData))]
  public async Task GetMatchingCriteria_BookingSearchCriteria_ShouldReturnExpectedBookings(
    BookingSearchCriteria criteria, IEnumerable<Booking> expected)
  {
    var returnedBookings = await _sut.GetMatchingCriteria(criteria);

    returnedBookings.Should().BeEquivalentTo(expected);
  }
}