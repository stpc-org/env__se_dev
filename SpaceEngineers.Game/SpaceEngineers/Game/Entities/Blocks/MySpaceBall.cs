// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MySpaceBall
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_SpaceBall))]
  [MyTerminalInterface(new System.Type[] {typeof (SpaceEngineers.Game.ModAPI.IMySpaceBall), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall)})]
  public class MySpaceBall : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMySpaceBall, SpaceEngineers.Game.ModAPI.IMyVirtualMass, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyVirtualMass, SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall
  {
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_friction;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_virtualMass;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_restitution;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_broadcastSync;
    private bool m_savedBroadcast;
    public const float DEFAULT_RESTITUTION = 0.4f;
    public const float DEFAULT_MASS = 100f;
    public const float DEFAULT_FRICTION = 0.5f;
    public const float REAL_MAXIMUM_RESTITUTION = 0.9f;
    public const float REAL_MINIMUM_MASS = 0.01f;

    public float Friction
    {
      get => (float) this.m_friction;
      set => this.m_friction.Value = value;
    }

    public float VirtualMass
    {
      get => (float) this.m_virtualMass;
      set => this.m_virtualMass.Value = value;
    }

    public float Restitution
    {
      get => (float) this.m_restitution;
      set => this.m_restitution.Value = value;
    }

    public bool Broadcast
    {
      get => this.RadioBroadcaster != null && this.RadioBroadcaster.Enabled;
      set => this.m_broadcastSync.Value = value;
    }

    private MySpaceBallDefinition BlockDefinition => (MySpaceBallDefinition) base.BlockDefinition;

    internal MyRadioBroadcaster RadioBroadcaster
    {
      get => (MyRadioBroadcaster) this.Components.Get<MyDataBroadcaster>();
      private set => this.Components.Add<MyDataBroadcaster>((MyDataBroadcaster) value);
    }

    internal MyRadioReceiver RadioReceiver
    {
      get => (MyRadioReceiver) this.Components.Get<MyDataReceiver>();
      set => this.Components.Add<MyDataReceiver>((MyDataReceiver) value);
    }

    public MySpaceBall()
    {
      this.CreateTerminalControls();
      this.m_baseIdleSound.Init("BlockArtMass");
      this.m_virtualMass.ValueChanged += (Action<SyncBase>) (x => this.RefreshPhysicsBody());
      this.m_broadcastSync.ValueChanged += (Action<SyncBase>) (x => this.BroadcastChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySpaceBall>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSlider<MySpaceBall> slider1 = new MyTerminalControlSlider<MySpaceBall>("VirtualMass", MySpaceTexts.BlockPropertyDescription_SpaceBallVirtualMass, MySpaceTexts.BlockPropertyDescription_SpaceBallVirtualMass);
      slider1.Getter = (MyTerminalValueControl<MySpaceBall, float>.GetterDelegate) (x => x.VirtualMass);
      slider1.Setter = (MyTerminalValueControl<MySpaceBall, float>.SetterDelegate) ((x, v) => x.VirtualMass = v);
      slider1.DefaultValueGetter = (MyTerminalValueControl<MySpaceBall, float>.GetterDelegate) (x => 100f);
      slider1.SetLimits((MyTerminalValueControl<MySpaceBall, float>.GetterDelegate) (x => 0.0f), (MyTerminalValueControl<MySpaceBall, float>.GetterDelegate) (x => x.BlockDefinition.MaxVirtualMass));
      slider1.Writer = (MyTerminalControl<MySpaceBall>.WriterDelegate) ((x, result) => MyValueFormatter.AppendWeightInBestUnit(x.VirtualMass, result));
      slider1.EnableActions<MySpaceBall>();
      MyTerminalControlFactory.AddControl<MySpaceBall>((MyTerminalControl<MySpaceBall>) slider1);
      if (MyPerGameSettings.BallFriendlyPhysics)
      {
        MyTerminalControlSlider<MySpaceBall> slider2 = new MyTerminalControlSlider<MySpaceBall>("Friction", MySpaceTexts.BlockPropertyDescription_SpaceBallFriction, MySpaceTexts.BlockPropertyDescription_SpaceBallFriction);
        slider2.Getter = (MyTerminalValueControl<MySpaceBall, float>.GetterDelegate) (x => x.Friction);
        slider2.Setter = (MyTerminalValueControl<MySpaceBall, float>.SetterDelegate) ((x, v) => x.Friction = v);
        slider2.DefaultValueGetter = (MyTerminalValueControl<MySpaceBall, float>.GetterDelegate) (x => 0.5f);
        slider2.SetLimits(0.0f, 1f);
        slider2.Writer = (MyTerminalControl<MySpaceBall>.WriterDelegate) ((x, result) => result.AppendInt32((int) ((double) x.Friction * 100.0)).Append("%"));
        slider2.EnableActions<MySpaceBall>();
        MyTerminalControlFactory.AddControl<MySpaceBall>((MyTerminalControl<MySpaceBall>) slider2);
        MyTerminalControlSlider<MySpaceBall> slider3 = new MyTerminalControlSlider<MySpaceBall>("Restitution", MySpaceTexts.BlockPropertyDescription_SpaceBallRestitution, MySpaceTexts.BlockPropertyDescription_SpaceBallRestitution);
        slider3.Getter = (MyTerminalValueControl<MySpaceBall, float>.GetterDelegate) (x => x.Restitution);
        slider3.Setter = (MyTerminalValueControl<MySpaceBall, float>.SetterDelegate) ((x, v) => x.Restitution = v);
        slider3.DefaultValueGetter = (MyTerminalValueControl<MySpaceBall, float>.GetterDelegate) (x => 0.4f);
        slider3.SetLimits(0.0f, 1f);
        slider3.Writer = (MyTerminalControl<MySpaceBall>.WriterDelegate) ((x, result) => result.AppendInt32((int) ((double) x.Restitution * 100.0)).Append("%"));
        slider3.EnableActions<MySpaceBall>();
        MyTerminalControlFactory.AddControl<MySpaceBall>((MyTerminalControl<MySpaceBall>) slider3);
      }
      MyTerminalControlCheckbox<MySpaceBall> checkbox = new MyTerminalControlCheckbox<MySpaceBall>("EnableBroadCast", MySpaceTexts.Antenna_EnableBroadcast, MySpaceTexts.Antenna_EnableBroadcast);
      checkbox.Getter = (MyTerminalValueControl<MySpaceBall, bool>.GetterDelegate) (x => x.Broadcast);
      checkbox.Setter = (MyTerminalValueControl<MySpaceBall, bool>.SetterDelegate) ((x, v) => x.Broadcast = v);
      checkbox.EnableAction<MySpaceBall>();
      MyTerminalControlFactory.AddControl<MySpaceBall>((MyTerminalControl<MySpaceBall>) checkbox);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.SyncFlag = true;
      this.NeedsWorldMatrix = true;
      this.RadioReceiver = new MyRadioReceiver();
      this.RadioBroadcaster = new MyRadioBroadcaster(50f);
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_SpaceBall builderSpaceBall = (MyObjectBuilder_SpaceBall) objectBuilder;
      this.m_virtualMass.SetLocalValue(MathHelper.Clamp(builderSpaceBall.VirtualMass, 0.0f, this.BlockDefinition.MaxVirtualMass));
      this.m_restitution.SetLocalValue(MathHelper.Clamp(builderSpaceBall.Restitution, 0.0f, 1f));
      this.m_friction.SetLocalValue(MathHelper.Clamp(builderSpaceBall.Friction, 0.0f, 1f));
      this.IsWorkingChanged += new Action<MyCubeBlock>(this.MySpaceBall_IsWorkingChanged);
      this.UpdateIsWorking();
      this.RefreshPhysicsBody();
      this.m_savedBroadcast = builderSpaceBall.EnableBroadcast;
      this.ShowOnHUD = false;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_SpaceBall builderCubeBlock = (MyObjectBuilder_SpaceBall) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.VirtualMass = (float) this.m_virtualMass;
      builderCubeBlock.Restitution = this.Restitution;
      builderCubeBlock.Friction = this.Friction;
      builderCubeBlock.EnableBroadcast = this.RadioBroadcaster.Enabled;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.CubeGrid.OnPhysicsChanged += new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
    }

    public override void OnRemovedFromScene(object source)
    {
      base.OnRemovedFromScene(source);
      this.CubeGrid.OnPhysicsChanged -= new Action<MyEntity>(this.CubeGrid_OnPhysicsChanged);
    }

    private void CubeGrid_OnPhysicsChanged(MyEntity obj) => this.UpdatePhysics();

    private void RefreshPhysicsBody()
    {
      if (!this.CubeGrid.CreatePhysics)
        return;
      if (this.Physics != null)
        this.Physics.Close();
      HkSphereShape hkSphereShape = new HkSphereShape(this.CubeGrid.GridSize * 0.5f);
      HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeSphereVolumeMassProperties(hkSphereShape.Radius, (double) this.VirtualMass != 0.0 ? this.VirtualMass : 0.01f);
      this.Physics = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_KEYFRAMED_REPORTING);
      this.Physics.IsPhantom = false;
      this.Physics.CreateFromCollisionObject((HkShape) hkSphereShape, Vector3.Zero, this.WorldMatrix, new HkMassProperties?(volumeMassProperties), 25);
      this.UpdateIsWorking();
      this.Physics.Enabled = this.IsWorking && this.CubeGrid.Physics != null && this.CubeGrid.Physics.Enabled;
      this.Physics.RigidBody.Activate();
      hkSphereShape.Base.RemoveReference();
      if (this.CubeGrid == null || this.CubeGrid.Physics == null || this.CubeGrid.IsStatic)
        return;
      this.CubeGrid.Physics.UpdateMass();
    }

    private void UpdatePhysics()
    {
      if (this.Physics == null)
        return;
      this.Physics.Enabled = this.IsWorking && this.CubeGrid.Physics != null && this.CubeGrid.Physics.Enabled;
    }

    public void UpdateRadios(bool isTrue)
    {
      if (this.RadioBroadcaster == null || this.RadioReceiver == null)
        return;
      this.RadioBroadcaster.WantsToBeEnabled = isTrue;
      this.RadioReceiver.Enabled = isTrue & this.Enabled;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.CubeGrid.Physics != null && !this.CubeGrid.IsStatic)
        this.CubeGrid.Physics.UpdateMass();
      if (this.Physics != null)
        this.UpdatePhysics();
      this.UpdateRadios(this.m_savedBroadcast);
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.RadioReceiver.UpdateBroadcastersInRange();
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      this.RadioReceiver.UpdateBroadcastersInRange();
    }

    private void MySpaceBall_IsWorkingChanged(MyCubeBlock obj) => this.UpdateRadios(this.IsWorking);

    public override void ContactPointCallback(ref MyGridContactInfo value)
    {
      HkContactPointProperties contactProperties = value.Event.ContactProperties;
      value.EnableDeformation = false;
      value.EnableParticles = false;
      value.RubberDeformation = false;
      if (!MyPerGameSettings.BallFriendlyPhysics)
        return;
      contactProperties.Friction = this.Friction;
      contactProperties.Restitution = (double) this.Restitution > 0.899999976158142 ? 0.9f : this.Restitution;
    }

    protected override void WorldPositionChanged(object source)
    {
      base.WorldPositionChanged(source);
      if (this.RadioBroadcaster == null)
        return;
      this.RadioBroadcaster.MoveBroadcaster();
    }

    public override float GetMass() => (double) this.VirtualMass <= 0.0 ? 0.01f : this.VirtualMass;

    private void BroadcastChanged()
    {
      this.RadioBroadcaster.Enabled = (bool) this.m_broadcastSync;
      this.RaisePropertiesChanged();
    }

    public override List<MyHudEntityParams> GetHudParams(bool allowBlink)
    {
      if (this.ShowOnHUD || this.IsBeingHacked & allowBlink)
        return base.GetHudParams(allowBlink);
      this.m_hudParams.Clear();
      return this.m_hudParams;
    }

    float SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.VirtualMass
    {
      get => this.GetMass();
      set => this.VirtualMass = MathHelper.Clamp(value, 0.01f, this.BlockDefinition.MaxVirtualMass);
    }

    float SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.Friction
    {
      get => this.Friction;
      set => this.Friction = MathHelper.Clamp(value, 0.0f, 1f);
    }

    float SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.Restitution
    {
      get => this.Restitution;
      set => this.Restitution = MathHelper.Clamp(value, 0.0f, 1f);
    }

    bool SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.IsBroadcasting => this.Broadcast;

    bool SpaceEngineers.Game.ModAPI.Ingame.IMySpaceBall.Broadcasting
    {
      get => this.Broadcast;
      set => this.Broadcast = value;
    }

    protected class m_friction\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MySpaceBall) obj0).m_friction = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_virtualMass\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MySpaceBall) obj0).m_virtualMass = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_restitution\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MySpaceBall) obj0).m_restitution = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_broadcastSync\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MySpaceBall) obj0).m_broadcastSync = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
