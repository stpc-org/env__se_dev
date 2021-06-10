// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyWelderDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_WelderDefinition), null)]
  internal class MyWelderDefinition : MyEngineerToolBaseDefinition
  {
    public string FlameEffect;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      this.FlameEffect = (builder as MyObjectBuilder_WelderDefinition).FlameEffect;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_WelderDefinition objectBuilder = (MyObjectBuilder_WelderDefinition) base.GetObjectBuilder();
      objectBuilder.FlameEffect = this.FlameEffect;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Definitions_MyWelderDefinition\u003C\u003EActor : IActivator, IActivator<MyWelderDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyWelderDefinition();

      MyWelderDefinition IActivator<MyWelderDefinition>.CreateInstance() => new MyWelderDefinition();
    }
  }
}
