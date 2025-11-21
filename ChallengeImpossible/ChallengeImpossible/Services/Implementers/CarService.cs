using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Services.Implementers
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }

        public Car Create(Car car)
        {
            ValidateCarNotNull(car);           

            if(CarExists(car.CarMake!, car.CarModel!, car.EngineCapacity))
            {
                throw new ArgumentException("Já existe um carro com a mesma Marca, Modelo e Cilindrada.", nameof(car));
            }

            return _carRepository.Create(car);
        }

        public Car Read(int id)
        {
            ValidatePositiveId(id);
          
            var car = _carRepository.Read(id);
            if(car == null)
            {
                throw new ArgumentException($"Carro com ID {id} não encontrado.", nameof(id));
            }
            return car;
        }

        public List<Car> ReadAll()
        {
            return _carRepository.ReadAll();
        }

        public Car Update(Car car)
        {
            ValidateCarNotNull(car);
            ValidatePositiveId(car.Id);            

            var existingCar = _carRepository.Read(car.Id);
            if(existingCar == null)
            {
                throw new ArgumentException($"Carro com ID {car.Id} não encontrado para atualização.", nameof(car.Id));
            }

            if(CarExists(car.CarMake!, car.CarModel!, car.EngineCapacity, excludeId: car.Id))
            {
                throw new ArgumentException("A atualização resultaria num carro duplicado (mesma Marca, Modelo e Cilindrada).", nameof(car));
            }

            return _carRepository.Update(car);
        }

        public bool Delete(int id)
        {
            ValidatePositiveId(id);            

            var carToDelete = _carRepository.Read(id);
            if(carToDelete == null)
            {
                throw new InvalidOperationException($"Carro com ID {id} não encontrado para eliminar.");
            }
           return _carRepository.Delete(id);            
        }

        public List<Car> GetAvailableCars()
        {
            return _carRepository.GetAvailableCars();
        }

        public void MarkCarAsUnavailable(int carId)
        {
            ValidatePositiveId(carId);
            _carRepository.MarkCarAsUnavailable(carId);
        }

        public void MarkCarAsAvailable(int carId)
        {
            ValidatePositiveId(carId);
            _carRepository.MarkCarAsAvailable(carId);
        }

        private void ValidateCarNotNull(Car car)
        {
            if(car == null)
            {
                throw new ArgumentNullException(nameof(car), "O objecto Car não pode ser nulo.");
            }
        }

        private void ValidatePositiveId(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException($"ID deve ser número posítivo. ID fornecido: {id}", nameof(id));
            }
        }

        private bool CarExists(string make, string model, double engineCapacity, int excludeId = 0)
        {
            var allCars = _carRepository.ReadAll()
                .Where(c => c.Id != excludeId)
                .ToList();

            return allCars.Any(c =>
                c.CarMake!.Equals(make, StringComparison.OrdinalIgnoreCase) &&
                c.CarModel!.Equals(model, StringComparison.OrdinalIgnoreCase) &&
                c.EngineCapacity == engineCapacity
            );
        }
    }
}

