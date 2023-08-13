using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IPassengerRepository
{
  Passenger? GetById(int id);
}