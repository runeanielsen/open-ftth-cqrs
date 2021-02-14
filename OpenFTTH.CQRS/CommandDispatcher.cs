using OpenFTTH.CQRS;
using System;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public async Task<TResult> HandleAsync<TCommand, TResult>(TCommand query) where TCommand : ICommand<TResult>
        {
            var service = this._serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResult>)) as ICommandHandler<TCommand, TResult>;

            if (service == null)
                throw new ApplicationException($"The Command Dispatcher cannot find command handler: {typeof(TCommand).Name} Notice that you can use the AddCQRS extension in OpenFTTH.CQRS to easily add command and query handlers.");

            return await service.HandleAsync(query);
        }
    }
}
