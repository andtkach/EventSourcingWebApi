using Core.Queries;

namespace Statement.Query.Api.Queries
{
    public class FindStatementsWithLikesQuery : BaseQuery
    {
        public int NumberOfLikes { get; set; }
    }
}