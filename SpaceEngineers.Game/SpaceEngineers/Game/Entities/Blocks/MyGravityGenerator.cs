// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyGravityGenerator
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Havok;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
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
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_GravityGenerator))]
  [MyTerminalInterface(new System.Type[] {typeof (SpaceEngineers.Game.ModAPI.IMyGravityGenerator), typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator)})]
  public class MyGravityGenerator : MyGravityGeneratorBase, SpaceEngineers.Game.ModAPI.IMyGravityGenerator, SpaceEngineers.Game.ModAPI.IMyGravityGeneratorBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase, IMyGravityProvider, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator
  {
    private const int NUM_DECIMALS = 0;
    private BoundingBox m_gizmoBoundingBox;
    private readonly VRage.Sync.Sync<Vector3, SyncDirection.BothWays> m_fieldSize;

    private MyGravityGeneratorDefinition BlockDefinition => (MyGravityGeneratorDefinition) ((MyCubeBlock) this).BlockDefinition;

    public Vector3 FieldSize
    {
      get => (Vector3) this.m_fieldSize;
      set
      {
        if (!(this.m_fieldSize.Value != value))
          return;
        Vector3 vector3 = value;
        vector3.X = MathHelper.Clamp(vector3.X, this.BlockDefinition.MinFieldSize.X, this.BlockDefinition.MaxFieldSize.X);
        vector3.Y = MathHelper.Clamp(vector3.Y, this.BlockDefinition.MinFieldSize.Y, this.BlockDefinition.MaxFieldSize.Y);
        vector3.Z = MathHelper.Clamp(vector3.Z, this.BlockDefinition.MinFieldSize.Z, this.BlockDefinition.MaxFieldSize.Z);
        this.m_fieldSize.Value = vector3;
      }
    }

    public override BoundingBox? GetBoundingBox()
    {
      this.m_gizmoBoundingBox.Min = this.PositionComp.LocalVolume.Center - this.FieldSize / 2f;
      this.m_gizmoBoundingBox.Max = this.PositionComp.LocalVolume.Center + this.FieldSize / 2f;
      return new BoundingBox?(this.m_gizmoBoundingBox);
    }

    public MyGravityGenerator()
    {
      this.CreateTerminalControls();
      this.m_fieldSize.ValueChanged += (Action<SyncBase>) (x => this.UpdateFieldShape());
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyGravityGenerator>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlSlider<MyGravityGenerator> slider1 = new MyTerminalControlSlider<MyGravityGenerator>("Width", MySpaceTexts.BlockPropertyTitle_GravityFieldWidth, MySpaceTexts.BlockPropertyDescription_GravityFieldWidth);
      slider1.SetLimits((MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.BlockDefinition.MinFieldSize.X), (MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.BlockDefinition.MaxFieldSize.X));
      slider1.DefaultValue = new float?(150f);
      slider1.Getter = (MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.m_fieldSize.Value.X);
      slider1.Setter = (MyTerminalValueControl<MyGravityGenerator, float>.SetterDelegate) ((x, v) =>
      {
        Vector3 fieldSize = (Vector3) x.m_fieldSize;
        fieldSize.X = v;
        x.m_fieldSize.Value = fieldSize;
      });
      slider1.Writer = (MyTerminalControl<MyGravityGenerator>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.m_fieldSize.Value.X, 0)).Append(" m"));
      slider1.EnableActions<MyGravityGenerator>();
      MyTerminalControlFactory.AddControl<MyGravityGenerator>((MyTerminalControl<MyGravityGenerator>) slider1);
      MyTerminalControlSlider<MyGravityGenerator> slider2 = new MyTerminalControlSlider<MyGravityGenerator>("Height", MySpaceTexts.BlockPropertyTitle_GravityFieldHeight, MySpaceTexts.BlockPropertyDescription_GravityFieldHeight);
      slider2.SetLimits((MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.BlockDefinition.MinFieldSize.Y), (MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.BlockDefinition.MaxFieldSize.Y));
      slider2.DefaultValue = new float?(150f);
      slider2.Getter = (MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.m_fieldSize.Value.Y);
      slider2.Setter = (MyTerminalValueControl<MyGravityGenerator, float>.SetterDelegate) ((x, v) =>
      {
        Vector3 fieldSize = (Vector3) x.m_fieldSize;
        fieldSize.Y = v;
        x.m_fieldSize.Value = fieldSize;
      });
      slider2.Writer = (MyTerminalControl<MyGravityGenerator>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.m_fieldSize.Value.Y, 0)).Append(" m"));
      slider2.EnableActions<MyGravityGenerator>();
      MyTerminalControlFactory.AddControl<MyGravityGenerator>((MyTerminalControl<MyGravityGenerator>) slider2);
      MyTerminalControlSlider<MyGravityGenerator> slider3 = new MyTerminalControlSlider<MyGravityGenerator>("Depth", MySpaceTexts.BlockPropertyTitle_GravityFieldDepth, MySpaceTexts.BlockPropertyDescription_GravityFieldDepth);
      slider3.SetLimits((MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.BlockDefinition.MinFieldSize.Z), (MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.BlockDefinition.MaxFieldSize.Z));
      slider3.DefaultValue = new float?(150f);
      slider3.Getter = (MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.m_fieldSize.Value.Z);
      slider3.Setter = (MyTerminalValueControl<MyGravityGenerator, float>.SetterDelegate) ((x, v) =>
      {
        Vector3 fieldSize = (Vector3) x.m_fieldSize;
        fieldSize.Z = v;
        x.m_fieldSize.Value = fieldSize;
      });
      slider3.Writer = (MyTerminalControl<MyGravityGenerator>.WriterDelegate) ((x, result) => result.Append(MyValueFormatter.GetFormatedFloat(x.m_fieldSize.Value.Z, 0)).Append(" m"));
      slider3.EnableActions<MyGravityGenerator>();
      MyTerminalControlFactory.AddControl<MyGravityGenerator>((MyTerminalControl<MyGravityGenerator>) slider3);
      MyTerminalControlSlider<MyGravityGenerator> slider4 = new MyTerminalControlSlider<MyGravityGenerator>("Gravity", MySpaceTexts.BlockPropertyTitle_GravityAcceleration, MySpaceTexts.BlockPropertyDescription_GravityAcceleration);
      slider4.SetLimits((MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.BlockDefinition.MinGravityAcceleration), (MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.BlockDefinition.MaxGravityAcceleration));
      slider4.DefaultValue = new float?(9.81f);
      slider4.Getter = (MyTerminalValueControl<MyGravityGenerator, float>.GetterDelegate) (x => x.GravityAcceleration);
      slider4.Setter = (MyTerminalValueControl<MyGravityGenerator, float>.SetterDelegate) ((x, v) =>
      {
        if (float.IsNaN(v) || float.IsInfinity(v))
          v = 0.0f;
        x.GravityAcceleration = v;
      });
      slider4.Writer = (MyTerminalControl<MyGravityGenerator>.WriterDelegate) ((x, result) => result.AppendFormat("{0:F1} m/s\x00B2 ({1:F2} g)", (object) x.GravityAcceleration, (object) (x.GravityAcceleration / 9.81f)));
      slider4.EnableActions<MyGravityGenerator>();
      MyTerminalControlFactory.AddControl<MyGravityGenerator>((MyTerminalControl<MyGravityGenerator>) slider4);
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_GravityGenerator gravityGenerator = (MyObjectBuilder_GravityGenerator) objectBuilder;
      this.FieldSize = (Vector3) gravityGenerator.FieldSize;
      this.m_fieldSize.SetLocalValue(this.FieldSize);
      this.m_gravityAcceleration.SetLocalValue(MathHelper.Clamp(gravityGenerator.GravityAcceleration, this.BlockDefinition.MinGravityAcceleration, this.BlockDefinition.MaxGravityAcceleration));
      if (!(this.BlockDefinition.EmissiveColorPreset == MyStringHash.NullOrEmpty))
        return;
      this.BlockDefinition.EmissiveColorPreset = MyStringHash.GetOrCompute("GravityBlock");
    }

    protected override void InitializeSinkComponent()
    {
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(this.BlockDefinition.ResourceSinkGroup, this.BlockDefinition.RequiredPowerInput, new Func<float>(((MyGravityGeneratorBase) this).CalculateRequiredPowerInput));
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
      MyObjectBuilder_GravityGenerator builderCubeBlock = (MyObjectBuilder_GravityGenerator) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.FieldSize = (SerializableVector3) this.m_fieldSize.Value;
      builderCubeBlock.GravityAcceleration = (float) this.m_gravityAcceleration;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    protected override float CalculateRequiredPowerInput() => this.Enabled && this.IsFunctional ? (float) (0.000300000014249235 * (double) Math.Abs((float) this.m_gravityAcceleration) * Math.Pow((double) this.m_fieldSize.Value.Volume, 0.35)) : 0.0f;

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

    public override bool IsPositionInRange(Vector3D worldPoint)
    {
      Vector3 vector3 = this.m_fieldSize.Value * 0.5f;
      MyOrientedBoundingBoxD orientedBoundingBoxD;
      ref MyOrientedBoundingBoxD local = ref orientedBoundingBoxD;
      MatrixD matrix = this.WorldMatrix;
      Vector3D translation = matrix.Translation;
      Vector3D halfExtents = (Vector3D) vector3;
      matrix = this.WorldMatrix;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
      local = new MyOrientedBoundingBoxD(translation, halfExtents, fromRotationMatrix);
      return orientedBoundingBoxD.Contains(ref worldPoint);
    }

    public override void GetProxyAABB(out BoundingBoxD aabb)
    {
      Vector3 vector3 = this.m_fieldSize.Value * 0.5f;
      MyOrientedBoundingBoxD orientedBoundingBoxD;
      ref MyOrientedBoundingBoxD local = ref orientedBoundingBoxD;
      MatrixD matrix = this.WorldMatrix;
      Vector3D translation = matrix.Translation;
      Vector3D halfExtents = (Vector3D) vector3;
      matrix = this.WorldMatrix;
      Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix);
      local = new MyOrientedBoundingBoxD(translation, halfExtents, fromRotationMatrix);
      aabb = orientedBoundingBoxD.GetAABB();
    }

    public override Vector3 GetWorldGravity(Vector3D worldPoint) => Vector3.TransformNormal(Vector3.Down * this.GravityAcceleration, this.WorldMatrix);

    protected override HkShape GetHkShape() => (HkShape) new HkBoxShape(this.m_fieldSize.Value * 0.5f);

    Vector3 SpaceEngineers.Game.ModAPI.IMyGravityGenerator.FieldSize
    {
      get => this.FieldSize;
      set => this.FieldSize = value;
    }

    float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator.FieldWidth => this.m_fieldSize.Value.X;

    float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator.FieldHeight => this.m_fieldSize.Value.Y;

    float SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator.FieldDepth => this.m_fieldSize.Value.Z;

    Vector3 SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator.FieldSize
    {
      get => this.FieldSize;
      set => this.FieldSize = value;
    }

    protected class m_fieldSize\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<Vector3, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<Vector3, SyncDirection.BothWays>(obj1, obj2));
        ((MyGravityGenerator) obj0).m_fieldSize = (VRage.Sync.Sync<Vector3, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
