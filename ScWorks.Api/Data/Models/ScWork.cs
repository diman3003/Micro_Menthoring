using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ScWorks.Api.Data.Models
{
    [Table("sc_works")]
    public class ScWork
    {
        [Column("sc_works_id")]
        public Guid Id { get; set; }
        [Column("author")]
        public string Author { get; set; }
        [Column("topic")]
        public string Topic { get; set; }
        [Column("annotation")]
        public string Annotation { get; set; }
        [Column("published")]
        public DateOnly? Published { get; set; }
        [Column("edited")]
        public DateOnly? Edited { get; set; }

        [NotMapped]
        public string FilePath { get; set; }
    }
}
