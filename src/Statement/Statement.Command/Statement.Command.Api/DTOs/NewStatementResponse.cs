using Statement.Common.DTOs;

namespace Statement.Command.Api.DTOs
{
    public class NewStatementResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}