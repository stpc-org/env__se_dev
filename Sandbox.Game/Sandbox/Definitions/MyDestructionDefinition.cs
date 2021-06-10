// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDestructionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DestructionDefinition), null)]
  public class MyDestructionDefinition : MyDefinitionBase
  {
    public float DestructionDamage;
    public new string[] Icons;
    public float ConvertedFractureIntegrityRatio;
    public MyDestructionDefinition.MyFracturedPieceDefinition[] FracturedPieceDefinitions;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_DestructionDefinition destructionDefinition = builder as MyObjectBuilder_DestructionDefinition;
      this.DestructionDamage = destructionDefinition.DestructionDamage;
      this.Icons = destructionDefinition.Icons;
      this.ConvertedFractureIntegrityRatio = destructionDefinition.ConvertedFractureIntegrityRatio;
      if (destructionDefinition.FracturedPieceDefinitions == null || destructionDefinition.FracturedPieceDefinitions.Length == 0)
        return;
      this.FracturedPieceDefinitions = new MyDestructionDefinition.MyFracturedPieceDefinition[destructionDefinition.FracturedPieceDefinitions.Length];
      for (int index = 0; index < destructionDefinition.FracturedPieceDefinitions.Length; ++index)
        this.FracturedPieceDefinitions[index] = new MyDestructionDefinition.MyFracturedPieceDefinition()
        {
          Id = (MyDefinitionId) destructionDefinition.FracturedPieceDefinitions[index].Id,
          Age = destructionDefinition.FracturedPieceDefinitions[index].Age
        };
    }

    public void Merge(MyDestructionDefinition src)
    {
      this.DestructionDamage = src.DestructionDamage;
      this.Icons = src.Icons;
      this.ConvertedFractureIntegrityRatio = src.ConvertedFractureIntegrityRatio;
      this.FracturedPieceDefinitions = src.FracturedPieceDefinitions;
    }

    public class MyFracturedPieceDefinition
    {
      public MyDefinitionId Id;
      public int Age;
    }

    private class Sandbox_Definitions_MyDestructionDefinition\u003C\u003EActor : IActivator, IActivator<MyDestructionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyDestructionDefinition();

      MyDestructionDefinition IActivator<MyDestructionDefinition>.CreateInstance() => new MyDestructionDefinition();
    }
  }
}
