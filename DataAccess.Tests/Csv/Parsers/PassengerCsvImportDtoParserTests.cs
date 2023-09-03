using DataAccess.Csv.Dtos;
using DataAccess.Csv.Parsers;
using FluentAssertions.Execution;

namespace DataAccess.Tests.Csv.Parsers;

public class PassengerCsvImportDtoParserTests
{
  private readonly PassengerCsvImportDtoParser _sut = new();

  [Theory]
  [InlineData(1)]
  [InlineData(3)]
  [InlineData(10)]
  public void Parse_NoOfAttributesDoesNotMatchTheRequired_ShouldReturnNull(int noOfAttributes)
  {
    var fixture = new Fixture();

    var attributes = fixture.CreateMany<string>(noOfAttributes).ToArray();

    var returnedPassenger = _sut.Parse(attributes);

    returnedPassenger.Should().BeNull();
  }

  [Theory]
  [MemberData(nameof(TestData.PassengersToParse), MemberType = typeof(TestData))]
  public void Parse_NoOfAttributesMatchTheRequired_ShouldAssignNullToTheInvalidDataAndAssignValidData(string[] attributes, PassengerCsvImportDto expected)
  {
    var returnedPassenger = _sut.Parse(attributes);
    
    returnedPassenger.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(expected);
  }
}