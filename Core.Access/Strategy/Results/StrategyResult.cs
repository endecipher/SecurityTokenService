namespace Core.Access.Models.Strategy
{
    public abstract class StrategyResult
    {
        public bool ShouldDisplayView { get; init; } = false;
    }
}
