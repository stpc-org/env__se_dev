// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.RadialMenuActions.MyActionBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
  public abstract class MyActionBase : IMyRadialMenuSystemAction
  {
    public abstract void ExecuteAction();

    public virtual MyRadialLabelText GetLabel(string shortcut, string name) => new MyRadialLabelText()
    {
      Name = name,
      State = string.Empty,
      Shortcut = shortcut
    };

    public virtual bool IsEnabled() => true;

    protected static string AppendingConjunctionState(MyRadialLabelText label) => !string.IsNullOrEmpty(label.State) ? " - " : "";

    public virtual int GetIconIndex() => 0;
  }
}
