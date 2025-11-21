using AppService.Interfaces;
using AppService.Services;
using Core.Model;
using Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public User? Retrieve(int userId)
        {
            return _userRepository.Retrieve(userId);
        }

        public void ChangePassword(int userId, string newPlainTextPassword)
        {
            if(userId <= 0)
            {
                throw new ArgumentException("O ID do utilizador deve ser um número inteiro positivo.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(newPlainTextPassword))
            {
                throw new ArgumentException("A nova password não pode ser vazia.", nameof(newPlainTextPassword));
            }

            var user = _userRepository.Retrieve(userId);
            
            if (user == null)
            {
                throw new InvalidOperationException($"Utilizador com ID {userId} não encontrado para alteração de password.");
            }

            user.UpdatePassword(newPlainTextPassword);
            _userRepository.Update(user);
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

            User? user = _userRepository.GetByEmail(email);

            // Nota: O uso de 'null!' aqui pressupõe que, se o utilizador não for encontrado ou a password falhar,
            // será lançado um erro noutro ponto ou tratado de forma específica. Mantive o comportamento.
            if (user != null && user.VerifyPassword(plainTextPassword))
            {
                return user;
            }

            return null!;
        }

        public int CountUsers()
        {
            return _userRepository.CountAll();
        }

        public User RegisterUserWithRole(string email, string userName, string plainTextPassword,
            UserRole role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(plainTextPassword) || 
                string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Email, Username e Password são campos obrigatórios.");
            }

            if (_userRepository.ExistsByEmail(email))
            {
                throw new ArgumentException($"Já existe um utilizador registado com o email '{email}'.");
            }

            bool isApproved = (role == UserRole.Admin) || (_userRepository.CountAll() == 0);

            try
            {
                // IsApproved é definido como true apenas se for ADMIN ou se for o primeiro utilizador.
                var newUser = new User(0, role, email, userName, plainTextPassword, false);
                _userRepository.Create(newUser);

                return newUser;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ocorreu um erro inesperado ao registar o utilizador.", ex);
            }
        }

        public List<User> GetPendingUsersForApproval()
        {
            return _userRepository.GetByApprovalStatus(false);
        }

        public void ApproveUser(int userId)
        {
            if(userId <= 0)
            {                
                throw new ArgumentException("ID de utilizador inválido.", nameof(userId));                
            }

            User? user = _userRepository.Retrieve(userId);

            if(user == null)
            {
                throw new InvalidOperationException($"Utilizador com ID {userId} não encontrado para aprovação.");
            }

            if (user.IsApproved)
            {
                return;
            }

            user.Approve();
            _userRepository.Update(user);
        }

        public void RejectAndDeleteUser(int userId)
        {
            if(userId <= 0)
            {
                throw new ArgumentException("ID de utilizador inválido.", nameof(userId));
            }

            bool deleted = _userRepository.Delete(userId);

            if (!deleted)
            {
                throw new InvalidOperationException($"Não foi possível remover o utilizador com ID {userId}. Pode não existir.");
            }
        }
    }
}
