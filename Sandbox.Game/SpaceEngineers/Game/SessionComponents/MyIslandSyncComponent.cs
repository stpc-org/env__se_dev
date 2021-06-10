// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.SessionComponents.MyIslandSyncComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  public class MyIslandSyncComponent : MySessionComponentBase
  {
    protected static Color[] m_colors = new Color[19]
    {
      new Color(0, 192, 192),
      Color.Orange,
      Color.BlueViolet * 1.5f,
      Color.BurlyWood,
      Color.Chartreuse,
      Color.CornflowerBlue,
      Color.Cyan,
      Color.ForestGreen,
      Color.Fuchsia,
      Color.Gold,
      Color.GreenYellow,
      Color.LightBlue,
      Color.LightGreen,
      Color.LimeGreen,
      Color.Magenta,
      Color.MintCream,
      Color.Orchid,
      Color.PeachPuff,
      Color.Purple
    };
    public static MyIslandSyncComponent Static = (MyIslandSyncComponent) null;
    private List<HkRigidBody> m_rigidBodies = new List<HkRigidBody>();
    private List<MyIslandSyncComponent.IslandData> m_rootIslands = new List<MyIslandSyncComponent.IslandData>();
    private Dictionary<IMyEntity, int> m_rootEntityIslandIndex = new Dictionary<IMyEntity, int>();

    public override void LoadData()
    {
      base.LoadData();
      MyIslandSyncComponent.Static = this;
      MyPositionComponent.SynchronizationEnabled = false;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MyIslandSyncComponent.Static = (MyIslandSyncComponent) null;
    }

    public override bool IsRequiredByGame => MyFakes.MP_ISLANDS;

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      this.m_rootIslands.Clear();
      this.m_rootEntityIslandIndex.Clear();
      if (!MyPhysics.GetClusterList().HasValue)
        return;
      foreach (HkWorld hkWorld in MyPhysics.GetClusterList().Value)
      {
        int simulationIslandsCount = hkWorld.GetActiveSimulationIslandsCount();
        for (int islandIndex = 0; islandIndex < simulationIslandsCount; ++islandIndex)
        {
          hkWorld.GetActiveSimulationIslandRigidBodies(islandIndex, this.m_rigidBodies);
          HashSet<IMyEntity> myEntitySet = (HashSet<IMyEntity>) null;
          foreach (HkEntity rigidBody in this.m_rigidBodies)
          {
            List<IMyEntity> allEntities = rigidBody.GetAllEntities();
            foreach (IMyEntity myEntity in allEntities)
            {
              IMyEntity topMostParent = myEntity.GetTopMostParent();
              foreach (MyIslandSyncComponent.IslandData rootIsland in this.m_rootIslands)
              {
                if (rootIsland.RootEntities.Contains(topMostParent))
                {
                  myEntitySet = rootIsland.RootEntities;
                  break;
                }
              }
            }
            allEntities.Clear();
          }
          if (myEntitySet == null)
          {
            MyIslandSyncComponent.IslandData islandData = new MyIslandSyncComponent.IslandData()
            {
              AABB = BoundingBoxD.CreateInvalid(),
              RootEntities = new HashSet<IMyEntity>(),
              ClientPriority = new Dictionary<ulong, float>()
            };
            myEntitySet = islandData.RootEntities;
            this.m_rootIslands.Add(islandData);
          }
          foreach (HkEntity rigidBody in this.m_rigidBodies)
          {
            List<IMyEntity> allEntities = rigidBody.GetAllEntities();
            foreach (IMyEntity myEntity in allEntities)
            {
              IMyEntity topMostParent = myEntity.GetTopMostParent();
              myEntitySet.Add(topMostParent);
            }
            allEntities.Clear();
          }
          this.m_rigidBodies.Clear();
        }
      }
      for (int index = 0; index < this.m_rootIslands.Count; ++index)
      {
        MyIslandSyncComponent.IslandData rootIsland = this.m_rootIslands[index];
        rootIsland.AABB = BoundingBoxD.CreateInvalid();
        foreach (IMyEntity rootEntity in rootIsland.RootEntities)
        {
          rootIsland.AABB.Include(rootEntity.PositionComp.WorldAABB);
          this.m_rootEntityIslandIndex[rootEntity] = index;
        }
        this.m_rootIslands[index] = rootIsland;
      }
    }

    protected static Color IndexToColor(int index) => MyIslandSyncComponent.m_colors[index % MyIslandSyncComponent.m_colors.Length];

    public override void Draw()
    {
      base.Draw();
      int index = 0;
      foreach (MyIslandSyncComponent.IslandData rootIsland in this.m_rootIslands)
      {
        MyRenderProxy.DebugDrawAABB(rootIsland.AABB, MyIslandSyncComponent.IndexToColor(index));
        string text = "Island " + (object) index + " : " + (object) rootIsland.RootEntities.Count + " root entities. Priorities: ";
        int num = 0;
        foreach (KeyValuePair<ulong, float> keyValuePair in rootIsland.ClientPriority)
        {
          text = text + "Client" + (object) num + ": " + (object) keyValuePair.Value;
          ++num;
        }
        MyRenderProxy.DebugDrawText2D(new Vector2(100f, (float) (index * 15)), text, MyIslandSyncComponent.IndexToColor(index), 0.7f);
        ++index;
      }
    }

    public bool GetIslandAABBForEntity(IMyEntity entity, out BoundingBoxD aabb)
    {
      aabb = BoundingBoxD.CreateInvalid();
      int index;
      if (!this.m_rootEntityIslandIndex.TryGetValue(entity, out index))
        return false;
      aabb = this.m_rootIslands[index].AABB;
      return true;
    }

    public void SetPriorityForIsland(IMyEntity entity, ulong client, float priority)
    {
      int index;
      if (!this.m_rootEntityIslandIndex.TryGetValue(entity, out index))
        return;
      this.m_rootIslands[index].ClientPriority[client] = priority;
    }

    public struct IslandData
    {
      public HashSet<IMyEntity> RootEntities;
      public BoundingBoxD AABB;
      public Dictionary<ulong, float> ClientPriority;
    }
  }
}
