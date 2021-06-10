// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyVirtualMass
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_VirtualMass))]
  [MyTerminalInterface(new System.Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyArtificialMassBlock), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyArtificialMassBlock)})]
  public class MyVirtualMass : MyFunctionalBlock, SpaceEngineers.Game.ModAPI.IMyArtificialMassBlock, SpaceEngineers.Game.ModAPI.IMyVirtualMass, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyVirtualMass, SpaceEngineers.Game.ModAPI.Ingame.IMyArtificialMassBlock
  {
    private MyVirtualMassDefinition BlockDefinition => (MyVirtualMassDefinition) base.BlockDefinition;

    protected override bool CheckIsWorking() => this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) && base.CheckIsWorking();

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      this.ResourceSink = new MyResourceSinkComponent();
      this.ResourceSink.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, new Func<float>(this.CalculateRequiredPowerInput));
      base.Init(objectBuilder, cubeGrid);
      this.ResourceSink.IsPoweredChanged += new Action(this.Receiver_IsPoweredChanged);
      this.ResourceSink.Update();
      if (this.Physics != null)
        this.Physics.Close();
      HkBoxShape hkBoxShape = new HkBoxShape(new Vector3(cubeGrid.GridSize / 3f));
      HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(hkBoxShape.HalfExtents, this.BlockDefinition.VirtualMass);
      this.Physics = new MyPhysicsBody((VRage.ModAPI.IMyEntity) this, RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
      this.Physics.IsPhantom = false;
      this.Physics.CreateFromCollisionObject((HkShape) hkBoxShape, Vector3.Zero, this.WorldMatrix, new HkMassProperties?(volumeMassProperties), 25);
      this.Physics.Enabled = this.IsWorking && cubeGrid.Physics != null && cubeGrid.Physics.Enabled;
      this.Physics.RigidBody.Activate();
      hkBoxShape.Base.RemoveReference();
      this.SetDetailedInfoDirty();
      this.NeedsWorldMatrix = true;
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
    }

    public override void OnBuildSuccess(long builtBy, bool instantBuild)
    {
      this.ResourceSink.Update();
      this.UpdatePhysics();
      this.SetDetailedInfoDirtyDelayed();
      base.OnBuildSuccess(builtBy, instantBuild);
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      if (this.Physics != null)
        this.UpdatePhysics();
      this.SetDetailedInfoDirtyDelayed();
      base.OnEnabledChanged();
    }

    private void SetDetailedInfoDirtyDelayed()
    {
      if (this.Closed || this.MarkedForClose || MySession.Static.IsUnloading)
        return;
      if ((this.CubeGrid.NeedsUpdate & MyEntityUpdateEnum.EACH_FRAME) == MyEntityUpdateEnum.NONE)
        MySandboxGame.Static.Invoke(new Action(this.SetDetailedInfoDirtyDelayed), "Virtual Mass");
      else
        MySandboxGame.Static.Invoke(new Action(this.InvalidateDetailedInfo), "Virtual Mass");
    }

    private void InvalidateDetailedInfo()
    {
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private float CalculateRequiredPowerInput() => this.Enabled && this.IsFunctional ? this.BlockDefinition.RequiredPowerInput : 0.0f;

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      this.ResourceSink.Update();
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentMass));
      detailedInfo.Append(this.IsWorking ? this.BlockDefinition.VirtualMass.ToString() : "0");
      detailedInfo.Append(" kg\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_RequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.CurrentInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
    }

    private void Receiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.UpdatePhysics();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    private void UpdatePhysics()
    {
      if (this.Physics == null)
        return;
      this.Physics.Enabled = this.IsWorking && this.CubeGrid.Physics != null && this.CubeGrid.Physics.Enabled;
      if (!this.IsWorking)
        return;
      this.Physics.RigidBody.Activate();
    }

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    float SpaceEngineers.Game.ModAPI.Ingame.IMyVirtualMass.VirtualMass => this.BlockDefinition.VirtualMass;
  }
}
