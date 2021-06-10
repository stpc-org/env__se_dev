// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyDebugComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using VRage.Input;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  public abstract class MyDebugComponent
  {
    private static float m_textOffset = 0.0f;
    private const int LINE_OFFSET = 15;
    private const int LINE_BREAK_OFFSET = 17;
    private static HashSet<ushort> m_enabledShortcutKeys = new HashSet<ushort>();
    private SortedSet<MyDebugComponent.MyShortcut> m_shortCuts = new SortedSet<MyDebugComponent.MyShortcut>((IComparer<MyDebugComponent.MyShortcut>) MyDebugComponent.MyShortcutComparer.Static);
    private HashSet<MyDebugComponent.MySwitch> m_switches = new HashSet<MyDebugComponent.MySwitch>();
    private bool m_enabled = true;
    public int m_frameCounter;

    public static float VerticalTextOffset => MyDebugComponent.m_textOffset;

    protected static float NextVerticalOffset
    {
      get
      {
        double textOffset = (double) MyDebugComponent.m_textOffset;
        MyDebugComponent.m_textOffset += 15f;
        return (float) textOffset;
      }
    }

    public static float NextTextOffset(float scale)
    {
      double textOffset = (double) MyDebugComponent.m_textOffset;
      MyDebugComponent.m_textOffset += 15f * scale;
      return (float) textOffset;
    }

    protected void Text(string message, params object[] arguments) => this.Text(Color.White, 1f, message, arguments);

    protected void Text(Color color, string message, params object[] arguments) => this.Text(color, 1f, message, arguments);

    protected void Text(Color color, float scale, string message, params object[] arguments)
    {
      if (arguments.Length != 0)
        message = string.Format(message, arguments);
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, MyDebugComponent.NextTextOffset(scale)), message, color, 0.6f * scale);
    }

    protected void MultilineText(string message, params object[] arguments) => this.MultilineText(Color.White, 1f, message, arguments);

    protected void MultilineText(Color color, string message, params object[] arguments) => this.MultilineText(color, 1f, message, arguments);

    protected void MultilineText(
      Color color,
      float scale,
      string message,
      params object[] arguments)
    {
      if (arguments.Length != 0)
        message = string.Format(message, arguments);
      int num1 = 0;
      foreach (char ch in message)
      {
        if (ch == '\n')
          ++num1;
      }
      message = message.Replace("\t", "    ");
      float num2 = (float) (15 + 17 * num1) * scale;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.0f, MyDebugComponent.m_textOffset), message, color, 0.6f * scale);
      MyDebugComponent.m_textOffset += num2;
    }

    public void Section(string text, params object[] formatArgs)
    {
      this.VSpace(5f);
      this.Text(Color.Yellow, 1.5f, text, formatArgs);
      this.VSpace(5f);
    }

    protected void VSpace(float space) => MyDebugComponent.m_textOffset += space;

    public MyDebugComponent()
      : this(false)
    {
    }

    public MyDebugComponent(bool enabled) => this.Enabled = enabled;

    public bool Enabled
    {
      get => this.m_enabled;
      set => this.m_enabled = value;
    }

    public virtual object InputData
    {
      get => (object) null;
      set
      {
      }
    }

    protected void Save()
    {
      SerializableDictionary<string, MyConfig.MyDebugInputData> debugInputComponents = MySandboxGame.Config.DebugInputComponents;
      string name = this.GetName();
      MyConfig.MyDebugInputData myDebugInputData = debugInputComponents[name];
      myDebugInputData.Enabled = this.Enabled;
      myDebugInputData.Data = this.InputData;
      debugInputComponents[name] = myDebugInputData;
      MySandboxGame.Config.Save();
    }

    public virtual bool HandleInput()
    {
      foreach (MyDebugComponent.MyShortcut shortCut in this.m_shortCuts)
      {
        bool flag = true & shortCut.Control == MyInput.Static.IsAnyCtrlKeyPressed() & shortCut.Shift == MyInput.Static.IsAnyShiftKeyPressed() & shortCut.Alt == MyInput.Static.IsAnyAltKeyPressed();
        if (flag)
        {
          if (shortCut.NewPress)
            flag &= MyInput.Static.IsNewKeyPressed(shortCut.Key);
          else
            flag &= MyInput.Static.IsKeyPress(shortCut.Key);
        }
        if (flag && shortCut._Action != null)
          return shortCut._Action();
      }
      foreach (MyDebugComponent.MySwitch mySwitch in this.m_switches)
      {
        if ((1 & (MyInput.Static.IsNewKeyPressed(mySwitch.Key) ? 1 : 0)) != 0 && mySwitch.Action != null)
          return mySwitch.Action(mySwitch.Key);
      }
      return false;
    }

    public abstract string GetName();

    public static void ResetFrame()
    {
      MyDebugComponent.m_textOffset = 0.0f;
      MyDebugComponent.m_enabledShortcutKeys.Clear();
    }

    public virtual void DispatchUpdate()
    {
      if (this.m_frameCounter % 10 == 0)
        this.Update10();
      if (this.m_frameCounter >= 100)
      {
        this.Update100();
        this.m_frameCounter = 0;
      }
      ++this.m_frameCounter;
    }

    public virtual void Draw()
    {
      if (MySandboxGame.Config.DebugComponentsInfo != MyDebugComponent.MyDebugComponentInfoState.FullInfo)
        return;
      float scale = 0.6f;
      MyRenderProxy.DebugDrawText2D(new Vector2(0.1f, MyDebugComponent.m_textOffset), this.GetName() + " debug input:", Color.Gold, scale);
      MyDebugComponent.m_textOffset += 15f;
      foreach (MyDebugComponent.MyShortcut shortCut in this.m_shortCuts)
      {
        string keysString = shortCut.GetKeysString();
        string text = shortCut.Description();
        Color color = MyDebugComponent.m_enabledShortcutKeys.Contains(shortCut.GetId()) ? Color.Red : Color.White;
        MyRenderProxy.DebugDrawText2D(new Vector2(100f, MyDebugComponent.m_textOffset), keysString + ":", color, scale, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
        MyRenderProxy.DebugDrawText2D(new Vector2(105f, MyDebugComponent.m_textOffset), text, Color.White, scale);
        MyDebugComponent.m_enabledShortcutKeys.Add(shortCut.GetId());
        MyDebugComponent.m_textOffset += 15f;
      }
      foreach (MyDebugComponent.MySwitch mySwitch in this.m_switches)
      {
        Color color = this.GetSwitchValue(mySwitch.Key) ? Color.Red : Color.White;
        MyRenderProxy.DebugDrawText2D(new Vector2(30f, MyDebugComponent.m_textOffset), "switch " + (object) mySwitch.Key + (mySwitch.Note.Length == 0 ? (object) "" : (object) (" " + mySwitch.Note)) + " is " + (this.GetSwitchValue(mySwitch.Key) ? (object) "On" : (object) "Off"), color, scale);
        MyDebugComponent.m_textOffset += 15f;
      }
      MyDebugComponent.m_textOffset += 5f;
    }

    public virtual void Update10()
    {
    }

    public virtual void Update100()
    {
    }

    protected void AddShortcut(
      MyKeys key,
      bool newPress,
      bool control,
      bool shift,
      bool alt,
      Func<string> description,
      Func<bool> action)
    {
      this.m_shortCuts.Add(new MyDebugComponent.MyShortcut()
      {
        Key = key,
        NewPress = newPress,
        Control = control,
        Shift = shift,
        Alt = alt,
        Description = description,
        _Action = action
      });
    }

    protected void AddSwitch(
      MyKeys key,
      Func<MyKeys, bool> action,
      string note = "",
      bool defaultValue = false)
    {
      this.m_switches.Add(new MyDebugComponent.MySwitch(key, action, note, defaultValue));
    }

    protected void AddSwitch(
      MyKeys key,
      Func<MyKeys, bool> action,
      MyDebugComponent.MyRef<bool> boolRef,
      string note = "")
    {
      this.m_switches.Add(new MyDebugComponent.MySwitch(key, action, boolRef, note));
    }

    protected void SetSwitch(MyKeys key, bool value)
    {
      foreach (MyDebugComponent.MySwitch mySwitch in this.m_switches)
      {
        if (mySwitch.Key == key)
        {
          mySwitch.IsSet = value;
          break;
        }
      }
    }

    public bool GetSwitchValue(MyKeys key)
    {
      foreach (MyDebugComponent.MySwitch mySwitch in this.m_switches)
      {
        if (mySwitch.Key == key)
          return mySwitch.IsSet;
      }
      return false;
    }

    public bool GetSwitchValue(string note)
    {
      foreach (MyDebugComponent.MySwitch mySwitch in this.m_switches)
      {
        if (mySwitch.Note == note)
          return mySwitch.IsSet;
      }
      return false;
    }

    public IMyInput Input => MyInput.Static;

    private class MyShortcutComparer : IComparer<MyDebugComponent.MyShortcut>
    {
      public static MyDebugComponent.MyShortcutComparer Static = new MyDebugComponent.MyShortcutComparer();

      public int Compare(MyDebugComponent.MyShortcut x, MyDebugComponent.MyShortcut y) => x.GetId().CompareTo(y.GetId());
    }

    private struct MyShortcut
    {
      public MyKeys Key;
      public bool NewPress;
      public bool Control;
      public bool Shift;
      public bool Alt;
      public Func<string> Description;
      public Func<bool> _Action;

      public string GetKeysString()
      {
        string str = "";
        if (this.Control)
          str += "Ctrl";
        if (this.Shift)
          str += string.IsNullOrEmpty(str) ? "Shift" : "+Shift";
        if (this.Alt)
          str += string.IsNullOrEmpty(str) ? "Alt" : "+Alt";
        return str + (string.IsNullOrEmpty(str) ? MyInput.Static.GetKeyName(this.Key) : "+" + MyInput.Static.GetKeyName(this.Key));
      }

      public ushort GetId() => (ushort) ((int) (ushort) ((int) (ushort) ((int) (ushort) ((uint) this.Key << 8) + (this.Control ? 4 : 0)) + (this.Shift ? 2 : 0)) + (this.Alt ? 1 : 0));
    }

    public class MyRef<T>
    {
      private Action<T> modify;
      private Func<T> getter;

      public MyRef(Func<T> getter, Action<T> modify)
      {
        this.modify = modify;
        this.getter = getter;
      }

      public T Value
      {
        get => this.getter();
        set => this.modify(value);
      }
    }

    private class MySwitch
    {
      public MyKeys Key;
      public Func<MyKeys, bool> Action;
      public string Note;
      private MyDebugComponent.MyRef<bool> m_boolReference;
      private bool m_value;

      public MySwitch(MyKeys key, Func<MyKeys, bool> action, string note = "")
      {
        this.Key = key;
        this.Action = action;
        this.Note = note;
      }

      public MySwitch(MyKeys key, Func<MyKeys, bool> action, string note = "", bool defaultValue = false)
      {
        this.Key = key;
        this.Action = action;
        this.Note = note;
        this.IsSet = defaultValue;
      }

      public MySwitch(
        MyKeys key,
        Func<MyKeys, bool> action,
        MyDebugComponent.MyRef<bool> field,
        string note = "")
      {
        this.m_boolReference = field;
        this.Key = key;
        this.Action = action;
        this.Note = note;
      }

      public bool IsSet
      {
        get => this.m_boolReference != null ? this.m_boolReference.Value : this.m_value;
        set
        {
          if (this.m_boolReference != null)
            this.m_boolReference.Value = value;
          else
            this.m_value = value;
        }
      }

      public ushort GetId() => (ushort) ((uint) this.Key << 8);
    }

    public enum MyDebugComponentInfoState
    {
      NoInfo,
      EnabledInfo,
      FullInfo,
    }
  }
}
