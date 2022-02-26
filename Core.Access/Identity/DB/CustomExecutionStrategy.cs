using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Core.Access.Identity.DB
{
    public class CustomExecutionStrategy : ICustomExecutionStrategy
    {
        private readonly AccessDbContext accessDbContext;

        public CustomExecutionStrategy(AccessDbContext accessDbContext)
        {
            this.accessDbContext = accessDbContext;
        }

        public TResult Execute<TState, TResult>(TState state, Func<DbContext, TState, TResult> operation, Func<DbContext, TState, ExecutionResult<TResult>> verifySucceeded)
        {
            using (var ctx = accessDbContext)
            {
                var strategy = ctx.Database.CreateExecutionStrategy();

                return strategy.Execute(state, operation, verifySucceeded);
            }
        }
    }
}
