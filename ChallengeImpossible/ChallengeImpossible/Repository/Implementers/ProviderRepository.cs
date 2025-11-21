using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    internal class ProviderRepository : IProviderRepository
    {
        public static List<Provider> _providerList = new List<Provider>();

        public int _idCounters = 1;

        public Provider Create(Provider provider)
        {
            if(provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "Fornecedor não pode ser nulo.");
            }
           
            provider.SetId(_idCounters++);
            _providerList.Add(provider);
            return provider;
        }

        public Provider? Read(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.", nameof(id));
            }
            return _providerList.FirstOrDefault(p => p.Id == id);
        }

        public List<Provider> ReadAll()
        {
            return _providerList.ToList();
        }

        public Provider Update(Provider provider)
        {
            if(provider == null)
            {
                throw new ArgumentNullException(nameof(provider), "Fornecedor não pode ser nulo.");
            }
            if (provider.Id <= 0)
            {
                throw new ArgumentException("O ID do fornecedor a atualizar deve ser um número positivo.", nameof(provider.Id));
            }

            Provider? existingProvider = _providerList.FirstOrDefault(p => p.Id == provider.Id);
            if (existingProvider != null)
            {
                return existingProvider;
            }
            return null!;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.", nameof(id));
            }
            int removedCount = _providerList.RemoveAll(p => p.Id == id);
            return removedCount > 0;
        }
    }
}
