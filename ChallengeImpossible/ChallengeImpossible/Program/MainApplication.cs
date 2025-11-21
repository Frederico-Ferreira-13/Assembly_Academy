using System;
using System.Text; // Para Console.OutputEncoding
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Implementers;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Implementers;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Program
{
    internal class MainApplication
    {
        private readonly UserProgram _userProgram;
        private readonly ClientProgram _clientProgram;
        private readonly SalesCarProgram _salesCarProgram;
        private readonly AddressProgram _addressProgram;
        private readonly CarProgram _carProgram;
        private readonly ContactProgram _contactProgram;
        private readonly EmployeeProgram _employeeProgram;
        private readonly ProviderProgram _providerProgram;
        private readonly ProviderContactProgram _providerContactProgram;

        private readonly IUserService _userService;
        
        public MainApplication()
        {
            IAddressRepository addressRepository = new AddressRepository();
            IAddressService addressService = new AddressService(addressRepository);

            ICarRepository carRepository = new CarRepository();
            ICarService carService = new CarService(carRepository);

            IClientRepository clientRepository = new ClientRepository();
            IClientService clientService = new ClientService(clientRepository);

            IContactRepository contactRepository = new ContactRepository();
            IContactService contactService = new ContactService(contactRepository);

            IEmployeeRepository employeeRepository = new EmployeeRepository();
            IEmployeeService employeeService = new EmployeeService(employeeRepository);

            IProviderContactRepository providerContactRepository = new ProviderContactRepository();
            IProviderContactService providerContactService = new ProviderContactService(providerContactRepository);

            IProviderRepository providerRepository = new ProviderRepository();
            IProviderService providerService = new ProviderService(providerRepository);

            ISalesCarRepository salesCarRepository = new SalesCarRepository(carRepository);
            ISalesCarService salesCarService = new SalesCarService(salesCarRepository, clientRepository, carRepository);

            IUserRepository userRepository = new UserRepository();
            IUserService userService = new UserService(userRepository);

            _userService = userService;

            _addressProgram = new AddressProgram(addressService);
            _carProgram = new CarProgram(carService);
            _clientProgram = new ClientProgram(clientService);
            _contactProgram = new ContactProgram(contactService);
            _employeeProgram = new EmployeeProgram(employeeService);
            _providerProgram = new ProviderProgram(providerService, providerContactService);
            _providerContactProgram = new ProviderContactProgram(providerContactService);
            _salesCarProgram = new SalesCarProgram(salesCarService, clientService, carService);
            _userProgram = new UserProgram(_userService);

            SeedAllData(userService, carService, clientService, contactService);
        }

        private void SeedAllData(IUserService userService, ICarService carService, IClientService clientService, IContactService contactService)
        {
            Console.WriteLine("A popular dados iniciais...");


            Address addr1 = new Address("Rua do Teste", "", "10", "1º Esq", "1000-001", "Lisboa", "Lisboa", "Portugal");
            Address addr2 = new Address("Avenida Central", "Loja 5", "20", "", "2000-002", "Porto", "Porto", "Portugal");
            Address addr3 = new Address("Travessa da Paz", "", "30", "R/C Dr", "3000-003", "Coimbra", "Coimbra", "Portugal");

            Contact contact1 = new Contact(
                "João",
                "Pereira",
                "joao.pereira@example.com",
                "911222333",
                "Gestor de Clientes"
            );

            Contact contact2 = new Contact(
                "Ana",
                "Santos",
                "ana.santos@example.com",
                "933444555",
                "Especialista de Vendas"
            );

            
            try
            {
                User adminUser = new User(0, UserRole.Admin, "Admin", "Geral", "910000000", "admin@email.com", addr1, "admin123");
                userService.Create(adminUser);

                User employeeUser = new User(0, UserRole.Employee, "Funcionario", "Silva", "920000000", "funcionario@email.com", addr2, "emp123");
                userService.Create(employeeUser);

                User clientUser = new User(0, UserRole.Client, "Cliente", "Sousa", "930000000", "cliente@email.com", addr3, "client123");
                userService.Create(clientUser);

                Console.WriteLine("Utilizadores iniciais criados.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro ao criar utilizador de teste: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao popular utilizadores: {ex.Message}");
            }

            try
            {
                Car car1 = new Car("Corolla", "Toyota", "Gasolina", "Prata", 1.8, 1200.0, 15000.0);
                carService.Create(car1);

                Car car2 = new Car("Civic", "Honda", "Híbrido", "Azul", 1.5, 1350.0, 22000.0);
                carService.Create(car2);

                Car car3 = new Car("Classe C", "Mercedes-Benz", "Diesel", "Preto", 2.0, 1600.0, 45000.0);
                carService.Create(car3);

                Console.WriteLine("Carros iniciais criados.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro ao criar carro de teste: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao popular carros: {ex.Message}");
            }

            try
            {
                Client cli1 = new Client(
                    "Joao Pereira",
                    addr1,
                    "Varejo",
                    "123456789",
                    addr1,
                    contact1
                );
                clientService.Create(cli1);

                Client cli2 = new Client(
                    "Ana Santos",
                    addr2,
                    "Atacado",
                    "987654321",
                    addr2,
                    contact2
                );
                clientService.Create(cli2);

                Console.WriteLine("Clientes iniciais criados.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro ao criar cliente de teste: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao popular clientes: {ex.Message}");
            }

            Console.WriteLine("População de dados iniciais concluída.");
        }

        private void RegisterNewUser()
        {
            Console.WriteLine("\n--- Registo do Novo Utilizador ---");
            Console.Write("Primeiro Nome: ");
            string firstName = Wrapper.ReadLineSafe();
            Console.Write("Sobrenome: ");
            string lastName = Wrapper.ReadLineSafe();
            Console.Write("Contato Telefónico: ");
            string phoneNumber = Wrapper.ReadLineSafe();
            Console.Write("E-mail: ");
            string email = Wrapper.ReadLineSafe();
            Console.Write("Passowrd (8 caracteres minimo): ");
            string password = Wrapper.ReadLineSafe();

            Console.WriteLine("\n--- Endereço (morada) ---");
            Console.Write("Rua: ");
            string street = Wrapper.ReadLineSafe();
            Console.Write("Número: ");
            string doorNumber = Wrapper.ReadLineSafe();
            Console.Write("Andar: ");
            string floor = Wrapper.ReadLineSafe();
            Console.Write("Código Postal (ex: 1234-567): ");
            string postalCode = Wrapper.ReadLineSafe();
            Console.Write("Localidade: ");
            string locate = Wrapper.ReadLineSafe();
            Console.Write("Cidade: ");
            string city = Wrapper.ReadLineSafe();
            Console.Write("Destrito: ");
            string disctrit = Wrapper.ReadLineSafe();
            string country = "Portugal";

            Address? newAddress = null;
            try
            {
                newAddress = new Address(street, doorNumber, floor, postalCode, locate, city, country, disctrit);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine($"Erro ao criar endereço: {ex.Message}. Por favor tente novamente.");
            }

            UserRole desiredRole = UserRole.Client;
            Console.WriteLine($"A conta será registada como {desiredRole.ToString()}.");

            try
            {
                User registeredUser = _userService.RegisterUser(firstName, lastName, phoneNumber, email, password, newAddress!, desiredRole);
                Console.WriteLine($"\nSUCESSO: Utilizador '{registeredUser.Email}' registado com sucesso como {registeredUser.Role.ToString()}! ID: {registeredUser.Id}");
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine($"\nErro de registo: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"\nErro inesperado ao registar: {ex.Message}");
            }
            Console.WriteLine("\nPrima qualquer tecla para continuar...");
            Console.ReadKey();
        }

        public void Run()
        {

            int option = 0;

            do
            {

                Console.WriteLine("\n**************************************");
                Console.WriteLine("***** Sistema de Gestão de Vendas *****");
                Console.WriteLine("**************************************");
                Console.WriteLine(" 1 - Login");
                Console.WriteLine(" 2 - Registar Nova Conta");                 
                Console.WriteLine(" 3 - Gestão de Carros");
                Console.WriteLine(" 4 - Gestão de Clientes");
                Console.WriteLine(" 5 - Gestão de Vendas de Carros");
                Console.WriteLine(" 6 - Gestão de Endereços");
                Console.WriteLine(" 7 - Gestão de Contactos");
                Console.WriteLine(" 8 - Gestão de Funcionários");
                Console.WriteLine(" 9 - Gestão de Fornecedores");
                Console.WriteLine(" 10 - Gestão de Contactos de Fornecedores");
                Console.WriteLine(" 0 - Sair");
                Console.Write("Escolha uma opção: ");

                if (int.TryParse(Console.ReadLine(), out option))
                {
                    switch (option)
                    {
                        case 1:
                            _userProgram.UserManagement();
                            break;
                        case 2:
                            RegisterNewUser();
                            break;
                        case 3:
                            _carProgram.CarManagement();
                            break;
                        case 4:
                            _clientProgram.ClientManagement();
                            break;
                        case 5:
                            _salesCarProgram.SalesCarManagement();
                            break;
                        case 6:
                            _addressProgram.AddressManagement();
                            break;
                        case 7:
                            _contactProgram.ContactManagement();
                            break;
                        case 8:
                            _employeeProgram.EmployeeManagement();
                            break;
                        case 9:
                            _providerProgram.ProviderManagement();
                            break;
                        case 10:
                            _providerContactProgram.ProviderContactManagement();
                            break;
                        default:
                            Console.WriteLine($"A opção {option} não existe, tente novamente.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Por favor, introduza um número inteiro como opção.");
                    option = -1;
                }
            } while (option != 0);
            Console.WriteLine("Obrigado por usar o nosso software.");
        }

        public static void Main(string[] args)
        {
            MainApplication app = new MainApplication();
            app.Run();
        }
    }
}
