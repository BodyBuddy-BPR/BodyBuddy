using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java.Util;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models
{
    [Table("UserRelations")]
    public class UserRelationsModel : BaseModel
    {
        [Column("user_id")]
        public string UserId { get; set; }

        [Column("friend_id")]
        public string FriendId { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Reference(typeof(UserModel))]
        public UserModel User { get; set; }

    }
}
