using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Services.Implementers
{
    public class SalesCarService : ISalesCarService
    {
        private readonly ISalesCarRepository _salesCarRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICarRepository _carRepository;

        public SalesCarService(ISalesCarRepository salesCarRepository, IClientRepository clientRepository, ICarRepository carRepository)
        {
            _salesCarRepository = salesCarRepository ?? throw new ArgumentNullException(nameof(salesCarRepository));
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }

        private void ValidateSalesCarBasic(SalesCar salesCar)
        {
            if(salesCar == null)
            {
                throw new ArgumentNullException(nameof(salesCar), "A venda do carro não pode ser nula.");
            }
        }

        private Client ValidateAndGetClient(Client client)
        {
            if(client == null || client.Id <= 0)
            {
                throw new ArgumentException("Dados do cliente inválidos ou ausentes na venda.", nameof(client));
            }

            var clientInDb = _clientRepository.Read(client.Id);
            if(clientInDb == null)
            {
                throw new InvalidOperationException($"O cliente com ID {client.Id} não existe.");
            }
            return clientInDb;
        }

        private List<Car> ValidateAndGetCarsForOperation(List<Car> cars)
        {
            if(cars == null || !cars.Any())
            {
                throw new ArgumentException("Uma venda deve conter pelo menos um carro.", nameof(cars));
            }

            var verifiedCars = new List<Car>();
            foreach(var carInSalesCar in cars)
            {
                if(carInSalesCar.Id <= 0)
                {
                    throw new ArgumentException($"O ID do carro ({carInSalesCar.Id}) na venda é inválido", nameof(carInSalesCar));
                }

                var carInInventory = _carRepository.Read(carInSalesCar.Id);
                if(carInInventory == null)
                {
                    throw new InvalidOperationException($"O carro com ID {carInSalesCar.Id} não existe no inventário.");
                }
                verifiedCars.Add(carInInventory);
            }
            return verifiedCars;
        }

        public SalesCar Create(SalesCar salesCar)
        {
            ValidateSalesCarBasic(salesCar);

            var validatedCustomer = ValidateAndGetClient(salesCar.Customer);
            var validatedCars = ValidateAndGetCarsForOperation(salesCar.Cars);

            var newSalesCar = new SalesCar(
                salesCar.Price,
                salesCar.PurchaseDate,
                validatedCustomer,
                validatedCars
            );

            if(newSalesCar.Price < newSalesCar.TotalCost)
            {
                throw new ArgumentException($"O preço de venda ({newSalesCar.Price:C2}) não pode ser inferio ao custo total dos carros ({newSalesCar.TotalCost:C2}).");
            }                   

            foreach (var car in newSalesCar.Cars)
            {
                if(!car.IsAvailable)
                {
                    throw new InvalidOperationException($"Carro '{car.CarMake} {car.CarModel}' (ID: {car.Id}) não está disponível para venda.");
                }
                _carRepository.MarkCarAsUnavailable(car.Id);
            }
            
            return _salesCarRepository.Create(newSalesCar);
        }

        public SalesCar Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("ID de venda de carro inválido. Deve ser um número inteiro positivo.", nameof(id));
            }

            var salesCar = _salesCarRepository.Read(id);
            if(salesCar == null)
            {
                throw new InvalidOperationException($"Venda de carro com ID {id} não encontrada.");
            }

            return salesCar;
        }

        public List<SalesCar> ReadAll()
        {
            return _salesCarRepository.ReadAll();
        }

        public SalesCar Update(SalesCar salesCar)
        {
            ValidateSalesCarBasic(salesCar);
           
            if(salesCar.Id <= 0)
            {
                throw new ArgumentException("O ID da venda de carro a atualizar é inválido.", nameof(salesCar.Id));
            }

            SalesCar? existingSalesCar = _salesCarRepository.Read(salesCar.Id);
            if(existingSalesCar == null)
            {
                throw new ArgumentException($"Venda de carro com ID {salesCar.Id} não encontrada para atualização.");
            }

            var validatedCustomer = ValidateAndGetClient(salesCar.Customer);
            var validatedCarsForUpdate = ValidateAndGetCarsForOperation(salesCar.Cars);

            var carsRemoved = existingSalesCar.Cars
                .Where(oldCar => !validatedCarsForUpdate.Any(newCar => newCar.Id == oldCar.Id))
                .ToList();
            foreach (var car in carsRemoved)
            {
                _carRepository.MarkCarAsAvailable(car.Id);
            }

            var newCarsToAdd = validatedCarsForUpdate
                .Where(newCar => !existingSalesCar.Cars.Any(oldCar => oldCar.Id == newCar.Id))
                .ToList();

            var successfullyMarkedUnavailable = new List<Car>();

            try
            {
                foreach(var newCar in newCarsToAdd)
                {
                    var carInInvetory = _carRepository.Read(newCar.Id);
                    if(carInInvetory == null || !carInInvetory.IsAvailable)
                    {
                        throw new InvalidOperationException($"Carro {newCar.CarMake} {newCar.CarModel} (ID: {newCar.Id}) não está disponível para ser adicionado a esta venda.");
                    }
                    _carRepository.MarkCarAsUnavailable(newCar.Id);
                    successfullyMarkedUnavailable.Add(newCar);
                }

                existingSalesCar.UpdateSalesCar(
                    salesCar.Price,
                    salesCar.PurchaseDate,
                    validatedCustomer,
                    validatedCarsForUpdate
                );

                if(existingSalesCar.Price < existingSalesCar.TotalCost)
                {
                    throw new ArgumentException($"O preço de venda ({existingSalesCar.Price:C2}) não pode ser inferior ao custo total dos carros ({existingSalesCar.TotalCost:C2}).");
                }
                return _salesCarRepository.Update(existingSalesCar);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro inesperado durante a operação de venda: {ex.Message}");
                Console.WriteLine($"Detalhes do erro: {ex.StackTrace}");

                foreach (var car in successfullyMarkedUnavailable)
                {
                    _carRepository.MarkCarAsAvailable(car.Id);
                }

                foreach(var car in carsRemoved)
                {
                    _carRepository.MarkCarAsUnavailable(car.Id);
                }
                throw;
            }            
        }

        public bool Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("ID de venda de carro inválido para remover.", nameof(id));
            }
            
            SalesCar? salesCarToRemove = _salesCarRepository.Read(id);
            if(salesCarToRemove == null)
            {
                return false;
            }

            foreach (var car in salesCarToRemove.Cars)
            {
                _carRepository.MarkCarAsAvailable(car.Id);
            }

            return _salesCarRepository.Delete(id);           
        }

        public List<Car> GetAvailableCars()
        {
            return _carRepository.GetAvailableCars();
        }
    }
}
