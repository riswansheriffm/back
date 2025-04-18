using KnomadixInfrastructure.AES256;
using KnomadixInfrastructure.RSACrypto;
using Microsoft.Extensions.Configuration;

namespace BackPack.Dependency.Library.Helpers
{
    public class Aes256Helper(IConfiguration configuration)
    {
        #region Aes256Encryption
        public string Aes256Encryption(string text)
        {
            string encryptedText = AES256.Encrypt(text, RSACrypto.Decrypt(configuration.GetSection("AES256Settings").GetSection("Password").Value ?? ""))!;

            return encryptedText;
        }
        #endregion

        #region Aes256Decryption
        public string Aes256Decryption(string encryptedText)
        {
            string decryptedText = AES256.Decrypt(encryptedText, RSACrypto.Decrypt(configuration.GetSection("AES256Settings").GetSection("Password").Value ?? ""))!;

            return decryptedText;
        }
        #endregion
    }
}
