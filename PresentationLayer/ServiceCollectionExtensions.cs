using BusinessLogic.Mappers;
using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.DateTimeProvider;
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
using Microsoft.Extensions.DependencyInjection;

namespace PresentationLayer;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddScopedServices(this IServiceCollection services)
  {
    return services
      .AddScoped<IDateTimeProvider, DateTimeProvider>()
      .AddScoped<IPassengerService, PassengerService>()
      .AddScoped<IFlightService, FlightService>()
      .AddScoped<IBookingService, BookingService>();
  }

  public static IServiceCollection AddScopedMappers(this IServiceCollection services)
  {
    return services
      .AddScoped<IMapper<Booking, BookingDto>, BookingToBookingDtoMapper>()
      .AddScoped<IMapper<Passenger, PassengerDto>, PassengerToPassengerDtoMapper>()
      .AddScoped<IMapper<Flight, FlightDto>, FlightToFlightDtoMapper>()
      .AddScoped<IMapper<FlightClassDetails, FlightClassDetailsDto>, FlightClassDetailsToFlightClassDetailsDtoMapper>()
      .AddScoped<IMapper<PassengerCsvImportDto, Passenger>, PassengerCsvImportDtoToPassengerMapper>()
      .AddScoped<IMapper<FlightCsvImportDto, Flight>, FlightCsvImportDtoToFlightMapper>()
      .AddScoped<IMapper<BookingCsvImportDto, Booking>, BookingCsvImportDtoToBookingMapper>();
  }

  public static IServiceCollection AddScopedCsvServices(this IServiceCollection services)
  {
    return services
      .AddScoped<ICsvReader, CsvReader>()
      .AddScoped<ICsvWriter, CsvWriter>()
      .AddScoped<IValidator, Validator>();
  }

  public static IServiceCollection AddScopedCsvParsers(this IServiceCollection services)
  {
    return services
      .AddScoped<IParser<PassengerCsvImportDto>, PassengerCsvImportDtoParser>()
      .AddScoped<IParser<FlightCsvImportDto>, FlightCsvImportDtoParser>()
      .AddScoped<IParser<BookingCsvImportDto>, BookingCsvImportDtoParser>();
  }

  public static IServiceCollection AddScopedCsvRepositories(this IServiceCollection services)
  {
    return services
      .AddScoped<IPassengerRepository, CsvPassengerRepository>()
      .AddScoped<IFlightRepository, CsvFlightRepository>()
      .AddScoped<IBookingRepository, CsvBookingRepository>();
  }

  public static IServiceCollection AddScopedImportFromCsvServices(this IServiceCollection services)
  {
    return services
      .AddScoped<IImportFromCsvService<FlightCsvImportDto, Flight>, ImportFromCsvService<FlightCsvImportDto, Flight>>()
      .AddScoped<IImportFromCsvService<BookingCsvImportDto, Booking>,
        ImportFromCsvService<BookingCsvImportDto, Booking>>()
      .AddScoped<IImportFromCsvService<PassengerCsvImportDto, Passenger>,
        ImportFromCsvService<PassengerCsvImportDto, Passenger>>();
  }
}