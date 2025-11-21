using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    internal class SalesCarRepository : ISalesCarRepository
    {
        public List<SalesCar> _salesCarList = new List<SalesCar>();

        public int _idCounters = 1;

        private readonly ICarRepository _carRepository;

        public SalesCarRepository(ICarRepository carRepository)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
        }         

        public SalesCar Create(SalesCar salesCar)
        {
            if(salesCar == null)
            {
                throw new ArgumentNullException(nameof(salesCar), "A venda de carro não pode ser nulo.");
            }
            if (salesCar.Id != 0)
            {
                throw new InvalidOperationException("Não é possível criar uma venda com um ID pré-definido. O ID é atribuído pelo repositório.");
            }        

            salesCar.SetId(_idCounters++);          

            _salesCarList.Add(salesCar);
            return salesCar;
        }

        public SalesCar? Read(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.", nameof(id));
            }
            return _salesCarList.FirstOrDefault(sc => sc.Id == id);
        }

        public List<SalesCar> ReadAll()
        {
            return _salesCarList.ToList();
        }

        public SalesCar Update(SalesCar salesCar)
        {
            if(salesCar == null)
            {
                throw new ArgumentNullException(nameof(salesCar), "Venda de Carro não pode ser nulo.");
            }
            if(salesCar.Id <= 0)
            {
                throw new ArgumentException("O ID da Venda de Carro tem que ser um número inteiro positivo.", nameof(salesCar.Id));
            }

            SalesCar? existingSalesCar = _salesCarList.FirstOrDefault(cs => cs.Id == salesCar.Id);
            if(existingSalesCar == null)
            {
                throw new InvalidOperationException($"Venda de carro com ID {salesCar.Id} não encontrada para atualização.");
            }

            existingSalesCar.UpdateSalesCar(salesCar.Price, salesCar.PurchaseDate, salesCar.Customer, salesCar.Cars);

            return existingSalesCar;
        }

        public bool Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("ID must be a positive integer.", nameof(id));
            }

            SalesCar? salesCarToRemove = _salesCarList.FirstOrDefault(sc => sc.Id == id);
            if (salesCarToRemove != null)
            {               
                _salesCarList.Remove(salesCarToRemove);
                return true;
            }
            return false;
        }        
    }
}
