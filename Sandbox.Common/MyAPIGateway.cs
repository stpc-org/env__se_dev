// Decompiled with JetBrains decompiler
// Type: Sandbox.ModAPI.MyAPIGateway
// Assembly: Sandbox.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 59BF3DFC-03D1-4F5E-90FB-54CDF536D906
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Common.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
  public static class MyAPIGateway
  {
    [Obsolete("Use IMyGui.GuiControlCreated")]
    public static Action<object> GuiControlCreated;
    public static IMyPlayerCollection Players;
    public static IMyCubeBuilder CubeBuilder;
    public static IMyTerminalActionsHelper TerminalActionsHelper;
    public static IMyTerminalControls TerminalControls;
    public static IMyUtilities Utilities;
    public static IMyMultiplayer Multiplayer;
    public static IMyParallelTask Parallel;
    public static IMyPhysics Physics;
    public static IMyGui Gui;
    public static IMyPrefabManager PrefabManager;
    public static IMyIngameScripting IngameScripting;
    public static IMyInput Input;
    public static IMyContractSystem ContractSystem;
    private static IMyEntities m_entitiesStorage;
    private static IMySession m_sessionStorage;
    public static IMyGridGroups GridGroups;
    public static IMyReflection Reflection;
    public static IMySpectatorTools SpectatorTools;

    public static IMySession Session
    {
      get => MyAPIGateway.m_sessionStorage;
      set => MyAPIGateway.m_sessionStorage = value;
    }

    public static IMyEntities Entities
    {
      get => MyAPIGateway.m_entitiesStorage;
      set
      {
        MyAPIGateway.m_entitiesStorage = value;
        if (MyAPIGateway.Entities != null)
        {
          MyAPIGatewayShortcuts.RegisterEntityUpdate = new Action<IMyEntity>(MyAPIGateway.Entities.RegisterForUpdate);
          MyAPIGatewayShortcuts.UnregisterEntityUpdate = new Action<IMyEntity, bool>(MyAPIGateway.Entities.UnregisterForUpdate);
        }
        else
        {
          MyAPIGatewayShortcuts.RegisterEntityUpdate = (Action<IMyEntity>) null;
          MyAPIGatewayShortcuts.UnregisterEntityUpdate = (Action<IMyEntity, bool>) null;
        }
      }
    }

    [Conditional("DEBUG")]
    [Obsolete]
    public static void GetMessageBoxPointer(ref IntPtr pointer)
    {
      IntPtr hModule = MyAPIGateway.LoadLibrary("user32.dll");
      pointer = MyAPIGateway.GetProcAddress(hModule, "MessageBoxW");
    }

    [DllImport("kernel32.dll")]
    private static extern IntPtr LoadLibrary(string dllname);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

    [Obsolete]
    public static void Clean()
    {
      MyAPIGateway.Session = (IMySession) null;
      MyAPIGateway.Entities = (IMyEntities) null;
      MyAPIGateway.Players = (IMyPlayerCollection) null;
      MyAPIGateway.CubeBuilder = (IMyCubeBuilder) null;
      if (MyAPIGateway.IngameScripting != null)
        MyAPIGateway.IngameScripting.Clean();
      MyAPIGateway.IngameScripting = (IMyIngameScripting) null;
      MyAPIGateway.TerminalActionsHelper = (IMyTerminalActionsHelper) null;
      MyAPIGateway.Utilities = (IMyUtilities) null;
      MyAPIGateway.Parallel = (IMyParallelTask) null;
      MyAPIGateway.Physics = (IMyPhysics) null;
      MyAPIGateway.Multiplayer = (IMyMultiplayer) null;
      MyAPIGateway.PrefabManager = (IMyPrefabManager) null;
      MyAPIGateway.Input = (IMyInput) null;
      MyAPIGateway.TerminalControls = (IMyTerminalControls) null;
      MyAPIGateway.GridGroups = (IMyGridGroups) null;
    }

    [Obsolete]
    public static StringBuilder DoorBase(string name)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in name)
      {
        if (ch == ' ')
          stringBuilder.Append(ch);
        byte num = (byte) ch;
        for (int index = 0; index < 8; ++index)
        {
          stringBuilder.Append(((int) num & 128) != 0 ? "Door" : "Base");
          num <<= 1;
        }
      }
      return stringBuilder;
    }
  }
}
