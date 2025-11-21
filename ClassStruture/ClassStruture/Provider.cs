using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassStruture
{
    public class Provider
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string ContactPersonName { get; set; }
        public string PersonJobTitle { get; set; }
        public int PhoneNumber { get; set; }
        public string ContactPersonEmail { get; set; }
        public string Sector { get; set; }
        
        public string GetProviderDetails(string name, string address, string contactPersonName,string personJobTitle, int phoneNumber, string contactPersonEmail, string sector)
        {

            return $"ID: {ID}, Name: {Name}, Address: {Address}, Contact: {ContactPersonName} ({PersonJobTitle}), Phone: {PhoneNumber}, Email: {ContactPersonEmail}, Setor: {Sector}";
        }
        public void AddProviderAddress(Address address)
        {
            Address = address;
        }
    }
}
