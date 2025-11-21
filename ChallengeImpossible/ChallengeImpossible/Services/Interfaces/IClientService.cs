using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Services.Interfaces
{
    public interface IClientService
    {
        Client Create(Client client);
        Client Read(int id);
        List<Client> ReadAll();
        Client Update(Client client);
        bool Delete(int id);
    }
}
