// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyAlesDebugInputComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Graphics.GUI;
using System;
using System.Linq;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  internal class MyAlesDebugInputComponent : MyDebugComponent
  {
    private bool m_questlogOpened;
    private MyGuiScreenBase guiScreen;
    private static MyRandom random = new MyRandom();
    private MyRandom m_random;

    public override string GetName() => "Ales";

    public MyAlesDebugInputComponent()
    {
      this.m_random = new MyRandom();
      this.AddShortcut(MyKeys.U, true, false, false, false, (Func<string>) (() => "Reload particles"), (Func<bool>) (() =>
      {
        this.ReloadParticleDefinition();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad0, true, false, false, false, (Func<string>) (() => "Teleport to gps"), (Func<bool>) (() =>
      {
        this.TravelToWaypointClient();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad0, true, false, false, false, (Func<string>) (() => "Init questlog"), (Func<bool>) (() =>
      {
        this.ToggleQuestlog();
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad1, true, false, false, false, (Func<string>) (() => "Show/Hide QL"), (Func<bool>) (() =>
      {
        this.m_questlogOpened = !this.m_questlogOpened;
        MyHud.Questlog.Visible = this.m_questlogOpened;
        return true;
      }));
      this.AddShortcut(MyKeys.NumPad2, true, false, false, false, (Func<string>) (() => "QL: Prew page"), (Func<bool>) (() => true));
      this.AddShortcut(MyKeys.NumPad3, true, false, false, false, (Func<string>) (() => "QL: Next page"), (Func<bool>) (() => true));
      int shortLine = 30;
      this.AddShortcut(MyKeys.NumPad4, true, false, false, false, (Func<string>) (() => "QL: Add short line"), (Func<bool>) (() =>
      {
        MyHud.Questlog.AddDetail(MyAlesDebugInputComponent.RandomString(shortLine));
        return true;
      }));
      int longLine = 60;
      this.AddShortcut(MyKeys.NumPad5, true, false, false, false, (Func<string>) (() => "QL: Add long line"), (Func<bool>) (() =>
      {
        MyHud.Questlog.AddDetail(MyAlesDebugInputComponent.RandomString(longLine));
        return true;
      }));
    }

    private void ToggleQuestlog()
    {
      MyHud.Questlog.QuestTitle = "Test Questlog title message";
      MyHud.Questlog.CleanDetails();
    }

    private void TravelToWaypointClient() => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenDialogTeleportCheat());

    [Event(null, 207)]
    [Reliable]
    [Server]
    public static void TravelToWaypoint(Vector3D pos) => MyMultiplayer.TeleportControlledEntity(pos);

    private void ReloadParticleDefinition() => MyDefinitionManager.Static.ReloadParticles();

    public static string RandomString(int length) => new string(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789           ", length).Select<string, char>((Func<string, char>) (s => s[MyAlesDebugInputComponent.random.Next(s.Length)])).ToArray<char>()).Trim();

    protected sealed class TravelToWaypoint\u003C\u003EVRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in Vector3D pos,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyAlesDebugInputComponent.TravelToWaypoint(pos);
      }
    }
  }
}
