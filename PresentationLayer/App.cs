using BusinessLogic.Services.Implementations;
using DataAccessLayer.Csv.CsvImportService;
using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Csv.Mappers;
using DataAccessLayer.Csv.Parsers;
using DataAccessLayer.Csv.Validators;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Implementations;
using PresentationLayer;

var validator = new Validator();
var passengerParser = new PassengerCsvImportDtoParser();
var passengerMapper = new PassengerCsvImportDtoToPassengerMapper();
var passengerCsvImportService =
  new ImportFromCsvService<PassengerCsvImportDto, Passenger>(passengerParser, validator, passengerMapper);

var passengerRepository = new CsvPassengerRepository(passengerCsvImportService);

var flightParser = new FlightCsvImportDtoParser();
var flightMapper = new FlightCsvImportDtoToFlightMapper();
var flightCsvImportService =
  new ImportFromCsvService<FlightCsvImportDto, Flight>(flightParser, validator, flightMapper);

var flightRepository = new CsvFlightRepository(flightCsvImportService);

var bookingParser = new BookingCsvImportDtoParser();
var bookingMapper = new BookingCsvImportDtoToBookingMapper(flightRepository, passengerRepository);
var bookingCsvImportService =
  new ImportFromCsvService<BookingCsvImportDto, Booking>(bookingParser, validator, bookingMapper);

var bookingRepository = new CsvBookingRepository(bookingCsvImportService);

var passengerService = new PassengerService(passengerRepository);

var flightService = new FlightService(flightRepository, bookingRepository, flightCsvImportService);

var bookingService = new BookingService(bookingRepository, flightRepository, passengerRepository);

var app = new MainMenu(bookingService, flightService, passengerService);

app.Run();