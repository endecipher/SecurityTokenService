using Core.Access.Identity.DB.EntitySets;
using System;

namespace Core.Access.Identity.DB
{
    public class EntityOperationResult : BaseOperationResult
    {
        public bool HasEntity => Entity != null;

        public IEntity Entity { get; set; } = null;

        public bool Evaluate(Action<EntityOperationResult> OnInternalFailure = null)
        {
            if (!IsSuccessful)
            {
                if (OnInternalFailure != null)
                    OnInternalFailure.Invoke(this);

                return false;
            }

            return true;
        }

        public bool Evaluate<T>(out T entity, Action<EntityOperationResult> OnInternalFailure = null, Action<EntityOperationResult> OnEntityNotFound = null) where T : IEntity
        {
            entity = default(T);

            if (!IsSuccessful)
            {
                if (OnInternalFailure != null)
                    OnInternalFailure.Invoke(this);

                return false;
            }

            if (!HasEntity)
            {
                if (OnEntityNotFound != null)
                    OnEntityNotFound.Invoke(this);

                return false;
            }

            entity = (T) Entity;
            return true;
        }
    }
}
