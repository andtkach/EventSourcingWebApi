using Microsoft.EntityFrameworkCore;
using Statement.Query.Domain.Entities;
using Statement.Query.Domain.Repositories;
using Statement.Query.Infrastructure.DataAccess;

namespace Statement.Query.Infrastructure.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public StatementRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(StatementEntity statement)
        {
            await using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Statements.Add(statement);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid statementId)
        {
            await using DatabaseContext context = _contextFactory.CreateDbContext();
            var statement = await GetByIdAsync(statementId);

            if (statement == null) return;

            context.Statements.Remove(statement);
            _ = await context.SaveChangesAsync();
        }

        public async Task<List<StatementEntity>> ListByAuthorAsync(string author)
        {
            await using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Statements.AsNoTracking()
                    .Include(i => i.Comments).AsNoTracking()
                    .Where(x => x.Author.Contains(author))
                    .ToListAsync();
        }

        public async Task<StatementEntity> GetByIdAsync(Guid statementId)
        {
            await using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Statements
                    .Include(i => i.Comments)
                    .FirstOrDefaultAsync(x => x.Id == statementId);
        }

        public async Task<List<StatementEntity>> ListAllAsync()
        {
            await using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Statements.AsNoTracking()
                    .Include(i => i.Comments).AsNoTracking()
                    .ToListAsync();
        }

        public async Task<List<StatementEntity>> ListWithCommentsAsync()
        {
            await using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Statements.AsNoTracking()
                    .Include(i => i.Comments).AsNoTracking()
                    .Where(x => x.Comments != null && x.Comments.Any())
                    .ToListAsync();
        }

        public async Task<List<StatementEntity>> ListWithLikesAsync(int numberOfLikes)
        {
            await using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Statements.AsNoTracking()
                    .Include(i => i.Comments).AsNoTracking()
                    .Where(x => x.Likes >= numberOfLikes)
                    .ToListAsync();
        }

        public async Task UpdateAsync(StatementEntity statement)
        {
            await using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Statements.Update(statement);

            _ = await context.SaveChangesAsync();
        }
    }
}