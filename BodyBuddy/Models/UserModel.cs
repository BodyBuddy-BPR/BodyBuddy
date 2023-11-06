﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java.Util;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models
{
    [Table("Users")]
    public class UserModel : BaseModel
    {
        [PrimaryKey("id")]
        public string Id { get; set; }

        [Column("email")]
        public string Email { get; set; }
    }
}
