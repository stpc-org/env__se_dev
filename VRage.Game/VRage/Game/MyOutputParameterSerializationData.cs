// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyOutputParameterSerializationData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyOutputParameterSerializationData
  {
    [ProtoMember(1)]
    public string Type;
    [ProtoMember(5)]
    public string Name;
    [ProtoMember(10)]
    public IdentifierList Outputs = new IdentifierList();

    protected class VRage_Game_MyOutputParameterSerializationData\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyOutputParameterSerializationData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyOutputParameterSerializationData owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyOutputParameterSerializationData owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_MyOutputParameterSerializationData\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyOutputParameterSerializationData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyOutputParameterSerializationData owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyOutputParameterSerializationData owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyOutputParameterSerializationData\u003C\u003EOutputs\u003C\u003EAccessor : IMemberAccessor<MyOutputParameterSerializationData, IdentifierList>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyOutputParameterSerializationData owner, in IdentifierList value) => owner.Outputs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyOutputParameterSerializationData owner,
        out IdentifierList value)
      {
        value = owner.Outputs;
      }
    }

    private class VRage_Game_MyOutputParameterSerializationData\u003C\u003EActor : IActivator, IActivator<MyOutputParameterSerializationData>
    {
      object IActivator.CreateInstance() => (object) new MyOutputParameterSerializationData();

      MyOutputParameterSerializationData IActivator<MyOutputParameterSerializationData>.CreateInstance() => new MyOutputParameterSerializationData();
    }
  }
}
