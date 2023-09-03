using DataAccess.Models;

namespace DataAccess.Tests.Repositories;

public class TestsData
{
  public static readonly List<Passenger> Passengers = new()
  {
    new() { Id = 1 },
    new() { Id = 1 },
    new() { Id = 2 }
  };
}