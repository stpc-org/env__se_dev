// Decompiled with JetBrains decompiler
// Type: VRage.IMyImeProcessor
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

namespace VRage
{
  public interface IMyImeProcessor
  {
    bool IsComposing { get; }

    void Activate(IMyImeActiveControl textElement);

    void Deactivate();

    void RecaptureTopScreen(IVRageGuiScreen screenWithFocus);

    void RegisterActiveScreen(IVRageGuiScreen screen);

    void UnregisterActiveScreen(IVRageGuiScreen screen);

    void ProcessInvoke();

    void CaretRepositionReaction();
  }
}
