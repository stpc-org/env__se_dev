// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyVoxelMapStorageDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_VoxelMapStorageDefinition), null)]
  public class MyVoxelMapStorageDefinition : MyDefinitionBase
  {
    public string StorageFile;
    public bool UseForProceduralRemovals;
    public bool UseForProceduralAdditions;
    public bool UseAsPrimaryProceduralAdditionShape;
    public float SpawnProbability;
    public HashSet<int> GeneratorVersions;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_VoxelMapStorageDefinition storageDefinition = (MyObjectBuilder_VoxelMapStorageDefinition) builder;
      this.StorageFile = storageDefinition.StorageFile;
      this.UseForProceduralRemovals = storageDefinition.UseForProceduralRemovals;
      this.UseForProceduralAdditions = storageDefinition.UseForProceduralAdditions;
      this.UseAsPrimaryProceduralAdditionShape = storageDefinition.UseAsPrimaryProceduralAdditionShape;
      this.SpawnProbability = storageDefinition.SpawnProbability;
      if (storageDefinition.ExplicitProceduralGeneratorVersions == null)
        return;
      this.GeneratorVersions = new HashSet<int>(((IEnumerable<string>) storageDefinition.ExplicitProceduralGeneratorVersions).Select<string, int>(new Func<string, int>(int.Parse)));
    }

    private class Sandbox_Definitions_MyVoxelMapStorageDefinition\u003C\u003EActor : IActivator, IActivator<MyVoxelMapStorageDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyVoxelMapStorageDefinition();

      MyVoxelMapStorageDefinition IActivator<MyVoxelMapStorageDefinition>.CreateInstance() => new MyVoxelMapStorageDefinition();
    }
  }
}
