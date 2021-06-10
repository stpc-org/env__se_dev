// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyDataBroadcaster
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Entities
{
  public class MyDataBroadcaster : MyEntityComponentBase, IMyEventProxy, IMyEventOwner
  {
    public Vector3D BroadcastPosition => this.Entity.PositionComp.GetPosition();

    public override string ComponentTypeDebugString => nameof (MyDataBroadcaster);

    public List<MyHudEntityParams> GetHudParams(bool allowBlink) => ((MyEntity) this.Entity).GetHudParams(allowBlink);

    public MyDataReceiver Receiver
    {
      get
      {
        MyDataReceiver component = (MyDataReceiver) null;
        if (this.Container != null)
          this.Container.TryGet<MyDataReceiver>(out component);
        return component;
      }
    }

    public bool Closed => this.Entity == null || this.Entity.MarkedForClose || this.Entity.Closed;

    public long Owner
    {
      get
      {
        MyIDModule entityIdModule = this.TryGetEntityIdModule();
        return entityIdModule == null ? 0L : entityIdModule.Owner;
      }
    }

    public virtual bool ShowOnHud => true;

    public bool ShowInTerminal
    {
      get
      {
        if (this.Entity is MyCharacter)
          return false;
        if (this.Entity is MyCubeBlock)
          return true;
        return this.Entity is MyProxyAntenna && !(this.Entity as MyProxyAntenna).IsCharacter;
      }
    }

    public bool CanBeUsedByPlayer(long playerId) => MyDataBroadcaster.CanBeUsedByPlayer(playerId, this.Entity);

    public MyAntennaSystem.BroadcasterInfo Info
    {
      get
      {
        if (this.Entity is MyCharacter)
          return new MyAntennaSystem.BroadcasterInfo()
          {
            EntityId = this.Entity.EntityId,
            Name = this.Entity.DisplayName
          };
        if (this.Entity is MyCubeBlock)
        {
          MyCubeGrid groupRepresentative = MyAntennaSystem.Static.GetLogicalGroupRepresentative((this.Entity as MyCubeBlock).CubeGrid);
          return new MyAntennaSystem.BroadcasterInfo()
          {
            EntityId = groupRepresentative.EntityId,
            Name = groupRepresentative.DisplayName
          };
        }
        return this.Entity is MyProxyAntenna ? (this.Entity as MyProxyAntenna).Info : new MyAntennaSystem.BroadcasterInfo();
      }
    }

    public long AntennaEntityId => this.Entity is MyProxyAntenna entity ? entity.AntennaEntityId : this.Entity.EntityId;

    public bool HasRemoteControl
    {
      get
      {
        if (this.Entity is MyCharacter)
          return false;
        MyCubeGrid hostingGrid = this.TryGetHostingGrid();
        if (hostingGrid != null)
          return hostingGrid.GetFatBlockCount<MyRemoteControl>() > 0;
        return this.Entity is MyProxyAntenna && (this.Entity as MyProxyAntenna).HasRemoteControl;
      }
    }

    public long? MainRemoteControlOwner
    {
      get
      {
        if (this.Entity is MyCharacter)
          return new long?();
        MyCubeGrid hostingGrid = this.TryGetHostingGrid();
        return hostingGrid != null ? MyDataBroadcaster.GetRemoteConrolForGrid(hostingGrid)?.OwnerId : (this.Entity is MyProxyAntenna ? (this.Entity as MyProxyAntenna).MainRemoteControlOwner : new long?());
      }
    }

    public long? MainRemoteControlId
    {
      get
      {
        if (this.Entity is MyCharacter)
          return new long?();
        MyCubeGrid hostingGrid = this.TryGetHostingGrid();
        return hostingGrid != null ? MyDataBroadcaster.GetRemoteConrolForGrid(hostingGrid)?.EntityId : (this.Entity is MyProxyAntenna ? (this.Entity as MyProxyAntenna).MainRemoteControlId : new long?());
      }
    }

    public MyOwnershipShareModeEnum MainRemoteControlSharing
    {
      get
      {
        if (this.Entity is MyCharacter)
          return MyOwnershipShareModeEnum.None;
        MyCubeGrid hostingGrid = this.TryGetHostingGrid();
        if (hostingGrid != null)
        {
          MyTerminalBlock remoteConrolForGrid = MyDataBroadcaster.GetRemoteConrolForGrid(hostingGrid);
          return remoteConrolForGrid == null ? MyOwnershipShareModeEnum.None : remoteConrolForGrid.IDModule.ShareMode;
        }
        return this.Entity is MyProxyAntenna ? (this.Entity as MyProxyAntenna).MainRemoteControlSharing : MyOwnershipShareModeEnum.None;
      }
    }

    public virtual void InitProxyObjectBuilder(MyObjectBuilder_ProxyAntenna ob)
    {
      ob.HasReceiver = this.Receiver != null;
      ob.IsCharacter = this.Entity is MyCharacter;
      ob.Position = (SerializableVector3D) this.BroadcastPosition;
      ob.HudParams = new List<MyObjectBuilder_HudEntityParams>();
      foreach (MyHudEntityParams hudParam in this.GetHudParams(false))
        ob.HudParams.Add(hudParam.GetObjectBuilder());
      ob.InfoEntityId = this.Info.EntityId;
      ob.InfoName = this.Info.Name;
      ob.Owner = this.Owner;
      ob.Share = this.GetShare();
      ob.AntennaEntityId = this.AntennaEntityId;
      ob.PersistentFlags |= MyPersistentEntityFlags2.Enabled | MyPersistentEntityFlags2.InScene;
      ob.HasRemote = this.HasRemoteControl;
      ob.MainRemoteOwner = this.MainRemoteControlOwner;
      ob.MainRemoteId = this.MainRemoteControlId;
      ob.MainRemoteSharing = this.MainRemoteControlSharing;
    }

    private MyOwnershipShareModeEnum GetShare()
    {
      MyIDModule entityIdModule = this.TryGetEntityIdModule();
      return entityIdModule == null ? MyOwnershipShareModeEnum.None : entityIdModule.ShareMode;
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      if (!Sync.IsServer || !(this.Entity is MyCubeBlock entity))
        return;
      entity.CubeGrid.OnNameChanged += new Action<MyCubeGrid>(this.RaiseNameChanged);
      if (!(entity is MyTerminalBlock myTerminalBlock))
        return;
      myTerminalBlock.CustomNameChanged += new Action<MyTerminalBlock>(this.RaiseAntennaNameChanged);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      if (!Sync.IsServer || !(this.Entity is MyCubeBlock entity))
        return;
      entity.CubeGrid.OnNameChanged -= new Action<MyCubeGrid>(this.RaiseNameChanged);
      if (!(entity is MyTerminalBlock myTerminalBlock))
        return;
      myTerminalBlock.CustomNameChanged -= new Action<MyTerminalBlock>(this.RaiseAntennaNameChanged);
    }

    public void RaiseOwnerChanged()
    {
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyDataBroadcaster, long, MyOwnershipShareModeEnum>(this, (Func<MyDataBroadcaster, Action<long, MyOwnershipShareModeEnum>>) (x => new Action<long, MyOwnershipShareModeEnum>(x.OnOwnerChanged)), this.Owner, this.GetShare());
      this.UpdateHudParams(this.Entity as MyEntity);
    }

    public void RaiseNameChanged(MyCubeGrid grid)
    {
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyDataBroadcaster, string>(this, (Func<MyDataBroadcaster, Action<string>>) (x => new Action<string>(x.OnNameChanged)), this.Info.Name);
    }

    public void RaiseAntennaNameChanged(MyTerminalBlock block) => this.UpdateHudParams((MyEntity) block);

    public void UpdateRemoteControlInfo()
    {
      if (!Sync.IsServer || this.Entity == null || !((MyEntity) this.Entity).IsReadyForReplication)
        return;
      MyMultiplayer.RaiseEvent<MyDataBroadcaster, bool, long?, MyOwnershipShareModeEnum, long?>(this, (Func<MyDataBroadcaster, Action<bool, long?, MyOwnershipShareModeEnum, long?>>) (x => new Action<bool, long?, MyOwnershipShareModeEnum, long?>(x.UpdateRemoteControlState)), this.HasRemoteControl, this.MainRemoteControlOwner, this.MainRemoteControlSharing, this.MainRemoteControlId);
    }

    public void UpdateHudParams(MyEntity entity)
    {
      if (!Sync.IsServer)
        return;
      List<MyObjectBuilder_HudEntityParams> builderHudEntityParamsList = new List<MyObjectBuilder_HudEntityParams>();
      foreach (MyHudEntityParams hudParam in entity.GetHudParams(false))
        builderHudEntityParamsList.Add(hudParam.GetObjectBuilder());
      MyMultiplayer.RaiseEvent<MyDataBroadcaster, List<MyObjectBuilder_HudEntityParams>>(this, (Func<MyDataBroadcaster, Action<List<MyObjectBuilder_HudEntityParams>>>) (x => new Action<List<MyObjectBuilder_HudEntityParams>>(x.OnUpdateHudParams)), builderHudEntityParamsList);
    }

    [Event(null, 354)]
    [Reliable]
    [Broadcast]
    private void OnOwnerChanged(long newOwner, MyOwnershipShareModeEnum newShare)
    {
      if (!(this.Entity is MyProxyAntenna entity))
        return;
      entity.ChangeOwner(newOwner, newShare);
    }

    [Event(null, 364)]
    [Reliable]
    [Broadcast]
    private void OnNameChanged(string newName)
    {
      if (!(this.Entity is MyProxyAntenna entity))
        return;
      entity.Info = new MyAntennaSystem.BroadcasterInfo()
      {
        EntityId = entity.Info.EntityId,
        Name = newName
      };
    }

    [Event(null, 374)]
    [Reliable]
    [Broadcast]
    private void UpdateRemoteControlState(
      bool hasRemote,
      long? owner,
      MyOwnershipShareModeEnum sharing,
      long? remoteId)
    {
      if (!(this.Entity is MyProxyAntenna entity))
        return;
      entity.HasRemoteControl = hasRemote;
      entity.MainRemoteControlOwner = owner;
      entity.MainRemoteControlId = remoteId;
      entity.MainRemoteControlSharing = sharing;
    }

    [Event(null, 389)]
    [Reliable]
    [Broadcast]
    private void OnUpdateHudParams(List<MyObjectBuilder_HudEntityParams> newHudParams)
    {
      if (!(this.Entity is MyProxyAntenna entity))
        return;
      entity.ChangeHudParams(newHudParams);
    }

    private MyCubeGrid TryGetHostingGrid() => !(this.Entity is MyCubeBlock entity) ? (MyCubeGrid) null : entity.CubeGrid;

    private static MyTerminalBlock GetRemoteConrolForGrid(MyCubeGrid cubeGrid)
    {
      if (cubeGrid.HasMainRemoteControl())
        return cubeGrid.MainRemoteControl;
      MyFatBlockReader<MyRemoteControl> fatBlocks = cubeGrid.GetFatBlocks<MyRemoteControl>();
      if (!fatBlocks.MoveNext())
        return (MyTerminalBlock) null;
      MyRemoteControl current = fatBlocks.Current;
      return !fatBlocks.MoveNext() ? (MyTerminalBlock) current : (MyTerminalBlock) null;
    }

    private MyIDModule TryGetEntityIdModule()
    {
      MyIDModule component;
      return this.Entity is IMyComponentOwner<MyIDModule> entity && entity.GetComponent(out component) ? component : (MyIDModule) null;
    }

    public static bool CanBeUsedByPlayer(long playerId, IMyEntity Entity)
    {
      MyIDModule component;
      if (Entity is MyTerminalBlock myTerminalBlock && myTerminalBlock.HasAdminUseTerminals(playerId) || (!(Entity is IMyComponentOwner<MyIDModule> myComponentOwner) || !myComponentOwner.GetComponent(out component)))
        return true;
      switch (component.GetUserRelationToOwner(playerId))
      {
        case MyRelationsBetweenPlayerAndBlock.NoOwnership:
        case MyRelationsBetweenPlayerAndBlock.Neutral:
        case MyRelationsBetweenPlayerAndBlock.Enemies:
          return false;
        default:
          return true;
      }
    }

    protected sealed class OnOwnerChanged\u003C\u003ESystem_Int64\u0023VRage_Game_MyOwnershipShareModeEnum : ICallSite<MyDataBroadcaster, long, MyOwnershipShareModeEnum, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyDataBroadcaster @this,
        in long newOwner,
        in MyOwnershipShareModeEnum newShare,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnOwnerChanged(newOwner, newShare);
      }
    }

    protected sealed class OnNameChanged\u003C\u003ESystem_String : ICallSite<MyDataBroadcaster, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyDataBroadcaster @this,
        in string newName,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnNameChanged(newName);
      }
    }

    protected sealed class UpdateRemoteControlState\u003C\u003ESystem_Boolean\u0023System_Nullable`1\u003CSystem_Int64\u003E\u0023VRage_Game_MyOwnershipShareModeEnum\u0023System_Nullable`1\u003CSystem_Int64\u003E : ICallSite<MyDataBroadcaster, bool, long?, MyOwnershipShareModeEnum, long?, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyDataBroadcaster @this,
        in bool hasRemote,
        in long? owner,
        in MyOwnershipShareModeEnum sharing,
        in long? remoteId,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.UpdateRemoteControlState(hasRemote, owner, sharing, remoteId);
      }
    }

    protected sealed class OnUpdateHudParams\u003C\u003ESystem_Collections_Generic_List`1\u003CVRage_Game_MyObjectBuilder_HudEntityParams\u003E : ICallSite<MyDataBroadcaster, List<MyObjectBuilder_HudEntityParams>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyDataBroadcaster @this,
        in List<MyObjectBuilder_HudEntityParams> newHudParams,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateHudParams(newHudParams);
      }
    }

    private class Sandbox_Game_Entities_MyDataBroadcaster\u003C\u003EActor : IActivator, IActivator<MyDataBroadcaster>
    {
      object IActivator.CreateInstance() => (object) new MyDataBroadcaster();

      MyDataBroadcaster IActivator<MyDataBroadcaster>.CreateInstance() => new MyDataBroadcaster();
    }
  }
}
