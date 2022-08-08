using Core.Events;

namespace Statement.Common.Events
{
    public class StatementRemovedEvent : BaseEvent
    {
        public StatementRemovedEvent() : base(nameof(StatementRemovedEvent))
        {
        }
    }
}