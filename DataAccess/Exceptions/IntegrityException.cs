using DataAccess.Validation;

namespace DataAccess.Exceptions;

public class IntegrityException : Exception
{
  public IntegrityException() : base(ValidationMessages.ObjectIntegrityError)
  {
  }
}