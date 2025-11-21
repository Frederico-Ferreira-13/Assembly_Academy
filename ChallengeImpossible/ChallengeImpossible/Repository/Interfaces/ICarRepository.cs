using System.Collections.Generic;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Repository.Interfaces
{
    public interface ICarRepository
    {
        Car Create(Car car);
        Car? Read(int id);
        List<Car> ReadAll();
        Car Update(Car car);
        bool Delete(int id);

        List<Car> GetAvailableCars();
        void MarkCarAsUnavailable(int carId);
        void MarkCarAsAvailable(int carId);
    }
}
