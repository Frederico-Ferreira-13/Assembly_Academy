using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Services.Implementers
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderService(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        public Provider Create(Provider provider)
        {
            if(provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "Fornecedor não pode ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(provider.Name)) // Adicionado validação de nome
            {
                throw new ArgumentException("O nome do fornecedor é obrigatório.", nameof(provider.Name));
            }

            if (_providerRepository.ReadAll().Any(p => p.Name == provider.Name))
            {
                throw new InvalidOperationException($"Já existe um fornecedor com o nome '{provider.Name}'");
            }
            return _providerRepository.Create(provider);
        }

        public Provider Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID do fornecedor inválido. Deve ser um número positivo.");
            }

            var provider = _providerRepository.Read(id);
            if(provider == null)
            {
                throw new InvalidOperationException($"Fornecedor com ID {id} não encontrado.");
            }

            return provider;
        }

        public List<Provider> ReadAll()
        {
            return _providerRepository.ReadAll();
        }

        public Provider Update(Provider provider)
        {
            if(provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "Fornecedor não pode ser nulo.");
            }
            if(provider.Id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(provider.Id), "ID do fornecedor inválido para atualização.");
            }
            if (string.IsNullOrWhiteSpace(provider.Name)) // Adicionado validação de nome
            {
                throw new ArgumentException("O nome do fornecedor é obrigatório para atualização.", nameof(provider.Name));
            }

            var existingProvider = _providerRepository.Read(provider.Id);
            if(existingProvider == null)
            {
                throw new InvalidOperationException($"Fornecedor com ID {provider.Id} não encontrado para atualização.");
            }

            var providerWithSameName = _providerRepository.ReadAll()
                .FirstOrDefault(p => p.Id != provider.Id && p.Name.Equals(provider.Name, StringComparison.OrdinalIgnoreCase));
            
            if(providerWithSameName != null)
            {
                throw new InvalidOperationException($"O nome '{provider.Name}' já está registado para outro fornecedor (ID: {providerWithSameName.Id}).");
            }

            return _providerRepository.Update(provider);
        }

        public bool Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "ID do fornecedor inválido para remover.");
            }

            var existingProvider = _providerRepository.Read(id);
            if(existingProvider == null)
            {
                return false;
            }
            return _providerRepository.Delete(id);            
        }
    }
}
