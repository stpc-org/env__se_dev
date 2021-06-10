// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionCreateBlueprint
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.SessionComponents.Clipboard;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public class MyActionCreateBlueprint : MyActionBase
  {
    public override void ExecuteAction() => MyClipboardComponent.Static.CreateBlueprint();

    public override bool IsEnabled() => true;
  }
}
