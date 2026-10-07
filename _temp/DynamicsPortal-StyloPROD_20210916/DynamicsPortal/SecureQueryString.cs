using GeneralAuxiliary;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DynamicsPortal
{
    public class SecureQueryString
    {
        public static string encrypt(string _clearText)
        {
            try
            {
                string encryptionKey = "!A&V2$P" + SessionVariables.getCurrentUserId() + "BbI%92]2";
                string clearText = _clearText.Trim();
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                HttpUtility.UrlEncode(clearText);
                return clearText;

                ////byte[] plainBytes = Encoding.UTF8.GetBytes(clearText);
                //////---- Encrypt text.
                ////string encryptedValue = Convert.ToBase64String(System.Web.Security.MachineKey.Protect(plainBytes, encryptionKey));
                //////--- URL encode to make encrypted value URL compatible.
                ////encryptedValue = HttpUtility.UrlEncode(encryptedValue);
                ////return encryptedValue;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return string.Empty;
            }
            finally
            { }
        }

        public static string decrypt(string _cipherText)
        {
            try
            {
                string encryptionKey = "!A&V2$P" + SessionVariables.getCurrentUserId() + "BbI%92]2";
                string cipherText = HttpUtility.UrlDecode(_cipherText);
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                ////byte[] bytes = Convert.FromBase64String(cipherText);
                ////byte[] output = System.Web.Security.MachineKey.Unprotect(bytes, encryptionKey);
                ////cipherText = Encoding.UTF8.GetString(output);

                return cipherText;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return string.Empty;
            }
            finally
            { }

        }

    }
}