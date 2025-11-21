using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    public class CarRepository : ICarRepository
    {
        public static readonly List<Car> _carList = new List<Car>();

        private static int _idCounters = 1;

        public CarRepository() { }

        public Car Create(Car car)
        {
            if(car.Id != 0)
            {
                throw new InvalidOperationException("Não é possível criar um carro com um Id pré-definido. O ID é atribuído pelo repositório.");
            }

            var newCar = new Car(_idCounters++, car.CarModel!, car.CarMake!, car.TypeOfFuel!, car.CarColor!, car.EngineCapacity, car.CarTare, car.CarPrice, true);
            _carList.Add(newCar);
            return newCar;
        }

        public Car? Read(int id)
        {
            ValidatePositiveId(id, nameof(id));
            return _carList.FirstOrDefault(c => c.Id == id);
        }

        public List<Car> ReadAll()
        {
            return _carList.ToList();
        }

        public Car Update(Car car)
        {
            ValidateNotNull(car, nameof(car));
            ValidatePositiveId(car.Id, nameof(car.Id));
                        
            Car? existingCar = _carList.FirstOrDefault(c => c.Id == car.Id);
            if (existingCar != null)
            {
                throw new InvalidOperationException($"Carro com ID {car.Id} não encontrado para atualização.");
            }
        
            existingCar!.UpdateDetails(
                car.CarModel,
                car.CarMake!,
                car.TypeOfFuel!,
                car.CarColor!,
                car.EngineCapacity,
                car.CarTare,
                car.CarPrice
            );
            existingCar.SetAvailability(car.IsAvailable);                
            
            return existingCar;
        }

        public bool Delete(int id)
        {
            ValidatePositiveId(id, nameof(id));

            Car? carToRemove = _carList.FirstOrDefault(c => c.Id == id);

            return carToRemove != null && _carList.Remove(carToRemove);
        }

        public List<Car> GetAvailableCars()
        {
            return _carList.Where(car => car.IsAvailable).ToList();
        }

        public void MarkCarAsUnavailable(int carId)
        {
            MarkCarAvailability(carId, false);
        }

        public void MarkCarAsAvailable(int carId)
        {
            MarkCarAvailability(carId, true);
        }

        private void MarkCarAvailability(int carId, bool isAvailable)
        {
            ValidatePositiveId(carId, nameof(carId));
            Car? carToUpdate = _carList.FirstOrDefault(c => c.Id == carId);
            if(carToUpdate == null)
            {
                throw new InvalidOperationException($"Carro com ID {carId} não encontrado para alterar a disponibilidade.");
            }
            carToUpdate.SetAvailability(isAvailable);
        }

        private void ValidatePositiveId(int id, string paramName)
        {
            if(id <= 0)
            {
                throw new ArgumentException($"O ID para a operação deve ser um número positivo. ID fornecido: {id}", paramName);
            }
        }

        private void ValidateNotNull<T>(T obj, string paramName) where T : class
        {
            if(obj == null)
            {
                throw new ArgumentNullException(paramName, $"O objeto {paramName} não pode ser nulo.");
            }
        }        
        
        public static void SeedData()
        {
            if (!_carList.Any())
            {
                var tempRepo = new CarRepository();

                Car car1 = new Car("Toyota", "Corolla", "Gasolina", "Preto", 1.6, 1200, 25000.00);
                Car car2 = new Car("Honda", "Civic", "Híbrido", "Branco", 1.5, 1300, 30000.00);
                Car car3 = new Car("Ford", "Mustang", "Gasolina", "Vermelho", 5.0, 1600, 45000.00);

                tempRepo.Create(car1);
                tempRepo.Create(car2);
                tempRepo.Create(car3);

                _carList.FirstOrDefault(c => c.CarModel == "Mustang")?.SetAvailability(false);
            }
        }
    }
}
