using Core.Events;

namespace Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Guid _id;
        private readonly List<BaseEvent> _changes = new();

        public Guid Id
        {
            get { return _id; }
        }

        public int Version { get; set; } = -1;

        public IEnumerable<BaseEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        private void ApplyChange(BaseEvent evt, bool isNew)
        {
            var method = this.GetType().GetMethod("Apply", new Type[] { evt.GetType() });

            if (method == null)
            {
                throw new ArgumentNullException(nameof(method), $"The Apply method was not found in the aggregate for {evt.GetType().Name}");
            }

            method.Invoke(this, new object[] { evt });

            if (isNew)
            {
                _changes.Add(evt);
            }
        }

        protected void RaiseEvent(BaseEvent evt)
        {
            ApplyChange(evt, true);
        }

        public void ReplayEvents(IEnumerable<BaseEvent> events)
        {
            foreach (var evt in events)
            {
                ApplyChange(evt, false);
            }
        }
    }
}