using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Program
{
    public class AddressProgram
    {
        private readonly IAddressService _addressService;

        public AddressProgram(IAddressService addressService)
        {
            _addressService = addressService;
        }
        public void AddressManagement()
        {
            int addressOption;
            do
            {
                Console.WriteLine("\n--- Gestão de Endereços ---");
                Console.WriteLine("1 - Adicionar Endereço");
                Console.WriteLine("2 - Obter Endereço por ID");
                Console.WriteLine("3 - Obter todos os Endereços");
                Console.WriteLine("4 - Atualizar Endereço");
                Console.WriteLine("5 - Remover Endereço");
                Console.WriteLine("0 - Voltar ao Menu Principal");
                Console.Write("Opção: ");

                if (int.TryParse(Console.ReadLine(), out addressOption))
                {
                    switch (addressOption)
                    {
                        case 1:
                            CreateAddress();
                            break;
                        case 2:
                            ReadAddress();
                            break;
                        case 3:
                            ReadAllAddresses();
                            break;
                        case 4:
                            UpdateAddress();
                            break;
                        case 5:
                            DeleteAddress();
                            break;
                        case 0:
                            Console.WriteLine("Voltar ao Menu Principal");
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Tente novamente");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Por favor, insira um número inteiro como opção");
                    addressOption = -1;
                }
            } while (addressOption != 0);
        }

        public void CreateAddress()
        {
            Console.WriteLine("\n--- Adicionar Morada ---");

            Console.Write("Rua: ");
            string street = Wrapper.ReadLineSafe();
            Console.Write("Número da Porta: ");
            string doorNumber = Wrapper.ReadLineSafe();
            Console.Write("Andar (opcional): ");
            string floor = Wrapper.ReadLineSafe();
            Console.Write("Código Postal(ex: 1234-567): ");
            string postalCode = Wrapper.ReadLineSafe();
            Console.Write("Localidade: ");
            string locate = Wrapper.ReadLineSafe();
            Console.Write("Cidade: ");
            string city = Wrapper.ReadLineSafe();            
            Console.Write("País: ");
            string country = Wrapper.ReadLineSafe();
            Console.Write("Rua 2 / Bloco (opcional): ");
            string? street2 = Wrapper.ReadLineSafe();

            try
            {
                Address newAddress = new Address(
                     street,
                     street2,
                     doorNumber,
                     floor,
                     postalCode,
                     locate,
                     city,                     
                     country
                );

                Address createAddress = _addressService.Create(newAddress);
                Console.WriteLine($"Endereço adicionado com sucesso! (ID: {createAddress.Id}");               
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro ao adicionar endereço: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
           
        }

        public void ReadAddress()
        {
            Console.WriteLine("\n--- Obter Morada por ID ---");
            Console.Write("ID da Morada: ");
            if(int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    Address address = _addressService.Read(id);                    
                    
                    Console.WriteLine($"\nDetalhes do Endereço (ID: {address.Id}):");
                    Console.WriteLine(address.GetAddress());
                  
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao procurar endereço: {ex.Message}");
                }
                catch (InvalidOperationException ex) // Captura exceções do serviço quando o endereço não é encontrado
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }   
        }

        public void ReadAllAddresses()
        {
            Console.WriteLine("\n--- Listar todas as Moradas ---");
            List<Address> addresses = _addressService.ReadAll();

            if(addresses != null && addresses.Any()) // if(addresses.Count > 0)
            {
                foreach(Address address in addresses)
                {
                    Console.WriteLine($"\n--- Endereço ID: {address.Id} ---");
                    Console.WriteLine(address.GetAddress());
                }
            }
            else
            {
                Console.WriteLine("Nenhum endereço registado.");
            }
        }
        
        public void UpdateAddress()
        {
            Console.WriteLine("\n--- Atualizar Morada ---");
            Console.Write("ID da Morada a Atualizar: ");
            if(int.TryParse(Wrapper.ReadLineSafe(), out int id))
            {
                try
                {
                    Address existingAddress = _addressService.Read(id);                

                    Console.WriteLine($"\nA atualizar Morada Id: {existingAddress.Id}");
                    Console.WriteLine("Pressione Enter para manter o valor atual.");

                    Console.WriteLine($"Rua ({existingAddress.Street}): ");
                    string street = Wrapper.ReadLineSafe();
                    if (string.IsNullOrWhiteSpace(street)) street = existingAddress.Street;

                    Console.WriteLine($"Número da Porta ({existingAddress.DoorNumber}):");
                    string doorNumber = Wrapper.ReadLineSafe();
                    if (string.IsNullOrWhiteSpace(doorNumber)) doorNumber = existingAddress.DoorNumber;

                    Console.WriteLine($"Andar ({existingAddress.Floor}):");
                    string? floor = Wrapper.ReadLineSafe();
                    if (string.IsNullOrWhiteSpace(floor)) floor = existingAddress.Floor;

                    Console.WriteLine($"Código Postal ({existingAddress.PostalCode}):");
                    string postalCode = Wrapper.ReadLineSafe();
                    if (string.IsNullOrWhiteSpace(postalCode)) postalCode = existingAddress.PostalCode;

                    Console.WriteLine($"Localidade ({existingAddress.Locate}):");
                    string locate = Wrapper.ReadLineSafe();
                    if (string.IsNullOrWhiteSpace(locate)) locate = existingAddress.Locate;

                    Console.WriteLine($"Cidade ({existingAddress.City}):");
                    string city = Wrapper.ReadLineSafe();
                    if (string.IsNullOrWhiteSpace(city)) city = existingAddress.City;

                    Console.WriteLine($"País ({existingAddress.Country}):");
                    string country = Wrapper.ReadLineSafe();
                    if (string.IsNullOrWhiteSpace(country)) country = existingAddress.Country;

                    Console.WriteLine($"Rua 2 / Bloco ({existingAddress.Street2}):");
                    string? street2 = Wrapper.ReadLineSafe();
                    if (string.IsNullOrWhiteSpace(street2)) street2 = existingAddress.Street2;
                    
                    Address updatedAddress = new Address(
                         street,
                         street2,
                         doorNumber,
                         floor,
                         locate,
                         postalCode,
                         city,
                         country
                    );

                    updatedAddress.SetId(existingAddress.Id);
                    Address result = _addressService.Update(updatedAddress);

                    Console.WriteLine("Endereço atualizado com sucesso.");                                  
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro de validação ao atualizar: {ex.Message}");
                }
                catch (InvalidOperationException ex) // Captura exceções do serviço quando o endereço não é encontrado
                {
                    Console.WriteLine($"Erro: {ex.Message}");
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

        public void DeleteAddress()
        {
            Console.WriteLine("\n--- Remover Morada ---");
            Console.Write("ID da Morada a remover: ");
            if(int.TryParse(Wrapper.ReadLineSafe(), out int id))
            {
                try
                {
                    bool deleted = _addressService.Delete(id);
                    if (deleted)
                    {
                        Console.WriteLine($"Endereço com ID {id} removido com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine($"Endereço com ID {id} não encontrado ou não foi possível remover.");
                    }
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao remover endereço: {ex.Message}");
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
    }
}
