using System;
using System.Text;

namespace ChallengeImpossible.Model
{
    public class Contact
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string JobTitle { get; private set; }
        

        public Contact(string firstName, string lastName, string email, string phoneNumber, string jobTitle)
        {
            ValidateContact(firstName, email, phoneNumber);           
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            JobTitle = jobTitle;
        }

        public Contact(int id, string firstName, string lastName, string email, string phoneNumber, string jobTitle) 
            : this(firstName, lastName, email, phoneNumber, jobTitle)
        {
            SetId(id);
        }       

        public void SetId(int id) 
        {
            if(Id != 0 && Id != id)
            {
                throw new InvalidOperationException("O ID do contacto já foi definido e não pode ser alterado.");
            }

            Id = id;
        }

        public void UpdateDetails(string firstName, string lastName, string email, 
            string phoneNumber, string jobTitle)
        {
            ValidateContact(firstName, email, phoneNumber);            
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            JobTitle = jobTitle;
        }

        private void ValidateContact(string firstName, string email, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("O contacto deve ter um Email ou um Número de Telefone.");
            }
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("O nome (FirstName) do contacto é obrigatório.");
            }
        }

        public override string ToString()
        {
            return GetContactSummary();            
        }
        
        public string GetContactSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{FirstName}");

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                sb.Append($" {LastName}");
            }
            if (!string.IsNullOrWhiteSpace(JobTitle))
            {
                sb.Append($" ({JobTitle})");
            }
            if (!string.IsNullOrWhiteSpace(Email))
            {
                sb.Append($" | Email: {Email}");
            }
            if (!string.IsNullOrWhiteSpace(PhoneNumber))
            {
                sb.Append($" | Tel: {PhoneNumber}");
            }
            return sb.ToString().Trim();
        }
    }
}

