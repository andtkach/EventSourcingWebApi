using Core.Events;

namespace Statement.Common.Events
{
    public class StatementUpdatedEvent : BaseEvent
    {
        public StatementUpdatedEvent() : base(nameof(StatementUpdatedEvent))
        {
        }

        public string Message { get; set; }
        public string Author { get; set; }
    }
}