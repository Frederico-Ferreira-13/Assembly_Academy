using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Services.Implementers
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Employee Create(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "O empregado não pode ser nulo.");
            }

            if (employee.Id != 0)
            {
                throw new InvalidOperationException("Não é possível criar um empregado com um ID pré-definido. O ID é atribuído pelo repositório.");
            }

            return _employeeRepository.Create(employee);
        }

        public Employee Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID do empregado deve ser um número posítivo.", nameof(id));
            }

            Employee? employee = _employeeRepository.Read(id);
            if(employee == null)
            {
                throw new InvalidOperationException($"Empregado com ID {id} não encontrado.");
            }
            return employee;
        }

        public List<Employee> ReadAll()
        {
            return _employeeRepository.ReadAll();
        }

        public Employee Update(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "O empregado a ser atualizado não pode ser nulo.");
            }
            if (employee.Id <= 0)
            {
                throw new ArgumentException("O ID do empregado a ser atualizado deve ser um número positivo.", nameof(employee.Id));
            }

            Employee? existingEmployee = _employeeRepository.Read(employee.Id);
            if(existingEmployee == null)
            {
                throw new InvalidOperationException($"Não foi possível atualizar: Emopregado com {employee.Id}");
            }

            return _employeeRepository.Update(employee);
        }

        public bool Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo", nameof(id));
            }

            Employee? employeeToDelete = _employeeRepository.Read(id);
            if(employeeToDelete == null)
            {
                return false;
            }
            return _employeeRepository.Delete(id);            
        }
    }
}
