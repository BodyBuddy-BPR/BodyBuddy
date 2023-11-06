using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;
using Java.Util;

namespace BodyBuddy.Mappers
{
    public class UserMapper
    {
        public UserModel MapToDatabase(UserDto userDto)
        {
            return new UserModel
            {
                Id = userDto.Id,
                Email = userDto.Email,
            };
        }


        public UserDto MapToDto(UserModel userModel)
        {
            return new UserDto
            {
                Id = userModel.Id.ToString(),
                Email = userModel.Email,
                Role = null
            };
        }

    }
}
