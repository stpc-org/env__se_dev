// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.DebugRenders.MyDebugRenderComponentSolarPanel
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using VRage.Game.Components;
using VRage.ModAPI;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
  public class MyDebugRenderComponentSolarPanel : MyDebugRenderComponent
  {
    private MyTerminalBlock m_solarBlock;
    private MySolarGameLogicComponent m_solarComponent;

    public MyDebugRenderComponentSolarPanel(MyTerminalBlock solarBlock)
      : base((IMyEntity) solarBlock)
    {
      this.m_solarBlock = solarBlock;
      MyGameLogicComponent component;
      if (this.m_solarBlock.Components.TryGet<MyGameLogicComponent>(out component))
        this.m_solarComponent = component as MySolarGameLogicComponent;
      MySolarGameLogicComponent solarComponent = this.m_solarComponent;
    }

    public override void DebugDraw()
    {
    }
  }
}
