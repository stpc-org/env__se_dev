// Decompiled with JetBrains decompiler
// Type: VRage.IVRageGuiScreen
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage
{
  public interface IVRageGuiScreen
  {
    IVRageGuiControl FocusedControl { get; set; }

    bool IsOpened { get; }

    void AddControl(IVRageGuiControl control);

    bool RemoveControl(IVRageGuiControl control);

    bool ContainsControl(IVRageGuiControl control);
  }
}
