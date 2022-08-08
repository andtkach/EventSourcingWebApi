using Core.Commands;

namespace Statement.Command.Api.Commands
{
    public class NewStatementCommand : BaseCommand
    {
        public string Author { get; set; }
        public string Message { get; set; }
    }
}