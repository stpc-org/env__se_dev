// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MySerialKey
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VRage.Utils
{
  public static class MySerialKey
  {
    private static int m_dataSize = 14;
    private static int m_hashSize = 4;

    public static string[] Generate(short productTypeId, short distributorId, int keyCount)
    {
      byte[] bytes1 = BitConverter.GetBytes(productTypeId);
      byte[] bytes2 = BitConverter.GetBytes(distributorId);
      using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
      {
        using (SHA1Managed shA1Managed = new SHA1Managed())
        {
          List<string> stringList = new List<string>(keyCount);
          byte[] numArray = new byte[MySerialKey.m_dataSize + MySerialKey.m_hashSize];
          for (int index1 = 0; index1 < keyCount; ++index1)
          {
            cryptoServiceProvider.GetBytes(numArray);
            numArray[0] = bytes1[0];
            numArray[1] = bytes1[1];
            numArray[2] = bytes2[0];
            numArray[3] = bytes2[1];
            for (int index2 = 0; index2 < 4; ++index2)
              numArray[index2] = (byte) ((uint) numArray[index2] ^ (uint) numArray[index2 + 4]);
            byte[] hash = shA1Managed.ComputeHash(numArray, 0, MySerialKey.m_dataSize);
            for (int index2 = 0; index2 < MySerialKey.m_hashSize; ++index2)
              numArray[MySerialKey.m_dataSize + index2] = hash[index2];
            stringList.Add(new string(My5BitEncoding.Default.Encode(((IEnumerable<byte>) numArray).ToArray<byte>())) + "X");
          }
          return stringList.ToArray();
        }
      }
    }

    public static string AddDashes(string key)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < key.Length; ++index)
      {
        if (index % 5 == 0 && index > 0)
          stringBuilder.Append('-');
        stringBuilder.Append(key[index]);
      }
      return stringBuilder.ToString();
    }

    public static string RemoveDashes(string key)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < key.Length; ++index)
      {
        if ((index + 1) % 6 != 0)
          stringBuilder.Append(key[index]);
      }
      return stringBuilder.ToString();
    }

    public static bool ValidateSerial(
      string serialKey,
      out int productTypeId,
      out int distributorId)
    {
      using (new RNGCryptoServiceProvider())
      {
        using (SHA1 shA1 = SHA1.Create())
        {
          if (serialKey.EndsWith("X"))
          {
            byte[] numArray = My5BitEncoding.Default.Decode(serialKey.Take<char>(serialKey.Length - 1).ToArray<char>());
            byte[] array = ((IEnumerable<byte>) numArray).Take<byte>(numArray.Length - MySerialKey.m_hashSize).ToArray<byte>();
            byte[] hash = shA1.ComputeHash(array);
            if (((IEnumerable<byte>) numArray).Skip<byte>(array.Length).Take<byte>(MySerialKey.m_hashSize).SequenceEqual<byte>(((IEnumerable<byte>) hash).Take<byte>(MySerialKey.m_hashSize)))
            {
              for (int index = 0; index < 4; ++index)
                array[index] = (byte) ((uint) array[index] ^ (uint) array[index + 4]);
              productTypeId = (int) BitConverter.ToInt16(array, 0);
              distributorId = (int) BitConverter.ToInt16(array, 2);
              return true;
            }
          }
          productTypeId = 0;
          distributorId = 0;
          return false;
        }
      }
    }
  }
}
