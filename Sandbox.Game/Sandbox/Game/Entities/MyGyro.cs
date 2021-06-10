// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyGyro
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Gyro))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyGyro), typeof (Sandbox.ModAPI.Ingame.IMyGyro)})]
  public class MyGyro : MyFunctionalBlock, Sandbox.ModAPI.IMyGyro, Sandbox.ModAPI.Ingame.IMyGyro, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
  {
    private MyGyroDefinition m_gyroDefinition;
    private int m_oldEmissiveState = -1;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_gyroPower;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_gyroOverride;
    private VRage.Sync.Sync<Vector3, SyncDirection.BothWays> m_gyroOverrideVelocity;
    private float m_gyroMultiplier = 1f;
    private float m_powerConsumptionMultiplier = 1f;

    public bool IsPowered => this.CubeGrid.GridSystems.GyroSystem.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    protected override bool CheckIsWorking() => this.IsPowered && base.CheckIsWorking();

    public float MaxGyroForce => this.m_gyroDefinition.ForceMagnitude * (float) this.m_gyroPower * this.m_gyroMultiplier;

    public float RequiredPowerInput => this.m_gyroDefinition.RequiredPowerInput * (float) this.m_gyroPower * this.m_powerConsumptionMultiplier;

    public float GyroPower
    {
      get => (float) this.m_gyroPower;
      set
      {
        value = MathHelper.Clamp(value, 0.0f, 1f);
        if ((double) value == (double) (float) this.m_gyroPower)
          return;
        this.m_gyroPower.Value = value;
      }
    }

    public bool GyroOverride
    {
      get => (bool) this.m_gyroOverride;
      set => this.m_gyroOverride.Value = value;
    }

    public Vector3 GyroOverrideVelocityGrid => Vector3.TransformNormal((Vector3) this.m_gyroOverrideVelocity, this.Orientation);

    private static float MaxAngularRadiansPerSecond(MyGyro gyro) => gyro.m_gyroDefinition.CubeSize == MyCubeSize.Small ? MyGridPhysics.GetSmallShipMaxAngularVelocity() : MyGridPhysics.GetLargeShipMaxAngularVelocity();

    private static float MaxAngularRPM(MyGyro gyro) => gyro.m_gyroDefinition.CubeSize == MyCubeSize.Small ? MyGridPhysics.GetSmallShipMaxAngularVelocity() * 9.549296f : MyGridPhysics.GetLargeShipMaxAngularVelocity() * 9.549296f;

    public MyGyro()
    {
      this.CreateTerminalControls();
      this.m_gyroPower.ValueChanged += (Action<SyncBase>) (x => this.GyroPowerChanged());
      this.m_gyroOverride.ValueChanged += (Action<SyncBase>) (x => this.GyroOverrideChanged());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyGyro>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSlider<MyGyro> slider1 = new MyTerminalControlSlider<MyGyro>("Power", MySpaceTexts.BlockPropertyTitle_GyroPower, MySpaceTexts.BlockPropertyDescription_GyroPower);
      slider1.Getter = (MyTerminalValueControl<MyGyro, float>.GetterDelegate) (x => x.GyroPower);
      slider1.Setter = (MyTerminalValueControl<MyGyro, float>.SetterDelegate) ((x, v) => x.GyroPower = v);
      slider1.Writer = (MyTerminalControl<MyGyro>.WriterDelegate) ((x, result) => result.AppendInt32((int) ((double) x.GyroPower * 100.0)).Append(" %"));
      slider1.DefaultValue = new float?(1f);
      slider1.EnableActions<MyGyro>(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE);
      MyTerminalControlFactory.AddControl<MyGyro>((MyTerminalControl<MyGyro>) slider1);
      if (!MyFakes.ENABLE_GYRO_OVERRIDE)
        return;
      MyTerminalControlCheckbox<MyGyro> checkbox = new MyTerminalControlCheckbox<MyGyro>("Override", MySpaceTexts.BlockPropertyTitle_GyroOverride, MySpaceTexts.BlockPropertyDescription_GyroOverride);
      checkbox.Getter = (MyTerminalValueControl<MyGyro, bool>.GetterDelegate) (x => x.GyroOverride);
      checkbox.Setter = (MyTerminalValueControl<MyGyro, bool>.SetterDelegate) ((x, v) => x.GyroOverride = v);
      checkbox.EnableAction<MyGyro>();
      MyTerminalControlFactory.AddControl<MyGyro>((MyTerminalControl<MyGyro>) checkbox);
      MyTerminalControlSlider<MyGyro> slider2 = new MyTerminalControlSlider<MyGyro>("Yaw", MySpaceTexts.BlockPropertyTitle_GyroYawOverride, MySpaceTexts.BlockPropertyDescription_GyroYawOverride);
      slider2.Getter = (MyTerminalValueControl<MyGyro, float>.GetterDelegate) (x => (float) (-(double) x.m_gyroOverrideVelocity.Value.Y * 9.54929637908936));
      slider2.Setter = (MyTerminalValueControl<MyGyro, float>.SetterDelegate) ((x, v) => MyGyro.SetGyroTorqueYaw(x, (float) (-(double) v * 0.104719758033752)));
      slider2.Writer = (MyTerminalControl<MyGyro>.WriterDelegate) ((x, result) => result.AppendDecimal((float) (-(double) x.m_gyroOverrideVelocity.Value.Y * 9.54929637908936), 2).Append(" RPM"));
      slider2.Enabled = (Func<MyGyro, bool>) (x => x.GyroOverride);
      slider2.DefaultValue = new float?(0.0f);
      slider2.SetDualLogLimits((MyTerminalValueControl<MyGyro, float>.GetterDelegate) (x => 0.01f), new MyTerminalValueControl<MyGyro, float>.GetterDelegate(MyGyro.MaxAngularRPM), 0.05f);
      slider2.EnableActions<MyGyro>(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE);
      MyTerminalControlFactory.AddControl<MyGyro>((MyTerminalControl<MyGyro>) slider2);
      MyTerminalControlSlider<MyGyro> slider3 = new MyTerminalControlSlider<MyGyro>("Pitch", MySpaceTexts.BlockPropertyTitle_GyroPitchOverride, MySpaceTexts.BlockPropertyDescription_GyroPitchOverride);
      slider3.Getter = (MyTerminalValueControl<MyGyro, float>.GetterDelegate) (x => x.m_gyroOverrideVelocity.Value.X * 9.549296f);
      slider3.Setter = (MyTerminalValueControl<MyGyro, float>.SetterDelegate) ((x, v) => MyGyro.SetGyroTorquePitch(x, v * ((float) Math.PI / 30f)));
      slider3.Writer = (MyTerminalControl<MyGyro>.WriterDelegate) ((x, result) => result.AppendDecimal(x.m_gyroOverrideVelocity.Value.X * 9.549296f, 2).Append(" RPM"));
      slider3.Enabled = (Func<MyGyro, bool>) (x => x.GyroOverride);
      slider3.DefaultValue = new float?(0.0f);
      slider3.SetDualLogLimits((MyTerminalValueControl<MyGyro, float>.GetterDelegate) (x => 0.01f), new MyTerminalValueControl<MyGyro, float>.GetterDelegate(MyGyro.MaxAngularRPM), 0.05f);
      slider3.EnableActions<MyGyro>(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE);
      MyTerminalControlFactory.AddControl<MyGyro>((MyTerminalControl<MyGyro>) slider3);
      MyTerminalControlSlider<MyGyro> slider4 = new MyTerminalControlSlider<MyGyro>("Roll", MySpaceTexts.BlockPropertyTitle_GyroRollOverride, MySpaceTexts.BlockPropertyDescription_GyroRollOverride);
      slider4.Getter = (MyTerminalValueControl<MyGyro, float>.GetterDelegate) (x => (float) (-(double) x.m_gyroOverrideVelocity.Value.Z * 9.54929637908936));
      slider4.Setter = (MyTerminalValueControl<MyGyro, float>.SetterDelegate) ((x, v) => MyGyro.SetGyroTorqueRoll(x, (float) (-(double) v * 0.104719758033752)));
      slider4.Writer = (MyTerminalControl<MyGyro>.WriterDelegate) ((x, result) => result.AppendDecimal((float) (-(double) x.m_gyroOverrideVelocity.Value.Z * 9.54929637908936), 2).Append(" RPM"));
      slider4.Enabled = (Func<MyGyro, bool>) (x => x.GyroOverride);
      slider4.DefaultValue = new float?(0.0f);
      slider4.SetDualLogLimits((MyTerminalValueControl<MyGyro, float>.GetterDelegate) (x => 0.01f), new MyTerminalValueControl<MyGyro, float>.GetterDelegate(MyGyro.MaxAngularRPM), 0.05f);
      slider4.EnableActions<MyGyro>(MyTerminalActionIcons.INCREASE, MyTerminalActionIcons.DECREASE);
      MyTerminalControlFactory.AddControl<MyGyro>((MyTerminalControl<MyGyro>) slider4);
    }

    private void GyroOverrideChanged() => this.SetGyroOverride(this.m_gyroOverride.Value);

    private void GyroPowerChanged()
    {
      this.SetEmissiveStateWorking();
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.m_gyroDefinition = (MyGyroDefinition) this.BlockDefinition;
      MyObjectBuilder_Gyro objectBuilderGyro = objectBuilder as MyObjectBuilder_Gyro;
      this.m_gyroPower.SetLocalValue(MathHelper.Clamp(objectBuilderGyro.GyroPower, 0.0f, 1f));
      if (MyFakes.ENABLE_GYRO_OVERRIDE)
      {
        this.GyroOverride = objectBuilderGyro.GyroOverride;
        float max = MyGyro.MaxAngularRadiansPerSecond(this);
        this.SetGyroTorque((Vector3) new SerializableVector3(MathHelper.Clamp(objectBuilderGyro.TargetAngularVelocity.x, -max, max), MathHelper.Clamp(objectBuilderGyro.TargetAngularVelocity.y, -max, max), MathHelper.Clamp(objectBuilderGyro.TargetAngularVelocity.z, -max, max)));
      }
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      this.SetDetailedInfoDirty();
      this.RaisePropertiesChanged();
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Gyro builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Gyro;
      builderCubeBlock.GyroPower = (float) this.m_gyroPower;
      builderCubeBlock.GyroOverride = this.GyroOverride;
      builderCubeBlock.TargetAngularVelocity = (SerializableVector3) this.m_gyroOverrideVelocity.Value;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void OnModelChange()
    {
      this.m_oldEmissiveState = -1;
      base.OnModelChange();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.IsWorking)
        this.OnStartWorking();
      this.m_oldEmissiveState = -1;
    }

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.RequiredPowerInput, detailedInfo);
    }

    public override bool SetEmissiveStateWorking()
    {
      if (this.GyroOverride)
        return this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Alternative, this.Render.RenderObjectIDs[0]);
      return (double) this.GyroPower <= 9.99999974737875E-06 ? this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Disabled, this.Render.RenderObjectIDs[0]) : this.SetEmissiveState(MyCubeBlock.m_emissiveNames.Working, this.Render.RenderObjectIDs[0]);
    }

    public void SetGyroOverride(bool value)
    {
      this.CubeGrid.GridSystems.GyroSystem.MarkDirty();
      if (!this.CheckIsWorking())
        return;
      this.SetEmissiveStateWorking();
    }

    private static void SetGyroTorqueYaw(MyGyro gyro, float yawValue)
    {
      Vector3 torque = gyro.m_gyroOverrideVelocity.Value;
      torque.Y = yawValue;
      gyro.SetGyroTorque(torque);
    }

    private static void SetGyroTorquePitch(MyGyro gyro, float pitchValue)
    {
      Vector3 torque = gyro.m_gyroOverrideVelocity.Value;
      torque.X = pitchValue;
      gyro.SetGyroTorque(torque);
    }

    private static void SetGyroTorqueRoll(MyGyro gyro, float rollValue)
    {
      Vector3 torque = gyro.m_gyroOverrideVelocity.Value;
      torque.Z = rollValue;
      gyro.SetGyroTorque(torque);
    }

    public void SetGyroTorque(Vector3 torque)
    {
      if (!torque.IsValid())
        return;
      this.m_gyroOverrideVelocity.Value = torque;
      this.CubeGrid.GridSystems.GyroSystem.MarkDirty();
    }

    float Sandbox.ModAPI.Ingame.IMyGyro.Yaw
    {
      get => -this.m_gyroOverrideVelocity.Value.Y;
      set
      {
        if (!this.GyroOverride)
          return;
        float max = MyGyro.MaxAngularRadiansPerSecond(this);
        value = MathHelper.Clamp(value, -max, max);
        MyGyro.SetGyroTorqueYaw(this, -value);
      }
    }

    float Sandbox.ModAPI.Ingame.IMyGyro.Pitch
    {
      get => -this.m_gyroOverrideVelocity.Value.X;
      set
      {
        if (!this.GyroOverride)
          return;
        float max = MyGyro.MaxAngularRadiansPerSecond(this);
        value = MathHelper.Clamp(value, -max, max);
        MyGyro.SetGyroTorquePitch(this, -value);
      }
    }

    float Sandbox.ModAPI.Ingame.IMyGyro.Roll
    {
      get => -this.m_gyroOverrideVelocity.Value.Z;
      set
      {
        if (!this.GyroOverride)
          return;
        float max = MyGyro.MaxAngularRadiansPerSecond(this);
        value = MathHelper.Clamp(value, -max, max);
        MyGyro.SetGyroTorqueRoll(this, -value);
      }
    }

    float Sandbox.ModAPI.IMyGyro.GyroStrengthMultiplier
    {
      get => this.m_gyroMultiplier;
      set
      {
        this.m_gyroMultiplier = value;
        if ((double) this.m_gyroMultiplier < 0.00999999977648258)
          this.m_gyroMultiplier = 0.01f;
        if (this.CubeGrid.GridSystems.GyroSystem == null)
          return;
        this.CubeGrid.GridSystems.GyroSystem.MarkDirty();
      }
    }

    float Sandbox.ModAPI.IMyGyro.PowerConsumptionMultiplier
    {
      get => this.m_powerConsumptionMultiplier;
      set
      {
        this.m_powerConsumptionMultiplier = value;
        if ((double) this.m_powerConsumptionMultiplier < 0.00999999977648258)
          this.m_powerConsumptionMultiplier = 0.01f;
        if (this.CubeGrid.GridSystems.GyroSystem != null)
          this.CubeGrid.GridSystems.GyroSystem.MarkDirty();
        this.SetDetailedInfoDirty();
        this.RaisePropertiesChanged();
      }
    }

    protected class m_gyroPower\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyGyro) obj0).m_gyroPower = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_gyroOverride\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyGyro) obj0).m_gyroOverride = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_gyroOverrideVelocity\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<Vector3, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<Vector3, SyncDirection.BothWays>(obj1, obj2));
        ((MyGyro) obj0).m_gyroOverrideVelocity = (VRage.Sync.Sync<Vector3, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_MyGyro\u003C\u003EActor : IActivator, IActivator<MyGyro>
    {
      object IActivator.CreateInstance() => (object) new MyGyro();

      MyGyro IActivator<MyGyro>.CreateInstance() => new MyGyro();
    }
  }
}
