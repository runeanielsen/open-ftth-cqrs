using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger)
        {
            this._serviceProvider = serviceProvider;
            this._logger = logger;
        }

        public async Task<TResult> HandleAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult> where TResult : Result
        {
            var service = this._serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResult>)) as IQueryHandler<TQuery, TResult>;

            if (service == null)
                throw new ApplicationException($"The Query Dispatcher cannot find query handler: {typeof(TQuery).Name} Notice that you can use the AddCQRS extension in OpenFTTH.CQRS to easily add command and query handlers.");

            try
            {
                return await service.HandleAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogError("UNHANDLED_QUERY_EXCEPTION: " + ex.Message, ex);
                return (TResult)Result.Fail("UNHANDLED_QUERY_EXCEPTION: " + ex.Message);
            }
        }
    }
}
