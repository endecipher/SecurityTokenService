using Core.UserClient.Data.Domain;

namespace Core.UserClient.Data.DB
{
    public interface IRepository
    {
        EntityOperationResult CreateUser(ClientUser client);

        EntityOperationResult FetchUserByUsername(string username);
    }
}