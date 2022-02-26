using System.Threading.Tasks;

namespace Core.Access.Models.Strategy
{
    public interface IStrategy<TModel, TContext> where TModel : BaseModel, new() where TContext : AbstractStrategyContext<TModel>
    {
        TContext Context { get; set; }

        TModel Model { get; }

        Task<StrategyResult> Execute(TModel model = null);
    }
}