using System;
using System.Diagnostics.Metrics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions; 

namespace ChallengeImpossible.Model
{
    public class Address
    {
        public int Id { get; private set; }
        public string Street { get; private set; }
        public string? Street2 { get; private set; }
        public string DoorNumber { get; private set; }
        public string? Floor { get; private set; }
        public string PostalCode { get; private set; }
        public string Locate { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }    

        public Address(string street, string? street2, string doorNumber, string? floor, string postalCode,
        string city, string country, string locate)
        {
            ValidateAddressData(street, street2, doorNumber, floor, postalCode, city, country, locate);
            ValidatePostalCode(postalCode);
                       
            Street = street;
            Street2 = street2;
            DoorNumber = doorNumber;
            Floor = floor;
            PostalCode = NormalizePostalCode(postalCode);
            Locate = locate;
            City = city;
            Country = country;            
        }

        public void UpdateDetails(string street, string? street2, string doorNumber, string? floor, string postalCode,
             string city, string country, string locate)
        {
            ValidateAddressData(street, street2, doorNumber, floor, postalCode, city, country, locate);
            ValidatePostalCode(postalCode);

            Street = street;
            Street2 = street2;
            DoorNumber = doorNumber;
            Floor = floor;
            PostalCode = NormalizePostalCode(postalCode);
            Locate = locate;
            City = city;
            Country = country;
        }

        public string GetAddress()
        {
            StringBuilder addressBuilder = new StringBuilder();
            addressBuilder.Append($"{Street}, Nº {DoorNumber}");

            if (!string.IsNullOrEmpty(Street2))
            {
                addressBuilder.Append($", {Street2}");
            }
            if (!string.IsNullOrWhiteSpace(Floor))
            {
                addressBuilder.Append($", {Floor}º");
            }

            addressBuilder.Append($"\n{PostalCode} {Locate}");
            addressBuilder.Append($"{City}, {Country}");

            return addressBuilder.ToString();
        }        

        internal void SetId(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("O ID deve ser um número positivo.", nameof(id));
            }
            
            Id = id;
        }

        private void ValidateAddressData(string street, string? street2, string doorNumber, string? floor, string postalCode,
            string city, string country, string locate)
        {
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentException("A rua é obrigatória.", nameof(street));
            }
            if (string.IsNullOrWhiteSpace(doorNumber))
            {
                throw new ArgumentException("O número da porta é obrigatório.", nameof(doorNumber));
            }
            if (string.IsNullOrWhiteSpace(postalCode))
            {
                throw new ArgumentException("O código postal é obrigatório.", nameof(postalCode));
            }            
            if (string.IsNullOrWhiteSpace(locate))
            {
                throw new ArgumentException("A localidade é obrigatória.", nameof(locate));
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("A Cidade é obrigatória.", nameof(city));
            }
            if (string.IsNullOrWhiteSpace(country))
            {
                throw new ArgumentException("O país é obrigatório.", nameof(country));
            }
        }

        private static void ValidatePostalCode(string postalCode)
        {
            if(!Regex.IsMatch(postalCode, @"^\d{4}-\d{3}$"))
            {
                throw new ArgumentException("O cópdigo postal deve ter o formato 'XXXX-YYY'(ex: 1234-567).", nameof(postalCode));
            }
        }

        private static string NormalizePostalCode(string postalCode)
        {
            return postalCode.Trim();
        }
    }
}

