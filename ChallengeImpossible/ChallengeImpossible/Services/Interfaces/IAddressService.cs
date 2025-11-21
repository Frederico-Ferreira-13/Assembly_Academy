using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Services.Interfaces
{
    public interface IAddressService
    {
        Address Create(Address address);
        Address Read(int id);
        List<Address> ReadAll();
        Address Update(Address address);
        bool Delete(int id);
    }
}

