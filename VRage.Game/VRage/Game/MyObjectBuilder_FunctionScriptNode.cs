// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_FunctionScriptNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_FunctionScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public int Version;
    [ProtoMember(5)]
    public string DeclaringType = string.Empty;
    [ProtoMember(10)]
    public string Type = string.Empty;
    [ProtoMember(15)]
    public string ExtOfType = string.Empty;
    [ProtoMember(20)]
    public int SequenceInputID = -2;
    [ProtoMember(23)]
    public List<int> SequenceInputs = new List<int>();
    [ProtoMember(25)]
    public int SequenceOutputID = -1;
    [ProtoMember(30)]
    public MyVariableIdentifier InstanceInputID = MyVariableIdentifier.Default;
    [ProtoMember(35)]
    public List<MyVariableIdentifier> InputParameterIDs = new List<MyVariableIdentifier>();
    [ProtoMember(40)]
    public List<IdentifierList> OutputParametersIDs = new List<IdentifierList>();
    [ProtoMember(45)]
    public List<MyParameterValue> InputParameterValues = new List<MyParameterValue>();

    public override void AfterDeserialize()
    {
      if (this.SequenceInputID == -2)
        return;
      if (this.SequenceInputID != -1)
        this.SequenceInputs.Add(this.SequenceInputID);
      this.SequenceInputID = -2;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EVersion\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in int value) => owner.Version = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out int value) => value = owner.Version;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EDeclaringType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value) => owner.DeclaringType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value) => value = owner.DeclaringType;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EExtOfType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value) => owner.ExtOfType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value) => value = owner.ExtOfType;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003ESequenceInputID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in int value) => owner.SequenceInputID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out int value) => value = owner.SequenceInputID;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003ESequenceInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in List<int> value) => owner.SequenceInputs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out List<int> value) => value = owner.SequenceInputs;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003ESequenceOutputID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in int value) => owner.SequenceOutputID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out int value) => value = owner.SequenceOutputID;
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EInstanceInputID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.InstanceInputID = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.InstanceInputID;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EInputParameterIDs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.InputParameterIDs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.InputParameterIDs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EOutputParametersIDs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, List<IdentifierList>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionScriptNode owner,
        in List<IdentifierList> value)
      {
        owner.OutputParametersIDs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionScriptNode owner,
        out List<IdentifierList> value)
      {
        value = owner.OutputParametersIDs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EInputParameterValues\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_FunctionScriptNode, List<MyParameterValue>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_FunctionScriptNode owner,
        in List<MyParameterValue> value)
      {
        owner.InputParameterValues = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_FunctionScriptNode owner,
        out List<MyParameterValue> value)
      {
        value = owner.InputParameterValues;
      }
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_FunctionScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_FunctionScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_FunctionScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_FunctionScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_FunctionScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_FunctionScriptNode();

      MyObjectBuilder_FunctionScriptNode IActivator<MyObjectBuilder_FunctionScriptNode>.CreateInstance() => new MyObjectBuilder_FunctionScriptNode();
    }
  }
}
