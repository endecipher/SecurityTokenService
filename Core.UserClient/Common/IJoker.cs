namespace Core.UserClient.Common
{
    public interface IJoker
    {
        string GetRandomDadJoke();
        string GetRandomYoMommaJoke();
        string GetRandomChuckNorrisJoke(bool unSafe = false);
    }
}
