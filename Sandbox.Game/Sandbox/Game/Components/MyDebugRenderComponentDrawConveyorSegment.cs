// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentDrawConveyorSegment
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems.Conveyors;
using VRage.ModAPI;

namespace Sandbox.Game.Components
{
  internal class MyDebugRenderComponentDrawConveyorSegment : MyDebugRenderComponent
  {
    private MyConveyorSegment m_conveyorSegment;

    public MyDebugRenderComponentDrawConveyorSegment(MyConveyorSegment conveyorSegment)
      : base((IMyEntity) null)
      => this.m_conveyorSegment = conveyorSegment;

    public override void DebugDraw() => this.m_conveyorSegment.DebugDraw();
  }
}
