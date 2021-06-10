// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MyHighlightSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Network;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
  [StaticEventOwner]
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyHighlightSystem : MySessionComponentBase
  {
    private static MyHighlightSystem m_static;
    private static int m_exclusiveKeyCounter = 10;
    private readonly Dictionary<int, MyHighlightSystem.ExclusiveHighlightIdentification> m_exclusiveKeysToIds = new Dictionary<int, MyHighlightSystem.ExclusiveHighlightIdentification>();
    private readonly HashSet<long> m_highlightedIds = new HashSet<long>();
    private readonly MyHudSelectedObject m_highlightCalculationHelper = new MyHudSelectedObject();
    private readonly List<uint> m_subPartIndicies = new List<uint>();
    private readonly HashSet<uint> m_highlightOverlappingIds = new HashSet<uint>();
    private readonly Dictionary<int, MyHighlightData> m_exclusiveHighlightsData = new Dictionary<int, MyHighlightData>();
    private readonly Dictionary<long, MyHighlightData> m_clientExclusiveHighlightsDataCache = new Dictionary<long, MyHighlightData>();
    private readonly List<long> m_toRemove = new List<long>();
    private StringBuilder m_highlightAttributeBuilder = new StringBuilder();

    public HashSetReader<uint> HighlightOverlappingRenderIds => new HashSetReader<uint>(this.m_highlightOverlappingIds);

    public event Action<MyHighlightData> HighlightRejected;

    public event Action<MyHighlightData> HighlightAccepted;

    public event Action<MyHighlightData, int> ExclusiveHighlightRejected;

    public event Action<MyHighlightData, int> ExclusiveHighlightAccepted;

    public MyHighlightSystem()
    {
      MyHighlightSystem.m_static = this;
      this.ExclusiveHighlightAccepted += new Action<MyHighlightData, int>(this.OnExclusiveHighlightAccepted);
      MyEntities.OnEntityAdd += new Action<MyEntity>(this.MyEntities_OnEntityAdd);
    }

    protected override void UnloadData()
    {
      MyEntities.OnEntityAdd -= new Action<MyEntity>(this.MyEntities_OnEntityAdd);
      this.m_clientExclusiveHighlightsDataCache.Clear();
      base.UnloadData();
      MyHighlightSystem.m_static = (MyHighlightSystem) null;
    }

    public void RequestHighlightChange(MyHighlightData data) => this.ProcessRequest(data, -1, false);

    public void RequestHighlightChangeExclusive(MyHighlightData data, int exclusiveKey = -1) => this.ProcessRequest(data, exclusiveKey, true);

    public bool IsHighlighted(long entityId) => this.m_highlightedIds.Contains(entityId);

    public bool IsReserved(
      MyHighlightSystem.ExclusiveHighlightIdentification highlightId)
    {
      return this.m_exclusiveKeysToIds.ContainsValue(highlightId);
    }

    public void AddHighlightOverlappingModel(uint modelRenderId)
    {
      if (modelRenderId == uint.MaxValue || this.m_highlightOverlappingIds.Contains(modelRenderId))
        return;
      this.m_highlightOverlappingIds.Add(modelRenderId);
      MyRenderProxy.UpdateHighlightOverlappingModel(modelRenderId);
    }

    public void RemoveHighlightOverlappingModel(uint modelRenderId)
    {
      if (modelRenderId == uint.MaxValue || !this.m_highlightOverlappingIds.Contains(modelRenderId))
        return;
      this.m_highlightOverlappingIds.Remove(modelRenderId);
      MyRenderProxy.UpdateHighlightOverlappingModel(modelRenderId, false);
    }

    public override void SaveData()
    {
      if (MyCampaignManager.Static == null || !MyCampaignManager.Static.IsCampaignRunning)
        return;
      MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
      if (component == null)
        return;
      component.ExclusiveHighlightsData = this.GetExclusiveHighlightsObjectBuilder();
    }

    public MyObjectBuilder_ExclusiveHighlights GetExclusiveHighlightsObjectBuilder()
    {
      MyObjectBuilder_ExclusiveHighlights exclusiveHighlights = new MyObjectBuilder_ExclusiveHighlights();
      exclusiveHighlights.ExclusiveHighlightData.AddRange((IEnumerable<MyHighlightData>) this.m_exclusiveHighlightsData.Values);
      return exclusiveHighlights;
    }

    public override void BeforeStart()
    {
      MyVisualScriptManagerSessionComponent component = MySession.Static.GetComponent<MyVisualScriptManagerSessionComponent>();
      if (component == null)
        return;
      MyObjectBuilder_ExclusiveHighlights exclusiveHighlightsData = component.ExclusiveHighlightsData;
      if (exclusiveHighlightsData == null)
        return;
      foreach (MyHighlightData highlightData in exclusiveHighlightsData.ExclusiveHighlightData)
      {
        MyVisualScriptLogicProvider.SetHighlight(highlightData, highlightData.PlayerId);
        if (!Sync.IsServer)
          this.m_clientExclusiveHighlightsDataCache[highlightData.EntityId] = highlightData;
      }
    }

    private void MyEntities_OnEntityAdd(MyEntity entity)
    {
      MyCubeGrid myCubeGrid = entity as MyCubeGrid;
      if (Sync.IsServer || myCubeGrid == null || this.m_clientExclusiveHighlightsDataCache.Count <= 0)
        return;
      foreach (KeyValuePair<long, MyHighlightData> keyValuePair in this.m_clientExclusiveHighlightsDataCache)
      {
        if (MyEntities.GetEntityById(keyValuePair.Key) != null)
        {
          MyVisualScriptLogicProvider.SetHighlight(keyValuePair.Value, keyValuePair.Value.PlayerId);
          this.m_toRemove.Add(keyValuePair.Key);
        }
      }
      foreach (long key in this.m_toRemove)
        this.m_clientExclusiveHighlightsDataCache.Remove(key);
      this.m_toRemove.Clear();
    }

    private void ProcessRequest(MyHighlightData data, int exclusiveKey, bool isExclusive)
    {
      if (data.PlayerId == -1L)
        data.PlayerId = MySession.Static.LocalPlayerId;
      if ((MyMultiplayer.Static == null || MyMultiplayer.Static.IsServer) && data.PlayerId != MySession.Static.LocalPlayerId)
      {
        MyPlayer.PlayerId result;
        if (!MySession.Static.Players.TryGetPlayerId(data.PlayerId, out result))
          return;
        MyMultiplayer.RaiseStaticEvent<MyHighlightSystem.HighlightMsg>((Func<IMyEventOwner, Action<MyHighlightSystem.HighlightMsg>>) (s => new Action<MyHighlightSystem.HighlightMsg>(MyHighlightSystem.OnHighlightOnClient)), new MyHighlightSystem.HighlightMsg()
        {
          Data = data,
          ExclusiveKey = exclusiveKey,
          IsExclusive = isExclusive
        }, new EndpointId(result.SteamId));
      }
      else
      {
        bool flag = data.Thickness > -1;
        MyHighlightSystem.ExclusiveHighlightIdentification other = new MyHighlightSystem.ExclusiveHighlightIdentification(data.EntityId, data.SubPartNames);
        MyHighlightSystem.ExclusiveHighlightIdentification highlightIdentification;
        if (this.m_exclusiveKeysToIds.ContainsValue(other) && (!this.m_exclusiveKeysToIds.TryGetValue(exclusiveKey, out highlightIdentification) || !highlightIdentification.Equals(other)))
        {
          if (this.HighlightRejected == null)
            return;
          this.HighlightRejected(data);
        }
        else if (isExclusive)
        {
          if (exclusiveKey == -1)
            exclusiveKey = MyHighlightSystem.m_exclusiveKeyCounter++;
          if (flag)
          {
            if (!this.m_exclusiveKeysToIds.ContainsKey(exclusiveKey))
              this.m_exclusiveKeysToIds.Add(exclusiveKey, other);
          }
          else
            this.m_exclusiveKeysToIds.Remove(exclusiveKey);
          this.MakeLocalHighlightChange(data);
          if (this.ExclusiveHighlightAccepted == null)
            return;
          this.ExclusiveHighlightAccepted(data, exclusiveKey);
        }
        else
        {
          this.MakeLocalHighlightChange(data);
          if (this.HighlightAccepted == null)
            return;
          this.HighlightAccepted(data);
        }
      }
    }

    private void MakeLocalHighlightChange(MyHighlightData data)
    {
      if (data.Thickness > -1)
        this.m_highlightedIds.Add(data.EntityId);
      else
        this.m_highlightedIds.Remove(data.EntityId);
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(data.EntityId, out entity))
        return;
      if (!data.IgnoreUseObjectData)
      {
        IMyUseObject useObject = entity as IMyUseObject;
        MyUseObjectsComponentBase objectsComponentBase = entity.Components.Get<MyUseObjectsComponentBase>();
        if (useObject != null || objectsComponentBase != null)
        {
          if (objectsComponentBase != null)
          {
            List<IMyUseObject> objects = new List<IMyUseObject>();
            objectsComponentBase.GetInteractiveObjects<IMyUseObject>(objects);
            for (int index = 0; index < objects.Count; ++index)
              this.HighlightUseObject(objects[index], data);
            if (objects.Count > 0)
            {
              if (this.HighlightAccepted == null)
                return;
              this.HighlightAccepted(data);
              return;
            }
          }
          else
          {
            this.HighlightUseObject(useObject, data);
            if (this.HighlightAccepted == null)
              return;
            this.HighlightAccepted(data);
            return;
          }
        }
      }
      this.m_subPartIndicies.Clear();
      this.CollectSubPartIndicies(entity);
      foreach (uint renderObjectId in entity.Render.RenderObjectIDs)
        MyRenderProxy.UpdateModelHighlight(renderObjectId, (string[]) null, this.m_subPartIndicies.ToArray(), data.OutlineColor, (float) data.Thickness, (float) data.PulseTimeInFrames);
      if (this.HighlightAccepted == null)
        return;
      this.HighlightAccepted(data);
    }

    private void CollectSubPartIndicies(MyEntity currentEntity)
    {
      if (currentEntity.Subparts == null || currentEntity.Render == null)
        return;
      foreach (MyEntitySubpart myEntitySubpart in currentEntity.Subparts.Values)
      {
        this.CollectSubPartIndicies((MyEntity) myEntitySubpart);
        this.m_subPartIndicies.AddRange((IEnumerable<uint>) myEntitySubpart.Render.RenderObjectIDs);
      }
    }

    private void HighlightUseObject(IMyUseObject useObject, MyHighlightData data)
    {
      this.m_highlightCalculationHelper.HighlightAttribute = (string) null;
      if (useObject.Dummy != null)
      {
        object obj;
        useObject.Dummy.CustomData.TryGetValue("highlight", out obj);
        if (!(obj is string str))
          return;
        if (data.SubPartNames != null)
        {
          this.m_highlightAttributeBuilder.Clear();
          string subPartNames = data.SubPartNames;
          char[] chArray = new char[1]{ ';' };
          foreach (string str in subPartNames.Split(chArray))
          {
            if (str.Contains(str))
              this.m_highlightAttributeBuilder.Append(str).Append(';');
          }
          if (this.m_highlightAttributeBuilder.Length > 0)
            this.m_highlightAttributeBuilder.TrimEnd(1);
          this.m_highlightCalculationHelper.HighlightAttribute = this.m_highlightAttributeBuilder.ToString();
        }
        else
          this.m_highlightCalculationHelper.HighlightAttribute = str;
        if (string.IsNullOrEmpty(this.m_highlightCalculationHelper.HighlightAttribute))
          return;
      }
      this.m_highlightCalculationHelper.Highlight(useObject);
      MyRenderProxy.UpdateModelHighlight(this.m_highlightCalculationHelper.InteractiveObject.RenderObjectID, this.m_highlightCalculationHelper.SectionNames, this.m_highlightCalculationHelper.SubpartIndices, data.OutlineColor, (float) data.Thickness, (float) data.PulseTimeInFrames, this.m_highlightCalculationHelper.InteractiveObject.InstanceID);
    }

    [Event(null, 496)]
    [Reliable]
    [Client]
    private static void OnHighlightOnClient(MyHighlightSystem.HighlightMsg msg)
    {
      MyHighlightSystem.ExclusiveHighlightIdentification other = new MyHighlightSystem.ExclusiveHighlightIdentification(msg.Data.EntityId, msg.Data.SubPartNames);
      MyHighlightSystem.ExclusiveHighlightIdentification highlightIdentification;
      if (MyHighlightSystem.m_static.m_exclusiveKeysToIds.ContainsValue(other) && (!MyHighlightSystem.m_static.m_exclusiveKeysToIds.TryGetValue(msg.ExclusiveKey, out highlightIdentification) || !highlightIdentification.Equals(other)))
      {
        if (MyHighlightSystem.m_static.HighlightRejected != null)
          MyHighlightSystem.m_static.HighlightRejected(msg.Data);
        MyMultiplayer.RaiseStaticEvent<MyHighlightSystem.HighlightMsg>((Func<IMyEventOwner, Action<MyHighlightSystem.HighlightMsg>>) (s => new Action<MyHighlightSystem.HighlightMsg>(MyHighlightSystem.OnRequestRejected)), msg, MyEventContext.Current.Sender);
      }
      else
      {
        MyHighlightSystem.m_static.MakeLocalHighlightChange(msg.Data);
        if (msg.IsExclusive)
        {
          bool flag = msg.Data.Thickness > -1;
          if (msg.ExclusiveKey == -1)
          {
            msg.ExclusiveKey = MyHighlightSystem.m_exclusiveKeyCounter++;
            if (flag && !MyHighlightSystem.m_static.m_exclusiveKeysToIds.ContainsKey(msg.ExclusiveKey))
              MyHighlightSystem.m_static.m_exclusiveKeysToIds.Add(msg.ExclusiveKey, other);
          }
          if (!flag)
            MyHighlightSystem.m_static.m_exclusiveKeysToIds.Remove(msg.ExclusiveKey);
        }
        MyMultiplayer.RaiseStaticEvent<MyHighlightSystem.HighlightMsg>((Func<IMyEventOwner, Action<MyHighlightSystem.HighlightMsg>>) (s => new Action<MyHighlightSystem.HighlightMsg>(MyHighlightSystem.OnRequestAccepted)), msg, MyEventContext.Current.Sender);
      }
    }

    [Event(null, 542)]
    [Reliable]
    [Server]
    private static void OnRequestRejected(MyHighlightSystem.HighlightMsg msg)
    {
      if (msg.IsExclusive)
      {
        MyHighlightSystem.m_static.NotifyExclusiveHighlightRejected(msg.Data, msg.ExclusiveKey);
      }
      else
      {
        if (MyHighlightSystem.m_static.HighlightRejected == null)
          return;
        MyHighlightSystem.m_static.HighlightRejected(msg.Data);
      }
    }

    [Event(null, 557)]
    [Reliable]
    [Server]
    private static void OnRequestAccepted(MyHighlightSystem.HighlightMsg msg)
    {
      if (msg.IsExclusive)
        MyHighlightSystem.m_static.NotifyExclusiveHighlightAccepted(msg.Data, msg.ExclusiveKey);
      else
        MyHighlightSystem.m_static.NotifyHighlightAccepted(msg.Data);
    }

    private void NotifyHighlightAccepted(MyHighlightData data)
    {
      if (this.HighlightAccepted == null)
        return;
      this.HighlightAccepted(data);
      foreach (Action<MyHighlightData> invocation in this.HighlightAccepted.GetInvocationList())
        this.HighlightAccepted -= invocation;
    }

    private void NotifyExclusiveHighlightAccepted(MyHighlightData data, int exclusiveKey)
    {
      if (this.ExclusiveHighlightAccepted == null)
        return;
      this.ExclusiveHighlightAccepted(data, exclusiveKey);
      foreach (Action<MyHighlightData, int> invocation in this.ExclusiveHighlightAccepted.GetInvocationList())
        this.ExclusiveHighlightAccepted -= invocation;
    }

    private void NotifyExclusiveHighlightRejected(MyHighlightData data, int exclusiveKey)
    {
      if (this.ExclusiveHighlightRejected == null)
        return;
      this.ExclusiveHighlightRejected(data, exclusiveKey);
      foreach (Action<MyHighlightData, int> invocation in this.ExclusiveHighlightRejected.GetInvocationList())
        this.ExclusiveHighlightRejected -= invocation;
    }

    private void OnExclusiveHighlightAccepted(MyHighlightData data, int key)
    {
      if (data.Thickness > 0)
        this.m_exclusiveHighlightsData[key] = data;
      else
        this.m_exclusiveHighlightsData.Remove(key);
    }

    public class ExclusiveHighlightIdentification : IEquatable<MyHighlightSystem.ExclusiveHighlightIdentification>
    {
      public long EntityId { get; private set; }

      public string SectionName { get; private set; }

      public ExclusiveHighlightIdentification(long entityId, string sectionName)
      {
        this.EntityId = entityId;
        this.SectionName = sectionName;
      }

      public ExclusiveHighlightIdentification(long entityId, string[] sectionNames)
        : this(entityId, sectionNames == null ? string.Empty : string.Join(";", sectionNames))
      {
      }

      public bool Equals(
        MyHighlightSystem.ExclusiveHighlightIdentification other)
      {
        return other != null && this.EntityId == other.EntityId && this.SectionName.Equals(other.SectionName, StringComparison.InvariantCulture);
      }

      public override int GetHashCode() => (-1285426570 * -1521134295 + this.EntityId.GetHashCode()) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.SectionName);
    }

    [Serializable]
    private struct HighlightMsg
    {
      public MyHighlightData Data;
      public int ExclusiveKey;
      public bool IsExclusive;

      protected class Sandbox_Game_SessionComponents_MyHighlightSystem\u003C\u003EHighlightMsg\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<MyHighlightSystem.HighlightMsg, MyHighlightData>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyHighlightSystem.HighlightMsg owner, in MyHighlightData value) => owner.Data = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyHighlightSystem.HighlightMsg owner, out MyHighlightData value) => value = owner.Data;
      }

      protected class Sandbox_Game_SessionComponents_MyHighlightSystem\u003C\u003EHighlightMsg\u003C\u003EExclusiveKey\u003C\u003EAccessor : IMemberAccessor<MyHighlightSystem.HighlightMsg, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyHighlightSystem.HighlightMsg owner, in int value) => owner.ExclusiveKey = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyHighlightSystem.HighlightMsg owner, out int value) => value = owner.ExclusiveKey;
      }

      protected class Sandbox_Game_SessionComponents_MyHighlightSystem\u003C\u003EHighlightMsg\u003C\u003EIsExclusive\u003C\u003EAccessor : IMemberAccessor<MyHighlightSystem.HighlightMsg, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyHighlightSystem.HighlightMsg owner, in bool value) => owner.IsExclusive = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyHighlightSystem.HighlightMsg owner, out bool value) => value = owner.IsExclusive;
      }
    }

    protected sealed class OnHighlightOnClient\u003C\u003ESandbox_Game_SessionComponents_MyHighlightSystem\u003C\u003EHighlightMsg : ICallSite<IMyEventOwner, MyHighlightSystem.HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyHighlightSystem.HighlightMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyHighlightSystem.OnHighlightOnClient(msg);
      }
    }

    protected sealed class OnRequestRejected\u003C\u003ESandbox_Game_SessionComponents_MyHighlightSystem\u003C\u003EHighlightMsg : ICallSite<IMyEventOwner, MyHighlightSystem.HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyHighlightSystem.HighlightMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyHighlightSystem.OnRequestRejected(msg);
      }
    }

    protected sealed class OnRequestAccepted\u003C\u003ESandbox_Game_SessionComponents_MyHighlightSystem\u003C\u003EHighlightMsg : ICallSite<IMyEventOwner, MyHighlightSystem.HighlightMsg, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyHighlightSystem.HighlightMsg msg,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyHighlightSystem.OnRequestAccepted(msg);
      }
    }
  }
}
