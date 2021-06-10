// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyEngineerToolBaseDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_EngineerToolBaseDefinition), null)]
  public class MyEngineerToolBaseDefinition : MyHandItemDefinition
  {
    public float SpeedMultiplier;
    public float DistanceMultiplier;
    public string Flare;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_EngineerToolBaseDefinition toolBaseDefinition = builder as MyObjectBuilder_EngineerToolBaseDefinition;
      this.SpeedMultiplier = toolBaseDefinition.SpeedMultiplier;
      this.DistanceMultiplier = toolBaseDefinition.DistanceMultiplier;
      this.Flare = toolBaseDefinition.Flare;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_EngineerToolBaseDefinition objectBuilder = (MyObjectBuilder_EngineerToolBaseDefinition) base.GetObjectBuilder();
      objectBuilder.SpeedMultiplier = this.SpeedMultiplier;
      objectBuilder.DistanceMultiplier = this.DistanceMultiplier;
      objectBuilder.Flare = this.Flare;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Definitions_MyEngineerToolBaseDefinition\u003C\u003EActor : IActivator, IActivator<MyEngineerToolBaseDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyEngineerToolBaseDefinition();

      MyEngineerToolBaseDefinition IActivator<MyEngineerToolBaseDefinition>.CreateInstance() => new MyEngineerToolBaseDefinition();
    }
  }
}
