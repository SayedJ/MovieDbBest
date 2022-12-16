using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using webapp_cloudrun.Models;

namespace webapp_cloudrun.Auth
{
    public class AuthServiceImpl : IAuthService
    {

        private readonly IList<User> users = new List<User>
    {
        new User
        {
            Email = "jalilhu@gmail.com",
          
            Name = "Sayed Jalil",
            Password = "4335",
            Role = "Admin",
            Username = "jalilhu",
           
        },
        new User
        {
           
            Email = "jhus12b@gmail.com",
         
            Name = "Jakob Rasmussen",
            Password = "4335",
            Role = "Student",
            Username = "jalil",
     
        }
    };

  
        public async Task<User> RegisterUser(User user)
        {

            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ValidationException("Username cannot be null");
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ValidationException("Password cannot be null");
            }
            // Do more user info validation here

            // save to persistence instead of list

            return  user;
                
            
        }

        public Task<User> ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
