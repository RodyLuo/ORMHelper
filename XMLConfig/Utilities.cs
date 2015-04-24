using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Ctrip.Flight.OrderProcess.DataBaseHelper.XMLConfig
{
    public class Utilities
    {
        private const string m_LongDefaultKey = "Nesc.Oversea";
        private const string m_LongDefaultIV = "Oversea3";

        public static T LoadFromXml<T>(string fileName)
        {
            FileStream fileStream = (FileStream)null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return (T)xmlSerializer.Deserialize((Stream)fileStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        public static void SaveToXml<T>(string fileName, T data)
        {
            FileStream fileStream = (FileStream)null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                xmlSerializer.Serialize((Stream)fileStream, (object)data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        public static string GetConfigurationFile(string appSection)
        {
            string path = ConfigurationManager.AppSettings[appSection];
            if (path == null)
                return "";
            if (!Path.IsPathRooted(path))
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.Replace('/', '\\').TrimStart('\\'));
            else
                return path.Replace('/', '\\').TrimStart('\\');
        }

        public static string GetFileFullPath(string fileName)
        {
            if (!Path.IsPathRooted(fileName))
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName.Replace('/', '\\').TrimStart('\\'));
            else
                return fileName.Replace('/', '\\').TrimStart('\\');
        }

        public static string GetConfigurationFullPath(string appSectionName)
        {
            string path = ConfigurationManager.AppSettings[appSectionName];
            if (path != null)
                path = Path.GetFullPath(path);
            return path;
        }

        public static string GetConfigurationValue(string appSection)
        {
            string str = string.Empty;
            return ConfigurationManager.AppSettings[appSection];
        }

        internal static string Encrypt(string encryptionText)
        {
            string str = string.Empty;
            if (encryptionText.Length > 0)
            {
                byte[] inArray = Utilities.Encrypt(Encoding.Unicode.GetBytes(encryptionText));
                if (inArray.Length > 0)
                    str = Convert.ToBase64String(inArray);
            }
            return str;
        }

        internal static string Decrypt(string encryptionText)
        {
            string str = string.Empty;
            if (encryptionText.Length > 0)
            {
                byte[] bytes = Utilities.Decrypt(Convert.FromBase64String(encryptionText));
                if (bytes.Length > 0)
                    str = Encoding.Unicode.GetString(bytes);
            }
            return str;
        }

        private static byte[] Encrypt(byte[] bytesData)
        {
            byte[] numArray = new byte[0];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                ICryptoTransform encryptor = Utilities.CreateAlgorithm().CreateEncryptor();
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    try
                    {
                        cryptoStream.Write(bytesData, 0, bytesData.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();
                        numArray = memoryStream.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error while writing decrypted data to the stream: \n" + ex.Message);
                    }
                }
                memoryStream.Close();
            }
            return numArray;
        }

        private static byte[] Decrypt(byte[] bytesData)
        {
            byte[] numArray = new byte[0];
            using (MemoryStream memoryStream = new MemoryStream())
            {
                ICryptoTransform decryptor = Utilities.CreateAlgorithm().CreateDecryptor();
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Write))
                {
                    try
                    {
                        cryptoStream.Write(bytesData, 0, bytesData.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();
                        numArray = memoryStream.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error while writing encrypted data to the stream: \n" + ex.Message);
                    }
                }
                memoryStream.Close();
            }
            return numArray;
        }

        private static Rijndael CreateAlgorithm()
        {
            Rijndael rijndael = (Rijndael)new RijndaelManaged();
            rijndael.Mode = CipherMode.CBC;
            byte[] bytes1 = Encoding.Unicode.GetBytes("Nesc.Oversea");
            byte[] bytes2 = Encoding.Unicode.GetBytes("Oversea3");
            rijndael.Key = bytes1;
            rijndael.IV = bytes2;
            return rijndael;
        }
    }
}
