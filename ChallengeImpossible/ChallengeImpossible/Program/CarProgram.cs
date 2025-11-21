using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using ChallengeImpossible.Model;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Program
{
    public class CarProgram
    {
        private readonly ICarService _carService;

        public CarProgram(ICarService carService)
        {
            _carService = carService;
        }

        public void CarManagement()
        {
            int carOption;

            do
            {
                Console.WriteLine("\n--- Gestão de Carros ---");
                Console.WriteLine(" 1 - Adicionar Carro");
                Console.WriteLine(" 2 - Obter Carro por ID");
                Console.WriteLine(" 3 - Obter todos os Carros");
                Console.WriteLine(" 4 - Atualizar Carro");
                Console.WriteLine(" 5 - Remover Carro");
                Console.WriteLine(" 0 - Voltar ao menu principal");
                Console.Write("Opção: ");

                if (int.TryParse(Console.ReadLine(), out carOption))
                {
                    switch (carOption)
                    {
                        case 1:
                            CreateCar();
                            break;
                        case 2:
                            ReadCar();
                            break;
                        case 3:
                            ReadAllCars();
                            break;
                        case 4:
                            UpdateCar();
                            break;
                        case 5:
                            DeleteCar();
                            break;
                        case 0:
                            Console.WriteLine("Voltar ao menu principal.");
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("Por favor, insira um número inteiro como opção.");
                    carOption = -1;
                }
            } while (carOption != 0);
        }
        public void CreateCar()
        {
            Console.WriteLine("\n--- Adicionar Carro ---");
            Console.Write("Marca: ");
            string make = Wrapper.ReadLineSafe();
            Console.Write("Modelo: ");
            string model = Wrapper.ReadLineSafe();
            Console.Write("Tipo de Combustível: ");
            string typeOfFuel = Wrapper.ReadLineSafe();
            Console.Write("Cor: ");
            string color = Wrapper.ReadLineSafe();

            double engineCapacity, carTare, carPrice;

            Console.Write("Cilindrada (L): ");
            while(!double.TryParse(Console.ReadLine(), out engineCapacity) || engineCapacity <= 0)
            {
                Console.WriteLine("Cilindrada inválida. Por favor, insira um número positivo.");
                Console.Write("Cilindrada (L): ");
            }

            Console.Write("Tara (Kg): ");
            while(!double.TryParse(Console.ReadLine(), out carTare) || carTare <= 0)
            {
                Console.WriteLine("Tara inválida. Por favor insira um número positivo.");
                Console.Write("Tara (Kg): ");
            }

            Console.Write("Preço (€): ");
            while(!double.TryParse(Console.ReadLine(), out carPrice) || carPrice < 0)
            {
                Console.WriteLine("Preço inválido. Por favor, insira um número positivo.");
                Console.Write("Preço (€): ");
            }

            try
            {
                Car newCar = new Car(model, make, typeOfFuel, color, engineCapacity, carTare, carPrice);
                Car createCar = _carService.Create(newCar);
                Console.WriteLine($"Carro adicionado com sucesso! (ID: {createCar.Id})");
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine($"Erro ao adicionar carro: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }            
        }

        public void ReadCar()
        {
            Console.WriteLine("\n--- Obter Carro por ID ---");
            Console.Write("ID do Carro: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    Car car = _carService.Read(id);                    
                    
                    Console.WriteLine($"\nDetalhes do Carro (ID: {car.Id}):");
                    Console.WriteLine(car.ToString());                  
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao procurar carro: {ex.Message}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
                }                
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        public void ReadAllCars()
        {
            Console.WriteLine("\n--- Listar todos os Carros ---");
            List<Car> cars = _carService.ReadAll();
            if (cars != null && cars.Any())
            {
                foreach (Car car in cars)
                {
                    Console.WriteLine($"\n --- Carro ID: {car.Id} ---");
                    Console.WriteLine(car.ToString());
                }
            }
            else
            {
                Console.WriteLine("Não há carros registados.");
            }
        }

        public void UpdateCar()
        {
            Console.WriteLine("\n--- Atualizar Carro ---");
            Console.Write("ID do Carro a Atualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
                return;
            }
            
            try
            {
                Car? existingCar = null;

                if (existingCar == null)
                {
                    Console.WriteLine($"Erro: Carro com o ID {id} não encontrado para atualização.");
                    return;
                }

                Console.WriteLine($"\nAtualizando Carro ID; {existingCar.Id}");
                Console.WriteLine("Pressione Enter para manter o valor atual.");

                string newMake = GetInput("Marca", existingCar.CarMake!);
                string newModel = GetInput("Modelo", existingCar.CarModel!);
                string newTypeOfFuel = GetInput("Tipo de Combustível", existingCar.TypeOfFuel!);
                string newColor = GetInput("Cor", existingCar.CarColor!);
                double newEngineCapacity = GetDoubleInput("Cilindrada (L)", existingCar.EngineCapacity);
                double newCarTare = GetDoubleInput("Tara (Kg)", existingCar.CarTare);
                double newCarPrice = GetDoubleInput("Preço (€)", existingCar.CarPrice);

                Car updatedCar = new Car(id, newModel, newMake, newTypeOfFuel, newColor, newEngineCapacity, newCarTare, newCarPrice, existingCar.IsAvailable);
                _carService.Update(updatedCar);
                Console.WriteLine("Carro atualizado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro ao carregar carro para atualização: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao carregar carro para atualização:{ex.Message}");
                return;
            }
        }

        public void DeleteCar()
        {
            Console.WriteLine("\n--- Remover Carro ---");
            Console.Write("ID do Carro a Remover: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    bool deleted = _carService.Delete(id);
                    if (deleted)
                    {
                        Console.WriteLine($"Carro com ID {id} removido com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine($"Erro: Não foi possível remover o carro com o ID {id}.");
                    }                    
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao remover carro: {ex.Message}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado durante a remoção: {ex.Message}");
                }                               
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        private string GetInput(string prompt, string existingValue)
        {
            Console.Write($"{prompt} ({existingValue}): ");
            string input = Wrapper.ReadLineSafe();
            return string.IsNullOrWhiteSpace(input) ? existingValue : input;
        }

        private double GetDoubleInput(string prompt, double existingValue)
        {
            Console.Write($"{prompt} ({existingValue:F1}): ");
            string input = Wrapper.ReadLineSafe();
            if (string.IsNullOrWhiteSpace(input))
            {
                return existingValue;
            }

            if(double.TryParse(input, out double newValue) && newValue > 0)
            {
                return newValue;
            }

            Console.WriteLine("Valor inválido. Mantendo o valor atual.");
            return existingValue;
        }
    }
}

