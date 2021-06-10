// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenWardrobe
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenWardrobe : MyGuiScreenBase
  {
    private const string m_hueScaleTexture = "Textures\\GUI\\HueScale.png";
    private MyGuiControlCombobox m_modelPicker;
    private MyGuiControlSlider m_sliderHue;
    private MyGuiControlSlider m_sliderSaturation;
    private MyGuiControlSlider m_sliderValue;
    private MyGuiControlLabel m_labelHue;
    private MyGuiControlLabel m_labelSaturation;
    private MyGuiControlLabel m_labelValue;
    private string m_selectedModel;
    private Vector3 m_selectedHSV;
    private MyCharacter m_user;
    private Dictionary<string, int> m_displayModels;
    private Dictionary<int, string> m_models;
    private string m_storedModel;
    private Vector3 m_storedHSV;
    private MyGuiScreenWardrobe.MyCameraControllerSettings m_storedCamera;
    private bool m_colorOrModelChanged;

    public static event MyWardrobeChangeDelegate LookChanged;

    public MyGuiScreenWardrobe(MyCharacter user, HashSet<string> customCharacterNames = null)
    {
      Vector2? size = new Vector2?(new Vector2(0.31f, 0.55f));
      // ISSUE: explicit constructor call
      base.\u002Ector(new Vector2?(MyGuiManager.ComputeFullscreenGuiCoordinate(MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), size, backgroundTexture: MyGuiConstants.TEXTURE_SCREEN_BACKGROUND.Texture);
      this.EnabledBackgroundFade = false;
      this.m_user = user;
      this.m_storedModel = this.m_user.ModelName;
      this.m_storedHSV = this.m_user.ColorMask;
      this.m_selectedModel = this.GetDisplayName(this.m_user.ModelName);
      this.m_selectedHSV = this.m_storedHSV;
      this.m_displayModels = new Dictionary<string, int>();
      this.m_models = new Dictionary<int, string>();
      int num = 0;
      if (customCharacterNames == null)
      {
        foreach (MyCharacterDefinition character in MyDefinitionManager.Static.Characters)
        {
          if ((!MySession.Static.SurvivalMode || character.UsableByPlayer) && character.Public)
          {
            this.m_displayModels[this.GetDisplayName(character.Name)] = num;
            this.m_models[num++] = character.Name;
          }
        }
      }
      else
      {
        DictionaryValuesReader<string, MyCharacterDefinition> characters = MyDefinitionManager.Static.Characters;
        foreach (string customCharacterName in customCharacterNames)
        {
          MyCharacterDefinition result;
          if (characters.TryGetValue(customCharacterName, out result) && (!MySession.Static.SurvivalMode || result.UsableByPlayer) && result.Public)
          {
            this.m_displayModels[this.GetDisplayName(result.Name)] = num;
            this.m_models[num++] = result.Name;
          }
        }
      }
      this.RecreateControls(true);
      this.m_sliderHue.Value = this.m_selectedHSV.X * 360f;
      this.m_sliderSaturation.Value = MathHelper.Clamp(this.m_selectedHSV.Y + MyColorPickerConstants.SATURATION_DELTA, 0.0f, 1f);
      this.m_sliderValue.Value = MathHelper.Clamp(this.m_selectedHSV.Z + MyColorPickerConstants.VALUE_DELTA - MyColorPickerConstants.VALUE_COLORIZE_DELTA, 0.0f, 1f);
      this.m_sliderHue.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_sliderSaturation.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_sliderValue.ValueChanged += new Action<MyGuiControlSlider>(this.OnValueChange);
      this.ChangeCamera();
      this.UpdateLabels();
    }

    private string GetDisplayName(string name) => MyTexts.GetString(name);

    public override string GetFriendlyName() => nameof (MyGuiScreenWardrobe);

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (MyInput.Static.IsNewGameControlPressed(MyControlsSpace.USE))
      {
        this.ChangeCameraBack();
        this.CloseScreen();
      }
      base.HandleInput(receivedFocusInThisUpdate);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      MyGuiControlLabel myGuiControlLabel = this.AddCaption(MyCommonTexts.PlayerCharacterModel);
      Vector2 itemSize = MyGuiControlListbox.GetVisualStyle(MyGuiControlListboxStyleEnum.Default).ItemSize;
      float y1 = -0.19f;
      this.m_modelPicker = new MyGuiControlCombobox(new Vector2?(new Vector2(0.0f, y1)));
      foreach (KeyValuePair<string, int> displayModel in this.m_displayModels)
        this.m_modelPicker.AddItem((long) displayModel.Value, new StringBuilder(displayModel.Key));
      if (this.m_displayModels.ContainsKey(this.m_selectedModel))
        this.m_modelPicker.SelectItemByKey((long) this.m_displayModels[this.m_selectedModel]);
      else if (this.m_displayModels.Count > 0)
        this.m_modelPicker.SelectItemByKey((long) this.m_displayModels.First<KeyValuePair<string, int>>().Value);
      this.m_modelPicker.ItemSelected += new MyGuiControlCombobox.ItemSelectedDelegate(this.OnItemSelected);
      float y2 = y1 + 0.045f;
      Vector2 vector2 = itemSize + myGuiControlLabel.Size;
      this.m_position.X -= vector2.X / 2.5f;
      this.m_position.Y += vector2.Y * 3.6f;
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.0f, y2)), text: MyTexts.GetString(MyCommonTexts.PlayerCharacterColor), originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
      float y3 = y2 + 0.04f;
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(-0.135f, y3)), text: MyTexts.GetString(MyCommonTexts.ScreenWardrobeOld_Hue)));
      this.m_labelHue = new MyGuiControlLabel(new Vector2?(new Vector2(0.09f, y3)), text: string.Empty);
      float y4 = y3 + 0.035f;
      this.m_sliderHue = new MyGuiControlSlider(new Vector2?(new Vector2(-0.135f, y4)), maxValue: 360f, width: 0.3f, labelDecimalPlaces: 0, labelSpaceWidth: 0.04166667f, visualStyle: MyGuiControlSliderStyleEnum.Hue, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, intValue: true);
      float y5 = y4 + 0.045f;
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(-0.135f, y5)), text: MyTexts.GetString(MyCommonTexts.ScreenWardrobeOld_Saturation)));
      this.m_labelSaturation = new MyGuiControlLabel(new Vector2?(new Vector2(0.09f, y5)), text: string.Empty);
      float y6 = y5 + 0.035f;
      this.m_sliderSaturation = new MyGuiControlSlider(new Vector2?(new Vector2(-0.135f, y6)), width: 0.3f, defaultValue: new float?(0.0f), labelSpaceWidth: 0.04166667f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      float y7 = y6 + 0.045f;
      this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(-0.135f, y7)), text: MyTexts.GetString(MyCommonTexts.ScreenWardrobeOld_Value)));
      this.m_labelValue = new MyGuiControlLabel(new Vector2?(new Vector2(0.09f, y7)), text: string.Empty);
      float y8 = y7 + 0.035f;
      this.m_sliderValue = new MyGuiControlSlider(new Vector2?(new Vector2(-0.135f, y8)), width: 0.3f, defaultValue: new float?(0.0f), labelSpaceWidth: 0.04166667f, originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      float num = y8 + 0.045f;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.Controls.Add((MyGuiControlBase) this.m_modelPicker);
      this.Controls.Add((MyGuiControlBase) this.m_labelHue);
      this.Controls.Add((MyGuiControlBase) this.m_labelSaturation);
      this.Controls.Add((MyGuiControlBase) this.m_labelValue);
      this.Controls.Add((MyGuiControlBase) this.m_sliderHue);
      this.Controls.Add((MyGuiControlBase) this.m_sliderSaturation);
      this.Controls.Add((MyGuiControlBase) this.m_sliderValue);
      this.Controls.Add((MyGuiControlBase) new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.16f)), text: MyTexts.Get(MyCommonTexts.ScreenWardrobeOld_Ok), onButtonClick: new Action<MyGuiControlButton>(this.OnOkClick)));
      this.Controls.Add((MyGuiControlBase) new MyGuiControlButton(new Vector2?(new Vector2(0.0f, 0.22f)), text: MyTexts.Get(MyCommonTexts.ScreenWardrobeOld_Cancel), onButtonClick: new Action<MyGuiControlButton>(this.OnCancelClick)));
      this.m_colorOrModelChanged = false;
    }

    protected override void Canceling()
    {
      this.m_sliderHue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_sliderSaturation.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_sliderValue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      this.ChangeCharacter(this.m_storedModel, this.m_storedHSV);
      this.ChangeCameraBack();
      base.Canceling();
    }

    protected override void OnClosed()
    {
      this.m_sliderHue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_sliderSaturation.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_sliderValue.ValueChanged -= new Action<MyGuiControlSlider>(this.OnValueChange);
      MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) null;
      base.OnClosed();
    }

    private void OnOkClick(MyGuiControlButton sender)
    {
      if (this.m_colorOrModelChanged && MyGuiScreenWardrobe.LookChanged != null)
        MyGuiScreenWardrobe.LookChanged(this.m_storedModel, this.m_storedHSV, this.m_user.ModelName, this.m_user.ColorMask);
      if (this.m_user.Definition.UsableByPlayer)
        MyLocalCache.SaveInventoryConfig(MySession.Static.LocalCharacter);
      this.ChangeCameraBack();
      this.CloseScreenNow();
    }

    private void OnCancelClick(MyGuiControlButton sender)
    {
      this.ChangeCharacter(this.m_storedModel, this.m_storedHSV);
      this.ChangeCameraBack();
      this.CloseScreenNow();
    }

    private void OnItemSelected()
    {
      this.m_selectedModel = this.m_models[(int) this.m_modelPicker.GetSelectedKey()];
      this.ChangeCharacter(this.m_selectedModel, this.m_selectedHSV);
    }

    private void OnValueChange(MyGuiControlSlider sender)
    {
      this.UpdateLabels();
      this.m_selectedHSV.X = this.m_sliderHue.Value / 360f;
      this.m_selectedHSV.Y = this.m_sliderSaturation.Value - MyColorPickerConstants.SATURATION_DELTA;
      this.m_selectedHSV.Z = this.m_sliderValue.Value - MyColorPickerConstants.VALUE_DELTA + MyColorPickerConstants.VALUE_COLORIZE_DELTA;
      this.m_selectedModel = this.m_models[(int) this.m_modelPicker.GetSelectedKey()];
      this.ChangeCharacter(this.m_selectedModel, this.m_selectedHSV);
    }

    private void UpdateLabels()
    {
      this.m_labelHue.Text = this.m_sliderHue.Value.ToString() + "°";
      this.m_labelSaturation.Text = this.m_sliderSaturation.Value.ToString("P1");
      this.m_labelValue.Text = this.m_sliderValue.Value.ToString("P1");
    }

    private void ChangeCamera()
    {
      if (!MySession.Static.Settings.Enable3rdPersonView)
        return;
      this.m_storedCamera.Controller = MySession.Static.GetCameraControllerEnum();
      this.m_storedCamera.Distance = (double) MySession.Static.GetCameraTargetDistance();
      MySession.Static.SetCameraController(MyCameraControllerEnum.ThirdPersonSpectator, (IMyEntity) null, new Vector3D?());
      MySession.Static.SetCameraTargetDistance(2.0);
    }

    private void ChangeCameraBack()
    {
      if (!MySession.Static.Settings.Enable3rdPersonView)
        return;
      MySession.Static.SetCameraController(this.m_storedCamera.Controller, (IMyEntity) this.m_user, new Vector3D?());
      MySession.Static.SetCameraTargetDistance(this.m_storedCamera.Distance);
    }

    private void ChangeCharacter(string model, Vector3 colorMaskHSV)
    {
      this.m_colorOrModelChanged = true;
      this.m_user.ChangeModelAndColor(model, colorMaskHSV, caller: MySession.Static.LocalPlayerId);
    }

    public struct MyCameraControllerSettings
    {
      public double Distance;
      public MyCameraControllerEnum Controller;
    }
  }
}
