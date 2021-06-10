// Decompiled with JetBrains decompiler
// Type: VRage.Http.MyHttpTools
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Text;

namespace VRage.Http
{
  public static class MyHttpTools
  {
    private static bool ByteArrayHasPrefix(byte[] prefix, byte[] byteArray)
    {
      if (prefix == null || byteArray == null || prefix.Length > byteArray.Length)
        return false;
      for (int index = 0; index < prefix.Length; ++index)
      {
        if ((int) prefix[index] != (int) byteArray[index])
          return false;
      }
      return true;
    }

    public static string ConvertToString(byte[] data, string preferredEncodingName = null)
    {
      if (data == null || data.Length == 0)
        return (string) null;
      Encoding encoding = (Encoding) null;
      int index1 = 0;
      if (!string.IsNullOrEmpty(preferredEncodingName))
      {
        try
        {
          encoding = Encoding.GetEncoding(preferredEncodingName);
          index1 = !MyHttpTools.ByteArrayHasPrefix(encoding.GetPreamble(), data) ? 0 : encoding.GetPreamble().Length;
        }
        catch (Exception ex)
        {
          encoding = Encoding.UTF8;
        }
      }
      if (encoding == null)
      {
        Encoding[] encodingArray = new Encoding[4]
        {
          Encoding.UTF8,
          Encoding.UTF32,
          Encoding.Unicode,
          Encoding.BigEndianUnicode
        };
        for (int index2 = 0; index2 < encodingArray.Length; ++index2)
        {
          byte[] preamble = encodingArray[index2].GetPreamble();
          if (MyHttpTools.ByteArrayHasPrefix(preamble, data))
          {
            encoding = encodingArray[index2];
            index1 = preamble.Length;
            break;
          }
        }
      }
      if (encoding == null)
      {
        encoding = Encoding.UTF8;
        if (index1 == -1)
        {
          byte[] preamble = encoding.GetPreamble();
          index1 = !MyHttpTools.ByteArrayHasPrefix(preamble, data) ? 0 : preamble.Length;
        }
      }
      return encoding.GetString(data, index1, data.Length - index1);
    }
  }
}
