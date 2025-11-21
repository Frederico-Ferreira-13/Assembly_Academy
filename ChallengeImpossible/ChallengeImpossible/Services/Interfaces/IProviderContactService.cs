using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Services.Interfaces
{
    public interface IProviderContactService
    {
        ProviderContact Create(ProviderContact providerContact);
        ProviderContact Read(int id);
        List<ProviderContact> ReadAll();
        ProviderContact Update(ProviderContact providerContact);
        bool Delete(int id);
    }
}
