using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ChallengeImpossible.Model;
using ChallengeImpossible.Services.Interfaces;


namespace ChallengeImpossible.Program
{
    internal class EmployeeProgram
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeProgram(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public void EmployeeManagement()
        {
            int employeeOption;
            do
            {
                Console.WriteLine("\n--- Gestão de Empregados ---");
                Console.WriteLine(" 1 - Adicionar Empregado");
                Console.WriteLine(" 2 - Obter Empregado por ID");
                Console.WriteLine(" 3 - Obter todos os Empregados");
                Console.WriteLine(" 4 - Atualizar Empregado");
                Console.WriteLine(" 5 - Remover Empregado");
                Console.WriteLine(" 0 - Voltar ao menu principal");
                Console.Write("Opção: ");

                if (int.TryParse(Console.ReadLine(), out employeeOption))
                {
                    switch (employeeOption)
                    {
                        case 1:
                            CreateEmployee();
                            break;
                        case 2:
                            ReadEmployee();
                            break;
                        case 3:
                            ReadAllEmployees();
                            break;
                        case 4:
                            UpdateEmployee();
                            break;
                        case 5:
                            DeleteEmployee();
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
                    employeeOption = -1; // Garante que o loop continua
                }
            } while (employeeOption != 0);
        }

        public void CreateEmployee()
        {
            Console.WriteLine("\n--- Adicionar Empregado ---");          

            try
            {
                string jobDescription = ReadInput("Descrição do Cargo");
                double salary = ReadDouble("Salário");
                string department = ReadInput("Departamento");
                string nif = ReadInputWithValidation("NIF", 9, "9 dígitos");
                string nib = ReadInputWithValidation("NIB", 21, "21 dígitos");
                int age = ReadInt("Idade");
                DateTime birthday = ReadDate("Aniversário");
                string emergencyContactName = ReadInput("Nome do contacto de emergência");
                string emergencyContactPhoneNumber = ReadInputWithValidation("Número de telefone de emergência", 9, "9 dígitos");
            
                Employee newEmployee = new Employee(jobDescription, salary, department, nif, nib, age,
                    birthday, emergencyContactName, emergencyContactPhoneNumber);

                _employeeService.Create(newEmployee);
                Console.WriteLine($"Empregado adicionado com sucesso. (ID: {newEmployee.Id})");
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine($"Erro de validação: {ex.Message}");
            }           
            catch(Exception ex)
            {
                Console.WriteLine($"Ocureu um erro inesperado ao adicionar o empregado: {ex.Message}");
            }            
        }               

        public void ReadEmployee()
        {
            Console.WriteLine("\n--- Obter Empregado por ID ---");
            int id = ReadInt("ID do Empregado: ");           
            
            try
            {
                Employee employee = _employeeService.Read(id);
                Console.WriteLine($"\n--- Detalhes do Empregado (ID: {employee.Id}) ---");
                Console.WriteLine(employee.ToString());
            }           
            catch(Exception ex)
            {
                Console.WriteLine($"Ocurreu um erro inesperado ao obter empregado: {ex.Message}");
            }          
        }

        public void ReadAllEmployees()
        {
            Console.WriteLine("\n--- Listar todos os Empregados ---");
            List<Employee> employees = _employeeService.ReadAll();
            if (employees != null && employees.Any()) // Melhor verificação para lista vazia
            {
                foreach (Employee employee in employees)
                {
                    Console.WriteLine($"\n--- Client ID: {employee.Id} ---");
                    Console.WriteLine(employee.ToString());
                }
            }
            else
            {
                Console.WriteLine("Não há Empregados registados.");
            }
        }

        public void UpdateEmployee()
        {
            Console.WriteLine("\n--- Atualizar Empregado ---");
            int id = ReadInt("ID do Empregado a Atualizar: ");
            try
            {
                Employee existingEmployee = _employeeService.Read(id);
                Console.WriteLine($"\nA atualizar Empregado com ID: {existingEmployee.Id}");
                Console.WriteLine("Pressione Enter para manter o valor atual.");

                string newJobDescription = ReadInput("Descrição do Cargo", existingEmployee.JobDescription);
                double newSalary = ReadDouble("Salário", existingEmployee.Salary);
                string newDepartment = ReadInput("Departamento", existingEmployee.Department);
                string newNif = ReadInputWithValidation("NIF", 9, "9 dígitos", existingEmployee.NIF);
                string newNib = ReadInputWithValidation("NIB", 21, "21 dígitos", existingEmployee.NIB);
                int newAge = ReadInt("Idade", existingEmployee.Age);
                DateTime newBirthday = ReadDate("Aniversário", existingEmployee.Birthday);
                string newEmergencyContactName = ReadInput("Nome do contacto de emergência", existingEmployee.EmergencyContactName);
                string newEmergencyContactPhoneNumber = ReadInputWithValidation("Número de telefone de emergência", 9, "9 dígitos", existingEmployee.EmergencyContactPhoneNumber);

                existingEmployee.Update(
                    newJobDescription, newSalary, newDepartment, newNif, newNib, newAge,
                    newBirthday, newEmergencyContactName, newEmergencyContactPhoneNumber);

                _employeeService.Update(existingEmployee);
                Console.WriteLine("Empregado atualizado com sucesso.");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ocurreu um erro durante a atualização: {ex.Message}");
            }             
        }

        public void DeleteEmployee()
        {
            Console.WriteLine("\n--- Remover Empregado ---");
            int id = ReadInt("ID do Empregado a Remover");
           
            try
            {
                bool deleted = _employeeService.Delete(id);
                if (deleted)
                {
                    Console.WriteLine($"Empregado com ID {id} removido com sucesso!");
                }
                else
                {
                    Console.WriteLine($"Empregado com ID {id} não foi encontrado");
                }
            }            
            catch(Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado ao remover empregado: {ex.Message}");
            }
            
        }

        private string ReadInput(string prompt, string currentValue = null)
        {
            Console.Write($"{prompt} (atual: {currentValue}): ");
            string input = Wrapper.ReadLineSafe();
            return string.IsNullOrWhiteSpace(input) ? currentValue : input;
        }

        private int ReadInt(string prompt, int? currentValue = null)
        {
            int result;
            string input = ReadInput(prompt, currentValue.HasValue ? currentValue.ToString() : "");
            while (!int.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine($"Entrada inválida. Por favor, insira um número inteiro positivo.");
                input = ReadInput(prompt, currentValue.HasValue ? currentValue.ToString() : "");
            }
            return result;
        }

        private double ReadDouble(string prompt, double? currentValue = null)
        {
            double result;
            string input = ReadInput(prompt, currentValue.HasValue ? currentValue.ToString() : "");
            while (!double.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine($"Entrada inválida. Por favor, insira um número inteiro positivo.");
                input = ReadInput(prompt, currentValue.HasValue ? currentValue.ToString() : "");
            }
            return result;
        }

        private DateTime ReadDate(string prompt, DateTime? currentValue = null)
        {
            DateTime result;
            string input = ReadInput(prompt, currentValue.HasValue ? currentValue.ToString() : "");
            while (!DateTime.TryParseExact(input, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out result))
            {
                Console.WriteLine($"Formato de data inválido. Por favor, insira a data no formato DD-MM-AAAA.");
                input = ReadInput(prompt, currentValue.HasValue ? currentValue.Value.ToString("dd-MM-yyyy") : "");
            }
            return result;
        }

        private string ReadInputWithValidation(string prompt, int expectedLength, string lengthDescription, string currentValue = null)
        {
            string input;
            bool isValid;
            do
            {
                Console.Write($"{prompt} (atual: {currentValue}): ");
                input = Wrapper.ReadLineSafe();
                if (string.IsNullOrWhiteSpace(input))
                {
                    if (currentValue != null) return currentValue;
                    isValid = false;
                }
                else
                {
                    isValid = input.Length == expectedLength && input.All(char.IsDigit);
                }

                if (!isValid)
                {
                    Console.WriteLine($"{prompt} inválido. Deve ter {lengthDescription} e conter apenas dígitos.");
                }

            } while (!isValid);
            return input;
        }
    }
}
