// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyGuiScreenProgressAsync
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using System;
using System.Text;
using VRage.Utils;

namespace Sandbox.Game.Screens
{
  public class MyGuiScreenProgressAsync : MyGuiScreenProgressBase
  {
    private Func<IMyAsyncResult> m_beginAction;
    private Action<IMyAsyncResult, MyGuiScreenProgressAsync> m_endAction;
    private IMyAsyncResult m_asyncResult;

    public string FriendlyName { get; set; }

    public object UserData { get; private set; }

    public MyGuiScreenProgressAsync(
      MyStringId text,
      MyStringId? cancelText,
      Func<IMyAsyncResult> beginAction,
      Action<IMyAsyncResult, MyGuiScreenProgressAsync> endAction,
      object userData = null)
      : base(text, cancelText)
    {
      this.FriendlyName = nameof (MyGuiScreenProgressAsync);
      this.m_beginAction = beginAction;
      this.m_endAction = endAction;
      this.UserData = userData;
    }

    public StringBuilder Text
    {
      get => this.m_progressTextLabel.TextToDraw;
      set => this.m_progressTextLabel.TextToDraw = value;
    }

    public new MyStringId ProgressText
    {
      get => base.ProgressText;
      set
      {
        if (!(base.ProgressText != value))
          return;
        this.m_progressTextLabel.PrepareForAsyncTextUpdate();
        base.ProgressText = value;
      }
    }

    public new string ProgressTextString
    {
      get => base.ProgressTextString;
      set
      {
        if (!(base.ProgressTextString != value))
          return;
        this.m_progressTextLabel.PrepareForAsyncTextUpdate();
        base.ProgressTextString = value;
      }
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_rotatingWheel.MultipleSpinningWheels = MyPerGameSettings.GUI.MultipleSpinningWheels;
    }

    protected override void ProgressStart() => this.m_asyncResult = this.m_beginAction();

    public override string GetFriendlyName() => this.FriendlyName;

    public override bool Update(bool hasFocus)
    {
      if (!base.Update(hasFocus) || this.State != MyGuiScreenState.OPENED)
        return false;
      if (this.m_asyncResult.IsCompleted)
        this.m_endAction(this.m_asyncResult, this);
      if (this.m_asyncResult != null && this.m_asyncResult.Task.Exceptions != null)
      {
        foreach (Exception exception in this.m_asyncResult.Task.Exceptions)
          MySandboxGame.Log.WriteLine(exception);
      }
      return true;
    }
  }
}
