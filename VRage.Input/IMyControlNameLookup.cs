// Decompiled with JetBrains decompiler
// Type: VRage.Input.IMyControlNameLookup
// Assembly: VRage.Input, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 02474C5E-7189-409A-98E6-D5E3CA7AB23A
// Assembly location: D:\Files\library_development\lib_se\VRage.Input.dll

namespace VRage.Input
{
  public interface IMyControlNameLookup
  {
    string GetKeyName(MyKeys key);

    string GetName(MyMouseButtonsEnum button);

    string GetName(MyJoystickButtonsEnum joystickButton);

    string GetName(MyJoystickAxesEnum joystickAxis);

    string UnassignedText { get; }
  }
}
