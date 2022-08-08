using Core.Infrastructure;
using Core.Queries;
using Statement.Query.Domain.Entities;

namespace Statement.Query.Infrastructure.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher<StatementEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<StatementEntity>>>> _handlers = new();

        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<StatementEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new IndexOutOfRangeException("You cannot register the same query handler twice");
            }

            _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
        }

        public async Task<List<StatementEntity>> SendAsync(BaseQuery query)
        {
            if (_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<StatementEntity>>> handler))
            {
                return await handler(query);
            }

            throw new ArgumentNullException(nameof(handler), "No query handler was registered");
        }
    }
}