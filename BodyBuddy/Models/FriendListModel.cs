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
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        [Column("friendId")]
        public int FriendId { get; set; }
    }
}
