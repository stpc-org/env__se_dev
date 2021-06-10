// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Utils.MyDataIntegrityChecker
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using VRage.FileSystem;

namespace Sandbox.Engine.Utils
{
  internal static class MyDataIntegrityChecker
  {
    public const int HASH_SIZE = 20;
    private static byte[] m_combinedData = new byte[40];
    private static byte[] m_hash = new byte[20];
    private static StringBuilder m_stringBuilder = new StringBuilder(8);

    public static void ResetHash() => Array.Clear((Array) MyDataIntegrityChecker.m_hash, 0, 20);

    public static void HashInFile(string fileName)
    {
      using (Stream data = MyFileSystem.OpenRead(fileName).UnwrapGZip())
        MyDataIntegrityChecker.HashInData(fileName.ToLower(), data);
      MySandboxGame.Log.WriteLine(MyDataIntegrityChecker.GetHashHex());
    }

    public static void HashInData(string dataName, Stream data)
    {
      using (HashAlgorithm hashAlgorithm = (HashAlgorithm) new SHA1Managed())
      {
        byte[] hash1 = hashAlgorithm.ComputeHash(data);
        byte[] hash2 = hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(dataName.ToCharArray()));
        Array.Copy((Array) hash1, (Array) MyDataIntegrityChecker.m_combinedData, 20);
        byte[] combinedData = MyDataIntegrityChecker.m_combinedData;
        Array.Copy((Array) hash2, 0, (Array) combinedData, 20, 20);
        byte[] hash3 = hashAlgorithm.ComputeHash(MyDataIntegrityChecker.m_combinedData);
        for (int index = 0; index < 20; ++index)
          MyDataIntegrityChecker.m_hash[index] ^= hash3[index];
      }
    }

    public static string GetHashHex()
    {
      uint num1 = 0;
      MyDataIntegrityChecker.m_stringBuilder.Clear();
      foreach (byte num2 in MyDataIntegrityChecker.m_hash)
      {
        MyDataIntegrityChecker.m_stringBuilder.AppendFormat("{0:x2}", (object) num2);
        num1 += (uint) num2;
      }
      return MyDataIntegrityChecker.m_stringBuilder.ToString();
    }

    public static string GetHashBase64() => Convert.ToBase64String(MyDataIntegrityChecker.m_hash);
  }
}
