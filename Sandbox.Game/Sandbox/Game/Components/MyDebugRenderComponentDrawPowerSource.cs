// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentDrawPowerSource
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.EntityComponents;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.Game.Components
{
  public class MyDebugRenderComponentDrawPowerSource : MyDebugRenderComponent
  {
    private readonly MyResourceSourceComponent m_source;
    private IMyEntity m_entity;

    public MyDebugRenderComponentDrawPowerSource(MyResourceSourceComponent source, IMyEntity entity)
      : base((IMyEntity) null)
    {
      this.m_source = source;
      this.m_entity = entity;
    }

    public override void DebugDraw() => this.m_source.DebugDraw((Matrix) ref this.m_entity.PositionComp.WorldMatrixRef);
  }
}
