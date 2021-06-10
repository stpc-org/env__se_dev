// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyExplosions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.Lights;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Generics;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
  public class MyExplosions : MySessionComponentBase
  {
    private static MyObjectsPool<MyExplosion> m_explosions = (MyObjectsPool<MyExplosion>) null;
    private static List<MyExplosionInfo> m_explosionBuffer1 = new List<MyExplosionInfo>();
    private static List<MyExplosionInfo> m_explosionBuffer2 = new List<MyExplosionInfo>();
    private static List<MyExplosionInfo> m_explosionsRead = MyExplosions.m_explosionBuffer1;
    private static List<MyExplosionInfo> m_explosionsWrite = MyExplosions.m_explosionBuffer2;
    private static List<MyExplosion> m_exploded = new List<MyExplosion>();
    private static HashSet<long> m_activeEntityKickbacks = new HashSet<long>();
    private static SortedDictionary<long, long> m_activeEntityKickbacksByTime = new SortedDictionary<long, long>();

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyLights)
    };

    public override void LoadData()
    {
      MySandboxGame.Log.WriteLine("MyExplosions.LoadData() - START");
      MySandboxGame.Log.IncreaseIndent();
      if (MyExplosions.m_explosions == null)
        MyExplosions.m_explosions = new MyObjectsPool<MyExplosion>(1024, clearFunction: ((Action<MyExplosion>) (x => x.Clear())));
      MySandboxGame.Log.DecreaseIndent();
      MySandboxGame.Log.WriteLine("MyExplosions.LoadData() - END");
    }

    protected override void UnloadData()
    {
      if (MyExplosions.m_explosions != null && MyExplosions.m_explosions.ActiveCount > 0)
      {
        foreach (MyExplosion myExplosion in MyExplosions.m_explosions.Active)
          myExplosion?.Close();
        MyExplosions.m_explosions.DeallocateAll();
      }
      MyExplosions.m_explosionsRead.Clear();
      MyExplosions.m_explosionsWrite.Clear();
      MyExplosions.m_activeEntityKickbacks.Clear();
      MyExplosions.m_activeEntityKickbacksByTime.Clear();
    }

    public static void AddExplosion(ref MyExplosionInfo explosionInfo, bool updateSync = true)
    {
      if (!MySessionComponentSafeZones.IsActionAllowed(BoundingBoxD.CreateFromSphere(explosionInfo.ExplosionSphere), MySafeZoneAction.Damage))
        return;
      if (Sync.IsServer & updateSync)
        MyMultiplayer.RaiseStaticEvent<MyExplosionInfoSimplified>((Func<IMyEventOwner, Action<MyExplosionInfoSimplified>>) (s => new Action<MyExplosionInfoSimplified>(MyExplosions.ProxyExplosionRequest)), new MyExplosionInfoSimplified()
        {
          Damage = explosionInfo.Damage,
          Center = explosionInfo.ExplosionSphere.Center,
          Radius = (float) explosionInfo.ExplosionSphere.Radius,
          Type = explosionInfo.ExplosionType,
          Flags = explosionInfo.ExplosionFlags,
          VoxelCenter = explosionInfo.VoxelExplosionCenter,
          ParticleScale = explosionInfo.ParticleScale,
          Velocity = explosionInfo.Velocity,
          IgnoreFriendlyFireSetting = explosionInfo.IgnoreFriendlyFireSetting
        }, position: new Vector3D?(explosionInfo.ExplosionSphere.Center));
      MyExplosions.m_explosionsWrite.Add(explosionInfo);
    }

    public override void UpdateBeforeSimulation()
    {
      this.SwapBuffers();
      this.UpdateEntityKickbacks();
      foreach (MyExplosionInfo explosionInfo in MyExplosions.m_explosionsRead)
      {
        MyExplosion myExplosion = (MyExplosion) null;
        MyExplosions.m_explosions.AllocateOrCreate(out myExplosion);
        myExplosion?.Start(explosionInfo);
      }
      MyExplosions.m_explosionsRead.Clear();
      foreach (MyExplosion myExplosion in MyExplosions.m_explosions.Active)
      {
        if (!myExplosion.Update())
        {
          MyExplosions.m_exploded.Add(myExplosion);
          MyExplosions.m_explosions.MarkForDeallocate(myExplosion);
        }
        else
          MyExplosions.m_exploded.Add(myExplosion);
      }
      foreach (MyExplosion myExplosion in MyExplosions.m_exploded)
        myExplosion.ApplyVolumetricDamageToGrid();
      MyExplosions.m_exploded.Clear();
      MyExplosions.m_explosions.DeallocateAllMarked();
      MyDebris.Static.UpdateBeforeSimulation();
    }

    [Event(null, 170)]
    [Reliable]
    [ServerInvoked]
    [BroadcastExcept]
    private static void ProxyExplosionRequest(MyExplosionInfoSimplified explosionInfo)
    {
      if (!MySession.Static.Ready || MyEventContext.Current.IsLocallyInvoked)
        return;
      MyExplosionInfo explosionInfo1 = new MyExplosionInfo()
      {
        PlayerDamage = 0.0f,
        Damage = explosionInfo.Damage,
        ExplosionType = explosionInfo.Type,
        ExplosionSphere = new BoundingSphereD(explosionInfo.Center, (double) explosionInfo.Radius),
        LifespanMiliseconds = 700,
        HitEntity = (MyEntity) null,
        ParticleScale = explosionInfo.ParticleScale,
        OwnerEntity = (MyEntity) null,
        Direction = new Vector3?(Vector3.Forward),
        VoxelExplosionCenter = explosionInfo.VoxelCenter,
        ExplosionFlags = explosionInfo.Flags,
        VoxelCutoutScale = 1f,
        PlaySound = true,
        ObjectsRemoveDelayInMiliseconds = 40,
        Velocity = explosionInfo.Velocity,
        IgnoreFriendlyFireSetting = explosionInfo.IgnoreFriendlyFireSetting
      };
      MyExplosions.AddExplosion(ref explosionInfo1, false);
    }

    private void SwapBuffers()
    {
      if (MyExplosions.m_explosionBuffer1 == MyExplosions.m_explosionsRead)
      {
        MyExplosions.m_explosionsWrite = MyExplosions.m_explosionBuffer1;
        MyExplosions.m_explosionsRead = MyExplosions.m_explosionBuffer2;
      }
      else
      {
        MyExplosions.m_explosionsWrite = MyExplosions.m_explosionBuffer2;
        MyExplosions.m_explosionsRead = MyExplosions.m_explosionBuffer1;
      }
    }

    public override void Draw()
    {
      foreach (MyExplosion myExplosion in MyExplosions.m_explosions.Active)
        myExplosion.DebugDraw();
    }

    public static bool ShouldUseMassScaleForEntity(MyEntity entity)
    {
      long entityId = entity.EntityId;
      if (MyExplosions.m_activeEntityKickbacks.Contains(entityId))
        return false;
      long key = (MySession.Static.ElapsedGameTime + TimeSpan.FromSeconds(2.0)).Ticks + entityId % 100L;
      while (MyExplosions.m_activeEntityKickbacksByTime.ContainsKey(key))
        ++key;
      MyExplosions.m_activeEntityKickbacks.Add(entityId);
      MyExplosions.m_activeEntityKickbacksByTime.Add(key, entityId);
      return true;
    }

    private void UpdateEntityKickbacks()
    {
      long ticks = MySession.Static.ElapsedGameTime.Ticks;
      while (MyExplosions.m_activeEntityKickbacksByTime.Count != 0)
      {
        using (SortedDictionary<long, long>.Enumerator enumerator = MyExplosions.m_activeEntityKickbacksByTime.GetEnumerator())
        {
          if (enumerator.MoveNext())
          {
            KeyValuePair<long, long> current = enumerator.Current;
            if (current.Key > ticks)
              break;
            long num = current.Value;
            MyExplosions.m_activeEntityKickbacks.Remove(num);
            MyExplosions.m_activeEntityKickbacksByTime.Remove(current.Key);
          }
        }
      }
    }

    protected sealed class ProxyExplosionRequest\u003C\u003ESandbox_Game_MyExplosionInfoSimplified : ICallSite<IMyEventOwner, MyExplosionInfoSimplified, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyExplosionInfoSimplified explosionInfo,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyExplosions.ProxyExplosionRequest(explosionInfo);
      }
    }
  }
}
