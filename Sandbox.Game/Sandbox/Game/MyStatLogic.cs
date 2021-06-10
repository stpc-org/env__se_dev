// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.MyStatLogic
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using ProtoBuf;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Game
{
  public class MyStatLogic
  {
    private string m_scriptName;
    private IMyCharacter m_character;
    protected Dictionary<MyStringHash, MyEntityStat> m_stats;
    private bool m_enableAutoHealing = true;
    private Dictionary<string, MyStatLogic.MyStatAction> m_statActions = new Dictionary<string, MyStatLogic.MyStatAction>();
    private Dictionary<string, MyStatLogic.MyStatRegenModifier> m_statRegenModifiers = new Dictionary<string, MyStatLogic.MyStatRegenModifier>();
    private Dictionary<string, MyStatLogic.MyStatEfficiencyModifier> m_statEfficiencyModifiers = new Dictionary<string, MyStatLogic.MyStatEfficiencyModifier>();
    public const int STAT_VALUE_TOO_LOW = 4;

    public string Name => this.m_scriptName;

    public IMyCharacter Character
    {
      get => this.m_character;
      set
      {
        IMyCharacter character = this.m_character;
        this.m_character = value;
        this.OnCharacterChanged(character);
      }
    }

    protected bool EnableAutoHealing => this.m_enableAutoHealing;

    public Dictionary<string, MyStatLogic.MyStatAction> StatActions => this.m_statActions;

    public Dictionary<string, MyStatLogic.MyStatRegenModifier> StatRegenModifiers => this.m_statRegenModifiers;

    public Dictionary<string, MyStatLogic.MyStatEfficiencyModifier> StatEfficiencyModifiers => this.m_statEfficiencyModifiers;

    public virtual void Init(
      IMyCharacter character,
      Dictionary<MyStringHash, MyEntityStat> stats,
      string scriptName)
    {
      this.m_scriptName = scriptName;
      this.Character = character;
      this.m_stats = stats;
      this.InitSettings();
    }

    private void InitSettings() => this.m_enableAutoHealing = MySession.Static.Settings.AutoHealing;

    public virtual void Update()
    {
    }

    public virtual void Update10()
    {
    }

    public virtual void Close()
    {
    }

    protected virtual void OnCharacterChanged(IMyCharacter oldCharacter)
    {
    }

    public void AddAction(string actionId, MyStatLogic.MyStatAction action) => this.m_statActions.Add(actionId, action);

    public void AddModifier(string modifierId, MyStatLogic.MyStatRegenModifier modifier) => this.m_statRegenModifiers.Add(modifierId, modifier);

    public void AddEfficiency(string modifierId, MyStatLogic.MyStatEfficiencyModifier modifier) => this.m_statEfficiencyModifiers.Add(modifierId, modifier);

    public bool CanDoAction(
      string actionId,
      bool continuous,
      out MyTuple<ushort, MyStringHash> message)
    {
      MyStatLogic.MyStatAction myStatAction;
      if (!this.m_statActions.TryGetValue(actionId, out myStatAction))
      {
        message = new MyTuple<ushort, MyStringHash>((ushort) 0, myStatAction.StatId);
        return true;
      }
      if (myStatAction.CanPerformWithout)
      {
        message = new MyTuple<ushort, MyStringHash>((ushort) 0, myStatAction.StatId);
        return true;
      }
      MyEntityStat myEntityStat;
      if (!this.m_stats.TryGetValue(myStatAction.StatId, out myEntityStat))
      {
        message = new MyTuple<ushort, MyStringHash>((ushort) 0, myStatAction.StatId);
        return true;
      }
      if (continuous)
      {
        if ((double) myEntityStat.Value < (double) myStatAction.Cost)
        {
          message = new MyTuple<ushort, MyStringHash>((ushort) 4, myStatAction.StatId);
          return false;
        }
      }
      else if ((double) myEntityStat.Value < (double) myStatAction.Cost || (double) myEntityStat.Value < (double) myStatAction.AmountToActivate)
      {
        message = new MyTuple<ushort, MyStringHash>((ushort) 4, myStatAction.StatId);
        return false;
      }
      message = new MyTuple<ushort, MyStringHash>((ushort) 0, myStatAction.StatId);
      return true;
    }

    public bool DoAction(string actionId)
    {
      MyStatLogic.MyStatAction myStatAction;
      MyEntityStat myEntityStat;
      if (!this.m_statActions.TryGetValue(actionId, out myStatAction) || !this.m_stats.TryGetValue(myStatAction.StatId, out myEntityStat))
        return false;
      if (myStatAction.CanPerformWithout)
      {
        myEntityStat.Value -= Math.Min(myEntityStat.Value, myStatAction.Cost);
        return true;
      }
      if (((double) myStatAction.Cost >= 0.0 && (double) myEntityStat.Value >= (double) myStatAction.Cost || (double) myStatAction.Cost < 0.0) && (double) myEntityStat.Value >= (double) myStatAction.AmountToActivate)
        myEntityStat.Value -= myStatAction.Cost;
      return true;
    }

    public void ApplyModifier(string modifierId)
    {
      MyStatLogic.MyStatRegenModifier statRegenModifier;
      MyEntityStat myEntityStat;
      if (!this.m_statRegenModifiers.TryGetValue(modifierId, out statRegenModifier) || !this.m_stats.TryGetValue(statRegenModifier.StatId, out myEntityStat))
        return;
      myEntityStat.ApplyRegenAmountMultiplier(statRegenModifier.AmountMultiplier, statRegenModifier.Duration);
    }

    public float GetEfficiencyModifier(string modifierId)
    {
      MyStatLogic.MyStatEfficiencyModifier efficiencyModifier;
      MyEntityStat myEntityStat;
      return !this.m_statEfficiencyModifiers.TryGetValue(modifierId, out efficiencyModifier) || !this.m_stats.TryGetValue(efficiencyModifier.StatId, out myEntityStat) ? 1f : myEntityStat.GetEfficiencyMultiplier(efficiencyModifier.EfficiencyMultiplier, efficiencyModifier.Threshold);
    }

    [ProtoContract]
    public struct MyStatAction
    {
      [ProtoMember(1)]
      public MyStringHash StatId;
      [ProtoMember(4)]
      public float Cost;
      [ProtoMember(7)]
      public float AmountToActivate;
      [ProtoMember(10)]
      public bool CanPerformWithout;

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatAction\u003C\u003EStatId\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatAction, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatAction owner, in MyStringHash value) => owner.StatId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatAction owner, out MyStringHash value) => value = owner.StatId;
      }

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatAction\u003C\u003ECost\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatAction, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatAction owner, in float value) => owner.Cost = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatAction owner, out float value) => value = owner.Cost;
      }

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatAction\u003C\u003EAmountToActivate\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatAction, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatAction owner, in float value) => owner.AmountToActivate = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatAction owner, out float value) => value = owner.AmountToActivate;
      }

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatAction\u003C\u003ECanPerformWithout\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatAction, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatAction owner, in bool value) => owner.CanPerformWithout = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatAction owner, out bool value) => value = owner.CanPerformWithout;
      }

      private class Sandbox_Game_MyStatLogic\u003C\u003EMyStatAction\u003C\u003EActor : IActivator, IActivator<MyStatLogic.MyStatAction>
      {
        object IActivator.CreateInstance() => (object) new MyStatLogic.MyStatAction();

        MyStatLogic.MyStatAction IActivator<MyStatLogic.MyStatAction>.CreateInstance() => new MyStatLogic.MyStatAction();
      }
    }

    [ProtoContract]
    public struct MyStatRegenModifier
    {
      [ProtoMember(13)]
      public MyStringHash StatId;
      [ProtoMember(16)]
      public float AmountMultiplier;
      [ProtoMember(19)]
      public float Duration;

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatRegenModifier\u003C\u003EStatId\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatRegenModifier, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatRegenModifier owner, in MyStringHash value) => owner.StatId = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatRegenModifier owner, out MyStringHash value) => value = owner.StatId;
      }

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatRegenModifier\u003C\u003EAmountMultiplier\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatRegenModifier, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatRegenModifier owner, in float value) => owner.AmountMultiplier = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatRegenModifier owner, out float value) => value = owner.AmountMultiplier;
      }

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatRegenModifier\u003C\u003EDuration\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatRegenModifier, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatRegenModifier owner, in float value) => owner.Duration = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatRegenModifier owner, out float value) => value = owner.Duration;
      }

      private class Sandbox_Game_MyStatLogic\u003C\u003EMyStatRegenModifier\u003C\u003EActor : IActivator, IActivator<MyStatLogic.MyStatRegenModifier>
      {
        object IActivator.CreateInstance() => (object) new MyStatLogic.MyStatRegenModifier();

        MyStatLogic.MyStatRegenModifier IActivator<MyStatLogic.MyStatRegenModifier>.CreateInstance() => new MyStatLogic.MyStatRegenModifier();
      }
    }

    [ProtoContract]
    public struct MyStatEfficiencyModifier
    {
      [ProtoMember(22)]
      public MyStringHash StatId;
      [ProtoMember(25)]
      public float Threshold;
      [ProtoMember(28)]
      public float EfficiencyMultiplier;

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatEfficiencyModifier\u003C\u003EStatId\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatEfficiencyModifier, MyStringHash>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyStatLogic.MyStatEfficiencyModifier owner,
          in MyStringHash value)
        {
          owner.StatId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyStatLogic.MyStatEfficiencyModifier owner,
          out MyStringHash value)
        {
          value = owner.StatId;
        }
      }

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatEfficiencyModifier\u003C\u003EThreshold\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatEfficiencyModifier, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatEfficiencyModifier owner, in float value) => owner.Threshold = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatEfficiencyModifier owner, out float value) => value = owner.Threshold;
      }

      protected class Sandbox_Game_MyStatLogic\u003C\u003EMyStatEfficiencyModifier\u003C\u003EEfficiencyMultiplier\u003C\u003EAccessor : IMemberAccessor<MyStatLogic.MyStatEfficiencyModifier, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyStatLogic.MyStatEfficiencyModifier owner, in float value) => owner.EfficiencyMultiplier = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyStatLogic.MyStatEfficiencyModifier owner, out float value) => value = owner.EfficiencyMultiplier;
      }

      private class Sandbox_Game_MyStatLogic\u003C\u003EMyStatEfficiencyModifier\u003C\u003EActor : IActivator, IActivator<MyStatLogic.MyStatEfficiencyModifier>
      {
        object IActivator.CreateInstance() => (object) new MyStatLogic.MyStatEfficiencyModifier();

        MyStatLogic.MyStatEfficiencyModifier IActivator<MyStatLogic.MyStatEfficiencyModifier>.CreateInstance() => new MyStatLogic.MyStatEfficiencyModifier();
      }
    }
  }
}
