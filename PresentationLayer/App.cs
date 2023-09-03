using BusinessLogic.Mappers;
using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.DateTimeProvider;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogic.Services.Implementations;
using BusinessLogic.Services.Interfaces;
using DataAccess.Csv;
using DataAccess.Csv.CsvImportService;
using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Csv.Parsers;
using DataAccess.Csv.Validators;
using DataAccess.Models;
using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;
using PresentationLayer;

var serviceProvider = new ServiceCollection()
  .AddScopedServices()
  .AddScopedMappers()
  .AddScopedCsvServices()
  .AddScopedCsvParsers()
  .AddScopedCsvRepositories()
  .AddScopedImportFromCsvServices()
  .AddScoped<MainMenu>();

var app = serviceProvider
  .BuildServiceProvider()
  .GetRequiredService<MainMenu>();

await app.Run();