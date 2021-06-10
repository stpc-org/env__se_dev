// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.PlayerContainerData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public class PlayerContainerData
  {
    [ProtoMember(13)]
    public long PlayerId;
    [ProtoMember(16)]
    public int Timer;
    [ProtoMember(19)]
    public bool Active;
    [ProtoMember(22)]
    public bool Competetive;
    [ProtoMember(25)]
    public long ContainerId;

    public PlayerContainerData()
    {
    }

    public PlayerContainerData(
      long player,
      int timer,
      bool active,
      bool competetive,
      long container)
    {
      this.PlayerId = player;
      this.Timer = timer;
      this.Active = active;
      this.Competetive = competetive;
      this.ContainerId = container;
    }

    protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData\u003C\u003EPlayerId\u003C\u003EAccessor : IMemberAccessor<PlayerContainerData, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerContainerData owner, in long value) => owner.PlayerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerContainerData owner, out long value) => value = owner.PlayerId;
    }

    protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData\u003C\u003ETimer\u003C\u003EAccessor : IMemberAccessor<PlayerContainerData, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerContainerData owner, in int value) => owner.Timer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerContainerData owner, out int value) => value = owner.Timer;
    }

    protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData\u003C\u003EActive\u003C\u003EAccessor : IMemberAccessor<PlayerContainerData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerContainerData owner, in bool value) => owner.Active = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerContainerData owner, out bool value) => value = owner.Active;
    }

    protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData\u003C\u003ECompetetive\u003C\u003EAccessor : IMemberAccessor<PlayerContainerData, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerContainerData owner, in bool value) => owner.Competetive = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerContainerData owner, out bool value) => value = owner.Competetive;
    }

    protected class VRage_Game_ObjectBuilders_Components_PlayerContainerData\u003C\u003EContainerId\u003C\u003EAccessor : IMemberAccessor<PlayerContainerData, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PlayerContainerData owner, in long value) => owner.ContainerId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PlayerContainerData owner, out long value) => value = owner.ContainerId;
    }

    private class VRage_Game_ObjectBuilders_Components_PlayerContainerData\u003C\u003EActor : IActivator, IActivator<PlayerContainerData>
    {
      object IActivator.CreateInstance() => (object) new PlayerContainerData();

      PlayerContainerData IActivator<PlayerContainerData>.CreateInstance() => new PlayerContainerData();
    }
  }
}
