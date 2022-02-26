using Core.Access.Identity.DB;
using Core.Access.Models.Strategy.Results;
using Core.Access.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Core.Access.Models.Strategy
{
    public abstract class AbstractStrategy<TModel, TContext> : IStrategy<TModel, TContext> where TModel : BaseModel, new() where TContext : AbstractStrategyContext<TModel>, new()
    {
        public AbstractStrategy(IRepository repository, UserManager<IdentityUser> userManager, IUtilities utilities)
        {
            Repository = repository;
            UserManager = userManager;
            Utilities = utilities;
        }

        protected IRepository Repository { get; }
        protected UserManager<IdentityUser> UserManager { get; }
        protected IUtilities Utilities { get; }

        protected virtual void Initialize() { }

        protected abstract Task<bool> Validate();

        protected abstract Task<StrategyResult> Process();

        protected StrategyResult Result { get; set; }

        public TContext Context { get; set; } 

        public TModel Model => Context.Model;

        public async Task<StrategyResult> Execute(TModel model = null)
        {
            try
            {
                Context = new TContext();

                Context.Model = model;

                Initialize();

                if (Context.IsDisposed)
                {
                    throw new ObjectDisposedException(nameof(TContext));
                }

                if (await Validate())
                {
                    Result = await Process();
                }
            }
            catch (Exception ex)
            {
                Result = new ExceptionResult(ex);
            }
            finally
            {
                Context.Dispose();
            }

            return Result;
        }
    }
}
