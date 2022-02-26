using System;

namespace Core.Access.Models.Strategy
{
    public abstract class AbstractStrategyContext<TModel> : IDisposable where TModel : BaseModel, new()
    {
        public AbstractStrategyContext()
        {
        }

        public bool IsDisposed { get; private set; } = false;

        public TModel Model { get; set; }

        public void Dispose()
        {
            IsDisposed = true;
            Model = null;
        }
    }
}
