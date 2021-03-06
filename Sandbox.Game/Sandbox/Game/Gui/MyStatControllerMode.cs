// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControllerMode
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControllerMode : MyStatBase
  {
    public MyStatControllerMode() => this.Id = MyStringHash.GetOrCompute("controller_mode");

    public override void Update() => this.CurrentValue = MyInput.Static.IsJoystickLastUsed ? 1f : 0.0f;
  }
}
