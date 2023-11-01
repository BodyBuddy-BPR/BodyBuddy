using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserUid { get; set; }

        public string Role { get; set; }
    }
}