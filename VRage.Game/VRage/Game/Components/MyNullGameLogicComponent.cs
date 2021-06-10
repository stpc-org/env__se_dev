// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyNullGameLogicComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game.Components
{
  public class MyNullGameLogicComponent : MyGameLogicComponent
  {
    public override void UpdateOnceBeforeFrame()
    {
    }

    public override void UpdateBeforeSimulation()
    {
    }

    public override void UpdateBeforeSimulation10()
    {
    }

    public override void UpdateBeforeSimulation100()
    {
    }

    public override void UpdateAfterSimulation()
    {
    }

    public override void UpdateAfterSimulation10()
    {
    }

    public override void UpdateAfterSimulation100()
    {
    }

    public override void Init(MyObjectBuilder_EntityBase objectBuilder)
    {
    }

    public override void MarkForClose()
    {
    }

    public override void Close()
    {
    }

    public override MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false) => (MyObjectBuilder_EntityBase) null;

    private class VRage_Game_Components_MyNullGameLogicComponent\u003C\u003EActor : IActivator, IActivator<MyNullGameLogicComponent>
    {
      object IActivator.CreateInstance() => (object) new MyNullGameLogicComponent();

      MyNullGameLogicComponent IActivator<MyNullGameLogicComponent>.CreateInstance() => new MyNullGameLogicComponent();
    }
  }
}
