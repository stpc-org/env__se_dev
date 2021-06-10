// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Definitions.MyEnvironmentModuleBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage.Game;
using VRage.Library.Utils;
using VRage.ObjectBuilders;

namespace Sandbox.Game.WorldEnvironment.Definitions
{
  public abstract class MyEnvironmentModuleBase : IMyEnvironmentModule
  {
    protected MyLogicalEnvironmentSectorBase Sector;

    public virtual void ProcessItems(
      Dictionary<short, MyLodEnvironmentItemSet> items,
      int changedLodMin,
      int changedLodMax)
    {
      using (MyEnvironmentModelUpdateBatch modelUpdateBatch = new MyEnvironmentModelUpdateBatch(this.Sector))
      {
        foreach (KeyValuePair<short, MyLodEnvironmentItemSet> keyValuePair in items)
        {
          MyRuntimeEnvironmentItemInfo def;
          this.Sector.GetItemDefinition((ushort) keyValuePair.Key, out def);
          MyPhysicalModelCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPhysicalModelCollectionDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalModelCollectionDefinition), def.Subtype));
          if (definition != null)
          {
            foreach (int seed in keyValuePair.Value.Items)
            {
              float sample = MyHashRandomUtils.UniformFloatFromSeed(seed);
              MyDefinitionId modelDef = definition.Items.Sample(sample);
              modelUpdateBatch.Add(modelDef, seed);
            }
          }
        }
      }
    }

    public virtual void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob) => this.Sector = sector;

    public abstract void Close();

    public abstract MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder();

    public abstract void OnItemEnable(int item, bool enable);

    public abstract void HandleSyncEvent(int logicalItem, object data, bool fromClient);

    public virtual void DebugDraw()
    {
    }
  }
}
