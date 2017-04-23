using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class LoginModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string Token { get; set; }
    }

    public class ChangePasswordModel
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class RegisterModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Culture { get; set; }
        public string Password { get; set; }

    }
}