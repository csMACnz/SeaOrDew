using System.Threading.Tasks;

namespace csMACnz.SeaOrDew
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}
