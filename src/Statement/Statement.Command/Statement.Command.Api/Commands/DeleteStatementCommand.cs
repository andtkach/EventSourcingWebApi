using Core.Commands;

namespace Statement.Command.Api.Commands
{
    public class DeleteStatementCommand : BaseCommand
    {
        public string Username { get; set; }
    }
}