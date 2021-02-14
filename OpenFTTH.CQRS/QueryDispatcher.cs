using System;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<TResult> HandleAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var service = this._serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResult>)) as IQueryHandler<TQuery, TResult>;

            if (service == null)
                throw new ApplicationException($"The Query Dispatcher cannot find query handler: {typeof(TQuery).Name} Notice that you can use the AddCQRS extension in OpenFTTH.CQRS to easily add command and query handlers.");

            return await service.HandleAsync(query);
        }
    }
}
