// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyUsableItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_UsableItemDefinition), null)]
  public class MyUsableItemDefinition : MyPhysicalItemDefinition
  {
    public string UseSound;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.UseSound = (builder as MyObjectBuilder_UsableItemDefinition).UseSound;
    }

    private class Sandbox_Definitions_MyUsableItemDefinition\u003C\u003EActor : IActivator, IActivator<MyUsableItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyUsableItemDefinition();

      MyUsableItemDefinition IActivator<MyUsableItemDefinition>.CreateInstance() => new MyUsableItemDefinition();
    }
  }
}
