// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyDoorBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_DoorBase))]
  public abstract class MyDoorBase : MyFunctionalBlock
  {
    private bool m_contactCallbackQueued;
    protected readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_open;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_anyoneCanUse;

    public MyDoorBase() => this.CreateTerminalControls();

    public bool Open
    {
      get => (bool) this.m_open;
      set
      {
        if ((bool) this.m_open == value || !this.Enabled || (!this.IsWorking || !this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)))
          return;
        this.m_open.Value = value;
        this.ResolveContactEnableSubparts(!value);
      }
    }

    public bool AnyoneCanUse
    {
      get => (bool) this.m_anyoneCanUse;
      set => this.m_anyoneCanUse.Value = value;
    }

    public override MyCubeBlockHighlightModes HighlightMode => this.AnyoneCanUse ? MyCubeBlockHighlightModes.AlwaysCanUse : MyCubeBlockHighlightModes.Default;

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.m_anyoneCanUse.SetLocalValue((objectBuilder as MyObjectBuilder_DoorBase).AnyoneCanUse);
      if (!this.EnableContactCallbacks())
        return;
      this.IsWorkingChanged += (Action<MyCubeBlock>) (x => this.ResolveContactEnableSubparts());
      this.EnabledChanged += (Action<MyTerminalBlock>) (x => this.ResolveContactEnableSubparts());
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_DoorBase builderCubeBlock = (MyObjectBuilder_DoorBase) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.AnyoneCanUse = this.m_anyoneCanUse.Value;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyDoorBase>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlOnOffSwitch<MyDoorBase> onOff = new MyTerminalControlOnOffSwitch<MyDoorBase>("Open", MySpaceTexts.Blank, on: new MyStringId?(MySpaceTexts.BlockAction_DoorOpen), off: new MyStringId?(MySpaceTexts.BlockAction_DoorClosed));
      onOff.Getter = (MyTerminalValueControl<MyDoorBase, bool>.GetterDelegate) (x => x.Open);
      onOff.Setter = (MyTerminalValueControl<MyDoorBase, bool>.SetterDelegate) ((x, v) => x.SetOpenRequest(v, x.OwnerId));
      onOff.EnableToggleAction<MyDoorBase>();
      onOff.EnableOnOffActions<MyDoorBase>();
      MyTerminalControlFactory.AddControl<MyDoorBase>((MyTerminalControl<MyDoorBase>) onOff);
      MyTerminalControlCheckbox<MyDoorBase> checkbox = new MyTerminalControlCheckbox<MyDoorBase>("AnyoneCanUse", MySpaceTexts.BlockPropertyText_AnyoneCanUse, MySpaceTexts.BlockPropertyDescription_AnyoneCanUse);
      checkbox.Getter = (MyTerminalValueControl<MyDoorBase, bool>.GetterDelegate) (x => x.AnyoneCanUse);
      checkbox.Setter = (MyTerminalValueControl<MyDoorBase, bool>.SetterDelegate) ((x, v) => x.AnyoneCanUse = v);
      checkbox.EnableAction<MyDoorBase>();
      MyTerminalControlFactory.AddControl<MyDoorBase>((MyTerminalControl<MyDoorBase>) checkbox);
    }

    public void SetOpenRequest(bool open, long identityId) => MyMultiplayer.RaiseEvent<MyDoorBase, bool, long>(this, (Func<MyDoorBase, Action<bool, long>>) (x => new Action<bool, long>(x.OpenRequest)), open, identityId);

    [Event(null, 124)]
    [Reliable]
    [Server(ValidationType.Access)]
    private void OpenRequest(bool open, long identityId)
    {
      bool flag = this.AnyoneCanUse || this.HasPlayerAccess(identityId);
      MyIdentity identity = MySession.Static.Players.TryGetIdentity(identityId);
      MyPlayer myPlayer = identity == null || identity.Character == null ? (MyPlayer) null : MyPlayer.GetPlayerFromCharacter(identity.Character);
      AdminSettingsEnum adminSettingsEnum;
      if (myPlayer != null && !flag && MySession.Static.RemoteAdminSettings.TryGetValue(myPlayer.Client.SteamUserId, out adminSettingsEnum))
        flag = adminSettingsEnum.HasFlag((Enum) AdminSettingsEnum.UseTerminals);
      if (!flag)
        return;
      this.Open = open;
    }

    protected void CreateSubpartConstraint(
      MyEntity subpart,
      out HkFixedConstraintData constraintData,
      out HkConstraint constraint)
    {
      constraintData = (HkFixedConstraintData) null;
      constraint = (HkConstraint) null;
      if (this.CubeGrid.Physics == null)
        return;
      uint info = HkGroupFilter.CalcFilterInfo(subpart.GetPhysicsBody().RigidBody.Layer, this.CubeGrid.GetPhysicsBody().HavokCollisionSystemID, 1, 1);
      subpart.Physics.RigidBody.SetCollisionFilterInfo(info);
      if (this.EnableContactCallbacks())
      {
        subpart.Physics.RigidBody.ContactPointCallback += new HkContactPointEventHandler(this.ContactCallback);
        this.ResolveContactEnableSubpart(subpart);
      }
      subpart.Physics.Enabled = true;
      constraintData = new HkFixedConstraintData();
      constraintData.SetSolvingMethod(HkSolvingMethod.MethodStabilized);
      constraintData.SetInertiaStabilizationFactor(1f);
      constraint = new HkConstraint(!((HkReferenceObject) this.CubeGrid.Physics.RigidBody2 != (HkReferenceObject) null) || !this.CubeGrid.Physics.Flags.HasFlag((Enum) RigidBodyFlag.RBF_DOUBLED_KINEMATIC) ? this.CubeGrid.Physics.RigidBody : this.CubeGrid.Physics.RigidBody2, subpart.Physics.RigidBody, (HkConstraintData) constraintData);
      constraint.WantRuntime = true;
    }

    private void ResolveContactEnableSubparts(bool skipClosingCheck = false)
    {
      bool contactEnableState = this.GetContactEnableState(skipClosingCheck);
      foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
      {
        if (subpart.Value.Physics != null && subpart.Value.Physics.RigidBody.ContactPointCallbackEnabled != contactEnableState)
          subpart.Value.Physics.RigidBody.ContactPointCallbackEnabled = contactEnableState;
      }
    }

    private void DisableSubpartCallbacks()
    {
      foreach (KeyValuePair<string, MyEntitySubpart> subpart in this.Subparts)
      {
        if (subpart.Value.Physics != null)
          subpart.Value.Physics.RigidBody.ContactPointCallbackEnabled = false;
      }
    }

    private void ResolveContactEnableSubpart(MyEntity subpart)
    {
      bool contactEnableState = this.GetContactEnableState();
      if (subpart.Physics.RigidBody.ContactPointCallbackEnabled == contactEnableState)
        return;
      subpart.Physics.RigidBody.ContactPointCallbackEnabled = contactEnableState;
    }

    private bool GetContactEnableState(bool skipClosingCheck = false)
    {
      if (!this.EnableContactCallbacks() || (bool) this.m_open || (!this.IsWorking || !this.Enabled))
        return false;
      return skipClosingCheck || this.IsClosing();
    }

    public abstract bool IsClosing();

    private void ContactCallback(ref HkContactPointEvent contactEvent)
    {
      if (this.m_contactCallbackQueued)
        return;
      this.m_contactCallbackQueued = true;
      MySandboxGame.Static.Invoke((Action) (() => this.ContactCallbackInternal()), "Door Callback");
    }

    public virtual void ContactCallbackInternal()
    {
      this.DisableSubpartCallbacks();
      this.m_contactCallbackQueued = false;
    }

    public virtual bool EnableContactCallbacks() => MyFakes.ENABLE_DOOR_SAFETY;

    protected void DisposeSubpartConstraint(
      ref HkConstraint constraint,
      ref HkFixedConstraintData constraintData)
    {
      if ((HkReferenceObject) constraint == (HkReferenceObject) null)
        return;
      this.CubeGrid.Physics.RemoveConstraint(constraint);
      constraint.Dispose();
      constraint = (HkConstraint) null;
      constraintData = (HkFixedConstraintData) null;
    }

    protected static void SetupDoorSubpart(
      MyEntitySubpart subpart,
      int havokCollisionSystemID,
      bool refreshInPlace)
    {
      if (subpart == null || subpart.Physics == null || (subpart.ModelCollision.HavokCollisionShapes == null || subpart.ModelCollision.HavokCollisionShapes.Length == 0))
        return;
      uint info = HkGroupFilter.CalcFilterInfo(subpart.GetPhysicsBody().RigidBody.Layer, havokCollisionSystemID, 1, 1);
      subpart.Physics.RigidBody.SetCollisionFilterInfo(info);
      if (!(subpart.GetPhysicsBody().HavokWorld != null & refreshInPlace))
        return;
      MyPhysics.RefreshCollisionFilter(subpart.GetPhysicsBody());
    }

    protected sealed class OpenRequest\u003C\u003ESystem_Boolean\u0023System_Int64 : ICallSite<MyDoorBase, bool, long, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyDoorBase @this,
        in bool open,
        in long identityId,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OpenRequest(open, identityId);
      }
    }

    protected class m_open\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyDoorBase) obj0).m_open = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_anyoneCanUse\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyDoorBase) obj0).m_anyoneCanUse = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
