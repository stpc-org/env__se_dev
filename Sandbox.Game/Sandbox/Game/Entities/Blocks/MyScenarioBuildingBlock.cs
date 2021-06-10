// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyScenarioBuildingBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
  public class MyScenarioBuildingBlock : MyTerminalBlock
  {
    public static List<MyTerminalBlock> Clipboard = new List<MyTerminalBlock>();
    private static TimeSpan m_lastAccess;

    private static void AddToClipboard(MyTerminalBlock block)
    {
      if (MySession.Static.ElapsedGameTime != MyScenarioBuildingBlock.m_lastAccess)
      {
        MyScenarioBuildingBlock.Clipboard.Clear();
        MyScenarioBuildingBlock.m_lastAccess = MySession.Static.ElapsedGameTime;
      }
      MyScenarioBuildingBlock.Clipboard.Add(block);
    }

    public MyScenarioBuildingBlock() => this.CreateTerminalControls();

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyTerminalBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlFactory.AddControl<MyTerminalBlock>((MyTerminalControl<MyTerminalBlock>) new MyTerminalControlSeparator<MyTerminalBlock>());
      MyTerminalControlButton<MyTerminalBlock> terminalControlButton = new MyTerminalControlButton<MyTerminalBlock>("CopyBlockID", MySpaceTexts.GuiScenarioEdit_CopyIds, MySpaceTexts.GuiScenarioEdit_CopyIdsTooltip, (Action<MyTerminalBlock>) (self => MyScenarioBuildingBlock.AddToClipboard(self)));
      terminalControlButton.Enabled = (Func<MyTerminalBlock, bool>) (x => true);
      terminalControlButton.Visible = (Func<MyTerminalBlock, bool>) (x => MySession.Static.Settings.ScenarioEditMode);
      terminalControlButton.SupportsMultipleBlocks = true;
      MyTerminalControlFactory.AddControl<MyTerminalBlock>((MyTerminalControl<MyTerminalBlock>) terminalControlButton);
    }

    private class Sandbox_Game_Entities_Blocks_MyScenarioBuildingBlock\u003C\u003EActor : IActivator, IActivator<MyScenarioBuildingBlock>
    {
      object IActivator.CreateInstance() => (object) new MyScenarioBuildingBlock();

      MyScenarioBuildingBlock IActivator<MyScenarioBuildingBlock>.CreateInstance() => new MyScenarioBuildingBlock();
    }
  }
}
