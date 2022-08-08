using Statement.Query.Domain.Entities;
using Statement.Query.Domain.Repositories;

namespace Statement.Query.Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IStatementRepository _statementRepository;

        public QueryHandler(IStatementRepository statementRepository)
        {
            _statementRepository = statementRepository;
        }

        public async Task<List<StatementEntity>> HandleAsync(FindAllStatementsQuery query)
        {
            return await _statementRepository.ListAllAsync();
        }

        public async Task<List<StatementEntity>> HandleAsync(FindStatementByIdQuery query)
        {
            var data = await _statementRepository.GetByIdAsync(query.Id);
            return new List<StatementEntity> { data };
        }

        public async Task<List<StatementEntity>> HandleAsync(FindStatementsByAuthorQuery query)
        {
            return await _statementRepository.ListByAuthorAsync(query.Author);
        }

        public async Task<List<StatementEntity>> HandleAsync(FindStatementsWithCommentsQuery query)
        {
            return await _statementRepository.ListWithCommentsAsync();
        }

        public async Task<List<StatementEntity>> HandleAsync(FindStatementsWithLikesQuery query)
        {
            return await _statementRepository.ListWithLikesAsync(query.NumberOfLikes);
        }
    }
}