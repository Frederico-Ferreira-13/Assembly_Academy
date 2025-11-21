using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    public class ProviderContactRepository : IProviderContactRepository
    {
        public static List<ProviderContact> _providerContactList = new List<ProviderContact>();

        public int _idCounters = 1;

        public ProviderContact Create(ProviderContact providerContact)
        {
            if (providerContact == null)
            {
                throw new ArgumentNullException(nameof(providerContact), "Não é possível criar um contacto de fornecedor nulo.");
            }
            if (providerContact.Id != 0)
            {
                throw new InvalidOperationException("Não é possível criar um contacto de fornecedor com um ID pré-definido. O ID é atribuído pelo repositório.");
            }

            providerContact.SetId(_idCounters++);
            _providerContactList.Add(providerContact);
            return providerContact;
        }

        public ProviderContact? Read(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.", nameof(id));
            }
            return _providerContactList.FirstOrDefault(pc => pc.Id == id);
        }

        public List<ProviderContact> ReadAll()
        {
            return _providerContactList.ToList();
        }

        public ProviderContact Update(ProviderContact providerContact)
        {
            return providerContact;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.", nameof(id));
            }
            return _providerContactList.RemoveAll(pc => pc.Id == id) > 0;            
        }

        public bool EmailExists(string email, int? excludeId)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return _providerContactList.Any(pc =>
                pc.ContactDetails != null && // Garante que Email não é nulo antes de chamar .Equals
                pc.ContactDetails.Email != null && 
                pc.ContactDetails.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                pc.Id != excludeId
            );
        }
    }
}
