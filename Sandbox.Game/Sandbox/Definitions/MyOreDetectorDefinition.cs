// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyOreDetectorDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_OreDetectorDefinition), null)]
  public class MyOreDetectorDefinition : MyCubeBlockDefinition
  {
    public MyStringHash ResourceSinkGroup;
    public float MaximumRange;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_OreDetectorDefinition detectorDefinition = builder as MyObjectBuilder_OreDetectorDefinition;
      this.ResourceSinkGroup = MyStringHash.GetOrCompute(detectorDefinition.ResourceSinkGroup);
      this.MaximumRange = detectorDefinition.MaximumRange;
    }

    private class Sandbox_Definitions_MyOreDetectorDefinition\u003C\u003EActor : IActivator, IActivator<MyOreDetectorDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyOreDetectorDefinition();

      MyOreDetectorDefinition IActivator<MyOreDetectorDefinition>.CreateInstance() => new MyOreDetectorDefinition();
    }
  }
}
