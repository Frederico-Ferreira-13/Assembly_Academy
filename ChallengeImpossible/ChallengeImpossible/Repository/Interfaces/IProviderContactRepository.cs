using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Repository.Interfaces
{
    public interface IProviderContactRepository
    {
        ProviderContact Create(ProviderContact providerContact);
        ProviderContact? Read(int id);
        List<ProviderContact> ReadAll();
        ProviderContact Update(ProviderContact providerContact);
        bool Delete(int id);
        bool EmailExists(string email, int? excludeId);
    }
}
