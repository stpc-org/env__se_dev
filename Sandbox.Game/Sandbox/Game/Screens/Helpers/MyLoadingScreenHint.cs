// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyLoadingScreenHint
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyLoadingScreenHint : MyLoadingScreenText
  {
    public readonly List<object> Control = new List<object>();
    private List<object> m_control = new List<object>();

    public MyLoadingScreenHint(MyStringId text, List<object> control)
      : base(text)
    {
      this.Control = control;
      for (int index = 0; index < control.Count; ++index)
        this.m_control.Add((object) null);
      this.RefreshControls();
    }

    public override string ToString()
    {
      this.RefreshControls();
      return string.Format(MyTexts.GetString(this.Text), this.m_control.ToArray());
    }

    private void RefreshControls()
    {
      for (int index = 0; index < this.m_control.Count; ++index)
      {
        if (this.Control[index] != null)
        {
          if (this.Control[index] is MyStringId controlEnum)
            this.m_control[index] = (object) MyInput.Static.GetGameControl(controlEnum);
          else if (this.Control[index] is MyLoadingScreenHint.MyContextWithControl contextWithControl)
          {
            IMyControllerControl control = MyControllerHelper.TryGetControl(contextWithControl.Context, contextWithControl.Control) ?? MyControllerHelper.GetNullControl();
            this.m_control[index] = (object) control;
            if (MyControllerHelper.IsNullControl(control))
              MyLog.Default.Error("Control for hint is missing. Context - {0}, Control - {1}", (object) contextWithControl.Context.ToString(), (object) contextWithControl.Control.ToString());
          }
        }
      }
    }

    public static void Init()
    {
      MyLoadingScreenHint.GetSharedHints();
      MyLoadingScreenHint.GetKeyboardOnlyHints();
      MyLoadingScreenHint.GetGamepadOnlyHints();
    }

    private static void GetGamepadOnlyHints()
    {
      MyStringId nullOrEmpty1 = MyStringId.NullOrEmpty;
      MyStringId text;
      for (int index = 0; (text = MyStringId.TryGet(string.Format("HintGamepadOnly{0:00}Text", (object) index))) != MyStringId.NullOrEmpty; ++index)
      {
        int num = 0;
        MyStringId nullOrEmpty2 = MyStringId.NullOrEmpty;
        List<object> control = new List<object>();
        MyStringId id;
        for (; (id = MyStringId.TryGet(string.Format("HintGamepadOnly{0:00}Control{1}", (object) index, (object) num))) != MyStringId.NullOrEmpty; ++num)
        {
          string[] strArray = MyTexts.GetString(id).Split(':');
          if (strArray.Length == 1)
          {
            MyStringId orCompute = MyStringId.GetOrCompute(strArray[0]);
            control.Add((object) orCompute);
          }
          else if (strArray.Length == 2)
            control.Add((object) new MyLoadingScreenHint.MyContextWithControl()
            {
              Context = MyStringId.GetOrCompute(strArray[0]),
              Control = MyStringId.GetOrCompute(strArray[1])
            });
        }
        MyLoadingScreenText.m_textsGamepad.Add((MyLoadingScreenText) new MyLoadingScreenHint(text, control));
      }
    }

    private static void GetKeyboardOnlyHints()
    {
      MyStringId nullOrEmpty1 = MyStringId.NullOrEmpty;
      MyStringId text;
      for (int index = 0; (text = MyStringId.TryGet(string.Format("HintKeyboardOnly{0:00}Text", (object) index))) != MyStringId.NullOrEmpty; ++index)
      {
        int num = 0;
        MyStringId nullOrEmpty2 = MyStringId.NullOrEmpty;
        List<object> control = new List<object>();
        MyStringId id;
        for (; (id = MyStringId.TryGet(string.Format("HintKeyboardOnly{0:00}Control{1}", (object) index, (object) num))) != MyStringId.NullOrEmpty; ++num)
        {
          MyStringId orCompute = MyStringId.GetOrCompute(MyTexts.GetString(id));
          control.Add((object) orCompute);
        }
        MyLoadingScreenText.m_textsKeyboard.Add((MyLoadingScreenText) new MyLoadingScreenHint(text, control));
      }
    }

    private static void GetSharedHints()
    {
      MyStringId nullOrEmpty1 = MyStringId.NullOrEmpty;
      MyStringId text;
      for (int index = 0; (text = MyStringId.TryGet(string.Format("Hint{0:00}Text", (object) index))) != MyStringId.NullOrEmpty; ++index)
      {
        int num = 0;
        MyStringId nullOrEmpty2 = MyStringId.NullOrEmpty;
        List<object> control = new List<object>();
        MyStringId id;
        for (; (id = MyStringId.TryGet(string.Format("Hint{0:00}Control{1}", (object) index, (object) num))) != MyStringId.NullOrEmpty; ++num)
        {
          MyStringId orCompute = MyStringId.GetOrCompute(MyTexts.GetString(id));
          control.Add((object) orCompute);
        }
        MyLoadingScreenText.m_textsShared.Add((MyLoadingScreenText) new MyLoadingScreenHint(text, control));
      }
    }

    private struct MyContextWithControl
    {
      public MyStringId Context;
      public MyStringId Control;
    }
  }
}
