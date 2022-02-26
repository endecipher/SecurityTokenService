using Core.Access.Identity.DB.EntitySets;
using System;
using System.Linq;

namespace Core.Access.Identity.DB
{
    public class Repository : IRepository
    {
        private readonly AccessDbContext context;
        private readonly ITransaction transaction;

        public Repository(AccessDbContext ctx, ITransaction transaction)
        {
            context = ctx;
            this.transaction = transaction;
        }

        public ITransaction Transaction => transaction;

        public EntityOperationResult CreateClient(Client client)
        {
            var res = new EntityOperationResult();

            try
            {
                context.Clients.Add(client);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult CreateClientCodeRequest(ClientCodeRequest request)
        {
            var res = new EntityOperationResult();

            try
            {
                context.ClientCodeRequests.Add(request);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult CreateClientTokenRequest(ClientTokenRequest request)
        {
            var res = new EntityOperationResult();

            try
            {
                context.ClientTokenRequests.Add(request);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult FetchClientById(string ClientId)
        {
            var res = new EntityOperationResult();

            try
            {
                var client = context.Clients.Find(ClientId);
                res.Entity = (IEntity)client;
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult FetchClientByFriendlyName(string name)
        {
            var res = new EntityOperationResult();

            try
            {
                var client = context.Clients.Where(x => x.FriendlyName.Equals(name)).FirstOrDefault();
                res.Entity = (IEntity)client;
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult FetchClientCodeRequest(Func<ClientCodeRequest, bool> filter)
        {
            var res = new EntityOperationResult();

            try
            {
                var request = context.ClientCodeRequests.Where(filter).First();
                res.Entity = (IEntity)request;
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult FetchClientTokenRequest(Func<ClientTokenRequest, bool> filter)
        {
            var res = new EntityOperationResult();

            try
            {
                var request = context.ClientTokenRequests.Where(filter).First();
                res.Entity = (IEntity)request;
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult UpdateClientCodeRequest(ClientCodeRequest request)
        {
            var res = new EntityOperationResult();

            try
            {
                var entry = context.ClientCodeRequests.Update(request);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }

        public EntityOperationResult UpdateClientTokenRequest(ClientTokenRequest request)
        {
            var res = new EntityOperationResult();

            try
            {
                var entry = context.ClientTokenRequests.Update(request);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                res.Exception = ex;
            }

            return res;
        }
    }
}
