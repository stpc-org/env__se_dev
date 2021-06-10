// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyIngameHelpObjective
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Input;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
  internal abstract class MyIngameHelpObjective
  {
    public string Id;
    public string[] RequiredIds;
    public string FollowingId;
    public Func<bool> RequiredCondition;
    public MyStringId TitleEnum;
    public MyIngameHelpDetail[] Details = new MyIngameHelpDetail[0];
    public float DelayToHide;
    public float DelayToAppear;

    public static object GetHighlightedControl(MyStringId controlId)
    {
      string str1 = MyInput.Static.GetGameControl(controlId) != null ? MyInput.Static.GetGameControl(controlId).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) : (string) null;
      string str2 = MyInput.Static.GetGameControl(controlId) != null ? MyInput.Static.GetGameControl(controlId).GetControlButtonName(MyGuiInputDeviceEnum.Mouse) : (string) null;
      if (string.IsNullOrEmpty(str1))
        return (object) ("[" + str2 + "]");
      if (string.IsNullOrEmpty(str2))
        return (object) ("[" + str1 + "]");
      return (object) ("[" + str1 + "'/'" + str2 + "]");
    }

    public static object GetHighlightedControl(string text) => (object) ("[" + text + "]");

    public virtual void OnActivated()
    {
    }

    public virtual void OnBeforeActivate()
    {
    }

    public virtual void CleanUp()
    {
    }

    public virtual bool IsCritical() => false;

    protected interface IHelplet
    {
      void OnActivated();

      void CleanUp();
    }
  }
}
