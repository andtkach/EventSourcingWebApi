namespace Statement.Command.Api.Commands
{
    public interface ICommandHandler
    {
        Task HandleAsync(NewStatementCommand command);
        Task HandleAsync(EditStatementCommand command);
        Task HandleAsync(LikeStatementCommand command);
        Task HandleAsync(AddCommentCommand command);
        Task HandleAsync(EditCommentCommand command);
        Task HandleAsync(RemoveCommentCommand command);
        Task HandleAsync(DeleteStatementCommand command);
        Task HandleAsync(RestoreReadDbCommand command);
    }
}