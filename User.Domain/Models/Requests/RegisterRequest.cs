using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models.Requests
{
    public class RegisterRequest
    {

        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
