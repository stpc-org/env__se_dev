// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.UseObject.MyActionDescription
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Utils;

namespace VRage.Game.Entity.UseObject
{
  public struct MyActionDescription
  {
    public MyStringId Text;
    public object[] FormatParams;
    public bool IsTextControlHint;
    public MyStringId? JoystickText;
    public object[] JoystickFormatParams;
    public bool ShowForGamepad;
  }
}
