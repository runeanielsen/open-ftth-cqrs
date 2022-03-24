using FluentResults;
using System.Threading.Tasks;

namespace OpenFTTH.CQRS
{
    public interface IQueryDispatcher
    {
        Task<TResult> HandleAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult> where TResult : Result;
    }
}
