using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class User : IdentityUser
    {
        //public string Username { get; set; }
        public string Email { get; set; }
        //public string Password { get; set; }
        public string Role { get; set; }

    }
}
