using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    public class UserRepository : IUserRepository
    {
        public static List<User> _userList = new List<User>();
        public static int _idCounters = 1;

        public User Create(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user), "Não é possível criar um utilizador nulo.");
            }
            if(user.Id == 0)
            {
                user.SetId(_idCounters++);
            }
            else
            {
                if(_userList.Any(u => u.Id == user.Id))
                {
                    throw new ArgumentException($"Um utilizador com o ID {user.Id} já existe.");
                }
                if(user.Id >= _idCounters)
                {
                    _idCounters = user.Id + 1;
                }
            }
            if (_userList.Any(u => u.Id != user.Id && u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"O email '{user.Email}' já está em uso por outro utilizador");
            }
            _userList.Add(user);
            return user;
        }

        public User? Read(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.", nameof(id));
            }
            return _userList.FirstOrDefault(u => u.Id == id);
        }

        public List<User> ReadAll()
        {
            return _userList.ToList();
        }

        public User Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "O utilizador a atualizar não pode ser nulo.");
            }
            if (user.Id <= 0)
            {
                throw new ArgumentException("O ID do utilizador a atualizar deve ser um número positivo.", nameof(user.Id));
            }

            User? existingUser = _userList.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                if(_userList.Any(u => u.Id != user.Id && u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException($"O email '{user.Email}' já está em uso por outro Utilizador.");
                }

                existingUser.UpdatePersonalInformation(user.FirstName, user.LastName, user.PhoneNumber);
                existingUser.UpdateEmail(user.Email);
                existingUser.UpdateAddress(user.Addresses);
                existingUser.UpdateRole(user.Role);

                return existingUser;
            }
            return null!;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.", nameof(id));
            }

            int removedCount = _userList.RemoveAll(u => u.Id == id);
            return removedCount > 0;
        }

        public User? GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("O email não pode ser nulo ou vazio para a pesquisa.", nameof(email));
            }
            return _userList.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
