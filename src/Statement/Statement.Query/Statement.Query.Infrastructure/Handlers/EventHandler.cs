using Statement.Common.Events;
using Statement.Query.Domain.Entities;
using Statement.Query.Domain.Repositories;

namespace Statement.Query.Infrastructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IStatementRepository _statementRepository;
        private readonly ICommentRepository _commentRepository;

        public EventHandler(IStatementRepository statementRepository, ICommentRepository commentRepository)
        {
            _statementRepository = statementRepository;
            _commentRepository = commentRepository;
        }

        public async Task On(StatementCreatedEvent evt)
        {
            var statement = new StatementEntity
            {
                Id = evt.Id,
                Author = evt.Author,
                Message = evt.Message,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = evt.Author,
                UpdatedAt = DateTime.MinValue,
                UpdatedBy = string.Empty
            };

            await _statementRepository.CreateAsync(statement);
        }

        public async Task On(StatementUpdatedEvent evt)
        {
            var statement = await _statementRepository.GetByIdAsync(evt.Id);

            if (statement == null) return;

            statement.Message = evt.Message;
            statement.UpdatedAt = DateTime.UtcNow;
            statement.UpdatedBy = evt.Author;
                
            await _statementRepository.UpdateAsync(statement);
        }

        public async Task On(StatementLikedEvent evt)
        {
            var statement = await _statementRepository.GetByIdAsync(evt.Id);

            if (statement == null) return;

            statement.Likes++;
            await _statementRepository.UpdateAsync(statement);
        }

        public async Task On(CommentAddedEvent evt)
        {
            var comment = new CommentEntity
            {
                StatementId = evt.Id,
                CommentId = evt.CommentId,
                CommentDate = evt.CommentDate,
                Comment = evt.Comment,
                Username = evt.Username,
                Edited = false
            };

            await _commentRepository.CreateAsync(comment);
        }

        public async Task On(CommentUpdatedEvent evt)
        {
            var comment = await _commentRepository.GetByIdAsync(evt.CommentId);

            if (comment == null) return;

            comment.Comment = evt.Comment;
            comment.Edited = true;
            comment.CommentDate = evt.EditDate;

            await _commentRepository.UpdateAsync(comment);
        }

        public async Task On(CommentRemovedEvent evt)
        {
            await _commentRepository.DeleteAsync(evt.CommentId);
        }

        public async Task On(StatementRemovedEvent evt)
        {
            await _statementRepository.DeleteAsync(evt.Id);
        }
    }
}