using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Services.Implementers;
using ChallengeImpossible.Services.Interfaces;
using System.Text;

namespace ChallengeImpossible.Program
{
    public class UserProgram
    {
        private readonly IUserService _userService;
        private User? _loggedInUser;

        public UserProgram(IUserService userService)
        {
            _userService = userService;           
        }

        public void UserManagement()
        {
            if (_loggedInUser == null)
            {
                bool loggedIn = PerformLogin();
                if (!loggedIn)
                {
                    Console.WriteLine("Login falhou. Não é possível aceder à gestão de utilizadores.");
                    Console.WriteLine("Prima qualquer tecla para continuar...");
                    Console.ReadKey();
                    return;
                }
            }
            int userOption;
            do
            {
                Console.WriteLine($"\n--- Gestão de Utilizadores (Logado como: {_loggedInUser!.FirstName} ({_loggedInUser.Role})) ---");
                Console.WriteLine(" 1 - Adicionar Novo Utilizador");
                Console.WriteLine(" 2 - Ver Detalhes do utilizador");
                Console.WriteLine(" 3 - Obter todos os Utilizadores");
                Console.WriteLine(" 4 - Atualizar Utilizador");
                Console.WriteLine(" 5 - Remover Utilizador");
                Console.WriteLine(" 6 - Alterar Password do Utilziador");
                Console.WriteLine(" 7 - Logout");
                Console.WriteLine(" 0 - Voltar ao menu principal");
                Console.Write("Opção: ");

                if (int.TryParse(Wrapper.ReadLineSafe(), out userOption))
                {
                    try
                    {
                        switch (userOption)
                        {
                            case 1:
                                CreateUser();
                                break;
                            case 2:
                                ReadUser();
                                break;
                            case 3:
                                ReadAllUsers();
                                break;
                            case 4:
                                UpdateUser();
                                break;
                            case 5:
                                DeleteUser();
                                break;
                            case 6:
                                ChangeLoggedInUserPassword();
                                break;
                            case 7:
                                PerformLogout();
                                userOption = 0;
                                break;
                            case 0:
                                Console.WriteLine("A voltar ao menu principal...");
                                break;
                            default:
                                Console.WriteLine("Opção inválida. Tente novamente.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Por favor, insira um número inteiro como opção.");
                    userOption = -1;
                }

                if(userOption != 0)
                {
                    Console.WriteLine("\nPrima qualquer tecla para continuar...");
                    Console.ReadKey();
                }
                if (_loggedInUser == null) break;
                } while (userOption != 0);
            }
        
        private void DisplayUserSummary(User user)
        {
            Console.WriteLine("\n--- Resumo do Utilizador ---");
            Console.WriteLine($"ID: {user.Id}");
            Console.WriteLine($"Nome: {user.FirstName}{user.LastName}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Telefone: {user.PhoneNumber ?? "Não definido"}");
            Console.WriteLine($"Papel: {user.Role}");
            Console.WriteLine($"Endereço:\n {user.Addresses?.GetAddress().Replace("\n", "\n ") ?? "Não definido"}");
            Console.WriteLine("Histórico de Compras");

            if (user.PurchaseHistory.Any())
            {
                foreach(SalesCar sale in user.PurchaseHistory)
                {
                    Console.WriteLine($" - Venda ID: {sale.Id}, Data: {sale.PurchaseDate.ToShortDateString()}, Total: {sale.TotalCost:C2}");
                }
            }
            else
            {
                Console.WriteLine("Nenhum registo.");
            }
        }

        private bool PerformLogin()
        {
            Console.WriteLine("\n--- Autenticação ---");
            int attempts = 3;
            while(attempts > 0)
            {
                Console.Write("Email: ");
                string email = Wrapper.ReadLineSafe();
                Console.Write("Password: ");
                string password = Wrapper.ReadLineSafe();

                try
                {
                    _loggedInUser = _userService.ValidateLogin(email, password);
                    if(_loggedInUser != null)
                    {
                        Console.WriteLine($"Login bem-sucedido! Bem-vindo, {_loggedInUser.FirstName} ({_loggedInUser.Role})");
                        return true;
                    }
                    else
                    {
                        attempts--;
                        Console.WriteLine($"Email ou password inválidos. Tentativas restantes: {attempts}");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro de login: {ex.Message}. Tentativas restantes: {attempts}");
                    attempts--;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro durante o login: {ex.Message}");
                }
            }
            Console.WriteLine("Número máximo de tentivas excedido.");
            Console.WriteLine("Prima qualquer tecla para continuar...");
            Console.ReadKey();
            return false;
        }

        private void PerformLogout()
        {
            Console.WriteLine("A efetuar logout...");
            _loggedInUser = null!;
            Console.WriteLine("Logout bem-sucedido.");
            Console.WriteLine("Prima qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public void CreateUser()
        {
            if(_loggedInUser!.Role != UserRole.Admin)
            {
                Console.WriteLine("Apenas Administradores podem criar novos utilizadores.");
                Console.WriteLine("Prima qualquer tecla para continuar...");
                Console.ReadKey();
                return;                
            }

            Console.WriteLine("\n--- Criar Novo Utilizador ---");
            try
            {
                Console.Write("Primeiro Nome: ");
                string firstName = Wrapper.ReadLineSafe();

                Console.Write("Último Nome: ");
                string lastName = Wrapper.ReadLineSafe();

                Console.Write("Número de telefone (opcional): ");
                string phoneNumber = Wrapper.ReadLineSafe();

                Console.Write("Email: ");
                string email = Wrapper.ReadLineSafe();

                Console.Write("Password: ");
                string password = Wrapper.ReadLineSafe();

                Console.WriteLine("\n--- Endereço do Utilizador ---");
                Address address = GetAddressInput("do utilizador");

                Console.WriteLine("Selecionar Papel (Role):");
                foreach (UserRole r in Enum.GetValues(typeof(UserRole)))
                {
                    Console.WriteLine($"  {(int)r} - {r}");
                }
                Console.Write($"Papel (padrão: {UserRole.Client}): ");
                string roleInput = Wrapper.ReadLineSafe();
                UserRole role = UserRole.Client;

                if (!string.IsNullOrWhiteSpace(roleInput) && int.TryParse(roleInput, out int roleValue) && Enum.IsDefined(typeof(UserRole), roleValue))
                {
                    role = (UserRole)roleValue;
                }
                else
                {
                    Console.WriteLine($"Opção de papel inválida ou vazia. Usando o papel padrão: {role}.");
                }

                User newUser = new User(0, role, firstName, lastName, phoneNumber, email, address, password);
                _userService.Create(newUser);
                Console.WriteLine($"Utilizador '{newUser.FirstName} {newUser.LastName}' ({newUser.Role}) criado com sucesso! (ID: {newUser.Id})");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Erro de operação: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao adicionar a venda: {ex.Message}");
            }
        }

        public void ReadUser()
        {
            if(_loggedInUser!.Role == UserRole.Client)
            {
                Console.WriteLine("\n--- Os Meus Detalhes de Utilizar ---");
                Console.WriteLine(_loggedInUser.GetUserSummary());
                return;
            }

            Console.WriteLine("\n--- Ver Detalhes do Utilizador ---");
            Console.Write("ID da Venda(0 para o seu próprio perfil): ");
            if (int.TryParse(Wrapper.ReadLineSafe(), out int id))
            {
                User? userToDisplay = null;
                if (id <= 0 || id == _loggedInUser.Id)
                {
                    userToDisplay = _loggedInUser;
                }
                else
                {
                    if(_loggedInUser.Role == UserRole.Admin || _loggedInUser.Role == UserRole.Employee)
                    {
                        userToDisplay = _userService.Read(id);
                    }
                    else
                    {
                        Console.WriteLine("Não tem permissão para ver detalhes de outros utilizadores.");
                        return;
                    }
                }

                if (userToDisplay != null)
                {
                    Console.WriteLine(userToDisplay.GetUserSummary());
                }
                else
                {
                    Console.WriteLine($"Utilizador com ID {id} não encontrado.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        public void ReadAllUsers()
        {
            if(_loggedInUser!.Role != UserRole.Admin && _loggedInUser.Role != UserRole.Employee)
            {
                Console.WriteLine("Apenas Administradores e Funcionários podem listar todos os utilizadores.");
                Console.WriteLine("Prima qualquer tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n--- Listar todos as Utilizadores ---");
            try
            {
                List<User> users = _userService.ReadAll();
                if (users != null && users.Any()) // Melhor verificação para lista vazia
                {
                    foreach (User user in users)
                    {
                        Console.WriteLine(user.GetUserSummary());
                        Console.WriteLine("---------------------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("Não há utilizadores registados.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao listar vendas: {ex.Message}");
            }
        }

        public void UpdateUser()
        {
            if(_loggedInUser == null)
            {
                Console.WriteLine("Não há utilizador logado para atualizar.");
                Console.WriteLine("Prima qualquer tecla para continuar...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n--- Atualizar Utilizar ---");
            Console.Write("ID do Utilizador a Atualizar (0 para o seu próprio perfil): ");
            if (int.TryParse(Console.ReadLine(), out int idToUpdate))
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro positivo.");
                return;
            }

            User? userToUpdate = null;
            try
            {
                if (idToUpdate == 0 || idToUpdate == _loggedInUser.Id)
                {
                    userToUpdate = _loggedInUser;
                }
                else
                {
                    if (_loggedInUser.Role != UserRole.Admin)
                    {
                        Console.WriteLine("Não tem permissão para atualizar outros utilizadores.");
                        return;
                    }
                }
            }
            catch (InvalidOperationException ex) // Cliente não encontrado no serviço (via _userService.Read)
            {
                Console.WriteLine($"Erro ao obter utilizador para atualização: {ex.Message}");
                return;
            }
            catch (ArgumentException ex) // ID inválido passado ao serviço (via _userService.Read)
            {
                Console.WriteLine($"Erro ao obter utilizador para atualização: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao obter utilizador para atualização: {ex.Message}");
                return;
            }
            
            if (userToUpdate == null) // Pode ser nulo se _userService.Read retornar null e não lançar exceção
            {
                Console.WriteLine($"Utilizador com ID {idToUpdate} não encontrado.");
                return;
            }

            Console.WriteLine($"\nA atualizar Utilizador (ID: {userToUpdate.Id}: {userToUpdate.FirstName} {userToUpdate.LastName}");
            Console.WriteLine("Pressione Enter para manter o valor atual.");

            try
            {
                Console.Write($"Primeiro Nome ({userToUpdate.FirstName}): ");
                string firstName = Wrapper.ReadLineSafe();
                Console.Write($"Último Nome ({userToUpdate.LastName}): ");
                string lastName = Wrapper.ReadLineSafe();
                Console.Write($"Número de Telefone ({userToUpdate.PhoneNumber ?? "Não definido"}): ");
                string phoneNumber = Wrapper.ReadLineSafe();
                Console.Write($"Email ({userToUpdate.Email}): ");
                string email = Wrapper.ReadLineSafe();

                string newFirstName = string.IsNullOrWhiteSpace(firstName) ? userToUpdate.FirstName : firstName;
                string newLastName = string.IsNullOrWhiteSpace(lastName) ? userToUpdate.LastName : lastName;
                string? newPhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? userToUpdate.PhoneNumber : phoneNumber;
                string newEmail = string.IsNullOrWhiteSpace(email) ? userToUpdate.Email : email;

                userToUpdate.UpdatePersonalInformation(newFirstName, newLastName, newPhoneNumber!);
                userToUpdate.UpdateEmail(newEmail);

                Console.Write("Deseja atualizar o endereço? (S/N) ");
                if(Wrapper.ReadLineSafe().ToUpper() == "S")
                {
                    Address updatedAddress =  UpdateAddressInput(userToUpdate.Addresses, "do utilizador");
                    if(updatedAddress != null)
                    {
                        userToUpdate.UpdateAddress(updatedAddress);
                    }                    
                }

                if(_loggedInUser.Role == UserRole.Admin && idToUpdate != _loggedInUser.Id)
                {
                    Console.WriteLine($"Papel Atual ({userToUpdate.Role}). Selecionar Novo Papel (0 para manter)");
                    foreach(UserRole r in Enum.GetValues(typeof(UserRole)))
                    {
                        Console.WriteLine($"  {(int)r} - {r}");
                    }
                    Console.Write("Novo Papel: ");
                    string newRoleInput = Wrapper.ReadLineSafe();
                    if(!string.IsNullOrWhiteSpace(newRoleInput) && int.TryParse(newRoleInput, out int roleValue) && 
                        Enum.IsDefined(typeof(UserRole), roleValue))
                    {
                        userToUpdate.UpdateRole((UserRole)roleValue);
                    }
                    else if (!string.IsNullOrWhiteSpace(newRoleInput))
                    {
                        Console.WriteLine("Papel inválido. Mantendo o papel atual.");
                    }
                }

                _userService.Update(userToUpdate);
                Console.WriteLine("Utilizador atualizado com sucesso!");

                if(idToUpdate == _loggedInUser.Id)
                {
                    _loggedInUser = _userService.Read(_loggedInUser.Id);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação nos novos Utilizadores: {ex.Message}");
            }           
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado durante a atualização do utilizador: {ex.Message}");
            }
        }

        private void ChangeLoggedInUserPassword()
        {
            if(_loggedInUser == null)
            {
                Console.WriteLine("Não há utilizador logado para alterar password.");
                return;
            }

            Console.WriteLine("\n--- Alterar Password ---");
            Console.Write("Password Atual: ");
            string currentPassword = Wrapper.ReadLineSafe();

            if (!_loggedInUser.VerifyPassword(currentPassword))
            {
                Console.WriteLine("Password atual incorreta.");
                return;
            }

            Console.Write("Nova Password (min. 8 caracteres): ");
            string newPassword = Wrapper.ReadLineSafe();
            Console.Write("Confirme Nova Password: ");
            string confirmPassword = Wrapper.ReadLineSafe();
            
            if(newPassword != confirmPassword)
            {
                Console.WriteLine("As novas passwords não conicidem.");
                return;
            }

            try
            {
                _loggedInUser.UpdatePassword(newPassword);
                _userService.Update(_loggedInUser);
                Console.WriteLine("Password alterada com sucesso!");
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao alterar a password: {ex.Message}");
            }            
        }

        public void DeleteUser()
        {
            if(_loggedInUser == null || _loggedInUser.Role != UserRole.Admin)
            {
                Console.WriteLine("Apenas Administradores podem remover utilizadores.");
                return;
            }

            Console.WriteLine("\n--- Remover Utilizadores ---");
            Console.Write("ID do Utilizador a Remover: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (id <= 0)
                {
                    Console.WriteLine("ID inválido. Por favor, insira um número inteiro positivo");
                    return;
                }
                if(id == _loggedInUser.Id)
                {
                    Console.WriteLine("Não pode remover a sua própria conta enquanto estiver logado.");
                    return;
                }

                try
                {                   

                    bool deleted = _userService.Delete(id);
                    if (deleted)
                    {
                        Console.WriteLine($"Utilizador com ID {id} removida com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine($"Utilizador com ID {id} não encontrada ou não foi possível remover.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro ao remover utilizador: {ex.Message}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Erro de operação ao remover utilizador: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro inesperado ao remover Utilizador: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, insira um número inteiro.");
            }
        }

        private Address GetAddressInput(string addressType, bool isOptional = false)
        { 
            string street, street2, doorNumber, floor, postalCode, locate, city, country;
            Address? address = null;
            bool valid = false;

            while (!valid)
            {
                Console.Write($"Rua do Endereço {addressType}: ");
                street = Wrapper.ReadLineSafe();
                Console.Write($"Morada complementar do Endereço {addressType} (opcional): ");
                street2 = Wrapper.ReadLineSafe();
                Console.Write($"Número de Porta do Endereço {addressType}: ");
                doorNumber = Wrapper.ReadLineSafe();
                Console.Write($"Andar do Endereço {addressType} (opcional): ");
                floor = Wrapper.ReadLineSafe();
                Console.Write($"Código Postal do Endereço {addressType} (Ex: 1234-567): ");
                postalCode = Wrapper.ReadLineSafe();
                Console.Write($"Localidade do Endereço {addressType}: ");
                locate = Wrapper.ReadLineSafe();
                Console.Write($"Cidade do Endereço {addressType}: ");
                city = Wrapper.ReadLineSafe();
                Console.Write($"País do Endereço {addressType}: ");
                country = Wrapper.ReadLineSafe();
                
                if (isOptional && string.IsNullOrWhiteSpace(street) && string.IsNullOrWhiteSpace(street2) &&
                    string.IsNullOrWhiteSpace(doorNumber) && string.IsNullOrWhiteSpace(floor) &&
                    string.IsNullOrWhiteSpace(postalCode) && string.IsNullOrWhiteSpace(locate) &&
                    string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(country))
                {
                    return null!;
                }

                try
                {                    
                    address = new Address(street, street2, doorNumber, floor, postalCode, city, country, locate);
                    valid = true;
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine($"Erro no endereço {addressType}: {ex.Message}. Por favor, insira novamente.");
                }
            }
            return address!;
        }

        private Address UpdateAddressInput(Address currentAddress, string addressType)
        {
            Console.WriteLine($"\nA atualizar endereço {addressType} (atual: {currentAddress?.GetAddress() ?? "N/D"})");
            Console.WriteLine("Pressione Enter para manter o valor atual.");

            string currentStreet = currentAddress?.Street ?? "";
            string currentStreet2 = currentAddress?.Street2 ?? "";
            string currentDoorNumber = currentAddress?.DoorNumber ?? "";
            string currentFloor = currentAddress?.Floor ?? "";
            string currentPostalCode = currentAddress?.PostalCode ?? "";
            string currentLocate = currentAddress?.Locate ?? "";
            string currentCity = currentAddress?.City ?? "";
            string currentCountry = currentAddress?.Country ?? "";

            string newStreet, newStreet2, newDoorNumber, newFloor, newPostalCode, newLocate, newCity, newCountry;
            Address? updatedAddress = null;
            bool valid = false;

            while (!valid)
            {
                Console.Write($"Rua (atual: {currentStreet}): ");
                newStreet = Wrapper.ReadLineSafe();
                newStreet = string.IsNullOrWhiteSpace(newStreet) ? currentStreet : newStreet;

                Console.Write($"Morada complementar (atual: {currentStreet2}): ");
                newStreet2 = Wrapper.ReadLineSafe();
                newStreet2 = string.IsNullOrWhiteSpace(newStreet2) ? currentStreet2 : newStreet2;

                Console.Write($"Número de Porta (atual: {currentDoorNumber}): ");
                newDoorNumber = Wrapper.ReadLineSafe();
                newDoorNumber = string.IsNullOrWhiteSpace(newDoorNumber) ? currentDoorNumber : newDoorNumber;

                Console.Write($"Andar (atual: {currentFloor}): ");
                newFloor = Wrapper.ReadLineSafe();
                newFloor = string.IsNullOrWhiteSpace(newFloor) ? currentFloor : newFloor;

                Console.Write($"Código Postal (atual: {currentPostalCode}): ");
                newPostalCode = Wrapper.ReadLineSafe();
                newPostalCode = string.IsNullOrWhiteSpace(newPostalCode) ? currentPostalCode : newPostalCode;

                Console.Write($"Localidade (atual: {currentLocate}): ");
                newLocate = Wrapper.ReadLineSafe();
                newLocate = string.IsNullOrWhiteSpace(newLocate) ? currentLocate : newLocate;

                Console.Write($"Cidade (atual: {currentCity}): ");
                newCity = Wrapper.ReadLineSafe();
                newCity = string.IsNullOrWhiteSpace(newCity) ? currentCity : newCity;

                Console.Write($"País (atual: {currentCountry}): ");
                newCountry = Wrapper.ReadLineSafe();
                newCountry = string.IsNullOrWhiteSpace(newCountry) ? currentCountry : newCountry;

                try
                {
                    updatedAddress = new Address(newStreet, newStreet2, newDoorNumber, newFloor, newPostalCode, newCity, newCountry, newLocate);
                    valid = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro na atualização do endereço {addressType}: {ex.Message}. Por favor, insira novamente.");
                }
            }
            return updatedAddress!;
        }

        private Contact GetContactInput(string contactType)
        {
            string firstName, lastName, email, phone, jobTitle;
            Contact? contact = null;
            bool valid = false;

            while (!valid)
            {
                Console.Write($"Primeiro Nome do Contacto {contactType}: ");
                firstName = Wrapper.ReadLineSafe();
                Console.Write($"Último Nome do Contacto {contactType} (opcional): ");
                lastName = Wrapper.ReadLineSafe();
                Console.Write($"E-mail do contacto {contactType}: ");
                email = Wrapper.ReadLineSafe();
                Console.Write($"Telefone do Contacto {contactType}: ");
                phone = Wrapper.ReadLineSafe();
                Console.Write($"Título do Cargo do Contacto {contactType} (opcional): ");
                jobTitle = Wrapper.ReadLineSafe();

                try
                {
                    // Corrigido o construtor de Contact para corresponder à definição da classe Contact
                    // Contact(string firstName, string lastName, string email, string phoneNumber, string jobTitle)
                    contact = new Contact(0, firstName, lastName, email, phone, jobTitle);
                    valid = true;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro no contacto {contactType}: {ex.Message}. Por favor, insira novamente.");
                }
            }
            return contact!;
        }

        private Contact UpdateContactInput(Contact currentContact, string contactType)
        {
            
            string currentFirstName = currentContact?.FirstName ?? "";
            string currentLastName = currentContact?.LastName ?? "";
            string currentEmail = currentContact?.Email ?? "";
            string currentPhoneNumber = currentContact?.PhoneNumber ?? "";
            string currentJobTitle = currentContact?.JobTitle ?? "";

            Console.WriteLine($"\nAtualizando Contacto {contactType} (atual: {currentContact?.GetContactSummary() ?? "N/D"})");
            Console.WriteLine("Pressione Enter para manter o valor atual.");

            Console.Write($"Primeiro Nome do Contacto (atual: {currentFirstName}): ");
            string newFirstNameInput = Wrapper.ReadLineSafe();
            string newFirstName = string.IsNullOrWhiteSpace(newFirstNameInput) ? currentFirstName : newFirstNameInput;

            Console.Write($"Último Nome do Contacto (atual: {currentLastName}): ");
            string newLastNameInput = Wrapper.ReadLineSafe();
            string newLastName = string.IsNullOrWhiteSpace(newLastNameInput) ? currentLastName : newLastNameInput;

            Console.Write($"E-mail (atual: {currentEmail}): ");
            string newEmailInput = Wrapper.ReadLineSafe();
            string newEmail = string.IsNullOrWhiteSpace(newEmailInput) ? currentEmail : newEmailInput;

            Console.Write($"Telefone (atual: {currentPhoneNumber}): ");
            string newPhoneNumberInput = Wrapper.ReadLineSafe();
           
            string newPhoneNumber = string.IsNullOrWhiteSpace(newPhoneNumberInput) ? currentPhoneNumber : newPhoneNumberInput;

            Console.Write($"Título do Cargo (atual: {currentJobTitle}): "); // Corrigido para JobTitle
            string newJobTitleInput = Wrapper.ReadLineSafe();
            
            string newJobTitle = string.IsNullOrWhiteSpace(newJobTitleInput) ? currentJobTitle : newJobTitleInput;

            try
            {                
                return new Contact(newFirstName, newLastName, newEmail, newPhoneNumber, newJobTitle);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro ao criar/atualizar contacto {contactType}: {ex.Message}. Mantendo contacto atual.");
                return currentContact!; // Retorna o contacto atual se houver erro de validação com os novos dados
            }
        }
    }
}