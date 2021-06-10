// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.MyServerFilterOptions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Screens
{
  public abstract class MyServerFilterOptions
  {
    public bool AllowedGroups;
    public bool SameVersion;
    public bool SameData;
    public bool? HasPassword;
    public bool CreativeMode;
    public bool SurvivalMode;
    public bool AdvancedFilter;
    public int Ping;
    public bool CheckPlayer;
    public SerializableRange PlayerCount;
    public bool CheckMod;
    public SerializableRange ModCount;
    public bool CheckDistance;
    public SerializableRange ViewDistance;
    public bool ModsExclusive;
    public HashSet<WorkshopId> Mods = new HashSet<WorkshopId>();
    private Dictionary<byte, IMyFilterOption> m_filters;
    public const int MAX_PING = 150;

    public Dictionary<byte, IMyFilterOption> Filters
    {
      get => this.m_filters ?? (this.m_filters = this.CreateFilters());
      set => this.m_filters = value;
    }

    public MyServerFilterOptions() => this.SetDefaults();

    public void SetDefaults(bool resetMods = false)
    {
      this.AdvancedFilter = false;
      this.CheckPlayer = false;
      this.CheckMod = false;
      this.CheckDistance = false;
      this.AllowedGroups = true;
      this.SameVersion = true;
      this.SameData = true;
      this.CreativeMode = true;
      this.SurvivalMode = true;
      this.HasPassword = new bool?();
      this.Ping = 150;
      this.m_filters = this.CreateFilters();
      if (!resetMods)
        return;
      this.Mods.Clear();
    }

    public MyServerFilterOptions(MyObjectBuilder_ServerFilterOptions ob) => this.Init(ob);

    protected abstract Dictionary<byte, IMyFilterOption> CreateFilters();

    public abstract bool FilterServer(MyCachedServerItem server);

    public abstract bool FilterLobby(IMyLobby lobby);

    public abstract MySessionSearchFilter GetNetworkFilter(
      MySupportedPropertyFilters supportedFilters,
      string searchText);

    public MyObjectBuilder_ServerFilterOptions GetObjectBuilder()
    {
      MyObjectBuilder_ServerFilterOptions serverFilterOptions1 = new MyObjectBuilder_ServerFilterOptions();
      serverFilterOptions1.AllowedGroups = this.AllowedGroups;
      serverFilterOptions1.SameVersion = this.SameVersion;
      serverFilterOptions1.SameData = this.SameData;
      serverFilterOptions1.HasPassword = this.HasPassword;
      serverFilterOptions1.CreativeMode = this.CreativeMode;
      serverFilterOptions1.SurvivalMode = this.SurvivalMode;
      serverFilterOptions1.CheckPlayer = this.CheckPlayer;
      serverFilterOptions1.PlayerCount = this.PlayerCount;
      serverFilterOptions1.CheckMod = this.CheckMod;
      serverFilterOptions1.ModCount = this.ModCount;
      serverFilterOptions1.CheckDistance = this.CheckDistance;
      serverFilterOptions1.ViewDistance = this.ViewDistance;
      serverFilterOptions1.Advanced = this.AdvancedFilter;
      serverFilterOptions1.Ping = this.Ping;
      MyObjectBuilder_ServerFilterOptions serverFilterOptions2 = serverFilterOptions1;
      HashSet<WorkshopId> mods = this.Mods;
      List<WorkshopId> workshopIdList = mods != null ? mods.ToList<WorkshopId>() : (List<WorkshopId>) null;
      serverFilterOptions2.WorkshopMods = workshopIdList;
      serverFilterOptions1.ModsExclusive = this.ModsExclusive;
      serverFilterOptions1.Filters = new SerializableDictionary<byte, string>();
      foreach (KeyValuePair<byte, IMyFilterOption> filter in this.Filters)
        serverFilterOptions1.Filters[filter.Key] = filter.Value.SerializedValue;
      return serverFilterOptions1;
    }

    public void Init(MyObjectBuilder_ServerFilterOptions ob)
    {
      this.AllowedGroups = ob.AllowedGroups;
      this.SameVersion = ob.SameVersion;
      this.SameData = ob.SameData;
      this.HasPassword = ob.HasPassword;
      this.CreativeMode = ob.CreativeMode;
      this.SurvivalMode = ob.SurvivalMode;
      this.CheckPlayer = ob.CheckPlayer;
      this.PlayerCount = ob.PlayerCount;
      this.CheckMod = ob.CheckMod;
      this.ModCount = ob.ModCount;
      this.CheckDistance = ob.CheckDistance;
      this.ViewDistance = ob.ViewDistance;
      this.AdvancedFilter = ob.Advanced;
      this.Ping = ob.Ping;
      this.ModsExclusive = ob.ModsExclusive;
      this.Mods = ob.WorkshopMods == null ? new HashSet<WorkshopId>() : new HashSet<WorkshopId>((IEnumerable<WorkshopId>) ob.WorkshopMods);
      if (ob.Filters == null)
        return;
      foreach (KeyValuePair<byte, string> keyValuePair in ob.Filters.Dictionary)
      {
        IMyFilterOption myFilterOption;
        if (!this.Filters.TryGetValue(keyValuePair.Key, out myFilterOption))
          throw new Exception("Unrecognized filter key");
        myFilterOption.Configure(keyValuePair.Value);
      }
    }

    protected struct MySessionSearchFilterHelper
    {
      public MySessionSearchFilter Filter;
      public Dictionary<string, MySearchConditionFlags> SupportedConditions;

      public static MyServerFilterOptions.MySessionSearchFilterHelper Create(
        MySupportedPropertyFilters supportedFilters)
      {
        return new MyServerFilterOptions.MySessionSearchFilterHelper()
        {
          Filter = new MySessionSearchFilter(),
          SupportedConditions = supportedFilters.ToDictionary<MySupportedPropertyFilters.Entry, string, MySearchConditionFlags>((Func<MySupportedPropertyFilters.Entry, string>) (x => x.Property), (Func<MySupportedPropertyFilters.Entry, MySearchConditionFlags>) (x => x.SupportedConditions))
        };
      }

      public void AddConditional(string property, MySearchCondition condition, object value)
      {
        MySearchConditionFlags self;
        if (!this.SupportedConditions.TryGetValue(property, out self) || !self.Contains(condition))
          return;
        this.Filter.AddQuery(property, condition, value.ToString());
      }

      public void AddCustomConditional(string property, MySearchCondition condition, object value)
      {
        MySearchConditionFlags self;
        if (!this.SupportedConditions.TryGetValue("SERVER_CPROP_", out self) || !self.Contains(condition))
          return;
        this.Filter.AddQueryCustom(property, condition, value.ToString());
      }
    }
  }
}
