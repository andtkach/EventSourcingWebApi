using Core.Queries;

namespace Statement.Query.Api.Queries
{
    public class FindStatementByIdQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}