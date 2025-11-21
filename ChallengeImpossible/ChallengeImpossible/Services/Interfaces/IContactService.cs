using System.Collections.Generic;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Services.Interfaces
{
    public interface IContactService
    {
        Contact Create(Contact contact);
        Contact Read(int id);
        List<Contact> ReadAll();
        Contact Update(Contact contact);
        bool Delete(int id);
    }
}
