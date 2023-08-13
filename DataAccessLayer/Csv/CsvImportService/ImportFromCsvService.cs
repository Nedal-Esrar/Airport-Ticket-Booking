using DataAccessLayer.Csv.Mappers;
using DataAccessLayer.Csv.Parsers;
using DataAccessLayer.Csv.Validators;
using DataAccessLayer.Validation;

namespace DataAccessLayer.Csv.CsvImportService;

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

  public DataImportResult<TOutput> ImportFromCsv(string filePath)
  {
    var reader = new CsvReader<TInput>(filePath);

    var readObjects = reader.Read(_parser.Parse);

    var validObjects = new List<TOutput>();

    var validationErrors = new List<string>();

    var count = 2;

    foreach (var readObject in readObjects)
    {
      if (_validator.IsValid(readObject))
      {
        var objectToAdd = _mapper.Map(readObject);
        
        if (objectToAdd != null)
        {
          validObjects.Add(objectToAdd);
        }
      }
      else
      {
        validationErrors.Add(ValidationMessages.GenerateLineIndicatorMessage(count));
        validationErrors.AddRange(_validator.GetErrorMessages(readObject));
      }

      ++count;
    }

    return new DataImportResult<TOutput>
    {
      ValidObjects = validObjects,
      ValidationErrors = validationErrors
    };
  }
}