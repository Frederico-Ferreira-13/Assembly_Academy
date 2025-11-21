using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeImpossible.Model
{
    public class SalesCar
    {
        public int Id { get; private set; }
        public double Price { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public Client Customer { get; private set; }
        public List<Car> Cars { get; private set; }

        public double TotalCost
        {
            get
            {
                return Cars?.Sum(car => car.CarPrice) ?? 0;
            }
        }

        public SalesCar(double price, DateTime purchaseDate, Client customer, List<Car> cars)
            : this(0, price, purchaseDate, customer, cars)
        {           
        }

        public SalesCar(int id, double price, DateTime purchaseDate, Client customer, List<Car> cars) 
        {           
            ValidateSalesCarData(price, purchaseDate, customer, cars);
            SetId(id);
            Price = price;
            PurchaseDate = purchaseDate;
            Customer = customer;
            SetCarsInternal(cars);
        }      

        private void ValidateSalesCarData(double price, DateTime purchaseDate, Client customer, List<Car> cars)
        {
            if (price <= 0)
            {
                throw new ArgumentException("O preço da venda deve ser superior a zero.", nameof(price));
            }
            if (purchaseDate > DateTime.Now)
            {
                throw new ArgumentException("A data da compra não pode ser no futuro.", nameof(purchaseDate));
            }
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer), "O cliente da venda é obrigatório.");
            }            
            if (cars == null || !cars.Any())
            {
                throw new ArgumentException("A venda deve conter pelo menos um carro.", nameof(cars));
            }
            if (cars.Select(c => c.Id).Distinct().Count() != cars.Count)
            {
                throw new ArgumentException("A venda não pode conter carros duplicados.", nameof(cars));
            }
        }

        internal void SetId(int id)
        {
            if(Id != 0 && Id != id)
            {
                throw new InvalidOperationException("O ID da venda já foi definido e não pode ser alterado.");
            }
            if(id <= 0)
            {
                throw new ArgumentException("O ID deve ser um número inteiro positivo.");
            }
            Id = id;
        }

        private void SetCarsInternal(List<Car> cars)
        {
            Cars = new List<Car>(cars);
        }

        public void UpdateSalesCar(double newPrice, DateTime newPurchaseDate, Client newCustomer, List<Car> newCars)
        {
            ValidateSalesCarData(newPrice, newPurchaseDate, newCustomer, newCars);
            Price = newPrice;
            PurchaseDate = newPurchaseDate;
            Customer = newCustomer;
            Cars = new List<Car>(newCars);
        }

        public string GetPurchaseSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"--- Detalhes da compra ID: {Id} ---");

            if(Customer != null)
            {
                string clientSummary = Customer.GetClientSummary();
                string[] lines = clientSummary.Split(new[] { '\n' }, StringSplitOptions.None);
                sb.AppendLine($"Comprado por: {lines[0]}");

                for(int i = 1; i < lines.Length; i++)
                {
                    sb.AppendLine($" {lines[i]}");
                }
            }
            else
            {
                sb.AppendLine("Comprador: Desconhecido");
            }

            sb.AppendLine($"Data da Compra: {PurchaseDate.ToShortDateString()}");
            sb.AppendLine("Carro(s) Comprado(s): ");
            if(Cars != null && Cars.Any())
            {
                foreach(Car car in Cars)
                {
                    sb.AppendLine($" - {car.ToString()}");
                }
            }
            else
            {
                sb.AppendLine("Nenhum carro listado para esta compra.");
            }

            sb.AppendLine($"Custo total: {TotalCost:C2}");
            sb.AppendLine($"Preço de venda: {Price:C2}");
            return sb.ToString();
        }
    
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"--- Venda de Carro ID: {Id} ---");
            sb.AppendLine($"Preço de venda: {Price:C2}");
            sb.AppendLine($"Data da Compra: {PurchaseDate.ToShortDateString()}");
            sb.AppendLine($"Cliente: {(Customer != null ? Customer.GetClientSummary() : "Desconhecido")}");
            sb.AppendLine("Carros Incluidos: ");
            if (Cars != null && Cars.Any())
            {
                foreach (Car car in Cars)
                {
                    sb.AppendLine($" - {car.ToString()} (Preço de Custo: {car.CarPrice:C2})");
                }
            }
            else 
            {
                sb.AppendLine(" -  Nenhum carro listado.");
            }
            sb.AppendLine($"Custo Total dos Carros: {TotalCost:C2}");
            return sb.ToString();
        }
    }
}
