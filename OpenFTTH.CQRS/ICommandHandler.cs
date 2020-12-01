using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
