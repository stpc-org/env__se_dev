// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Weapons.MyMissiles
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Weapons
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyMissiles : MySessionComponentBase
  {
    private static readonly Dictionary<MyMissiles.MissileId, Queue<MyMissile>> m_missiles = new Dictionary<MyMissiles.MissileId, Queue<MyMissile>>();
    private static readonly MyDynamicAABBTreeD m_missileTree = new MyDynamicAABBTreeD(Vector3D.One * 10.0, 10.0);
    private static Queue<MyObjectBuilder_Missile> m_newMissiles = new Queue<MyObjectBuilder_Missile>();

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyPhysics)
    };

    public override void LoadData()
    {
    }

    protected override void UnloadData()
    {
      foreach (KeyValuePair<MyMissiles.MissileId, Queue<MyMissile>> missile in MyMissiles.m_missiles)
      {
        while (missile.Value.Count > 0)
          missile.Value.Dequeue().Close();
      }
      MyMissiles.m_missiles.Clear();
      MyMissiles.m_missileTree.Clear();
    }

    public static void Add(MyObjectBuilder_Missile builder)
    {
      if (MyEntities.IsAsyncUpdateInProgress)
      {
        lock (MyMissiles.m_newMissiles)
        {
          if (MyMissiles.m_newMissiles.Count == 0)
            MyEntities.InvokeLater(new Action(MyMissiles.AddMissiles));
          MyMissiles.m_newMissiles.Enqueue(builder);
        }
      }
      else
        MyMissiles.AddMissileInternal(builder);
    }

    private static void AddMissileInternal(MyObjectBuilder_Missile builder)
    {
      MyMissiles.MissileId key = new MyMissiles.MissileId()
      {
        AmmoMagazineId = builder.AmmoMagazineId,
        WeaponDefinitionId = builder.WeaponDefinitionId
      };
      Queue<MyMissile> myMissileQueue;
      if (MyMissiles.m_missiles.TryGetValue(key, out myMissileQueue) && myMissileQueue.Count > 0)
      {
        MyMissile missile = myMissileQueue.Dequeue();
        missile.UpdateData((MyObjectBuilder_EntityBase) builder);
        missile.m_pruningProxyId = -1;
        MyEntities.Add((MyEntity) missile);
        MyMissiles.RegisterMissile(missile);
      }
      else
        MyEntities.CreateFromObjectBuilderParallel((MyObjectBuilder_EntityBase) builder, true, (Action<MyEntity>) (x =>
        {
          MyMissile missile = x as MyMissile;
          missile.m_pruningProxyId = -1;
          MyMissiles.RegisterMissile(missile);
        }));
    }

    private static void AddMissiles()
    {
      lock (MyMissiles.m_newMissiles)
      {
        MyObjectBuilder_Missile result;
        while (MyMissiles.m_newMissiles.TryDequeue<MyObjectBuilder_Missile>(out result))
          MyMissiles.AddMissileInternal(result);
      }
    }

    public static void Remove(long entityId)
    {
      if (!(MyEntities.GetEntityById(entityId) is MyMissile entityById))
        return;
      MyMissiles.Return(entityById);
    }

    public static void Return(MyMissile missile)
    {
      if (!missile.InScene)
        return;
      MyMissiles.MissileId key = new MyMissiles.MissileId()
      {
        AmmoMagazineId = missile.AmmoMagazineId,
        WeaponDefinitionId = missile.WeaponDefinitionId
      };
      Queue<MyMissile> myMissileQueue;
      if (!MyMissiles.m_missiles.TryGetValue(key, out myMissileQueue))
      {
        myMissileQueue = new Queue<MyMissile>();
        MyMissiles.m_missiles.Add(key, myMissileQueue);
      }
      myMissileQueue.Enqueue(missile);
      MyEntities.Remove((MyEntity) missile);
      MyMissiles.UnregisterMissile(missile);
    }

    private static void RegisterMissile(MyMissile missile)
    {
      if (missile.m_pruningProxyId != -1)
        return;
      BoundingSphereD sphere = new BoundingSphereD(missile.PositionComp.GetPosition(), 1.0);
      BoundingBoxD result;
      BoundingBoxD.CreateFromSphere(ref sphere, out result);
      missile.m_pruningProxyId = MyMissiles.m_missileTree.AddProxy(ref result, (object) missile, 0U);
    }

    private static void UnregisterMissile(MyMissile missile)
    {
      if (missile.m_pruningProxyId == -1)
        return;
      MyMissiles.m_missileTree.RemoveProxy(missile.m_pruningProxyId);
      missile.m_pruningProxyId = -1;
    }

    public static void OnMissileMoved(MyMissile missile, ref Vector3 velocity)
    {
      if (missile.m_pruningProxyId == -1)
        return;
      BoundingSphereD sphere = new BoundingSphereD(missile.PositionComp.GetPosition(), 1.0);
      BoundingBoxD result;
      BoundingBoxD.CreateFromSphere(ref sphere, out result);
      MyMissiles.m_missileTree.MoveProxy(missile.m_pruningProxyId, ref result, (Vector3D) velocity);
    }

    public static void GetAllMissilesInSphere(ref BoundingSphereD sphere, List<MyEntity> result) => MyMissiles.m_missileTree.OverlapAllBoundingSphere<MyEntity>(ref sphere, result, false);

    private struct MissileId
    {
      public SerializableDefinitionId WeaponDefinitionId;
      public SerializableDefinitionId AmmoMagazineId;
    }
  }
}
