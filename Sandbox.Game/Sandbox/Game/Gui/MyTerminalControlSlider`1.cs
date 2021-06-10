// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlSlider`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Text;
using VRage;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyTerminalControlSlider<TBlock> : MyTerminalValueControl<TBlock, float>, IMyTerminalControlSlider, IMyTerminalControl, IMyTerminalValueControl<float>, ITerminalProperty, IMyTerminalControlTitleTooltip
    where TBlock : MyTerminalBlock
  {
    public MyStringId Title;
    public MyStringId Tooltip;
    private MyGuiControlSlider m_slider;
    private MyGuiControlBlockProperty m_control;
    private Action<float> m_amountConfirmed;
    public bool AmountDialogEnabled = true;
    public MyTerminalControl<TBlock>.WriterDelegate Writer;
    public MyTerminalControl<TBlock>.WriterDelegate CompactWriter;
    public MyTerminalControl<TBlock>.AdvancedWriterDelegate AdvancedWriter;
    public MyTerminalControlSlider<TBlock>.FloatFunc Normalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((b, f) => f);
    public MyTerminalControlSlider<TBlock>.FloatFunc Denormalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((b, f) => f);
    private float m_halfStepLength;
    private bool m_isAutoScaleEnabled;
    private bool m_isAutoEllipsisEnabled;
    public MyTerminalValueControl<TBlock, float>.GetterDelegate DefaultValueGetter;
    private Action<MyGuiControlSlider> m_valueChanged;

    public float? DefaultValue
    {
      set => this.DefaultValueGetter = value.HasValue ? (MyTerminalValueControl<TBlock, float>.GetterDelegate) (block => value.Value) : (MyTerminalValueControl<TBlock, float>.GetterDelegate) null;
    }

    public string Formatter
    {
      set => this.Writer = value != null ? (MyTerminalControl<TBlock>.WriterDelegate) ((block, result) => result.AppendFormat(value, (object) this.GetValue(block))) : (MyTerminalControl<TBlock>.WriterDelegate) null;
    }

    public MyTerminalControlSlider(
      string id,
      MyStringId title,
      MyStringId tooltip,
      bool isAutoscaleEnabled = false,
      bool isAutoEllipsisEnabled = false)
      : base(id)
    {
      this.Title = title;
      this.Tooltip = tooltip;
      this.m_isAutoEllipsisEnabled = isAutoEllipsisEnabled;
      this.m_isAutoScaleEnabled = isAutoscaleEnabled;
      this.CompactWriter = new MyTerminalControl<TBlock>.WriterDelegate(this.CompactWriterMethod);
      this.m_amountConfirmed = new Action<float>(this.AmountSetter);
      this.Serializer = (MyTerminalValueControl<TBlock, float>.SerializerDelegate) ((BitStream stream, ref float value) => stream.Serialize(ref value));
    }

    protected override MyGuiControlBase CreateGui()
    {
      float preferredControlWidth = MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH;
      this.m_slider = new MyGuiControlSlider(new Vector2?(Vector2.Zero), width: preferredControlWidth);
      this.m_valueChanged = new Action<MyGuiControlSlider>(this.OnValueChange);
      this.m_slider.ValueChanged = this.m_valueChanged;
      this.m_slider.SliderSetValueManual = new Func<MyGuiControlSlider, bool>(this.SliderSetValueManual);
      string title = MyTexts.GetString(this.Title);
      string tooltip = MyTexts.GetString(this.Tooltip);
      MyGuiControlSlider slider = this.m_slider;
      bool autoScaleEnabled = this.m_isAutoScaleEnabled;
      float x = this.m_slider.Size.X;
      int num1 = this.m_isAutoEllipsisEnabled ? 1 : 0;
      int num2 = autoScaleEnabled ? 1 : 0;
      double num3 = (double) x;
      this.m_control = new MyGuiControlBlockProperty(title, tooltip, (MyGuiControlBase) slider, isAutoEllipsisEnabled: (num1 != 0), isAutoScaleEnabled: (num2 != 0), max_Width: ((float) num3));
      this.RecalculateEllipsisAndScale();
      return (MyGuiControlBase) this.m_control;
    }

    public void CompactWriterMethod(TBlock block, StringBuilder appendTo)
    {
      int length = appendTo.Length;
      this.Writer(block, appendTo);
      int index = this.FirstIndexOf(appendTo, length, ".,");
      if (index < 0)
        return;
      this.RemoveNumbersFrom(index, appendTo);
    }

    public void SetMinStep(float step) => this.m_halfStepLength = step / 2f;

    private int FirstIndexOf(StringBuilder sb, int start, string chars, int count = 2147483647)
    {
      int num = Math.Min(start + count, sb.Length);
      for (int index1 = start; index1 < num; ++index1)
      {
        char ch = sb[index1];
        for (int index2 = 0; index2 < chars.Length; ++index2)
        {
          if ((int) ch == (int) chars[index2])
            return index1;
        }
      }
      return -1;
    }

    private void RemoveNumbersFrom(int index, StringBuilder sb)
    {
      sb.Remove(index, 1);
      while (index < sb.Length && (sb[index] >= '0' && sb[index] <= '9' || sb[index] == ' '))
        sb.Remove(index, 1);
      if (sb[0] != '-' || sb[1] != '0')
        return;
      sb.Remove(0, 1);
    }

    public void SetLimits(float min, float max)
    {
      this.Normalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) => MathHelper.Clamp((float) (((double) f - (double) min) / ((double) max - (double) min)), 0.0f, 1f));
      this.Denormalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) => MathHelper.Clamp(min + f * (max - min), min, max));
    }

    public void SetLogLimits(float min, float max)
    {
      this.Normalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) => MathHelper.Clamp(MathHelper.InterpLogInv(f, min, max), 0.0f, 1f));
      this.Denormalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) => MathHelper.Clamp(MathHelper.InterpLog(f, min, max), min, max));
    }

    public void SetDualLogLimits(float absMin, float absMax, float centerBand)
    {
      this.Normalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) => MyTerminalControlSlider<TBlock>.DualLogNormalizer(block, f, absMin, absMax, centerBand));
      this.Denormalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) => MyTerminalControlSlider<TBlock>.DualLogDenormalizer(block, f, absMin, absMax, centerBand));
    }

    private static float DualLogDenormalizer(
      TBlock block,
      float value,
      float min,
      float max,
      float centerBand)
    {
      float num = (float) ((double) value * 2.0 - 1.0);
      return (double) Math.Abs(num) < (double) centerBand ? 0.0f : MathHelper.Clamp(MathHelper.InterpLog((float) (((double) Math.Abs(num) - (double) centerBand) / (1.0 - (double) centerBand)), min, max), min, max) * (float) Math.Sign(num);
    }

    private static float DualLogNormalizer(
      TBlock block,
      float value,
      float min,
      float max,
      float centerBand)
    {
      if ((double) Math.Abs(value) < (double) min)
        return 0.5f;
      float num1 = (float) (0.5 - (double) centerBand / 2.0);
      float num2 = MathHelper.Clamp(MathHelper.InterpLogInv(Math.Abs(value), min, max), 0.0f, 1f) * num1;
      return (double) value >= 0.0 ? num2 + num1 + centerBand : num1 - num2;
    }

    public void SetLimits(
      MyTerminalValueControl<TBlock, float>.GetterDelegate minGetter,
      MyTerminalValueControl<TBlock, float>.GetterDelegate maxGetter)
    {
      this.Normalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) =>
      {
        float num1 = minGetter(block);
        float num2 = maxGetter(block);
        return MathHelper.Clamp((float) (((double) f - (double) num1) / ((double) num2 - (double) num1)), 0.0f, 1f);
      });
      this.Denormalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) =>
      {
        float min = minGetter(block);
        float max = maxGetter(block);
        return MathHelper.Clamp(min + f * (max - min), min, max);
      });
    }

    void IMyTerminalControlSlider.SetLimits(
      Func<IMyTerminalBlock, float> minGetter,
      Func<IMyTerminalBlock, float> maxGetter)
    {
      this.SetLimits(new MyTerminalValueControl<TBlock, float>.GetterDelegate(minGetter.Invoke), new MyTerminalValueControl<TBlock, float>.GetterDelegate(maxGetter.Invoke));
    }

    public void SetLogLimits(
      MyTerminalValueControl<TBlock, float>.GetterDelegate minGetter,
      MyTerminalValueControl<TBlock, float>.GetterDelegate maxGetter)
    {
      this.Normalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) =>
      {
        float amount1 = minGetter(block);
        float amount2 = maxGetter(block);
        return MathHelper.Clamp(MathHelper.InterpLogInv(f, amount1, amount2), 0.0f, 1f);
      });
      this.Denormalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) =>
      {
        float num1 = minGetter(block);
        float num2 = maxGetter(block);
        return MathHelper.Clamp(MathHelper.InterpLog(f, num1, num2), num1, num2);
      });
    }

    void IMyTerminalControlSlider.SetLogLimits(
      Func<IMyTerminalBlock, float> minGetter,
      Func<IMyTerminalBlock, float> maxGetter)
    {
      this.SetLogLimits(new MyTerminalValueControl<TBlock, float>.GetterDelegate(minGetter.Invoke), new MyTerminalValueControl<TBlock, float>.GetterDelegate(maxGetter.Invoke));
    }

    public void SetDualLogLimits(
      MyTerminalValueControl<TBlock, float>.GetterDelegate minGetter,
      MyTerminalValueControl<TBlock, float>.GetterDelegate maxGetter,
      float centerBand)
    {
      this.Normalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) =>
      {
        float min = minGetter(block);
        float max = maxGetter(block);
        return MyTerminalControlSlider<TBlock>.DualLogNormalizer(block, f, min, max, centerBand);
      });
      this.Denormalizer = (MyTerminalControlSlider<TBlock>.FloatFunc) ((block, f) =>
      {
        float min = minGetter(block);
        float max = maxGetter(block);
        return MyTerminalControlSlider<TBlock>.DualLogDenormalizer(block, f, min, max, centerBand);
      });
    }

    void IMyTerminalControlSlider.SetDualLogLimits(
      Func<IMyTerminalBlock, float> minGetter,
      Func<IMyTerminalBlock, float> maxGetter,
      float centerBand)
    {
      this.SetDualLogLimits(new MyTerminalValueControl<TBlock, float>.GetterDelegate(minGetter.Invoke), new MyTerminalValueControl<TBlock, float>.GetterDelegate(maxGetter.Invoke), centerBand);
    }

    protected override void OnUpdateVisual()
    {
      base.OnUpdateVisual();
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock == null || this.m_slider == null)
        return;
      this.m_slider.ValueChanged = (Action<MyGuiControlSlider>) null;
      this.m_slider.DefaultValue = this.DefaultValueGetter != null ? new float?(this.Normalizer(firstBlock, this.DefaultValueGetter(firstBlock))) : new float?();
      float val = this.GetValue(firstBlock);
      this.m_slider.Value = this.Normalizer(firstBlock, val);
      this.m_slider.MinimumStepOverride = new float?(this.Normalizer(firstBlock, val + this.m_halfStepLength) - this.Normalizer(firstBlock, val - this.m_halfStepLength));
      this.m_slider.ValueChanged = this.m_valueChanged;
      this.UpdateDetailedInfo(firstBlock);
    }

    private void UpdateDetailedInfo(TBlock block)
    {
      if (this.AdvancedWriter != null)
        this.m_control.SetDetailedInfo<TBlock>(this.AdvancedWriter, block);
      else
        this.m_control.SetDetailedInfo<TBlock>(this.Writer, block);
      this.RecalculateEllipsisAndScale();
    }

    private void RecalculateEllipsisAndScale()
    {
      if (!this.m_isAutoEllipsisEnabled && !this.m_isAutoScaleEnabled)
        return;
      this.m_control.TitleLabel.IsAutoScaleEnabled = this.m_isAutoScaleEnabled;
      this.m_control.ExtraInfoLabel.IsAutoScaleEnabled = this.m_isAutoScaleEnabled;
      this.m_control.TitleLabel.IsAutoEllipsisEnabled = this.m_isAutoEllipsisEnabled;
      this.m_control.ExtraInfoLabel.IsAutoEllipsisEnabled = this.m_isAutoEllipsisEnabled;
      float x = this.m_slider.Size.X;
      this.m_control.ExtraInfoLabel.RecalculateSize();
      this.m_control.TitleLabel.RecalculateSize();
      this.m_control.ExtraInfoLabel.SetMaxSize(new Vector2(x, this.m_control.ExtraInfoLabel.MaxSize.Y));
      this.m_control.TitleLabel.SetMaxSize(new Vector2(x - this.m_control.ExtraInfoLabel.Size.X, this.m_control.TitleLabel.MaxSize.Y));
      this.m_control.TitleLabel.DoEllipsisAndScaleAdjust(true, 0.8f, true);
    }

    private void OnValueChange(MyGuiControlSlider slider)
    {
      this.SetValue(slider.Value);
      this.UpdateDetailedInfo(this.FirstBlock);
    }

    private void SetValue(float value)
    {
      foreach (TBlock targetBlock in this.TargetBlocks)
        this.SetValue(targetBlock, this.Denormalizer(targetBlock, value));
    }

    private void AmountSetter(float value)
    {
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock == null)
        return;
      this.m_slider.Value = this.Normalizer(firstBlock, value);
    }

    private bool SliderSetValueManual(MyGuiControlSlider arg) => this.AmountDialogEnabled && this.SetValueManual(arg);

    private bool SetValueManual(MyGuiControlSlider arg)
    {
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock == null)
        return false;
      double num1 = (double) this.Denormalizer(firstBlock, 0.0f);
      float num2 = this.Denormalizer(firstBlock, 1f);
      float num3 = this.Denormalizer(firstBlock, arg.Value);
      double num4 = (double) num2;
      MyStringId amountSetValueCaption = MyCommonTexts.DialogAmount_SetValueCaption;
      float? defaultAmount = new float?(num3);
      double uiBkOpacity = (double) MySandboxGame.Config.UIBkOpacity;
      double uiOpacity = (double) MySandboxGame.Config.UIOpacity;
      MyGuiScreenDialogAmount screenDialogAmount = new MyGuiScreenDialogAmount((float) num1, (float) num4, amountSetValueCaption, defaultAmount: defaultAmount, backgroundTransition: ((float) uiBkOpacity), guiTransition: ((float) uiOpacity));
      screenDialogAmount.Closed += new MyGuiScreenBase.ScreenHandler(this.Dialog_Closed);
      screenDialogAmount.OnConfirmed += this.m_amountConfirmed;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) screenDialogAmount);
      return true;
    }

    private void Dialog_Closed(MyGuiScreenBase source, bool isUnloading) => this.m_slider.SetControllCaptured(false);

    private void IncreaseAction(TBlock block, float step)
    {
      float num = this.Normalizer(block, this.GetValue(block));
      this.SetValue(block, this.Denormalizer(block, MathHelper.Clamp(num + step, 0.0f, 1f)));
    }

    private void DecreaseAction(TBlock block, float step)
    {
      float num = this.Normalizer(block, this.GetValue(block));
      this.SetValue(block, this.Denormalizer(block, MathHelper.Clamp(num - step, 0.0f, 1f)));
    }

    private void ResetAction(TBlock block)
    {
      if (this.DefaultValueGetter == null)
        return;
      this.SetValue(block, this.DefaultValueGetter(block));
    }

    private void ActionWriter(TBlock block, StringBuilder appendTo) => (this.CompactWriter ?? this.Writer)(block, appendTo);

    private void SetActions(params MyTerminalAction<TBlock>[] actions) => this.Actions = actions;

    public void EnableActions(
      string increaseIcon,
      string decreaseIcon,
      StringBuilder increaseName,
      StringBuilder decreaseName,
      float step,
      string resetIcon = null,
      StringBuilder resetName = null,
      Func<TBlock, bool> enabled = null,
      Func<TBlock, bool> callable = null)
    {
      MyTerminalAction<TBlock> myTerminalAction1 = new MyTerminalAction<TBlock>("Increase" + this.Id, increaseName, (Action<TBlock>) (b => this.IncreaseAction(b, step)), new MyTerminalControl<TBlock>.WriterDelegate(this.ActionWriter), increaseIcon, enabled, callable);
      MyTerminalAction<TBlock> myTerminalAction2 = new MyTerminalAction<TBlock>("Decrease" + this.Id, decreaseName, (Action<TBlock>) (b => this.DecreaseAction(b, step)), new MyTerminalControl<TBlock>.WriterDelegate(this.ActionWriter), decreaseIcon, enabled, callable);
      if (resetIcon != null)
        this.SetActions(myTerminalAction1, myTerminalAction2, new MyTerminalAction<TBlock>("Reset" + this.Id, resetName, new Action<TBlock>(this.ResetAction), new MyTerminalControl<TBlock>.WriterDelegate(this.ActionWriter), resetIcon, enabled, callable));
      else
        this.SetActions(myTerminalAction1, myTerminalAction2);
    }

    public override void SetValue(TBlock block, float value) => base.SetValue(block, MathHelper.Clamp(value, this.Denormalizer(block, 0.0f), this.Denormalizer(block, 1f)));

    public override float GetDefaultValue(TBlock block) => this.DefaultValueGetter(block);

    public override float GetMinimum(TBlock block) => this.Denormalizer(block, 0.0f);

    public override float GetMaximum(TBlock block) => this.Denormalizer(block, 1f);

    public override float GetValue(TBlock block) => base.GetValue(block);

    MyStringId IMyTerminalControlTitleTooltip.Title
    {
      get => this.Title;
      set => this.Title = value;
    }

    MyStringId IMyTerminalControlTitleTooltip.Tooltip
    {
      get => this.Tooltip;
      set => this.Tooltip = value;
    }

    Action<IMyTerminalBlock, StringBuilder> IMyTerminalControlSlider.Writer
    {
      get
      {
        MyTerminalControl<TBlock>.WriterDelegate oldWriter = this.Writer;
        return (Action<IMyTerminalBlock, StringBuilder>) ((x, y) => oldWriter((TBlock) x, y));
      }
      set => this.Writer = new MyTerminalControl<TBlock>.WriterDelegate(value.Invoke);
    }

    public delegate float FloatFunc(TBlock block, float val) where TBlock : MyTerminalBlock;
  }
}
