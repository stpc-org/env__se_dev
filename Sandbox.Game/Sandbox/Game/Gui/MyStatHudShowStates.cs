// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatHudShowStates
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatHudShowStates : MyStatBase
  {
    public MyStatHudShowStates() => this.Id = MyStringHash.GetOrCompute("hud_show_states");

    public override void Update()
    {
    }
  }
}
