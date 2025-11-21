using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Services.Interfaces
{
    public interface IProviderService
    {
        Provider Create(Provider provider);
        Provider Read(int id);
        List<Provider> ReadAll();
        Provider Update(Provider provider);
        bool Delete(int id);
    }
}
