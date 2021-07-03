using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ChartViewer.Helpers
{
    internal class CryptographicHelper
    {
        internal string DecryptData(string data)
        {
            string plaintext;
            var privateKey = Convert.ToString(ConfigurationManager.AppSettings["EncryptionCode"]);

            byte[] key = Encoding.UTF8.GetBytes(privateKey);                            //Convert AES-256 bit private key to byte array
            byte[] iv = new byte[16];                                                   //Initialize variable for initialization vector byte array
            byte[] phase = Convert.FromBase64String(data);                              //Convert encrypted data to byte array
            Array.Copy(phase, 0, iv, 0, iv.Length);                                     //Copy the first 16 bytes to set initialization vector
            byte[] cipherText = new byte[phase.Length - 16];                            //remove 16 bytes to get cipher length
            Array.Copy(phase, 16, cipherText, 0, cipherText.Length);                    //Copy bytes after 16-byte initialization vector to get cipher

            using (AesManaged aesAlg = new AesManaged())                                //Instantiate AesManaged to proceed with decryption
            {
                aesAlg.KeySize = 256;                                                   //Set key size to 256
                aesAlg.Mode = CipherMode.CBC;                                           //Set mode to cipher block chaining
                aesAlg.Padding = PaddingMode.PKCS7;                                     //Set padding bytes
                aesAlg.Key = key;                                                       //Set the private key converted from 256-bit string
                aesAlg.IV = iv;                                                         //Set the initialization vector

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;            
        }

        /// <summary>
        /// Encrypts the model to send as payload in URL
        /// </summary>
        /// <param name="objData"></param>
        /// <returns></returns>
        internal string EncryptData(string objData)
        {
            var privateKey = Convert.ToString(ConfigurationManager.AppSettings["EncryptionCode"]);
            byte[] key = Encoding.UTF8.GetBytes(privateKey);                            //Convert AES-256 bit private key to byte array

            var encryptedString = string.Empty;
            byte[] cipherText;
            byte[] phase;

            //var objStr = JsonConvert.SerializeObject(objData);

            using (AesManaged aesAlg = new AesManaged())                                //Instantiate AesManaged to proceed with encryption
            {
                aesAlg.KeySize = 256;                                                   //Set key size to 256
                aesAlg.Mode = CipherMode.CBC;                                           //Set mode to cipher block chaining
                aesAlg.Padding = PaddingMode.PKCS7;                                     //Set padding bytes
                aesAlg.Key = key;                                                       //Set the private key converted from 256-bit string
                aesAlg.GenerateIV();                                                    //Set the initialization vector

                // Create a encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. Version1 Passing MemoryStream empty.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(objData);
                        }

                        var arrLength = msEncrypt.ToArray().Length;

                        cipherText = new byte[arrLength];
                        Array.Copy(msEncrypt.ToArray(), 0, cipherText, 0, arrLength);   //Build the cipher text from bytes stream
                    }
                }

                phase = new byte[cipherText.Length + aesAlg.IV.Length];                 //Initialize the phase
                Array.Copy(aesAlg.IV, 0, phase, 0, aesAlg.IV.Length);                   //Copy Initialization Vector as first 16 bytes 
                Array.Copy(cipherText, 0, phase, aesAlg.IV.Length, cipherText.Length);  //Copy cipher text as the rest of the bytes

                encryptedString = Convert.ToBase64String(phase);                        //Convert to Base64 string
            }

            return encryptedString;
        }

        internal string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }
    }
}