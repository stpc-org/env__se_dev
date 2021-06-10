// Decompiled with JetBrains decompiler
// Type: VRage.Game.IdentifierList
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class IdentifierList
  {
    [ProtoMember(1)]
    public string OriginName;
    [ProtoMember(5)]
    public string OriginType;
    [ProtoMember(10)]
    public List<MyVariableIdentifier> Ids;

    public IdentifierList() => this.Ids = new List<MyVariableIdentifier>();

    protected class VRage_Game_IdentifierList\u003C\u003EOriginName\u003C\u003EAccessor : IMemberAccessor<IdentifierList, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref IdentifierList owner, in string value) => owner.OriginName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref IdentifierList owner, out string value) => value = owner.OriginName;
    }

    protected class VRage_Game_IdentifierList\u003C\u003EOriginType\u003C\u003EAccessor : IMemberAccessor<IdentifierList, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref IdentifierList owner, in string value) => owner.OriginType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref IdentifierList owner, out string value) => value = owner.OriginType;
    }

    protected class VRage_Game_IdentifierList\u003C\u003EIds\u003C\u003EAccessor : IMemberAccessor<IdentifierList, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref IdentifierList owner, in List<MyVariableIdentifier> value) => owner.Ids = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref IdentifierList owner, out List<MyVariableIdentifier> value) => value = owner.Ids;
    }

    private class VRage_Game_IdentifierList\u003C\u003EActor : IActivator, IActivator<IdentifierList>
    {
      object IActivator.CreateInstance() => (object) new IdentifierList();

      IdentifierList IActivator<IdentifierList>.CreateInstance() => new IdentifierList();
    }
  }
}
