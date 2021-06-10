// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.WorldEnvironment.Modules.MyBreakableEnvironmentProxy
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Weapons;
using Sandbox.Game.WorldEnvironment.Definitions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.WorldEnvironment.Modules
{
  public class MyBreakableEnvironmentProxy : IMyEnvironmentModuleProxy
  {
    private const int BrokenItemLifeSpan = 20000;
    private int m_scheduledBreaksCount;
    private ConcurrentDictionary<int, MyBreakableEnvironmentProxy.BreakAtData> m_scheduledBreaks = new ConcurrentDictionary<int, MyBreakableEnvironmentProxy.BreakAtData>();
    private readonly Action m_BreakAtDelegate;
    private MyEnvironmentSector m_sector;

    public MyBreakableEnvironmentProxy() => this.m_BreakAtDelegate = new Action(this.BreakAtInvoke);

    public void Init(MyEnvironmentSector sector, List<int> items)
    {
      this.m_sector = sector;
      if (!Sync.IsServer)
        return;
      this.m_sector.OnContactPoint += new MySectorContactEvent(this.SectorOnContactPoint);
    }

    private void SectorOnContactPoint(
      int itemId,
      MyEntity other,
      ref MyPhysics.MyContactPointEvent e)
    {
      if (this.m_sector.DataView.Items[itemId].ModelIndex < (short) 0)
        return;
      float num1 = Math.Abs(e.ContactPointEvent.SeparatingVelocity);
      if (other == null || other.Physics == null || (other is MyFloatingObject || other is IMyHandheldGunObject<MyDeviceBase>) || (HkReferenceObject) other.Physics.RigidBody != (HkReferenceObject) null && other.Physics.RigidBody.Layer == 20)
        return;
      float num2 = !(other is MyCubeGrid localGrid) ? MyDestructionHelper.MassFromHavok(other.Physics.Mass) : MyGridPhysicalGroupData.GetGroupSharedProperties(localGrid, false).Mass;
      double impactEnergy = (double) num1 * (double) num1 * (double) num2;
      if (impactEnergy > this.ItemResilience(itemId))
      {
        int num3 = Interlocked.Increment(ref this.m_scheduledBreaksCount);
        Vector3D position = e.Position;
        Vector3 normal = e.ContactPointEvent.ContactPoint.Normal;
        if (this.m_scheduledBreaks.TryAdd(itemId, new MyBreakableEnvironmentProxy.BreakAtData(itemId, position, (Vector3D) normal, impactEnergy)) && num3 == 1)
          MySandboxGame.Static.Invoke(this.m_BreakAtDelegate, "MyBreakableEnvironmentProxy::BreakAt");
      }
      if (!(other is MyMeteor))
        return;
      this.m_sector.EnableItem(itemId, false);
    }

    private void BreakAtInvoke()
    {
      foreach (MyBreakableEnvironmentProxy.BreakAtData breakAtData in (IEnumerable<MyBreakableEnvironmentProxy.BreakAtData>) this.m_scheduledBreaks.Values)
        this.BreakAt(breakAtData.itemId, breakAtData.hitpos, breakAtData.hitnormal, breakAtData.impactEnergy);
      this.m_scheduledBreaks.Clear();
      this.m_scheduledBreaksCount = 0;
    }

    public void BreakAt(int itemId, Vector3D hitpos, Vector3D hitnormal, double impactEnergy)
    {
      impactEnergy = MathHelper.Clamp(impactEnergy, 0.0, this.ItemResilience(itemId) * 10.0);
      MyBreakableEnvironmentProxy.Impact imp = new MyBreakableEnvironmentProxy.Impact(hitpos, hitnormal, impactEnergy);
      this.m_sector.RaiseItemEvent<MyBreakableEnvironmentProxy, MyBreakableEnvironmentProxy.Impact>(this, itemId, imp);
      this.DisableItemAndCreateDebris(ref imp, itemId);
    }

    private void DisableItemAndCreateDebris(ref MyBreakableEnvironmentProxy.Impact imp, int itemId)
    {
      if (this.m_sector.GetModelIndex(itemId) < (short) 0)
        return;
      MyParticlesManager.TryCreateParticleEffect("Tree Destruction", MatrixD.CreateTranslation(imp.Position), out MyParticleEffect _);
      if (this.m_sector.LodLevel <= 1)
      {
        MyEntity debris = this.CreateDebris(itemId);
        if (debris != null)
        {
          float mass = debris.Physics.Mass;
          Vector3D vector3D1 = Math.Sqrt(imp.Energy / (double) mass) / (0.0166666675359011 * (double) MyFakes.SIMULATION_SPEED) * 0.800000011920929 * imp.Normal;
          Vector3D vector3D2 = debris.Physics.CenterOfMassWorld + debris.WorldMatrix.Up * (imp.Position - debris.Physics.CenterOfMassWorld).Length();
          debris.Physics.AddForce(MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE, new Vector3?((Vector3) vector3D1), new Vector3D?(vector3D2), new Vector3?());
        }
      }
      this.m_sector.EnableItem(itemId, false);
    }

    private MyEntity CreateDebris(int itemId)
    {
      Sandbox.Game.WorldEnvironment.ItemInfo itemInfo = this.m_sector.DataView.Items[itemId];
      MyRuntimeEnvironmentItemInfo def;
      this.m_sector.Owner.GetDefinition((ushort) itemInfo.DefinitionIndex, out def);
      if (def != null && def.Type.Name != "Tree")
        return (MyEntity) null;
      Vector3D vector3D = itemInfo.Position + this.m_sector.SectorCenter;
      MyPhysicalModelDefinition modelForId = this.m_sector.Owner.GetModelForId(itemInfo.ModelIndex);
      string modelAsset = modelForId.Model.Insert(modelForId.Model.Length - 4, "_broken");
      bool flag = false;
      string model = modelForId.Model;
      if (MyModels.GetModelOnlyData(modelAsset) != null)
      {
        flag = true;
        model = modelAsset;
      }
      MyEntity treeDebris = MyDebris.Static.CreateTreeDebris(model);
      MyDebrisBase.MyDebrisBaseLogic gameLogic = (MyDebrisBase.MyDebrisBaseLogic) treeDebris.GameLogic;
      gameLogic.LifespanInMiliseconds = 20000;
      MatrixD fromQuaternion = MatrixD.CreateFromQuaternion(itemInfo.Rotation);
      fromQuaternion.Translation = vector3D + fromQuaternion.Up * (flag ? 0.0 : 5.0);
      gameLogic.Start(fromQuaternion, (Vector3D) Vector3.Zero, false);
      return treeDebris;
    }

    private double ItemResilience(int itemId) => 200000.0;

    public void Close() => this.m_sector.OnContactPoint -= new MySectorContactEvent(this.SectorOnContactPoint);

    public void CommitLodChange(int lodBefore, int lodAfter)
    {
    }

    public void CommitPhysicsChange(bool enabled)
    {
    }

    public void OnItemChange(int item, short newModel)
    {
    }

    public void OnItemChangeBatch(List<int> items, int offset, short newModel)
    {
    }

    public void HandleSyncEvent(int item, object data, bool fromClient)
    {
      MyBreakableEnvironmentProxy.Impact imp = (MyBreakableEnvironmentProxy.Impact) data;
      this.DisableItemAndCreateDebris(ref imp, item);
    }

    public void DebugDraw()
    {
    }

    public long SectorId => this.m_sector.SectorId;

    private struct BreakAtData
    {
      public readonly int itemId;
      public readonly Vector3D hitpos;
      public readonly Vector3D hitnormal;
      public readonly double impactEnergy;

      public BreakAtData(int itemId, Vector3D hitpos, Vector3D hitnormal, double impactEnergy)
      {
        this.itemId = itemId;
        this.hitpos = hitpos;
        this.hitnormal = hitnormal;
        this.impactEnergy = impactEnergy;
      }
    }

    [Serializable]
    private struct Impact
    {
      public Vector3D Position;
      public Vector3D Normal;
      public double Energy;

      public Impact(Vector3D position, Vector3D normal, double energy)
      {
        this.Position = position;
        this.Normal = normal;
        this.Energy = energy;
      }

      protected class Sandbox_Game_WorldEnvironment_Modules_MyBreakableEnvironmentProxy\u003C\u003EImpact\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyBreakableEnvironmentProxy.Impact, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBreakableEnvironmentProxy.Impact owner, in Vector3D value) => owner.Position = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBreakableEnvironmentProxy.Impact owner, out Vector3D value) => value = owner.Position;
      }

      protected class Sandbox_Game_WorldEnvironment_Modules_MyBreakableEnvironmentProxy\u003C\u003EImpact\u003C\u003ENormal\u003C\u003EAccessor : IMemberAccessor<MyBreakableEnvironmentProxy.Impact, Vector3D>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBreakableEnvironmentProxy.Impact owner, in Vector3D value) => owner.Normal = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBreakableEnvironmentProxy.Impact owner, out Vector3D value) => value = owner.Normal;
      }

      protected class Sandbox_Game_WorldEnvironment_Modules_MyBreakableEnvironmentProxy\u003C\u003EImpact\u003C\u003EEnergy\u003C\u003EAccessor : IMemberAccessor<MyBreakableEnvironmentProxy.Impact, double>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyBreakableEnvironmentProxy.Impact owner, in double value) => owner.Energy = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyBreakableEnvironmentProxy.Impact owner, out double value) => value = owner.Energy;
      }
    }
  }
}
