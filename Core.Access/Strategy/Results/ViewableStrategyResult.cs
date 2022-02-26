using System.Collections.Generic;
using System.Linq;

namespace Core.Access.Models.Strategy.Results
{
    public class ViewableStrategyResult : StrategyResult
    {
        public ViewableStrategyResult(BaseModel baseModel, string viewPath = null, bool clearModelState = false, IDictionary<string, object> viewDataAppends = null)
        {
            ShouldDisplayView = true;
            BaseModel = baseModel;
            ViewPath = viewPath;
            ClearModelState = clearModelState;
            ViewDataAppends = viewDataAppends;
        }

        public BaseModel BaseModel { get; }
        public string ViewPath { get; }
        public bool ClearModelState { get; } = false;
        public IDictionary<string, object> ViewDataAppends { get; }

        public bool HasViewPath => !string.IsNullOrEmpty(ViewPath);
        public bool HasViewDataToSet => ViewDataAppends != null && ViewDataAppends.Any();

    }
}
