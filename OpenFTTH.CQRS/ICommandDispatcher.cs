using System;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public interface ICommandDispatcher
    {
        Task<TResult> HandleAsync<TCommand, TResult>(TCommand command) where TCommand : ICommand<TResult>;
    }
}
