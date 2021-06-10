// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.MySpaceGameCustomInitialization
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.Definitions.SafeZone;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Scripting;
using VRageMath;

namespace SpaceEngineers.Game
{
  public class MySpaceGameCustomInitialization : MySandboxGame.IGameCustomInitialization
  {
    public void InitIlChecker()
    {
      using (IMyWhitelistBatch myWhitelistBatch = MyVRage.Platform.Scripting.OpenWhitelistBatch())
      {
        myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.Both, typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyButtonPanel), typeof (LandingGearMode));
        myWhitelistBatch.AllowNamespaceOfTypes(MyWhitelistTarget.ModApi, typeof (SpaceEngineers.Game.ModAPI.IMyButtonPanel), typeof (MySafeZoneBlockDefinition));
      }
    }

    public void InitIlCompiler()
    {
      List<string> stringList = new List<string>()
      {
        Path.Combine(Assembly.Load("netstandard").Location),
        Path.Combine(MyFileSystem.ExePath, "Sandbox.Game.dll"),
        Path.Combine(MyFileSystem.ExePath, "Sandbox.Common.dll"),
        Path.Combine(MyFileSystem.ExePath, "Sandbox.Graphics.dll"),
        Path.Combine(MyFileSystem.ExePath, "VRage.dll"),
        Path.Combine(MyFileSystem.ExePath, "VRage.Library.dll"),
        Path.Combine(MyFileSystem.ExePath, "VRage.Math.dll"),
        Path.Combine(MyFileSystem.ExePath, "VRage.Game.dll"),
        Path.Combine(MyFileSystem.ExePath, "VRage.Render.dll"),
        Path.Combine(MyFileSystem.ExePath, "VRage.Input.dll"),
        Path.Combine(MyFileSystem.ExePath, "SpaceEngineers.ObjectBuilders.dll"),
        Path.Combine(MyFileSystem.ExePath, "SpaceEngineers.Game.dll"),
        Path.Combine(MyFileSystem.ExePath, "System.Collections.Immutable.dll"),
        Path.Combine(MyFileSystem.ExePath, "ProtoBuf.Net.Core.dll")
      };
      foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
      {
        string name = assembly.GetName().Name;
        if (name == "System.Runtime" || name == "System.Collections")
          stringList.Add(assembly.Location);
      }
      MyVRage.Platform.Scripting.Initialize(MySandboxGame.Static.UpdateThread, (IEnumerable<string>) stringList, new Type[14]
      {
        typeof (MyTuple),
        typeof (Vector2),
        typeof (VRage.Game.Game),
        typeof (ITerminalAction),
        typeof (IMyGridTerminalSystem),
        typeof (MyModelComponent),
        typeof (IMyComponentAggregate),
        typeof (ListReader<>),
        typeof (MyObjectBuilder_FactionDefinition),
        typeof (IMyCubeBlock),
        typeof (MyIni),
        typeof (ImmutableArray),
        typeof (SpaceEngineers.Game.ModAPI.Ingame.IMyAirVent),
        typeof (MySprite)
      }, new string[6]
      {
        this.GetPrefixedBranchName(),
        "STABLE",
        string.Empty,
        string.Empty,
        "VERSION_" + (object) ((Version) MyFinalBuildConstants.APP_VERSION).Minor,
        "BUILD_" + (object) ((Version) MyFinalBuildConstants.APP_VERSION).Build
      }, MyFakes.ENABLE_ROSLYN_SCRIPT_DIAGNOSTICS ? Path.Combine(MyFileSystem.UserDataPath, "ScriptDiagnostics") : (string) null, (MyFakes.ENABLE_SCRIPTS_PDB ? 1 : 0) != 0);
    }

    private string GetPrefixedBranchName()
    {
      string branchName = MyGameService.BranchName;
      return "BRANCH_" + (!string.IsNullOrEmpty(branchName) ? Regex.Replace(branchName, "[^a-zA-Z0-9_]", "_").ToUpper() : "STABLE");
    }
  }
}
