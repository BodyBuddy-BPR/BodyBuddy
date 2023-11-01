using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models
{
    [Table("Users")]
    public class UserModel : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("email")]
        public string Email { get; set; }

        public List<FriendListModel> Friends { get; set; }

    }
}
