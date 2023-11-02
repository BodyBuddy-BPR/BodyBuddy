using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models
{
    [Table("FriendList")]
    public class FriendListModel : BaseModel
    {
        [PrimaryKey("userId")]
        public int? UserId { get; set; }

        [PrimaryKey("friendId")]
        public int? FriendId { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Reference(typeof(UserModel))]
        public UserModel User { get; set; }

    }
}
