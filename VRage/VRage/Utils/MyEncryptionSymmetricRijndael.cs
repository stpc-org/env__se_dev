// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyEncryptionSymmetricRijndael
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VRage.Utils
{
  public static class MyEncryptionSymmetricRijndael
  {
    public static string EncryptString(string inputText, string password)
    {
      if (inputText.Length <= 0)
        return "";
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      byte[] bytes1 = Encoding.Unicode.GetBytes(inputText);
      byte[] bytes2 = Encoding.ASCII.GetBytes(password.Length.ToString());
      PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(password, bytes2);
      byte[] bytes3 = passwordDeriveBytes.GetBytes(32);
      byte[] bytes4 = passwordDeriveBytes.GetBytes(16);
      ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes3, bytes4);
      MemoryStream memoryStream = new MemoryStream();
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write);
      cryptoStream.Write(bytes1, 0, bytes1.Length);
      cryptoStream.FlushFinalBlock();
      byte[] array = memoryStream.ToArray();
      memoryStream.Close();
      cryptoStream.Close();
      return Convert.ToBase64String(array);
    }

    public static string DecryptString(string inputText, string password)
    {
      if (inputText.Length <= 0)
        return "";
      RijndaelManaged rijndaelManaged = new RijndaelManaged();
      byte[] buffer = Convert.FromBase64String(inputText);
      byte[] bytes1 = Encoding.ASCII.GetBytes(password.Length.ToString());
      PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(password, bytes1);
      byte[] bytes2 = passwordDeriveBytes.GetBytes(32);
      byte[] bytes3 = passwordDeriveBytes.GetBytes(16);
      ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes2, bytes3);
      MemoryStream memoryStream = new MemoryStream(buffer);
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read);
      byte[] numArray = new byte[buffer.Length];
      int count = cryptoStream.Read(numArray, 0, numArray.Length);
      memoryStream.Close();
      cryptoStream.Close();
      return Encoding.Unicode.GetString(numArray, 0, count);
    }
  }
}
