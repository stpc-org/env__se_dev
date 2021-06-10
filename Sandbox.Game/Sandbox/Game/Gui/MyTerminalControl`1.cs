// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControl`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Extensions;

namespace Sandbox.Game.Gui
{
  public abstract class MyTerminalControl<TBlock> : ITerminalControl, IMyTerminalControl
    where TBlock : MyTerminalBlock
  {
    public static readonly float PREFERRED_CONTROL_WIDTH = 355f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
    public static readonly MyTerminalBlock[] Empty = new MyTerminalBlock[0];
    public readonly string Id;
    public Func<TBlock, bool> Enabled = (Func<TBlock, bool>) (b => true);
    public Func<TBlock, bool> Visible = (Func<TBlock, bool>) (b => true);
    private MyGuiControlBase m_control;

    public MyTerminalControl<TBlock>.TooltipGetter DynamicTooltipGetter { get; set; }

    MyTerminalBlock[] ITerminalControl.TargetBlocks { get; set; }

    protected ArrayOfTypeEnumerator<MyTerminalBlock, ArrayEnumerator<MyTerminalBlock>, TBlock> TargetBlocks => ((ITerminalControl) this).TargetBlocks.OfTypeFast<MyTerminalBlock, TBlock>();

    protected TBlock FirstBlock
    {
      get
      {
        foreach (TBlock targetBlock in this.TargetBlocks)
        {
          if (targetBlock.HasLocalPlayerAccess())
            return targetBlock;
        }
        using (ArrayOfTypeEnumerator<MyTerminalBlock, ArrayEnumerator<MyTerminalBlock>, TBlock> enumerator = this.TargetBlocks.GetEnumerator())
        {
          if (enumerator.MoveNext())
            return enumerator.Current;
        }
        return default (TBlock);
      }
    }

    public MyGuiControlBase GetGuiControl()
    {
      if (this.m_control == null)
        this.m_control = this.CreateGui();
      return this.m_control;
    }

    public bool SupportsMultipleBlocks { get; set; }

    public MyTerminalControl(string id)
    {
      this.Id = id;
      this.SupportsMultipleBlocks = true;
      ((ITerminalControl) this).TargetBlocks = MyTerminalControl<TBlock>.Empty;
    }

    protected abstract MyGuiControlBase CreateGui();

    protected virtual void OnUpdateVisual()
    {
      bool flag1 = false;
      foreach (TBlock targetBlock in this.TargetBlocks)
      {
        bool flag2 = targetBlock.HasLocalPlayerAccess() && this.Enabled(targetBlock);
        if (flag2 && targetBlock.CubeGrid != null)
        {
          List<long> bigOwners = targetBlock.CubeGrid.BigOwners;
          // ISSUE: explicit non-virtual call
          if ((bigOwners != null ? ((uint) __nonvirtual (bigOwners.Count) > 0U ? 1 : 0) : 1) != 0)
          {
            List<long> smallOwners = targetBlock.CubeGrid.SmallOwners;
            // ISSUE: explicit non-virtual call
            if ((smallOwners != null ? ((uint) __nonvirtual (smallOwners.Count) > 0U ? 1 : 0) : 1) != 0 && !targetBlock.HasLocalPlayerAdminUseTerminals() && (targetBlock.IDModule == null && !targetBlock.CubeGrid.SmallOwners.Contains(MySession.Static.LocalPlayerId)))
              flag2 = false;
          }
        }
        flag1 |= flag2;
      }
      if (this.m_control.Enabled != flag1)
        this.m_control.Enabled = flag1;
      TBlock firstBlock = this.FirstBlock;
      if ((object) firstBlock == null || this.DynamicTooltipGetter == null)
        return;
      this.m_control.SetTooltip(this.DynamicTooltipGetter(firstBlock));
    }

    public void UpdateVisual()
    {
      if (this.m_control == null)
        return;
      this.OnUpdateVisual();
    }

    public void RedrawControl()
    {
      if (this.m_control == null)
        return;
      this.m_control = this.CreateGui();
    }

    bool ITerminalControl.IsVisible(MyTerminalBlock block) => this.Visible((TBlock) block);

    public MyTerminalAction<TBlock>[] Actions { get; protected set; }

    ITerminalAction[] ITerminalControl.Actions => (ITerminalAction[]) this.Actions;

    string ITerminalControl.Id => this.Id;

    string IMyTerminalControl.Id => this.Id;

    Func<IMyTerminalBlock, bool> IMyTerminalControl.Enabled
    {
      get
      {
        Func<TBlock, bool> oldEnabled = this.Enabled;
        return (Func<IMyTerminalBlock, bool>) (x => oldEnabled((TBlock) x));
      }
      set => this.Enabled = (Func<TBlock, bool>) value;
    }

    Func<IMyTerminalBlock, bool> IMyTerminalControl.Visible
    {
      get
      {
        Func<TBlock, bool> oldVisible = this.Visible;
        return (Func<IMyTerminalBlock, bool>) (x => oldVisible((TBlock) x));
      }
      set => this.Visible = (Func<TBlock, bool>) value;
    }

    public delegate string TooltipGetter(TBlock block) where TBlock : MyTerminalBlock;

    public delegate void WriterDelegate(TBlock block, StringBuilder writeTo) where TBlock : MyTerminalBlock;

    public delegate void AdvancedWriterDelegate(
      TBlock block,
      MyGuiControlBlockProperty control,
      StringBuilder writeTo)
      where TBlock : MyTerminalBlock;
  }
}
