// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.DebugInputComponents.MyReloadTestComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using System;
using System.Diagnostics;
using System.Linq;
using VRage.Game.Components;
using VRage.Utils;

namespace Sandbox.Game.Gui.DebugInputComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
  internal class MyReloadTestComponent : MySessionComponentBase
  {
    public static bool Enabled;

    public override void UpdateAfterSimulation()
    {
      if (!MyReloadTestComponent.Enabled || !MySandboxGame.IsGameReady || MySession.Static == null || MySession.Static.ElapsedPlayTime.TotalSeconds <= 5.0)
        return;
      GC.Collect(2, GCCollectionMode.Forced);
      MyLog log1 = MySandboxGame.Log;
      long num = GC.GetTotalMemory(false);
      string msg1 = string.Format("RELOAD TEST, Game GC: {0} B", (object) num.ToString("##,#"));
      log1.WriteLine(msg1);
      MyLog log2 = MySandboxGame.Log;
      num = Process.GetCurrentProcess().PrivateMemorySize64;
      string msg2 = string.Format("RELOAD TEST, Game WS: {0} B", (object) num.ToString("##,#"));
      log2.WriteLine(msg2);
      MySessionLoader.UnloadAndExitToMenu();
    }

    public static void DoReload()
    {
      GC.Collect(2, GCCollectionMode.Forced);
      MySandboxGame.Log.WriteLine(string.Format("RELOAD TEST, Menu GC: {0} B", (object) GC.GetTotalMemory(false).ToString("##,#")));
      MySandboxGame.Log.WriteLine(string.Format("RELOAD TEST, Menu WS: {0} B", (object) Process.GetCurrentProcess().PrivateMemorySize64.ToString("##,#")));
      Tuple<string, MyWorldInfo> tuple = MyLocalCache.GetAvailableWorldInfos().OrderByDescending<Tuple<string, MyWorldInfo>, DateTime>((Func<Tuple<string, MyWorldInfo>, DateTime>) (s => s.Item2.LastSaveTime)).FirstOrDefault<Tuple<string, MyWorldInfo>>();
      if (tuple == null)
        return;
      MySessionLoader.LoadSingleplayerSession(tuple.Item1);
    }
  }
}
