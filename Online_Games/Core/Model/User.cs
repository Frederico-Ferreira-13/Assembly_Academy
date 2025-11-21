using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class User : BaseModel<int>
    {        
        public string? Pass { get; private set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public UserRole Role { get; set; }
        public bool IsApproved { get; set; } = false;

        // Construtor Necessário para o SeedAllData e RegisterNewUser
        public User(int id, UserRole role, string email, string userName, string password, bool isApproved)
            : base(id)
        {            
            this.Role = role;
            this.Email = email;
            this.UserName = userName;
            this.Pass = password;
            this.IsApproved = isApproved;
        }

        public User(int id, DateTime createdAt, DateTime? lastUpdatedAt, bool isActive,
                   string password, string email, string userName, UserRole role, bool isApproved)
            : base(id, createdAt, lastUpdatedAt, isActive)
        {
            this.Role = role;
            this.Email = email;
            this.UserName = userName;
            this.Pass = password;
            this.IsApproved = isApproved;
        }

        public void Approve()
        {
            if (this.IsApproved)
            {
                return;
            }

            this.IsApproved = true;
            base.LastUpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRole(UserRole newRole)
        {
            this.Role = newRole;
            base.LastUpdatedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(string newEmail)
        {
            this.Email = newEmail;
            base.LastUpdatedAt = DateTime.UtcNow;
        }

        public void UpdateUserName(string newUserName)
        {
            this.UserName = newUserName;
            base.LastUpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePassword(string newPassword)
        {
            this.Pass = newPassword;
            base.LastUpdatedAt = DateTime.UtcNow;
        }

        public bool VerifyPassword(string currentPassword)
        {
            return this.Pass == currentPassword;            
        }       

        public string GetUserSummary()
        {
            return $"ID: {this.Id} | Nickname: {this.UserName} | Email: {this.Email} | Role: {this.Role}";
        }
    }
}