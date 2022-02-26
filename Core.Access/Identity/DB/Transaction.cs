using Core.Access.Utility.ConfigParser;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Transactions;

namespace Core.Access.Identity.DB
{
    public class Transaction : ITransaction
    {
        private readonly ICustomExecutionStrategy strategy;

        public Transaction(AccessDbContext accessDbContext, ICustomExecutionStrategy strategy, IConfigParser configParser)
        {
            AccessDbContext = accessDbContext;
            this.strategy = strategy;
            ConfigParser = configParser;
        }

        private AccessDbContext AccessDbContext { get; }
        public IConfigParser ConfigParser { get; }

        public StatefulOperationResult<T> Execute<T>(T State, Action<T, AccessDbContext> action)
        {
            var statefulResults = new StatefulOperationResult<T>(State);

            return strategy.Execute(statefulResults,
            operation: (dbContext, state) =>
            {
                using (var scope = new TransactionScope(scopeOption: TransactionScopeOption.RequiresNew, transactionOptions: new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = ConfigParser.TransactionTimeoutInSeconds
                }, 
                TransactionScopeAsyncFlowOption.Enabled))
                {

                    try
                    {
                        action.Invoke(state.State, AccessDbContext);
                        scope.Complete();
                    }
                    catch (Exception e)
                    {
                        state.Exception = e;
                        scope.Dispose();
                    }

                    return state;
                }
            },
            verifySucceeded: (dbContext, state) =>
            {
                return new ExecutionResult<StatefulOperationResult<T>>(state.IsSuccessful, state);
            });
        }
    }
}
