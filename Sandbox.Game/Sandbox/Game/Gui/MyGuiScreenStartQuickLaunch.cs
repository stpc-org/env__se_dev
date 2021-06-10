// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenStartQuickLaunch
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using VRage.Game;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenStartQuickLaunch : MyGuiScreenBase
  {
    private MyQuickLaunchType m_quickLaunchType;
    private bool m_childScreenLaunched;
    public static MyGuiScreenStartQuickLaunch CurrentScreen;

    public MyGuiScreenStartQuickLaunch(MyQuickLaunchType quickLaunchType, MyStringId progressText)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR))
    {
      this.m_quickLaunchType = quickLaunchType;
      MyGuiScreenStartQuickLaunch.CurrentScreen = this;
    }

    public override void LoadContent() => base.LoadContent();

    public override string GetFriendlyName() => nameof (MyGuiScreenStartQuickLaunch);

    private static MyWorldGenerator.Args CreateBasicQuickstartArgs() => new MyWorldGenerator.Args()
    {
      Scenario = MyDefinitionManager.Static.GetScenarioDefinition(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ScenarioDefinition), "EasyStart1")),
      AsteroidAmount = 0
    };

    private static MyObjectBuilder_SessionSettings CreateBasicQuickStartSettings()
    {
      MyObjectBuilder_SessionSettings newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_SessionSettings>();
      newObject.GameMode = MyGameModeEnum.Creative;
      newObject.EnableToolShake = true;
      newObject.EnableSunRotation = MyPerGameSettings.Game == GameEnum.SE_GAME;
      newObject.VoxelGeneratorVersion = 4;
      newObject.CargoShipsEnabled = true;
      newObject.EnableOxygen = true;
      newObject.EnableSpiders = false;
      newObject.EnableWolfs = false;
      MyWorldGenerator.SetProceduralSettings(new int?(-1), newObject);
      return newObject;
    }

    public static void QuickstartSandbox(
      MyObjectBuilder_SessionSettings quickstartSettings,
      MyWorldGenerator.Args? quickstartArgs)
    {
      MyLog.Default.WriteLine("QuickstartSandbox - START");
      MyScreenManager.RemoveAllScreensExcept((MyGuiScreenBase) null);
      MySessionLoader.StartLoading((Action) (() =>
      {
        MyObjectBuilder_SessionSettings settings = quickstartSettings ?? MyGuiScreenStartQuickLaunch.CreateBasicQuickStartSettings();
        MyWorldGenerator.Args generationArgs = quickstartArgs ?? MyGuiScreenStartQuickLaunch.CreateBasicQuickstartArgs();
        List<MyObjectBuilder_Checkpoint.ModItem> mods = new List<MyObjectBuilder_Checkpoint.ModItem>(0);
        MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Quickstart);
        MySession.Start("Created " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "", "", settings, mods, generationArgs);
      }));
      MyLog.Default.WriteLine("QuickstartSandbox - END");
    }

    public override bool Update(bool hasFocus)
    {
      if (!hasFocus)
        return base.Update(hasFocus);
      if (this.m_childScreenLaunched & hasFocus)
        this.CloseScreenNow();
      if (this.m_childScreenLaunched)
        return base.Update(hasFocus);
      if (MyInput.Static.IsKeyPress(MyKeys.Escape))
      {
        MySessionLoader.UnloadAndExitToMenu();
        return base.Update(hasFocus);
      }
      switch (this.m_quickLaunchType)
      {
        case MyQuickLaunchType.NEW_SANDBOX:
          MyGuiScreenStartQuickLaunch.QuickstartSandbox((MyObjectBuilder_SessionSettings) null, new MyWorldGenerator.Args?());
          this.m_childScreenLaunched = true;
          break;
        case MyQuickLaunchType.LAST_SANDBOX:
          string lastSessionPath = MyLocalCache.GetLastSessionPath();
          if (lastSessionPath != null && (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD && MyCloudHelper.ExtractFilesTo(MyCloudHelper.LocalToCloudWorldPath(lastSessionPath + "/"), lastSessionPath, false) || Directory.Exists(lastSessionPath)))
            MySessionLoader.LoadSingleplayerSession(lastSessionPath);
          else
            MySandboxGame.Static.ShowIntroMessages();
          this.m_childScreenLaunched = true;
          break;
        default:
          throw new InvalidBranchException();
      }
      return base.Update(hasFocus);
    }
  }
}
