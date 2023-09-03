using DataAccess.Validation;

namespace DataAccess.Exceptions;

public class NotACsvFileException : Exception
{
  public NotACsvFileException(string filePath) : base(ValidationMessages.GenerateNotACsvFileMessage(filePath))
  {
  }
}