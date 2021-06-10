// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyWarhead
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Warhead))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyWarhead), typeof (Sandbox.ModAPI.Ingame.IMyWarhead)})]
  public class MyWarhead : MyTerminalBlock, IMyDestroyableObject, Sandbox.ModAPI.IMyWarhead, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyWarhead
  {
    private const int MAX_COUNTDOWN = 3600;
    private const int MIN_COUNTDOWN = 1;
    private const float m_maxExplosionRadius = 30f;
    public static float ExplosionImpulse = 30000f;
    private bool m_isExploded;
    private MyStringHash m_damageType = MyDamageType.Deformation;
    public int RemainingMS;
    private BoundingSphereD m_explosionShrinkenSphere;
    private BoundingSphereD m_explosionFullSphere;
    private BoundingSphereD m_explosionParticleSphere;
    private bool m_marked;
    private int m_warheadsInsideCount;
    private readonly List<MyEntity> m_entitiesInShrinkenSphere = new List<MyEntity>();
    private bool m_countdownEmissivityColor;
    private readonly VRage.Sync.Sync<int, SyncDirection.BothWays> m_countdownMs;
    public static Action<MyWarhead> OnCreated;
    public static Action<MyWarhead> OnDeleted;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_isArmed;
    private MyWarheadDefinition m_warheadDefinition;

    public bool IsCountingDown { get; private set; }

    private int BlinkDelay
    {
      get
      {
        if ((int) this.m_countdownMs < 10000)
          return 100;
        if ((int) this.m_countdownMs < 30000)
          return 250;
        return (int) this.m_countdownMs < 60000 ? 500 : 1000;
      }
    }

    public bool IsArmed
    {
      get => (bool) this.m_isArmed;
      set => this.m_isArmed.Value = value;
    }

    public MyWarhead()
    {
      this.CreateTerminalControls();
      this.m_isArmed.ValueChanged += (Action<SyncBase>) (x => this.SetEmissiveStateWorking());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyWarhead>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSlider<MyWarhead> slider = new MyTerminalControlSlider<MyWarhead>("DetonationTime", MySpaceTexts.TerminalControlPanel_Warhead_DetonationTime, MySpaceTexts.TerminalControlPanel_Warhead_DetonationTime);
      slider.SetLogLimits(1f, 3600f);
      slider.DefaultValue = new float?(10f);
      slider.Enabled = (Func<MyWarhead, bool>) (x => !x.IsCountingDown);
      slider.Getter = (MyTerminalValueControl<MyWarhead, float>.GetterDelegate) (x => x.DetonationTime);
      slider.Setter = (MyTerminalValueControl<MyWarhead, float>.SetterDelegate) ((x, v) => x.m_countdownMs.Value = (int) ((double) v * 1000.0));
      slider.Writer = (MyTerminalControl<MyWarhead>.WriterDelegate) ((x, sb) => MyValueFormatter.AppendTimeExact(Math.Max((int) x.m_countdownMs, 1000) / 1000, sb));
      slider.SetMinStep(1f);
      slider.EnableActions<MyWarhead>();
      MyTerminalControlFactory.AddControl<MyWarhead>((MyTerminalControl<MyWarhead>) slider);
      MyTerminalControlButton<MyWarhead> button1 = new MyTerminalControlButton<MyWarhead>("StartCountdown", MySpaceTexts.TerminalControlPanel_Warhead_StartCountdown, MySpaceTexts.TerminalControlPanel_Warhead_StartCountdown, (Action<MyWarhead>) (b => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyWarhead, bool>(b, (Func<MyWarhead, Action<bool>>) (x => new Action<bool>(x.SetCountdown)), true)), true);
      button1.EnableAction<MyWarhead>();
      MyTerminalControlFactory.AddControl<MyWarhead>((MyTerminalControl<MyWarhead>) button1);
      MyTerminalControlButton<MyWarhead> button2 = new MyTerminalControlButton<MyWarhead>("StopCountdown", MySpaceTexts.TerminalControlPanel_Warhead_StopCountdown, MySpaceTexts.TerminalControlPanel_Warhead_StopCountdown, (Action<MyWarhead>) (b => Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyWarhead, bool>(b, (Func<MyWarhead, Action<bool>>) (x => new Action<bool>(x.SetCountdown)), false)));
      button2.EnableAction<MyWarhead>();
      MyTerminalControlFactory.AddControl<MyWarhead>((MyTerminalControl<MyWarhead>) button2);
      MyTerminalControlFactory.AddControl<MyWarhead>((MyTerminalControl<MyWarhead>) new MyTerminalControlSeparator<MyWarhead>());
      MyTerminalControlCheckbox<MyWarhead> checkbox = new MyTerminalControlCheckbox<MyWarhead>("Safety", MySpaceTexts.TerminalControlPanel_Warhead_Safety, MySpaceTexts.TerminalControlPanel_Warhead_SafetyTooltip, new MyStringId?(MySpaceTexts.TerminalControlPanel_Warhead_SwitchTextArmed), new MyStringId?(MySpaceTexts.TerminalControlPanel_Warhead_SwitchTextDisarmed));
      checkbox.Getter = (MyTerminalValueControl<MyWarhead, bool>.GetterDelegate) (x => x.IsArmed);
      checkbox.Setter = (MyTerminalValueControl<MyWarhead, bool>.SetterDelegate) ((x, v) => x.IsArmed = v);
      checkbox.EnableAction<MyWarhead>();
      MyTerminalControlFactory.AddControl<MyWarhead>((MyTerminalControl<MyWarhead>) checkbox);
      MyTerminalControlButton<MyWarhead> button3 = new MyTerminalControlButton<MyWarhead>("Detonate", MySpaceTexts.TerminalControlPanel_Warhead_Detonate, MySpaceTexts.TerminalControlPanel_Warhead_Detonate, (Action<MyWarhead>) (b =>
      {
        if (!b.IsArmed)
          return;
        Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyWarhead>(b, (Func<MyWarhead, Action>) (x => new Action(x.DetonateRequest)));
      }));
      button3.Enabled = (Func<MyWarhead, bool>) (x => x.IsArmed);
      button3.EnableAction<MyWarhead>();
      MyTerminalControlFactory.AddControl<MyWarhead>((MyTerminalControl<MyWarhead>) button3);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.m_warheadDefinition = (MyWarheadDefinition) this.BlockDefinition;
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_Warhead objectBuilderWarhead = (MyObjectBuilder_Warhead) objectBuilder;
      this.m_countdownMs.SetLocalValue(MathHelper.Clamp(objectBuilderWarhead.CountdownMs, 1000, 3600000));
      this.m_isArmed.SetLocalValue(objectBuilderWarhead.IsArmed);
      this.IsCountingDown = objectBuilderWarhead.IsCountingDown;
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MyWarhead_IsWorkingChanged);
      this.UseDamageSystem = true;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Warhead builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Warhead;
      builderCubeBlock.CountdownMs = (int) this.m_countdownMs;
      builderCubeBlock.IsCountingDown = this.IsCountingDown;
      builderCubeBlock.IsArmed = this.IsArmed;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void MyWarhead_IsWorkingChanged(MyCubeBlock obj)
    {
      if (!this.IsCountingDown || this.IsWorking)
        return;
      this.StopCountdown();
    }

    public override bool SetEmissiveStateWorking()
    {
      if (!this.IsWorking)
        return false;
      return this.IsCountingDown ? (this.m_countdownEmissivityColor ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Warning, this.Render.RenderObjectIDs[0])) : (this.IsArmed ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Locked, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]));
    }

    public override void ContactPointCallback(ref MyGridContactInfo value)
    {
      base.ContactPointCallback(ref value);
      if (value.CollidingEntity is MyDebrisBase || (double) Math.Abs(value.Event.SeparatingVelocity) <= 5.0 || (!this.IsFunctional || !this.IsArmed) || !this.CubeGrid.BlocksDestructionEnabled)
        return;
      this.Explode();
    }

    public bool StartCountdown()
    {
      if (!this.IsFunctional || this.IsCountingDown)
        return false;
      this.IsCountingDown = true;
      MyWarheads.AddWarhead(this);
      this.RaisePropertiesChanged();
      this.SetEmissiveStateWorking();
      return true;
    }

    public bool StopCountdown()
    {
      if (!this.IsFunctional || !this.IsCountingDown)
        return false;
      this.IsCountingDown = false;
      MyWarheads.RemoveWarhead(this);
      this.RaisePropertiesChanged();
      this.SetEmissiveStateWorking();
      return true;
    }

    public bool Countdown(int frameMs)
    {
      if (!this.IsFunctional)
        return false;
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_countdownMs.Value -= frameMs;
      if ((int) this.m_countdownMs % this.BlinkDelay < frameMs)
      {
        this.m_countdownEmissivityColor = !this.m_countdownEmissivityColor;
        this.SetEmissiveStateWorking();
      }
      this.RaisePropertiesChanged();
      return (int) this.m_countdownMs <= 0;
    }

    public void Detonate()
    {
      if (!this.IsFunctional)
        return;
      this.Explode();
    }

    public void Explode()
    {
      if (this.m_isExploded || !MySession.Static.WeaponsEnabled || this.CubeGrid.Physics == null)
        return;
      this.m_isExploded = true;
      if (!this.m_marked)
        this.MarkForExplosion();
      MyExplosionTypeEnum explosionTypeEnum = this.m_explosionFullSphere.Radius > 6.0 ? (this.m_explosionFullSphere.Radius > 20.0 ? (this.m_explosionFullSphere.Radius > 40.0 ? MyExplosionTypeEnum.WARHEAD_EXPLOSION_50 : MyExplosionTypeEnum.WARHEAD_EXPLOSION_30) : MyExplosionTypeEnum.WARHEAD_EXPLOSION_15) : MyExplosionTypeEnum.WARHEAD_EXPLOSION_02;
      MyExplosionInfo explosionInfo = new MyExplosionInfo()
      {
        PlayerDamage = 0.0f,
        Damage = this.m_warheadDefinition.WarheadExplosionDamage,
        ExplosionType = explosionTypeEnum,
        ExplosionSphere = this.m_explosionFullSphere,
        LifespanMiliseconds = 700,
        HitEntity = (MyEntity) this,
        ParticleScale = 1f,
        OwnerEntity = (MyEntity) this.CubeGrid,
        Direction = new Vector3?((Vector3) this.WorldMatrix.Forward),
        VoxelExplosionCenter = this.m_explosionFullSphere.Center,
        ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.AFFECT_VOXELS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.CREATE_PARTICLE_EFFECT | MyExplosionFlags.CREATE_SHRAPNELS | MyExplosionFlags.APPLY_DEFORMATION,
        VoxelCutoutScale = 1f,
        PlaySound = true,
        ApplyForceAndDamage = true,
        ObjectsRemoveDelayInMiliseconds = 40
      };
      if (this.CubeGrid.Physics != null)
        explosionInfo.Velocity = this.CubeGrid.Physics.LinearVelocity;
      MyExplosions.AddExplosion(ref explosionInfo);
    }

    private void MarkForExplosion()
    {
      this.m_marked = true;
      float num1 = 4f;
      float num2 = this.CubeGrid.GridSize * num1;
      float num3 = 0.85f;
      this.m_explosionShrinkenSphere = new BoundingSphereD(this.PositionComp.GetPosition(), (double) num2 * (double) num3);
      this.m_explosionParticleSphere = BoundingSphereD.CreateInvalid();
      MyGamePruningStructure.GetAllEntitiesInSphere(ref this.m_explosionShrinkenSphere, this.m_entitiesInShrinkenSphere);
      this.m_warheadsInsideCount = 0;
      foreach (MyEntity myEntity in this.m_entitiesInShrinkenSphere)
      {
        if (!(myEntity is MyDebrisBase) && (!(myEntity is MyCubeBlock) || (myEntity as MyCubeBlock).CubeGrid.Projector == null) && (Vector3D.DistanceSquared(this.PositionComp.GetPosition(), myEntity.PositionComp.GetPosition()) < (double) num2 * (double) num3 * (double) num2 * (double) num3 && myEntity is MyWarhead myWarhead))
        {
          ++this.m_warheadsInsideCount;
          this.m_explosionParticleSphere = this.m_explosionParticleSphere.Include(new BoundingSphereD(myWarhead.PositionComp.GetPosition(), (double) this.CubeGrid.GridSize * (double) num1 + (double) myWarhead.CubeGrid.GridSize));
        }
      }
      this.m_entitiesInShrinkenSphere.Clear();
      this.m_explosionFullSphere = new BoundingSphereD(this.m_explosionParticleSphere.Center, Math.Max((double) Math.Min(30f, (float) (1.0 + 0.0240000002086163 * (double) this.m_warheadsInsideCount) * this.m_warheadDefinition.ExplosionRadius), this.m_explosionParticleSphere.Radius));
      if (!MyExplosion.DEBUG_EXPLOSIONS)
        return;
      MyWarheads.DebugWarheadShrinks.Add((BoundingSphere) this.m_explosionShrinkenSphere);
      MyWarheads.DebugWarheadGroupSpheres.Add((BoundingSphere) this.m_explosionFullSphere);
      BoundingSphereD explosionParticleSphere = this.m_explosionParticleSphere;
    }

    public override void OnDestroy()
    {
      if (!this.IsFunctional || !this.IsArmed)
        return;
      if (this.m_damageType == MyDamageType.Bullet)
      {
        this.Explode();
      }
      else
      {
        this.MarkForExplosion();
        this.ExplodeDelayed(500);
      }
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.IsCountingDown)
      {
        this.IsCountingDown = false;
        this.StartCountdown();
      }
      else
        this.SetEmissiveStateWorking();
      if (MyWarhead.OnCreated == null)
        return;
      MyWarhead.OnCreated(this);
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      if (this.IsCountingDown)
      {
        this.StopCountdown();
        this.IsCountingDown = true;
      }
      if (MyWarhead.OnDeleted == null)
        return;
      MyWarhead.OnDeleted(this);
    }

    private void ExplodeDelayed(int maxMiliseconds)
    {
      this.RemainingMS = MyUtils.GetRandomInt(maxMiliseconds);
      this.m_countdownMs.Value = 0;
      this.StartCountdown();
    }

    public bool UseDamageSystem { get; private set; }

    [Event(null, 463)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void DetonateRequest() => this.Detonate();

    [Event(null, 469)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void SetCountdown(bool countdownState)
    {
      if (!(!countdownState ? this.StopCountdown() : this.StartCountdown()))
        return;
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyWarhead, bool>(this, (Func<MyWarhead, Action<bool>>) (x => new Action<bool>(x.SetCountdownClient)), countdownState);
    }

    [Event(null, 484)]
    [Reliable]
    [Broadcast]
    private void SetCountdownClient(bool countdownState)
    {
      if (countdownState)
        this.StartCountdown();
      else
        this.StopCountdown();
    }

    void IMyDestroyableObject.OnDestroy() => this.OnDestroy();

    bool IMyDestroyableObject.DoDamage(
      float damage,
      MyStringHash damageType,
      bool sync,
      MyHitInfo? hitInfo,
      long attackerId,
      long realHitEntityId = 0)
    {
      if (!MySession.Static.DestructibleBlocks || !this.IsArmed || !MySessionComponentSafeZones.IsActionAllowed((MyEntity) this.CubeGrid, MySafeZoneAction.Damage))
        return false;
      if (sync)
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          MySyncDamage.DoDamageSynced((MyEntity) this, damage, damageType, attackerId);
      }
      else
      {
        MyDamageInformation info = new MyDamageInformation(false, damage, damageType, attackerId);
        if (this.UseDamageSystem)
          MyDamageSystem.Static.RaiseBeforeDamageApplied((object) this, ref info);
        this.m_damageType = damageType;
        if ((double) info.Amount > 0.0)
        {
          if (this.UseDamageSystem)
            MyDamageSystem.Static.RaiseAfterDamageApplied((object) this, info);
          if ((bool) this.m_isArmed)
            this.OnDestroy();
          if (this.UseDamageSystem)
            MyDamageSystem.Static.RaiseDestroyed((object) this, info);
        }
      }
      return true;
    }

    float IMyDestroyableObject.Integrity => 1f;

    bool IMyDestroyableObject.UseDamageSystem => this.UseDamageSystem;

    public float DetonationTime
    {
      get => (float) ((int) this.m_countdownMs / 1000);
      set => this.m_countdownMs.Value = (int) ((double) value * 1000.0);
    }

    bool Sandbox.ModAPI.Ingame.IMyWarhead.IsCountingDown => this.IsCountingDown;

    float Sandbox.ModAPI.Ingame.IMyWarhead.DetonationTime
    {
      get => this.DetonationTime;
      set => this.DetonationTime = value;
    }

    void Sandbox.ModAPI.Ingame.IMyWarhead.Detonate() => this.Detonate();

    protected sealed class DetonateRequest\u003C\u003E : ICallSite<MyWarhead, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyWarhead @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.DetonateRequest();
      }
    }

    protected sealed class SetCountdown\u003C\u003ESystem_Boolean : ICallSite<MyWarhead, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyWarhead @this,
        in bool countdownState,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetCountdown(countdownState);
      }
    }

    protected sealed class SetCountdownClient\u003C\u003ESystem_Boolean : ICallSite<MyWarhead, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyWarhead @this,
        in bool countdownState,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetCountdownClient(countdownState);
      }
    }

    protected class m_countdownMs\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.BothWays>(obj1, obj2));
        ((MyWarhead) obj0).m_countdownMs = (VRage.Sync.Sync<int, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_isArmed\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyWarhead) obj0).m_isArmed = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyWarhead\u003C\u003EActor : IActivator, IActivator<MyWarhead>
    {
      object IActivator.CreateInstance() => (object) new MyWarhead();

      MyWarhead IActivator<MyWarhead>.CreateInstance() => new MyWarhead();
    }
  }
}
