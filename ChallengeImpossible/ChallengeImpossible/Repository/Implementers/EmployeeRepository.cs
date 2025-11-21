using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public static List<Employee> _employeeList = new List<Employee>();

        public int _idCounters = 1;

        public Employee Create(Employee employee)
        {
            employee.SetId(_idCounters++);
            _employeeList.Add(employee);
            return employee;
        }

        public Employee? Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.", nameof(id));
            }
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public List<Employee> ReadAll()
        {
            return _employeeList.ToList();
        }

        public Employee Update(Employee employee)
        {
            if (employee == null || employee.Id <= 0)
            {
                throw new ArgumentException("O empregado a atualizar ou o seu ID são inválidos.");
            }            

            Employee? existingEmployee = _employeeList.FirstOrDefault(e => e.Id == employee.Id);
            if (existingEmployee != null)
            {              
                return existingEmployee;
            }
            return null!;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.");
            }
            return _employeeList.RemoveAll(cl => cl.Id == id) > 0;
        }
    }
}
