// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyEntityStatComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Sandbox.Game.Components
{
  [StaticEventOwner]
  [MyComponentType(typeof (MyEntityStatComponent))]
  [MyComponentBuilder(typeof (MyObjectBuilder_EntityStatComponent), true)]
  public class MyEntityStatComponent : MyEntityComponentBase
  {
    private Dictionary<MyStringHash, MyEntityStat> m_stats;
    protected List<MyStatLogic> m_scripts;
    private List<MyEntityStat> m_statSyncList = new List<MyEntityStat>();
    private int m_updateCounter;
    private bool m_statActionsRequested;

    private void SendStatsChanged(List<MyEntityStat> stats)
    {
      List<MyEntityStatComponent.StatInfo> statInfoList = new List<MyEntityStatComponent.StatInfo>();
      foreach (MyEntityStat stat in stats)
      {
        double forLongestEffect = (double) stat.CalculateRegenLeftForLongestEffect();
        statInfoList.Add(new MyEntityStatComponent.StatInfo()
        {
          StatId = stat.StatId,
          Amount = stat.Value,
          RegenLeft = stat.StatRegenLeft
        });
      }
      MyMultiplayer.RaiseStaticEvent<long, List<MyEntityStatComponent.StatInfo>>((Func<IMyEventOwner, Action<long, List<MyEntityStatComponent.StatInfo>>>) (s => new Action<long, List<MyEntityStatComponent.StatInfo>>(MyEntityStatComponent.OnStatChangedMessage)), this.Entity.EntityId, statInfoList);
    }

    [Event(null, 55)]
    [Reliable]
    [Broadcast]
    private static void OnStatChangedMessage(
      long entityId,
      List<MyEntityStatComponent.StatInfo> changedStats)
    {
      MyEntity entity;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return;
      MyEntityStatComponent component = (MyEntityStatComponent) null;
      if (!entity.Components.TryGet<MyEntityStatComponent>(out component))
        return;
      foreach (MyEntityStatComponent.StatInfo changedStat in changedStats)
      {
        MyEntityStat outStat;
        if (component.TryGetStat(changedStat.StatId, out outStat))
        {
          outStat.Value = changedStat.Amount;
          outStat.StatRegenLeft = changedStat.RegenLeft;
        }
      }
    }

    [Event(null, 76)]
    [Reliable]
    [Server]
    private static void OnStatActionRequest(long entityId)
    {
      MyEntity entity = (MyEntity) null;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return;
      MyEntityStatComponent component = (MyEntityStatComponent) null;
      if (!entity.Components.TryGet<MyEntityStatComponent>(out component))
        return;
      Dictionary<string, MyStatLogic.MyStatAction> statActions = new Dictionary<string, MyStatLogic.MyStatAction>();
      foreach (MyStatLogic script in component.m_scripts)
      {
        foreach (KeyValuePair<string, MyStatLogic.MyStatAction> statAction in script.StatActions)
        {
          if (!statActions.ContainsKey(statAction.Key))
            statActions.Add(statAction.Key, statAction.Value);
        }
      }
      if (MyEventContext.Current.IsLocallyInvoked)
        MyEntityStatComponent.OnStatActionMessage(entityId, statActions);
      else
        MyMultiplayer.RaiseStaticEvent<long, Dictionary<string, MyStatLogic.MyStatAction>>((Func<IMyEventOwner, Action<long, Dictionary<string, MyStatLogic.MyStatAction>>>) (s => new Action<long, Dictionary<string, MyStatLogic.MyStatAction>>(MyEntityStatComponent.OnStatActionMessage)), entityId, statActions, MyEventContext.Current.Sender);
    }

    [Event(null, 108)]
    [Reliable]
    [Client]
    private static void OnStatActionMessage(
      long entityId,
      Dictionary<string, MyStatLogic.MyStatAction> statActions)
    {
      MyEntity entity = (MyEntity) null;
      if (!MyEntities.TryGetEntityById(entityId, out entity))
        return;
      MyEntityStatComponent component = (MyEntityStatComponent) null;
      if (!entity.Components.TryGet<MyEntityStatComponent>(out component))
        return;
      MyStatLogic myStatLogic = new MyStatLogic();
      myStatLogic.Init(entity as IMyCharacter, component.m_stats, "LocalStatActionScript");
      foreach (KeyValuePair<string, MyStatLogic.MyStatAction> statAction in statActions)
        myStatLogic.AddAction(statAction.Key, statAction.Value);
      component.m_scripts.Add(myStatLogic);
    }

    public DictionaryValuesReader<MyStringHash, MyEntityStat> Stats => new DictionaryValuesReader<MyStringHash, MyEntityStat>(this.m_stats);

    public MyEntityStatComponent()
    {
      this.m_stats = new Dictionary<MyStringHash, MyEntityStat>((IEqualityComparer<MyStringHash>) MyStringHash.Comparer);
      this.m_scripts = new List<MyStatLogic>();
    }

    public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
    {
      MyObjectBuilder_ComponentBase builderComponentBase = base.Serialize();
      if (!(builderComponentBase is MyObjectBuilder_CharacterStatComponent characterStatComponent))
        return builderComponentBase;
      characterStatComponent.Stats = (MyObjectBuilder_EntityStat[]) null;
      characterStatComponent.ScriptNames = (string[]) null;
      if (this.m_stats != null && this.m_stats.Count > 0)
      {
        characterStatComponent.Stats = new MyObjectBuilder_EntityStat[this.m_stats.Count];
        int num = 0;
        foreach (KeyValuePair<MyStringHash, MyEntityStat> stat in this.m_stats)
          characterStatComponent.Stats[num++] = stat.Value.GetObjectBuilder();
      }
      if (this.m_scripts != null && this.m_scripts.Count > 0)
      {
        characterStatComponent.ScriptNames = new string[this.m_scripts.Count];
        int num = 0;
        foreach (MyStatLogic script in this.m_scripts)
          characterStatComponent.ScriptNames[num++] = script.Name;
      }
      return (MyObjectBuilder_ComponentBase) characterStatComponent;
    }

    public override void Deserialize(MyObjectBuilder_ComponentBase objectBuilder)
    {
      MyObjectBuilder_CharacterStatComponent characterStatComponent = objectBuilder as MyObjectBuilder_CharacterStatComponent;
      foreach (MyStatLogic script in this.m_scripts)
        script.Close();
      this.m_scripts.Clear();
      if (characterStatComponent != null)
      {
        if (characterStatComponent.Stats != null)
        {
          foreach (MyObjectBuilder_EntityStat stat in characterStatComponent.Stats)
          {
            MyEntityStatDefinition definition = (MyEntityStatDefinition) null;
            if (MyDefinitionManager.Static.TryGetDefinition<MyEntityStatDefinition>(new MyDefinitionId(stat.TypeId, stat.SubtypeId), out definition) && definition.Enabled && (definition.EnabledInCreative && MySession.Static.CreativeMode || definition.AvailableInSurvival && MySession.Static.SurvivalMode))
              this.AddStat(MyStringHash.GetOrCompute(definition.Name), stat, true);
          }
        }
        if (characterStatComponent.ScriptNames != null && Sync.IsServer)
        {
          characterStatComponent.ScriptNames = ((IEnumerable<string>) characterStatComponent.ScriptNames).Distinct<string>().ToArray<string>();
          foreach (string scriptName in characterStatComponent.ScriptNames)
            this.InitScript(scriptName);
        }
      }
      base.Deserialize(objectBuilder);
    }

    public override bool IsSerialized() => true;

    public bool HasAnyComsumableEffect()
    {
      foreach (KeyValuePair<MyStringHash, MyEntityStat> stat in this.m_stats)
      {
        if (stat.Value.HasAnyEffect())
          return true;
      }
      return false;
    }

    public override void Init(MyComponentDefinitionBase definition)
    {
      base.Init(definition);
      if (!(definition is MyEntityStatComponentDefinition componentDefinition) || !componentDefinition.Enabled || MySession.Static == null || !componentDefinition.AvailableInSurvival && MySession.Static.SurvivalMode)
      {
        if (!Sync.IsServer)
          return;
        this.m_statActionsRequested = true;
      }
      else
      {
        foreach (MyDefinitionId stat in componentDefinition.Stats)
        {
          MyEntityStatDefinition definition1 = (MyEntityStatDefinition) null;
          if (MyDefinitionManager.Static.TryGetDefinition<MyEntityStatDefinition>(stat, out definition1) && definition1.Enabled && (definition1.EnabledInCreative || !MySession.Static.CreativeMode) && (definition1.AvailableInSurvival || !MySession.Static.SurvivalMode))
          {
            MyStringHash orCompute = MyStringHash.GetOrCompute(definition1.Name);
            MyEntityStat myEntityStat = (MyEntityStat) null;
            if (!this.m_stats.TryGetValue(orCompute, out myEntityStat) || !(myEntityStat.StatDefinition.Id.SubtypeId == definition1.Id.SubtypeId))
            {
              MyObjectBuilder_EntityStat objectBuilder = new MyObjectBuilder_EntityStat();
              objectBuilder.SubtypeName = stat.SubtypeName;
              objectBuilder.MaxValue = 1f;
              objectBuilder.Value = definition1.DefaultValue / definition1.MaxValue;
              this.AddStat(orCompute, objectBuilder);
            }
          }
        }
        if (!Sync.IsServer)
          return;
        foreach (string script in componentDefinition.Scripts)
          this.InitScript(script);
        this.m_statActionsRequested = true;
      }
    }

    public virtual void Update()
    {
      if (this.Container.Entity == null)
        return;
      if (!this.m_statActionsRequested)
      {
        MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyEntityStatComponent.OnStatActionRequest)), this.Entity.EntityId);
        this.m_statActionsRequested = true;
      }
      foreach (MyStatLogic script in this.m_scripts)
        script.Update();
      if (this.m_updateCounter++ % 10 == 0)
      {
        foreach (MyStatLogic script in this.m_scripts)
          script.Update10();
      }
      foreach (MyEntityStat myEntityStat in this.m_stats.Values)
      {
        myEntityStat.Update();
        if (Sync.IsServer && myEntityStat.ShouldSync)
          this.m_statSyncList.Add(myEntityStat);
      }
      if (this.m_statSyncList.Count <= 0)
        return;
      this.SendStatsChanged(this.m_statSyncList);
      this.m_statSyncList.Clear();
    }

    public bool TryGetStat(MyStringHash statId, out MyEntityStat outStat) => this.m_stats.TryGetValue(statId, out outStat);

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      foreach (MyStatLogic script in this.m_scripts)
        script.Character = this.Entity as IMyCharacter;
    }

    public override void OnBeforeRemovedFromContainer()
    {
      foreach (MyStatLogic script in this.m_scripts)
        script.Close();
      base.OnBeforeRemovedFromContainer();
    }

    public bool CanDoAction(
      string actionId,
      out MyTuple<ushort, MyStringHash> message,
      bool continuous = false)
    {
      message = new MyTuple<ushort, MyStringHash>((ushort) 0, MyStringHash.NullOrEmpty);
      if (this.m_scripts == null || this.m_scripts.Count == 0)
        return true;
      bool flag = true;
      foreach (MyStatLogic script in this.m_scripts)
      {
        MyTuple<ushort, MyStringHash> message1;
        flag &= !script.CanDoAction(actionId, continuous, out message1);
        if (message1.Item1 != (ushort) 0)
          message = message1;
      }
      return !flag;
    }

    public bool DoAction(string actionId)
    {
      bool flag = false;
      foreach (MyStatLogic script in this.m_scripts)
      {
        if (script.DoAction(actionId))
          flag = true;
      }
      return flag;
    }

    public void ApplyModifier(string modifierId)
    {
      foreach (MyStatLogic script in this.m_scripts)
        script.ApplyModifier(modifierId);
    }

    public float GetEfficiencyModifier(string modifierId)
    {
      float num = 1f;
      foreach (MyStatLogic script in this.m_scripts)
        num *= script.GetEfficiencyModifier(modifierId);
      return num;
    }

    private void InitScript(string scriptName)
    {
      if (scriptName == "SpaceStatEffect")
      {
        MySpaceStatEffect mySpaceStatEffect = new MySpaceStatEffect();
        mySpaceStatEffect.Init(this.Entity as IMyCharacter, this.m_stats, "SpaceStatEffect");
        this.m_scripts.Add((MyStatLogic) mySpaceStatEffect);
      }
      else
      {
        Type type;
        if (!MyScriptManager.Static.StatScripts.TryGetValue(scriptName, out type))
          return;
        MyStatLogic instance = (MyStatLogic) Activator.CreateInstance(type);
        if (instance == null)
          return;
        instance.Init(this.Entity as IMyCharacter, this.m_stats, scriptName);
        this.m_scripts.Add(instance);
      }
    }

    private MyEntityStat AddStat(
      MyStringHash statId,
      MyObjectBuilder_EntityStat objectBuilder,
      bool forceNewValues = false)
    {
      MyEntityStat myEntityStat1 = (MyEntityStat) null;
      if (this.m_stats.TryGetValue(statId, out myEntityStat1))
      {
        if (!forceNewValues)
          objectBuilder.Value = myEntityStat1.CurrentRatio;
        myEntityStat1.ClearEffects();
        this.m_stats.Remove(statId);
      }
      MyEntityStat myEntityStat2 = new MyEntityStat();
      myEntityStat2.Init((MyObjectBuilder_Base) objectBuilder);
      this.m_stats.Add(statId, myEntityStat2);
      return myEntityStat2;
    }

    public override string ComponentTypeDebugString => "Stats";

    [Serializable]
    private struct StatInfo
    {
      public MyStringHash StatId;
      public float Amount;
      public float RegenLeft;

      protected class Sandbox_Game_Components_MyEntityStatComponent\u003C\u003EStatInfo\u003C\u003EStatId\u003C\u003EAccessor : IMemberAccessor<MyEntityStatComponent.StatInfo, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityStatComponent.StatInfo owner, in MyStringHash value) => owner.StatId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityStatComponent.StatInfo owner, out MyStringHash value) => value = owner.StatId;
      }

      protected class Sandbox_Game_Components_MyEntityStatComponent\u003C\u003EStatInfo\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyEntityStatComponent.StatInfo, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityStatComponent.StatInfo owner, in float value) => owner.Amount = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityStatComponent.StatInfo owner, out float value) => value = owner.Amount;
      }

      protected class Sandbox_Game_Components_MyEntityStatComponent\u003C\u003EStatInfo\u003C\u003ERegenLeft\u003C\u003EAccessor : IMemberAccessor<MyEntityStatComponent.StatInfo, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyEntityStatComponent.StatInfo owner, in float value) => owner.RegenLeft = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyEntityStatComponent.StatInfo owner, out float value) => value = owner.RegenLeft;
      }
    }

    protected sealed class OnStatChangedMessage\u003C\u003ESystem_Int64\u0023System_Collections_Generic_List`1\u003CSandbox_Game_Components_MyEntityStatComponent\u003C\u003EStatInfo\u003E : ICallSite<IMyEventOwner, long, List<MyEntityStatComponent.StatInfo>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in List<MyEntityStatComponent.StatInfo> changedStats,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyEntityStatComponent.OnStatChangedMessage(entityId, changedStats);
      }
    }

    protected sealed class OnStatActionRequest\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyEntityStatComponent.OnStatActionRequest(entityId);
      }
    }

    protected sealed class OnStatActionMessage\u003C\u003ESystem_Int64\u0023System_Collections_Generic_Dictionary`2\u003CSystem_String\u0023Sandbox_Game_MyStatLogic\u003C\u003EMyStatAction\u003E : ICallSite<IMyEventOwner, long, Dictionary<string, MyStatLogic.MyStatAction>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Dictionary<string, MyStatLogic.MyStatAction> statActions,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyEntityStatComponent.OnStatActionMessage(entityId, statActions);
      }
    }

    private class Sandbox_Game_Components_MyEntityStatComponent\u003C\u003EActor : IActivator, IActivator<MyEntityStatComponent>
    {
      object IActivator.CreateInstance() => (object) new MyEntityStatComponent();

      MyEntityStatComponent IActivator<MyEntityStatComponent>.CreateInstance() => new MyEntityStatComponent();
    }
  }
}
