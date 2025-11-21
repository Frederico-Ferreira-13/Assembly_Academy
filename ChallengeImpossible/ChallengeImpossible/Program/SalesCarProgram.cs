using ChallengeImpossible.Model;
using ChallengeImpossible.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;

namespace ChallengeImpossible.Program
{
    public class SalesCarProgram
    {
        private readonly ISalesCarService _salesCarService;
        private readonly IClientService _clientService;
        private readonly ICarService _carService;

        public SalesCarProgram(ISalesCarService salesCarService, IClientService clientService, ICarService carService)
        {
            _salesCarService = salesCarService;
            _clientService = clientService;
            _carService = carService;
        }

        public void SalesCarManagement()
        {
            int salesCarOption;
            do
            {
                Console.WriteLine("\n--- Gestão de Vendas de Carros ---");
                Console.WriteLine(" 1 - Adicionar Venda de Carro");
                Console.WriteLine(" 2 - Obter Venda de Carro por ID");
                Console.WriteLine(" 3 - Obter todos as Vendas de Carros");
                Console.WriteLine(" 4 - Atualizar Venda de Carro");
                Console.WriteLine(" 5 - Remover Venda de Carro");
                Console.WriteLine(" 0 - Voltar ao menu principal");
                Console.Write("Opção: ");

                if (int.TryParse(Console.ReadLine(), out salesCarOption))
                {
                    switch (salesCarOption)
                    {
                        case 1:
                            CreateSalesCar();
                            break;
                        case 2:
                            ReadSalesCar();
                            break;
                        case 3:
                            ReadAllSalesCars();
                            break;
                        case 4:
                            UpdateSalesCar();
                            break;
                        case 5:
                            DeleteSalesCar();
                            break;
                        case 0:
                            Console.WriteLine("A voltar ao menu principal...");
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Por favor, insira um número inteiro como opção.");
                    salesCarOption = -1; // Garante que o loop continua
                }
            } while (salesCarOption != 0);
        }

        public void CreateSalesCar()
        {
            Console.WriteLine("\n--- Adicionar Venda de Carro ---");
            try
            {
                double price = ReadDouble("Preço de Venda Final");

                DateTime purchaseDate = ReadDate("Data da Compra (DD-MM-AAAA, deixe em branco para hoje)", DateTime.Now);

                Client selectedClient = SelectClient();
                if(selectedClient == null)
                {
                    Console.WriteLine("Seleção de cliente cancelada ou nenhum cliente válido selecionado. Venda não criada.");
                    return;
                }

                List<Car> selectedCars = SelectCarsForSale();
                if(selectedCars == null || !selectedCars.Any())
                {
                    Console.WriteLine("Seleção de carros cancelada ou nenhum carro válido selecionado. Venda não criada.");
                    return;
                }

                SalesCar newSalesCar = new SalesCar(price, purchaseDate, selectedClient, selectedCars);
                _salesCarService.Create(newSalesCar);

                Console.WriteLine($"Venda adicionada com sucesso! (ID: {newSalesCar.Id})");
                Console.WriteLine(newSalesCar.GetPurchaseSummary());
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação: {ex.Message}");
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine($"Erro de operação: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocurreu um erro inesperado ao adicionar a venda: {ex.Message}");
            }
        }

        public void ReadSalesCar()
        {
            Console.WriteLine("\n--- Obter Venda de Carro por ID ---");
            try
            {
                int id = ReadInt("ID da Venda");
                SalesCar salesCar = _salesCarService.Read(id);
                Console.WriteLine(salesCar.GetPurchaseSummary());
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao obter a venda: {ex.Message}");
            }
        }

        public void ReadAllSalesCars()
        {
            Console.WriteLine("\n--- Listar todos as Vendas de Carros ---");
            try
            {
                List<SalesCar> salesCars = _salesCarService.ReadAll();
                if (salesCars != null && salesCars.Any()) // Melhor verificação para lista vazia
                {
                    foreach (SalesCar salesCar in salesCars)
                    {
                        Console.WriteLine(salesCar.GetPurchaseSummary());
                        Console.WriteLine("---------------------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("Não há Venda de carros registados.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao listar vendas: {ex.Message}");
            }
        }

        public void UpdateSalesCar()
        {
            Console.WriteLine("\n--- Atualizar Venda de Carro ---");
            try
            {
                int id = ReadInt("ID da Venda a Atualizar");
                SalesCar existingSalesCar = _salesCarService.Read(id);

                Console.WriteLine($"\nA atualizar Venda com o ID: {existingSalesCar.Id}");

                double newPrice = ReadDouble($"Preço de Venda Final", existingSalesCar.Price);
                DateTime newPurchaseDate = ReadDate($"Data da Compra", existingSalesCar.PurchaseDate);

                Client newCustomer = existingSalesCar.Customer;
                if (ReadBool($"Deseja alterar o cliente? (Cliente Atual: {existingSalesCar.Customer.Name})"))
                {
                    Client potentialNewCustomer = SelectClient();
                    if (potentialNewCustomer != null)
                    {
                        newCustomer = potentialNewCustomer;
                    }
                    else
                    {
                        Console.WriteLine("Nenhum novo cliente selecionado. Mantendo o cliente atual.");
                    }
                }

                List<Car> updatedCars = new List<Car>(existingSalesCar.Cars);
                if (ReadBool("Deseja atualizar os carros da venda?"))
                {
                    List<Car> selectedNewCars = SelectCarsForSale();
                    if (selectedNewCars != null && selectedNewCars.Any())
                    {
                        updatedCars = selectedNewCars;
                    }
                    else
                    {
                        Console.WriteLine("Nenhum carro novo selecionado. Mantendo os carros atuais.");
                    }
                }

                SalesCar salesCarToUpdate = new SalesCar(id, newPrice, newPurchaseDate, newCustomer, updatedCars);
                _salesCarService.Update(salesCarToUpdate);

                Console.WriteLine("Venda de carro atualizada com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação nos novos dados da venda: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro de operação: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado durante a atualização da venda: {ex.Message}");
            }
        }

        public void DeleteSalesCar()
        {
            Console.WriteLine("\n--- Remover Venda de Carro ---");
            try
            {
                int id = ReadInt("ID da Venda a Remover");
                bool deleted = _salesCarService.Delete(id);
                if (deleted)
                {
                    Console.WriteLine($"Venda com ID {id} removida com sucesso! Carros associados agora disponíveis.");
                }
                else
                {
                    Console.WriteLine($"Venda com ID {id} não encontrada ou não foi possível remover.");
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao remover a venda: {ex.Message}");
            }
        }

        private Client SelectClient()
        {
            Console.WriteLine("\n--- Selecionar Cliente ---");
            List<Client> availableClients = _clientService.ReadAll();

            if(availableClients == null || !availableClients.Any())
            {
                Console.WriteLine("Não há clientes registados para associar à venda.");
                return null!;
            }

            Console.WriteLine("Clientes Disponíveis:");
            foreach(var client in availableClients)
            {
                Console.WriteLine($"ID: {client.Id}, Nome: {client.Name}");
            }

            int clientId = ReadInt("Insira o ID do cliente (0 para cancelar)");
            if (clientId == 0) return null!;

            Client? selectedClient = availableClients.FirstOrDefault(c => c.Id == clientId);
            if (selectedClient == null)
            {
                Console.WriteLine("ID de cliente inválido ou cliente não encontrado.");
            }
            return selectedClient!;
        }

        private List<Car> SelectCarsForSale()
        {
            Console.WriteLine("\n--- Selecionar Carros para Venda ---");
            List<Car> availableCars = _carService.ReadAll().Where(c => c.IsAvailable).ToList();

            if (availableCars == null || !availableCars.Any())
            {
                Console.WriteLine("Não há carros disponíveis para venda.");
                return null!;
            }

            List<Car> selectedCars = new List<Car>();
            int carId;

            do
            {
                Console.WriteLine("\nCarros Disponíveis:");
                foreach (var car in availableCars)
                {
                    if (!selectedCars.Any(sc => sc.Id == car.Id))
                    {
                        Console.WriteLine($"ID: {car.Id}, Marca: {car.CarMake}, Modelo: {car.CarModel}, Preço: {car.CarPrice:C2}");
                    }
                }
                Console.WriteLine("----------------------");
                Console.WriteLine("Carros Selecionados para esta venda:");
                if (selectedCars.Any())
                {
                    foreach (var car in selectedCars)
                    {
                        Console.WriteLine($" - ID: {car.Id}, {car.CarMake} {car.CarModel}");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum carro selecionado ainda.");
                }
                carId = ReadInt("Insira o ID do carro para adicionar (0 para terminar a seleção)");

                if (carId == 0)
                {
                    break;
                }
                Car? carToAdd = availableCars.FirstOrDefault(c => c.Id == carId);
                if (carToAdd != null)
                {
                    if (selectedCars.Any(sc => sc.Id == carToAdd.Id))
                    {
                        Console.WriteLine("Este carro já foi selecionado para esta venda.");
                    }
                    else
                    {
                        selectedCars.Add(carToAdd);
                        Console.WriteLine($"Carro '{carToAdd.CarMake} {carToAdd.CarModel}' adicionado.");
                    }
                }
                else
                {
                    Console.WriteLine("ID de carro inválido ou carro não encontrado/disponível.");
                }
            } while (true);

            return selectedCars;
        }

        public static int ReadInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write($"{prompt}: ");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out value))
                {
                    return value;
                }
                Console.WriteLine("Entrada inválida. Por favor, insira um número inteiro.");
            }
        }

        public static double ReadDouble(string prompt, double? defaultValue = null)
        {
            double value;
            while (true)
            {
                Console.Write($"{prompt} {(defaultValue.HasValue ? $"({defaultValue.Value:C2})" : "")}: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) && defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                if (double.TryParse(input, NumberStyles.Currency, CultureInfo.CurrentCulture, out value))
                {
                    return value;
                }
                Console.WriteLine("Entrada inválida. Por favor, insira um valor monetário.");
            }
        }

        public static DateTime ReadDate(string prompt, DateTime? defaultValue = null)
        {
            DateTime value;
            while (true)
            {
                Console.Write($"{prompt} {(defaultValue.HasValue ? $" ({defaultValue.Value.ToShortDateString()})" : "")}: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) && defaultValue.HasValue)
                {
                    return defaultValue.Value;
                }
                if (DateTime.TryParse(input, out value))
                {
                    return value;
                }
                Console.WriteLine("Entrada inválida. Por favor, insira uma data no formato DD-MM-AAAA.");
            }
        }

        public static bool ReadBool(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (s/n): ");
                string? input = Console.ReadLine()?.ToLower();
                if (input == "s")
                {
                    return true;
                }
                if (input == "n")
                {
                    return false;
                }
                Console.WriteLine("Entrada inválida. Por favor, responda 's' para sim ou 'n' para não.");
            }
        }
    }    
}
