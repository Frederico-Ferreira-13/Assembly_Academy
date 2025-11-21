using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace ChallengeImpossible.Model
{
    public class Provider
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Address MainAddress { get; private set; }        
        public string Sector { get; set; }
        public ProviderContact MainProviderContact { get; private set; }

        public Provider(string name, Address mainAddress,
            string sector, ProviderContact mainProviderContact)
        {
            ValidateProvider(name, mainAddress, sector, mainProviderContact);

            Name = name;
            MainAddress = mainAddress;            
            Sector = sector;
            MainProviderContact = mainProviderContact;
        }
        
        private void ValidateProvider(string name, Address mainAddress, string sector, ProviderContact mainProviderContact)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("O nome do fornecedor é obrigatório.", nameof(name));
            }
            if (mainAddress == null)
            {
                throw new ArgumentNullException(nameof(mainAddress), "O endereço principal do fornecedor é obrigatório.");
            }
            if (mainProviderContact == null)
            {
                throw new ArgumentNullException(nameof(mainProviderContact), "O contacto principal do fornecedor é obrigatório.");
            }
        }

        public void Update(string newName, Address newAddress, string newSector, ProviderContact newProviderContact)
        {
            ValidateProvider(newName, newAddress, newSector, newProviderContact);

            Name = newName;
            MainAddress = newAddress;
            Sector = newSector;
            MainProviderContact = newProviderContact;
        }

        internal void SetId(int id)
        {
            if(Id != 0)
            {
                throw new InvalidOperationException("ID do Forncedor já foi atribuido e não pode ser alterado.");
            }
            Id = id;
        }

        public string GetProviderDetails()
        {

            string addressInfo = MainAddress?.GetAddress() ?? "Endereço não definido";
            string contactInfo = MainProviderContact?.GetFormattedContactInfo() ?? "Contacto não definido";

            StringBuilder detailsBuilder = new StringBuilder();
            detailsBuilder.AppendLine($"ID: {Id}");
            detailsBuilder.AppendLine($"Nome do Fornecedor: {Name}");
            detailsBuilder.AppendLine($"Setor: {Sector}");           
            detailsBuilder.AppendLine($"Endereço principal: {addressInfo}"); // Mostra o resumo completo do contacto
            detailsBuilder.AppendLine($"Detalhes do Contacto Principal: {contactInfo}");

            return detailsBuilder.ToString();            
        }

        public override string ToString()
        {
            return GetProviderDetails();
        }                
    }
}

