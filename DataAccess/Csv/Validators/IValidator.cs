namespace DataAccess.Csv.Validators;

public interface IValidator
{
  bool IsValid(object? obj);
  
  IList<string> GetErrorMessages(object? obj);
}