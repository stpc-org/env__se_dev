// Decompiled with JetBrains decompiler
// Type: VRage.Game.DefinitionIdBlit
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  public struct DefinitionIdBlit
  {
    [ProtoMember(1)]
    [NoSerialize]
    public MyRuntimeObjectBuilderId TypeId;
    [ProtoMember(4)]
    public MyStringHash SubtypeId;

    [Serialize]
    private ushort m_typeIdSerialized
    {
      get => this.TypeId.Value;
      set => this.TypeId = new MyRuntimeObjectBuilderId(value);
    }

    public DefinitionIdBlit(MyObjectBuilderType type, MyStringHash subtypeId)
    {
      this.TypeId = (MyRuntimeObjectBuilderId) type;
      this.SubtypeId = subtypeId;
    }

    public DefinitionIdBlit(MyRuntimeObjectBuilderId typeId, MyStringHash subtypeId)
    {
      this.TypeId = typeId;
      this.SubtypeId = subtypeId;
    }

    public bool IsValid => this.TypeId.Value > (ushort) 0;

    public static implicit operator MyDefinitionId(DefinitionIdBlit id) => new MyDefinitionId((MyObjectBuilderType) id.TypeId, id.SubtypeId);

    public static implicit operator DefinitionIdBlit(MyDefinitionId id) => new DefinitionIdBlit(id.TypeId, id.SubtypeId);

    public override string ToString() => ((MyDefinitionId) this).ToString();

    protected class VRage_Game_DefinitionIdBlit\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<DefinitionIdBlit, MyRuntimeObjectBuilderId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref DefinitionIdBlit owner, in MyRuntimeObjectBuilderId value) => owner.TypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref DefinitionIdBlit owner, out MyRuntimeObjectBuilderId value) => value = owner.TypeId;
    }

    protected class VRage_Game_DefinitionIdBlit\u003C\u003ESubtypeId\u003C\u003EAccessor : IMemberAccessor<DefinitionIdBlit, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref DefinitionIdBlit owner, in MyStringHash value) => owner.SubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref DefinitionIdBlit owner, out MyStringHash value) => value = owner.SubtypeId;
    }

    protected class VRage_Game_DefinitionIdBlit\u003C\u003Em_typeIdSerialized\u003C\u003EAccessor : IMemberAccessor<DefinitionIdBlit, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref DefinitionIdBlit owner, in ushort value) => owner.m_typeIdSerialized = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref DefinitionIdBlit owner, out ushort value) => value = owner.m_typeIdSerialized;
    }

    private class VRage_Game_DefinitionIdBlit\u003C\u003EActor : IActivator, IActivator<DefinitionIdBlit>
    {
      object IActivator.CreateInstance() => (object) new DefinitionIdBlit();

      DefinitionIdBlit IActivator<DefinitionIdBlit>.CreateInstance() => new DefinitionIdBlit();
    }
  }
}
