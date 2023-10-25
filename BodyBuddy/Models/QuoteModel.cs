using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
