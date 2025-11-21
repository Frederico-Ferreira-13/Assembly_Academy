using System;
using System.Text;

namespace ChallengeImpossible.Model
{
    public class Employee
    {
        public int Id { get; private set; }
        public string JobDescription { get; set; }
        public double Salary { get; set; }
        public string Department { get; set; }
        public string NIF { get; set; }
        public string NIB { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhoneNumber { get; set; }       

        public Employee(string jobDescription, double salary, string department, string nif, string nib, int age,
            DateTime birthday, string emergencyContactName, string emergencyContactPhoneNumber)
        {
            ValidateEmployee(jobDescription, salary, department, nif, nib, age, birthday, emergencyContactName, emergencyContactPhoneNumber);

            JobDescription = jobDescription;
            Salary = salary;
            Department = department;
            NIF = nif;
            NIB = nib;
            Age = age;
            Birthday = birthday;
            EmergencyContactName = emergencyContactName;
            EmergencyContactPhoneNumber = emergencyContactPhoneNumber;
        }

        private void ValidateEmployee(string jobDescription, double salary, string department, string nif, string nib, int age,
             DateTime birthday, string emergencyContactName, string emergencyContactPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(jobDescription))
            {
                throw new ArgumentException("A descrição do cargo é obrigatória.");
            }
            if (salary <= 0)
            {
                throw new ArgumentException("O salário deve ser um valor positivo.");
            }
            if (string.IsNullOrWhiteSpace(department))
            {
                throw new ArgumentException("O departamento é obrigatório.");
            }
            if (age <= 0)
            {
                throw new ArgumentException("A idade deve ser um valor positivo.");
            }
            if (string.IsNullOrWhiteSpace(nif) || nif.Length != 9 || !nif.All(char.IsDigit))
            {
                throw new ArgumentException("NIF inválido. Deve ter 9 dígitos.");
            }
            if (string.IsNullOrWhiteSpace(nib) || nib.Length != 21 || !nib.All(char.IsDigit))
            {
                throw new ArgumentException("NIB inválido. Deve ter 21 dígitos.");
            }
            if (string.IsNullOrWhiteSpace(emergencyContactPhoneNumber) || emergencyContactPhoneNumber.Length != 9 || !emergencyContactPhoneNumber.All(char.IsDigit))
            {
                throw new ArgumentException("Número de telefone de emergência inválido. Deve ter 9 dígitos.");
            }
        }

        public void Update(string jobDescription, double salary, string department, string nif, string nib, int age,
            DateTime birthday, string emergencyContactName, string emergencyContactPhoneNumber)
        {
            ValidateEmployee(jobDescription, salary, department, nif, nib, age, birthday, emergencyContactName, emergencyContactPhoneNumber);

            JobDescription = jobDescription;
            Salary = salary;
            Department = department;
            NIF = nif;
            NIB = nib;
            Age = age;
            Birthday = birthday;
            EmergencyContactName = emergencyContactName;
            EmergencyContactPhoneNumber = emergencyContactPhoneNumber;
        }

        public void SetId(int id)
        {
            if(Id != 0 && Id != id)
            {
                throw new InvalidOperationException("O ID do empregado já foi definido e não pode ser alterado.");
            }
            Id = id;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ID: {Id}");
            sb.AppendLine($"Cargo: {JobDescription}");
            sb.AppendLine($"Salário: {Salary:C}");
            sb.AppendLine($"Departamento: {Department}");
            sb.AppendLine($"NIF: {NIF}");
            sb.AppendLine($"NIB: {NIB}");
            sb.AppendLine($"Idade: {Age}");
            sb.AppendLine($"Aniversário: {Birthday:dd-MM-yyyy}");
            sb.AppendLine($"Contacto de Emergência: {EmergencyContactName} ({EmergencyContactPhoneNumber})");

            return sb.ToString().Trim();
        }        
    }
}

