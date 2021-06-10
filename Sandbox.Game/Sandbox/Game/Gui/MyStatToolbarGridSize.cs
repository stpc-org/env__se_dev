// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatToolbarGridSize
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatToolbarGridSize : MyStatBase
  {
    public MyStatToolbarGridSize() => this.Id = MyStringHash.GetOrCompute("toolbar_grid_size");

    public override void Update()
    {
      if (!MyCubeBuilder.Static.IsActivated || MyCubeBuilder.Static.ToolbarBlockDefinition == null)
        this.CurrentValue = -1f;
      else
        this.CurrentValue = MyCubeBuilder.Static.ToolbarBlockDefinition.CubeSize == MyCubeSize.Small ? 0.0f : 1f;
    }
  }
}
