using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassStruture
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        
        public int ID()
        {
            return Id;
        }
        
        public string GetFullName()
        {
            return FirstName + "" + LastName;
        }

        public void GetContacts(string phoneNumber, string email)
        {
            if(int.TryParse(phoneNumber, out int getphoneNumber))
            {
                PhoneNumber = getphoneNumber;
            }
            else
            {
                throw new ArgumentException("Contacto inválido!");
            }
            if(!string.IsNullOrWhiteSpace(email) && email.Contains("@"))
            {
                Email = email;
            }
            else
            {
                throw new ArgumentException("Email inválido!");
            }
            
        }
        public string GetAddress()
        {
            return Address;
        }
    }       
}
