using System.Collections.Generic;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Repository.Interfaces
{
    public interface IContactRepository
    {
        Contact Create(Contact contact);
        Contact? Read(int id);
        List<Contact> ReadAll();
        Contact Update(Contact contact);
        bool Delete(int id);
    }
}
