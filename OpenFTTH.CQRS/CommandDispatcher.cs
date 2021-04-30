using FluentResults;
using Microsoft.Extensions.Logging;
using OpenFTTH.EventSourcing;
using System;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly ILogger<CommandDispatcher> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventStore _eventStore;

        public CommandDispatcher(ILogger<CommandDispatcher> logger, IServiceProvider serviceProvider, IEventStore eventStore)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _eventStore = eventStore;
        }

        public async Task<TResult> HandleAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>
        {
            if (_serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResult>)) is not ICommandHandler<TCommand, TResult> service)
                throw new ApplicationException($"The Command Dispatcher cannot find command handler: {typeof(TCommand).Name} Notice that you can use the AddCQRS extension in OpenFTTH.CQRS to easily add command and query handlers.");

            var cmdResult = await service.HandleAsync(command);

            if (command is BaseCommand baseCommand && cmdResult is Result result)
            {
                if (baseCommand.CorrelationId == Guid.Empty)
                    _logger.LogError($"{ typeof(TCommand).Name } command has empty correlation id. Please make sure all initated commands has a correlation id set.");

                if (result.IsFailed && result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogWarning($"{ typeof(TCommand).Name } command with id {baseCommand.CmdId}, correlation id: {baseCommand.CorrelationId}, invoked by user: '{baseCommand.UserContext?.UserName}', failed with message: {error.Message}");
                    }
                }
                else
                {
                    _logger.LogInformation($"{ typeof(TCommand).Name } command with id {baseCommand.CmdId}, correlation id: {baseCommand.CorrelationId}, invoked by user: '{baseCommand.UserContext?.UserName}', was successfully processed.");
                }

                // Store command in event store
                if (_eventStore != null)
                {
                    var cmdLogEntry = new CommandLogEntry(baseCommand.CmdId, command, result);
                    _eventStore.CommandLog.Store(cmdLogEntry);
                }
            }

            return cmdResult;
        }
    }
}
