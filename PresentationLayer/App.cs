using Microsoft.Extensions.DependencyInjection;
using BusinessLogic.Services.Implementations;
using BusinessLogic.Services.Interfaces;
using DataAccessLayer.Csv.CsvImportService;
using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Csv.Mappers;
using DataAccessLayer.Csv.Parsers;
using DataAccessLayer.Csv.Validators;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations;
using DataAccessLayer.Repositories.Interfaces;
using PresentationLayer;

var serviceProvider = new ServiceCollection()
  .AddScoped<IValidator, Validator>()
  .AddScoped<IParser<PassengerCsvImportDto>, PassengerCsvImportDtoParser>()
  .AddScoped<IMapper<PassengerCsvImportDto, Passenger>, PassengerCsvImportDtoToPassengerMapper>()
  .AddScoped<IImportFromCsvService<PassengerCsvImportDto, Passenger>,
    ImportFromCsvService<PassengerCsvImportDto, Passenger>>()
  .AddScoped<IParser<FlightCsvImportDto>, FlightCsvImportDtoParser>()
  .AddScoped<IMapper<FlightCsvImportDto, Flight>, FlightCsvImportDtoToFlightMapper>()
  .AddScoped<IImportFromCsvService<FlightCsvImportDto, Flight>, ImportFromCsvService<FlightCsvImportDto, Flight>>()
  .AddScoped<IParser<BookingCsvImportDto>, BookingCsvImportDtoParser>()
  .AddScoped<IMapper<BookingCsvImportDto, Booking>, BookingCsvImportDtoToBookingMapper>()
  .AddScoped<IImportFromCsvService<BookingCsvImportDto, Booking>, ImportFromCsvService<BookingCsvImportDto, Booking>>()
  .AddScoped<IPassengerRepository, CsvPassengerRepository>()
  .AddScoped<IFlightRepository, CsvFlightRepository>()
  .AddScoped<IBookingRepository, CsvBookingRepository>()
  .AddScoped<IPassengerService, PassengerService>()
  .AddScoped<IFlightService, FlightService>()
  .AddScoped<IBookingService, BookingService>()
  .AddScoped<MainMenu>();

var app = serviceProvider
  .BuildServiceProvider()
  .GetRequiredService<MainMenu>();

app.Run();