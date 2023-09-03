using Microsoft.Extensions.DependencyInjection;
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