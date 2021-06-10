// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyGyroDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_GyroDefinition), null)]
  public class MyGyroDefinition : MyCubeBlockDefinition
  {
    public string ResourceSinkGroup;
    public float ForceMagnitude;
    public float RequiredPowerInput;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_GyroDefinition builderGyroDefinition = (MyObjectBuilder_GyroDefinition) builder;
      this.ResourceSinkGroup = builderGyroDefinition.ResourceSinkGroup;
      this.ForceMagnitude = builderGyroDefinition.ForceMagnitude;
      this.RequiredPowerInput = builderGyroDefinition.RequiredPowerInput;
    }

    private class Sandbox_Definitions_MyGyroDefinition\u003C\u003EActor : IActivator, IActivator<MyGyroDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyGyroDefinition();

      MyGyroDefinition IActivator<MyGyroDefinition>.CreateInstance() => new MyGyroDefinition();
    }
  }
}
