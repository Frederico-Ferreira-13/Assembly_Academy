using System.Collections.Generic;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Repository.Interfaces
{
    public interface IAddressRepository
    {
        Address Create(Address address);
        Address? Read(int id);
        List<Address> ReadAll();
        Address Update(Address address);
        bool Delete(int id);
    }
}
