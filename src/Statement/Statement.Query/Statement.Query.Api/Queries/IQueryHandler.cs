using Statement.Query.Domain.Entities;

namespace Statement.Query.Api.Queries
{
    public interface IQueryHandler
    {
        Task<List<StatementEntity>> HandleAsync(FindAllStatementsQuery query);
        Task<List<StatementEntity>> HandleAsync(FindStatementByIdQuery query);
        Task<List<StatementEntity>> HandleAsync(FindStatementsByAuthorQuery query);
        Task<List<StatementEntity>> HandleAsync(FindStatementsWithCommentsQuery query);
        Task<List<StatementEntity>> HandleAsync(FindStatementsWithLikesQuery query);
    }
}