// Decompiled with JetBrains decompiler
// Type: VRage.Input.MyInput
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System;

namespace VRage.Input
{
  public class MyInput
  {
    public static bool EnableModifierKeyEmulation;

    public static IMyInput Static { get; set; }

    public static void Initialize(IMyInput implementation) => MyInput.Static = MyInput.Static == null ? implementation : throw new InvalidOperationException("Input already initialized.");

    public static void UnloadData()
    {
      if (MyInput.Static == null)
        return;
      MyInput.Static.UnloadData();
      MyInput.Static = (IMyInput) null;
    }
  }
}
