using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;

namespace ChallengeImpossible.Repository.Implementers
{
    public class AddressRepository : IAddressRepository
    {
        public static List<Address> _addressList = new List<Address>();
        private static int _idCounters = 1;

        public Address Create(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Não é possível criar um endereço nulo.");
            }
            if(address.Id != 0)
            {
                throw new InvalidOperationException("Não é possível criar um endereço com um ID pré-definido. O ID é atribuido pelo repositório.");
            }

            address.SetId(_idCounters++);
            _addressList.Add(address);
            return address;
        }

        public Address? Read(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.");
            }
            return _addressList.FirstOrDefault(a => a.Id == id);
        }

        public List<Address> ReadAll()
        {
            return _addressList.ToList();
        }

        public Address Update(Address address)
        {
            if(address == null)
            {
                throw new ArgumentNullException(nameof(address), "O endereço a atualizar não pode ser nulo.");
            }
            if(address.Id <= 0)
            {
                throw new ArgumentException("O ID do endereço a atualizar deve ser um número positivo.");
            }

            Address? existingAddres = _addressList.FirstOrDefault(b => b.Id == address.Id);

            if(existingAddres != null)
            {
                existingAddres.UpdateDetails(
                    address.Street,
                    address.Street2,                    
                    address.DoorNumber,
                    address.Floor,
                    address.PostalCode,
                    address.Locate,
                    address.City,
                    address.Country                    
                );
                return existingAddres;
            }
            throw new InvalidOperationException($"Endereço com ID {address.Id} não encontrado para atualização.");
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.", nameof(id));
            }
            return _addressList.RemoveAll(b => b.Id == id) > 0;            
        }
    }
}
