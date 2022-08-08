using Core.Domain;
using Statement.Common.Events;

namespace Statement.Command.Domain.Aggregates
{
    public class StatementAggregate : AggregateRoot
    {
        private bool _active;
        private string _author;
        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active { get => _active; set => _active = value; }

        public StatementAggregate()
        {
        }

        public StatementAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new StatementCreatedEvent
            {
                Id = id,
                Author = author,
                Message = message,
                PostedAt = DateTime.UtcNow
            });
        }

        public void Apply(StatementCreatedEvent evt)
        {
            _id = evt.Id;
            _active = true;
            _author = evt.Author;
        }

        public void EditMessage(string author, string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the message of an inactive statement");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty. Please provide a valid {nameof(message)}");
            }

            RaiseEvent(new StatementUpdatedEvent
            {
                Id = _id,
                Message = message,
                Author = author
            });
        }

        public void Apply(StatementUpdatedEvent evt)
        {
            _id = evt.Id;
        }

        public void LikeStatement()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive statement");
            }

            RaiseEvent(new StatementLikedEvent
            {
                Id = _id
            });
        }

        public void Apply(StatementLikedEvent evt)
        {
            _id = evt.Id;
        }

        public void AddComment(string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add a comment to an inactive statement");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty. Please provide a valid {nameof(comment)}");
            }

            RaiseEvent(new CommentAddedEvent
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now
            });
        }

        public void Apply(CommentAddedEvent evt)
        {
            _id = evt.Id;
            _comments.Add(evt.CommentId, new Tuple<string, string>(evt.Comment, evt.Username));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit a comment of an inactive statement");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user");
            }

            RaiseEvent(new CommentUpdatedEvent
            {
                Id = _id,
                CommentId = commentId,
                Comment = comment,
                Username = username,
                EditDate = DateTime.Now
            });
        }

        public void Apply(CommentUpdatedEvent evt)
        {
            _id = evt.Id;
            _comments[evt.CommentId] = new Tuple<string, string>(evt.Comment, evt.Username);
        }

        public void RemoveComment(Guid commentId, string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove a comment of an inactive statement");
            }

            if (!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user");
            }

            RaiseEvent(new CommentRemovedEvent
            {
                Id = _id,
                CommentId = commentId
            });
        }

        public void Apply(CommentRemovedEvent evt)
        {
            _id = evt.Id;
            _comments.Remove(evt.CommentId);
        }

        public void DeleteStatement(string username)
        {
            if (!_active)
            {
                throw new InvalidOperationException("The statement has already been removed");
            }

            if (!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a statement that was made by somebody else");
            }

            RaiseEvent(new StatementRemovedEvent { Id = _id });
        }

        public void Apply(StatementRemovedEvent evt)
        {
            _id = evt.Id;
            _active = false;
        }
    }
}