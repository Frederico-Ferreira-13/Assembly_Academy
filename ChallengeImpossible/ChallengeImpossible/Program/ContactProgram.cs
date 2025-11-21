using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Services.Implementers;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Program
{
    public class ContactProgram
    {
        private readonly IContactService _contactService;

        public ContactProgram(IContactService contactService)
        {
            _contactService = contactService;
        }

        public void ContactManagement()
        {
            int contactOption;
            do
            {
                Console.WriteLine("\n--- Gestão de Contactos ---");
                Console.WriteLine(" 1 - Adicionar Contacto");
                Console.WriteLine(" 2 - Obter Contacto por ID");
                Console.WriteLine(" 3 - Obter todos os Contactos");
                Console.WriteLine(" 4 - Atualizar Contacto");
                Console.WriteLine(" 5 - Remover Contacto");
                Console.WriteLine(" 0 - Voltar ao menu principal");
                Console.Write("Opção: ");

                if (int.TryParse(Console.ReadLine(), out contactOption))
                {
                    switch (contactOption)
                    {
                        case 1:
                            CreateContact();
                            break;
                        case 2:
                            ReadContact();
                            break;
                        case 3:
                            ReadAllContacts();
                            break;
                        case 4:
                            UpdateContact();
                            break;
                        case 5:
                            DeleteContact();
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
                    contactOption = -1; // Garante que o loop continua
                }
            } while (contactOption != 0);
        }

        public void CreateContact()
        {
            Console.WriteLine("\n--- Adicionar Contacto ---");
            Contact newContact = GetContactInput();          

            try
            {                
                _contactService.Create(newContact);
                Console.WriteLine($"Contacto adicionado com sucesso! (ID: {newContact.Id}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro ao adicionar contacto: {ex.Message}");
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine($"Erro de operação ao adicionar contacto: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao adicionar o contacto: {ex.Message}");
            }
        }

        public void ReadContact()
        {
            Console.WriteLine("\n--- Obter Contacto por ID ---");
            Console.Write("ID do Contacto: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    Contact contact = _contactService.Read(id);               
                    Console.WriteLine($"\n--- Detalhes do Contacto (ID: {contact.Id}) ---");
                    Console.WriteLine(contact.GetContactSummary());             
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao procurar contacto: {ex.Message}");
                }
                catch(InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro ao procurar contacto: {ex.Message}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado ao obter contacto: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        public void ReadAllContacts()
        {
            Console.WriteLine("\n--- Listar todos os Contactos ---");
            List<Contact> contacts = _contactService.ReadAll();
            if (contacts != null && contacts.Any()) // Melhor verificação para lista vazia
            {
                foreach (Contact contact in contacts)
                {
                    Console.WriteLine($"\n--- Contacto ID: {contact.Id} ---");
                    Console.WriteLine(contact.GetContactSummary()); // Usar o ToString que está subrescrito do Contact
                }
            }
            else
            {
                Console.WriteLine("Não há contactos registados.");
            }
        }

        public void UpdateContact()
        {
            Console.WriteLine("\n--- Atualizar Contacto ---");
            Console.Write("ID do Contacto a Atualizar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Contact? existingContact = null;
                try
                {
                    existingContact = _contactService.Read(id);
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao carregar contacto para atualização: {ex.Message}");
                    return;
                }
                catch(InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro ao carregar contacto para atualização: {ex.Message}");
                    return;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado ao carregar contacto para atualização: {ex.Message}");
                    return;
                }                

                Console.WriteLine($"\nA atualizar Contacto ID: {existingContact.Id}");
                Console.WriteLine("Pressione Enter para manter o valor atual.");

                string newFirstName = GetInputWithDefault("Primeiro Nome", existingContact.FirstName);
                string newLastName = GetInputWithDefault("Último Nome", existingContact.LastName);
                string newEmail = GetInputWithDefault("Email", existingContact.Email);
                string newPhoneNumber = GetInputWithDefault("Telefone", existingContact.PhoneNumber);
                string newJobTitle = GetInputWithDefault("Cargo", existingContact.JobTitle);

                try
                {
                    Contact updatedContact = new Contact(newFirstName, newLastName, newEmail, newPhoneNumber, newJobTitle);
                    updatedContact.SetId(existingContact.Id);

                    _contactService.Update(updatedContact);
                    Console.WriteLine("Contacto atualizado com sucesso!");                    
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro de validação nos novos dados do contacto: {ex.Message}");
                }
                catch(InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro de operação ao atualizar contacto: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado durante a atualização: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        public void DeleteContact()
        {
            Console.WriteLine("\n--- Remover Contacto ---");
            Console.Write("ID do Cliente a Remover: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    bool deleted = _contactService.Delete(id);
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
                    Console.WriteLine($"Erro ao remover contacto: {ex.Message}");
                }
                catch(InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro de operação ao remover contacto: {ex.Message}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado ao remover contacto: {ex.Message}");
                }                
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        private Contact GetContactInput(Contact? currentContact = null)
        {
            string firstName, lastName, email, phoneNumber, jobTitle;
            Contact? contact = null;
            bool valid = false;

            while (!valid)
            {
                firstName = GetInputWithDefault("Primeiro Nome", currentContact?.FirstName ?? "");
                lastName = GetInputWithDefault("Último Nome", currentContact?.LastName ?? "");
                email = GetInputWithDefault("Email", currentContact?.Email ?? "");
                phoneNumber = GetInputWithDefault("Telefone", currentContact?.PhoneNumber ?? "");
                jobTitle = GetInputWithDefault("Cargo", currentContact?.JobTitle ?? "");

                try
                {
                    contact = new Contact(firstName, lastName, email, phoneNumber, jobTitle);
                    valid = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro na validação do contacto: {ex.Message}. Por favor, insira novamente.");
                }
            }
            return contact!;        
        }

        private string GetInputWithDefault(string prompt, string? defaultValue)
        {
            Console.Write($"{prompt} {(string.IsNullOrWhiteSpace(defaultValue) ? "(opcional)" : $"(atual: {defaultValue})")}: ");
            string input = Console.ReadLine() ?? string.Empty;
            return string.IsNullOrWhiteSpace(input) ? defaultValue ?? string.Empty : input;
        }
    }
}
