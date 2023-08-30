using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IPassengerRepository
{
  Passenger? GetById(int id);
}