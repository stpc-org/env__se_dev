// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyGuiScreenConsole
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using System.Text;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
  internal class MyGuiScreenConsole : MyGuiScreenBase
  {
    private static MyGuiScreenConsole m_instance;
    private MyGuiControlTextbox m_commandLine;
    private MyGuiControlMultilineText m_displayScreen;
    private MyGuiControlContextMenu m_autoComplete;
    private StringBuilder m_commandText = new StringBuilder();
    private string BufferText = "";
    private float m_screenScale;
    private Vector2 m_margin;
    private static MyGuiScreenConsole.MyConsoleKeyTimerController[] m_keys;

    public override string GetFriendlyName() => "Console Screen";

    public MyGuiScreenConsole()
      : base()
    {
      this.m_backgroundTexture = MyGuiConstants.TEXTURE_MESSAGEBOX_BACKGROUND_INFO.Texture;
      this.m_backgroundColor = new Vector4?(new Vector4(0.0f, 0.0f, 0.0f, 0.75f));
      this.m_position = new Vector2(0.5f, 0.25f);
      this.m_screenScale = (float) ((double) MyGuiManager.GetHudSize().X / (double) MyGuiManager.GetHudSize().Y / 1.33333337306976);
      this.m_size = new Vector2?(new Vector2(this.m_screenScale, 0.5f));
      this.m_margin = new Vector2(0.06f, 0.04f);
      MyGuiScreenConsole.m_keys = new MyGuiScreenConsole.MyConsoleKeyTimerController[3];
      MyGuiScreenConsole.m_keys[0] = new MyGuiScreenConsole.MyConsoleKeyTimerController(MyKeys.Up);
      MyGuiScreenConsole.m_keys[1] = new MyGuiScreenConsole.MyConsoleKeyTimerController(MyKeys.Down);
      MyGuiScreenConsole.m_keys[2] = new MyGuiScreenConsole.MyConsoleKeyTimerController(MyKeys.Enter);
    }

    public override void RecreateControls(bool constructor)
    {
      this.m_screenScale = (float) ((double) MyGuiManager.GetHudSize().X / (double) MyGuiManager.GetHudSize().Y / 1.33333337306976);
      this.m_size = new Vector2?(new Vector2(this.m_screenScale, 0.5f));
      base.RecreateControls(constructor);
      Vector4 vector4 = new Vector4(1f, 1f, 0.0f, 1f);
      float num = 1f;
      this.m_commandLine = new MyGuiControlTextbox(new Vector2?(new Vector2(0.0f, 0.25f)), textColor: new Vector4?(vector4));
      MyGuiControlTextbox commandLine = this.m_commandLine;
      commandLine.Position = commandLine.Position - new Vector2(0.0f, this.m_commandLine.Size.Y + this.m_margin.Y / 2f);
      this.m_commandLine.Size = new Vector2(this.m_screenScale, this.m_commandLine.Size.Y) - 2f * this.m_margin;
      this.m_commandLine.ColorMask = new Vector4(0.0f, 0.0f, 0.0f, 0.5f);
      this.m_commandLine.VisualStyle = MyGuiControlTextboxStyleEnum.Debug;
      this.m_commandLine.TextChanged += new Action<MyGuiControlTextbox>(this.commandLine_TextChanged);
      this.m_commandLine.Name = "CommandLine";
      this.m_autoComplete = new MyGuiControlContextMenu();
      this.m_autoComplete.ItemClicked += new Action<MyGuiControlContextMenu, MyGuiControlContextMenu.EventArgs>(this.autoComplete_ItemClicked);
      this.m_autoComplete.Deactivate();
      this.m_autoComplete.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_autoComplete.ColorMask = new Vector4(0.0f, 0.0f, 0.0f, 0.5f);
      this.m_autoComplete.AllowKeyboardNavigation = true;
      this.m_autoComplete.Name = "AutoComplete";
      this.m_displayScreen = new MyGuiControlMultilineText(new Vector2?(new Vector2(-0.5f * this.m_screenScale, -0.25f) + this.m_margin), new Vector2?(new Vector2(this.m_screenScale, 0.5f - this.m_commandLine.Size.Y) - 2f * this.m_margin), font: "Debug", textBoxAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, selectable: true);
      this.m_displayScreen.TextColor = Color.Yellow;
      this.m_displayScreen.TextScale = num;
      this.m_displayScreen.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_displayScreen.Text = MyConsole.DisplayScreen;
      this.m_displayScreen.ColorMask = new Vector4(0.0f, 0.0f, 0.0f, 0.5f);
      this.m_displayScreen.Name = "DisplayScreen";
      this.Controls.Add((MyGuiControlBase) this.m_displayScreen);
      this.Controls.Add((MyGuiControlBase) this.m_commandLine);
      this.Controls.Add((MyGuiControlBase) this.m_autoComplete);
    }

    public static void Show()
    {
      MyGuiScreenConsole.m_instance = new MyGuiScreenConsole();
      MyGuiScreenConsole.m_instance.RecreateControls(true);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiScreenConsole.m_instance);
    }

    protected override void OnClosed() => base.OnClosed();

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      bool flag = false;
      if (this.FocusedControl == this.m_commandLine && MyInput.Static.IsKeyPress(MyKeys.Up) && !this.m_autoComplete.Visible)
      {
        if (this.IsEnoughDelay(MyGuiScreenConsole.MyConsoleKeys.UP, 100) && !this.m_autoComplete.Visible)
        {
          this.UpdateLastKeyPressTimes(MyGuiScreenConsole.MyConsoleKeys.UP);
          if (MyConsole.GetLine() == "")
            this.BufferText = this.m_commandLine.Text;
          MyConsole.PreviousLine();
          this.m_commandLine.Text = !(MyConsole.GetLine() == "") ? MyConsole.GetLine() : this.BufferText;
          this.m_commandLine.MoveCarriageToEnd();
        }
        flag = true;
      }
      if (this.FocusedControl == this.m_commandLine && MyInput.Static.IsKeyPress(MyKeys.Down) && !this.m_autoComplete.Visible)
      {
        if (this.IsEnoughDelay(MyGuiScreenConsole.MyConsoleKeys.DOWN, 100) && !this.m_autoComplete.Visible)
        {
          this.UpdateLastKeyPressTimes(MyGuiScreenConsole.MyConsoleKeys.DOWN);
          if (MyConsole.GetLine() == "")
            this.BufferText = this.m_commandLine.Text;
          MyConsole.NextLine();
          this.m_commandLine.Text = !(MyConsole.GetLine() == "") ? MyConsole.GetLine() : this.BufferText;
          this.m_commandLine.MoveCarriageToEnd();
        }
        flag = true;
      }
      if (this.FocusedControl == this.m_commandLine && MyInput.Static.IsKeyPress(MyKeys.Enter) && (!this.m_commandLine.Text.Equals("") && !this.m_autoComplete.Visible) && this.IsEnoughDelay(MyGuiScreenConsole.MyConsoleKeys.ENTER, 100))
      {
        this.UpdateLastKeyPressTimes(MyGuiScreenConsole.MyConsoleKeys.ENTER);
        if (!this.m_autoComplete.Visible)
        {
          this.BufferText = "";
          MyConsole.ParseCommand(this.m_commandLine.Text);
          MyConsole.NextLine();
          this.m_displayScreen.Text = MyConsole.DisplayScreen;
          this.m_displayScreen.ScrollbarOffsetV = 1f;
          this.m_commandLine.Text = "";
          flag = true;
        }
      }
      if (flag)
        return;
      base.HandleInput(receivedFocusInThisUpdate);
    }

    public void commandLine_TextChanged(MyGuiControlTextbox sender)
    {
      string text = sender.Text;
      if (text.Length == 0 || !sender.Text.ElementAt<char>(sender.Text.Length - 1).Equals('.'))
      {
        if (!this.m_autoComplete.Enabled)
          return;
        this.m_autoComplete.Enabled = false;
        this.m_autoComplete.Deactivate();
      }
      else
      {
        MyCommand command;
        if (!MyConsole.TryGetCommand(text.Substring(0, text.Length - 1), out command))
          return;
        this.m_autoComplete.CreateNewContextMenu();
        this.m_autoComplete.Position = new Vector2((float) ((1.0 - (double) this.m_screenScale) / 2.0) + this.m_margin.X, this.m_size.Value.Y - 2f * this.m_margin.Y) + MyGuiManager.MeasureString("Debug", new StringBuilder(this.m_commandLine.Text), this.m_commandLine.TextScaleWithLanguage);
        foreach (string method in command.Methods)
          this.m_autoComplete.AddItem(new StringBuilder(method).Append(" ").Append((object) command.GetHint(method)), "", "", (object) method);
        this.m_autoComplete.Enabled = true;
        this.m_autoComplete.Activate(false, new Vector2?());
      }
    }

    public void autoComplete_ItemClicked(
      MyGuiControlContextMenu sender,
      MyGuiControlContextMenu.EventArgs args)
    {
      this.m_commandLine.Text += (string) this.m_autoComplete.Items[args.ItemIndex].UserData;
      this.m_commandLine.MoveCarriageToEnd();
      this.FocusedControl = (MyGuiControlBase) this.m_commandLine;
    }

    private bool IsEnoughDelay(MyGuiScreenConsole.MyConsoleKeys key, int forcedDelay)
    {
      MyGuiScreenConsole.MyConsoleKeyTimerController key1 = MyGuiScreenConsole.m_keys[(int) key];
      return key1 == null || MyGuiManager.TotalTimeInMilliseconds - key1.LastKeyPressTime > forcedDelay;
    }

    private void UpdateLastKeyPressTimes(MyGuiScreenConsole.MyConsoleKeys key)
    {
      MyGuiScreenConsole.MyConsoleKeyTimerController key1 = MyGuiScreenConsole.m_keys[(int) key];
      if (key1 == null)
        return;
      key1.LastKeyPressTime = MyGuiManager.TotalTimeInMilliseconds;
    }

    private enum MyConsoleKeys
    {
      UP,
      DOWN,
      ENTER,
    }

    private class MyConsoleKeyTimerController
    {
      public MyKeys Key;
      public int LastKeyPressTime;

      public MyConsoleKeyTimerController(MyKeys key)
      {
        this.Key = key;
        this.LastKeyPressTime = -60000;
      }
    }
  }
}
