using DataAccess.Csv.Mappers;
using DataAccess.Csv.Parsers;
using DataAccess.Csv.Validators;
using DataAccess.Validation;

namespace DataAccess.Csv.CsvImportService;

public class ImportFromCsvService<TInput, TOutput> : IImportFromCsvService<TInput, TOutput>
{
  private readonly IParser<TInput> _parser;

  private readonly IValidator _validator;

  private readonly IMapper<TInput, TOutput> _mapper;

  public ImportFromCsvService(IParser<TInput> parser, IValidator validator, IMapper<TInput, TOutput> mapper)
  {
    _parser = parser;
    _validator = validator;
    _mapper = mapper;
  }

  public async Task<DataImportResult<TOutput>> ImportFromCsv(string filePath)
  {
    var reader = new CsvReader<TInput>(filePath);

    var readObjects = await reader.ReadAsync(_parser.Parse);

    var validObjects = new List<TOutput>();

    var validationErrors = new List<string>();

    var currentLine = 2;

    foreach (var readObject in readObjects)
    {
      if (_validator.IsValid(readObject))
      {
        try
        {
          var objectToAdd = _mapper.Map(readObject);
          
          validObjects.Add(objectToAdd);
        }
        catch (Exception exception)
        {
          validationErrors.Add(ValidationMessages.GenerateLineIndicatorMessage(currentLine));
          validationErrors.Add(exception.Message);
        }
      }
      else
      {
        validationErrors.Add(ValidationMessages.GenerateLineIndicatorMessage(currentLine));
        validationErrors.AddRange(_validator.GetErrorMessages(readObject));
      }

      ++currentLine;
    }

    return new DataImportResult<TOutput>
    {
      ValidObjects = validObjects,
      ValidationErrors = validationErrors
    };
  }
}