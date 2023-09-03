using BusinessLogic.Services.Interfaces;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.Commands.ManagerCommands;

public class ImportFlightsFromCsvCommand : ICommand
{
  private readonly IFlightService _flightService;

  public ImportFlightsFromCsvCommand(IFlightService flightService)
  {
    _flightService = flightService;
  }

  public async Task Execute()
  {
    Console.WriteLine(FileImportMessages.EnterFilePath);

    var filePath = InputParser.GetInput<string>(InputPrompts.FilePathPrompt,
      ParseFunctionsWithoutSkip.TryParseString);

    try
    {
      var errorMessages = await _flightService.ImportFromCsv(filePath);

      if (errorMessages.Any())
      {
        DisplayErrorMessages(errorMessages);
      }
      else
      {
        Console.WriteLine(FileImportMessages.SuccessImport);
      }
    }
    catch (Exception exception)
    {
      Console.WriteLine(exception.Message);
    }
  }

  private void DisplayErrorMessages(IEnumerable<string> errorMessages)
  {
    Console.WriteLine(FileImportMessages.ErrorWereFound);

    foreach (var message in errorMessages)
    {
      Console.WriteLine(message);
    }
  }
}