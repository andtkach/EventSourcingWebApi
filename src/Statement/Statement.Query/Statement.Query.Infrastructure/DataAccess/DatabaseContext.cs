using Microsoft.EntityFrameworkCore;
using Statement.Query.Domain.Entities;

namespace Statement.Query.Infrastructure.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<StatementEntity> Statements { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
    }
}