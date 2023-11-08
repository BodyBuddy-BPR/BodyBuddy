﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BodyBuddy.Dtos;
using BodyBuddy.Models;

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


        public UserDto MapUserRelationsToDto(UserRelationsModel relationModel)
        {
            return new UserDto
            {
                Id = relationModel.FriendId,
                Email = relationModel.User.Email,
                Role = null,
                Type = relationModel.Type
            };
        }

    }
}