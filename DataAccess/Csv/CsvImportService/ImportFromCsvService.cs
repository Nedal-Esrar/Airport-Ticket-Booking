using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Csv.Parsers;
using DataAccess.Csv.Validators;
using DataAccess.Validation;

namespace DataAccess.Csv.CsvImportService;

public class ImportFromCsvService<TInput, TOutput> : IImportFromCsvService<TInput, TOutput>
{
  private readonly IMapper<TInput, TOutput> _mapper;
  
  private readonly IParser<TInput> _parser;

  private readonly ICsvReader _reader;

  private readonly IValidator _validator;

  public ImportFromCsvService(IParser<TInput> parser, IValidator validator, IMapper<TInput, TOutput> mapper,
    ICsvReader reader)
  {
    _parser = parser;
    
    _validator = validator;
    
    _mapper = mapper;
    
    _reader = reader;
  }

  public async Task<DataImportResult<TOutput>> ImportFromCsv(string filePath)
  {
    var readObjects = await _reader.ReadAsync(filePath, _parser);

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