// Decompiled with JetBrains decompiler
// Type: VRage.Cryptography.MySHA256
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System.Security.Cryptography;

namespace VRage.Cryptography
{
  public static class MySHA256
  {
    private static bool m_supportsFips = true;

    private static SHA256 CreateInternal() => MySHA256.m_supportsFips ? (SHA256) new SHA256CryptoServiceProvider() : (SHA256) new SHA256Managed();

    public static SHA256 Create()
    {
      try
      {
        return MySHA256.CreateInternal();
      }
      catch
      {
        MySHA256.m_supportsFips = false;
        return MySHA256.CreateInternal();
      }
    }
  }
}
