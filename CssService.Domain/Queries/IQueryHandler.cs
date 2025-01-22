using MediatR;

namespace CssService.Domain.Queries
{
    internal interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
    }
}