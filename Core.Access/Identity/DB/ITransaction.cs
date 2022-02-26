using System;

namespace Core.Access.Identity.DB
{
    public interface ITransaction
    {
        StatefulOperationResult<T> Execute<T>(T State, Action<T, AccessDbContext> action);
    }
}