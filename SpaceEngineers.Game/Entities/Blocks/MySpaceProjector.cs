// Decompiled with JetBrains decompiler
// Type: Entities.Blocks.MySpaceProjector
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.ModAPI;
using System;
using System.Text;
using VRage;

namespace Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Projector))]
  [MyTerminalInterface(new Type[] {typeof (Sandbox.ModAPI.IMyProjector), typeof (Sandbox.ModAPI.Ingame.IMyProjector)})]
  public class MySpaceProjector : MyProjectorBase
  {
    private const float ROTATION_LIMIT = 180f;

    public MySpaceProjector() => this.CreateTerminalControls();

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySpaceProjector>())
        return;
      base.CreateTerminalControls();
      if (!MyFakes.ENABLE_PROJECTOR_BLOCK)
        return;
      MyTerminalControlButton<MySpaceProjector> terminalControlButton1 = new MyTerminalControlButton<MySpaceProjector>("Blueprint", MyCommonTexts.Blueprints, MySpaceTexts.Blank, (Action<MySpaceProjector>) (p => p.SelectBlueprint()));
      terminalControlButton1.Enabled = (Func<MySpaceProjector, bool>) (b => b.CanProject());
      terminalControlButton1.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) terminalControlButton1);
      MyTerminalControlButton<MySpaceProjector> terminalControlButton2 = new MyTerminalControlButton<MySpaceProjector>("Remove", MySpaceTexts.RemoveProjectionButton, MySpaceTexts.Blank, (Action<MySpaceProjector>) (p => p.SendRemoveProjection()));
      terminalControlButton2.Enabled = (Func<MySpaceProjector, bool>) (b => b.IsProjecting());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) terminalControlButton2);
      MyTerminalControlCheckbox<MySpaceProjector> checkbox = new MyTerminalControlCheckbox<MySpaceProjector>("KeepProjection", MySpaceTexts.KeepProjectionToggle, MySpaceTexts.KeepProjectionTooltip);
      checkbox.Getter = (MyTerminalValueControl<MySpaceProjector, bool>.GetterDelegate) (x => x.KeepProjection);
      checkbox.Setter = (MyTerminalValueControl<MySpaceProjector, bool>.SetterDelegate) ((x, v) => x.KeepProjection = v);
      checkbox.EnableAction<MySpaceProjector>();
      checkbox.Enabled = (Func<MySpaceProjector, bool>) (b => b.IsProjecting() && b.AllowWelding);
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) checkbox);
      MyTerminalControlCheckbox<MySpaceProjector> terminalControlCheckbox1 = new MyTerminalControlCheckbox<MySpaceProjector>("ShowOnlyBuildable", MySpaceTexts.ShowOnlyBuildableBlockToggle, MySpaceTexts.ShowOnlyBuildableTooltip, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true, maxWidth: 0.2f);
      terminalControlCheckbox1.Getter = (MyTerminalValueControl<MySpaceProjector, bool>.GetterDelegate) (x => x.m_showOnlyBuildable);
      terminalControlCheckbox1.Setter = (MyTerminalValueControl<MySpaceProjector, bool>.SetterDelegate) ((x, v) =>
      {
        x.m_showOnlyBuildable = v;
        x.OnOffsetsChanged();
      });
      terminalControlCheckbox1.Enabled = (Func<MySpaceProjector, bool>) (b => b.IsProjecting() && b.AllowWelding);
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) terminalControlCheckbox1);
      MyTerminalControlSlider<MySpaceProjector> slider1 = new MyTerminalControlSlider<MySpaceProjector>("X", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetX, MySpaceTexts.Blank);
      slider1.SetLimits(-50f, 50f);
      slider1.DefaultValue = new float?(0.0f);
      slider1.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (x => (float) x.m_projectionOffset.X);
      slider1.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((x, v) =>
      {
        x.m_projectionOffset.X = Convert.ToInt32(v);
        x.OnOffsetsChanged();
      });
      slider1.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((x, result) => result.AppendInt32(x.m_projectionOffset.X));
      slider1.EnableActions<MySpaceProjector>(0.01f);
      slider1.Enabled = (Func<MySpaceProjector, bool>) (x => x.IsProjecting());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) slider1);
      MyTerminalControlSlider<MySpaceProjector> slider2 = new MyTerminalControlSlider<MySpaceProjector>("Y", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetY, MySpaceTexts.Blank);
      slider2.SetLimits(-50f, 50f);
      slider2.DefaultValue = new float?(0.0f);
      slider2.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (x => (float) x.m_projectionOffset.Y);
      slider2.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((x, v) =>
      {
        x.m_projectionOffset.Y = Convert.ToInt32(v);
        x.OnOffsetsChanged();
      });
      slider2.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((x, result) => result.AppendInt32(x.m_projectionOffset.Y));
      slider2.EnableActions<MySpaceProjector>(0.01f);
      slider2.Enabled = (Func<MySpaceProjector, bool>) (x => x.IsProjecting());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) slider2);
      MyTerminalControlSlider<MySpaceProjector> slider3 = new MyTerminalControlSlider<MySpaceProjector>("Z", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetZ, MySpaceTexts.Blank);
      slider3.SetLimits(-50f, 50f);
      slider3.DefaultValue = new float?(0.0f);
      slider3.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (x => (float) x.m_projectionOffset.Z);
      slider3.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((x, v) =>
      {
        x.m_projectionOffset.Z = Convert.ToInt32(v);
        x.OnOffsetsChanged();
      });
      slider3.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((x, result) => result.AppendInt32(x.m_projectionOffset.Z));
      slider3.EnableActions<MySpaceProjector>(0.01f);
      slider3.Enabled = (Func<MySpaceProjector, bool>) (x => x.IsProjecting());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) slider3);
      MyTerminalControlSlider<MySpaceProjector> slider4 = new MyTerminalControlSlider<MySpaceProjector>("RotX", MySpaceTexts.BlockPropertyTitle_ProjectionRotationX, MySpaceTexts.Blank);
      slider4.SetLimits(-180f, 180f);
      slider4.DefaultValue = new float?(0.0f);
      slider4.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (x => (float) (x.m_projectionRotation.X * x.BlockDefinition.RotationAngleStepDeg));
      slider4.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((x, v) =>
      {
        x.m_projectionRotation.X = Convert.ToInt32(v / (float) x.BlockDefinition.RotationAngleStepDeg);
        x.OnOffsetsChanged();
      });
      slider4.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((x, result) => result.AppendInt32(x.m_projectionRotation.X * x.BlockDefinition.RotationAngleStepDeg).Append("°"));
      slider4.EnableActions<MySpaceProjector>(0.25f);
      slider4.Enabled = (Func<MySpaceProjector, bool>) (x => x.IsProjecting());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) slider4);
      MyTerminalControlSlider<MySpaceProjector> slider5 = new MyTerminalControlSlider<MySpaceProjector>("RotY", MySpaceTexts.BlockPropertyTitle_ProjectionRotationY, MySpaceTexts.Blank);
      slider5.SetLimits(-180f, 180f);
      slider5.DefaultValue = new float?(0.0f);
      slider5.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (x => (float) (x.m_projectionRotation.Y * x.BlockDefinition.RotationAngleStepDeg));
      slider5.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((x, v) =>
      {
        x.m_projectionRotation.Y = Convert.ToInt32(v / (float) x.BlockDefinition.RotationAngleStepDeg);
        x.OnOffsetsChanged();
      });
      slider5.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((x, result) => result.AppendInt32(x.m_projectionRotation.Y * x.BlockDefinition.RotationAngleStepDeg).Append("°"));
      slider5.EnableActions<MySpaceProjector>(0.25f);
      slider5.Enabled = (Func<MySpaceProjector, bool>) (x => x.IsProjecting());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) slider5);
      MyTerminalControlSlider<MySpaceProjector> slider6 = new MyTerminalControlSlider<MySpaceProjector>("RotZ", MySpaceTexts.BlockPropertyTitle_ProjectionRotationZ, MySpaceTexts.Blank);
      slider6.SetLimits(-180f, 180f);
      slider6.DefaultValue = new float?(0.0f);
      slider6.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (x => (float) (x.m_projectionRotation.Z * x.BlockDefinition.RotationAngleStepDeg));
      slider6.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((x, v) =>
      {
        x.m_projectionRotation.Z = Convert.ToInt32(v / (float) x.BlockDefinition.RotationAngleStepDeg);
        x.OnOffsetsChanged();
      });
      slider6.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((x, result) => result.AppendInt32(x.m_projectionRotation.Z * x.BlockDefinition.RotationAngleStepDeg).Append("°"));
      slider6.EnableActions<MySpaceProjector>(0.25f);
      slider6.Enabled = (Func<MySpaceProjector, bool>) (x => x.IsProjecting());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) slider6);
      MyTerminalControlSlider<MySpaceProjector> slider7 = new MyTerminalControlSlider<MySpaceProjector>("Scale", MySpaceTexts.BlockPropertyTitle_Scale, MySpaceTexts.Blank);
      slider7.SetLimits(0.02f, 1f);
      slider7.DefaultValue = new float?(1f);
      slider7.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (x => x.m_projectionScale);
      slider7.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((x, v) =>
      {
        x.m_projectionScale = v;
        x.OnOffsetsChanged();
      });
      slider7.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((x, result) => result.AppendInt32((int) ((double) x.m_projectionScale * 100.0)).Append("%"));
      slider7.EnableActions<MySpaceProjector>(0.01f);
      slider7.Enabled = (Func<MySpaceProjector, bool>) (x => x.IsProjecting() && x.AllowScaling);
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) slider7);
      MyTerminalControlSeparator<MySpaceProjector> controlSeparator = new MyTerminalControlSeparator<MySpaceProjector>();
      controlSeparator.Visible = (Func<MySpaceProjector, bool>) (p => p.ScenarioSettingsEnabled());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) controlSeparator);
      MyTerminalControlLabel<MySpaceProjector> terminalControlLabel = new MyTerminalControlLabel<MySpaceProjector>(MySpaceTexts.TerminalScenarioSettingsLabel);
      terminalControlLabel.Visible = (Func<MySpaceProjector, bool>) (p => p.ScenarioSettingsEnabled());
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) terminalControlLabel);
      MyTerminalControlButton<MySpaceProjector> button = new MyTerminalControlButton<MySpaceProjector>("SpawnProjection", MySpaceTexts.BlockPropertyTitle_ProjectionSpawn, MySpaceTexts.Blank, (Action<MySpaceProjector>) (p => p.TrySpawnProjection()));
      button.Visible = (Func<MySpaceProjector, bool>) (p => p.ScenarioSettingsEnabled());
      button.Enabled = (Func<MySpaceProjector, bool>) (p => p.CanSpawnProjection());
      button.EnableAction<MySpaceProjector>();
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) button);
      MyTerminalControlCheckbox<MySpaceProjector> terminalControlCheckbox2 = new MyTerminalControlCheckbox<MySpaceProjector>("InstantBuilding", MySpaceTexts.BlockPropertyTitle_Projector_InstantBuilding, MySpaceTexts.BlockPropertyTitle_Projector_InstantBuilding_Tooltip);
      terminalControlCheckbox2.Visible = (Func<MySpaceProjector, bool>) (p => p.ScenarioSettingsEnabled());
      terminalControlCheckbox2.Enabled = (Func<MySpaceProjector, bool>) (p => p.CanEnableInstantBuilding());
      terminalControlCheckbox2.Getter = (MyTerminalValueControl<MySpaceProjector, bool>.GetterDelegate) (p => p.InstantBuildingEnabled);
      terminalControlCheckbox2.Setter = (MyTerminalValueControl<MySpaceProjector, bool>.SetterDelegate) ((p, v) => p.TrySetInstantBuilding(v));
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) terminalControlCheckbox2);
      MyTerminalControlCheckbox<MySpaceProjector> terminalControlCheckbox3 = new MyTerminalControlCheckbox<MySpaceProjector>("GetOwnership", MySpaceTexts.BlockPropertyTitle_Projector_GetOwnership, MySpaceTexts.BlockPropertiesTooltip_Projector_GetOwnership);
      terminalControlCheckbox3.Visible = (Func<MySpaceProjector, bool>) (p => p.ScenarioSettingsEnabled());
      terminalControlCheckbox3.Enabled = (Func<MySpaceProjector, bool>) (p => p.CanEditInstantBuildingSettings());
      terminalControlCheckbox3.Getter = (MyTerminalValueControl<MySpaceProjector, bool>.GetterDelegate) (p => p.GetOwnershipFromProjector);
      terminalControlCheckbox3.Setter = (MyTerminalValueControl<MySpaceProjector, bool>.SetterDelegate) ((p, v) => p.TrySetGetOwnership(v));
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) terminalControlCheckbox3);
      MyTerminalControlSlider<MySpaceProjector> terminalControlSlider1 = new MyTerminalControlSlider<MySpaceProjector>("NumberOfProjections", MySpaceTexts.BlockPropertyTitle_Projector_NumberOfProjections, MySpaceTexts.BlockPropertyTitle_Projector_NumberOfProjections_Tooltip);
      terminalControlSlider1.Visible = (Func<MySpaceProjector, bool>) (p => p.ScenarioSettingsEnabled());
      terminalControlSlider1.Enabled = (Func<MySpaceProjector, bool>) (p => p.CanEditInstantBuildingSettings());
      terminalControlSlider1.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (p => (float) p.MaxNumberOfProjections);
      terminalControlSlider1.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((p, v) => p.TryChangeNumberOfProjections(v));
      terminalControlSlider1.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((p, s) =>
      {
        if (p.MaxNumberOfProjections == 1000)
          s.AppendStringBuilder(MyTexts.Get(MySpaceTexts.ScreenTerminal_Infinite));
        else
          s.AppendInt32(p.MaxNumberOfProjections);
      });
      terminalControlSlider1.SetLogLimits(1f, 1000f);
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) terminalControlSlider1);
      MyTerminalControlSlider<MySpaceProjector> terminalControlSlider2 = new MyTerminalControlSlider<MySpaceProjector>("NumberOfBlocks", MySpaceTexts.BlockPropertyTitle_Projector_BlocksPerProjection, MySpaceTexts.BlockPropertyTitle_Projector_BlocksPerProjection_Tooltip);
      terminalControlSlider2.Visible = (Func<MySpaceProjector, bool>) (p => p.ScenarioSettingsEnabled());
      terminalControlSlider2.Enabled = (Func<MySpaceProjector, bool>) (p => p.CanEditInstantBuildingSettings());
      terminalControlSlider2.Getter = (MyTerminalValueControl<MySpaceProjector, float>.GetterDelegate) (p => (float) p.MaxNumberOfBlocksPerProjection);
      terminalControlSlider2.Setter = (MyTerminalValueControl<MySpaceProjector, float>.SetterDelegate) ((p, v) => p.TryChangeMaxNumberOfBlocksPerProjection(v));
      terminalControlSlider2.Writer = (MyTerminalControl<MySpaceProjector>.WriterDelegate) ((p, s) =>
      {
        if (p.MaxNumberOfBlocksPerProjection == 10000)
          s.AppendStringBuilder(MyTexts.Get(MySpaceTexts.ScreenTerminal_Infinite));
        else
          s.AppendInt32(p.MaxNumberOfBlocksPerProjection);
      });
      terminalControlSlider2.SetLogLimits(1f, 10000f);
      MyTerminalControlFactory.AddControl<MySpaceProjector>((MyTerminalControl<MySpaceProjector>) terminalControlSlider2);
      MyMultiTextPanelComponent.CreateTerminalControls<MySpaceProjector>();
    }

    protected override bool CheckIsWorking() => (this.ResourceSink == null || this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId)) && base.CheckIsWorking();
  }
}
