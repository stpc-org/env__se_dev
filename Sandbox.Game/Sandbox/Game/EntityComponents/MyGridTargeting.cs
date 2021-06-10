// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.EntityComponents.MyGridTargeting
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
  public class MyGridTargeting : MyEntityComponentBase
  {
    private MyCubeGrid m_grid;
    private BoundingSphere m_queryLocal = new BoundingSphere(Vector3.Zero, float.MinValue);
    private List<MyEntity> m_targetRoots = new List<MyEntity>();
    private MyListDictionary<MyCubeGrid, MyEntity> m_targetBlocks = new MyListDictionary<MyCubeGrid, MyEntity>();
    private List<long> m_ownersB = new List<long>();
    private List<long> m_ownersA = new List<long>();
    private FastResourceLock m_scanLock = new FastResourceLock();
    private int m_lastScan;
    public bool AllowScanning = true;

    public FastResourceLock ScanLock => this.m_scanLock;

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.m_grid = this.Entity as MyCubeGrid;
      this.m_grid.OnBlockAdded += new Action<MySlimBlock>(this.m_grid_OnBlockAdded);
    }

    private void m_grid_OnBlockAdded(MySlimBlock obj)
    {
      if (!(obj.FatBlock is MyLargeTurretBase fatBlock))
        return;
      this.m_queryLocal.Include(new BoundingSphere(obj.FatBlock.PositionComp.LocalMatrixRef.Translation, fatBlock.SearchingRange));
      fatBlock.PropertiesChanged += new Action<MyTerminalBlock>(this.TurretOnPropertiesChanged);
    }

    private void TurretOnPropertiesChanged(MyTerminalBlock obj)
    {
      if (!(obj is MyLargeTurretBase myLargeTurretBase))
        return;
      this.m_queryLocal.Include(new BoundingSphere(obj.PositionComp.LocalMatrixRef.Translation, myLargeTurretBase.SearchingRange));
    }

    public List<MyEntity> TargetRoots
    {
      get
      {
        if (this.AllowScanning && MySession.Static.GameplayFrameCounter - this.m_lastScan > 100)
          this.Scan();
        return this.m_targetRoots;
      }
    }

    public MyListDictionary<MyCubeGrid, MyEntity> TargetBlocks
    {
      get
      {
        if (this.AllowScanning && MySession.Static.GameplayFrameCounter - this.m_lastScan > 100)
          this.Scan();
        return this.m_targetBlocks;
      }
    }

    private void Scan()
    {
      using (this.m_scanLock.AcquireExclusiveUsing())
      {
        if (!this.AllowScanning || MySession.Static.GameplayFrameCounter - this.m_lastScan <= 100)
          return;
        BoundingSphereD sphere = new BoundingSphereD(Vector3D.Transform(this.m_queryLocal.Center, this.m_grid.WorldMatrix), (double) this.m_queryLocal.Radius);
        this.m_targetRoots.Clear();
        this.m_targetBlocks.Clear();
        MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, this.m_targetRoots);
        MyMissiles.GetAllMissilesInSphere(ref sphere, this.m_targetRoots);
        int count = this.m_targetRoots.Count;
        this.m_ownersA.AddRange((IEnumerable<long>) this.m_grid.SmallOwners);
        this.m_ownersA.AddRange((IEnumerable<long>) this.m_grid.BigOwners);
        for (int index = 0; index < count; ++index)
        {
          if (this.m_targetRoots[index] is MyCubeGrid targetRoot && (targetRoot.Physics == null || targetRoot.Physics.Enabled))
          {
            bool flag = false;
            if (targetRoot.BigOwners.Count == 0 && targetRoot.SmallOwners.Count == 0)
            {
              foreach (long owner in this.m_ownersA)
              {
                if (MyIDModule.GetRelationPlayerBlock(owner, 0L) == MyRelationsBetweenPlayerAndBlock.NoOwnership)
                {
                  flag = true;
                  break;
                }
              }
            }
            else
            {
              this.m_ownersB.AddRange((IEnumerable<long>) targetRoot.BigOwners);
              this.m_ownersB.AddRange((IEnumerable<long>) targetRoot.SmallOwners);
              foreach (long owner in this.m_ownersA)
              {
                foreach (long user in this.m_ownersB)
                {
                  if (MyIDModule.GetRelationPlayerBlock(owner, user) == MyRelationsBetweenPlayerAndBlock.Enemies)
                  {
                    flag = true;
                    break;
                  }
                }
                if (flag)
                  break;
              }
              this.m_ownersB.Clear();
            }
            if (flag)
            {
              List<MyEntity> orAdd = this.m_targetBlocks.GetOrAdd(targetRoot);
              using (targetRoot.Pin())
              {
                if (!targetRoot.MarkedForClose)
                  targetRoot.Hierarchy.QuerySphere(ref sphere, orAdd);
              }
            }
            else
            {
              foreach (MyCubeBlock fatBlock in targetRoot.GetFatBlocks())
              {
                if (fatBlock is IMyComponentOwner<MyIDModule> myComponentOwner && myComponentOwner.GetComponent(out MyIDModule _))
                {
                  long ownerId = fatBlock.OwnerId;
                  foreach (long owner in this.m_ownersA)
                  {
                    if (MyIDModule.GetRelationPlayerBlock(owner, ownerId) == MyRelationsBetweenPlayerAndBlock.Enemies)
                    {
                      flag = true;
                      break;
                    }
                  }
                  if (flag)
                    break;
                }
              }
              if (flag)
              {
                List<MyEntity> orAdd = this.m_targetBlocks.GetOrAdd(targetRoot);
                if (!targetRoot.Closed)
                  targetRoot.Hierarchy.QuerySphere(ref sphere, orAdd);
              }
            }
          }
        }
        this.m_ownersA.Clear();
        for (int index = this.m_targetRoots.Count - 1; index >= 0; --index)
        {
          MyEntity targetRoot = this.m_targetRoots[index];
          if (targetRoot is MyDebrisBase || targetRoot is MyFloatingObject || targetRoot.Physics != null && !targetRoot.Physics.Enabled || (targetRoot.GetTopMostParent((Type) null).Physics == null || !targetRoot.GetTopMostParent((Type) null).Physics.Enabled))
            this.m_targetRoots.RemoveAtFast<MyEntity>(index);
        }
        this.m_lastScan = MySession.Static.GameplayFrameCounter;
      }
    }

    private static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant) => potentialDescendant.IsSubclassOf(potentialBase) || potentialDescendant == potentialBase;

    public override string ComponentTypeDebugString => nameof (MyGridTargeting);

    public void RescanIfNeeded()
    {
      if (!this.AllowScanning || MySession.Static.GameplayFrameCounter - this.m_lastScan <= 100)
        return;
      this.Scan();
    }

    private class Sandbox_Game_EntityComponents_MyGridTargeting\u003C\u003EActor : IActivator, IActivator<MyGridTargeting>
    {
      object IActivator.CreateInstance() => (object) new MyGridTargeting();

      MyGridTargeting IActivator<MyGridTargeting>.CreateInstance() => new MyGridTargeting();
    }
  }
}
