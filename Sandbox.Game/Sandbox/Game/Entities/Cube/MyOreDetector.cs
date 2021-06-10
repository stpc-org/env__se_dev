// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyOreDetector
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_OreDetector))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyOreDetector), typeof (Sandbox.ModAPI.Ingame.IMyOreDetector)})]
  public class MyOreDetector : MyFunctionalBlock, IMyComponentOwner<MyOreDetectorComponent>, Sandbox.ModAPI.IMyOreDetector, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyOreDetector
  {
    private MyOreDetectorDefinition m_definition;
    private readonly MyOreDetectorComponent m_oreDetectorComponent = new MyOreDetectorComponent();
    private VRage.Sync.Sync<bool, SyncDirection.BothWays> m_broadcastUsingAntennas;
    private VRage.Sync.Sync<float, SyncDirection.BothWays> m_range;
    private static readonly short UPDATE_HUD_TIMEOUT = 200;

    public MyOreDetector()
    {
      this.CreateTerminalControls();
      this.m_broadcastUsingAntennas.ValueChanged += (Action<SyncBase>) (entity => this.BroadcastChanged());
      this.m_range.ValueChanged += (Action<SyncBase>) (entity => this.UpdateRange());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyOreDetector>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSlider<MyOreDetector> terminalControlSlider = new MyTerminalControlSlider<MyOreDetector>("Range", MySpaceTexts.BlockPropertyTitle_OreDetectorRange, MySpaceTexts.BlockPropertyDescription_OreDetectorRange);
      terminalControlSlider.SetLimits((MyTerminalValueControl<MyOreDetector, float>.GetterDelegate) (x => 0.0f), (MyTerminalValueControl<MyOreDetector, float>.GetterDelegate) (x => x.m_definition.MaximumRange));
      terminalControlSlider.DefaultValue = new float?(100f);
      terminalControlSlider.Getter = (MyTerminalValueControl<MyOreDetector, float>.GetterDelegate) (x => (float) ((double) x.Range * (double) x.m_definition.MaximumRange * 0.00999999977648258));
      terminalControlSlider.Setter = (MyTerminalValueControl<MyOreDetector, float>.SetterDelegate) ((x, v) => x.Range = (float) ((double) v / (double) x.m_definition.MaximumRange * 100.0));
      terminalControlSlider.Writer = (MyTerminalControl<MyOreDetector>.WriterDelegate) ((x, result) => result.AppendInt32((int) x.m_oreDetectorComponent.DetectionRadius).Append(" m"));
      MyTerminalControlFactory.AddControl<MyOreDetector>((MyTerminalControl<MyOreDetector>) terminalControlSlider);
      MyTerminalControlCheckbox<MyOreDetector> checkbox = new MyTerminalControlCheckbox<MyOreDetector>("BroadcastUsingAntennas", MySpaceTexts.BlockPropertyDescription_BroadcastUsingAntennas, MySpaceTexts.BlockPropertyDescription_BroadcastUsingAntennas);
      checkbox.Getter = (MyTerminalValueControl<MyOreDetector, bool>.GetterDelegate) (x => x.m_oreDetectorComponent.BroadcastUsingAntennas);
      checkbox.Setter = (MyTerminalValueControl<MyOreDetector, bool>.SetterDelegate) ((x, v) => x.m_broadcastUsingAntennas.Value = v);
      checkbox.EnableAction<MyOreDetector>();
      MyTerminalControlFactory.AddControl<MyOreDetector>((MyTerminalControl<MyOreDetector>) checkbox);
    }

    private void BroadcastChanged() => this.BroadcastUsingAntennas = (bool) this.m_broadcastUsingAntennas;

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.m_definition = this.BlockDefinition as MyOreDetectorDefinition;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.m_definition.ResourceSinkGroup, 1f / 500f, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      this.ResourceSink = resourceSinkComponent;
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_OreDetector builderOreDetector = objectBuilder as MyObjectBuilder_OreDetector;
      this.m_oreDetectorComponent.DetectionRadius = (double) builderOreDetector.DetectionRadius == 0.0 ? MathHelper.Clamp(0.5f * this.m_definition.MaximumRange, 1f, this.m_definition.MaximumRange) : MathHelper.Clamp(builderOreDetector.DetectionRadius, 1f, this.m_definition.MaximumRange);
      this.m_oreDetectorComponent.BroadcastUsingAntennas = builderOreDetector.BroadcastUsingAntennas;
      this.m_broadcastUsingAntennas.SetLocalValue(this.m_oreDetectorComponent.BroadcastUsingAntennas);
      this.m_oreDetectorComponent.OnCheckControl += new MyOreDetectorComponent.CheckControlDelegate(this.OnCheckControl);
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.OnClose += new Action<MyEntity>(this.MyOreDetector_OnClose);
    }

    private void MyOreDetector_OnClose(MyEntity obj)
    {
      if (this.m_oreDetectorComponent == null)
        return;
      this.m_oreDetectorComponent.DiscardNextQuery();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_OreDetector builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_OreDetector;
      builderCubeBlock.DetectionRadius = this.m_oreDetectorComponent.DetectionRadius;
      builderCubeBlock.BroadcastUsingAntennas = this.m_oreDetectorComponent.BroadcastUsingAntennas;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
      if (this.Enabled)
        return;
      this.m_oreDetectorComponent.Clear();
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      if (this.IsWorking)
        return;
      this.m_oreDetectorComponent.Clear();
    }

    public override void OnUnregisteredFromGridSystems()
    {
      this.m_oreDetectorComponent.Clear();
      base.OnUnregisteredFromGridSystems();
    }

    public override void UpdateBeforeSimulation100()
    {
      base.UpdateBeforeSimulation100();
      bool flag1 = this.HasLocalPlayerAccess();
      if (!this.IsWorking)
        return;
      bool flag2 = false;
      if (flag1 && MySession.Static.LocalCharacter != null)
      {
        if (this.m_oreDetectorComponent.BroadcastUsingAntennas)
        {
          MyCharacter localCharacter = MySession.Static.LocalCharacter;
          if (this.GetTopMostParent((Type) null) is MyCubeGrid topMostParent)
          {
            MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(topMostParent);
            if (group != null && localCharacter.HasAccessToLogicalGroup(group.GroupData))
              flag2 = true;
          }
        }
        else
        {
          IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
          if (controlledEntity != null && controlledEntity.Entity != null && controlledEntity.Entity.GetTopMostParent((Type) null) is MyCubeGrid topMostParent)
          {
            MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group1 = MyCubeGridGroups.Static.Logical.GetGroup(topMostParent);
            MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group2 = MyCubeGridGroups.Static.Logical.GetGroup(this.CubeGrid);
            if (topMostParent == this.CubeGrid || group1 != null && group2 != null && group1.GroupData == group2.GroupData)
              flag2 = true;
          }
        }
      }
      if (flag2)
      {
        this.m_oreDetectorComponent.Update(this.PositionComp.GetPosition(), this.EntityId, false);
        this.m_oreDetectorComponent.SetRelayedRequest = true;
      }
      else
        this.m_oreDetectorComponent.Clear();
    }

    private bool OnCheckControl()
    {
      MyCubeGrid nodeA = (MyCubeGrid) null;
      if (MySession.Static.ControlledEntity != null)
        nodeA = MySession.Static.ControlledEntity.Entity.Parent as MyCubeGrid;
      return nodeA != null && this.IsWorking & (nodeA == this.CubeGrid || MyCubeGridGroups.Static.Logical.HasSameGroup(nodeA, this.CubeGrid));
    }

    public float Range
    {
      get => (float) ((double) this.m_oreDetectorComponent.DetectionRadius / (double) this.m_definition.MaximumRange * 100.0);
      set => this.m_range.Value = value;
    }

    private void UpdateRange()
    {
      this.m_oreDetectorComponent.DetectionRadius = (float) this.m_range / 100f * this.m_definition.MaximumRange;
      this.RaisePropertiesChanged();
      this.CheckEmissiveState(false);
    }

    public override void CheckEmissiveState(bool force = false)
    {
      base.CheckEmissiveState(force);
      if (this.IsWorking)
      {
        if ((double) this.m_oreDetectorComponent.DetectionRadius < 9.99999974737875E-06)
          this.SetEmissiveStateDisabled();
        else
          this.SetEmissiveStateWorking();
      }
      else if (this.IsFunctional)
        this.SetEmissiveStateDisabled();
      else
        this.SetEmissiveStateDamaged();
    }

    bool IMyComponentOwner<MyOreDetectorComponent>.GetComponent(
      out MyOreDetectorComponent component)
    {
      component = this.m_oreDetectorComponent;
      return this.IsWorking;
    }

    public bool BroadcastUsingAntennas
    {
      get => this.m_oreDetectorComponent.BroadcastUsingAntennas;
      set
      {
        this.m_oreDetectorComponent.BroadcastUsingAntennas = value;
        this.RaisePropertiesChanged();
      }
    }

    bool Sandbox.ModAPI.Ingame.IMyOreDetector.BroadcastUsingAntennas
    {
      get => this.BroadcastUsingAntennas;
      set => this.BroadcastUsingAntennas = value;
    }

    float Sandbox.ModAPI.Ingame.IMyOreDetector.Range => this.Range;

    protected class m_broadcastUsingAntennas\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyOreDetector) obj0).m_broadcastUsingAntennas = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_range\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyOreDetector) obj0).m_range = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Cube_MyOreDetector\u003C\u003EActor : IActivator, IActivator<MyOreDetector>
    {
      object IActivator.CreateInstance() => (object) new MyOreDetector();

      MyOreDetector IActivator<MyOreDetector>.CreateInstance() => new MyOreDetector();
    }
  }
}
