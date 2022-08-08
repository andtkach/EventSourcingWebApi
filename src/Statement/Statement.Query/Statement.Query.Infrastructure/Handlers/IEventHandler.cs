using Statement.Common.Events;

namespace Statement.Query.Infrastructure.Handlers
{
    public interface IEventHandler
    {
        Task On(StatementCreatedEvent evt);
        Task On(StatementUpdatedEvent evt);
        Task On(StatementLikedEvent evt);
        Task On(CommentAddedEvent evt);
        Task On(CommentUpdatedEvent evt);
        Task On(CommentRemovedEvent evt);
        Task On(StatementRemovedEvent evt);
    }
}