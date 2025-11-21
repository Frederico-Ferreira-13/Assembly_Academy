using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Services.Interfaces
{
    public interface IUserService
    {
        User Create(User user);
        User Read(int id);
        List<User> ReadAll();
        User Update(User user);
        bool Delete(int id);
        User ValidateLogin(string email, string plainTextpasswprd);
        User RegisterUser(string firstName, string lastName, string phoneNumber, string Email,
            string plainTextPassword, Address address, UserRole role);
    }
}

