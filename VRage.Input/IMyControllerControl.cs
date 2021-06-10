// Decompiled with JetBrains decompiler
// Type: VRage.Input.IMyControllerControl
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

using System;

namespace VRage.Input
{
  public interface IMyControllerControl
  {
    byte Code { get; }

    Func<bool> Condition { get; }

    bool IsNewPressed();

    bool IsNewPressedRepeating();

    bool IsPressed();

    bool IsNewReleased();

    float AnalogValue();

    object ControlCode();

    bool IsNewPressedXinput();

    bool IsNewReleasedXinput();
  }
}
