// Decompiled with JetBrains decompiler
// Type: VRage.Game.SuitResourceDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;

namespace VRage.Game
{
  [ProtoContract]
  public class SuitResourceDefinition
  {
    [ProtoMember(6)]
    public SerializableDefinitionId Id;
    [ProtoMember(7)]
    public float MaxCapacity;
    [ProtoMember(8)]
    public float Throughput;

    protected class VRage_Game_SuitResourceDefinition\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<SuitResourceDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SuitResourceDefinition owner, in SerializableDefinitionId value) => owner.Id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SuitResourceDefinition owner, out SerializableDefinitionId value) => value = owner.Id;
    }

    protected class VRage_Game_SuitResourceDefinition\u003C\u003EMaxCapacity\u003C\u003EAccessor : IMemberAccessor<SuitResourceDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SuitResourceDefinition owner, in float value) => owner.MaxCapacity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SuitResourceDefinition owner, out float value) => value = owner.MaxCapacity;
    }

    protected class VRage_Game_SuitResourceDefinition\u003C\u003EThroughput\u003C\u003EAccessor : IMemberAccessor<SuitResourceDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SuitResourceDefinition owner, in float value) => owner.Throughput = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SuitResourceDefinition owner, out float value) => value = owner.Throughput;
    }

    private class VRage_Game_SuitResourceDefinition\u003C\u003EActor : IActivator, IActivator<SuitResourceDefinition>
    {
      object IActivator.CreateInstance() => (object) new SuitResourceDefinition();

      SuitResourceDefinition IActivator<SuitResourceDefinition>.CreateInstance() => new SuitResourceDefinition();
    }
  }
}
