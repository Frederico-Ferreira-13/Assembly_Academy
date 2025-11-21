using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassStruture
{
    public class Client
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Sector { get; set; }
        public string NIF { get; set; }
        public string DeliveryAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        public void AddNewClient(string name, Address address, string sector, string nif, string deliveryAddress, string phoneNumber, string email)
        {
            Name = name;
            Address = address;
            Sector = sector;
            NIF = nif;
            DeliveryAddress = deliveryAddress;
            PhoneNumber = phoneNumber;
            Email = email;
        }
        public string AddClientAddress(Address newAddress)
        {
            if(Address == null)
            {
                Address = newAddress;
                return $"Address added to client {Name}.";
            }
            else
            {
               return $"Client {Name} already has an address. Use update address client to change it. ";
            }
        }

        public string UpdateClientAddress(Address newAddress)
        {
            if(Address != null)
            {
                Address = newAddress;
                return $"Address for client {Name} has been updated.";
            }
            else
            {
                return $"Client {Name} does not have an address yet. Use Add CLient Address to add one.";
            }
        }
    }
}
