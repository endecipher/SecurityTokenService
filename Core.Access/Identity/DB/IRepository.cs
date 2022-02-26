using Core.Access.Identity.DB.EntitySets;
using System;

namespace Core.Access.Identity.DB
{
    public interface IRepository
    {
        EntityOperationResult CreateClient(Client client);

        EntityOperationResult FetchClientById(string ClientId);

        EntityOperationResult FetchClientByFriendlyName(string clientName);

        EntityOperationResult CreateClientCodeRequest(ClientCodeRequest request);

        EntityOperationResult FetchClientCodeRequest(Func<ClientCodeRequest, bool> filter);

        EntityOperationResult UpdateClientCodeRequest(ClientCodeRequest request);

        EntityOperationResult CreateClientTokenRequest(ClientTokenRequest request);

        EntityOperationResult FetchClientTokenRequest(Func<ClientTokenRequest, bool> filter);

        EntityOperationResult UpdateClientTokenRequest(ClientTokenRequest request);

        ITransaction Transaction { get; }
    }
}