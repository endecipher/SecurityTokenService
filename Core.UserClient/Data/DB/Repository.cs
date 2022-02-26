using Core.UserClient.Data.Domain;
using System;

namespace Core.UserClient.Data.DB
{
    public class Repository : IRepository
    {
        private readonly ClientDbContext context;

        public Repository(ClientDbContext context)
        {
            this.context = context;
        }

        public EntityOperationResult CreateUser(ClientUser user)
        {
            var res = new EntityOperationResult();

            try
            {
                context.ClientUsers.Add(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult FetchUserByUsername(string username)
        {
            var res = new EntityOperationResult();

            try
            {
                var client = context.ClientUsers.Find(username);
                res.Entity = (IEntity) client;
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }
    }
}
