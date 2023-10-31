using Postgrest.Attributes;
using Postgrest.Models;

namespace BodyBuddy.Models
{
    [Table("Quotes")]
    public class QuoteModel : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("quote")]
        public string Quote { get; set; }

        [Column("author")]
        public string Author { get; set; }

    }
}
