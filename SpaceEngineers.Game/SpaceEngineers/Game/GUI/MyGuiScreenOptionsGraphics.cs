// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenOptionsGraphics
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.GUI
{
  public class MyGuiScreenOptionsGraphics : MyGuiScreenBase
  {
    private static readonly MyPerformanceSettings[] m_presets = new MyPerformanceSettings[8]
    {
      new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.NONE,
          AntialiasingMode = MyAntialiasingMode.NONE,
          ShadowQuality = MyShadowsQuality.LOW,
          ShadowGPUQuality = MyRenderQualityEnum.LOW,
          AmbientOcclusionEnabled = false,
          TextureQuality = MyTextureQuality.LOW,
          VoxelTextureQuality = MyTextureQuality.LOW,
          ModelQuality = MyRenderQualityEnum.LOW,
          VoxelQuality = MyRenderQualityEnum.LOW,
          GrassDrawDistance = 50f,
          GrassDensityFactor = 0.0f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.LOW,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.LOW,
          AtmosphereShaderQuality = MyRenderQualityEnum.LOW,
          DistanceFade = 500f,
          ParticleQuality = MyRenderQualityEnum.LOW
        },
        EnableDamageEffects = false
      },
      new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.NONE,
          AntialiasingMode = MyAntialiasingMode.FXAA,
          ShadowQuality = MyShadowsQuality.MEDIUM,
          ShadowGPUQuality = MyRenderQualityEnum.NORMAL,
          AmbientOcclusionEnabled = true,
          TextureQuality = MyTextureQuality.MEDIUM,
          VoxelTextureQuality = MyTextureQuality.MEDIUM,
          ModelQuality = MyRenderQualityEnum.NORMAL,
          VoxelQuality = MyRenderQualityEnum.NORMAL,
          GrassDrawDistance = 160f,
          GrassDensityFactor = 1f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.NORMAL,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.NORMAL,
          AtmosphereShaderQuality = MyRenderQualityEnum.NORMAL,
          DistanceFade = 1000f,
          ParticleQuality = MyRenderQualityEnum.NORMAL
        },
        EnableDamageEffects = true
      },
      new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.ANISO_16,
          AntialiasingMode = MyAntialiasingMode.FXAA,
          ShadowQuality = MyShadowsQuality.HIGH,
          ShadowGPUQuality = MyRenderQualityEnum.HIGH,
          AmbientOcclusionEnabled = true,
          TextureQuality = MyTextureQuality.HIGH,
          VoxelTextureQuality = MyTextureQuality.HIGH,
          ModelQuality = MyRenderQualityEnum.HIGH,
          VoxelQuality = MyRenderQualityEnum.HIGH,
          GrassDrawDistance = 1000f,
          GrassDensityFactor = 3f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.HIGH,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.HIGH,
          AtmosphereShaderQuality = MyRenderQualityEnum.HIGH,
          DistanceFade = 2000f,
          ParticleQuality = MyRenderQualityEnum.HIGH
        },
        EnableDamageEffects = true
      },
      new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.ANISO_16,
          AntialiasingMode = MyAntialiasingMode.FXAA,
          ShadowQuality = MyShadowsQuality.EXTREME,
          ShadowGPUQuality = MyRenderQualityEnum.EXTREME,
          AmbientOcclusionEnabled = true,
          TextureQuality = MyTextureQuality.HIGH,
          VoxelTextureQuality = MyTextureQuality.HIGH,
          ModelQuality = MyRenderQualityEnum.EXTREME,
          VoxelQuality = MyRenderQualityEnum.EXTREME,
          GrassDrawDistance = 1000f,
          GrassDensityFactor = 3f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.EXTREME,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.HIGH,
          AtmosphereShaderQuality = MyRenderQualityEnum.HIGH,
          DistanceFade = 2000f,
          ParticleQuality = MyRenderQualityEnum.EXTREME
        },
        EnableDamageEffects = true
      },
      new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.ANISO_4,
          AntialiasingMode = MyAntialiasingMode.FXAA,
          ShadowQuality = MyShadowsQuality.MEDIUM,
          ShadowGPUQuality = MyRenderQualityEnum.HIGH,
          AmbientOcclusionEnabled = true,
          TextureQuality = MyTextureQuality.MEDIUM,
          VoxelTextureQuality = MyTextureQuality.MEDIUM,
          ModelQuality = MyRenderQualityEnum.NORMAL,
          VoxelQuality = MyRenderQualityEnum.NORMAL,
          GrassDrawDistance = 600f,
          GrassDensityFactor = 1f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.HIGH,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.NORMAL,
          AtmosphereShaderQuality = MyRenderQualityEnum.HIGH,
          DistanceFade = 1500f,
          ParticleQuality = MyRenderQualityEnum.NORMAL
        },
        EnableDamageEffects = true
      },
      new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.NONE,
          AntialiasingMode = MyAntialiasingMode.FXAA,
          ShadowQuality = MyShadowsQuality.LOW,
          ShadowGPUQuality = MyRenderQualityEnum.LOW,
          AmbientOcclusionEnabled = false,
          TextureQuality = MyTextureQuality.LOW,
          VoxelTextureQuality = MyTextureQuality.LOW,
          ModelQuality = MyRenderQualityEnum.NORMAL,
          VoxelQuality = MyRenderQualityEnum.LOW,
          GrassDrawDistance = 150f,
          GrassDensityFactor = 1f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.LOW,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.LOW,
          AtmosphereShaderQuality = MyRenderQualityEnum.LOW,
          DistanceFade = 750f,
          ParticleQuality = MyRenderQualityEnum.LOW
        },
        EnableDamageEffects = true
      },
      new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.ANISO_16,
          AntialiasingMode = MyAntialiasingMode.FXAA,
          ShadowQuality = MyShadowsQuality.HIGH,
          ShadowGPUQuality = MyRenderQualityEnum.HIGH,
          AmbientOcclusionEnabled = true,
          TextureQuality = MyTextureQuality.HIGH,
          VoxelTextureQuality = MyTextureQuality.MEDIUM,
          ModelQuality = MyRenderQualityEnum.HIGH,
          VoxelQuality = MyRenderQualityEnum.HIGH,
          GrassDrawDistance = 600f,
          GrassDensityFactor = 1.5f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.HIGH,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.HIGH,
          AtmosphereShaderQuality = MyRenderQualityEnum.HIGH,
          DistanceFade = 2000f,
          ParticleQuality = MyRenderQualityEnum.HIGH
        },
        EnableDamageEffects = true
      },
      new MyPerformanceSettings()
      {
        RenderSettings = new MyRenderSettings1()
        {
          AnisotropicFiltering = MyTextureAnisoFiltering.ANISO_4,
          AntialiasingMode = MyAntialiasingMode.FXAA,
          ShadowQuality = MyShadowsQuality.MEDIUM,
          ShadowGPUQuality = MyRenderQualityEnum.NORMAL,
          AmbientOcclusionEnabled = true,
          TextureQuality = MyTextureQuality.MEDIUM,
          VoxelTextureQuality = MyTextureQuality.MEDIUM,
          ModelQuality = MyRenderQualityEnum.NORMAL,
          VoxelQuality = MyRenderQualityEnum.NORMAL,
          GrassDrawDistance = 500f,
          GrassDensityFactor = 1f,
          HqDepth = true,
          VoxelShaderQuality = MyRenderQualityEnum.NORMAL,
          AlphaMaskedShaderQuality = MyRenderQualityEnum.NORMAL,
          AtmosphereShaderQuality = MyRenderQualityEnum.NORMAL,
          DistanceFade = 1200f,
          ParticleQuality = MyRenderQualityEnum.NORMAL
        },
        EnableDamageEffects = true
      }
    };
    private bool m_writingSettings;
    private MyGuiControlCombobox m_comboAntialiasing;
    private MyGuiControlCombobox m_comboShadowMapResolution;
    private MyGuiControlCheckbox m_checkboxAmbientOcclusionHBAO;
    private MyGuiControlCheckbox m_checkboxPostProcessing;
    private MyGuiControlCombobox m_comboTextureQuality;
    private MyGuiControlCombobox m_comboShaderQuality;
    private MyGuiControlCombobox m_comboAnisotropicFiltering;
    private MyGuiControlCombobox m_comboGraphicsPresets;
    private MyGuiControlCombobox m_comboModelQuality;
    private MyGuiControlCombobox m_comboVoxelQuality;
    private MyGuiControlSliderBase m_vegetationViewDistance;
    private MyGuiControlSlider m_grassDensitySlider;
    private MyGuiControlSliderBase m_grassDrawDistanceSlider;
    private MyGuiControlSlider m_sliderFov;
    private MyGuiControlSlider m_sliderFlares;
    private MyGuiControlCheckbox m_checkboxEnableDamageEffects;
    private MyGraphicsSettings m_settingsOld;
    private MyGraphicsSettings m_settingsNew;
    private MyGuiControlElementGroup m_elementGroup;
    private MyGuiControlButton m_buttonOk;
    private MyGuiControlButton m_buttonCancel;

    public MyGuiScreenOptionsGraphics()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.6535714f, 0.9379771f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.EnabledBackgroundFade = true;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      if (!constructor)
        return;
      base.RecreateControls(constructor);
      this.m_elementGroup = new MyGuiControlElementGroup();
      this.AddCaption(MyTexts.GetString(MyCommonTexts.ScreenCaptionGraphicsOptions), captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList1 = new MyGuiControlSeparatorList();
      controlSeparatorList1.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList1);
      MyGuiControlSeparatorList controlSeparatorList2 = new MyGuiControlSeparatorList();
      controlSeparatorList2.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.829999983310699 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465)), this.m_size.Value.X * 0.83f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList2);
      MyGuiDrawAlignEnum guiDrawAlignEnum1 = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiDrawAlignEnum guiDrawAlignEnum2 = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      Vector2 vector2_1 = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      Vector2 vector2_2 = new Vector2(54f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      float x1 = 455f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      float x2 = 25f;
      float y = MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 0.5f;
      float num1 = 0.0015f;
      Vector2 vector2_3 = new Vector2(0.0f, 0.008f);
      Vector2 vector2_4 = new Vector2(0.0f, 0.045f);
      float num2 = 0.0f;
      Vector2 vector2_5 = new Vector2(0.05f, 0.0f);
      Vector2 vector2_6 = (this.m_size.Value / 2f - vector2_1) * new Vector2(-1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_7 = (this.m_size.Value / 2f - vector2_1) * new Vector2(1f, -1f) + new Vector2(0.0f, y);
      Vector2 vector2_8 = (this.m_size.Value / 2f - vector2_2) * new Vector2(0.0f, 1f);
      Vector2 vector2_9 = new Vector2(vector2_7.X - (x1 + num1), vector2_7.Y);
      Vector2 vector2_10 = vector2_6 + new Vector2(0.255f, 0.0f);
      Vector2 vector2_11 = vector2_9 + new Vector2(0.26f, 0.0f);
      float num3 = num2 - 0.045f;
      MyGuiControlLabel myGuiControlLabel1 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset));
      myGuiControlLabel1.Position = vector2_6 + num3 * vector2_4 + vector2_3;
      myGuiControlLabel1.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel2 = myGuiControlLabel1;
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_QualityPreset));
      guiControlCombobox1.Position = vector2_7 + num3 * vector2_4;
      guiControlCombobox1.OriginAlign = guiDrawAlignEnum2;
      this.m_comboGraphicsPresets = guiControlCombobox1;
      this.m_comboGraphicsPresets.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset_Low));
      this.m_comboGraphicsPresets.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset_Medium));
      this.m_comboGraphicsPresets.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset_High));
      this.m_comboGraphicsPresets.AddItem(3L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_QualityPreset_Custom));
      float num4 = num3 + 1f;
      MyGuiControlLabel myGuiControlLabel3 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ModelQuality));
      myGuiControlLabel3.Position = vector2_6 + num4 * vector2_4 + vector2_3;
      myGuiControlLabel3.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel4 = myGuiControlLabel3;
      MyGuiControlCombobox guiControlCombobox2 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_ModelQuality));
      guiControlCombobox2.Position = vector2_7 + num4 * vector2_4;
      guiControlCombobox2.OriginAlign = guiDrawAlignEnum2;
      this.m_comboModelQuality = guiControlCombobox2;
      this.m_comboModelQuality.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Low));
      this.m_comboModelQuality.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Medium));
      this.m_comboModelQuality.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_High));
      this.m_comboModelQuality.AddItem(3L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Extreme) + " " + MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_PerformanceHeavy));
      float num5 = num4 + 1f;
      MyGuiControlLabel myGuiControlLabel5 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.ScreenGraphicsOptions_ShaderQuality));
      myGuiControlLabel5.Position = vector2_6 + num5 * vector2_4 + vector2_3;
      myGuiControlLabel5.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel6 = myGuiControlLabel5;
      MyGuiControlCombobox guiControlCombobox3 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_ShaderQuality));
      guiControlCombobox3.Position = vector2_7 + num5 * vector2_4;
      guiControlCombobox3.OriginAlign = guiDrawAlignEnum2;
      this.m_comboShaderQuality = guiControlCombobox3;
      this.m_comboShaderQuality.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Low));
      this.m_comboShaderQuality.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Medium));
      this.m_comboShaderQuality.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_High));
      float num6 = num5 + 1f;
      MyGuiControlLabel myGuiControlLabel7 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.ScreenGraphicsOptions_VoxelQuality));
      myGuiControlLabel7.Position = vector2_6 + num6 * vector2_4 + vector2_3;
      myGuiControlLabel7.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel8 = myGuiControlLabel7;
      MyGuiControlCombobox guiControlCombobox4 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_VoxelQuality));
      guiControlCombobox4.Position = vector2_7 + num6 * vector2_4;
      guiControlCombobox4.OriginAlign = guiDrawAlignEnum2;
      this.m_comboVoxelQuality = guiControlCombobox4;
      this.m_comboVoxelQuality.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Low));
      this.m_comboVoxelQuality.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Medium));
      this.m_comboVoxelQuality.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_High));
      this.m_comboVoxelQuality.AddItem(3L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_FoliageDetails_Extreme) + " " + MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_PerformanceHeavy));
      float num7 = num6 + 1f;
      MyGuiControlLabel myGuiControlLabel9 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_TextureQuality));
      myGuiControlLabel9.Position = vector2_6 + num7 * vector2_4 + vector2_3;
      myGuiControlLabel9.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel10 = myGuiControlLabel9;
      MyGuiControlCombobox guiControlCombobox5 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_TextureQuality));
      guiControlCombobox5.Position = vector2_7 + num7 * vector2_4;
      guiControlCombobox5.OriginAlign = guiDrawAlignEnum2;
      this.m_comboTextureQuality = guiControlCombobox5;
      this.m_comboTextureQuality.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_TextureQuality_Low));
      this.m_comboTextureQuality.AddItem(1L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_TextureQuality_Medium));
      this.m_comboTextureQuality.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_TextureQuality_High));
      float num8 = num7 + 1f;
      MyGuiControlLabel myGuiControlLabel11 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.ScreenGraphicsOptions_ShadowMapResolution));
      myGuiControlLabel11.Position = vector2_6 + num8 * vector2_4 + vector2_3;
      myGuiControlLabel11.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel12 = myGuiControlLabel11;
      MyGuiControlCombobox guiControlCombobox6 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_ShadowQuality));
      guiControlCombobox6.Position = vector2_7 + num8 * vector2_4;
      guiControlCombobox6.OriginAlign = guiDrawAlignEnum2;
      this.m_comboShadowMapResolution = guiControlCombobox6;
      this.m_comboShadowMapResolution.AddItem(3L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ShadowMapResolution_Disabled));
      this.m_comboShadowMapResolution.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ShadowMapResolution_Low));
      this.m_comboShadowMapResolution.AddItem(1L, MyTexts.GetString(MySpaceTexts.ScreenGraphicsOptions_ShadowMapResolution_Medium));
      this.m_comboShadowMapResolution.AddItem(2L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ShadowMapResolution_High));
      this.m_comboShadowMapResolution.AddItem(4L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_ShadowMapResolution_Extreme) + " " + MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_PerformanceHeavy));
      float num9 = num8 + 1f;
      MyGuiControlLabel myGuiControlLabel13 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AntiAliasing));
      myGuiControlLabel13.Position = vector2_6 + num9 * vector2_4 + vector2_3;
      myGuiControlLabel13.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel14 = myGuiControlLabel13;
      MyGuiControlCombobox guiControlCombobox7 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_Antialiasing));
      guiControlCombobox7.Position = vector2_7 + num9 * vector2_4;
      guiControlCombobox7.OriginAlign = guiDrawAlignEnum2;
      this.m_comboAntialiasing = guiControlCombobox7;
      this.m_comboAntialiasing.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AntiAliasing_None));
      this.m_comboAntialiasing.AddItem(1L, "FXAA");
      float num10 = num9 + 1f;
      MyGuiControlLabel myGuiControlLabel15 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AnisotropicFiltering));
      myGuiControlLabel15.Position = vector2_6 + num10 * vector2_4 + vector2_3;
      myGuiControlLabel15.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel16 = myGuiControlLabel15;
      MyGuiControlCombobox guiControlCombobox8 = new MyGuiControlCombobox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_AnisotropicFiltering));
      guiControlCombobox8.Position = vector2_7 + num10 * vector2_4;
      guiControlCombobox8.OriginAlign = guiDrawAlignEnum2;
      this.m_comboAnisotropicFiltering = guiControlCombobox8;
      this.m_comboAnisotropicFiltering.AddItem(0L, MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AnisotropicFiltering_Off));
      this.m_comboAnisotropicFiltering.AddItem(1L, "1x");
      this.m_comboAnisotropicFiltering.AddItem(2L, "4x");
      this.m_comboAnisotropicFiltering.AddItem(3L, "8x");
      this.m_comboAnisotropicFiltering.AddItem(4L, "16x");
      float num11 = num10 + 1.05f;
      MyGuiControlLabel myGuiControlLabel17 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.FieldOfView));
      myGuiControlLabel17.Position = vector2_6 + num11 * vector2_4 + vector2_3;
      myGuiControlLabel17.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel18 = myGuiControlLabel17;
      float minRadians;
      float maxRadians;
      MyVideoSettingsManager.GetFovBounds(out minRadians, out maxRadians);
      if (!MySandboxGame.Config.ExperimentalMode)
        maxRadians = Math.Min(maxRadians, MyConstants.FIELD_OF_VIEW_CONFIG_MAX_SAFE);
      Vector2? position1 = new Vector2?();
      string str1 = MyTexts.GetString(MyCommonTexts.ToolTipVideoOptionsFieldOfView);
      double degrees1 = (double) MathHelper.ToDegrees(minRadians);
      double degrees2 = (double) MathHelper.ToDegrees(maxRadians);
      string str2 = new StringBuilder("{0}").ToString();
      float? defaultValue1 = new float?(MathHelper.ToDegrees(MySandboxGame.Config.FieldOfView));
      Vector4? color1 = new Vector4?();
      string labelText1 = str2;
      string toolTip1 = str1;
      MyGuiControlSlider guiControlSlider1 = new MyGuiControlSlider(position1, (float) degrees1, (float) degrees2, defaultValue: defaultValue1, color: color1, labelText: labelText1, labelSpaceWidth: 0.07f, labelFont: "Blue", toolTip: toolTip1, showLabel: true);
      guiControlSlider1.Position = vector2_7 + num11 * vector2_4;
      guiControlSlider1.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider1.Size = new Vector2(x1, 0.0f);
      this.m_sliderFov = guiControlSlider1;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_FOV));
      stringBuilder.Append(" ");
      stringBuilder.AppendFormat(MyCommonTexts.DefaultFOV, (object) MathHelper.ToDegrees(MyConstants.FIELD_OF_VIEW_CONFIG_DEFAULT));
      this.m_sliderFov.SetToolTip(stringBuilder.ToString());
      float num12 = num11 + 1.1f;
      MyGuiControlLabel myGuiControlLabel19 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.FlaresIntensity));
      myGuiControlLabel19.Position = vector2_6 + num12 * vector2_4 + vector2_3;
      myGuiControlLabel19.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel20 = myGuiControlLabel19;
      Vector2? position2 = new Vector2?();
      string str3 = MyTexts.GetString(MySpaceTexts.ToolTipFlaresIntensity);
      string str4 = new StringBuilder("{0}").ToString();
      float? defaultValue2 = new float?(MySandboxGame.Config.FlaresIntensity);
      Vector4? color2 = new Vector4?();
      string labelText2 = str4;
      string toolTip2 = str3;
      MyGuiControlSlider guiControlSlider2 = new MyGuiControlSlider(position2, 0.1f, 2f, defaultValue: defaultValue2, color: color2, labelText: labelText2, labelSpaceWidth: 0.07f, labelFont: "Blue", toolTip: toolTip2, showLabel: true);
      guiControlSlider2.Position = vector2_7 + num12 * vector2_4;
      guiControlSlider2.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider2.Size = new Vector2(x1, 0.0f);
      this.m_sliderFlares = guiControlSlider2;
      float num13 = num12 + 1.1f;
      MyGuiControlLabel myGuiControlLabel21 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.WorldSettings_GrassDrawDistance));
      myGuiControlLabel21.Position = vector2_6 + num13 * vector2_4 + vector2_3;
      myGuiControlLabel21.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel22 = myGuiControlLabel21;
      MyGuiControlSlider guiControlSlider3 = new MyGuiControlSlider(labelSpaceWidth: 0.07f, labelFont: "Blue", toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_GrassDrawDistance), showLabel: true);
      guiControlSlider3.Position = vector2_7 + num13 * vector2_4;
      guiControlSlider3.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider3.Size = new Vector2(x1, 0.0f);
      guiControlSlider3.Propeties = (MyGuiSliderProperties) new MyGuiSliderPropertiesExponential(50f, 5000f, integer: true);
      this.m_grassDrawDistanceSlider = (MyGuiControlSliderBase) guiControlSlider3;
      float num14 = num13 + 1.1f;
      MyGuiControlLabel myGuiControlLabel23 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.WorldSettings_GrassDensity));
      myGuiControlLabel23.Position = vector2_6 + num14 * vector2_4 + vector2_3;
      myGuiControlLabel23.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel24 = myGuiControlLabel23;
      Vector2? position3 = new Vector2?();
      string str5 = new StringBuilder("{0}").ToString();
      string str6 = MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_GrassDensity);
      float? grassDensityFactor = MySandboxGame.Config.GrassDensityFactor;
      Vector4? color3 = new Vector4?();
      string labelText3 = str5;
      string toolTip3 = str6;
      MyGuiControlSlider guiControlSlider4 = new MyGuiControlSlider(position3, maxValue: 10f, defaultValue: grassDensityFactor, color: color3, labelText: labelText3, labelSpaceWidth: 0.07f, labelFont: "Blue", toolTip: toolTip3, showLabel: true);
      guiControlSlider4.Position = vector2_7 + num14 * vector2_4;
      guiControlSlider4.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider4.Size = new Vector2(x1, 0.0f);
      this.m_grassDensitySlider = guiControlSlider4;
      this.m_grassDensitySlider.SetBounds(0.0f, 10f);
      float num15 = num14 + 1.1f;
      MyGuiControlLabel myGuiControlLabel25 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.WorldSettings_VegetationDistance));
      myGuiControlLabel25.Position = vector2_6 + num15 * vector2_4 + vector2_3;
      myGuiControlLabel25.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel26 = myGuiControlLabel25;
      MyGuiControlSlider guiControlSlider5 = new MyGuiControlSlider(labelSpaceWidth: 0.07f, labelFont: "Blue", toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_TreeDrawDistance), showLabel: true);
      guiControlSlider5.Position = vector2_7 + num15 * vector2_4;
      guiControlSlider5.OriginAlign = guiDrawAlignEnum2;
      guiControlSlider5.Size = new Vector2(x1, 0.0f);
      guiControlSlider5.Propeties = (MyGuiSliderProperties) new MyGuiSliderPropertiesExponential(500f, 10000f, integer: true);
      this.m_vegetationViewDistance = (MyGuiControlSliderBase) guiControlSlider5;
      float num16 = num15 + 1.1f;
      MyGuiControlLabel myGuiControlLabel27 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_AmbientOcclusion));
      myGuiControlLabel27.Position = vector2_6 + num16 * vector2_4 + vector2_3;
      myGuiControlLabel27.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel28 = myGuiControlLabel27;
      MyGuiControlCheckbox guiControlCheckbox1 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_AmbientOcclusion));
      guiControlCheckbox1.Position = vector2_10 + num16 * vector2_4;
      guiControlCheckbox1.OriginAlign = guiDrawAlignEnum1;
      this.m_checkboxAmbientOcclusionHBAO = guiControlCheckbox1;
      MyGuiControlLabel myGuiControlLabel29 = new MyGuiControlLabel(text: MyTexts.GetString(MySpaceTexts.EnableDamageEffects));
      myGuiControlLabel29.Position = vector2_9 + vector2_5 + num16 * vector2_4 + vector2_3;
      myGuiControlLabel29.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel30 = myGuiControlLabel29;
      MyGuiControlCheckbox guiControlCheckbox2 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipVideoOptionsEnableDamageEffects));
      guiControlCheckbox2.Position = vector2_11 + num16 * vector2_4;
      guiControlCheckbox2.OriginAlign = guiDrawAlignEnum1;
      this.m_checkboxEnableDamageEffects = guiControlCheckbox2;
      float num17 = num16 + 1f;
      MyGuiControlLabel myGuiControlLabel31 = new MyGuiControlLabel(text: MyTexts.GetString(MyCommonTexts.ScreenGraphicsOptions_PostProcessing));
      myGuiControlLabel31.Position = vector2_6 + num17 * vector2_4 + vector2_3;
      myGuiControlLabel31.OriginAlign = guiDrawAlignEnum1;
      MyGuiControlLabel myGuiControlLabel32 = myGuiControlLabel31;
      MyGuiControlCheckbox guiControlCheckbox3 = new MyGuiControlCheckbox(toolTip: MyTexts.GetString(MySpaceTexts.ToolTipOptionsGraphics_PostProcessing));
      guiControlCheckbox3.Position = vector2_10 + num17 * vector2_4;
      guiControlCheckbox3.OriginAlign = guiDrawAlignEnum1;
      guiControlCheckbox3.IsChecked = MySandboxGame.Config.PostProcessingEnabled;
      this.m_checkboxPostProcessing = guiControlCheckbox3;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel20);
      this.Controls.Add((MyGuiControlBase) this.m_sliderFlares);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel18);
      this.Controls.Add((MyGuiControlBase) this.m_sliderFov);
      if (MyVideoSettingsManager.RunningGraphicsRenderer == MySandboxGame.DirectX11RendererKey)
      {
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel2);
        this.Controls.Add((MyGuiControlBase) this.m_comboGraphicsPresets);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel14);
        this.Controls.Add((MyGuiControlBase) this.m_comboAntialiasing);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel12);
        this.Controls.Add((MyGuiControlBase) this.m_comboShadowMapResolution);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel10);
        this.Controls.Add((MyGuiControlBase) this.m_comboTextureQuality);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel4);
        this.Controls.Add((MyGuiControlBase) this.m_comboModelQuality);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel6);
        this.Controls.Add((MyGuiControlBase) this.m_comboShaderQuality);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel8);
        this.Controls.Add((MyGuiControlBase) this.m_comboVoxelQuality);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel16);
        this.Controls.Add((MyGuiControlBase) this.m_comboAnisotropicFiltering);
        if (MyFakes.ENABLE_PLANETS)
        {
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel22);
          this.Controls.Add((MyGuiControlBase) this.m_grassDrawDistanceSlider);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel24);
          this.Controls.Add((MyGuiControlBase) this.m_grassDensitySlider);
          this.Controls.Add((MyGuiControlBase) myGuiControlLabel26);
          this.Controls.Add((MyGuiControlBase) this.m_vegetationViewDistance);
        }
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel30);
        this.Controls.Add((MyGuiControlBase) this.m_checkboxEnableDamageEffects);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel28);
        this.Controls.Add((MyGuiControlBase) this.m_checkboxAmbientOcclusionHBAO);
        this.Controls.Add((MyGuiControlBase) myGuiControlLabel32);
        this.Controls.Add((MyGuiControlBase) this.m_checkboxPostProcessing);
      }
      this.m_buttonOk = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkClick));
      this.m_buttonOk.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Ok));
      this.m_buttonOk.Position = vector2_8 + new Vector2(-x2, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonOk.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM;
      this.m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonCancel = new MyGuiControlButton(text: MyTexts.Get(MyCommonTexts.Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancelClick));
      this.m_buttonCancel.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
      this.m_buttonCancel.Position = vector2_8 + new Vector2(x2, 0.0f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
      this.m_buttonCancel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM;
      this.m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.Controls.Add((MyGuiControlBase) this.m_buttonOk);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_buttonOk);
      this.Controls.Add((MyGuiControlBase) this.m_buttonCancel);
      this.m_elementGroup.Add((MyGuiControlBase) this.m_buttonCancel);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel33 = new MyGuiControlLabel(new Vector2?(new Vector2(vector2_6.X, this.m_buttonOk.Position.Y - minSizeGui.Y / 2f)));
      myGuiControlLabel33.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel33);
      this.m_settingsOld = MyVideoSettingsManager.CurrentGraphicsSettings;
      this.m_settingsNew = this.m_settingsOld;
      this.WriteSettingsToControls(this.m_settingsOld);
      this.ReadSettingsFromControls(ref this.m_settingsOld);
      this.ReadSettingsFromControls(ref this.m_settingsNew);
      MyGuiControlCombobox.ItemSelectedDelegate selectedDelegate = new MyGuiControlCombobox.ItemSelectedDelegate(this.OnSettingsChanged);
      Action<MyGuiControlCheckbox> action = (Action<MyGuiControlCheckbox>) (checkbox => this.OnSettingsChanged());
      this.m_comboGraphicsPresets.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnPresetSelected);
      this.m_comboAnisotropicFiltering.ItemSelected += selectedDelegate;
      this.m_comboAntialiasing.ItemSelected += selectedDelegate;
      this.m_comboShadowMapResolution.ItemSelected += selectedDelegate;
      this.m_checkboxAmbientOcclusionHBAO.IsCheckedChanged += action;
      this.m_comboVoxelQuality.ItemSelected += selectedDelegate;
      this.m_comboModelQuality.ItemSelected += selectedDelegate;
      this.m_comboTextureQuality.ItemSelected += selectedDelegate;
      this.m_comboShaderQuality.ItemSelected += selectedDelegate;
      this.m_sliderFlares.ValueChanged = (Action<MyGuiControlSlider>) (slider => this.OnSettingsChanged());
      this.m_checkboxEnableDamageEffects.IsCheckedChanged = action;
      this.m_sliderFov.ValueChanged = (Action<MyGuiControlSlider>) (slider => this.OnSettingsChanged());
      this.m_checkboxPostProcessing.IsCheckedChanged += action;
      this.RefreshPresetCombo(this.m_settingsOld);
      this.CloseButtonEnabled = true;
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.GraphicsOptions_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_comboGraphicsPresets;
    }

    private void RefreshPresetCombo(MyGraphicsSettings settings)
    {
      int index = 0;
      while (index < MyGuiScreenOptionsGraphics.m_presets.Length && !MyGuiScreenOptionsGraphics.m_presets[index].Equals((object) settings.PerformanceSettings))
        ++index;
      if (index >= 3)
        this.m_comboGraphicsPresets.SelectItemByKey(3L);
      else
        this.m_comboGraphicsPresets.SelectItemByKey((long) index, false);
    }

    public override string GetFriendlyName() => "MyGuiScreenOptionsVideo";

    private void OnPresetSelected()
    {
      MyGuiScreenOptionsGraphics.PresetEnum selectedKey = (MyGuiScreenOptionsGraphics.PresetEnum) this.m_comboGraphicsPresets.GetSelectedKey();
      if (selectedKey == MyGuiScreenOptionsGraphics.PresetEnum.Custom)
        return;
      this.m_settingsNew.PerformanceSettings = MyGuiScreenOptionsGraphics.m_presets[(int) selectedKey];
      this.WriteSettingsToControls(this.m_settingsNew);
    }

    private void OnSettingsChanged()
    {
      this.m_comboGraphicsPresets.SelectItemByKey(3L);
      this.ReadSettingsFromControls(ref this.m_settingsNew);
      this.RefreshPresetCombo(this.m_settingsNew);
    }

    private bool ReadSettingsFromControls(ref MyGraphicsSettings graphicsSettings)
    {
      if (this.m_writingSettings)
        return false;
      MyGraphicsSettings graphicsSettings1 = new MyGraphicsSettings()
      {
        GraphicsRenderer = graphicsSettings.GraphicsRenderer,
        FieldOfView = MathHelper.ToRadians(this.m_sliderFov.Value),
        PostProcessingEnabled = this.m_checkboxPostProcessing.IsChecked,
        FlaresIntensity = this.m_sliderFlares.Value,
        PerformanceSettings = new MyPerformanceSettings()
        {
          EnableDamageEffects = this.m_checkboxEnableDamageEffects.IsChecked,
          RenderSettings = {
            AntialiasingMode = (MyAntialiasingMode) this.m_comboAntialiasing.GetSelectedKey(),
            AmbientOcclusionEnabled = this.m_checkboxAmbientOcclusionHBAO.IsChecked,
            ShadowQuality = (MyShadowsQuality) this.m_comboShadowMapResolution.GetSelectedKey(),
            ShadowGPUQuality = (MyRenderQualityEnum) this.m_comboShaderQuality.GetSelectedKey(),
            TextureQuality = (MyTextureQuality) this.m_comboTextureQuality.GetSelectedKey(),
            VoxelTextureQuality = (MyTextureQuality) this.m_comboTextureQuality.GetSelectedKey(),
            AnisotropicFiltering = (MyTextureAnisoFiltering) this.m_comboAnisotropicFiltering.GetSelectedKey(),
            ModelQuality = (MyRenderQualityEnum) this.m_comboModelQuality.GetSelectedKey(),
            VoxelQuality = (MyRenderQualityEnum) this.m_comboVoxelQuality.GetSelectedKey(),
            GrassDrawDistance = this.m_grassDrawDistanceSlider.Value,
            GrassDensityFactor = this.m_grassDensitySlider.Value,
            VoxelShaderQuality = (MyRenderQualityEnum) this.m_comboShaderQuality.GetSelectedKey(),
            AlphaMaskedShaderQuality = (MyRenderQualityEnum) this.m_comboShaderQuality.GetSelectedKey(),
            AtmosphereShaderQuality = (MyRenderQualityEnum) this.m_comboShaderQuality.GetSelectedKey(),
            HqDepth = true,
            DistanceFade = this.m_vegetationViewDistance.Value,
            ParticleQuality = (MyRenderQualityEnum) this.m_comboShaderQuality.GetSelectedKey()
          }
        }
      };
      int num = graphicsSettings1.GraphicsRenderer != graphicsSettings.GraphicsRenderer ? 1 : 0;
      graphicsSettings = graphicsSettings1;
      return num != 0;
    }

    private void WriteSettingsToControls(MyGraphicsSettings graphicsSettings)
    {
      this.m_writingSettings = true;
      this.m_sliderFlares.Value = graphicsSettings.FlaresIntensity;
      this.m_sliderFov.Value = MathHelper.ToDegrees(graphicsSettings.FieldOfView);
      this.m_checkboxPostProcessing.IsChecked = graphicsSettings.PostProcessingEnabled;
      this.m_comboModelQuality.SelectItemByKey((long) graphicsSettings.PerformanceSettings.RenderSettings.ModelQuality, false);
      this.m_comboVoxelQuality.SelectItemByKey((long) graphicsSettings.PerformanceSettings.RenderSettings.VoxelQuality, false);
      this.m_grassDrawDistanceSlider.Value = graphicsSettings.PerformanceSettings.RenderSettings.GrassDrawDistance;
      this.m_grassDensitySlider.Value = graphicsSettings.PerformanceSettings.RenderSettings.GrassDensityFactor;
      this.m_vegetationViewDistance.Value = graphicsSettings.PerformanceSettings.RenderSettings.DistanceFade;
      this.m_checkboxEnableDamageEffects.IsChecked = graphicsSettings.PerformanceSettings.EnableDamageEffects;
      this.m_comboAntialiasing.SelectItemByKey((long) graphicsSettings.PerformanceSettings.RenderSettings.AntialiasingMode, false);
      this.m_checkboxAmbientOcclusionHBAO.IsChecked = graphicsSettings.PerformanceSettings.RenderSettings.AmbientOcclusionEnabled;
      this.m_comboShadowMapResolution.SelectItemByKey((long) graphicsSettings.PerformanceSettings.RenderSettings.ShadowQuality, false);
      this.m_comboTextureQuality.SelectItemByKey((long) graphicsSettings.PerformanceSettings.RenderSettings.TextureQuality, false);
      this.m_comboShaderQuality.SelectItemByKey((long) graphicsSettings.PerformanceSettings.RenderSettings.VoxelShaderQuality, false);
      this.m_comboAnisotropicFiltering.SelectItemByKey((long) graphicsSettings.PerformanceSettings.RenderSettings.AnisotropicFiltering, false);
      this.m_writingSettings = false;
    }

    public void OnCancelClick(MyGuiControlButton sender)
    {
      int num = (int) MyVideoSettingsManager.Apply(this.m_settingsOld);
      MyVideoSettingsManager.SaveCurrentSettings();
      this.CloseScreen();
    }

    public void OnOkClick(MyGuiControlButton sender)
    {
      if (this.ReadSettingsFromControls(ref this.m_settingsNew))
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: MyTexts.Get(MySpaceTexts.MessageBoxTextRestartNeededAfterRendererSwitch), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
      int num = (int) MyVideoSettingsManager.Apply(this.m_settingsNew);
      MyVideoSettingsManager.SaveCurrentSettings();
      this.CloseScreen();
    }

    public static MyPerformanceSettings GetPreset(
      MyRenderPresetEnum adapterQuality)
    {
      return MyGuiScreenOptionsGraphics.m_presets[(int) adapterQuality];
    }

    public override bool Update(bool hasFocus)
    {
      int num = base.Update(hasFocus) ? 1 : 0;
      if (!hasFocus)
        return num != 0;
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OnOkClick((MyGuiControlButton) null);
      this.m_buttonOk.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_buttonCancel.Visible = !MyInput.Static.IsJoystickLastUsed;
      return num != 0;
    }

    private enum PresetEnum
    {
      Low,
      Medium,
      High,
      Custom,
    }
  }
}
