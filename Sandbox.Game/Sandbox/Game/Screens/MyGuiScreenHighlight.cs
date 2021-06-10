// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenHighlight
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics.GUI;
using System.Collections.Generic;
using VRage.Input;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenHighlight : MyGuiScreenBase
  {
    private uint m_closeInFrames = uint.MaxValue;
    private readonly MyGuiControls m_highlightedControls;
    private readonly MyGuiScreenHighlight.MyHighlightControl[] m_highlightedControlsData;
    private static readonly Vector2 HIGHLIGHT_TEXTURE_SIZE;
    private static readonly Vector2 HIGHLIGHT_TEXTURE_OFFSET;

    public override string GetFriendlyName() => "HighlightScreen";

    public override int GetTransitionOpeningTime() => 500;

    public override int GetTransitionClosingTime() => 500;

    public static void HighlightControls(
      MyGuiScreenHighlight.MyHighlightControl[] controlsData)
    {
      MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenHighlight(controlsData));
    }

    public static void HighlightControl(MyGuiScreenHighlight.MyHighlightControl control) => MyGuiScreenHighlight.HighlightControls(new MyGuiScreenHighlight.MyHighlightControl[1]
    {
      control
    });

    private MyGuiScreenHighlight(
      MyGuiScreenHighlight.MyHighlightControl[] controlsData)
      : base(new Vector2?(Vector2.Zero), size: new Vector2?(Vector2.One * 2.5f))
    {
      this.m_highlightedControlsData = controlsData;
      this.m_highlightedControls = new MyGuiControls((IMyGuiControlsOwner) this);
      foreach (MyGuiScreenHighlight.MyHighlightControl highlightControl in this.m_highlightedControlsData)
      {
        if (highlightControl.CustomToolTips != null)
        {
          highlightControl.CustomToolTips.Highlight = true;
          highlightControl.CustomToolTips.HighlightColor = (Vector4) (highlightControl.Color ?? Color.Yellow);
        }
        this.m_highlightedControls.AddWeak(highlightControl.Control);
      }
      this.m_backgroundColor = new Vector4?((Vector4.One * 0.86f).ToSRGB());
      this.m_backgroundFadeColor = (Color) (Vector4.One * 0.86f).ToSRGB();
      this.CanBeHidden = false;
      this.CanHaveFocus = true;
      this.m_canShareInput = false;
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = true;
      this.DrawMouseCursor = true;
      this.CloseButtonEnabled = false;
    }

    public override MyGuiControls Controls => this.m_highlightedControls;

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      this.UniversalInputHandling();
      foreach (MyGuiControlBase highlightedControl in this.m_highlightedControls)
      {
        highlightedControl.IsMouseOver = MyGuiControlBase.CheckMouseOver(highlightedControl.Size, highlightedControl.GetPositionAbsolute(), highlightedControl.OriginAlign);
        for (MyGuiControlBase owner = highlightedControl.Owner as MyGuiControlBase; owner != null; owner = owner.Owner as MyGuiControlBase)
          owner.IsMouseOver = MyGuiControlBase.CheckMouseOver(owner.Size, owner.GetPositionAbsolute(), owner.OriginAlign);
        if (this.m_closeInFrames == uint.MaxValue && highlightedControl.IsMouseOver && MyInput.Static.IsNewLeftMousePressed())
          this.m_closeInFrames = 10U;
      }
      base.HandleInput(receivedFocusInThisUpdate);
      if (this.m_closeInFrames == 0U)
      {
        this.CloseScreen(false);
      }
      else
      {
        if (this.m_closeInFrames >= uint.MaxValue)
          return;
        --this.m_closeInFrames;
      }
    }

    public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
    {
      this.UniversalInputHandling();
      base.HandleUnhandledInput(receivedFocusInThisUpdate);
    }

    public override bool Draw()
    {
      foreach (MyGuiScreenHighlight.MyHighlightControl highlightControl in this.m_highlightedControlsData)
      {
        if (highlightControl.Control is MyGuiControlGrid control)
        {
          if (control.ModalItems == null)
            control.ModalItems = new Dictionary<int, Color>();
          else
            control.ModalItems.Clear();
          if (highlightControl.Indices != null)
          {
            foreach (int index in highlightControl.Indices)
              control.ModalItems.Add(index, highlightControl.Color.HasValue ? highlightControl.Color.Value : Color.Yellow);
          }
        }
      }
      base.Draw();
      foreach (MyGuiScreenHighlight.MyHighlightControl highlightControl in this.m_highlightedControlsData)
      {
        if (highlightControl.Control is MyGuiControlGrid control && control.ModalItems != null)
          control.ModalItems.Clear();
        foreach (MyGuiControlBase element in highlightControl.Control.Elements)
        {
          if (element is MyGuiControlGrid myGuiControlGrid && myGuiControlGrid.ModalItems != null)
            myGuiControlGrid.ModalItems.Clear();
        }
      }
      foreach (MyGuiScreenHighlight.MyHighlightControl highlightControl in this.m_highlightedControlsData)
      {
        if (this.State == MyGuiScreenState.OPENED && highlightControl.CustomToolTips != null)
        {
          Vector2 absoluteTopRight = highlightControl.Control.GetPositionAbsoluteTopRight();
          absoluteTopRight.Y -= highlightControl.CustomToolTips.Size.Y + 0.045f;
          absoluteTopRight.X -= 0.01f;
          highlightControl.CustomToolTips.Draw(absoluteTopRight);
        }
        if (!(highlightControl.Control is MyGuiControlGrid) && !(highlightControl.Control is MyGuiControlGridDragAndDrop))
        {
          MyGuiControlBase control = highlightControl.Control;
          Vector2 size = control.Size + MyGuiScreenHighlight.HIGHLIGHT_TEXTURE_SIZE;
          Vector2 positionLeftTop = control.GetPositionAbsoluteTopLeft() - MyGuiScreenHighlight.HIGHLIGHT_TEXTURE_OFFSET;
          Color colorMask = highlightControl.Color.HasValue ? highlightControl.Color.Value : Color.Yellow;
          colorMask.A = (byte) ((double) colorMask.A * (double) this.m_transitionAlpha);
          MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.Draw(positionLeftTop, size, colorMask);
          control.Draw(this.m_transitionAlpha, this.m_backgroundTransition);
        }
      }
      return true;
    }

    private void UniversalInputHandling()
    {
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.Escape))
        return;
      this.CloseScreen(false);
    }

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.m_highlightedControls.ClearWeaks();
      return base.CloseScreen(isUnloading);
    }

    static MyGuiScreenHighlight()
    {
      MyGuiSizedTexture myGuiSizedTexture1 = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.LeftCenter;
      double x1 = (double) myGuiSizedTexture1.SizeGui.X;
      myGuiSizedTexture1 = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.RightCenter;
      double x2 = (double) myGuiSizedTexture1.SizeGui.X;
      double num1 = x1 + x2;
      myGuiSizedTexture1 = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.CenterTop;
      double y1 = (double) myGuiSizedTexture1.SizeGui.Y;
      myGuiSizedTexture1 = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.CenterBottom;
      double y2 = (double) myGuiSizedTexture1.SizeGui.Y;
      double num2 = y1 + y2;
      MyGuiScreenHighlight.HIGHLIGHT_TEXTURE_SIZE = new Vector2((float) num1, (float) num2);
      MyGuiSizedTexture myGuiSizedTexture2 = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.LeftCenter;
      double x3 = (double) myGuiSizedTexture2.SizeGui.X;
      myGuiSizedTexture2 = MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.CenterTop;
      double y3 = (double) myGuiSizedTexture2.SizeGui.Y;
      MyGuiScreenHighlight.HIGHLIGHT_TEXTURE_OFFSET = new Vector2((float) x3, (float) y3);
    }

    public struct MyHighlightControl
    {
      public MyGuiControlBase Control;
      public int[] Indices;
      public Color? Color;
      public MyToolTips CustomToolTips;
    }
  }
}
