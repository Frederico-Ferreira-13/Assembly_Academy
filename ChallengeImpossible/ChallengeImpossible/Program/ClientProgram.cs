using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Implementers;
using ChallengeImpossible.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ChallengeImpossible.Program
{
    public class ClientProgram
    {
        private readonly IClientService _clientSerivce;

        public ClientProgram(IClientService clientService)
        {
            _clientSerivce = clientService;
        }
        public void ClientManagement()
        {
            int clientOption;

            do
            {
                Console.WriteLine("\n--- Gestão de Clientes ---");
                Console.WriteLine(" 1 - Adicionar Cliente");
                Console.WriteLine(" 2 - Obter Cliente por ID");
                Console.WriteLine(" 3 - Obter todos os Clientes");
                Console.WriteLine(" 4 - Atualizar Cliente");
                Console.WriteLine(" 5 - Remover Cliente"); 
                Console.WriteLine(" 0 - Voltar ao menu principal");
                Console.Write("Opção: ");

                if (int.TryParse(Console.ReadLine(), out clientOption))
                {
                    switch (clientOption)
                    {
                        case 1:
                            CreateClient();
                            break;
                        case 2:
                            ReadClient();
                            break;
                        case 3:
                            ReadAllClients();
                            break;
                        case 4:
                            UpdateClient();
                            break;
                        case 5:
                            DeleteClient();
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
                    clientOption = -1; // Garante que o loop continua
                }
            } while (clientOption != 0);
        }

        public void CreateClient()
        {
            Console.WriteLine("\n--- Adicionar Client ---");
            Console.Write("Nome do CLiente: ");
            string clientName = Wrapper.ReadLineSafe();

            Console.Write("Setor: ");
            string sector = Wrapper.ReadLineSafe();

            string nif = GetValidatedNifInput();            

            Console.Write("\n--- Endereço Principal do Cliente ---");
            Address mainAddress = GetAddressInput("principal");

            Console.WriteLine("\n--- Endereço de Entrega do Cliente (Opcional - Pressione Enter em todos os campos para ignorar) ---");
            Address deliveryAddress = GetAddressInput("de entrega", isOptional: true);

            Console.WriteLine("\n--- Contacto Principal do Cliente ---");
            Contact mainContact = GetContactInput("principal");

            try
            {
                Client newClient = new Client(clientName, mainAddress, sector, nif, deliveryAddress, mainContact);
                _clientSerivce.Create(newClient);
                Console.WriteLine($"Cliente '{newClient.Name}' adicionado com sucesso! (ID: {newClient.Id}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação ao adicionar cliente: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro de operação ao adicionar cliente: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao adicionar cliente: {ex.Message}");
            }
        }

        public void ReadClient()
        {
            Console.WriteLine("\n--- Obter Cliente por ID ---");
            Console.Write("ID do Cliente: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    Client client = _clientSerivce.Read(id);                  
                    
                    Console.WriteLine("\nDetalhes do Cliente:");
                    Console.WriteLine(client.GetClientSummary());
                    
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao procurar cliente: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {                
                    Console.WriteLine($"Erro ao procurar cliente: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado ao obter cliente: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        public void ReadAllClients()
        {
            Console.WriteLine("\n--- Listar todos os Clientes ---");
            List<Client> clients = _clientSerivce.ReadAll();
            if (clients != null && clients.Any()) // Melhor verificação para lista vazia
            {
                foreach (Client client in clients)
                {
                    Console.WriteLine($"\n--- Client ID: {client.Id} ---");
                    Console.WriteLine(client.GetClientSummary()); // Usar GetDescription
                }
            }
            else
            {
                Console.WriteLine("Não há clientes registados.");
            }
        }

        public void UpdateClient()
        {
            Console.WriteLine("\n--- Atualizar Cliente ---");
            Console.Write("ID do Cliente a Atualizar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Client? existingClient;
                try
                {
                    existingClient = _clientSerivce.Read(id);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro ao atualizar: {ex.Message}");
                    return;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao atualizar: {ex.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado ao obter cliente para atualização: {ex.Message}");
                    return;
                }

                Console.WriteLine($"\nAtualizando Cliente ID: {existingClient.Id}");
                Console.WriteLine("Pressione Enter para manter o valor atual.");

                string newName = GetInputWithDefault("Nome do Cliente", existingClient.Name);
                string newSector = GetInputWithDefault("Setor", existingClient.Sector);
                string newNIF = GetValidatedNifInput(existingClient.NIF);
                Address updateMainAddress = GetAddressInput("principal", existingClient.Address);
                Address updateDeliveryAddress = GetAddressInput("de entrega", existingClient.DeliveryAddress, isOptional: true);
                Contact updateMainContact = GetContactInput("principal", existingClient.Contact);

                try
                {
                    Client clientToUpdate = new Client(id, newName, updateMainAddress, newSector, newNIF, updateDeliveryAddress, updateMainContact);
                    _clientSerivce.Update(clientToUpdate);
                    Console.WriteLine("Cliente atualizado com sucesso.");                    
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro de validação ao adicionar cliente: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro de operação ao adicionar cliente: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado durante a atualização de cliente: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }                
        }

        public void DeleteClient()
        {
            Console.WriteLine("\n--- Remover Cliente ---");
            Console.Write("ID do Cliente a Remover: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    bool deleted = _clientSerivce.Delete(id);
                    if (deleted)
                    {
                        Console.WriteLine($"Cliente com ID {id} removido com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"Cliente com ID {id} não encontrado ou não foi possível remover.");
                    }
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao remover cliente: {ex.Message}");
                }
                catch(InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro de operação ao remover cliente: {ex.Message}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado ao remover cliente: {ex.Message}");
                }                
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        private string GetValidatedNifInput(string currentNif = null!)
        {
            string nif;
            while (true)
            {
                Console.Write($"NIF ({(currentNif != null ? $"atual: {currentNif} - " : "")}9 digitos numéricos): ");
                nif = Wrapper.ReadLineSafe();

                if (string.IsNullOrWhiteSpace(nif))
                {
                    if(currentNif != null)
                    {
                        return currentNif;
                    }
                    Console.WriteLine("O NIF é obrigatório.");
                }
                else
                {
                    try
                    {
                        var tempClient = new Client("Temp Name", null!, "Temp Sector", nif, null!, null!);
                        return nif;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private string GetInputWithDefault(string prompt, string defaultValue)
        {
            Console.Write($"{prompt} (atual: {defaultValue}): ");
            string input = Wrapper.ReadLineSafe();
            return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
        }

        private Address GetAddressInput(string addressType, Address currentAddress = null!, bool isOptional = false)
        {
            string street, street2, doorNumber, floor, postalCode, locate, city, country;
            Address? address = null;
            bool valid = false;

            while (!valid)
            {
                string streetPrompt = $"Rua do Endereço {addressType}";
                street = GetInputWithDefault(streetPrompt, currentAddress?.Street ?? "");

                string street2Prompt = $"Rua (2) do Endereço {addressType}";
                street2 = GetInputWithDefault(street2Prompt, currentAddress?.Street2 ?? "");

                string doorNumberPrompt = $"Nº da Porta {addressType}";
                doorNumber = GetInputWithDefault(doorNumberPrompt, currentAddress?.DoorNumber ?? "");

                string floorPrompt = $"Andar {addressType}";
                floor = GetInputWithDefault(floorPrompt, currentAddress?.Floor ?? "");

                string postalCodePrompt = $"Código Postal do Endereço {addressType} (Ex: 1234-567)";
                postalCode = GetInputWithDefault(postalCodePrompt, currentAddress?.PostalCode ?? "");

                string locatePrompt = $"Localidade do Endereço {addressType}";
                locate = GetInputWithDefault(locatePrompt, currentAddress?.Locate ?? "");

                string cityPrompt = $"Cidade do Endereço {addressType}";
                city = GetInputWithDefault(cityPrompt, currentAddress?.City ?? "");

                string countryPrompt = $"País do Endereço {addressType}";
                country = GetInputWithDefault(countryPrompt, currentAddress?.Country ?? "");

                if (isOptional && string.IsNullOrWhiteSpace(street) && string.IsNullOrWhiteSpace(street2) &&
                    string.IsNullOrWhiteSpace(doorNumber) && string.IsNullOrWhiteSpace(floor) &&
                    string.IsNullOrWhiteSpace(postalCode) && string.IsNullOrWhiteSpace(locate) &&
                    string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(country))
                {
                    return null!;
                }

                try
                {
                    address = new Address(street, street2, doorNumber, floor, postalCode, locate, city, country);
                    valid = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro no endereço {addressType}: {ex.Message}. Por favor, insira novamente.");
                }
            }
            return address!;
        }

        private Contact GetContactInput(string contactType, Contact currentContact = null!)
        {
            string firstName, lastName, email, phone, jobTitle;
            Contact? contact = null;
            bool valid = false;

            while (!valid)
            {
                firstName = GetInputWithDefault($"Primeiro Nome do Contacto {contactType}", currentContact?.FirstName ?? "Não definido");
                lastName = GetInputWithDefault($"Último Nome do Contacto {contactType}", currentContact?.LastName ?? "Não definido");
                email = GetInputWithDefault($"E-mail do Contacto {contactType}", currentContact?.Email ?? "Não definido");
                phone = GetInputWithDefault($"Telefone do Contacto {contactType}", currentContact?.PhoneNumber ?? "Não definido");
                jobTitle = GetInputWithDefault($"Título do Cargo do Contacto {contactType} (opcional)", currentContact?.JobTitle ?? "Não definido");
               

                try
                {
                    contact = new Contact(firstName, lastName, phone, email, jobTitle);
                    valid = true;
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro no contacto {contactType}: {ex.Message}. Por favor, insira novamente.");
                }
            }
            return contact!;
        }        
    }
}