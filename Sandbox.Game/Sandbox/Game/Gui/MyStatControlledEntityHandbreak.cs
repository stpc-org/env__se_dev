// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControlledEntityHandbreak
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatControlledEntityHandbreak : MyStatBase
  {
    public MyStatControlledEntityHandbreak() => this.Id = MyStringHash.GetOrCompute("controlled_handbreak");

    public override void Update()
    {
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      if (controlledEntity == null || !(controlledEntity.Entity.Parent is MyCubeGrid parent))
        return;
      this.CurrentValue = parent.GridSystems.WheelSystem.HandBrake ? 1f : 0.0f;
    }
  }
}
