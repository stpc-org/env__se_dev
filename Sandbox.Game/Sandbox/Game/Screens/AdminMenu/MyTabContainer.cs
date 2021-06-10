// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.AdminMenu.MyTabContainer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
  internal abstract class MyTabContainer
  {
    protected const float TEXT_SCALE = 0.8f;
    protected const float HIDDEN_PART_RIGHT = 0.04f;
    protected static readonly Vector4 DEFAULT_COLOR = Color.Yellow.ToVector4();
    protected static readonly Vector4 LABEL_COLOR = Color.White.ToVector4();
    protected MyGuiScreenBase m_parentScreen;

    internal MyGuiControlParent Control { get; }

    public MyTabContainer(MyGuiScreenBase parentScreen)
    {
      this.m_parentScreen = parentScreen;
      this.Control = new MyGuiControlParent();
      this.Control.Position = Vector2.Zero;
      this.Control.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_parentScreen.Controls.Add((MyGuiControlBase) this.Control);
    }

    internal abstract bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings settings);

    protected MyGuiControlTextbox AddTextbox(
      ref Vector2 currentPosition,
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
      MyGuiControlTextbox guiControlTextbox = new MyGuiControlTextbox(new Vector2?(currentPosition), value, 6, color, scale, type);
      guiControlTextbox.OriginAlign = originAlign;
      if (onTextChanged != null)
        guiControlTextbox.EnterPressed += onTextChanged;
      if (addToControls)
        this.m_parentScreen.Controls.Add((MyGuiControlBase) guiControlTextbox);
      controlGroup?.Add((MyGuiControlBase) guiControlTextbox);
      return guiControlTextbox;
    }

    public MyGuiControlButton AddButton(
      ref Vector2 currentPosition,
      string text,
      Action<MyGuiControlButton> onClick,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? textColor = null,
      Vector2? size = null)
    {
      return this.AddButton(ref currentPosition, new StringBuilder(text), onClick, controlGroup, textColor, size);
    }

    public MyGuiControlButton AddButton(
      ref Vector2 currentPosition,
      StringBuilder text,
      Action<MyGuiControlButton> onClick,
      List<MyGuiControlBase> controlGroup = null,
      Vector4? textColor = null,
      Vector2? size = null,
      bool increaseSpacing = true,
      bool addToControls = true,
      bool autoScaleEnabled = false,
      bool isAutoEllipsisEnabled = false)
    {
      MyGuiControlButton guiControlButton = new MyGuiControlButton(new Vector2?(currentPosition), MyGuiControlButtonStyleEnum.Debug, colorMask: new Vector4?(MyTabContainer.DEFAULT_COLOR), text: text, textScale: ((float) (0.800000011920929 * (double) MyGuiConstants.DEBUG_BUTTON_TEXT_SCALE * 0.800000011920929)), onButtonClick: onClick, isAutoscaleEnabled: autoScaleEnabled, isEllipsisEnabled: isAutoEllipsisEnabled);
      guiControlButton.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
      if (addToControls)
        this.m_parentScreen.Controls.Add((MyGuiControlBase) guiControlButton);
      if (increaseSpacing)
        currentPosition.Y += guiControlButton.Size.Y + 0.01f;
      controlGroup?.Add((MyGuiControlBase) guiControlButton);
      return guiControlButton;
    }

    protected MyGuiControlButton CreateDebugButton(
      ref Vector2 currentPosition,
      float usableWidth,
      MyStringId text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null,
      bool increaseSpacing = true,
      bool addToControls = true,
      bool isAutoScaleEnabled = false,
      bool isAutoEllipsisEnabled = false)
    {
      MyGuiControlButton guiControlButton = this.AddButton(ref currentPosition, MyTexts.Get(text), onClick, increaseSpacing: increaseSpacing, addToControls: addToControls, isAutoEllipsisEnabled: isAutoEllipsisEnabled);
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton.TextScale = 0.8f;
      guiControlButton.IsAutoScaleEnabled = isAutoScaleEnabled;
      guiControlButton.Size = new Vector2(usableWidth, guiControlButton.Size.Y);
      guiControlButton.Enabled = enabled;
      if (tooltip.HasValue)
        guiControlButton.SetToolTip(tooltip.Value);
      return guiControlButton;
    }
  }
}
