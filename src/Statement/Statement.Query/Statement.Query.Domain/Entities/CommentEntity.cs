using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Statement.Query.Domain.Entities
{
    [Table("Comment", Schema = "dbo")]
    public class CommentEntity: TrackEntity
    {
        [Key]
        public Guid CommentId { get; set; }
        public string Username { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
        public bool Edited { get; set; }
        public Guid StatementId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual StatementEntity Statement { get; set; }
    }
}