// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.PerEntityData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game.ObjectBuilders.Components
{
  [ProtoContract]
  public class PerEntityData
  {
    [ProtoMember(4)]
    public long EntityId;
    [ProtoMember(7)]
    public SerializableDictionary<int, PerFrameData> Data;

    protected class VRage_Game_ObjectBuilders_Components_PerEntityData\u003C\u003EEntityId\u003C\u003EAccessor : IMemberAccessor<PerEntityData, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref PerEntityData owner, in long value) => owner.EntityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref PerEntityData owner, out long value) => value = owner.EntityId;
    }

    protected class VRage_Game_ObjectBuilders_Components_PerEntityData\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<PerEntityData, SerializableDictionary<int, PerFrameData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref PerEntityData owner,
        in SerializableDictionary<int, PerFrameData> value)
      {
        owner.Data = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref PerEntityData owner,
        out SerializableDictionary<int, PerFrameData> value)
      {
        value = owner.Data;
      }
    }

    private class VRage_Game_ObjectBuilders_Components_PerEntityData\u003C\u003EActor : IActivator, IActivator<PerEntityData>
    {
      object IActivator.CreateInstance() => (object) new PerEntityData();

      PerEntityData IActivator<PerEntityData>.CreateInstance() => new PerEntityData();
    }
  }
}
