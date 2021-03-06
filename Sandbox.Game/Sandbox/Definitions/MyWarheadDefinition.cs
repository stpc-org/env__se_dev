// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyWarheadDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_WarheadDefinition), null)]
  public class MyWarheadDefinition : MyCubeBlockDefinition
  {
    public float ExplosionRadius;
    public float WarheadExplosionDamage;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_WarheadDefinition warheadDefinition = (MyObjectBuilder_WarheadDefinition) builder;
      this.ExplosionRadius = warheadDefinition.ExplosionRadius;
      this.WarheadExplosionDamage = warheadDefinition.WarheadExplosionDamage;
    }

    private class Sandbox_Definitions_MyWarheadDefinition\u003C\u003EActor : IActivator, IActivator<MyWarheadDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyWarheadDefinition();

      MyWarheadDefinition IActivator<MyWarheadDefinition>.CreateInstance() => new MyWarheadDefinition();
    }
  }
}
