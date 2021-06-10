// Decompiled with JetBrains decompiler
// Type: VRage.IAfterMath
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;

namespace VRage
{
  public interface IAfterMath
  {
    int Init(IntPtr device);

    void Shutdown();

    string GetInfo(IntPtr context);

    void SetEventMarker(IntPtr nativePointer, string tag);
  }
}
