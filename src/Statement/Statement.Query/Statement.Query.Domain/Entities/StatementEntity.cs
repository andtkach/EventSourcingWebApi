using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Statement.Query.Domain.Entities
{
    [Table("Statement", Schema = "dbo")]
    public class StatementEntity: TrackEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<CommentEntity> Comments { get; set; }
    }
}