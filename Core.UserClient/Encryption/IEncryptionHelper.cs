namespace Core.UserClient.Encryption
{
    public interface IEncryptionHelper
    {
        string EncryptString(string plainText);

        string DecryptString(string cipherText);
    }
}