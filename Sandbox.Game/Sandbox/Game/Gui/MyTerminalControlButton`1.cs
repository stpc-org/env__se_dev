// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyTerminalControlButton`1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyTerminalControlButton<TBlock> : MyTerminalControl<TBlock>, IMyTerminalControlButton, IMyTerminalControl, IMyTerminalControlTitleTooltip
    where TBlock : MyTerminalBlock
  {
    private System.Action<TBlock> m_action;
    private System.Action<MyGuiControlButton> m_buttonClicked;
    public MyStringId Title;
    public MyStringId Tooltip;
    private bool m_isAutoscaleEnabled;

    public System.Action<TBlock> Action
    {
      get => this.m_action;
      set => this.m_action = value;
    }

    public MyTerminalControlButton(
      string id,
      MyStringId title,
      MyStringId tooltip,
      System.Action<TBlock> action,
      bool isAutoscaleEnabled = false)
      : base(id)
    {
      this.Title = title;
      this.Tooltip = tooltip;
      this.m_action = action;
      this.m_isAutoscaleEnabled = isAutoscaleEnabled;
    }

    protected override MyGuiControlBase CreateGui()
    {
      Vector2? position = new Vector2?();
      Vector2? size = new Vector2?();
      Vector4? colorMask = new Vector4?();
      StringBuilder stringBuilder = MyTexts.Get(this.Title);
      string toolTip = MyTexts.GetString(this.Tooltip);
      StringBuilder text = stringBuilder;
      int? buttonIndex = new int?();
      int num = this.m_isAutoscaleEnabled ? 1 : 0;
      MyGuiControlButton guiControlButton = new MyGuiControlButton(position, size: size, colorMask: colorMask, toolTip: toolTip, text: text, buttonIndex: buttonIndex, isAutoscaleEnabled: (num != 0));
      this.m_buttonClicked = new System.Action<MyGuiControlButton>(this.OnButtonClicked);
      guiControlButton.ButtonClicked += this.m_buttonClicked;
      return (MyGuiControlBase) guiControlButton;
    }

    private void OnButtonClicked(MyGuiControlButton obj)
    {
      foreach (TBlock targetBlock in this.TargetBlocks)
      {
        if (this.m_action != null)
          this.m_action(targetBlock);
      }
    }

    protected override void OnUpdateVisual() => MySandboxGame.Static.Invoke((System.Action) (() => base.OnUpdateVisual()), "TerminalControlButton");

    public MyTerminalAction<TBlock> EnableAction(
      string icon,
      StringBuilder name,
      MyTerminalControl<TBlock>.WriterDelegate writer = null)
    {
      MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>(this.Id, name, this.m_action, writer, icon);
      this.Actions = new MyTerminalAction<TBlock>[1]
      {
        myTerminalAction
      };
      return myTerminalAction;
    }

    System.Action<IMyTerminalBlock> IMyTerminalControlButton.Action
    {
      get
      {
        System.Action<TBlock> oldAction = this.Action;
        return (System.Action<IMyTerminalBlock>) (x => oldAction((TBlock) x));
      }
      set => this.Action = (System.Action<TBlock>) value;
    }

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
  }
}
