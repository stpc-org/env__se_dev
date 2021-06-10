// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyVoxelMapCollectionDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_VoxelMapCollectionDefinition), null)]
  public class MyVoxelMapCollectionDefinition : MyDefinitionBase
  {
    public MyDiscreteSampler<MyDefinitionId> StorageFiles;
    public MyStringHash Modifier;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      if (!(builder is MyObjectBuilder_VoxelMapCollectionDefinition collectionDefinition))
        return;
      List<MyDefinitionId> values = new List<MyDefinitionId>();
      List<float> floatList = new List<float>();
      for (int index = 0; index < collectionDefinition.StorageDefs.Length; ++index)
      {
        MyObjectBuilder_VoxelMapCollectionDefinition.VoxelMapStorage storageDef = collectionDefinition.StorageDefs[index];
        values.Add(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_VoxelMapStorageDefinition), storageDef.Storage));
        floatList.Add(storageDef.Probability);
      }
      this.StorageFiles = new MyDiscreteSampler<MyDefinitionId>(values, (IEnumerable<float>) floatList);
      this.Modifier = MyStringHash.GetOrCompute(collectionDefinition.Modifier);
    }

    private class Sandbox_Game_WorldEnvironment_Definitions_MyVoxelMapCollectionDefinition\u003C\u003EActor : IActivator, IActivator<MyVoxelMapCollectionDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyVoxelMapCollectionDefinition();

      MyVoxelMapCollectionDefinition IActivator<MyVoxelMapCollectionDefinition>.CreateInstance() => new MyVoxelMapCollectionDefinition();
    }
  }
}
