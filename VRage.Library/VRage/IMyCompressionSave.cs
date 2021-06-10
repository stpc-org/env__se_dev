// Decompiled with JetBrains decompiler
// Type: VRage.IMyCompressionSave
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;

namespace VRage
{
  public interface IMyCompressionSave : IDisposable
  {
    void Add(byte[] value);

    void Add(byte[] value, int count);

    void Add(float value);

    void Add(int value);

    void Add(byte value);
  }
}
