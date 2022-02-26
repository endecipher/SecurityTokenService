using System;

namespace Core.Access.Identity.DB
{
    public abstract class BaseOperationResult
    {
        public bool IsSuccessful => Exception == null;

        public Exception Exception { get; set; } = null;
    }
}
