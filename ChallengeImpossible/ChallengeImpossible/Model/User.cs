using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChallengeImpossible.Services;

namespace ChallengeImpossible.Model
{
    public class User
    {
        public int Id { get; private set; }
        public UserRole Role { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }
        public Address Addresses { get; private set; }

        private string _passwordHash;
        private readonly List<SalesCar> _purachaseHistory = new List<SalesCar>();

        public IReadOnlyList<SalesCar> PurchaseHistory => _purachaseHistory;

        public User(int id, UserRole role, string firstName, string lastName, string phoneNumber,
            string email, Address addresses, string plainTextPassword)
        {
            ValidateUser(id, firstName, lastName, phoneNumber, email, addresses, plainTextPassword);

            Id = id;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Addresses = addresses;
            
            _passwordHash = SecurePasswordHasher.Hash(plainTextPassword);
        }

        private void ValidateUser(int id, string firstName, string lastName, string phoneNumber, string email,
            Address addresses, string plainTextPassword)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID do utilizador deve ser um número positivo.", nameof(id));
            }
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("O primeiro nome do utilizador é obrigatório.", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("O último nome do utilizador é obrigatório.", nameof(lastName));
            }
            if (string.IsNullOrWhiteSpace(email) || !ValidationHelper.IsValidEmail(email))
            {
                throw new ArgumentException("O endereço de email é obrigatório e deve ser válido.", nameof(email));
            }
            if (addresses == null)
            {
                throw new ArgumentNullException(nameof(addresses), "O endereço do utilizador é obrigatório.");
            }
            if (string.IsNullOrWhiteSpace(plainTextPassword) || plainTextPassword.Length < 8)
            {
                throw new ArgumentException("A password é obrigatória e deve ter pelo menos 8 caracteres.", nameof(plainTextPassword));
            }
        }
        
        internal void SetId(int id)
        {
            if(id <= 0)
            {                
               throw new ArgumentException("O ID deve ser um número inteiro positivo.", nameof(id));               
            }

            Id = id;
        }

        public bool VerifyPassword(string plainTextPassword)
        {
            return SecurePasswordHasher.Verify(plainTextPassword, _passwordHash);
        }

        public void UpdatePersonalInformation(string newFirstName, string newLastName, 
            string newPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(newFirstName))
            {
                throw new ArgumentException("O primeiro nome não pode ser vazio.", nameof(newFirstName));
            }
            if (string.IsNullOrWhiteSpace(newLastName))
            {
                throw new ArgumentException("O último nome não pode ser vazio.", nameof(newLastName));
            }

            FirstName = newFirstName;
            LastName = newLastName;
            PhoneNumber = newPhoneNumber;
        }

        public void UpdateEmail(string newEmail)
        {
            if(string.IsNullOrWhiteSpace(newEmail) || !ValidationHelper.IsValidEmail(newEmail))
            {
                throw new ArgumentException("O novo endereço de email é inválido ou obrigatório.", nameof(newEmail));
            }
            Email = newEmail;
        }

        public void UpdateAddress(Address newAddress)
        {
            if(newAddress == null)
            {
                throw new ArgumentNullException(nameof(newAddress), "O novo endereço não pode ser nulo.");
            }

            Addresses = newAddress;
        }       

        public void UpdateRole(UserRole newRole)
        {
            Role = newRole;
        }

        public void UpdatePassword(string newPlainTextPassword)
        {
            if(string.IsNullOrWhiteSpace(newPlainTextPassword) || newPlainTextPassword.Length < 0)
            {
                throw new ArgumentException("A nova password é obrigatória e deve ter pelo menos 8 caracteres.", nameof(newPlainTextPassword));
            }
            _passwordHash = SecurePasswordHasher.Hash(newPlainTextPassword);
        }

        public void AddPurchase(SalesCar sale)
        {
            if(sale == null)
            {
                throw new ArgumentNullException(nameof(sale), "A venda não pode ser nula.");
            }

            _purachaseHistory.Add(sale);
        }

        public string GetUserSummary()
        {
            StringBuilder summaryBuilder = new StringBuilder();
            summaryBuilder.AppendLine($"ID: {Id}");
            summaryBuilder.AppendLine($"Name: {FirstName} {LastName}");
            summaryBuilder.AppendLine($"Email: {Email}");
            summaryBuilder.AppendLine($"Telefone: {PhoneNumber}");
            summaryBuilder.AppendLine($"Papel: {Role}");
            summaryBuilder.AppendLine($"Endereço:\n {Addresses?.GetAddress().Replace("\n", "\n ") ?? "Não definido"}");
            summaryBuilder.AppendLine("Histórico de compras: ");

            if (_purachaseHistory.Any())
            {
                
                foreach(SalesCar sale in _purachaseHistory)
                {
                    summaryBuilder.AppendLine($"  - Venda ID: {sale.Id}, Data: {sale.PurchaseDate.ToShortDateString()}, Total: {sale.TotalCost:C2}");
                }
            }
            else
            {
                summaryBuilder.AppendLine("Histórico de compras: Nenhum registo.");
            }

            return summaryBuilder.ToString();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch(RegexMatchTimeoutException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
