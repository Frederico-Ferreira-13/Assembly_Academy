using System;
using System.Collections.Generic;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Services.Implementers
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Contact Create(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "O contacto a criar não pode ser nulo.");
            }
            return _contactRepository.Create(contact);
        }

        public Contact Read(int id)
        {
            if( id <= 0)
            {
                throw new ArgumentException("O ID do contacto deve ser um número positivo.", nameof(id));
            }

            Contact? contact = _contactRepository.Read(id);
            if(contact == null)
            {
                throw new InvalidOperationException($"Contacto com ID {id} não encontrado.");
            }
            return contact;
        }

        public List<Contact> ReadAll()
        {
            return _contactRepository.ReadAll();
        }

        public Contact Update(Contact contact)
        {
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "O contacto a atualizar não pode ser nulo.");
            }
            if (contact.Id <= 0)
            {
                throw new ArgumentException("O ID do contacto a ser atualizado deve ser um número positivo.", nameof(contact.Id));
            }

            Contact? existingContact = _contactRepository.Read(contact.Id);
            if(existingContact == null)
            {
                throw new InvalidOperationException($"Nao foi possível atualizar: Contacto com ID {contact.Id} não encontrado.");
            }
            return _contactRepository.Update(contact);
        }

        public bool Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.", nameof(id));
            }

            Contact? contactToDelete = _contactRepository.Read(id);
            if(contactToDelete == null)
            {
                return false;
            }
            return _contactRepository.Delete(id);            
        }
    }
}
