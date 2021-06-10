// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.SafeZone.MySafeZoneComponent
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Components.Beacon;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks.SafeZone
{
  [MyComponentBuilder(typeof (MyObjectBuilder_SafeZoneComponent), true)]
  public class MySafeZoneComponent : MyEntityComponentBase, IMyEventProxy, IMyEventOwner
  {
    private static MyDefinitionId DEFINITION_ZONECHIP = new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_Component), "ZoneChip");
    private MySafeZoneBlock m_parentBlock;
    private VRage.Sync.Sync<long, SyncDirection.FromServer> m_safeZoneEntityId;
    private long m_safeZoneActivationTimeMS;
    private bool m_activating;
    private MyObjectBuilder_SafeZone m_obSafezoneWhenDisabled;
    private TimeSpan m_upkeepTime;
    private TimeSpan m_timeLeft;
    private bool m_processingActivation;

    internal event Action SafeZoneChanged;

    public long SafeZoneEntityId
    {
      get => (long) this.m_safeZoneEntityId;
      private set => this.m_safeZoneEntityId.Value = value;
    }

    public bool WaitingResponse { get; private set; }

    public override string ComponentTypeDebugString => "MyBeaconSafeZoneManager";

    internal void Init(MySafeZoneBlock parentBlock, long safeZoneId)
    {
      this.m_parentBlock = parentBlock;
      this.m_parentBlock.SyncType.Append((object) this);
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.m_safeZoneEntityId.Value = safeZoneId;
      this.m_safeZoneEntityId.ValueChanged += new Action<SyncBase>(this.OnSafeZoneIdChanged);
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_SafeZoneComponent safeZoneComponent = base.Serialize(copy) as MyObjectBuilder_SafeZoneComponent;
      safeZoneComponent.UpkeepTime = this.m_upkeepTime.TotalMilliseconds - (double) MySandboxGame.TotalGamePlayTimeInMilliseconds;
      safeZoneComponent.Activating = this.m_activating;
      safeZoneComponent.ActivationTime = this.m_safeZoneActivationTimeMS - (long) MySandboxGame.TotalGamePlayTimeInMilliseconds;
      if (this.m_obSafezoneWhenDisabled != null)
      {
        MyObjectBuilder_SafeZone objectBuilderSafeZone = this.m_obSafezoneWhenDisabled.Clone() as MyObjectBuilder_SafeZone;
        objectBuilderSafeZone.Factions = objectBuilderSafeZone.Factions == null ? Array.Empty<long>() : objectBuilderSafeZone.Factions;
        objectBuilderSafeZone.Players = objectBuilderSafeZone.Players == null ? Array.Empty<long>() : objectBuilderSafeZone.Players;
        objectBuilderSafeZone.Entities = objectBuilderSafeZone.Entities == null ? Array.Empty<long>() : objectBuilderSafeZone.Entities;
        safeZoneComponent.SafeZoneOb = (MyObjectBuilder_EntityBase) objectBuilderSafeZone;
      }
      return (MyObjectBuilder_ComponentBase) safeZoneComponent;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase builder)
    {
      MyObjectBuilder_SafeZoneComponent safeZoneComponent = builder as MyObjectBuilder_SafeZoneComponent;
      this.m_upkeepTime = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds + safeZoneComponent.UpkeepTime);
      this.m_activating = safeZoneComponent.Activating;
      this.m_safeZoneActivationTimeMS = (long) MySandboxGame.TotalGamePlayTimeInMilliseconds + safeZoneComponent.ActivationTime;
      this.m_timeLeft = this.m_upkeepTime - TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      this.m_obSafezoneWhenDisabled = safeZoneComponent.SafeZoneOb as MyObjectBuilder_SafeZone;
      if (this.m_obSafezoneWhenDisabled != null)
      {
        this.m_obSafezoneWhenDisabled.Factions = this.m_obSafezoneWhenDisabled.Factions == null ? Array.Empty<long>() : this.m_obSafezoneWhenDisabled.Factions;
        this.m_obSafezoneWhenDisabled.Players = this.m_obSafezoneWhenDisabled.Players == null ? Array.Empty<long>() : this.m_obSafezoneWhenDisabled.Players;
        this.m_obSafezoneWhenDisabled.Entities = this.m_obSafezoneWhenDisabled.Entities == null ? Array.Empty<long>() : this.m_obSafezoneWhenDisabled.Entities;
        this.m_obSafezoneWhenDisabled.PositionAndOrientation = new MyPositionAndOrientation?();
      }
      if (this.m_activating || this.m_timeLeft > TimeSpan.Zero)
        this.m_parentBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      base.Deserialize(builder);
    }

    public override bool IsSerialized() => true;

    private void OnSafeZoneIdChanged(SyncBase obj)
    {
      this.WaitingResponse = false;
      Action safeZoneChanged = this.SafeZoneChanged;
      if (safeZoneChanged == null)
        return;
      safeZoneChanged();
    }

    internal void OnSafezoneCreateRemove_Request(bool turnOnSafeZone)
    {
      this.WaitingResponse = true;
      MyMultiplayer.RaiseEvent<MySafeZoneComponent, bool>(this, (Func<MySafeZoneComponent, Action<bool>>) (x => new Action<bool>(x.OnSafezoneCreateRemove)), turnOnSafeZone);
    }

    [Event(null, 148)]
    [Reliable]
    [Server]
    private void OnSafezoneCreateRemove(bool turnOnSafeZone)
    {
      if (turnOnSafeZone && this.SafeZoneEntityId == 0L)
      {
        float radius = this.GetRadius();
        if (this.m_obSafezoneWhenDisabled != null)
          radius = this.m_obSafezoneWhenDisabled.Radius;
        long implementationPlayer = MySessionComponentSafeZones.CreateSafeZone_ImplementationPlayer(this.m_parentBlock.EntityId, radius, false, MyEventContext.Current.Sender.Value);
        this.OnSafezoneCreated(implementationPlayer);
        if (this.m_obSafezoneWhenDisabled != null)
        {
          this.m_obSafezoneWhenDisabled.EntityId = implementationPlayer;
          this.m_obSafezoneWhenDisabled.SafeZoneBlockId = this.m_parentBlock.EntityId;
          MySessionComponentSafeZones.UpdateSafeZone(this.m_obSafezoneWhenDisabled, true);
        }
      }
      else
      {
        if (this.SafeZoneEntityId != 0L)
        {
          this.SaveSafeZoneSettings();
          MySessionComponentSafeZones.DeleteSafeZone_ImplementationPlayer(this.m_parentBlock.EntityId, this.SafeZoneEntityId, MyEventContext.Current.Sender.Value);
          this.m_timeLeft = this.m_upkeepTime - TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
          this.m_activating = false;
          this.m_parentBlock.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
        }
        this.SafeZoneEntityId = 0L;
      }
      if (MyEventContext.Current.IsLocallyInvoked)
        this.WaitingResponse = false;
      Action safeZoneChanged = this.SafeZoneChanged;
      if (safeZoneChanged == null)
        return;
      safeZoneChanged();
    }

    internal void OnSafezoneCreated(long safeZoneEntId)
    {
      if (safeZoneEntId == 0L)
      {
        this.SafeZoneEntityId = 0L;
        this.WaitingResponse = false;
        Action safeZoneChanged = this.SafeZoneChanged;
        if (safeZoneChanged == null)
          return;
        safeZoneChanged();
      }
      else
      {
        this.SafeZoneEntityId = safeZoneEntId;
        this.WaitingResponse = false;
        this.m_processingActivation = true;
        MySafeZone entity;
        MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity);
        if (!MySessionComponentSafeZones.IsSafeZoneColliding(safeZoneEntId, entity.WorldMatrix, entity.Shape, entity.Radius, entity.Size) && this.m_parentBlock.IsWorking && (this.m_timeLeft > TimeSpan.Zero || this.TryConsumeUpkeep()))
        {
          this.m_processingActivation = false;
          this.StartActivationCountdown();
        }
        this.m_processingActivation = false;
        Action safeZoneChanged = this.SafeZoneChanged;
        if (safeZoneChanged == null)
          return;
        safeZoneChanged();
      }
    }

    private void StartActivationCountdown()
    {
      this.m_safeZoneActivationTimeMS = (long) MySandboxGame.TotalGamePlayTimeInMilliseconds + (long) (this.m_parentBlock.Definition.SafeZoneActivationTimeS * 1000U);
      this.m_activating = true;
      this.m_parentBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MySafeZoneComponent>(this, (Func<MySafeZoneComponent, Action>) (x => new Action(x.StartActivationCoundown_Client)));
    }

    [Event(null, 236)]
    [Reliable]
    [Broadcast]
    private void StartActivationCoundown_Client() => this.StartActivationCountdown();

    internal void SafeZoneRemove_Server()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MySafeZone entity;
      if (MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
      {
        this.SaveSafeZoneSettings();
        entity.Close();
        this.m_timeLeft = this.m_upkeepTime - TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
        this.m_activating = false;
        this.m_parentBlock.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
      }
      this.SafeZoneEntityId = 0L;
    }

    internal void SafeZoneCreate_Server()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || this.SafeZoneEntityId != 0L || !(this.m_timeLeft > TimeSpan.Zero))
        return;
      float startRadius = this.GetRadius();
      if (this.m_obSafezoneWhenDisabled != null && this.m_obSafezoneWhenDisabled.Radius.IsValid())
        startRadius = MathHelper.Clamp(this.m_obSafezoneWhenDisabled.Radius, MySafeZone.MIN_RADIUS, MySafeZone.MAX_RADIUS);
      MyEntity myEntity = MySessionComponentSafeZones.CrateSafeZone(this.m_parentBlock.PositionComp.WorldMatrixRef, MySafeZoneShape.Sphere, MySafeZoneAccess.Whitelist, (long[]) null, (long[]) null, startRadius, false, color: Color.SkyBlue.ToVector3(), safeZoneBlockId: this.m_parentBlock.EntityId);
      this.OnSafezoneCreated(myEntity.EntityId);
      if (this.m_obSafezoneWhenDisabled == null)
        return;
      this.m_obSafezoneWhenDisabled.EntityId = myEntity.EntityId;
      this.m_obSafezoneWhenDisabled.SafeZoneBlockId = this.m_parentBlock.EntityId;
      MySessionComponentSafeZones.UpdateSafeZone(this.m_obSafezoneWhenDisabled, true);
    }

    private void SaveSafeZoneSettings()
    {
      MySafeZone entity;
      if (!MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
        return;
      this.m_obSafezoneWhenDisabled = entity.GetObjectBuilder(false) as MyObjectBuilder_SafeZone;
      this.m_obSafezoneWhenDisabled.ContainedEntities = Array.Empty<long>();
      this.m_obSafezoneWhenDisabled.Enabled = false;
      this.m_obSafezoneWhenDisabled.PositionAndOrientation = new MyPositionAndOrientation?();
    }

    internal float GetRadius()
    {
      MySafeZone entity;
      return this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity) ? entity.Radius : this.m_parentBlock.Definition.DefaultSafeZoneRadius;
    }

    public bool IsSafeZoneInWorld() => (long) this.m_safeZoneEntityId > 0L && MyEntities.GetEntityById((long) this.m_safeZoneEntityId) != null;

    public bool IsSafeZoneEnabled() => (long) this.m_safeZoneEntityId > 0L && MyEntities.GetEntityById((long) this.m_safeZoneEntityId) is MySafeZone entityById && entityById.Enabled;

    internal void SetRadius(float radius)
    {
      MySafeZone entity;
      if (this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
      {
        if (MySessionComponentSafeZones.IsSafeZoneColliding(this.SafeZoneEntityId, entity.WorldMatrix, MySafeZoneShape.Sphere, radius))
        {
          this.m_parentBlock.RaisePropertiesChanged();
          return;
        }
        MySessionComponentSafeZones.RequestUpdateSafeZoneRadius_Player(this.m_parentBlock.EntityId, this.SafeZoneEntityId, radius);
        if (!this.m_parentBlock.IsWorking)
          return;
        MyMultiplayer.RaiseEvent<MySafeZoneComponent>(this, (Func<MySafeZoneComponent, Action>) (x => new Action(x.OnRadiusChanged_Server)));
      }
      Action safeZoneChanged = this.SafeZoneChanged;
      if (safeZoneChanged == null)
        return;
      safeZoneChanged();
    }

    [Event(null, 360)]
    [Reliable]
    [Server]
    private void OnRadiusChanged_Server()
    {
      if (!this.m_parentBlock.IsWorking)
        return;
      Action safeZoneChanged = this.SafeZoneChanged;
      if (safeZoneChanged != null)
        safeZoneChanged();
      MySafeZone entity;
      if (!MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity) || MySessionComponentSafeZones.IsSafeZoneColliding(this.SafeZoneEntityId, entity.WorldMatrix, entity.Shape, entity.Radius, entity.Size) || (entity.Enabled || this.m_activating))
        return;
      this.SetActivate_Server(true);
    }

    internal void SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum sizeEnum, float newValue)
    {
      MySafeZone entity;
      if (this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
      {
        Vector3 newSize = Vector3.Zero;
        switch (sizeEnum)
        {
          case MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.X:
            newSize = new Vector3(newValue, entity.Size.Y, entity.Size.Z);
            break;
          case MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Y:
            newSize = new Vector3(entity.Size.X, newValue, entity.Size.Z);
            break;
          case MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Z:
            newSize = new Vector3(entity.Size.X, entity.Size.Y, newValue);
            break;
        }
        if (MySessionComponentSafeZones.IsSafeZoneColliding(this.SafeZoneEntityId, entity.WorldMatrix, MySafeZoneShape.Sphere, newSize: newSize))
        {
          this.m_parentBlock.RaisePropertiesChanged();
          return;
        }
        MyObjectBuilder_SafeZone objectBuilder = entity.GetObjectBuilder(false) as MyObjectBuilder_SafeZone;
        objectBuilder.Size = newSize;
        MySessionComponentSafeZones.RequestUpdateSafeZone_Player(this.m_parentBlock.EntityId, objectBuilder);
      }
      Action safeZoneChanged = this.SafeZoneChanged;
      if (safeZoneChanged == null)
        return;
      safeZoneChanged();
    }

    internal Vector3 GetSize()
    {
      MySafeZone entity;
      return this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity) ? entity.Size : new Vector3(this.m_parentBlock.Definition.DefaultSafeZoneRadius);
    }

    internal void SetColor(Color newColor)
    {
      MySafeZone entity;
      if (this.SafeZoneEntityId == 0L || !MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
        return;
      MyObjectBuilder_SafeZone objectBuilder = entity.GetObjectBuilder(false) as MyObjectBuilder_SafeZone;
      objectBuilder.ModelColor = (SerializableVector3) newColor.ToVector3();
      MySessionComponentSafeZones.RequestUpdateSafeZone_Player(this.m_parentBlock.EntityId, objectBuilder);
    }

    internal Color GetColor()
    {
      MySafeZone entity;
      return this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity) ? entity.ModelColor : Color.SkyBlue;
    }

    internal void SetActivate_Server(bool activate)
    {
      MySafeZone entity;
      if (!Sandbox.Game.Multiplayer.Sync.IsServer || this.m_processingActivation || (this.SafeZoneEntityId == 0L || !MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity)))
        return;
      this.m_processingActivation = true;
      if (this.m_timeLeft <= TimeSpan.Zero & activate && !this.m_activating && (!entity.Enabled && this.TryConsumeUpkeep()))
      {
        this.StartActivationCountdown();
        this.m_timeLeft = this.m_upkeepTime - TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      }
      else if (activate && (this.m_activating || this.m_timeLeft > TimeSpan.Zero))
        this.StartActivationCountdown();
      else if (!activate)
      {
        this.UpdateSafeZoneEnabled(entity, activate);
        this.m_timeLeft = this.m_upkeepTime - TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
      }
      this.m_processingActivation = false;
    }

    private void UpdateSafeZoneEnabled(MySafeZone safeZone, bool activate)
    {
      MyObjectBuilder_SafeZone objectBuilder = safeZone.GetObjectBuilder(false) as MyObjectBuilder_SafeZone;
      objectBuilder.Enabled = activate;
      MySessionComponentSafeZones.UpdateSafeZone(objectBuilder, true);
      Action safeZoneChanged = this.SafeZoneChanged;
      if (safeZoneChanged == null)
        return;
      safeZoneChanged();
    }

    internal void OnSafeZoneFilterBtnPressed()
    {
      MySafeZone entity;
      if (this.SafeZoneEntityId == 0L || !MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
        return;
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenSafeZoneFilter(new Vector2(0.5f, 0.5f), entity, new long?(this.Entity.EntityId)));
    }

    internal void OnSafeZoneSettingChanged(MySafeZoneAction safeZoneAction, bool isChecked)
    {
      MySafeZone entity;
      if (this.SafeZoneEntityId == 0L || !MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
        return;
      MyObjectBuilder_SafeZone objectBuilder = entity.GetObjectBuilder(false) as MyObjectBuilder_SafeZone;
      if (isChecked)
        objectBuilder.AllowedActions |= safeZoneAction;
      else
        objectBuilder.AllowedActions &= ~safeZoneAction;
      MySessionComponentSafeZones.RequestUpdateSafeZone_Player(this.m_parentBlock.EntityId, objectBuilder);
    }

    internal bool GetSafeZoneSetting(MySafeZoneAction safeZoneAction)
    {
      MySafeZone entity;
      return this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity) && entity.AllowedActions.HasFlag((Enum) safeZoneAction);
    }

    internal void OnSafeZoneShapeChanged(MySafeZoneShape newShape)
    {
      MySafeZone entity;
      if (this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
      {
        MyObjectBuilder_SafeZone objectBuilder = entity.GetObjectBuilder(false) as MyObjectBuilder_SafeZone;
        objectBuilder.Shape = newShape;
        MySessionComponentSafeZones.RequestUpdateSafeZone_Player(this.m_parentBlock.EntityId, objectBuilder);
      }
      Action safeZoneChanged = this.SafeZoneChanged;
      if (safeZoneChanged == null)
        return;
      safeZoneChanged();
    }

    internal long GetSafeZoneShape()
    {
      MySafeZone entity;
      return this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity) ? (long) entity.Shape : 0L;
    }

    internal long GetTexture()
    {
      MySafeZone entity;
      return this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity) ? (long) (int) entity.CurrentTexture : 0L;
    }

    internal void SetTexture(MyStringHash texture)
    {
      MySafeZone entity;
      if (this.SafeZoneEntityId == 0L || !MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
        return;
      MyObjectBuilder_SafeZone objectBuilder = entity.GetObjectBuilder(false) as MyObjectBuilder_SafeZone;
      objectBuilder.Texture = texture.String;
      MySessionComponentSafeZones.RequestUpdateSafeZone_Player(this.m_parentBlock.EntityId, objectBuilder);
    }

    internal float GetPowerDrain()
    {
      float num1 = 1E-06f;
      MySafeZone entity;
      if (this.SafeZoneEntityId == 0L || !MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity) || !entity.Enabled)
        return num1;
      float num2 = this.m_parentBlock.Definition.MaxSafeZonePowerDrainkW - this.m_parentBlock.Definition.MinSafeZonePowerDrainkW;
      float num3 = this.m_parentBlock.Definition.MaxSafeZoneRadius - this.m_parentBlock.Definition.MinSafeZoneRadius;
      float num4;
      if ((double) this.GetSafeZoneShape() == 0.0)
      {
        num4 = (float) (((double) this.GetRadius() - (double) this.m_parentBlock.Definition.MinSafeZoneRadius) / (double) num3 * (double) num2 / 1000.0);
      }
      else
      {
        float num5 = this.m_parentBlock.Definition.MaxSafeZoneRadius * 2f - this.m_parentBlock.Definition.MinSafeZoneRadius;
        Vector3 size = this.GetSize();
        num4 = (float) ((((double) size.X - (double) this.m_parentBlock.Definition.MinSafeZoneRadius) / (double) num5 / 3.0 + ((double) size.Y - (double) this.m_parentBlock.Definition.MinSafeZoneRadius) / (double) num5 / 3.0 + ((double) size.Z - (double) this.m_parentBlock.Definition.MinSafeZoneRadius) / (double) num5 / 3.0) * (double) num2 / 1000.0);
      }
      return this.m_parentBlock.Definition.MinSafeZonePowerDrainkW / 1000f + num4;
    }

    internal bool Update()
    {
      long num = this.m_safeZoneActivationTimeMS - (long) MySandboxGame.TotalGamePlayTimeInMilliseconds;
      MySafeZone entity;
      if (this.SafeZoneEntityId == 0L || !MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
        return this.m_activating;
      if (this.m_activating && num < 0L)
      {
        this.m_activating = false;
        if (entity != null && Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          if (this.m_timeLeft > TimeSpan.Zero)
          {
            this.m_upkeepTime = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds) + this.m_timeLeft;
            MyMultiplayer.RaiseEvent<MySafeZoneComponent, double>(this, (Func<MySafeZoneComponent, Action<double>>) (x => new Action<double>(x.SetUpkeepCoundown_Client)), this.m_timeLeft.TotalMinutes);
            this.m_timeLeft = TimeSpan.Zero;
            this.UpdateSafeZoneEnabled(entity, true);
          }
          else
            this.UpdateSafeZoneEnabled(entity, true);
        }
      }
      else if (entity.Enabled && this.m_upkeepTime - TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds) < TimeSpan.Zero && Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        this.m_processingActivation = true;
        if (!this.TryConsumeUpkeep())
          this.SafeZoneRemove_Server();
        this.m_processingActivation = false;
      }
      return this.m_activating || entity.Enabled || this.m_upkeepTime > TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
    }

    private bool TryConsumeUpkeep()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
      {
        MyLog.Default.Error("Trying to consume zone chips on client. This is not legal");
        return false;
      }
      if (!MyFakes.ENABLE_ZONE_CHIP_REQ)
        return true;
      MyInventory inventory = MyEntityExtensions.GetInventory(this.m_parentBlock);
      if (inventory == null)
        return false;
      if ((long) (int) inventory.GetItemAmount(MySafeZoneComponent.DEFINITION_ZONECHIP, MyItemFlags.None, false) >= (long) this.m_parentBlock.Definition.SafeZoneUpkeep)
      {
        inventory.RemoveItemsOfType((MyFixedPoint) (int) this.m_parentBlock.Definition.SafeZoneUpkeep, MySafeZoneComponent.DEFINITION_ZONECHIP, MyItemFlags.None, false);
      }
      else
      {
        if ((long) (int) this.m_parentBlock.CubeGrid.GridSystems.ConveyorSystem.PullItem(MySafeZoneComponent.DEFINITION_ZONECHIP, new MyFixedPoint?((MyFixedPoint) (int) this.m_parentBlock.Definition.SafeZoneUpkeep), (IMyConveyorEndpointBlock) this.m_parentBlock, MyEntityExtensions.GetInventory(this.m_parentBlock), false, MyFakes.CONV_PULL_CACL_IMMIDIATLY_STORE_SAFEZONE) < (long) this.m_parentBlock.Definition.SafeZoneUpkeep)
          return false;
        inventory.RemoveItemsOfType((MyFixedPoint) (int) this.m_parentBlock.Definition.SafeZoneUpkeep, MySafeZoneComponent.DEFINITION_ZONECHIP, MyItemFlags.None, false);
      }
      this.m_upkeepTime = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes((double) this.m_parentBlock.Definition.SafeZoneUpkeepTimeM);
      MyMultiplayer.RaiseEvent<MySafeZoneComponent, double>(this, (Func<MySafeZoneComponent, Action<double>>) (x => new Action<double>(x.SetUpkeepCoundown_Client)), (double) this.m_parentBlock.Definition.SafeZoneUpkeepTimeM);
      return true;
    }

    [Event(null, 743)]
    [Reliable]
    [Broadcast]
    private void SetUpkeepCoundown_Client(double minutes)
    {
      this.m_upkeepTime = TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds) + TimeSpan.FromMinutes(minutes);
      this.m_parentBlock.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    internal void SetTextInfo(StringBuilder sBuilderToSet)
    {
      sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Desc));
      if (this.WaitingResponse)
        sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Initializing));
      else if (this.SafeZoneEntityId == 0L)
        sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Disabled));
      else if (this.SafeZoneEntityId >= 0L)
      {
        bool flag = true;
        if (this.m_parentBlock.IsWorking)
        {
          if (this.m_activating)
          {
            long num = (this.m_safeZoneActivationTimeMS - (long) MySandboxGame.TotalGamePlayTimeInMilliseconds) / 1000L;
            if (num < 0L)
              num = 0L;
            sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Initializing));
            sBuilderToSet.Append(" " + (object) num);
            flag = false;
          }
          else
          {
            MySafeZone entity;
            if (this.SafeZoneEntityId != 0L && MyEntities.TryGetEntityById<MySafeZone>(this.SafeZoneEntityId, out entity))
            {
              if (entity.Enabled)
              {
                sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Enabled));
                if (MyFakes.ENABLE_ZONE_CHIP_REQ)
                {
                  sBuilderToSet.AppendLine();
                  StringBuilder otherStringBuilder = MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_NextUnkeepIn);
                  sBuilderToSet.AppendStringBuilder(otherStringBuilder);
                  TimeSpan timeSpan = this.m_upkeepTime - TimeSpan.FromMilliseconds((double) MySandboxGame.TotalGamePlayTimeInMilliseconds);
                  if (timeSpan < TimeSpan.Zero)
                    timeSpan = TimeSpan.Zero;
                  sBuilderToSet.Append(timeSpan.ToString("hh\\:mm\\:ss"));
                }
                flag = false;
              }
              else if (MySessionComponentSafeZones.IsSafeZoneColliding(this.SafeZoneEntityId, entity.WorldMatrix, entity.Shape, entity.Radius, entity.Size))
              {
                sBuilderToSet.Append((object) MyTexts.Get(MySpaceTexts.SafeZoneBlock_Safezone_Collision));
                flag = false;
              }
            }
          }
        }
        if (flag)
          sBuilderToSet.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_Inactive));
      }
      MyInventory inventory = MyEntityExtensions.GetInventory(this.m_parentBlock);
      if (inventory == null)
        return;
      sBuilderToSet.AppendLine();
      MyFixedPoint itemAmount = inventory.GetItemAmount(MySafeZoneComponent.DEFINITION_ZONECHIP, MyItemFlags.None, false);
      StringBuilder otherStringBuilder1 = MyTexts.Get(MySpaceTexts.Beacon_SafeZone_Info_ZoneChips);
      sBuilderToSet.AppendStringBuilder(otherStringBuilder1);
      sBuilderToSet.Append(itemAmount.ToString());
    }

    protected sealed class OnSafezoneCreateRemove\u003C\u003ESystem_Boolean : ICallSite<MySafeZoneComponent, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneComponent @this,
        in bool turnOnSafeZone,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSafezoneCreateRemove(turnOnSafeZone);
      }
    }

    protected sealed class StartActivationCoundown_Client\u003C\u003E : ICallSite<MySafeZoneComponent, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneComponent @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.StartActivationCoundown_Client();
      }
    }

    protected sealed class OnRadiusChanged_Server\u003C\u003E : ICallSite<MySafeZoneComponent, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneComponent @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRadiusChanged_Server();
      }
    }

    protected sealed class SetUpkeepCoundown_Client\u003C\u003ESystem_Double : ICallSite<MySafeZoneComponent, double, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySafeZoneComponent @this,
        in double minutes,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetUpkeepCoundown_Client(minutes);
      }
    }

    protected class m_safeZoneEntityId\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<long, SyncDirection.FromServer> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<long, SyncDirection.FromServer>(obj1, obj2));
        ((MySafeZoneComponent) obj0).m_safeZoneEntityId = (VRage.Sync.Sync<long, SyncDirection.FromServer>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
