using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Repository.Interfaces
{
    public interface IUserRepository
    {
        User Create(User user);
        User? Read(int id);
        List<User> ReadAll();
        User Update(User user);
        bool Delete(int id);

        User? GetByEmail(string email);
    }
}

