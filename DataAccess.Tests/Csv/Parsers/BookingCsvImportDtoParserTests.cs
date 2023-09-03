using DataAccess.Csv.Dtos;
using DataAccess.Csv.Parsers;
using FluentAssertions.Execution;

namespace DataAccess.Tests.Csv.Parsers;

public class BookingCsvImportDtoParserTests
{
  private readonly BookingCsvImportDtoParser _sut = new();

  [Theory]
  [InlineData(1)]
  [InlineData(3)]
  [InlineData(10)]
  public void Parse_NoOfAttributesDoesNotMatchTheRequired_ShouldReturnNull(int noOfAttributes)
  {
    var fixture = new Fixture();

    var attributes = fixture.CreateMany<string>(noOfAttributes).ToArray();

    var returnedBooking = _sut.Parse(attributes);

    returnedBooking.Should().BeNull();
  }

  [Theory]
  [MemberData(nameof(TestData.BookingsToParse), MemberType = typeof(TestData))]
  public void Parse_NoOfAttributesMatchTheRequired_ShouldAssignNullToTheInvalidDataAndAssignValidData(string[] attributes, 
    BookingCsvImportDto expected)
  {
    var returnedBooking = _sut.Parse(attributes);
    
    returnedBooking.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(expected);
  }
}