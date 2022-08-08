using Core.Events;

namespace Statement.Common.Events
{
    public class StatementLikedEvent : BaseEvent
    {
        public StatementLikedEvent() : base(nameof(StatementLikedEvent))
        {
        }
    }
}