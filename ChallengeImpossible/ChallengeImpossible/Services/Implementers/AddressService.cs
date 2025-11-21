using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeImpossible.Model;
using ChallengeImpossible.Repository.Interfaces;
using ChallengeImpossible.Services.Interfaces;

namespace ChallengeImpossible.Services.Implementers
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public Address Create(Address address)
        {
            if(address == null)
            {
                throw new ArgumentNullException(nameof(address), "O endereço a ser criado não pode ser nulo.");
            }
            if(IsAddressDuplicated(address, excludeId: 0))
            {
                throw new ArgumentException("Já existe um endereço com a mesma Rua, Número, Código Postal e Lolidade.", nameof(address));
            }
            return _addressRepository.Create(address);
        }

        public Address Read(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID para leitura deve ser um número positivo.", nameof(id));
            }
            var address = _addressRepository.Read(id);
            if(address == null)
            {
                throw new InvalidOperationException($"Endereço com ID {id} não encontrado.");
            }
            return address;
        }
        public List<Address> ReadAll()
        {
            return _addressRepository.ReadAll();
        }

        public Address Update(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "O endereço a ser atualizado não pode ser nulo.");
            }
            if (address.Id <= 0)
            {
                throw new ArgumentException("O ID do endereço a ser atualizado deve ser um número positivo.", nameof(address.Id));
            }

            var existingAddress = _addressRepository.Read(address.Id);
            if(existingAddress == null)
            {
                throw new ArgumentException($"Endereço com ID {address.Id} não encontrado para atualização.", nameof(address.Id));
            }
            if(IsAddressDuplicated(address, excludeId: address.Id))
            {
                throw new ArgumentException("A atualização resultaria num endereço duplicado (mesma Rua, Número, Código Postal e Localidade).", nameof(address));
            }
            return _addressRepository.Update(address);
        }

        public bool Delete(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID para eliminação deve ser um número positivo.", nameof(id));
            }

            var existingAddress = _addressRepository.Read(id);
            if(existingAddress == null)
            {
                return false;
            }
            
            return _addressRepository.Delete(id);            
        }

        private bool IsAddressDuplicated(Address addressToCheck, int excludeId)
        {
            var allAddress = _addressRepository.ReadAll()
                .Where(a => a.Id != excludeId)
                .ToList();

            return allAddress.Any(a =>
            a.Street.Equals(addressToCheck.Street, StringComparison.OrdinalIgnoreCase) &&
            a.DoorNumber.Equals(addressToCheck.DoorNumber, StringComparison.OrdinalIgnoreCase) &&
            a.PostalCode.Equals(addressToCheck.PostalCode, StringComparison.OrdinalIgnoreCase) &&
            a.Locate.Equals(addressToCheck.Locate, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
