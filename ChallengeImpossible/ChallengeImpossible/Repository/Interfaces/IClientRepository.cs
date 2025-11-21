using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;

namespace ChallengeImpossible.Repository.Interfaces
{
    public interface IClientRepository
    {
        Client Create(Client client);
        Client? Read(int id);
        List<Client> ReadAll();
        Client Update(Client client);
        bool Delete(int id);
        bool NifExists(string nif, int excludeId = 0);
        Client? ReadByNIF(string nif);
    }
}
