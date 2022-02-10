using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace Utilities
{
    public class Utils
    {
        public byte[] IV = Encoding.ASCII.GetBytes("Devjoker7.37hAES");

        public static double DateDiffHours(DateTime start, DateTime end, ref string strError)
        {
            double totalHours = -1;
            try
            {
                if (end > start)
                {
                    TimeSpan t = end - start;
                    totalHours = t.TotalHours;
                }
                else
                    strError = "Start date is greater than end date";
            }
            catch (Exception ex)
            {
                strError = ex.InnerException != null ?
                    ex.Message + " (" + ex.InnerException.Message + ")" :
                    ex.Message;
            }
            return totalHours;
        }

        public static double DateDiffSeconds(DateTime start, DateTime end, ref string strError)
        {
            double totalSeconds = -1;
            try
            {
                if (end > start)
                {
                    TimeSpan t = end - start;
                    totalSeconds = t.TotalSeconds;
                }
                else
                    strError = "Start date is greater than end date";
            }
            catch (Exception ex)
            {
                strError = ex.InnerException != null ?
                    ex.Message + " (" + ex.InnerException.Message + ")" :
                    ex.Message;
            }
            return totalSeconds;
        }

        public static int DateDiffDays(DateTime start, DateTime end, ref string strError)
        {
            int totalDays = -1;
            try
            {
                if (end > start)
                {
                    TimeSpan t = end - start;
                    totalDays = Convert.ToInt32(Math.Round(t.TotalDays, 0));
                }
                else
                    strError = "Start date is greater than end date";
            }
            catch (Exception ex)
            {
                strError = ex.InnerException != null ?
                    ex.Message + " (" + ex.InnerException.Message + ")" :
                    ex.Message;
            }
            return totalDays;
        }

        public static string encodePassword(string originalPassword)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();

            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(originalPassword);
            byte[] hash = sha1.ComputeHash(inputBytes);

            return Convert.ToBase64String(hash);
        }
       public static string EncryptStringToBytes_Aes(string plainText)
        {
            string valueIV = ConfigurationManager.AppSettings["InitialVector"].ToString();
            byte[] IV = valueIV.Split(new[] { ',' }).Select(s => Convert.ToByte(s)).ToArray();
            string valueKeyAlgorithm = ConfigurationManager.AppSettings["KeyAlgorithm"].ToString();
            byte[] Key = valueKeyAlgorithm.Split(new[] { ',' }).Select(s => Convert.ToByte(s)).ToArray();
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes in base 64 string from the memory stream.
            return Convert.ToBase64String(encrypted);

        }
       public static string DecryptStringFromBytes_Aes(string EncriptedText)
       {
           string valueIV = ConfigurationManager.AppSettings["InitialVector"].ToString();
           byte[] IV = valueIV.Split(new[] { ',' }).Select(s => Convert.ToByte(s)).ToArray();
           string valueKeyAlgorithm = ConfigurationManager.AppSettings["KeyAlgorithm"].ToString();
           byte[] Key = valueKeyAlgorithm.Split(new[] { ',' }).Select(s => Convert.ToByte(s)).ToArray();
           // Check arguments.
           if (EncriptedText == null || EncriptedText.Length <= 0)
               throw new ArgumentNullException("EncriptedText");
           if (Key == null || Key.Length <= 0)
               throw new ArgumentNullException("Key");
           if (IV == null || IV.Length <= 0)
               throw new ArgumentNullException("IV");
           // convert base 64 string to array of bytes
           byte[] cipherText = System.Convert.FromBase64String(EncriptedText);
           // Declare the string used to hold
           // the decrypted text.
           string plaintext = null;

           // Create an AesManaged object
           // with the specified key and IV.
           using (AesManaged aesAlg = new AesManaged())
           {
               aesAlg.Key = Key;
               aesAlg.IV = IV;

               // Create a decryptor to perform the stream transform.
               ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

               // Create the streams used for decryption.
               using (MemoryStream msDecrypt = new MemoryStream(cipherText))
               {
                   using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                   {
                       using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                       {

                           // Read the decrypted bytes from the decrypting stream
                           // and place them in a string.
                           plaintext = srDecrypt.ReadToEnd();
                       }
                   }
               }

           }

           return plaintext;

       }
    }
}
