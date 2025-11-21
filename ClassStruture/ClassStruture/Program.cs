using System.ComponentModel.Design;
using System.Runtime.Intrinsics.X86;

namespace ClassStruture
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();
            List<Provider> providers = new List<Provider>();
            List<Client> clients = new List<Client>();
            List<Address> addresses = new List<Address>();
            List<Person> persons = new List<Person>();
            List<ProviderContact> providerContacts = new List<ProviderContact>();

            while (true)
            {
                Console.WriteLine("Select an option: ");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Add Provider");
                Console.WriteLine("3. Add Client");
                Console.WriteLine("4. View Employees");
                Console.WriteLine("5. View Providers");
                Console.WriteLine("6. View Clients");
                Console.WriteLine("7. Add Address to Existing Client");
                Console.WriteLine("8. Update Address of Existing Client");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEmployee(employees);
                        break;
                    case "2":
                        AddProvider(providers, addresses);
                        break;
                    case "3":
                        AddClient(clients, addresses);
                        break;
                    case "4":
                        ViewEmployees(employees);
                        break;
                    case "5":
                        ViewProviders(providers);
                        break;
                    case "6":
                        ViewClients(clients);
                        break;
                    case "7":
                        AddAdressToClient(clients, addresses);
                        break;
                    case "8":
                        UpdateClientAddress(clients, addresses);
                        break;
                    case "9":
                        Console.WriteLine("Exiting application.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;

                }
            }
        }

        
        static void AddEmployee(List<Employee> employees)
        {
            Employee newEmployee = new Employee();
            Console.Write("Enter Employe ID: ");
            if(int.TryParse(Console.ReadLine(), out int id))
            {
                newEmployee.Id = id;
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            Console.WriteLine("Enter Job Description: ");
            newEmployee.JobDescription = Console.ReadLine();

            Console.WriteLine("Enter Salary: ");
            if(double.TryParse(Console.ReadLine(), out double salary))
            {
                newEmployee.Salary = salary;
            }
            else
            {
                Console.WriteLine("Invalid salarty format.");
                return;
            }

            Console.WriteLine("Enter Department: ");
            newEmployee.Department = Console.ReadLine();

            Console.Write("Enter NIF: ");
            if(int.TryParse(Console.ReadLine(), out int nif))
            {
                newEmployee.NIF = nif;
            }
            else
            {
                Console.WriteLine("Invalid NIF format.");
                return;
            }

            Console.Write("Enter NIB: ");
            if(int.TryParse(Console.ReadLine(), out int nib))
            {
                newEmployee.NIB = nib;
            }
            else
            {
                Console.WriteLine("Invalid NIB format.");
                return;
            }

            Console.Write("Enter Age: ");
            if(int.TryParse(Console.ReadLine(), out int age))
            {
                newEmployee.Age = age;
            }
            else
            {
                Console.WriteLine("Invalid age format.");
                return;
            }

            Console.Write("Enter Birthday (yyyy-mm-dd): ");
            if(DateTime.TryParse(Console.ReadLine(), out DateTime birthday))
            {
                newEmployee.Birthday = birthday;
            }
            else
            {
                Console.WriteLine("Invalid Birthday format.");
                return;
            }

            Console.Write("Enter Emergency Contact Name: ");
            newEmployee.EmergencyContactName = Console.ReadLine();

            Console.Write("Enter Emergency Coontact Phone Number: ");
            if(int.TryParse(Console.ReadLine(), out int emergencyContactPhoneNumber))
            {
                newEmployee.EmergencyContactPhoneNumber = emergencyContactPhoneNumber;
            }
            else
            {
                Console.WriteLine("Invalid phone number format.");
                return;
            }

            employees.Add(newEmployee);
            Console.WriteLine("Employee added sucessfully!");
        }

        static void AddProvider(List<Provider> providers, List<Address> addresses)
        {
            Provider newProvider = new Provider();
            Console.Write("Enter Provider ID: ");
            if(int.TryParse(Console.ReadLine(), out int id))
            {
                newProvider.ID = id;
            }
            else
            {
                Console.WriteLine("Inválid ID format.");
                return;
            }

            Console.WriteLine("Enter Provider Name: ");
            newProvider.Name = Console.ReadLine();

            Address providerAddress = CreateAddress();
            if(providerAddress != null)
            {
                newProvider.Address = providerAddress;
                addresses.Add(providerAddress);
            }
            else
            {
                return;
            }

            Console.Write("Enter Contact Person Name: ");
            newProvider.ContactPersonName = Console.ReadLine();

            Console.Write("Enter Contact Person Job Title: ");
            newProvider.PersonJobTitle = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            if(int.TryParse(Console.ReadLine(), out int phoneNumber))
            {
                newProvider.PhoneNumber = phoneNumber;
            }
            else
            {
                Console.WriteLine("Invalid phone number format.");
                return;
            }

            Console.Write("Enter Contact Person Email: ");
            newProvider.ContactPersonEmail = Console.ReadLine();

            Console.Write("Enter Sector: ");
            newProvider.Sector = Console.ReadLine();

            providers.Add(newProvider);
            Console.WriteLine("Provider added successfully!");
        }

        static void AddClient(List<Client> clients, List<Address> addresses)
        {
            Client newCLient = new Client();
            Console.Write("Enter Client ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                newCLient.ID = id;
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            Console.Write("Enter Client Name: ");
            newCLient.Name = Console.ReadLine();

            Address clientAddress = CreateAddress();
            if(clientAddress != null)
            {
                newCLient.Address = clientAddress;
                addresses.Add(clientAddress);
            }
            else
            {
                return;
            }

            Console.Write("Enter Sector: ");
            newCLient.Sector = Console.ReadLine();

            Console.Write("Enter NIF: ");
            newCLient.NIF = Console.ReadLine();

            Console.Write("Enter Delivery Address: ");
            newCLient.DeliveryAddress = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            newCLient.PhoneNumber = Console.ReadLine();

            Console.Write("Enter Email: ");
            newCLient.Email = Console.ReadLine();

            clients.Add(newCLient);
            Console.WriteLine("Client addedd successfully!");
        }

        static Address CreateAddress()
        {
            Address newAddress = new Address();
            Console.WriteLine("\nEnter Address Details: ");
            Console.Write("Street: ");
            string street = Console.ReadLine();
            Console.Write("Strest 2 (optional): ");
            string street2 = Console.ReadLine();
            Console.Write("Door Number: ");
            if(!int.TryParse(Console.ReadLine(), out int doorNumber) || doorNumber <= 0)
            {
                Console.WriteLine("Invalid door number: ");
                return null;
            }
            Console.Write("Floor (0 if ground floor): ");
            if(!int.TryParse(Console.ReadLine(), out int floor))
            {
                Console.WriteLine("Invalid floor format.");
                return null;
            }
            Console.Write("Postal Code: ");
            if(!int.TryParse(Console.ReadLine(), out int postalCode) || postalCode <= 0)
            {
                Console.WriteLine("Invalid postal code.");
                return null;
            }
            Console.Write("Locate: ");
            string locate = Console.ReadLine();
            Console.Write("City: ");
            string city = Console.ReadLine();
            Console.Write("Country: ");
            string country = Console.ReadLine();

            if(newAddress.SetAddress(street, street2, doorNumber, floor, postalCode, locate, city, country))
            {
                return newAddress;
            }
            return null;
        }

        static void ViewEmployees(List<Employee> employees)
        {
            Console.WriteLine("\n--- Employees ---");
            if(employees.Count == 0)
            {
                Console.WriteLine("No employyes added yet.");
                return;
            }
            for(int i = 0; i < employees.Count; i++)
            {
                Console.WriteLine($"ID: {employees[i].Id}, Job: {employees[i].JobDescription}, Salary: {employees[i].Salary}, Department: {employees[i].Department}");

            }
        }

        static void ViewProviders(List<Provider> providers)
        {
            Console.WriteLine("\n--- Providers ---");
            if(providers.Count == 0)
            {
                Console.WriteLine("No providers added yet.");
                return;
            }
            for(int i = 0; i <providers.Count; i++)
            {
                Provider prov = providers[i];
                Console.WriteLine($"ID: {prov.ID}, Name: {prov.Name}, Contact: {prov.ContactPersonName}({prov.PersonJobTitle}), Sector: {prov.Sector}");
                if(prov.Address != null)
                {
                    Console.WriteLine($"  Address: {prov.Address.GetAddress()}");
                }
            }
        }

        static void ViewClients(List<Client> clients)
        {
            Console.WriteLine("\n--- Clients ---");
            if(clients.Count == 0)
            {
                Console.WriteLine("No clients added yet.");
                return;
            }
            for(int i = 0; i < clients.Count; i++)
            {
                Client cli = clients[i];
                Console.WriteLine($"ID: {cli.ID}, Name: {cli.Name}, Sector: {cli.Sector}, NIF: {cli.NIF}, Phone: {cli.PhoneNumber}, Email: {cli.Email}");
                if(cli.Address != null)
                {
                    Console.WriteLine($" Address: {cli.Address.GetAddress()}");
                }
                if (!string.IsNullOrEmpty(cli.DeliveryAddress))
                {
                    Console.WriteLine($"Delivery Address: {cli.DeliveryAddress}");
                }
            }
        }

        static void AddAdressToClient(List<Client> clients, List<Address> addresses)
        {
            Console.Write("Enter the ID of the client to add an address to: ");
            if(int.TryParse(Console.ReadLine(), out int clientId))
            {
                Client clientToUpdate = clients.FirstOrDefault(c => c.ID == clientId);
                if(clientToUpdate != null)
                {
                    if(clientToUpdate.Address == null)
                    {
                        Address newAddress = CreateAddress();
                        
                        if(newAddress != null)
                        {
                            clientToUpdate.AddClientAddress(newAddress);
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine($"Client {clientToUpdate.Name} already has an address.");
                    }
                }
                else
                {
                    Console.WriteLine($"Client with ID {clientId} not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
            
        } 
        static void UpdateClientAddress(List<Client> clients, List<Address> addresses)
        {
            Console.Write("Enter the ID of the client to update the address for: ");
            if(int.TryParse(Console.ReadLine(), out int clientId))
            {
                Client clientToUpdate = clients.FirstOrDefault(c => c.ID == clientId);
                if(clientToUpdate != null)
                {
                    if(clientToUpdate.Address != null)
                    {
                        Address updatedAddress = CreateAddress();
                        if(updatedAddress != null)
                        {
                            clientToUpdate.UpdateClientAddress(updatedAddress);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Client {clientToUpdate.Name} does not have an address yet.");
                    }
                }
                else
                {
                    Console.WriteLine($"CLient with ID {clientId} not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }

    }
}
