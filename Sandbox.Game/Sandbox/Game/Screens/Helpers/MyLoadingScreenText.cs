// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyLoadingScreenText
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Collections;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
  internal class MyLoadingScreenText
  {
    public readonly MyStringId Text;
    protected static List<MyLoadingScreenText> m_textsShared;
    protected static List<MyLoadingScreenText> m_textsKeyboard;
    protected static List<MyLoadingScreenText> m_textsGamepad;

    public MyLoadingScreenText(MyStringId text) => this.Text = text;

    public override string ToString() => this.Text.ToString();

    public static ListReader<MyLoadingScreenText> TextsShared => (ListReader<MyLoadingScreenText>) MyLoadingScreenText.m_textsShared;

    static MyLoadingScreenText()
    {
      if (MyLoadingScreenText.m_textsShared == null)
        MyLoadingScreenText.m_textsShared = new List<MyLoadingScreenText>();
      else
        MyLoadingScreenText.m_textsShared.Clear();
      if (MyLoadingScreenText.m_textsKeyboard == null)
        MyLoadingScreenText.m_textsKeyboard = new List<MyLoadingScreenText>();
      else
        MyLoadingScreenText.m_textsKeyboard.Clear();
      if (MyLoadingScreenText.m_textsGamepad == null)
        MyLoadingScreenText.m_textsGamepad = new List<MyLoadingScreenText>();
      else
        MyLoadingScreenText.m_textsGamepad.Clear();
      MyLoadingScreenQuote.Init();
      MyLoadingScreenHint.Init();
    }

    public static MyLoadingScreenText GetSharedScreenText(int i)
    {
      i = MyMath.Mod(i, MyLoadingScreenText.m_textsShared.Count);
      return MyLoadingScreenText.m_textsShared[i];
    }

    public static MyLoadingScreenText GetGamepadScreenText(int i)
    {
      i = MyMath.Mod(i, MyLoadingScreenText.m_textsGamepad.Count);
      return MyLoadingScreenText.m_textsGamepad[i];
    }

    public static MyLoadingScreenText GetKeyboardScreenText(int i)
    {
      i = MyMath.Mod(i, MyLoadingScreenText.m_textsKeyboard.Count);
      return MyLoadingScreenText.m_textsKeyboard[i];
    }

    public static MyLoadingScreenText GetRandomText()
    {
      int count = MyLoadingScreenText.m_textsShared.Count;
      if (MyInput.Static.IsJoystickLastUsed)
      {
        int i = MyRandom.Instance.Next(0, MyLoadingScreenText.m_textsShared.Count + MyLoadingScreenText.m_textsGamepad.Count);
        return i < count ? MyLoadingScreenText.GetSharedScreenText(i) : MyLoadingScreenText.GetGamepadScreenText(i - count);
      }
      int i1 = MyRandom.Instance.Next(0, MyLoadingScreenText.m_textsShared.Count + MyLoadingScreenText.m_textsKeyboard.Count);
      return i1 < count ? MyLoadingScreenText.GetSharedScreenText(i1) : MyLoadingScreenText.GetKeyboardScreenText(i1 - count);
    }

    public static MyLoadingScreenText GetText(int i = 0, bool forceGamepad = false)
    {
      int count = MyLoadingScreenText.m_textsShared.Count;
      if (MyInput.Static.IsJoystickLastUsed | forceGamepad)
      {
        int num = MyLoadingScreenText.m_textsShared.Count + MyLoadingScreenText.m_textsGamepad.Count;
        int i1 = i % num;
        return i1 < count ? MyLoadingScreenText.GetSharedScreenText(i1) : MyLoadingScreenText.GetGamepadScreenText(i1 - count);
      }
      int num1 = MyLoadingScreenText.m_textsShared.Count + MyLoadingScreenText.m_textsKeyboard.Count;
      int i2 = i % num1;
      return i2 < count ? MyLoadingScreenText.GetSharedScreenText(i2) : MyLoadingScreenText.GetKeyboardScreenText(i2 - count);
    }
  }
}
