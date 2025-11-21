using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Program
{
    public class ProviderContactProgram
    {
        private readonly IProviderContactService _providerContactService;

        public ProviderContactProgram(IProviderContactService providerContactService)
        {
            _providerContactService = providerContactService;
        }

        public void ProviderContactManagement()
        {
            int providerContactOption;
            do
            {
                Console.WriteLine("\n--- Gestão de Contactos de Fornecedores ---");
                Console.WriteLine(" 1 - Adicionar Contacto de Fornecedor");
                Console.WriteLine(" 2 - Obter Contacto de Fornecedor por ID");
                Console.WriteLine(" 3 - Obter todos os Contactos de Fornecedores");
                Console.WriteLine(" 4 - Atualizar Contacto de Fornecedor");
                Console.WriteLine(" 5 - Remover Contacto de Fornecedor");
                Console.WriteLine(" 0 - Voltar ao menu principal");
                Console.Write("Opção: ");

                if (int.TryParse(Console.ReadLine(), out providerContactOption))
                {
                    switch (providerContactOption)
                    {
                        case 1:
                            CreateProviderContact();
                            break;
                        case 2:
                            ReadProviderContact();
                            break;
                        case 3:
                            ReadAllProviderContacts();
                            break;
                        case 4:
                            UpdateProviderContact();
                            break;
                        case 5:
                            DeleteProviderContact();
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
                    providerContactOption = -1;
                }
            } while (providerContactOption != 0);
        }

        public void CreateProviderContact()
        {
            Console.WriteLine("\n--- Adicionar Contacto de Fornecedor ---");

            try
            {
                string firstName = ReadInput("Nome (Contacto do Fornecedor)");
                string lastName = ReadInput("Apelido (Contacto do Fornecedor)");
                string email = ReadInput("Email (Contacto do Fornecedor)");
                string phone = ReadInput("Telefone (Contacto do Fornecedor)");
                string jobTitle = ReadInput("Cargo do Contacto (Ex: Gerente de vendas)");

                Contact contactDetails = new Contact(firstName, lastName, email, phone, jobTitle);
                ProviderContact newProviderContact = new ProviderContact(contactDetails);
                ProviderContact createdContact = _providerContactService.Create(newProviderContact);

                Console.WriteLine($"Contacto de Fornecedor adicionado com sucesso. (ID: {createdContact.Id})");
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação: {ex.Message}");
            }
            catch(InvalidDataException ex)
            {
                Console.WriteLine($"Erro na operação: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao adicionar o contacto de Fornecedor: {ex.Message}");
            }
        }

        public void ReadProviderContact()
        {
            Console.WriteLine("\n--- Obter Contacto de Fornecedor por ID ---");

            try
            {
                int id = ReadInt("ID do Contacto do Fornecedor");
                ProviderContact providerContact = _providerContactService.Read(id);

                Console.WriteLine($"\n--- Detalhes do Contacto de Fornecedor (ID: {providerContact.Id}) ---");
                Console.WriteLine(providerContact.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        public void ReadAllProviderContacts()
        {
            Console.WriteLine("\n--- Listar todos os Contacto de Fornecedores ---");
            try
            {
                List<ProviderContact> providerContacts = _providerContactService.ReadAll();
                if (providerContacts != null && providerContacts.Any()) // Melhor verificação para lista vazia
                {
                    foreach (ProviderContact providerContact in providerContacts)
                    {
                        Console.WriteLine($"\n--- Client ID: {providerContact.Id} ---");
                        Console.WriteLine(providerContact.GetFormattedContactInfo());
                    }
                }
                else
                {
                    Console.WriteLine("Não há contactos de fornecedores registados.");
                }
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao listar os contactos dos fornecedores: {ex.Message}");
            }
        }

        public void UpdateProviderContact()
        {
            Console.WriteLine("\n--- Atualizar Contacto de Fornecedor ---");
            try
            {
                int id = ReadInt("ID do Contacto de Fornecedor a Atualizar");
                ProviderContact existingProviderContact = _providerContactService.Read(id);

                Console.WriteLine($"\nA atulizar Contacto ID: {existingProviderContact.Id}");
                Console.WriteLine("Pressione Enter para manter o valo atual.");

                string newFirstName = ReadInput("Primeiro Nome", existingProviderContact.ContactDetails.FirstName);
                string newLasName = ReadInput("Apelido", existingProviderContact.ContactDetails.LastName);
                string newEamil = ReadInput("Email", existingProviderContact.ContactDetails.Email);
                string newPhoneNumber = ReadInput("Telefone", existingProviderContact.ContactDetails.PhoneNumber);
                string newJobTitle = ReadInput("Cargo", existingProviderContact.ContactDetails.JobTitle);

                Contact tempUpdateContactDetails = new Contact(newFirstName, newLasName, newEamil, newPhoneNumber, newJobTitle);
                tempUpdateContactDetails.SetId(existingProviderContact.ContactDetails.Id);

                ProviderContact updatedProviderContact = new ProviderContact(
                    existingProviderContact.Id, tempUpdateContactDetails);

                _providerContactService.Update(updatedProviderContact);

                Console.WriteLine("Contacto de Fornecedor atualizado com sucesso.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro durante a atualização: {ex.Message}");
            }
        }

        public void DeleteProviderContact()
        {
            Console.WriteLine("\n--- Remover Contacto de Fornecedor ---");
            Console.Write("ID do Contacto de Fornecedor a Remover: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    bool deleted = _providerContactService.Delete(id);
                    if (deleted)
                    {
                        Console.WriteLine($"Contacto de Fornecedor com ID {id} removido com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"Contacto de Fornecedor com ID {id} não encontrado ou não foi possível remover.");
                    }
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao Remover Contacto de Fornecedor: {ex.Message}");
                }
                catch(InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro na operação de Remover Contacto de Fornecedor: {ex.Message}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado ao remover o contacto de fornecedor: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        private string ReadInput(string prompt, string? currentValue = null)
        {
            Console.WriteLine($"{prompt}{(currentValue != null ? $"(atual: {currentValue})" : "")}: ");
            string input = Wrapper.ReadLineSafe();
            return string.IsNullOrWhiteSpace(input) ? currentValue : input;
        }

        private int ReadInt(string prompt, int? currentValue = null)
        {            
            string input = ReadInput(prompt, currentValue?.ToString());
            int result;
            while (!int.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine("Entrada inválida. Por favor, insira um número inteiro positivo.");
                input = ReadInput(prompt, currentValue?.ToString());
            }
            return result;
        }
    }
}
