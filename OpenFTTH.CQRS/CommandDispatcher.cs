using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ILogger<CommandDispatcher> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(ILogger<CommandDispatcher> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> HandleAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            if (_serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResult>)) is not ICommandHandler<TCommand, TResult> service)
                throw new ApplicationException($"The Command Dispatcher cannot find command handler: {typeof(TCommand).Name} Notice that you can use the AddCQRS extension in OpenFTTH.CQRS to easily add command and query handlers.");

            var cmdResult = await service.HandleAsync(command);

            if (command is BaseCommand baseCommand && cmdResult is Result result)
            {
                if (result.IsFailed && result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogWarning($"{ typeof(TCommand).Name } command with id {baseCommand.CmdId}, invoked by user: '{baseCommand.UserContext?.UserName}', failed with message: {error.Message}");
                    }
                }
            }

            return cmdResult;
        }
    }
}
