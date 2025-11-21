using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Services.Implementers
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Client Create(Client client)
        {
            ValidateClientForService(client);         

            if(_clientRepository.NifExists(client.NIF, 0))
            {
                var clientWithSameNif = _clientRepository.ReadByNIF(client.NIF);
                throw new InvalidOperationException(
                    $"O NIF {client.NIF} já está registado para outro cliente (ID: {clientWithSameNif?.Id}," +
                    $"Nome: {clientWithSameNif?.Name}).");
            }            
           
            return _clientRepository.Create(client);
        }

        public Client Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID do cliente deve ser um valor positivo.", nameof(id));
            }
            
            var client = _clientRepository.Read(id);
            if(client == null)
            {
                throw new InvalidOperationException($"CLiente com {id} não encontrado.");
            }
            return client;
        }

        public List<Client> ReadAll()
        {
            return _clientRepository.ReadAll();
        }

        public Client Update(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client), "Cliente não pode ser nulo.");
            }
            if (client.Id <= 0)
            {
                throw new ArgumentException("O ID do cliente a atualizar deve ser um valor positivo.", nameof(client.Id));
            }

            ValidateClientForService(client);
            client.ValidateNif(client.NIF);

            var existingCLient = _clientRepository.Read(client.Id);
            if(existingCLient == null)
            {
                throw new InvalidOperationException($"Client com ID {client.Id} não encontrado para atualização.");
            }
            
            if (_clientRepository.NifExists(client.NIF, client.Id))
            {
                var clientWithSameNif = _clientRepository.ReadByNIF(client.NIF);
                if (clientWithSameNif != null)
                {
                    throw new InvalidOperationException(
                        $"O NIF '{client.NIF}' já está registado para outro cliente (ID: {clientWithSameNif.Id}, Nome: {clientWithSameNif.Name}).");
                }               
            }   

            return _clientRepository.Update(existingCLient);
        }

        public bool Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID do cliente para eliminação deve ser positivo.", nameof(id));
            }

            var existingClient = _clientRepository.Read(id);
            if(existingClient == null)
            {
                return false;
            }
            return _clientRepository.Delete(id);            
        }

        private void ValidateClientForService(Client client)
        {
            if(client == null)
            {
                throw new ArgumentNullException(nameof(client), "Cliente não pode ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(client.Name))
            {
                throw new ArgumentException("O nome do cleinte não pode ser vazio.", nameof(client.Name));
            }
            if (string.IsNullOrWhiteSpace(client.NIF))
            {
                throw new ArgumentException("O NIF do cliente não pode ser vazio.", nameof(client.NIF));
            }
            if(client.Contact != null && !IsValidEmail(client.Contact.Email))
            {
                throw new ArgumentException("O formato do email do contacto do cliente é inválido.", nameof(client.Contact.Email));
            }
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch(RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
