// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MySpaceServerFilterOptions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Utils;

namespace Sandbox.Game.Screens
{
  public class MySpaceServerFilterOptions : MyServerFilterOptions
  {
    public const byte SPACE_BOOL_OFFSET = 128;

    public MySpaceServerFilterOptions()
    {
    }

    public MySpaceServerFilterOptions(MyObjectBuilder_ServerFilterOptions ob)
      : base(ob)
    {
    }

    public MyFilterRange GetFilter(MySpaceNumericOptionEnum key) => (MyFilterRange) this.Filters[(byte) key];

    public MyFilterBool GetFilter(MySpaceBoolOptionEnum key) => (MyFilterBool) this.Filters[(byte) key];

    protected override Dictionary<byte, IMyFilterOption> CreateFilters()
    {
      Dictionary<byte, IMyFilterOption> dictionary = new Dictionary<byte, IMyFilterOption>();
      foreach (byte key in Enum.GetValues(typeof (MySpaceNumericOptionEnum)))
        dictionary.Add(key, (IMyFilterOption) new MyFilterRange());
      foreach (byte key in Enum.GetValues(typeof (MySpaceBoolOptionEnum)))
        dictionary.Add(key, (IMyFilterOption) new MyFilterBool());
      return dictionary;
    }

    public override bool FilterServer(MyCachedServerItem server)
    {
      MyObjectBuilder_SessionSettings settings = server.Settings;
      if (settings == null)
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by no settings");
        return false;
      }
      if (!this.GetFilter(MySpaceNumericOptionEnum.InventoryMultipier).IsMatch(settings.InventorySizeMultiplier))
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by InventorySizeMultiplier");
        return false;
      }
      if (!this.GetFilter(MySpaceNumericOptionEnum.EnvionmentHostility).IsMatch((float) settings.EnvironmentHostility))
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by EnvironmentHostility");
        return false;
      }
      MyFilterRange filter = this.GetFilter(MySpaceNumericOptionEnum.ProductionMultipliers);
      if (!filter.IsMatch(settings.AssemblerEfficiencyMultiplier) || !filter.IsMatch(settings.AssemblerSpeedMultiplier) || !filter.IsMatch(settings.RefinerySpeedMultiplier))
      {
        MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by ProductionMultipliers");
        return false;
      }
      if (!this.GetFilter(MySpaceBoolOptionEnum.Spectator).IsMatch((object) settings.EnableSpectator) || !this.GetFilter(MySpaceBoolOptionEnum.CopyPaste).IsMatch((object) settings.EnableCopyPaste) || (!this.GetFilter(MySpaceBoolOptionEnum.ThrusterDamage).IsMatch((object) settings.ThrusterDamage) || !this.GetFilter(MySpaceBoolOptionEnum.PermanentDeath).IsMatch((object) settings.PermanentDeath)) || (!this.GetFilter(MySpaceBoolOptionEnum.Weapons).IsMatch((object) settings.WeaponsEnabled) || !this.GetFilter(MySpaceBoolOptionEnum.CargoShips).IsMatch((object) settings.CargoShipsEnabled) || (!this.GetFilter(MySpaceBoolOptionEnum.BlockDestruction).IsMatch((object) settings.DestructibleBlocks) || !this.GetFilter(MySpaceBoolOptionEnum.Scripts).IsMatch((object) settings.EnableIngameScripts))) || (!this.GetFilter(MySpaceBoolOptionEnum.Oxygen).IsMatch((object) settings.EnableOxygen) || !this.GetFilter(MySpaceBoolOptionEnum.ThirdPerson).IsMatch((object) settings.Enable3rdPersonView) || (!this.GetFilter(MySpaceBoolOptionEnum.Encounters).IsMatch((object) settings.EnableEncounters) || !this.GetFilter(MySpaceBoolOptionEnum.Airtightness).IsMatch((object) settings.EnableOxygenPressurization)) || (!this.GetFilter(MySpaceBoolOptionEnum.UnsupportedStations).IsMatch((object) settings.StationVoxelSupport) || !this.GetFilter(MySpaceBoolOptionEnum.VoxelDestruction).IsMatch((object) settings.EnableVoxelDestruction) || (!this.GetFilter(MySpaceBoolOptionEnum.Drones).IsMatch((object) settings.EnableDrones) || !this.GetFilter(MySpaceBoolOptionEnum.Wolves).IsMatch((object) settings.EnableWolfs)))) || (!this.GetFilter(MySpaceBoolOptionEnum.Spiders).IsMatch((object) settings.EnableSpiders) || !this.GetFilter(MySpaceBoolOptionEnum.RespawnShips).IsMatch((object) settings.EnableRespawnShips)))
        return false;
      if (server.Rules != null && this.GetFilter(MySpaceBoolOptionEnum.ExternalServerManagement).IsMatch((object) server.Rules.ContainsKey("SM")))
        return true;
      MyLog.Default.WriteLine("Server filtered: " + server.Server.Name + " by ExternalServerManagement");
      return false;
    }

    public override bool FilterLobby(IMyLobby lobby)
    {
      if (!this.GetFilter(MySpaceNumericOptionEnum.InventoryMultipier).IsMatch(MyMultiplayerLobby.GetLobbyFloat("inventoryMultiplier", lobby, 1f)))
        return false;
      MyFilterRange filter = this.GetFilter(MySpaceNumericOptionEnum.ProductionMultipliers);
      return filter.IsMatch(MyMultiplayerLobby.GetLobbyFloat("refineryMultiplier", lobby, 1f)) && filter.IsMatch(MyMultiplayerLobby.GetLobbyFloat("assemblerMultiplier", lobby, 1f));
    }

    public override MySessionSearchFilter GetNetworkFilter(
      MySupportedPropertyFilters supportedFilters,
      string searchText)
    {
      MyServerFilterOptions.MySessionSearchFilterHelper searchFilterHelper = MyServerFilterOptions.MySessionSearchFilterHelper.Create(supportedFilters);
      if (!string.IsNullOrWhiteSpace(searchText))
        searchFilterHelper.AddConditional("SERVER_PROP_NAMES", MySearchCondition.Contains, (object) searchText);
      if (this.SameVersion)
        searchFilterHelper.AddConditional("SERVER_PROP_DATA", MySearchCondition.Equal, (object) MyFinalBuildConstants.APP_VERSION);
      if (MyPlatformGameSettings.CONSOLE_COMPATIBLE)
        searchFilterHelper.AddCustomConditional("CONSOLE_COMPATIBLE", MySearchCondition.Equal, (object) "1");
      if (this.CheckPlayer)
      {
        searchFilterHelper.AddConditional("SERVER_PROP_PLAYER_COUNT", MySearchCondition.GreaterOrEqual, (object) this.PlayerCount.Min);
        searchFilterHelper.AddConditional("SERVER_PROP_PLAYER_COUNT", MySearchCondition.LesserOrEqual, (object) this.PlayerCount.Max);
      }
      if (this.SurvivalMode != this.CreativeMode)
      {
        if (this.SurvivalMode)
          searchFilterHelper.AddConditional("SERVER_PROP_TAGS", MySearchCondition.Contains, (object) "gamemodeS");
        if (this.CreativeMode)
          searchFilterHelper.AddConditional("SERVER_PROP_TAGS", MySearchCondition.Contains, (object) "gamemodeC");
      }
      if (MyFakes.ENABLE_MP_DATA_HASHES && this.SameData)
        searchFilterHelper.AddConditional("SERVER_PROP_TAGS", MySearchCondition.Contains, (object) ("datahash" + MyDataIntegrityChecker.GetHashBase64()));
      if (this.Ping > -1)
        searchFilterHelper.AddConditional("SERVER_PROP_PING", MySearchCondition.LesserOrEqual, (object) this.Ping);
      return searchFilterHelper.Filter;
    }
  }
}
