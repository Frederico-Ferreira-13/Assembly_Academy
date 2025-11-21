using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Repository.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee Create(Employee employee);
        Employee? Read(int id);
        List<Employee> ReadAll();
        Employee Update(Employee employee);
        bool Delete(int id);
    }
}
