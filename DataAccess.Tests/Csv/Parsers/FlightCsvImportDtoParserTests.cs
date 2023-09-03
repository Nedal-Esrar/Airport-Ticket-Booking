using DataAccess.Csv.Dtos;
using DataAccess.Csv.Parsers;
using FluentAssertions.Execution;

namespace DataAccess.Tests.Csv.Parsers;

public class FlightCsvImportDtoParserTests
{
  private readonly FlightCsvImportDtoParser _sut = new();

  private readonly Fixture _fixture = new();

  [Theory]
  [InlineData(1)]
  [InlineData(3)]
  [InlineData(20)]
  public void Parse_NoOfAttributesDoesNotMatchTheRequired_ShouldReturnNull(int noOfAttributes)
  {
    var attributes = _fixture.CreateMany<string>(noOfAttributes).ToArray();

    var returnedFlight = _sut.Parse(attributes);

    returnedFlight.Should().BeNull();
  }

  [Fact]
  public void Parse_NoOfDetailsAttributesIsNotIdentical_ShouldReturnNull()
  {
    var attributes = _fixture.CreateMany<string>(7).ToList();

    attributes.AddRange(new[] {"1;2", "1;3;4", "1"});
    
    var returnedFlight = _sut.Parse(attributes.ToArray());

    returnedFlight.Should().BeNull();
  }

  [Theory]
  [MemberData(nameof(TestData.FlightsToParse), MemberType = typeof(TestData))]
  public void Parse_NoOfAttributesMatchTheRequired_ShouldAssignNullToTheInvalidDataAndAssignValidData(string[] attributes, 
    FlightCsvImportDto expected)
  {
    var returnedFlight = _sut.Parse(attributes);
    
    returnedFlight.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(expected);
  }
}