using MediatR;

namespace CssService.Domain.Queries
{
    internal interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}