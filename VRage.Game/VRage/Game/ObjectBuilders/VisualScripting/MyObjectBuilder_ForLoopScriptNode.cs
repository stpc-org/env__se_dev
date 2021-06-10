// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_ForLoopScriptNode
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

namespace VRage.Game.ObjectBuilders.VisualScripting
{
  [MyObjectBuilderDefinition(null, null)]
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ForLoopScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public List<int> SequenceInputs = new List<int>();
    [ProtoMember(5)]
    public int SequenceBody = -1;
    [ProtoMember(10)]
    public int SequenceOutput = -1;
    [ProtoMember(15)]
    public MyVariableIdentifier FirstIndexValueInput = MyVariableIdentifier.Default;
    [ProtoMember(20)]
    public MyVariableIdentifier LastIndexValueInput = MyVariableIdentifier.Default;
    [ProtoMember(25)]
    public MyVariableIdentifier IncrementValueInput = MyVariableIdentifier.Default;
    [ProtoMember(27)]
    public MyVariableIdentifier ConditionValueInput = MyVariableIdentifier.Default;
    [ProtoMember(30)]
    public List<MyVariableIdentifier> CounterValueOutputs = new List<MyVariableIdentifier>();
    [ProtoMember(35)]
    public string FirstIndexValue = "0";
    [ProtoMember(40)]
    public string LastIndexValue = "0";
    [ProtoMember(45)]
    public string IncrementValue = "1";
    [ProtoMember(50)]
    public string ConditionValue = "";

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003ESequenceInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in List<int> value) => owner.SequenceInputs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out List<int> value) => value = owner.SequenceInputs;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003ESequenceBody\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in int value) => owner.SequenceBody = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out int value) => value = owner.SequenceBody;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003ESequenceOutput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in int value) => owner.SequenceOutput = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out int value) => value = owner.SequenceOutput;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EFirstIndexValueInput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.FirstIndexValueInput = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.FirstIndexValueInput;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003ELastIndexValueInput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.LastIndexValueInput = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.LastIndexValueInput;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EIncrementValueInput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.IncrementValueInput = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.IncrementValueInput;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EConditionValueInput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.ConditionValueInput = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.ConditionValueInput;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003ECounterValueOutputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.CounterValueOutputs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ForLoopScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.CounterValueOutputs;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EFirstIndexValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value) => owner.FirstIndexValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value) => value = owner.FirstIndexValue;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003ELastIndexValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value) => owner.LastIndexValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value) => value = owner.LastIndexValue;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EIncrementValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value) => owner.IncrementValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value) => value = owner.IncrementValue;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EConditionValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value) => owner.ConditionValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value) => value = owner.ConditionValue;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ForLoopScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ForLoopScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ForLoopScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_ForLoopScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ForLoopScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ForLoopScriptNode();

      MyObjectBuilder_ForLoopScriptNode IActivator<MyObjectBuilder_ForLoopScriptNode>.CreateInstance() => new MyObjectBuilder_ForLoopScriptNode();
    }
  }
}
