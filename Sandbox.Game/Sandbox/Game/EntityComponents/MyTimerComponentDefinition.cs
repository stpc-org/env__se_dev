// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyTimerComponentDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
  [MyDefinitionType(typeof (MyObjectBuilder_TimerComponentDefinition), null)]
  public class MyTimerComponentDefinition : MyComponentDefinitionBase
  {
    public float TimeToRemoveMin;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.TimeToRemoveMin = (builder as MyObjectBuilder_TimerComponentDefinition).TimeToRemoveMin;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_TimerComponentDefinition objectBuilder = base.GetObjectBuilder() as MyObjectBuilder_TimerComponentDefinition;
      objectBuilder.TimeToRemoveMin = this.TimeToRemoveMin;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Game_EntityComponents_MyTimerComponentDefinition\u003C\u003EActor : IActivator, IActivator<MyTimerComponentDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyTimerComponentDefinition();

      MyTimerComponentDefinition IActivator<MyTimerComponentDefinition>.CreateInstance() => new MyTimerComponentDefinition();
    }
  }
}
