// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Microsoft.CSharp.RuntimeBinder;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public abstract class MyGuiScreenDebugBase : MyGuiScreenBase
  {
    private static Vector4 m_defaultColor = Color.Yellow.ToVector4();
    private static Vector4 m_defaultTextColor = new Vector4(1f, 1f, 0.0f, 1f);
    protected Vector2 m_currentPosition;
    protected float m_scale = 1f;
    protected float m_buttonXOffset;
    protected float m_sliderDebugScale = 1f;
    private float m_maxWidth;
    protected float Spacing;

    public override string GetFriendlyName() => this.GetType().Name;

    protected MyGuiScreenDebugBase(Vector4? backgroundColor = null, bool isTopMostScreen = false)
      : this(new Vector2(MyGuiManager.GetMaxMouseCoord().X - 0.16f, 0.5f), new Vector2?(new Vector2(0.32f, 1f)), new Vector4?(backgroundColor ?? 0.85f * Color.Black.ToVector4()), isTopMostScreen)
    {
      this.m_closeOnEsc = true;
      this.m_drawEvenWithoutFocus = true;
      this.m_isTopMostScreen = false;
      this.CanHaveFocus = false;
      this.m_isTopScreen = true;
    }

    protected MyGuiScreenDebugBase(
      Vector2 position,
      Vector2? size,
      Vector4? backgroundColor,
      bool isTopMostScreen)
      : base(new Vector2?(position), backgroundColor, size, isTopMostScreen)
    {
      this.CanBeHidden = false;
      this.CanHideOthers = false;
      this.m_canCloseInCloseAllScreenCalls = false;
      this.m_canShareInput = true;
      this.m_isTopScreen = true;
    }

    protected MyGuiControlMultilineText AddMultilineText(
      Vector2? size = null,
      Vector2? offset = null,
      float textScale = 1f,
      bool selectable = false)
    {
      Vector2 vector2 = size ?? this.Size ?? new Vector2(0.5f, 0.5f);
      MyGuiControlMultilineText controlMultilineText = new MyGuiControlMultilineText(new Vector2?(this.m_currentPosition + vector2 / 2f + (offset ?? Vector2.Zero)), new Vector2?(vector2), new Vector4?(MyGuiScreenDebugBase.m_defaultColor), "Debug", this.m_scale * textScale, textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, selectable: selectable);
      this.Controls.Add((MyGuiControlBase) controlMultilineText);
      return controlMultilineText;
    }

    private MyGuiControlCheckbox AddCheckBox(
      string text,
      bool enabled = true,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      Vector2? checkBoxOffset = null)
    {
      Vector2? position1 = new Vector2?(this.m_currentPosition);
      Vector2? size1 = new Vector2?();
      string text1 = text;
      Vector4? nullable = color;
      Vector4? colorMask = new Vector4?(nullable ?? MyGuiScreenDebugBase.m_defaultTextColor);
      double num = 0.800000011920929 * (double) MyGuiConstants.DEBUG_LABEL_TEXT_SCALE * (double) this.m_scale;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(position1, size1, text1, colorMask, (float) num, "Debug");
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_maxWidth = Math.Max(this.m_maxWidth, myGuiControlLabel.GetTextSize().X + 0.02f);
      myGuiControlLabel.Enabled = enabled;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      Vector2? size2 = this.GetSize();
      Vector2? position2 = new Vector2?();
      nullable = color;
      Vector4? color1 = new Vector4?(nullable ?? MyGuiScreenDebugBase.m_defaultColor);
      MyGuiControlCheckbox guiControlCheckbox = new MyGuiControlCheckbox(position2, color1, visualStyle: MyGuiControlCheckboxStyleEnum.Debug, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
      guiControlCheckbox.Position = this.m_currentPosition + new Vector2(size2.Value.X - guiControlCheckbox.Size.X, 0.0f) + (checkBoxOffset ?? Vector2.Zero);
      guiControlCheckbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
      guiControlCheckbox.Enabled = enabled;
      this.Controls.Add((MyGuiControlBase) guiControlCheckbox);
      this.m_currentPosition.Y += Math.Max(guiControlCheckbox.Size.Y, myGuiControlLabel.Size.Y) + this.Spacing;
      if (controlGroup != null)
      {
        controlGroup.Add((MyGuiControlBase) myGuiControlLabel);
        controlGroup.Add((MyGuiControlBase) guiControlCheckbox);
      }
      return guiControlCheckbox;
    }

    protected MyGuiControlCheckbox AddCheckBox(string text, bool enabled)
    {
      MyGuiControlCheckbox guiControlCheckbox = this.AddCheckBox(text);
      guiControlCheckbox.IsChecked = enabled;
      return guiControlCheckbox;
    }

    protected MyGuiControlCheckbox AddCheckBox(
      string text,
      MyDebugComponent component,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      Vector2? checkBoxOffset = null)
    {
      MyGuiControlCheckbox guiControlCheckbox = this.AddCheckBox(text, controlGroup: controlGroup, color: color, checkBoxOffset: checkBoxOffset);
      guiControlCheckbox.IsChecked = component.Enabled;
      guiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (sender => component.Enabled = sender.IsChecked);
      return guiControlCheckbox;
    }

    protected MyGuiControlCheckbox AddCheckBox(
      MyStringId textEnum,
      bool checkedState,
      Action<MyGuiControlCheckbox> checkBoxChange,
      bool enabled = true,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      Vector2? checkBoxOffset = null)
    {
      return this.AddCheckBox(MyTexts.GetString(textEnum), checkedState, checkBoxChange, enabled, controlGroup, color, checkBoxOffset);
    }

    protected MyGuiControlCheckbox AddCheckBox(
      string text,
      bool checkedState,
      Action<MyGuiControlCheckbox> checkBoxChange,
      bool enabled = true,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      Vector2? checkBoxOffset = null)
    {
      MyGuiControlCheckbox guiControlCheckbox = this.AddCheckBox(text, enabled, controlGroup, color, checkBoxOffset);
      guiControlCheckbox.IsChecked = checkedState;
      if (checkBoxChange != null)
        guiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (sender =>
        {
          checkBoxChange(sender);
          this.ValueChanged((MyGuiControlBase) sender);
        });
      return guiControlCheckbox;
    }

    protected MyGuiControlCheckbox AddCheckBox(
      MyStringId textEnum,
      Func<bool> getter,
      Action<bool> setter,
      bool enabled = true,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      Vector2? checkBoxOffset = null)
    {
      return this.AddCheckBox(MyTexts.GetString(textEnum), getter, setter, enabled, controlGroup, color, checkBoxOffset);
    }

    protected MyGuiControlCheckbox AddCheckBox(
      string text,
      Func<bool> getter,
      Action<bool> setter,
      bool enabled = true,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      Vector2? checkBoxOffset = null)
    {
      MyGuiControlCheckbox guiControlCheckbox = this.AddCheckBox(text, enabled, controlGroup, color, checkBoxOffset);
      if (getter != null)
        guiControlCheckbox.IsChecked = getter();
      if (setter != null)
        guiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (sender =>
        {
          setter(sender.IsChecked);
          this.ValueChanged((MyGuiControlBase) sender);
        });
      return guiControlCheckbox;
    }

    protected MyGuiControlCheckbox AddCheckBox(
      string text,
      object instance,
      MemberInfo memberInfo,
      bool enabled = true,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      Vector2? checkBoxOffset = null)
    {
      MyGuiControlCheckbox guiControlCheckbox = this.AddCheckBox(text, enabled, controlGroup, color, checkBoxOffset);
      if ((object) (memberInfo as PropertyInfo) != null)
      {
        PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
        guiControlCheckbox.IsChecked = (bool) propertyInfo.GetValue(instance, new object[0]);
        guiControlCheckbox.UserData = (object) new Tuple<object, PropertyInfo>(instance, propertyInfo);
        guiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (sender =>
        {
          Tuple<object, PropertyInfo> userData = sender.UserData as Tuple<object, PropertyInfo>;
          userData.Item2.SetValue(userData.Item1, (object) sender.IsChecked, new object[0]);
          this.ValueChanged((MyGuiControlBase) sender);
        });
      }
      else if ((object) (memberInfo as FieldInfo) != null)
      {
        FieldInfo fieldInfo = (FieldInfo) memberInfo;
        guiControlCheckbox.IsChecked = (bool) fieldInfo.GetValue(instance);
        guiControlCheckbox.UserData = (object) new Tuple<object, FieldInfo>(instance, fieldInfo);
        guiControlCheckbox.IsCheckedChanged = (Action<MyGuiControlCheckbox>) (sender =>
        {
          Tuple<object, FieldInfo> userData = sender.UserData as Tuple<object, FieldInfo>;
          userData.Item2.SetValue(userData.Item1, (object) sender.IsChecked);
          this.ValueChanged((MyGuiControlBase) sender);
        });
      }
      return guiControlCheckbox;
    }

    protected virtual void ValueChanged(MyGuiControlBase sender)
    {
    }

    private MyGuiControlSliderBase AddSliderBase(
      string text,
      MyGuiSliderProperties props,
      Vector4? color = null)
    {
      MyGuiControlSliderBase controlSliderBase1 = new MyGuiControlSliderBase(new Vector2?(this.m_currentPosition), 460f / MyGuiConstants.GUI_OPTIMAL_SIZE.X, props, labelScale: (0.75f * this.m_scale), labelFont: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      controlSliderBase1.DebugScale = this.m_sliderDebugScale;
      MyGuiControlSliderBase controlSliderBase2 = controlSliderBase1;
      Vector4? nullable = color;
      Vector4 vector4 = nullable ?? MyGuiScreenDebugBase.m_defaultColor;
      controlSliderBase2.ColorMask = vector4;
      this.Controls.Add((MyGuiControlBase) controlSliderBase1);
      Vector2? position = new Vector2?(this.m_currentPosition + new Vector2(0.015f, 0.0f));
      Vector2? size = new Vector2?();
      string text1 = text;
      nullable = color;
      Vector4? colorMask = new Vector4?(nullable ?? MyGuiScreenDebugBase.m_defaultTextColor);
      double num = 0.64000004529953 * (double) this.m_scale;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(position, size, text1, colorMask, (float) num, "Debug");
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_maxWidth = Math.Max(this.m_maxWidth, myGuiControlLabel.GetTextSize().X + 0.02f);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_currentPosition.Y += controlSliderBase1.Size.Y + this.Spacing;
      return controlSliderBase1;
    }

    private MyGuiControlSlider AddSlider(
      string text,
      float valueMin,
      float valueMax,
      Vector4? color = null)
    {
      Vector2? position1 = new Vector2?(this.m_currentPosition);
      float num1 = 460f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
      double num2 = (double) valueMin;
      double num3 = (double) valueMax;
      double num4 = (double) num1;
      float? defaultValue = new float?();
      Vector4? color1 = new Vector4?();
      string labelText = new StringBuilder(" {0}").ToString();
      double num5 = 0.75 * (double) this.m_scale;
      MyGuiControlSlider guiControlSlider1 = new MyGuiControlSlider(position1, (float) num2, (float) num3, (float) num4, defaultValue, color1, labelText, 3, (float) num5, labelFont: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, showLabel: true);
      guiControlSlider1.DebugScale = this.m_sliderDebugScale;
      MyGuiControlSlider guiControlSlider2 = guiControlSlider1;
      Vector4? nullable = color;
      Vector4 vector4 = nullable ?? MyGuiScreenDebugBase.m_defaultColor;
      guiControlSlider2.ColorMask = vector4;
      this.Controls.Add((MyGuiControlBase) guiControlSlider1);
      Vector2? position2 = new Vector2?(this.m_currentPosition + new Vector2(0.015f, 0.0f));
      Vector2? size = new Vector2?();
      string text1 = text;
      nullable = color;
      Vector4? colorMask = new Vector4?(nullable ?? MyGuiScreenDebugBase.m_defaultTextColor);
      double num6 = 0.64000004529953 * (double) this.m_scale;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(position2, size, text1, colorMask, (float) num6, "Debug");
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_maxWidth = Math.Max(this.m_maxWidth, myGuiControlLabel.GetTextSize().X + 0.02f);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_currentPosition.Y += guiControlSlider1.Size.Y + this.Spacing;
      return guiControlSlider1;
    }

    protected MyGuiControlSlider AddSlider(
      string text,
      float value,
      float valueMin,
      float valueMax,
      Vector4? color = null)
    {
      MyGuiControlSlider guiControlSlider = this.AddSlider(text, valueMin, valueMax, color);
      guiControlSlider.Value = value;
      return guiControlSlider;
    }

    protected MyGuiControlSlider AddSlider(
      string text,
      float value,
      float valueMin,
      float valueMax,
      Action<MyGuiControlSlider> valueChange,
      Vector4? color = null)
    {
      MyGuiControlSlider guiControlSlider = this.AddSlider(text, valueMin, valueMax, color);
      guiControlSlider.Value = value;
      guiControlSlider.ValueChanged = valueChange;
      guiControlSlider.ValueChanged += new Action<MyGuiControlSlider>(this.ValueChanged);
      return guiControlSlider;
    }

    protected MyGuiControlSlider AddSlider(
      string text,
      float valueMin,
      float valueMax,
      Func<float> getter,
      Action<float> setter,
      Vector4? color = null)
    {
      MyGuiControlSlider guiControlSlider = this.AddSlider(text, valueMin, valueMax, color);
      guiControlSlider.Value = getter();
      guiControlSlider.UserData = (object) setter;
      guiControlSlider.ValueChanged = (Action<MyGuiControlSlider>) (sender =>
      {
        ((Action<float>) sender.UserData)(sender.Value);
        this.ValueChanged((MyGuiControlBase) sender);
      });
      return guiControlSlider;
    }

    protected MyGuiControlSlider AddSlider(
      string text,
      float valueMin,
      float valueMax,
      float valueDefault,
      Func<float> getter,
      Action<float> setter,
      Vector4? color = null)
    {
      MyGuiControlSlider guiControlSlider = this.AddSlider(text, valueMin, valueMax, color);
      if (getter != null)
        guiControlSlider.Value = getter();
      guiControlSlider.UserData = (object) setter;
      guiControlSlider.DefaultValue = new float?(valueDefault);
      guiControlSlider.ValueChanged = (Action<MyGuiControlSlider>) (sender =>
      {
        Action<float> userData = (Action<float>) sender.UserData;
        if (userData != null)
          userData(sender.Value);
        this.ValueChanged((MyGuiControlBase) sender);
      });
      return guiControlSlider;
    }

    protected MyGuiControlSliderBase AddSlider(
      string text,
      MyGuiSliderProperties properties,
      Func<float> getter,
      Action<float> setter,
      Vector4? color = null)
    {
      MyGuiControlSliderBase controlSliderBase = this.AddSliderBase(text, properties, color);
      controlSliderBase.Value = getter();
      controlSliderBase.UserData = (object) setter;
      controlSliderBase.ValueChanged = (Action<MyGuiControlSliderBase>) (sender =>
      {
        ((Action<float>) sender.UserData)(sender.Value);
        this.ValueChanged((MyGuiControlBase) sender);
      });
      return controlSliderBase;
    }

    protected MyGuiControlSlider AddSlider(
      string text,
      float valueMin,
      float valueMax,
      object instance,
      MemberInfo memberInfo,
      Vector4? color = null)
    {
      MyGuiControlSlider guiControlSlider = this.AddSlider(text, valueMin, valueMax, color);
      if ((object) (memberInfo as PropertyInfo) != null)
      {
        PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
        guiControlSlider.Value = (float) propertyInfo.GetValue(instance, new object[0]);
        guiControlSlider.UserData = (object) new Tuple<object, PropertyInfo>(instance, propertyInfo);
        guiControlSlider.ValueChanged = (Action<MyGuiControlSlider>) (sender =>
        {
          Tuple<object, PropertyInfo> userData = sender.UserData as Tuple<object, PropertyInfo>;
          userData.Item2.SetValue(userData.Item1, (object) sender.Value, new object[0]);
          this.ValueChanged((MyGuiControlBase) sender);
        });
      }
      else if ((object) (memberInfo as FieldInfo) != null)
      {
        FieldInfo fieldInfo = (FieldInfo) memberInfo;
        guiControlSlider.Value = (float) fieldInfo.GetValue(instance);
        guiControlSlider.UserData = (object) new Tuple<object, FieldInfo>(instance, fieldInfo);
        guiControlSlider.ValueChanged = (Action<MyGuiControlSlider>) (sender =>
        {
          Tuple<object, FieldInfo> userData = sender.UserData as Tuple<object, FieldInfo>;
          userData.Item2.SetValue(userData.Item1, (object) sender.Value);
          this.ValueChanged((MyGuiControlBase) sender);
        });
      }
      return guiControlSlider;
    }

    protected MyGuiControlTextbox AddTextbox(
      string value,
      Action<MyGuiControlTextbox> onTextChanged,
      Vector4? color = null,
      float scale = 1f,
      MyGuiControlTextboxType type = MyGuiControlTextboxType.Normal,
      List<MyGuiControlBase> controlGroup = null,
      string font = "Debug",
      MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP,
      bool addToControls = true)
    {
      MyGuiControlTextbox textbox = new MyGuiControlTextbox(new Vector2?(this.m_currentPosition), value, textColor: color, textScale: scale, type: type);
      textbox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      if (onTextChanged != null)
      {
        textbox.EnterPressed += onTextChanged;
        textbox.FocusChanged += (Action<MyGuiControlBase, bool>) ((_, hasFocus) =>
        {
          if (hasFocus)
            return;
          onTextChanged(textbox);
        });
      }
      if (addToControls)
        this.Controls.Add((MyGuiControlBase) textbox);
      this.m_currentPosition.Y += textbox.Size.Y + 0.01f + this.Spacing;
      controlGroup?.Add((MyGuiControlBase) textbox);
      return textbox;
    }

    protected MyGuiControlLabel AddLabel(
      string text,
      Vector4 color,
      float scale,
      List<MyGuiControlBase> controlGroup = null,
      string font = "Debug")
    {
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(this.m_currentPosition), text: text, colorMask: new Vector4?(color), textScale: (0.8f * MyGuiConstants.DEBUG_LABEL_TEXT_SCALE * scale * this.m_scale), font: font);
      myGuiControlLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_maxWidth = Math.Max(this.m_maxWidth, myGuiControlLabel.GetTextSize().X + 0.02f);
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_currentPosition.Y += myGuiControlLabel.Size.Y + this.Spacing;
      controlGroup?.Add((MyGuiControlBase) myGuiControlLabel);
      return myGuiControlLabel;
    }

    protected MyGuiControlLabel AddSubcaption(
      MyStringId textEnum,
      Vector4? captionTextColor = null,
      Vector2? captionOffset = null,
      float captionScale = 0.8f)
    {
      return this.AddSubcaption(MyTexts.GetString(textEnum), captionTextColor, captionOffset, captionScale);
    }

    protected MyGuiControlLabel AddSubcaption(
      string text,
      Vector4? captionTextColor = null,
      Vector2? captionOffset = null,
      float captionScale = 0.8f)
    {
      float num = !this.m_size.HasValue ? 0.0f : this.m_size.Value.X / 2f;
      this.m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y;
      this.m_currentPosition.X += num;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(this.m_currentPosition + (captionOffset.HasValue ? captionOffset.Value : Vector2.Zero)), text: text, colorMask: new Vector4?(captionTextColor ?? MyGuiScreenDebugBase.m_defaultColor), textScale: captionScale, font: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      this.Elements.Add((MyGuiControlBase) myGuiControlLabel);
      this.m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y + this.Spacing;
      this.m_currentPosition.X -= num;
      return myGuiControlLabel;
    }

    private MyGuiControlColor AddColor(string text)
    {
      MyGuiControlColor myGuiControlColor = new MyGuiControlColor(text, this.m_scale, this.m_currentPosition, Color.White, Color.White, MyCommonTexts.DialogAmount_AddAmountCaption, font: "Debug");
      myGuiControlColor.ColorMask = Color.Yellow.ToVector4();
      myGuiControlColor.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.Controls.Add((MyGuiControlBase) myGuiControlColor);
      this.m_currentPosition.Y += myGuiControlColor.Size.Y;
      return myGuiControlColor;
    }

    protected MyGuiControlColor AddColor(
      string text,
      Func<Color> getter,
      Action<Color> setter)
    {
      return this.AddColor(text, getter(), (Action<MyGuiControlColor>) (c => setter(c.GetColor())));
    }

    protected MyGuiControlColor AddColor(
      string text,
      Color value,
      Action<MyGuiControlColor> setter)
    {
      MyGuiControlColor colorControl = this.AddColor(text);
      colorControl.SetColor(value);
      colorControl.OnChange += (Action<MyGuiControlColor>) (sender => setter(colorControl));
      return colorControl;
    }

    protected MyGuiControlColor AddColor(
      string text,
      object instance,
      MemberInfo memberInfo)
    {
      MyGuiControlColor myGuiControlColor = this.AddColor(text);
      if ((object) (memberInfo as PropertyInfo) != null)
      {
        PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
        switch (propertyInfo.GetValue(instance, new object[0]))
        {
          case Color color:
            myGuiControlColor.SetColor(color);
            break;
          case Vector3 color:
            myGuiControlColor.SetColor(color);
            break;
          case Vector4 color:
            myGuiControlColor.SetColor(color);
            break;
        }
        myGuiControlColor.UserData = (object) new Tuple<object, PropertyInfo>(instance, propertyInfo);
        myGuiControlColor.OnChange += (Action<MyGuiControlColor>) (sender =>
        {
          Tuple<object, PropertyInfo> userData = sender.UserData as Tuple<object, PropertyInfo>;
          if (userData.Item2.MemberType.GetType() == typeof (Color))
          {
            userData.Item2.SetValue(userData.Item1, (object) sender.GetColor(), new object[0]);
            this.ValueChanged((MyGuiControlBase) sender);
          }
          else if (userData.Item2.MemberType.GetType() == typeof (Vector3))
          {
            userData.Item2.SetValue(userData.Item1, (object) sender.GetColor().ToVector3(), new object[0]);
            this.ValueChanged((MyGuiControlBase) sender);
          }
          else
          {
            if (!(userData.Item2.MemberType.GetType() == typeof (Vector4)))
              return;
            userData.Item2.SetValue(userData.Item1, (object) sender.GetColor().ToVector4(), new object[0]);
            this.ValueChanged((MyGuiControlBase) sender);
          }
        });
      }
      else if ((object) (memberInfo as FieldInfo) != null)
      {
        FieldInfo fieldInfo = (FieldInfo) memberInfo;
        switch (fieldInfo.GetValue(instance))
        {
          case Color color:
            myGuiControlColor.SetColor(color);
            break;
          case Vector3 color:
            myGuiControlColor.SetColor(color);
            break;
          case Vector4 color:
            myGuiControlColor.SetColor(color);
            break;
        }
        myGuiControlColor.UserData = (object) new Tuple<object, FieldInfo>(instance, fieldInfo);
        myGuiControlColor.OnChange += (Action<MyGuiControlColor>) (sender =>
        {
          Tuple<object, FieldInfo> userData = sender.UserData as Tuple<object, FieldInfo>;
          if (userData.Item2.FieldType == typeof (Color))
          {
            userData.Item2.SetValue(userData.Item1, (object) sender.GetColor());
            this.ValueChanged((MyGuiControlBase) sender);
          }
          else if (userData.Item2.FieldType == typeof (Vector3))
          {
            userData.Item2.SetValue(userData.Item1, (object) sender.GetColor().ToVector3());
            this.ValueChanged((MyGuiControlBase) sender);
          }
          else
          {
            if (!(userData.Item2.FieldType == typeof (Vector4)))
              return;
            userData.Item2.SetValue(userData.Item1, (object) sender.GetColor().ToVector4());
            this.ValueChanged((MyGuiControlBase) sender);
          }
        });
      }
      return myGuiControlColor;
    }

    public MyGuiControlButton AddButton(
      string text,
      Action<MyGuiControlButton> onClick,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? textColor = null,
      Vector2? size = null)
    {
      return this.AddButton(new StringBuilder(text), onClick, controlGroup, textColor, size);
    }

    public MyGuiControlButton AddButton(
      StringBuilder text,
      Action<MyGuiControlButton> onClick,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? textColor = null,
      Vector2? size = null,
      bool increaseSpacing = true,
      bool addToControls = true)
    {
      MyGuiControlButton guiControlButton = new MyGuiControlButton(new Vector2?(new Vector2(this.m_buttonXOffset, this.m_currentPosition.Y)), MyGuiControlButtonStyleEnum.Debug, colorMask: new Vector4?(MyGuiScreenDebugBase.m_defaultColor), text: text, textScale: (0.8f * MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * this.m_scale), onButtonClick: onClick);
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      if (addToControls)
        this.Controls.Add((MyGuiControlBase) guiControlButton);
      if (increaseSpacing)
        this.m_currentPosition.Y += guiControlButton.Size.Y + 0.01f + this.Spacing;
      controlGroup?.Add((MyGuiControlBase) guiControlButton);
      return guiControlButton;
    }

    protected MyGuiControlCombobox AddCombo(
      List<MyGuiControlBase> controlGroup = null,
      Vector4? textColor = null,
      Vector2? size = null,
      int openAreaItemsCount = 10,
      bool addToControls = true,
      Vector2? overridePos = null,
      bool isAutoscaleEnabled = false,
      bool isAutoEllipsisEnabled = false)
    {
      Vector2? position = new Vector2?(!overridePos.HasValue || !overridePos.HasValue ? this.m_currentPosition : overridePos.Value);
      Vector2? size1 = size;
      Vector4? backgroundColor = new Vector4?();
      Vector2? textOffset = new Vector2?();
      Vector4? nullable = textColor;
      int openAreaItemsCount1 = openAreaItemsCount;
      Vector2? iconSize = new Vector2?();
      Vector4? textColor1 = nullable;
      int num1 = isAutoscaleEnabled ? 1 : 0;
      int num2 = isAutoEllipsisEnabled ? 1 : 0;
      MyGuiControlCombobox guiControlCombobox1 = new MyGuiControlCombobox(position, size1, backgroundColor, textOffset, openAreaItemsCount1, iconSize, textColor: textColor1, isAutoscaleEnabled: (num1 != 0), isAutoEllipsisEnabled: (num2 != 0));
      guiControlCombobox1.VisualStyle = MyGuiControlComboboxStyleEnum.Debug;
      guiControlCombobox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlCombobox guiControlCombobox2 = guiControlCombobox1;
      if (addToControls)
        this.Controls.Add((MyGuiControlBase) guiControlCombobox2);
      if (!overridePos.HasValue || !overridePos.HasValue)
        this.m_currentPosition.Y += guiControlCombobox2.Size.Y + 0.01f + this.Spacing;
      controlGroup?.Add((MyGuiControlBase) guiControlCombobox2);
      return guiControlCombobox2;
    }

    protected MyGuiControlCombobox AddCombo<TEnum>(
      TEnum selectedItem,
      Action<TEnum> valueChanged,
      bool enabled = true,
      int openAreaItemsCount = 10,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      bool isAutoscaleEnabled = false,
      bool isAutoEllipsisEnabled = false)
      where TEnum : struct, IComparable, IFormattable, IConvertible
    {
      MyGuiControlCombobox combobox = this.AddCombo(controlGroup, color, openAreaItemsCount: openAreaItemsCount, isAutoscaleEnabled: isAutoscaleEnabled, isAutoEllipsisEnabled: isAutoEllipsisEnabled);
      foreach (TEnum @enum in MyEnum<TEnum>.Values)
        combobox.AddItem((long) (int) (ValueType) @enum, new StringBuilder(MyTexts.TrySubstitute(@enum.ToString())));
      combobox.SelectItemByKey((long) (int) (ValueType) selectedItem);
      combobox.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => valueChanged(MyEnum<TEnum>.SetValue((ulong) combobox.GetSelectedKey())));
      return combobox;
    }

    protected MyGuiControlCombobox AddCombo<TEnum>(
      object instance,
      MemberInfo memberInfo,
      bool enabled = true,
      int openAreaItemsCount = 10,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? color = null,
      bool isAutoscaleEnabled = false,
      bool isAutoEllipsisEnabled = false)
      where TEnum : struct, IComparable, IFormattable, IConvertible
    {
      MyGuiControlCombobox combobox = this.AddCombo(controlGroup, color, openAreaItemsCount: openAreaItemsCount, isAutoscaleEnabled: isAutoscaleEnabled, isAutoEllipsisEnabled: isAutoEllipsisEnabled);
      foreach (TEnum @enum in MyEnum<TEnum>.Values)
      {
        MyGuiControlCombobox guiControlCombobox = combobox;
        // ISSUE: reference to a compiler-generated field
        if (MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, int>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (MyGuiScreenDebugBase)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        long key = (long) MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__0.Target((CallSite) MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__0, (object) @enum);
        StringBuilder stringBuilder = new StringBuilder(@enum.ToString());
        int? sortOrder = new int?();
        guiControlCombobox.AddItem(key, stringBuilder, sortOrder);
      }
      if ((object) (memberInfo as PropertyInfo) != null)
      {
        PropertyInfo property = memberInfo as PropertyInfo;
        MyGuiControlCombobox guiControlCombobox = combobox;
        // ISSUE: reference to a compiler-generated field
        if (MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, int>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (MyGuiScreenDebugBase)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        long key = (long) MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__1.Target((CallSite) MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__1, property.GetValue(instance, new object[0]));
        guiControlCombobox.SelectItemByKey(key);
        combobox.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => property.SetValue(instance, Enum.ToObject(typeof (TEnum), combobox.GetSelectedKey()), new object[0]));
      }
      else if ((object) (memberInfo as FieldInfo) != null)
      {
        FieldInfo field = memberInfo as FieldInfo;
        MyGuiControlCombobox guiControlCombobox = combobox;
        // ISSUE: reference to a compiler-generated field
        if (MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, int>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (int), typeof (MyGuiScreenDebugBase)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        long key = (long) MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__2.Target((CallSite) MyGuiScreenDebugBase.\u003C\u003Eo__41<TEnum>.\u003C\u003Ep__2, field.GetValue(instance));
        guiControlCombobox.SelectItemByKey(key);
        combobox.ItemSelected += (MyGuiControlCombobox.ItemSelectedDelegate) (() => field.SetValue(instance, Enum.ToObject(typeof (TEnum), combobox.GetSelectedKey())));
      }
      return combobox;
    }

    protected MyGuiControlListbox AddListBox(
      float verticalSize,
      List<MyGuiControlBase> controlGroup = null)
    {
      MyGuiControlListbox guiControlListbox1 = new MyGuiControlListbox(new Vector2?(this.m_currentPosition));
      guiControlListbox1.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      MyGuiControlListbox guiControlListbox2 = guiControlListbox1;
      guiControlListbox2.Size = new Vector2(guiControlListbox2.Size.X, verticalSize);
      this.Controls.Add((MyGuiControlBase) guiControlListbox2);
      this.m_currentPosition.Y += guiControlListbox2.Size.Y + 0.01f + this.Spacing;
      controlGroup?.Add((MyGuiControlBase) guiControlListbox2);
      return guiControlListbox2;
    }

    protected void AddShareFocusHint() => this.Controls.Add((MyGuiControlBase) new MyGuiControlLabel(new Vector2?(new Vector2(0.01f, (float) (-(double) this.m_size.Value.Y / 2.0 + 0.0700000002980232))), text: "(press ALT to share focus)", colorMask: new Vector4?(Color.Yellow.ToVector4()), textScale: 0.56f, font: "Debug", originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP));

    protected void AddVerticalSpacing(float value = 0.01f) => this.m_currentPosition.Y += value;

    public override bool Draw() => MyGuiSandbox.IsDebugScreenEnabled() && base.Draw();
  }
}
