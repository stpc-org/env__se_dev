// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyMultiBlockDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_MultiBlockDefinition), null)]
  public class MyMultiBlockDefinition : MyDefinitionBase
  {
    public MyMultiBlockDefinition.MyMultiBlockPartDefinition[] BlockDefinitions;
    public Vector3I Min;
    public Vector3I Max;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_MultiBlockDefinition multiBlockDefinition = builder as MyObjectBuilder_MultiBlockDefinition;
      if (multiBlockDefinition.BlockDefinitions == null || multiBlockDefinition.BlockDefinitions.Length == 0)
        return;
      this.BlockDefinitions = new MyMultiBlockDefinition.MyMultiBlockPartDefinition[multiBlockDefinition.BlockDefinitions.Length];
      for (int index = 0; index < multiBlockDefinition.BlockDefinitions.Length; ++index)
      {
        this.BlockDefinitions[index] = new MyMultiBlockDefinition.MyMultiBlockPartDefinition();
        MyObjectBuilder_MultiBlockDefinition.MyOBMultiBlockPartDefinition blockDefinition = multiBlockDefinition.BlockDefinitions[index];
        this.BlockDefinitions[index].Id = (MyDefinitionId) blockDefinition.Id;
        this.BlockDefinitions[index].Min = (Vector3I) blockDefinition.Position;
        this.BlockDefinitions[index].Forward = blockDefinition.Orientation.Forward;
        this.BlockDefinitions[index].Up = blockDefinition.Orientation.Up;
      }
    }

    public class MyMultiBlockPartDefinition
    {
      public MyDefinitionId Id;
      public Vector3I Min;
      public Vector3I Max;
      public Base6Directions.Direction Forward;
      public Base6Directions.Direction Up;
    }

    private class Sandbox_Definitions_MyMultiBlockDefinition\u003C\u003EActor : IActivator, IActivator<MyMultiBlockDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyMultiBlockDefinition();

      MyMultiBlockDefinition IActivator<MyMultiBlockDefinition>.CreateInstance() => new MyMultiBlockDefinition();
    }
  }
}
