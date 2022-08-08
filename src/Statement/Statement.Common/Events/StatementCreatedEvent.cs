using Core.Events;

namespace Statement.Common.Events
{
    public class StatementCreatedEvent : BaseEvent
    {
        public StatementCreatedEvent() : base(nameof(StatementCreatedEvent))
        {
        }

        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime PostedAt { get; set; }
    }
}