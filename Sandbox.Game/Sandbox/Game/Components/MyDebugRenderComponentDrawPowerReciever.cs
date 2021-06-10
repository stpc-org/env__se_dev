// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentDrawPowerReciever
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using System;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Components
{
  public class MyDebugRenderComponentDrawPowerReciever : MyDebugRenderComponent
  {
    private readonly MyResourceSinkComponent m_sink;
    private IMyEntity m_entity;

    public MyDebugRenderComponentDrawPowerReciever(MyResourceSinkComponent sink, IMyEntity entity)
      : base((IMyEntity) null)
    {
      this.m_sink = sink;
      this.m_entity = entity;
      this.m_sink.IsPoweredChanged += new Action(this.IsPoweredChanged);
    }

    private void IsPoweredChanged()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS)
        return;
      MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotification(MyStringId.GetOrCompute(string.Format("{0} PowerChanged:{1}", (object) this.m_entity, (object) this.m_sink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))), 4000));
    }

    public override void DebugDraw() => this.m_sink.DebugDraw((Matrix) ref this.m_entity.PositionComp.WorldMatrixRef);
  }
}
