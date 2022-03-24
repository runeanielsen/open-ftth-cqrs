using FluentResults;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult> where TResult : Result
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
