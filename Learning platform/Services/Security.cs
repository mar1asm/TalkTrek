using System.Security.Cryptography;
using System.Text;

namespace Learning_platform.Services
{
    public class Security
    {
        /*public static string DecryptWithNonce(string encryptedData, string nonce)
        {
            // Convert Base64-encoded string to byte array
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] nonceBytes = Encoding.UTF8.GetBytes(nonce);

            // Extract IV from nonce
            byte[] iv = new byte[16];
            Buffer.BlockCopy(nonceBytes, 0, iv, 0, 16);

            // Extract key from nonce
            byte[] key = new byte[16];
            Buffer.BlockCopy(nonceBytes, 16, key, 0, 16);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // Create a buffer to hold the decrypted bytes
                        byte[] decryptedBytes = new byte[encryptedBytes.Length];
                        int decryptedByteCount = csDecrypt.Read(decryptedBytes, 0, decryptedBytes.Length);

                        // Convert the decrypted bytes to a string
                        string decryptedData = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedByteCount);
                        return decryptedData;
                    }
                }
            }
        }*/

        public static string DecryptPassword(string encryptedPassword, string secretKey)
        {
            // Convert the encrypted password from Base64 to bytes
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);

            // Convert the secret key to bytes
            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            // Extract the IV (first 16 bytes) from the encrypted password
            byte[] iv = new byte[16];
            Buffer.BlockCopy(encryptedBytes, 0, iv, 0, 16);

            // Extract the encrypted data (remaining bytes) from the encrypted password
            byte[] encryptedData = new byte[encryptedBytes.Length - 16];
            Buffer.BlockCopy(encryptedBytes, 16, encryptedData, 0, encryptedData.Length);

            // Create an AES decryptor with the key and IV
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create a decryptor to perform the stream transform
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create streams for decryption
                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Return the decrypted password
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
