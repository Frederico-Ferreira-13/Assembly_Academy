using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Core.Model;

namespace AppService.Interfaces
{
    public interface IUserService
    {
        User? Retrieve(int userId);
        void ChangePassword(int userId, string newPlainTextPassword);
        User ValidateLogin(string email, string plainTextPassword);

        User RegisterUserWithRole(string email, string userName, string plainTextPassword, UserRole userRole);

        int CountUsers();

        List<User> GetPendingUsersForApproval();
        void ApproveUser(int userId);
        void RejectAndDeleteUser(int userId);
    }
}
