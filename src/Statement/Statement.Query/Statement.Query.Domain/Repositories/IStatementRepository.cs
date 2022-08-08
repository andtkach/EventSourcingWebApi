using Statement.Query.Domain.Entities;

namespace Statement.Query.Domain.Repositories
{
    public interface IStatementRepository
    {
        Task CreateAsync(StatementEntity statement);
        Task UpdateAsync(StatementEntity statement);
        Task DeleteAsync(Guid statementId);
        Task<StatementEntity> GetByIdAsync(Guid statementId);
        Task<List<StatementEntity>> ListAllAsync();
        Task<List<StatementEntity>> ListByAuthorAsync(string author);
        Task<List<StatementEntity>> ListWithLikesAsync(int numberOfLikes);
        Task<List<StatementEntity>> ListWithCommentsAsync();
    }
}