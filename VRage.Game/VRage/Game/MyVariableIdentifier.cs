// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyVariableIdentifier
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Game
{
  [ProtoContract]
  public struct MyVariableIdentifier
  {
    [ProtoMember(1)]
    public int NodeID;
    [ProtoMember(5)]
    public string VariableName;
    [ProtoMember(10)]
    public string OriginName;
    [ProtoMember(15)]
    public string OriginType;
    [NoSerialize]
    public static MyVariableIdentifier Default = new MyVariableIdentifier()
    {
      NodeID = -1,
      VariableName = ""
    };

    public MyVariableIdentifier(int nodeId, string variableName)
    {
      this.NodeID = nodeId;
      this.VariableName = variableName;
      this.OriginName = string.Empty;
      this.OriginType = string.Empty;
    }

    public MyVariableIdentifier(int nodeId, string variableName, ParameterInfo parameter)
      : this(nodeId, variableName)
    {
      this.OriginName = parameter.Name;
      this.OriginType = this.Signature(parameter.ParameterType);
      this.VariableName = variableName;
    }

    public string Signature(Type type) => type.IsEnum ? type.FullName.Replace("+", ".") : type.FullName;

    public MyVariableIdentifier(ParameterInfo parameter)
      : this(-1, string.Empty, parameter)
    {
    }

    public override bool Equals(object obj) => obj is MyVariableIdentifier variableIdentifier && this.NodeID == variableIdentifier.NodeID && this.VariableName == variableIdentifier.VariableName;

    protected class VRage_Game_MyVariableIdentifier\u003C\u003ENodeID\u003C\u003EAccessor : IMemberAccessor<MyVariableIdentifier, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyVariableIdentifier owner, in int value) => owner.NodeID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyVariableIdentifier owner, out int value) => value = owner.NodeID;
    }

    protected class VRage_Game_MyVariableIdentifier\u003C\u003EVariableName\u003C\u003EAccessor : IMemberAccessor<MyVariableIdentifier, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyVariableIdentifier owner, in string value) => owner.VariableName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyVariableIdentifier owner, out string value) => value = owner.VariableName;
    }

    protected class VRage_Game_MyVariableIdentifier\u003C\u003EOriginName\u003C\u003EAccessor : IMemberAccessor<MyVariableIdentifier, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyVariableIdentifier owner, in string value) => owner.OriginName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyVariableIdentifier owner, out string value) => value = owner.OriginName;
    }

    protected class VRage_Game_MyVariableIdentifier\u003C\u003EOriginType\u003C\u003EAccessor : IMemberAccessor<MyVariableIdentifier, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyVariableIdentifier owner, in string value) => owner.OriginType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyVariableIdentifier owner, out string value) => value = owner.OriginType;
    }

    private class VRage_Game_MyVariableIdentifier\u003C\u003EActor : IActivator, IActivator<MyVariableIdentifier>
    {
      object IActivator.CreateInstance() => (object) new MyVariableIdentifier();

      MyVariableIdentifier IActivator<MyVariableIdentifier>.CreateInstance() => new MyVariableIdentifier();
    }
  }
}
