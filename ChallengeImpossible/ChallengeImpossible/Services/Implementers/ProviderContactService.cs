using System;
using System.Collections.Generic;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Services.Implementers
{
    public class ProviderContactService : IProviderContactService
    {
        private readonly IProviderContactRepository _providerContactRepoitory;

        public ProviderContactService(IProviderContactRepository providerContactRepository)
        {
            _providerContactRepoitory = providerContactRepository;
        }

        public ProviderContact Create(ProviderContact providerContact)
        {
            if (providerContact == null)
            {
                throw new ArgumentNullException(nameof(providerContact), "O contacto de fornecedor a criar não pode ser nulo.");
            }
            if (providerContact.ContactDetails == null) // Validação para ContactDetails não ser nulo
            {
                throw new ArgumentException("Os detalhes de contacto são obrigatórios para um contacto de fornecedor.", nameof(providerContact.ContactDetails));
            }
            if (_providerContactRepoitory.EmailExists(providerContact.ContactDetails.Email, null))
            {
                throw new ArgumentException("O email fornecido já está registado para outro contacto de fornecedor.", nameof(providerContact.ContactDetails));
            }
            return _providerContactRepoitory.Create(providerContact);
        }

        public ProviderContact Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("ID de contacto de fornecedor inválido.", nameof(id));
            }

            var providerContact = _providerContactRepoitory.Read(id);
            if(providerContact == null)
            {
                throw new InvalidOperationException($"Contacto de fornecedor com ID {id} não encontrado.");
            }
            return providerContact;
        }

        public List<ProviderContact> ReadAll()
        {
            return _providerContactRepoitory.ReadAll();
        }

        public ProviderContact Update(ProviderContact providerContact)
        {
            if (providerContact == null)
            {
                throw new ArgumentNullException(nameof(providerContact), "O contacto de fornecedor a atualizar não pode ser nulo.");
            }
            if (providerContact.Id <= 0)
            {
                throw new ArgumentException("O ID do contacto de fornecedor a atualizar deve ser um número positivo.", nameof(providerContact.Id));
            }
            if (providerContact.ContactDetails == null) // Validação para ContactDetails não ser nulo
            {
                throw new ArgumentException("Os detalhes de contacto são obrigatórios para um contacto de fornecedor.", nameof(providerContact.ContactDetails));
            }

            ProviderContact? existingContact = _providerContactRepoitory.Read(providerContact.Id);
            if(existingContact == null)
            {
                throw new ArgumentException($"Contacto de fornecedor com ID {providerContact.Id} não encontrado para atualização.");
            }

            if(_providerContactRepoitory.EmailExists(providerContact.ContactDetails.Email, providerContact.Id))
            {
                throw new ArgumentException("O email fornecido já está registado para outro contacto de fornecedor.", nameof(providerContact.ContactDetails.Email));
            }
            return _providerContactRepoitory.Update(providerContact);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID de contacto de fornecedor inválido para eliminação. Deve ser um número positivo.", nameof(id));
            }

            ProviderContact? existingContact = _providerContactRepoitory.Read(id);
            if(existingContact == null)
            {
                return false;
            }
           
            return _providerContactRepoitory.Delete(id);            
        }
    }
}
