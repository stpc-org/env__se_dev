// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyIngameScriptComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Blocks;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.EntityComponents
{
  [MyEntityComponentDescriptor(typeof (MyObjectBuilder_MyProgrammableBlock), false, new string[] {})]
  public class MyIngameScriptComponent : MyGameLogicComponent
  {
    private MyProgrammableBlock m_block;
    private UpdateType m_nextUpdate;

    public UpdateType NextUpdate
    {
      get => this.m_nextUpdate;
      set
      {
        if (value != UpdateType.None)
        {
          this.m_nextUpdate |= value;
          this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }
        else
        {
          this.m_nextUpdate = value;
          this.NeedsUpdate = MyEntityUpdateEnum.NONE;
        }
      }
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
      base.Init(objectBuilder);
      this.m_block = (MyProgrammableBlock) this.Entity;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      UpdateType nextUpdate = this.m_nextUpdate;
      this.m_nextUpdate = UpdateType.None;
      if (nextUpdate == UpdateType.None)
        return;
      this.m_block.Run(string.Empty, nextUpdate);
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      this.NextUpdate |= UpdateType.Update1;
    }

    public override void UpdateBeforeSimulation10()
    {
      base.UpdateBeforeSimulation();
      this.NextUpdate |= UpdateType.Update10;
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation();
      this.NextUpdate |= UpdateType.Update100;
    }

    private class Sandbox_Game_EntityComponents_MyIngameScriptComponent\u003C\u003EActor : IActivator, IActivator<MyIngameScriptComponent>
    {
      object IActivator.CreateInstance() => (object) new MyIngameScriptComponent();

      MyIngameScriptComponent IActivator<MyIngameScriptComponent>.CreateInstance() => new MyIngameScriptComponent();
    }
  }
}
