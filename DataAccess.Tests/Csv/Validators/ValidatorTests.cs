using DataAccess.Csv.Dtos;
using DataAccess.Csv.Validators;

namespace DataAccess.Tests.Csv.Validators;

public class ValidatorTests
{
  private readonly Validator _sut = new();
  
  [Theory]
  [MemberData(nameof(TestData.InvalidObjects), MemberType = typeof(TestData))]
  public void IsValid_InvalidObjects_ShouldReturnFalse(object? obj)
  {
    var returnedResult = _sut.IsValid(obj);

    returnedResult.Should().BeFalse();
  }
  
  [Theory]
  [MemberData(nameof(TestData.ValidObjects), MemberType = typeof(TestData))]
  public void IsValid_ValidObjects_ShouldReturnTrue(object? obj)
  {
    var returnedResult = _sut.IsValid(obj);

    returnedResult.Should().BeTrue();
  }

  [Theory]
  [MemberData(nameof(TestData.ObjectsWithErrorMessages), MemberType = typeof(TestData))]
  public void GetErrorMessages_Objects_ShouldReturnExpectedListOfErrors(object? obj,
    List<string> expectedErrorMessages)
  {
    var returnedErrorMessages = _sut.GetErrorMessages(obj);

    returnedErrorMessages.Should().BeEquivalentTo(expectedErrorMessages);
  }
}