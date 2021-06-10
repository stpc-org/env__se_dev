// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MySingleCrypto
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage.Utils
{
  public class MySingleCrypto
  {
    private readonly byte[] m_password;

    private MySingleCrypto()
    {
    }

    public MySingleCrypto(byte[] password) => this.m_password = (byte[]) password.Clone();

    public void Encrypt(byte[] data, int length)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < length; ++index2)
      {
        data[index2] = (byte) ((uint) data[index2] + (uint) this.m_password[index1]);
        index1 = (index1 + 1) % this.m_password.Length;
      }
    }

    public void Decrypt(byte[] data, int length)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < length; ++index2)
      {
        data[index2] = (byte) ((uint) data[index2] - (uint) this.m_password[index1]);
        index1 = (index1 + 1) % this.m_password.Length;
      }
    }
  }
}
