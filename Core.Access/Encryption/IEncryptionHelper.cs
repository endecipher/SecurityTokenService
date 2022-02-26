
namespace Core.Access.Encryption
{
    public interface IEncryptionHelper
    {
        string EncryptString(string plainText);

        string DecryptString(string cipherText);
    }
}