// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyInputParameterSerializationData
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRage.Game
{
  [ProtoContract]
  public class MyInputParameterSerializationData
  {
    [ProtoMember(1)]
    public string Type;
    [ProtoMember(5)]
    public string Name;
    [ProtoMember(10)]
    public MyVariableIdentifier Input;

    public MyInputParameterSerializationData() => this.Input = MyVariableIdentifier.Default;

    protected class VRage_Game_MyInputParameterSerializationData\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyInputParameterSerializationData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputParameterSerializationData owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputParameterSerializationData owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_MyInputParameterSerializationData\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyInputParameterSerializationData, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyInputParameterSerializationData owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyInputParameterSerializationData owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyInputParameterSerializationData\u003C\u003EInput\u003C\u003EAccessor : IMemberAccessor<MyInputParameterSerializationData, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyInputParameterSerializationData owner,
        in MyVariableIdentifier value)
      {
        owner.Input = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyInputParameterSerializationData owner,
        out MyVariableIdentifier value)
      {
        value = owner.Input;
      }
    }

    private class VRage_Game_MyInputParameterSerializationData\u003C\u003EActor : IActivator, IActivator<MyInputParameterSerializationData>
    {
      object IActivator.CreateInstance() => (object) new MyInputParameterSerializationData();

      MyInputParameterSerializationData IActivator<MyInputParameterSerializationData>.CreateInstance() => new MyInputParameterSerializationData();
    }
  }
}
