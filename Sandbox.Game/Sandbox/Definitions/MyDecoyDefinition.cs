// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDecoyDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DecoyDefinition), null)]
  public class MyDecoyDefinition : MyCubeBlockDefinition
  {
    public float LightningRodRadiusLarge;
    public float LightningRodRadiusSmall;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_DecoyDefinition builderDecoyDefinition = builder as MyObjectBuilder_DecoyDefinition;
      this.LightningRodRadiusLarge = builderDecoyDefinition.LightningRodRadiusLarge;
      this.LightningRodRadiusSmall = builderDecoyDefinition.LightningRodRadiusSmall;
    }

    private class Sandbox_Definitions_MyDecoyDefinition\u003C\u003EActor : IActivator, IActivator<MyDecoyDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyDecoyDefinition();

      MyDecoyDefinition IActivator<MyDecoyDefinition>.CreateInstance() => new MyDecoyDefinition();
    }
  }
}
