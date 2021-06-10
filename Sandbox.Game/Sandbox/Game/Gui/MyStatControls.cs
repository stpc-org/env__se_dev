// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatControls
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.GUI;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
  public class MyStatControls
  {
    private readonly List<MyStatControls.StatBinding> m_bindings = new List<MyStatControls.StatBinding>();
    private readonly Dictionary<VisualStyleCategory, float> m_alphaMultipliersByCategory = new Dictionary<VisualStyleCategory, float>();
    private MyObjectBuilder_StatControls m_objectBuilder;
    private float m_resolutionScaleRatio;
    private double m_lastDrawTimeMs;
    private IMyHudStat m_showStatesStat;
    private float m_uiScaleFactor;
    private Vector2 m_position;

    public float ChildrenScaleFactor => this.m_uiScaleFactor;

    public MyStatControls(MyObjectBuilder_StatControls ob, float uiScale = 1f)
    {
      this.m_objectBuilder = ob;
      this.m_uiScaleFactor = uiScale;
      if (this.m_objectBuilder.StatStyles != null)
      {
        foreach (MyObjectBuilder_StatVisualStyle statStyle in this.m_objectBuilder.StatStyles)
        {
          if (statStyle.StatId != MyStringHash.NullOrEmpty)
          {
            IMyHudStat stat = MyHud.Stats.GetStat(statStyle.StatId);
            if (stat != null)
              this.AddControl(stat, statStyle);
          }
          else
            this.AddControl((IMyHudStat) null, statStyle);
        }
      }
      if (ob.VisibleCondition != null)
        MyStatControls.InitConditions(ob.VisibleCondition);
      this.m_showStatesStat = MyHud.Stats.GetStat(MyStringHash.GetOrCompute("hud_show_states"));
      this.m_lastDrawTimeMs = MySession.Static.ElapsedGameTime.TotalMilliseconds;
      foreach (VisualStyleCategory enumValue in typeof (VisualStyleCategory).GetEnumValues())
        this.m_alphaMultipliersByCategory[enumValue] = 1f;
    }

    public Vector2 Position
    {
      get => this.m_position;
      set
      {
        this.OnPositionChanged(this.m_position, value);
        this.m_position = value;
      }
    }

    private void InitStatControl(
      IMyStatControl control,
      IMyHudStat stat,
      MyObjectBuilder_StatVisualStyle style)
    {
      if (stat != null)
      {
        control.StatMaxValue = stat.MaxValue;
        control.StatMinValue = stat.MinValue;
        control.StatCurrent = stat.CurrentValue;
        control.StatString = stat.GetValueString();
      }
      if (style.Blink != null)
      {
        control.BlinkBehavior.Blink = style.Blink.Blink;
        control.BlinkBehavior.IntervalMs = style.Blink.IntervalMs;
        control.BlinkBehavior.MinAlpha = style.Blink.MinAlpha;
        control.BlinkBehavior.MaxAlpha = style.Blink.MaxAlpha;
        if (style.Blink.ColorMask.HasValue)
          control.BlinkBehavior.ColorMask = style.Blink.ColorMask;
      }
      if (style.FadeInTimeMs.HasValue)
        control.FadeInTimeMs = style.FadeInTimeMs.Value;
      if (style.FadeOutTimeMs.HasValue)
        control.FadeOutTimeMs = style.FadeOutTimeMs.Value;
      if (style.MaxOnScreenTimeMs.HasValue)
        control.MaxOnScreenTimeMs = style.MaxOnScreenTimeMs.Value;
      if (style.BlinkCondition != null)
        MyStatControls.InitConditions(style.BlinkCondition);
      if (style.VisibleCondition != null)
        MyStatControls.InitConditions(style.VisibleCondition);
      if (style.Category.HasValue)
        control.Category = style.Category.Value;
      else
        style.Category = new VisualStyleCategory?(VisualStyleCategory.None);
    }

    private static void InitConditions(ConditionBase conditionBase)
    {
      switch (conditionBase)
      {
        case StatCondition statCondition:
          IMyHudStat stat = MyHud.Stats.GetStat(statCondition.StatId);
          statCondition.SetStat(stat);
          break;
        case Condition condition:
          foreach (ConditionBase term in condition.Terms)
            MyStatControls.InitConditions(term);
          break;
      }
    }

    private void AddControl(IMyHudStat stat, MyObjectBuilder_StatVisualStyle style)
    {
      IMyStatControl control = (IMyStatControl) null;
      switch (style)
      {
        case MyObjectBuilder_CircularProgressBarStatVisualStyle _:
          control = this.InitCircularProgressBar((MyObjectBuilder_CircularProgressBarStatVisualStyle) style);
          break;
        case MyObjectBuilder_ProgressBarStatVisualStyle _:
          control = this.InitProgressBar((MyObjectBuilder_ProgressBarStatVisualStyle) style);
          break;
        case MyObjectBuilder_TextStatVisualStyle _:
          control = this.InitText((MyObjectBuilder_TextStatVisualStyle) style);
          break;
        case MyObjectBuilder_ImageStatVisualStyle _:
          control = this.InitImage((MyObjectBuilder_ImageStatVisualStyle) style);
          break;
      }
      if (control == null)
        return;
      this.InitStatControl(control, stat, style);
      this.m_bindings.Add(new MyStatControls.StatBinding()
      {
        Control = control,
        Style = style,
        Stat = stat
      });
    }

    private IMyStatControl InitCircularProgressBar(
      MyObjectBuilder_CircularProgressBarStatVisualStyle style)
    {
      MyObjectBuilder_GuiTexture texture1 = (MyObjectBuilder_GuiTexture) null;
      if (!MyGuiTextures.Static.TryGetTexture(style.SegmentTexture, out texture1))
        return (IMyStatControl) null;
      MyObjectBuilder_GuiTexture texture2 = (MyObjectBuilder_GuiTexture) null;
      if (style.BackgroudTexture.HasValue)
        MyGuiTextures.Static.TryGetTexture(style.BackgroudTexture.Value, out texture2);
      MyObjectBuilder_GuiTexture texture3 = (MyObjectBuilder_GuiTexture) null;
      if (style.FirstSegmentTexture.HasValue)
        MyGuiTextures.Static.TryGetTexture(style.FirstSegmentTexture.Value, out texture3);
      MyObjectBuilder_GuiTexture texture4 = (MyObjectBuilder_GuiTexture) null;
      if (style.LastSegmentTexture.HasValue)
        MyGuiTextures.Static.TryGetTexture(style.LastSegmentTexture.Value, out texture4);
      MyStatControlCircularProgressBar circularProgressBar1 = new MyStatControlCircularProgressBar(this, texture1, texture2, texture3, texture4);
      circularProgressBar1.Position = this.Position + style.OffsetPx * this.m_uiScaleFactor;
      circularProgressBar1.Size = style.SizePx * this.m_uiScaleFactor;
      Vector2? segmentOrigin = style.SegmentOrigin;
      float uiScaleFactor = this.m_uiScaleFactor;
      circularProgressBar1.SegmentOrigin = (segmentOrigin.HasValue ? new Vector2?(segmentOrigin.GetValueOrDefault() * uiScaleFactor) : new Vector2?()) ?? style.SegmentSizePx * this.m_uiScaleFactor / 2f;
      circularProgressBar1.SegmentSize = style.SegmentSizePx * this.m_uiScaleFactor;
      MyStatControlCircularProgressBar circularProgressBar2 = circularProgressBar1;
      if (style.AngleOffset.HasValue)
        circularProgressBar2.TextureRotationOffset = style.AngleOffset.Value;
      if (style.SpacingAngle.HasValue)
        circularProgressBar2.TextureRotationAngle = style.SpacingAngle.Value;
      if (style.Animate.HasValue)
        circularProgressBar2.Animate = style.Animate.Value;
      if (style.AnimatedSegmentColorMask.HasValue)
        circularProgressBar2.AnimatedSegmentColorMask = style.AnimatedSegmentColorMask.Value;
      if (style.FullSegmentColorMask.HasValue)
        circularProgressBar2.FullSegmentColorMask = style.FullSegmentColorMask.Value;
      if (style.EmptySegmentColorMask.HasValue)
        circularProgressBar2.EmptySegmentColorMask = style.EmptySegmentColorMask.Value;
      if (style.AnimationDelayMs.HasValue)
        circularProgressBar2.AnimationDelay = style.AnimationDelayMs.Value;
      if (style.AnimationSegmentDelayMs.HasValue)
        circularProgressBar2.SegmentAnimationMs = style.AnimationSegmentDelayMs.Value;
      if (style.NumberOfSegments.HasValue)
        circularProgressBar2.NumberOfSegments = style.NumberOfSegments.Value;
      if (style.ShowEmptySegments.HasValue)
        circularProgressBar2.ShowEmptySegments = style.ShowEmptySegments.Value;
      return (IMyStatControl) circularProgressBar2;
    }

    private IMyStatControl InitProgressBar(
      MyObjectBuilder_ProgressBarStatVisualStyle style)
    {
      if (style.NineTiledStyle.HasValue)
      {
        MyObjectBuilder_CompositeTexture texture;
        if (!MyGuiTextures.Static.TryGetCompositeTexture(style.NineTiledStyle.Value.Texture, out texture))
          return (IMyStatControl) null;
        MyStatControlProgressBar controlProgressBar1 = new MyStatControlProgressBar(this, texture);
        controlProgressBar1.Position = this.Position + style.OffsetPx * this.m_uiScaleFactor;
        controlProgressBar1.Size = style.SizePx * this.m_uiScaleFactor;
        MyStatControlProgressBar controlProgressBar2 = controlProgressBar1;
        if (style.NineTiledStyle.Value.ColorMask.HasValue)
          controlProgressBar2.ColorMask = style.NineTiledStyle.Value.ColorMask.Value;
        if (style.Inverted.HasValue)
          controlProgressBar2.Inverted = style.Inverted.Value;
        return (IMyStatControl) controlProgressBar2;
      }
      if (!style.SimpleStyle.HasValue)
        return (IMyStatControl) null;
      MyObjectBuilder_GuiTexture texture1;
      MyObjectBuilder_GuiTexture texture2;
      if (!MyGuiTextures.Static.TryGetTexture(style.SimpleStyle.Value.BackgroundTexture, out texture1) || !MyGuiTextures.Static.TryGetTexture(style.SimpleStyle.Value.ProgressTexture, out texture2))
        return (IMyStatControl) null;
      MyStatControlProgressBar controlProgressBar3 = new MyStatControlProgressBar(this, texture1, texture2, style.SimpleStyle.Value.ProgressTextureOffsetPx, style.SimpleStyle.Value.BackgroundColorMask, style.SimpleStyle.Value.ProgressColorMask);
      controlProgressBar3.Position = this.Position + style.OffsetPx * this.m_uiScaleFactor;
      controlProgressBar3.Size = style.SizePx * this.m_uiScaleFactor;
      MyStatControlProgressBar controlProgressBar4 = controlProgressBar3;
      if (style.Inverted.HasValue)
        controlProgressBar4.Inverted = style.Inverted.Value;
      return (IMyStatControl) controlProgressBar4;
    }

    private IMyStatControl InitText(MyObjectBuilder_TextStatVisualStyle style)
    {
      MyStatControlText myStatControlText1 = new MyStatControlText(this, style.Text);
      myStatControlText1.Position = this.Position + style.OffsetPx * this.m_uiScaleFactor;
      myStatControlText1.Size = style.SizePx * this.m_uiScaleFactor;
      myStatControlText1.Font = style.Font;
      myStatControlText1.Scale = style.Scale * this.m_uiScaleFactor;
      MyStatControlText myStatControlText2 = myStatControlText1;
      if (style.ColorMask.HasValue)
        myStatControlText2.TextColorMask = style.ColorMask.Value;
      if (style.TextAlign.HasValue)
        myStatControlText2.TextAlign = style.TextAlign.Value;
      return (IMyStatControl) myStatControlText2;
    }

    private IMyStatControl InitImage(MyObjectBuilder_ImageStatVisualStyle style)
    {
      MyObjectBuilder_GuiTexture texture;
      if (!MyGuiTextures.Static.TryGetTexture(style.Texture, out texture))
        return (IMyStatControl) null;
      MyStatControlImage statControlImage1 = new MyStatControlImage(this);
      statControlImage1.Size = style.SizePx * this.m_uiScaleFactor;
      statControlImage1.Position = this.Position + style.OffsetPx * this.m_uiScaleFactor;
      MyStatControlImage statControlImage2 = statControlImage1;
      statControlImage2.Texture = texture;
      if (style.ColorMask.HasValue)
        statControlImage2.ColorMask = style.ColorMask.Value;
      return (IMyStatControl) statControlImage2;
    }

    public void Draw(float transitionAlpha, float backgroundTransitionAlpha)
    {
      ConditionBase visibleCondition = this.m_objectBuilder.VisibleCondition;
      if ((visibleCondition != null ? (!visibleCondition.Eval() ? 1 : 0) : 1) != 0)
        return;
      double num1 = MySession.Static.ElapsedGameTime.TotalMilliseconds - this.m_lastDrawTimeMs;
      this.m_lastDrawTimeMs = MySession.Static.ElapsedGameTime.TotalMilliseconds;
      foreach (MyStatControls.StatBinding binding in this.m_bindings)
      {
        IMyStatControl control = binding.Control;
        if (binding.Stat != null)
        {
          control.StatCurrent = binding.Stat.CurrentValue;
          control.StatString = binding.Stat.GetValueString();
          control.StatMaxValue = binding.Stat.MaxValue;
          control.StatMinValue = binding.Stat.MinValue;
        }
        if (binding.Style.BlinkCondition != null)
          control.BlinkBehavior.Blink = binding.Style.BlinkCondition.Eval();
        bool flag = true;
        if (binding.Style.VisibleCondition != null)
          flag = binding.Style.VisibleCondition.Eval();
        binding.Control.SpentInStateTimeMs += (uint) num1;
        float val2 = 1f;
        switch (binding.Control.State)
        {
          case MyStatControlState.FadingOut:
            if (flag && !binding.LastVisibleConditionCheckResult)
            {
              binding.Control.State = MyStatControlState.FadingIn;
              binding.Control.SpentInStateTimeMs = binding.Control.MaxOnScreenTimeMs - binding.Control.SpentInStateTimeMs;
              goto case MyStatControlState.FadingIn;
            }
            else
            {
              if (binding.Control.SpentInStateTimeMs > binding.Control.FadeOutTimeMs)
              {
                binding.Control.State = MyStatControlState.Invisible;
                binding.Control.SpentInStateTimeMs = 0U;
                break;
              }
              val2 = (float) (1.0 - (double) binding.Control.SpentInStateTimeMs / (double) binding.Control.FadeOutTimeMs);
              break;
            }
          case MyStatControlState.FadingIn:
            if (!flag)
            {
              binding.Control.State = MyStatControlState.FadingOut;
              binding.Control.SpentInStateTimeMs = binding.Control.MaxOnScreenTimeMs - binding.Control.SpentInStateTimeMs;
              goto case MyStatControlState.FadingOut;
            }
            else
            {
              if (binding.Control.SpentInStateTimeMs > binding.Control.FadeInTimeMs)
              {
                binding.Control.State = MyStatControlState.Visible;
                binding.Control.SpentInStateTimeMs = 0U;
                break;
              }
              val2 = (float) binding.Control.SpentInStateTimeMs / (float) binding.Control.FadeInTimeMs;
              break;
            }
          case MyStatControlState.Visible:
            if ((!flag || binding.Control.MaxOnScreenTimeMs > 0U && binding.Control.MaxOnScreenTimeMs < binding.Control.SpentInStateTimeMs) && (double) this.m_showStatesStat.CurrentValue <= 0.5)
            {
              binding.Control.State = MyStatControlState.FadingOut;
              binding.Control.SpentInStateTimeMs = 0U;
              break;
            }
            break;
          case MyStatControlState.Invisible:
            if (flag && (!binding.LastVisibleConditionCheckResult || (double) this.m_showStatesStat.CurrentValue >= 0.5))
            {
              binding.Control.State = MyStatControlState.FadingIn;
              binding.Control.SpentInStateTimeMs = 0U;
              val2 = 0.0f;
              break;
            }
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        if ((binding.Control.State & (MyStatControlState.FadingOut | MyStatControlState.FadingIn | MyStatControlState.Visible)) != (MyStatControlState) 0)
        {
          float num2 = Math.Min(transitionAlpha, val2);
          binding.Control.Draw(num2 * this.m_alphaMultipliersByCategory[binding.Control.Category]);
        }
        binding.LastVisibleConditionCheckResult = flag;
      }
    }

    public void RegisterAlphaMultiplier(VisualStyleCategory category, float multiplier) => this.m_alphaMultipliersByCategory[category] = multiplier;

    [Conditional("DEBUG")]
    private void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_HUD)
        return;
      Vector2 pointTo1 = this.Position + new Vector2(50f, 0.0f);
      Vector2 pointTo2 = this.Position + new Vector2(0.0f, 50f);
      MyRenderProxy.DebugDrawLine2D(this.Position, pointTo1, Color.Green, Color.Green);
      MyRenderProxy.DebugDrawLine2D(this.Position, pointTo2, Color.Green, Color.Green);
      foreach (MyStatControls.StatBinding binding in this.m_bindings)
      {
        Vector2 position1 = binding.Control.Position;
        Vector2 position2 = binding.Control.Position;
        position2.X += binding.Control.Size.X;
        Vector2 vector2 = binding.Control.Position + binding.Control.Size;
        Vector2 position3 = binding.Control.Position;
        position3.Y += binding.Control.Size.Y;
        MyRenderProxy.DebugDrawLine2D(position1, position2, Color.Red, Color.Red);
        MyRenderProxy.DebugDrawLine2D(position2, vector2, Color.Red, Color.Red);
        MyRenderProxy.DebugDrawLine2D(vector2, position3, Color.Red, Color.Red);
        MyRenderProxy.DebugDrawLine2D(position3, position1, Color.Red, Color.Red);
      }
    }

    protected void OnPositionChanged(Vector2 oldPosition, Vector2 newPosition)
    {
      Vector2 vector2 = newPosition - oldPosition;
      foreach (MyStatControls.StatBinding binding in this.m_bindings)
        binding.Control.Position += vector2;
    }

    private class StatBinding
    {
      public IMyStatControl Control;
      public IMyHudStat Stat;
      public MyObjectBuilder_StatVisualStyle Style;
      public bool LastVisibleConditionCheckResult;
    }
  }
}
