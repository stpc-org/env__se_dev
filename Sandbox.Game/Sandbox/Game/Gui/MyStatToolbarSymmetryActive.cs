// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatToolbarSymmetryActive
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatToolbarSymmetryActive : MyStatBase
  {
    public MyStatToolbarSymmetryActive() => this.Id = MyStringHash.GetOrCompute("toolbar_symmetry");

    public override void Update() => this.CurrentValue = MyCubeBuilder.Static.UseSymmetry ? 1f : 0.0f;
  }
}
