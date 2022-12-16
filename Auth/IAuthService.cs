using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using webapp_cloudrun.Models;

namespace webapp_cloudrun.Auth
{
    public interface IAuthService
    {


       Task<User> RegisterUser(User user);

       Task<User> ValidateUser(string username, string password);

       
    }
}
