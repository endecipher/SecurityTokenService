namespace Core.Access.Identity.DB
{
    public class StatefulOperationResult<T> : BaseOperationResult
    {
        public StatefulOperationResult(T State)
        {
            this.State = State;
        }

        public T State { get; }
    }
}
