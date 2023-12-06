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

        public override bool Equals(object obj)
        {
            return obj is UserModel user && Id == user.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
