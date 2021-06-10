// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ArithmeticScriptNode
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
  public class MyObjectBuilder_ArithmeticScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public List<MyVariableIdentifier> OutputNodeIDs = new List<MyVariableIdentifier>();
    [ProtoMember(5)]
    public string Operation;
    [ProtoMember(10)]
    public string Type;
    [ProtoMember(15)]
    public MyVariableIdentifier InputAID = MyVariableIdentifier.Default;
    [ProtoMember(20)]
    public MyVariableIdentifier InputBID = MyVariableIdentifier.Default;
    [ProtoMember(25)]
    public string ValueA = string.Empty;
    [ProtoMember(30)]
    public string ValueB = string.Empty;

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EOutputNodeIDs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ArithmeticScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.OutputNodeIDs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ArithmeticScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.OutputNodeIDs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EOperation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value) => owner.Operation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value) => value = owner.Operation;
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EInputAID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ArithmeticScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.InputAID = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ArithmeticScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.InputAID;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EInputBID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ArithmeticScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.InputBID = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ArithmeticScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.InputBID;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EValueA\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value) => owner.ValueA = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value) => value = owner.ValueA;
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EValueB\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value) => owner.ValueB = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value) => value = owner.ValueB;
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ArithmeticScriptNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ArithmeticScriptNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ArithmeticScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ArithmeticScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ArithmeticScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ArithmeticScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ArithmeticScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ArithmeticScriptNode();

      MyObjectBuilder_ArithmeticScriptNode IActivator<MyObjectBuilder_ArithmeticScriptNode>.CreateInstance() => new MyObjectBuilder_ArithmeticScriptNode();
    }
  }
}
