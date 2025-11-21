using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    public class ContactRepository : IContactRepository
    {
        public static List<Contact> _contactList = new List<Contact>();

        public int _idCounters = 1;

        public Contact Create(Contact contact)
        {
            if(contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "Não é possível criar um contacto nulo.");
            }
            if (contact.Id != 0)
            {
                throw new InvalidOperationException("Não é possível criar um contacto com ID pré-definido. O ID é atribuído pelo repositório.");
            }

            contact.SetId(_idCounters++);
            _contactList.Add(contact);
            return contact;
        }

        public Contact? Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.", nameof(id));
            }
            return _contactList.FirstOrDefault(co => co.Id == id);
        }

        public List<Contact> ReadAll()
        {
            return _contactList.ToList();
        }

        public Contact Update(Contact contact)
        {
            if(contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "O contacto a atualizar não pode ser nulo");
            }
            if(contact.Id <= 0)
            {
                throw new ArgumentException("O ID do contacto a atualizar deve ser um número positivo.", nameof(contact.Id));
            }

            Contact? existingContact = _contactList.FirstOrDefault(co => co.Id == contact.Id);
            if(existingContact != null)
            {
                _contactList.Remove(existingContact);
                _contactList.Add(contact);
                return contact;                
            }
            return null!;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.");
            }
            return _contactList.RemoveAll(cl => cl.Id == id) > 0;
        }
    }
}
