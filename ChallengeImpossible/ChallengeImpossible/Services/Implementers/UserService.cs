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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Create(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user), "O utilizador não pode pode ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new ArgumentException($"Já existe um utilizador com o email '{user.Email}'.");
            }

            if(_userRepository.ReadAll().Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Já existe um utilizador com o email '{user.Email}'.");
            }
            return _userRepository.Create(user);
        }

        public User Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID deve ser um número inteiro positivo.", nameof(id));
            }
            
            var user = _userRepository.Read(id);
            if (user == null)
            {
                throw new InvalidOperationException($"Utilizador com ID {id} não encontrado.");
            }
            return user;
        }

        public List<User> ReadAll()
        {
            return _userRepository.ReadAll();
        }

        public User Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "O utilizador a atualizar não pode ser nulo.");
            }
            if (user.Id <= 0) // Adicionado validação para o ID do utilizador
            {
                throw new ArgumentException("O ID do utilizador a atualizar deve ser um número positivo.", nameof(user.Id));
            }
            if (string.IsNullOrWhiteSpace(user.Email)) // Validação para o Email ser obrigatório
            {
                throw new ArgumentException("O email do utilizador é obrigatório para atualização.", nameof(user.Email));
            }

            var existingUser = _userRepository.Read(user.Id);
            if (existingUser == null)
            {
                throw new InvalidOperationException($"Utilizador com ID {user.Id} não encontrado para atualização.");
            }

            // Correção na lógica de verificação de email duplicado em atualização
            var existingUserWithSameEmail = _userRepository.ReadAll()
                .FirstOrDefault(u => u.Id != user.Id && u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));
            if (existingUserWithSameEmail != null)
            {
                throw new ArgumentException($"O email '{user.Email}' já está em uso por outro utilizador (ID: {existingUserWithSameEmail.Id}).");
            }

            // Se o modelo User tiver private set nas propriedades, o ideal é chamar um método UpdateDetails aqui.
            // existingUser.UpdateDetails(user.Name, user.Email, user.PasswordHash, user.Role);

            return _userRepository.Update(user);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID deve ser um número inteiro positivo.", nameof(id));
            }
            var userToDelete = _userRepository.Read(id); // Variável para verificar existência
            if (userToDelete == null)
            {
                return false; // Retorna false se não for encontrado
            }
            return _userRepository.Delete(id);
        }

        public User ValidateLogin(string email, string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email não pode ser vazio.", nameof(email));
            }
            if (string.IsNullOrWhiteSpace(plainTextPassword))
            {
                throw new ArgumentException("Password não pode ser vazia.", nameof(plainTextPassword));
            }

            User? user = _userRepository.ReadAll().FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if(user != null && user.VerifyPassword(plainTextPassword))
            {
                return user;
            }

            return null!;
        }

        public User RegisterUser(string firstName, string lastName, string phoneNumber, string email, 
            string plainTextPassword, Address address, UserRole role)
        {
            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(plainTextPassword) ||
                string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                address == null)
            {
                throw new ArgumentException("Todos os campos obrigatórios (Nome, Sobrenome, Email, Password, Morada) devem ser preenchidos.");
            }
            if(_userRepository.GetByEmail(email) != null)
            {
                throw new ArgumentException($"Já existe um utilizador registado com o email '{email}'.");
            }

            try
            {
                var newUser = new User(0, role, firstName, lastName, phoneNumber, email, address, plainTextPassword);
                return _userRepository.Create(newUser);
            }
            catch(ArgumentException ex)
            {
                throw new ArgumentException($"Erro ao criar o utilizador: {ex.Message}");
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Ocurreu um erro inesperado ao registar o utilizador.", ex);
            }
        }
    }
}
