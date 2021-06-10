// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentWarningSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 1000, typeof (MyObjectBuilder_SessionComponent), null, false)]
  public class MySessionComponentWarningSystem : MySessionComponentBase
  {
    private static MySessionComponentWarningSystem m_static;
    private bool m_warningsDirty;
    private Dictionary<MySessionComponentWarningSystem.WarningKey, MySessionComponentWarningSystem.Warning> m_warnings;
    private bool m_updateRequested;
    private List<MySessionComponentWarningSystem.WarningData> m_serverWarnings;
    private Dictionary<long, MySessionComponentWarningSystem.WarningData> m_warningData;
    private HashSet<long> m_suppressedWarnings = new HashSet<long>();
    private int m_updateCounter;
    private List<MySessionComponentWarningSystem.WarningData> m_cachedUpdateList;

    public static MySessionComponentWarningSystem Static => MySessionComponentWarningSystem.m_static;

    public Dictionary<MySessionComponentWarningSystem.WarningKey, MySessionComponentWarningSystem.Warning>.ValueCollection CurrentWarnings
    {
      get
      {
        if (this.m_warningsDirty)
        {
          this.m_warnings.Clear();
          this.m_warningsDirty = false;
          foreach (MySessionComponentWarningSystem.WarningData serverWarning in this.m_serverWarnings)
            MySessionComponentWarningSystem.MergeWarning(this.m_warnings, serverWarning.ConstructWarning());
          foreach (MySessionComponentWarningSystem.WarningData warningData in this.m_warningData.Values)
            MySessionComponentWarningSystem.MergeWarning(this.m_warnings, warningData.ConstructWarning());
          this.UpdateImmediateWarnings((Action<MySessionComponentWarningSystem.WarningData>) (x =>
          {
            MySessionComponentWarningSystem.Warning warning = x.ConstructWarning();
            MySessionComponentWarningSystem.WarningKey key = warning.GetKey();
            if (MySessionComponentWarningSystem.m_static.m_warnings.ContainsKey(key))
              return;
            MySessionComponentWarningSystem.m_static.m_warnings.Add(key, warning);
          }));
        }
        return this.m_warnings.Values;
      }
    }

    public override void LoadData()
    {
      base.LoadData();
      MySessionComponentWarningSystem.m_static = this;
      this.m_warningData = new Dictionary<long, MySessionComponentWarningSystem.WarningData>();
      this.m_serverWarnings = new List<MySessionComponentWarningSystem.WarningData>();
      this.m_warnings = new Dictionary<MySessionComponentWarningSystem.WarningKey, MySessionComponentWarningSystem.Warning>();
      List<string> suppressedWarnings = this.Session.SessionSettings.SuppressedWarnings;
      if (suppressedWarnings == null)
        return;
      foreach (string str in suppressedWarnings)
        this.m_suppressedWarnings.Add((long) MyStringId.GetOrCompute(str).Id);
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MySessionComponentWarningSystem.m_static = (MySessionComponentWarningSystem) null;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      ++this.m_updateCounter;
      bool isServer = Sync.IsServer;
      bool isDedicated = Sync.IsDedicated;
      if ((double) this.m_updateCounter < 60.0 * (isDedicated ? 60.0 : 10.0) && ((double) this.m_updateCounter < 60.0 || !this.m_updateRequested))
        return;
      this.m_updateCounter = 0;
      this.m_updateRequested = false;
      if (isServer)
        this.UpdateServerWarnings();
      if (!isDedicated)
        this.UpdateClientWarnings();
      this.m_warningsDirty = true;
    }

    private void UpdateServerWarnings()
    {
      using (MyUtils.ClearCollectionToken<List<MySessionComponentWarningSystem.WarningData>, MySessionComponentWarningSystem.WarningData> clearCollectionToken = MyUtils.ReuseCollection<MySessionComponentWarningSystem.WarningData>(ref this.m_cachedUpdateList))
      {
        List<MySessionComponentWarningSystem.WarningData> collection = clearCollectionToken.Collection;
        this.UpdateImmediateWarnings(new Action<MySessionComponentWarningSystem.WarningData>(collection.Add));
        collection.AddRange((IEnumerable<MySessionComponentWarningSystem.WarningData>) this.m_warningData.Values);
        DateTime now = DateTime.Now;
        for (int index = 0; index < collection.Count; ++index)
        {
          MySessionComponentWarningSystem.WarningData warningData = collection[index];
          if (warningData.LastOccurence.HasValue)
          {
            TimeSpan timeSpan = now - warningData.LastOccurence.Value;
            if (timeSpan < TimeSpan.Zero)
              timeSpan = TimeSpan.Zero;
            warningData.LastOccurence = new DateTime?(DateTime.MinValue + timeSpan);
            collection[index] = warningData;
          }
        }
        MyMultiplayer.RaiseStaticEvent<List<MySessionComponentWarningSystem.WarningData>>((Func<IMyEventOwner, Action<List<MySessionComponentWarningSystem.WarningData>>>) (x => new Action<List<MySessionComponentWarningSystem.WarningData>>(MySessionComponentWarningSystem.OnUpdateWarnings)), collection);
      }
    }

    [Event(null, 182)]
    [Reliable]
    [Broadcast]
    private static void OnUpdateWarnings(
      List<MySessionComponentWarningSystem.WarningData> warnings)
    {
      DateTime now = DateTime.Now;
      for (int index = 0; index < warnings.Count; ++index)
      {
        MySessionComponentWarningSystem.WarningData warning = warnings[index];
        if (warning.LastOccurence.HasValue)
        {
          warning.LastOccurence = new DateTime?(now + (warning.LastOccurence.Value - DateTime.MinValue));
          warnings[index] = warning;
        }
      }
      MySessionComponentWarningSystem componentWarningSystem = MySessionComponentWarningSystem.Static;
      componentWarningSystem.m_serverWarnings = warnings;
      componentWarningSystem.RequestUpdate();
    }

    private void UpdateImmediateWarnings(
      Action<MySessionComponentWarningSystem.WarningData> add)
    {
      foreach (MyCubeGrid myCubeGrid in MyUnsafeGridsSessionComponent.UnsafeGrids.Values)
      {
        string descriptionString = string.Join(", ", myCubeGrid.UnsafeBlocks.Select<MyCubeBlock, string>((Func<MyCubeBlock, string>) (x => x.DisplayNameText)));
        add(new MySessionComponentWarningSystem.WarningData(myCubeGrid.DisplayName, descriptionString, MySessionComponentWarningSystem.Category.UnsafeGrids));
      }
      foreach (MyTuple<string, MyStringId> watchdogWarning in MyVRage.Platform.Scripting.GetWatchdogWarnings())
      {
        MyStringId description = watchdogWarning.Item2;
        string str = watchdogWarning.Item1 ?? MyTexts.GetString(MyCommonTexts.WorldSettings_Mods);
        add(new MySessionComponentWarningSystem.WarningData(MySessionComponentWarningSystem.Category.Other, MyStringId.NullOrEmpty, str, description, str, new DateTime?()));
      }
    }

    private void UpdateClientWarnings()
    {
      bool drawServerWarnings = MyDebugDrawSettings.DEBUG_DRAW_SERVER_WARNINGS;
      MySession mySession = MySession.Static;
      if ((mySession != null ? (mySession.SimplifiedSimulation ? 1 : 0) : 0) != 0)
        this.AddWarning((long) MyCommonTexts.PerformanceWarningIssues_SimplifiedSimulation_Message.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningIssues_SimplifiedSimulation_Header, MyCommonTexts.PerformanceWarningIssues_SimplifiedSimulation_Message, MySessionComponentWarningSystem.Category.General));
      if (!MySession.Static.HighSimulationQualityNotification | drawServerWarnings)
      {
        if (MyPlatformGameSettings.ENABLE_TRASH_REMOVAL_SETTING)
          this.AddWarning((long) MyCommonTexts.PerformanceWarningIssuesServer_Simspeed.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningAreaPhysics, MyCommonTexts.PerformanceWarningIssuesServer_Simspeed, MySessionComponentWarningSystem.Category.Server));
        else
          this.AddWarning((long) MyCommonTexts.PerformanceWarningIssuesServer_Simspeed.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningAreaPhysics, MyCommonTexts.PerformanceWarningIssuesServer_Simspeed_Simple, MySessionComponentWarningSystem.Category.Server));
      }
      if (MyMultiplayer.Static?.ReplicationLayer is MyReplicationClient replicationLayer && replicationLayer.ReplicationRange.HasValue)
        this.AddWarning((long) MyCommonTexts.PerformanceWarningIssuesServer_ReducedReplicationRange.Id, new MySessionComponentWarningSystem.WarningData(MyTexts.GetString(MyCommonTexts.PerformanceWarningHeading_ReducedReplicationRange), string.Format(MyTexts.GetString(MyCommonTexts.PerformanceWarningIssuesServer_ReducedReplicationRange), (object) replicationLayer.ReplicationRange), MySessionComponentWarningSystem.Category.Server));
      if (MySession.Static.ServerSaving | drawServerWarnings)
        this.AddWarning((long) MyCommonTexts.PerformanceWarningIssuesServer_Saving.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningHeading_Saving, MyCommonTexts.PerformanceWarningIssuesServer_Saving, MySessionComponentWarningSystem.Category.Server));
      if (((MySession.Static.MultiplayerAlive ? 0 : (!MySession.Static.ServerSaving ? 1 : 0)) | (drawServerWarnings ? 1 : 0)) != 0)
        this.AddWarning((long) MyCommonTexts.PerformanceWarningIssuesServer_NoConnection.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningIssuesServer_NoConnection, MyCommonTexts.Multiplayer_NoConnection, MySessionComponentWarningSystem.Category.Server));
      if (!MySession.Static.MultiplayerDirect | drawServerWarnings)
        this.AddWarning((long) MyCommonTexts.PerformanceWarningIssuesServer_Direct.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningIssuesServer_Direct, MyCommonTexts.Multiplayer_IndirectConnection, MySessionComponentWarningSystem.Category.Server));
      if (((Sync.IsServer ? 0 : (MySession.Static.MultiplayerPing.Milliseconds > 250.0 ? 1 : 0)) | (drawServerWarnings ? 1 : 0)) != 0)
        this.AddWarning((long) MyCommonTexts.PerformanceWarningIssuesServer_Latency.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningIssuesServer_Latency, MyCommonTexts.Multiplayer_HighPing, MySessionComponentWarningSystem.Category.Server));
      if (MyGeneralStats.Static.LowNetworkQuality | drawServerWarnings)
        this.AddWarning((long) MyCommonTexts.PerformanceWarningIssuesServer_PoorConnection.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningIssuesServer_PoorConnection, MyCommonTexts.Multiplayer_PacketLossDescription, MySessionComponentWarningSystem.Category.Server));
      if (!(MySandboxGame.Static.MemoryState >= MySandboxGame.MemState.Low | drawServerWarnings))
        return;
      this.AddWarning((long) MyCommonTexts.PerformanceWarningIssues_LowOnMemory.Id, new MySessionComponentWarningSystem.WarningData(MyCommonTexts.PerformanceWarningIssues_LowOnMemory, MyCommonTexts.Performance_LowOnMemory, MySessionComponentWarningSystem.Category.Performance));
    }

    public void RequestUpdate()
    {
      this.m_warningsDirty = true;
      this.m_updateRequested = true;
    }

    public void AddWarning(
      long id,
      MySessionComponentWarningSystem.WarningData warning)
    {
      if (this.m_suppressedWarnings.Contains(id))
        return;
      this.m_warningData[id] = warning;
      this.RequestUpdate();
    }

    private static void MergeWarning(
      Dictionary<MySessionComponentWarningSystem.WarningKey, MySessionComponentWarningSystem.Warning> warnings,
      MySessionComponentWarningSystem.Warning warning)
    {
      MySessionComponentWarningSystem.WarningKey key = warning.GetKey();
      MySessionComponentWarningSystem.Warning warning1;
      if (warnings.TryGetValue(key, out warning1))
      {
        if (warning1.Time.HasValue != warning.Time.HasValue || !warning.Time.HasValue || !(warning.Time.Value > warning1.Time.Value))
          return;
        warnings[key] = warning;
      }
      else
        warnings.Add(key, warning);
    }

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MyUnsafeGridsSessionComponent)
    };

    public override bool IsRequiredByGame => true;

    public enum Category
    {
      Graphics,
      Blocks,
      Other,
      UnsafeGrids,
      BlockLimits,
      Server,
      Performance,
      General,
    }

    [Serializable]
    public struct WarningData
    {
      public DateTime? LastOccurence;
      public MySessionComponentWarningSystem.Category Category;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string TitleIdKey;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string TitleString;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string DescriptionIdKey;
      [Serialize(MyObjectFlags.DefaultZero)]
      public string DescriptionString;

      public WarningData(
        MyStringId title,
        MyStringId description,
        MySessionComponentWarningSystem.Category category)
        : this(title, description, category, new DateTime?(DateTime.Now))
      {
      }

      public WarningData(
        MyStringId title,
        MyStringId description,
        MySessionComponentWarningSystem.Category category,
        DateTime? time)
      {
        this.Category = category;
        this.LastOccurence = time;
        this.TitleIdKey = title.String;
        this.DescriptionIdKey = description.String;
        this.TitleString = (string) null;
        this.DescriptionString = (string) null;
      }

      public WarningData(
        string titleString,
        string descriptionString,
        MySessionComponentWarningSystem.Category category,
        DateTime? time = null)
      {
        this.Category = category;
        this.TitleString = titleString;
        this.DescriptionString = descriptionString;
        this.LastOccurence = time;
        this.TitleIdKey = (string) null;
        this.DescriptionIdKey = (string) null;
      }

      public WarningData(
        MySessionComponentWarningSystem.Category category,
        MyStringId title,
        string titleString,
        MyStringId description,
        string descriptionString,
        DateTime? lastOccurence)
      {
        this.Category = category;
        this.TitleIdKey = title.String;
        this.TitleString = titleString;
        this.LastOccurence = lastOccurence;
        this.DescriptionIdKey = description.String;
        this.DescriptionString = descriptionString;
      }

      public MySessionComponentWarningSystem.Warning ConstructWarning() => new MySessionComponentWarningSystem.Warning(MySessionComponentWarningSystem.WarningData.ConstructLocalizedString(this.TitleIdKey, this.TitleString), MySessionComponentWarningSystem.WarningData.ConstructLocalizedString(this.DescriptionIdKey, this.DescriptionString), this.Category, this.LastOccurence);

      private static string ConstructLocalizedString(string formatKey, string strData)
      {
        if (string.IsNullOrEmpty(formatKey))
          return strData;
        string format = MyTexts.GetString(formatKey);
        return strData == null ? format : string.Format(format, (object) strData);
      }

      protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem\u003C\u003EWarningData\u003C\u003ELastOccurence\u003C\u003EAccessor : IMemberAccessor<MySessionComponentWarningSystem.WarningData, DateTime?>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySessionComponentWarningSystem.WarningData owner,
          in DateTime? value)
        {
          owner.LastOccurence = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySessionComponentWarningSystem.WarningData owner,
          out DateTime? value)
        {
          value = owner.LastOccurence;
        }
      }

      protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem\u003C\u003EWarningData\u003C\u003ECategory\u003C\u003EAccessor : IMemberAccessor<MySessionComponentWarningSystem.WarningData, MySessionComponentWarningSystem.Category>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySessionComponentWarningSystem.WarningData owner,
          in MySessionComponentWarningSystem.Category value)
        {
          owner.Category = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySessionComponentWarningSystem.WarningData owner,
          out MySessionComponentWarningSystem.Category value)
        {
          value = owner.Category;
        }
      }

      protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem\u003C\u003EWarningData\u003C\u003ETitleIdKey\u003C\u003EAccessor : IMemberAccessor<MySessionComponentWarningSystem.WarningData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySessionComponentWarningSystem.WarningData owner,
          in string value)
        {
          owner.TitleIdKey = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySessionComponentWarningSystem.WarningData owner,
          out string value)
        {
          value = owner.TitleIdKey;
        }
      }

      protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem\u003C\u003EWarningData\u003C\u003ETitleString\u003C\u003EAccessor : IMemberAccessor<MySessionComponentWarningSystem.WarningData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySessionComponentWarningSystem.WarningData owner,
          in string value)
        {
          owner.TitleString = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySessionComponentWarningSystem.WarningData owner,
          out string value)
        {
          value = owner.TitleString;
        }
      }

      protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem\u003C\u003EWarningData\u003C\u003EDescriptionIdKey\u003C\u003EAccessor : IMemberAccessor<MySessionComponentWarningSystem.WarningData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySessionComponentWarningSystem.WarningData owner,
          in string value)
        {
          owner.DescriptionIdKey = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySessionComponentWarningSystem.WarningData owner,
          out string value)
        {
          value = owner.DescriptionIdKey;
        }
      }

      protected class Sandbox_Game_SessionComponents_MySessionComponentWarningSystem\u003C\u003EWarningData\u003C\u003EDescriptionString\u003C\u003EAccessor : IMemberAccessor<MySessionComponentWarningSystem.WarningData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MySessionComponentWarningSystem.WarningData owner,
          in string value)
        {
          owner.DescriptionString = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MySessionComponentWarningSystem.WarningData owner,
          out string value)
        {
          value = owner.DescriptionString;
        }
      }
    }

    public struct WarningKey
    {
      public readonly string Title;
      public readonly string Description;
      public readonly MySessionComponentWarningSystem.Category Category;

      public WarningKey(
        string title,
        string description,
        MySessionComponentWarningSystem.Category category)
      {
        this.Title = title;
        this.Description = description;
        this.Category = category;
      }

      public bool Equals(MySessionComponentWarningSystem.WarningKey other) => string.Equals(this.Title, other.Title) && string.Equals(this.Description, other.Description) && this.Category == other.Category;

      public override bool Equals(object obj) => obj != null && obj is MySessionComponentWarningSystem.WarningKey other && this.Equals(other);

      public override int GetHashCode() => (int) ((MySessionComponentWarningSystem.Category) (((this.Title != null ? this.Title.GetHashCode() : 0) * 397 ^ (this.Description != null ? this.Description.GetHashCode() : 0)) * 397) ^ this.Category);

      public static bool operator ==(
        MySessionComponentWarningSystem.WarningKey left,
        MySessionComponentWarningSystem.WarningKey right)
      {
        return left.Equals(right);
      }

      public static bool operator !=(
        MySessionComponentWarningSystem.WarningKey left,
        MySessionComponentWarningSystem.WarningKey right)
      {
        return !left.Equals(right);
      }

      public static IEqualityComparer<MySessionComponentWarningSystem.WarningKey> Comparer { get; } = (IEqualityComparer<MySessionComponentWarningSystem.WarningKey>) new MySessionComponentWarningSystem.WarningKey.EqualityComparer();

      private sealed class EqualityComparer : IEqualityComparer<MySessionComponentWarningSystem.WarningKey>
      {
        public bool Equals(
          MySessionComponentWarningSystem.WarningKey x,
          MySessionComponentWarningSystem.WarningKey y)
        {
          return string.Equals(x.Title, y.Title) && string.Equals(x.Description, y.Description) && x.Category == y.Category;
        }

        public int GetHashCode(MySessionComponentWarningSystem.WarningKey obj) => (int) ((MySessionComponentWarningSystem.Category) (((obj.Title != null ? obj.Title.GetHashCode() : 0) * 397 ^ (obj.Description != null ? obj.Description.GetHashCode() : 0)) * 397) ^ obj.Category);
      }
    }

    public struct Warning
    {
      public DateTime? Time;
      public readonly string Title;
      public readonly string Description;
      public readonly MySessionComponentWarningSystem.Category Category;

      public Warning(
        string title,
        string description,
        MySessionComponentWarningSystem.Category category,
        DateTime? time)
      {
        this.Time = time;
        this.Title = title;
        this.Category = category;
        this.Description = description;
      }

      public MySessionComponentWarningSystem.WarningKey GetKey() => new MySessionComponentWarningSystem.WarningKey(this.Title, this.Description, this.Category);
    }

    protected sealed class OnUpdateWarnings\u003C\u003ESystem_Collections_Generic_List`1\u003CSandbox_Game_SessionComponents_MySessionComponentWarningSystem\u003C\u003EWarningData\u003E : ICallSite<IMyEventOwner, List<MySessionComponentWarningSystem.WarningData>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in List<MySessionComponentWarningSystem.WarningData> warnings,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MySessionComponentWarningSystem.OnUpdateWarnings(warnings);
      }
    }
  }
}
