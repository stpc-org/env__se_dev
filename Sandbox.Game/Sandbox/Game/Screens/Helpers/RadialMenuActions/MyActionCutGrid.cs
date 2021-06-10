// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionCutGrid
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionCutGrid : MyActionBase
  {
    public override void ExecuteAction() => MyClipboardComponent.Static.Cut();

    public override bool IsEnabled() => MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.CreativeMode;
  }
}
