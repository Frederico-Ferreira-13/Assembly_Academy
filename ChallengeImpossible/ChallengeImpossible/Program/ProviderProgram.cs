using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Program
{
    public class ProviderProgram
    {
        private readonly IProviderService _providerService;
        private readonly IProviderContactService _providerContactService;

        public ProviderProgram(IProviderService providerService, IProviderContactService providerContactService)
        {
            _providerService = providerService;
            _providerContactService = providerContactService;
        }

        public void ProviderManagement()
        {
            int providerOption;
            do
            {
                Console.WriteLine("\n--- Gestão de Fornecedores ---");
                Console.WriteLine(" 1 - Adicionar Fornecedor");
                Console.WriteLine(" 2 - Obter Fornecedor por ID");
                Console.WriteLine(" 3 - Obter todos Fornecedores");
                Console.WriteLine(" 4 - Atualizar Fornecedor");
                Console.WriteLine(" 5 - Remover Fornecedor");
                Console.WriteLine(" 0 - Voltar ao menu principal");
                Console.Write("Opção: ");

                if (int.TryParse(Console.ReadLine(), out providerOption))
                {
                    switch (providerOption)
                    {
                        case 1:
                            CreateProvider();
                            break;
                        case 2:
                            ReadProvider();
                            break;
                        case 3:
                            ReadAllProviders();
                            break;
                        case 4:
                            UpdateProvider();
                            break;
                        case 5:
                            DeleteProvider();
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
                    providerOption = -1;
                }
            } while (providerOption != 0);
        }

        public void CreateProvider()
        {
            Console.WriteLine("\n--- Adicionar Fornecedor ---");            
            
            try
            {
                string providerName = ReadInput("Nome da Empresa") ?? "";
                string sector = ReadInput("Setor do Fornecedor") ?? "";

                Address mainAddress = ReadAddressDetails();
                ProviderContact mainProviderContact = ReadProviderContactDetails();

                Provider newProvider = new Provider(providerName, mainAddress, sector, mainProviderContact);
                
                _providerService.Create(newProvider);
                Console.WriteLine($"Contacto adicionado com sucesso. (ID: {newProvider.Id}");

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao adicionar o fornecedor: {ex.Message}");
            }
        }

        public void ReadProvider()
        {
            Console.WriteLine("\n--- Obter Fornecedor por ID ---");
            try
            {
                int id = ReadInt("ID do Fornecedor");
                Provider provider = _providerService.Read(id);
                Console.WriteLine($"\n--- Detalhes do Fornecedor (ID: {provider.Id}) ---");
                Console.WriteLine(provider.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro ao procurar Fornecedor: {ex.Message}");
            }
        }

        public void ReadAllProviders()
        {
            Console.WriteLine("\n--- Listar todos os Fornecedores ---");
            try
            {
                List<Provider> providers = _providerService.ReadAll();
                if (providers != null && providers.Any())
                {
                    foreach (Provider provider in providers)
                    {
                        Console.WriteLine($"\n--- Client ID: {provider.Id} ---");
                        Console.WriteLine(provider.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Não há Fornecedores registados.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao listar fornecedores: {ex.Message}");
            }            
        }

        public void UpdateProvider()
        {
            try
            {
                int id = ReadInt("ID do Fornecedor a Atualizar");
                Provider existingProvider = _providerService.Read(id);

                Console.WriteLine($"\nA atualizar Fornecedor com o ID: {existingProvider.Id}");
                Console.WriteLine("Pressione Enter para manter o valor atual.");

                string newName = ReadInput("Nome da Empresa", existingProvider.Name) ?? "";
                string newSector = ReadInput("Setor", existingProvider.Sector) ?? "";
                Address newAddress = ReadAddressDetails(existingProvider.MainAddress);
                ProviderContact newContact = ReadProviderContactDetails(existingProvider.MainProviderContact);

                existingProvider.Update(newName, newAddress, newSector, newContact);

                _providerService.Update(existingProvider);
                Console.WriteLine("Fornecedor atualizado com sucesso.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocurreu um erro durante a atualização: {ex.Message}");
            }
        }
        
        public void DeleteProvider()
        {
            Console.WriteLine("\n--- Remover Fornecedor ---");
            try
            {
                int id = ReadInt("ID do Fornecedor a Remover");
                bool deleted = _providerService.Delete(id);
                if (deleted)
                {
                    Console.WriteLine($"Fornecedor com ID {id} removido com sucesso!");
                }
                else
                {
                    Console.WriteLine($"Fornecedor com ID {id} não foi encontrado.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao remover o Fornecedor: {ex.Message}");
            }
        }

        private string? ReadInput(string prompt, string currentValue = null!)
        {
            Console.WriteLine($"{prompt}{(currentValue != null ? $"(atual: {currentValue})" : "")}: ");
            string? input = Wrapper.ReadLineSafe();
            return string.IsNullOrWhiteSpace(input) ? currentValue : input;
        }

        private int ReadInt(string prompt, int? currentValue = null)
        {
            int result;
            string? input = ReadInput(prompt, currentValue?.ToString());
            while(!int.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine("Entrada inválida. Por favor, insira um número inteiro positivo.");
                input = ReadInput(prompt, currentValue?.ToString());
            }
            return result;
        }

        private Address ReadAddressDetails(Address? currentAddress = null)
        {
            Console.WriteLine("\n--- Detalhes do Endereço Principal ---");
            string street = ReadInput("Rua", currentAddress?.Street!) ?? "";
            string? street2 = ReadInput("Morada Complementar [Opcional]", currentAddress?.Street2!);
            string doorNumber = ReadInput("Número da Porta", currentAddress?.DoorNumber!) ?? "";
            string? floor = ReadInput("Andar [Opcional]", currentAddress?.Floor!);
            string postalCode = ReadInput("Código Postal", currentAddress?.PostalCode!) ?? "";
            string locate = ReadInput("Localidade", currentAddress?.Locate!) ?? "";
            string city = ReadInput("Cidade/Concelho", currentAddress?.City!) ?? "";
            string country = ReadInput("País", currentAddress?.Country!) ?? "";
            return new Address(street, street2, doorNumber, floor, postalCode, locate, city, country);
        }

        private ProviderContact ReadProviderContactDetails(ProviderContact? currentContact = null)
        {
            Console.WriteLine("\n--- Detalhes do Contacto Principal do Fornecedor ---");
            Contact? currentDetails = currentContact?.ContactDetails;
            string firstName = ReadInput("Nome (Contacto)", currentDetails?.FirstName!) ?? "";
            string lastName = ReadInput("Apelido (Contacto)", currentDetails?.LastName!) ?? "";
            string email = ReadInput("Email (Contacto)", currentDetails?.Email!) ?? "";
            string phone = ReadInput("Telefone (Contacto)", currentDetails?.PhoneNumber!) ?? "";
            string jobTitle = ReadInput("Cargo do Contacto", currentDetails?.JobTitle!) ?? "";

            Contact updatedContact = new Contact(firstName, lastName, email, phone, jobTitle);
            if (currentDetails != null)
            {
                updatedContact.SetId(currentDetails.Id);
            }
            return new ProviderContact(updatedContact);
        }
    }
}
