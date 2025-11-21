using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    public class ClientRepository : IClientRepository
    {
        public static List<Client> _clientList = new List<Client>();

        private static int _idCounters = 1;

        public Client Create(Client client)
        {
            if(client == null)
            {
                throw new ArgumentNullException(nameof(client), "Não é possível criar um cliente nulo.");
            }
            if(client.Id != 0)
            {
                throw new InvalidOperationException("Não é possível criar um cliente com um ID pré-definido. O ID é atribuído pelo repositório.");
            }

            client.SetId( _idCounters++);
            _clientList.Add(client);
            return client;
        }

        public Client? Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.", nameof(id));
            }
            return _clientList.FirstOrDefault(cl => cl.Id == id);
        }

        public List<Client> ReadAll()
        {
            return _clientList.ToList();
        }

        public Client Update(Client client)
        {
            if(client == null)
            {
                throw new ArgumentNullException(nameof(client), "O cliente a atualizar não pode ser nulo.");
            }
            if(client.Id <= 0)
            {
                throw new ArgumentException("O ID do cliente a atualizar deve ser um valor positivo.", nameof(client.Id));
            }
            
            Client? existingClient = _clientList.FirstOrDefault(cl => cl.Id == client.Id);
            if(existingClient != null)
            {
                _clientList.Remove(existingClient);
                _clientList.Add(client);
                return client;                
            }
            return null!;
        }

        public bool Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.", nameof(id));
            }
            return _clientList.RemoveAll(cl => cl.Id == id) > 0;            
        }

        public bool NifExists(string nif, int excludeId = 0)
        {
            if (string.IsNullOrWhiteSpace(nif))
            {
                return false;
            }

            return _clientList.Any(cl =>
            cl.NIF != null &&
            cl.NIF.Equals(nif, StringComparison.OrdinalIgnoreCase) &&
            cl.Id != excludeId
            );
        }

        public Client? ReadByNIF(string nif)
        {
            if (string.IsNullOrWhiteSpace(nif))
            {
                throw null!;
            }

            return _clientList.FirstOrDefault(cl => 
            cl.NIF != null &&
            cl.NIF.Equals(nif, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
