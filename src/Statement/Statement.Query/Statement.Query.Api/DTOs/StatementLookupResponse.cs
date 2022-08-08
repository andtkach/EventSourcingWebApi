using Statement.Common.DTOs;
using Statement.Query.Domain.Entities;

namespace Statement.Query.Api.DTOs
{
    public class StatementLookupResponse : BaseResponse
    {
        public List<StatementEntity> Statements { get; set; }
    }
}