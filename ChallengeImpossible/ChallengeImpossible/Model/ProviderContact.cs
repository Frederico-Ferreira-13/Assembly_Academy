using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeImpossible.Model
{
    public class ProviderContact
    {
        public int Id { get; private set; }        
        public Contact ContactDetails { get; private set; }

        public ProviderContact(Contact contactDetails)
        {
            if(contactDetails == null)
            {
                throw new ArgumentNullException(nameof(contactDetails), "A pessoa de contacto é obrigatória.");
            }
            
            ContactDetails = contactDetails;            
        }

        public ProviderContact(int id, Contact contactDetails) : this(contactDetails)
        {
            SetId(id);            
        }       

        public void SetId(int id)
        {
            if(Id != 0 && Id != id)
            {
                throw new InvalidOperationException("O ID do contacto do fornecedor já foi definido e não pode ser alterado.");
            }
            if(id <= 0)
            {
                throw new ArgumentException("O ID deve ser um número inteiro positivo.");
            }
            Id = id;
        }

        public void UpdateDetails(Contact contactDetails)
        {
            if(contactDetails == null)
            {
                throw new ArgumentNullException(nameof(Contact), "Os detalhes de contacto para atualização não podem ser nulos.");
            }

            ContactDetails.UpdateDetails(
                contactDetails.FirstName,
                contactDetails.LastName,
                contactDetails.Email,
                contactDetails.PhoneNumber,
                contactDetails.JobTitle
            );
        }

        public string GetFormattedContactInfo()
        {
            return ContactDetails?.ToString() ?? "Dados de contacto não disponiveis";                     
            
        }

        public override string ToString()
        {
            return $"--- Detalhes do Contacto do Forneceddor (ID: {Id}) ---\n +" +
                $"{GetFormattedContactInfo()}";
        }
    }    
}
