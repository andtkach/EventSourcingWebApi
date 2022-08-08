using Core.Queries;

namespace Statement.Query.Api.Queries
{
    public class FindStatementsByAuthorQuery : BaseQuery
    {
        public string Author { get; set; }
    }
}