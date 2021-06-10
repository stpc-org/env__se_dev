// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Modules.MyStaticEnvironmentModule
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.WorldEnvironment.Definitions;
using Sandbox.Game.WorldEnvironment.ObjectBuilders;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.WorldEnvironment.Modules
{
  public class MyStaticEnvironmentModule : MyEnvironmentModuleBase
  {
    private readonly HashSet<int> m_disabledItems = new HashSet<int>();
    private List<MyOrientedBoundingBoxD> m_boxes;
    private int m_minScannedLod = 15;

    public override void Init(MyLogicalEnvironmentSectorBase sector, MyObjectBuilder_Base ob)
    {
      base.Init(sector, ob);
      MyPlanetEnvironmentComponent owner = (MyPlanetEnvironmentComponent) sector.Owner;
      if (owner.CollisionCheckEnabled)
      {
        this.m_boxes = owner.GetCollidedBoxes(sector.Id);
        if (this.m_boxes != null)
          this.m_boxes = new List<MyOrientedBoundingBoxD>((IEnumerable<MyOrientedBoundingBoxD>) this.m_boxes);
      }
      MyObjectBuilder_StaticEnvironmentModule environmentModule = (MyObjectBuilder_StaticEnvironmentModule) ob;
      if (environmentModule != null)
      {
        HashSet<int> disabledItems = environmentModule.DisabledItems;
        foreach (int num in disabledItems)
        {
          if (!this.m_disabledItems.Contains(num))
            this.OnItemEnable(num, false);
        }
        this.m_disabledItems.UnionWith((IEnumerable<int>) disabledItems);
        if (environmentModule.Boxes != null && environmentModule.MinScanned > 0)
        {
          this.m_boxes = new List<MyOrientedBoundingBoxD>();
          foreach (SerializableOrientedBoundingBoxD box in environmentModule.Boxes)
            this.m_boxes.Add((MyOrientedBoundingBoxD) box);
          this.m_minScannedLod = environmentModule.MinScanned;
        }
      }
      if (this.m_boxes == null)
        return;
      Vector3D worldPos = sector.WorldPos;
      for (int index = 0; index < this.m_boxes.Count; ++index)
      {
        MyOrientedBoundingBoxD box = this.m_boxes[index];
        box.Center -= worldPos;
        this.m_boxes[index] = box;
      }
    }

    public override unsafe void ProcessItems(
      Dictionary<short, MyLodEnvironmentItemSet> items,
      int changedLodMin,
      int changedLodMax)
    {
      this.m_minScannedLod = changedLodMin;
      using (MyEnvironmentModelUpdateBatch modelUpdateBatch = new MyEnvironmentModelUpdateBatch(this.Sector))
      {
        foreach (KeyValuePair<short, MyLodEnvironmentItemSet> keyValuePair in items)
        {
          MyRuntimeEnvironmentItemInfo def;
          this.Sector.GetItemDefinition((ushort) keyValuePair.Key, out def);
          MyPhysicalModelCollectionDefinition definition = MyDefinitionManager.Static.GetDefinition<MyPhysicalModelCollectionDefinition>(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_PhysicalModelCollectionDefinition), def.Subtype));
          if (definition != null)
          {
            MyLodEnvironmentItemSet environmentItemSet = keyValuePair.Value;
            for (int index = environmentItemSet.LodOffsets[changedLodMin]; index < environmentItemSet.Items.Count; ++index)
            {
              int num = environmentItemSet.Items[index];
              if (!this.m_disabledItems.Contains(num) && !this.IsObstructed(num))
              {
                MyDefinitionId modelDef = definition.Items.Sample(MyHashRandomUtils.UniformFloatFromSeed(num));
                modelUpdateBatch.Add(modelDef, num);
              }
            }
          }
        }
      }
    }

    private bool IsObstructed(int position)
    {
      if (this.m_boxes != null)
      {
        ItemInfo itemInfo;
        this.Sector.GetItem(position, out itemInfo);
        for (int index = 0; index < this.m_boxes.Count; ++index)
        {
          if (this.m_boxes[index].Contains(ref itemInfo.Position))
            return true;
        }
      }
      return false;
    }

    public override void Close()
    {
    }

    public override MyObjectBuilder_EnvironmentModuleBase GetObjectBuilder()
    {
      if (this.m_disabledItems.Count <= 0)
        return (MyObjectBuilder_EnvironmentModuleBase) null;
      MyObjectBuilder_StaticEnvironmentModule environmentModule = new MyObjectBuilder_StaticEnvironmentModule()
      {
        DisabledItems = this.m_disabledItems,
        MinScanned = this.m_minScannedLod
      };
      if (this.m_boxes != null)
      {
        foreach (MyOrientedBoundingBoxD box in this.m_boxes)
          environmentModule.Boxes.Add((SerializableOrientedBoundingBoxD) box);
      }
      return (MyObjectBuilder_EnvironmentModuleBase) environmentModule;
    }

    public override void OnItemEnable(int itemId, bool enabled)
    {
      if (enabled)
        this.m_disabledItems.Remove(itemId);
      else
        this.m_disabledItems.Add(itemId);
      ItemInfo itemInfo;
      this.Sector.GetItem(itemId, out itemInfo);
      if (itemInfo.ModelIndex >= (short) 0 == enabled)
        return;
      short modelId = ~itemInfo.ModelIndex;
      this.Sector.UpdateItemModel(itemId, modelId);
    }

    public override void HandleSyncEvent(int logicalItem, object data, bool fromClient)
    {
    }

    public override void DebugDraw()
    {
      if (this.m_boxes == null)
        return;
      for (int index = 0; index < this.m_boxes.Count; ++index)
      {
        MyOrientedBoundingBoxD box = this.m_boxes[index];
        box.Center += this.Sector.WorldPos;
        MyRenderProxy.DebugDrawOBB(box, Color.Aquamarine, 0.3f, true, true);
      }
    }
  }
}
