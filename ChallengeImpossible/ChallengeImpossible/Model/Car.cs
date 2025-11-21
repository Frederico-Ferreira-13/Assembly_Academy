using System;

namespace ChallengeImpossible.Model
{
    public class Car
    {
        public int Id { get; private set; }
        public string? CarModel { get; private set; }
        public string? CarMake { get; private set; }
        public string? TypeOfFuel { get; private set; }
        public string? CarColor { get; private set; }
        public double EngineCapacity { get; private set; }
        public double CarTare { get; private set; }
        public double CarPrice { get; private set; }
        public bool IsAvailable { get; private set; }

        public Car(string carModel, string carMake, string typeOfFuel, string carColor, double engineCapacity,
            double carTare, double carPrice)
            : this(0, carModel, carMake, typeOfFuel, carColor, engineCapacity, carTare, carPrice, true)
        {
        }
        
        public Car(int id, string carModel, string carMake, string typeOfFuel, string carColor, double engineCapacity,
             double carTare, double carPrice, bool isAvailable)
        {
            if (id < 0)
            {
                throw new ArgumentException("O Id do carro deve ser um número positivo.", nameof(id));
            }

            ValidateCarDetails(carModel, carMake, typeOfFuel, carColor, engineCapacity, carTare, carPrice);

            Id = id;
            CarModel = carModel;
            CarMake = carMake;
            TypeOfFuel = typeOfFuel;
            CarColor = carColor;
            EngineCapacity = engineCapacity;
            CarTare = carTare;
            CarPrice = carPrice;
            IsAvailable = isAvailable;
        }        

        private void ValidateCarDetails(string? carModel, string carMake, string typeOfFuel, string carColor, double engineCapacity,
            double carTare, double carPrice)
        {
            if (string.IsNullOrWhiteSpace(carMake))
            {
                throw new ArgumentException("A marca do carro é obrigatória.", nameof(carMake));
            }
            if (string.IsNullOrWhiteSpace(carModel))
            {
                throw new ArgumentException("O modelo do carro é obrigatório.", nameof(carModel));
            }
            if (string.IsNullOrWhiteSpace(typeOfFuel))
            {
                throw new ArgumentException("O tipo de combustível é obrigatório.", nameof(typeOfFuel));
            }
            if (string.IsNullOrWhiteSpace(carColor))
            {
                throw new ArgumentException("A cor do carro é obrigatória.", nameof(carColor));
            }
            if (engineCapacity <= 0)
            {
                throw new ArgumentException("A cilindrada deve ser um valor positivo.", nameof(engineCapacity));
            }
            if (carTare <= 0)
            {
                throw new ArgumentException("A tara do carro deve ser um valor positivo.", nameof(carTare));
            }
            if (carPrice <= 0)
            {
                throw new ArgumentException("O preço do carro não pode ser negativo ou zero.", nameof(carPrice));
            }
        }

        public void SetAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;
        }

        public void MarkAsAvailable()
        {
            SetAvailability(true);
        }

        public void MarkAsUnavailable()
        {
            SetAvailability(false);
        }

        public void UpdateDetails(string? newCarModel, string newCarMake, string newTypeOfFuel, string newCarColor, double newEngineCapacity,
            double newCarTare, double newCarPrice)
        {
            ValidateCarDetails(newCarModel, newCarMake, newTypeOfFuel, newCarColor, newEngineCapacity, newCarTare, newCarPrice);

            CarModel = newCarModel;
            CarMake = newCarMake;
            TypeOfFuel = newTypeOfFuel;
            CarColor = newCarColor;
            EngineCapacity = newEngineCapacity;
            CarTare = newCarTare;
            CarPrice = newCarPrice;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Marca: {CarMake}, Modelo: {CarModel}, Cor: {CarColor}, Combustível: {TypeOfFuel}, Cilindrada: {EngineCapacity:F1}L, Tara: {CarTare:F1}Kg, Preço: {CarPrice:C2}, Disponível: {(IsAvailable ? "Sim" : "Não")}";
        }       
    }
}