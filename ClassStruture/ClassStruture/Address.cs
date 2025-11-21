using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassStruture
{
    public class Address
    {
        public int ID { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public int DoorNumber { get; set; }
        public int Floor { get; set; }
        public int PostalCode { get; set; }
        public string Locate { get; set; }
        public string City { get; set; }
        public string Country { get; set; }      
    

        public string GetAddress()
        {
            string adressPerson = $"{Street}, ";
            if (!string.IsNullOrEmpty(Street2))
            {
                adressPerson += $"{Street2}, ";
            }
            adressPerson += $"N.º {DoorNumber}, ";
            if(Floor > 0)
            {
                adressPerson = $"{Floor} ºFloor";
            }
            adressPerson += $"{PostalCode} {Locate}, {City}, {Country}";
            return adressPerson;
        }

        public bool SetAddress(string street, string street2, int doorNumber, int floor, int postalCode, string locate, string country, string city)
        {
            if(string.IsNullOrEmpty(street) || doorNumber <= 0 || postalCode <= 0 || string.IsNullOrEmpty(locate) || string.IsNullOrEmpty(country))
            {
                Console.WriteLine("Erro: Algumas informações da morada não estão certas");
                return false;
            }
            Street = street;
            Street2 = street2;
            DoorNumber = doorNumber;
            Floor = floor;
            PostalCode = postalCode;
            Locate = locate;
            City = city;
            Country = country;
            return true;
        }
    }
}
