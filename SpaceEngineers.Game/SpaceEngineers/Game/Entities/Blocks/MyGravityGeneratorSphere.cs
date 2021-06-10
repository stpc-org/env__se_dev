// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyGravityGeneratorSphere
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using SpaceEngineers.Game.EntityComponents.DebugRenders;
using System;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_GravityGeneratorSphere))]
  [MyTerminalInterface(new System.Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyGravityGeneratorSphere), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorSphere)})]
  public class MyGravityGeneratorSphere : MyGravityGeneratorBase, SpaceEngineers.Game.ModAPI.IMyGravityGeneratorSphere, SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase, IMyGravityProvider, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorSphere
  {
    private const float DEFAULT_RADIUS = 100f;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_radius;
    private float m_defaultVolume;

    private MyGravityGeneratorSphereDefinition BlockDefinition => (MyGravityGeneratorSphereDefinition) ((MyCubeBlock) this).BlockDefinition;

    public float Radius
    {
      get => (float) this.m_radius;
      set => this.m_radius.Value = value;
    }

    public override float GetRadius() => (float) this.m_radius;

    private float MaxInput => (float) Math.Pow((double) this.BlockDefinition.MaxRadius, (double) this.BlockDefinition.ConsumptionPower) / (float) Math.Pow(100.0, (double) this.BlockDefinition.ConsumptionPower) * this.BlockDefinition.BasePowerInput;

    public MyGravityGeneratorSphere()
    {
      this.CreateTerminalControls();
      this.m_radius.ValueChanged += (Action<SyncBase>) (x => this.UpdateFieldShape());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyGravityGeneratorSphere>())
        return;
      base.CreateTerminalControls();
      if (!MyFakes.ENABLE_GRAVITY_GENERATOR_SPHERE)
        return;
      MyTerminalControlSlider<MyGravityGeneratorSphere> slider1 = new MyTerminalControlSlider<MyGravityGeneratorSphere>("Radius", MySpaceTexts.BlockPropertyTitle_GravityFieldRadius, MySpaceTexts.BlockPropertyDescription_GravityFieldRadius);
      slider1.DefaultValue = new float?(100f);
      slider1.Getter = (MyTerminalValueControl<MyGravityGeneratorSphere, float>.GetterDelegate) (x => x.Radius);
      slider1.Setter = (MyTerminalValueControl<MyGravityGeneratorSphere, float>.SetterDelegate) ((x, v) =>
      {
        if ((double) v < (double) x.BlockDefinition.MinRadius)
          v = x.BlockDefinition.MinRadius;
        x.Radius = v;
      });
      slider1.Normalizer = (MyTerminalControlSlider<MyGravityGeneratorSphere>.FloatFunc) ((x, v) => (double) v == 0.0 ? 0.0f : (float) (((double) v - (double) x.BlockDefinition.MinRadius) / ((double) x.BlockDefinition.MaxRadius - (double) x.BlockDefinition.MinRadius)));
      slider1.Denormalizer = (MyTerminalControlSlider<MyGravityGeneratorSphere>.FloatFunc) ((x, v) => (double) v == 0.0 ? 0.0f : v * (x.BlockDefinition.MaxRadius - x.BlockDefinition.MinRadius) + x.BlockDefinition.MinRadius);
      slider1.Writer = (MyTerminalControl<MyGravityGeneratorSphere>.WriterDelegate) ((x, result) => result.AppendInt32((int) (float) x.m_radius).Append(" m"));
      slider1.EnableActions<MyGravityGeneratorSphere>();
      MyTerminalControlFactory.AddControl<MyGravityGeneratorSphere>((MyTerminalControl<MyGravityGeneratorSphere>) slider1);
      MyTerminalControlSlider<MyGravityGeneratorSphere> slider2 = new MyTerminalControlSlider<MyGravityGeneratorSphere>("Gravity", MySpaceTexts.BlockPropertyTitle_GravityAcceleration, MySpaceTexts.BlockPropertyDescription_GravityAcceleration);
      slider2.SetLimits((MyTerminalValueControl<MyGravityGeneratorSphere, float>.GetterDelegate) (x => x.BlockDefinition.MinGravityAcceleration), (MyTerminalValueControl<MyGravityGeneratorSphere, float>.GetterDelegate) (x => x.BlockDefinition.MaxGravityAcceleration));
      slider2.DefaultValue = new float?(9.81f);
      slider2.Getter = (MyTerminalValueControl<MyGravityGeneratorSphere, float>.GetterDelegate) (x => x.GravityAcceleration);
      slider2.Setter = (MyTerminalValueControl<MyGravityGeneratorSphere, float>.SetterDelegate) ((x, v) =>
      {
        if (float.IsNaN(v) || float.IsInfinity(v))
          v = 0.0f;
        x.GravityAcceleration = v;
      });
      slider2.Writer = (MyTerminalControl<MyGravityGeneratorSphere>.WriterDelegate) ((x, result) => result.AppendFormat("{0:F1} m/s\x00B2 ({1:F2} g)", (object) x.GravityAcceleration, (object) (x.GravityAcceleration / 9.81f)));
      slider2.EnableActions<MyGravityGeneratorSphere>();
      MyTerminalControlFactory.AddControl<MyGravityGeneratorSphere>((MyTerminalControl<MyGravityGeneratorSphere>) slider2);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_GravityGeneratorSphere gravityGeneratorSphere = (MyObjectBuilder_GravityGeneratorSphere) objectBuilder;
      this.m_radius.SetLocalValue(MathHelper.Clamp(gravityGeneratorSphere.Radius, this.BlockDefinition.MinRadius, this.BlockDefinition.MaxRadius));
      this.m_gravityAcceleration.SetLocalValue(MathHelper.Clamp(gravityGeneratorSphere.GravityAcceleration, this.BlockDefinition.MinGravityAcceleration, this.BlockDefinition.MaxGravityAcceleration));
      this.m_defaultVolume = (float) (Math.Pow(100.0, (double) this.BlockDefinition.ConsumptionPower) * Math.PI * 0.75);
      if (!this.CubeGrid.CreatePhysics)
        return;
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentGravityGeneratorSphere(this));
    }

    protected override void InitializeSinkComponent()
    {
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.MaxInput, new Func<float>(((MyGravityGeneratorBase) this).CalculateRequiredPowerInput));
      this.ResourceSink = resourceSinkComponent;
      if (!this.CubeGrid.CreatePhysics)
        return;
      this.ResourceSink.IsPoweredChanged += new Action(((MyGravityGeneratorBase) this).Receiver_IsPoweredChanged);
      this.ResourceSink.RequiredInputChanged += new MyRequiredResourceChangeDelegate(((MyGravityGeneratorBase) this).Receiver_RequiredInputChanged);
      this.IsWorkingChanged += new Action<MyCubeBlock>(((MyGravityGeneratorBase) this).OnIsWorkingChanged);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_GravityGeneratorSphere builderCubeBlock = (MyObjectBuilder_GravityGeneratorSphere) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.Radius = (float) this.m_radius;
      builderCubeBlock.GravityAcceleration = (float) this.m_gravityAcceleration;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void UpdateBeforeSimulation()
    {
      if (!MyFakes.ENABLE_GRAVITY_GENERATOR_SPHERE)
        return;
      base.UpdateBeforeSimulation();
    }

    protected override float CalculateRequiredPowerInput() => this.Enabled && this.IsFunctional ? this.CalculateRequiredPowerInputForRadius((float) this.m_radius) : 0.0f;

    private float CalculateRequiredPowerInputForRadius(float radius) => (float) (Math.Pow((double) radius, (double) this.BlockDefinition.ConsumptionPower) * Math.PI * 0.75 / (double) this.m_defaultVolume * (double) this.BlockDefinition.BasePowerInput * ((double) Math.Abs((float) this.m_gravityAcceleration) / 9.8100004196167));

    protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
    {
      base.UpdateDetailedInfo(detailedInfo);
      detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
      detailedInfo.Append(this.BlockDefinition.DisplayNameText);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MaxRequiredInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), detailedInfo);
      detailedInfo.Append("\n");
      detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
      MyValueFormatter.AppendWorkInBestUnit(this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? this.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0.0f, detailedInfo);
    }

    public override bool IsPositionInRange(Vector3D worldPoint) => (this.WorldMatrix.Translation - worldPoint).LengthSquared() < (double) (float) this.m_radius * (double) (float) this.m_radius;

    public override void GetProxyAABB(out BoundingBoxD aabb)
    {
      BoundingSphereD sphere = new BoundingSphereD(this.PositionComp.GetPosition(), (double) (float) this.m_radius);
      BoundingBoxD.CreateFromSphere(ref sphere, out aabb);
    }

    public override Vector3 GetWorldGravity(Vector3D worldPoint)
    {
      Vector3D vector3D = this.WorldMatrix.Translation - worldPoint;
      vector3D.Normalize();
      return (Vector3) vector3D * this.GravityAcceleration;
    }

    protected override HkShape GetHkShape() => (HkShape) new HkSphereShape((float) this.m_radius);

    float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase.Gravity => this.GravityAcceleration;

    float SpaceEngineers.Game.ModAPI.IMyGravityGeneratorSphere.Radius
    {
      get => this.Radius;
      set => this.Radius = MathHelper.Clamp(value, this.BlockDefinition.MinRadius, this.BlockDefinition.MaxRadius);
    }

    float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorSphere.Radius
    {
      get => this.Radius;
      set => this.Radius = MathHelper.Clamp(value, this.BlockDefinition.MinRadius, this.BlockDefinition.MaxRadius);
    }

    protected class m_radius\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyGravityGeneratorSphere) obj0).m_radius = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
