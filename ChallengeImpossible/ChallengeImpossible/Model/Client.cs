using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeImpossible.Model
{
    public class Client
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public string Sector { get; private set; }
        public string NIF { get; private set; }
        public Address DeliveryAddress { get; private set; }
        public Contact Contact { get; private set; }

        public Client(string name, Address address, string sector, string nif, Address deliveryAddress,
             Contact contact)
        {
            ValidateClientProperties(name, address, sector, nif, contact);
            ValidateNif(nif);

            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            Address = address;
            Sector = sector;
            NIF = nif;
            DeliveryAddress = deliveryAddress;
            Contact = contact;
        }

        public Client(int id, string name, Address address, string sector, string nif, Address deliveryAddress,
             Contact contact)
        {
            SetId(id);
            ValidateClientProperties(name, address, sector, nif, contact);
            ValidateNif(nif);

            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            Address = address;
            Sector = sector;
            NIF = nif;
            DeliveryAddress = deliveryAddress;
            Contact = contact;
        }

        internal void SetId(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O Id do cliente deve ser um número positivo.", nameof(id));
            }
            Id = id;
        }

        public void ValidateClientProperties(string name, Address address, string sector, string nif,
            Contact contact)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("O nome do cliente é obrigatório.", nameof(name));
            }                
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "O endereço principal do cliente é obrigatório.");
            }                
            if (string.IsNullOrWhiteSpace(nif))
            {
                throw new ArgumentException("O NIF do cliente é obrigatório.", nameof(nif));
            }                
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "O contacto do cliente é obrigatório.");
            }            
        } 
        
        public void ValidateNif(string nif)
        {
            if(nif.Length != 9 || !long.TryParse(nif, out _))
            {
                throw new ArgumentException("NIF inválido. Deve ter 9 digitos numéricos.", nameof(nif));
            }
        }        

        public string GetClientSummary()
        {
            // O operador ?. (null-conditional) ajuda a evitar NullReferenceException se Address ou Contact forem null.
            string addressInfo = Address?.GetAddress() ?? "Endereço principal não definido";
            string deliveryAddressInfo = DeliveryAddress?.GetAddress() ?? "Endereço de entrega não definido";
            string contactInfo = Contact?.GetContactSummary() ?? "Contacto não definido";

            return $"--- Detalhes do Cliente (ID: {Id} ---\n" +
                   $"Cliente: {Name} (NIF: {NIF})\n" +
                   $"Endereço Principal: {addressInfo}\n" +
                   $"Endereço de Entrega: {deliveryAddressInfo}\n" +
                   $"Contacto: {contactInfo}\n" +
                   $"Sector: {Sector}";
        }        
    }   
}

