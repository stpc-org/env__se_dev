// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.MyEnvironmentModelUpdateBatch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using VRage.Game;

namespace Sandbox.Game.WorldEnvironment
{
  public class MyEnvironmentModelUpdateBatch : IDisposable
  {
    private Dictionary<MyDefinitionId, MyEnvironmentModelUpdateBatch.ModelList> m_modelPerItemDefinition = new Dictionary<MyDefinitionId, MyEnvironmentModelUpdateBatch.ModelList>();
    private IMyEnvironmentOwner m_owner;
    private MyLogicalEnvironmentSectorBase m_sector;

    public MyEnvironmentModelUpdateBatch(MyLogicalEnvironmentSectorBase sector)
    {
      this.m_sector = sector;
      this.m_owner = this.m_sector.Owner;
    }

    public void Add(MyDefinitionId modelDef, int item)
    {
      MyEnvironmentModelUpdateBatch.ModelList modelList;
      if (!this.m_modelPerItemDefinition.TryGetValue(modelDef, out modelList))
      {
        modelList.Items = new List<int>();
        if (modelDef.TypeId.IsNull)
        {
          modelList.Model = (short) -1;
        }
        else
        {
          MyPhysicalModelDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPhysicalModelDefinition>(modelDef);
          modelList.Model = definition != null ? this.m_owner.GetModelId(definition) : (short) -1;
        }
        this.m_modelPerItemDefinition[modelDef] = modelList;
      }
      modelList.Items.Add(item);
    }

    public void Dispose() => this.Dispatch();

    public void Dispatch()
    {
      foreach (MyEnvironmentModelUpdateBatch.ModelList modelList in this.m_modelPerItemDefinition.Values)
        this.m_sector.UpdateItemModelBatch(modelList.Items, modelList.Model);
    }

    private struct ModelList
    {
      public List<int> Items;
      public short Model;
    }
  }
}
