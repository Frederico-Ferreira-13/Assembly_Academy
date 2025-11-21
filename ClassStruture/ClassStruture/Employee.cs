using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassStruture
{
    public class Employee
    {
        public int Id { get; set; }
        public string JobDescription { get; set; }
        public double Salary { get; set; }
        public string Department { get; set; }
        public int NIF { get; set; }
        public int NIB { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public string EmergencyContactName { get; set; }
        public int EmergencyContactPhoneNumber { get; set; }
        public Employee Employees { get; set; }
        

        public void AddEmplyee(Employee employees)
        {
            Employees = employees;
        }
        public void UpdateSalary(double newSalary)
        {
            Salary = newSalary;
        }
        public void UpdateDepartment(string newDepartment)
        {
            Department = newDepartment;
        }
        public void UpdateJobRole(string newJobDescription)
        {
            JobDescription = newJobDescription;
        }
        public void UpdateEmplyeeDetails(double salary, int nif, DateTime birthday, string emergencyContactName, int nib, int age, int emergencyContactPhoneNumber)
        {
            Salary = salary;
            NIF = nif;
            Birthday = birthday = DateTime.Now;
            NIB = nib;
            EmergencyContactName = emergencyContactName;
            Age = age;
            EmergencyContactPhoneNumber = emergencyContactPhoneNumber;
            Department = Department;
        }
    }
}
