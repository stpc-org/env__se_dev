// Decompiled with JetBrains decompiler
// Type: VRage.IMyCompressionLoad
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage
{
  public interface IMyCompressionLoad
  {
    int GetInt32();

    byte GetByte();

    int GetBytes(int bytes, byte[] output);

    bool EndOfFile();
  }
}
