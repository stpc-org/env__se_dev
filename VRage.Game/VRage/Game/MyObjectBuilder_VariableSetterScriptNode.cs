// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_VariableSetterScriptNode
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
  public class MyObjectBuilder_VariableSetterScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public string VariableName = string.Empty;
    [ProtoMember(5)]
    public string VariableValue = string.Empty;
    [ProtoMember(10)]
    public int SequenceInputID = -2;
    [ProtoMember(13)]
    public List<int> SequenceInputs = new List<int>();
    [ProtoMember(15)]
    public int SequenceOutputID = -1;
    [ProtoMember(20)]
    public MyVariableIdentifier ValueInputID = MyVariableIdentifier.Default;

    public override void AfterDeserialize()
    {
      if (this.SequenceInputID == -2)
        return;
      if (this.SequenceInputID != -1)
        this.SequenceInputs.Add(this.SequenceInputID);
      this.SequenceInputID = -2;
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003EVariableName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in string value) => owner.VariableName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out string value) => value = owner.VariableName;
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003EVariableValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in string value) => owner.VariableValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out string value) => value = owner.VariableValue;
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003ESequenceInputID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in int value) => owner.SequenceInputID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out int value) => value = owner.SequenceInputID;
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003ESequenceInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VariableSetterScriptNode owner,
        in List<int> value)
      {
        owner.SequenceInputs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VariableSetterScriptNode owner,
        out List<int> value)
      {
        value = owner.SequenceInputs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003ESequenceOutputID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in int value) => owner.SequenceOutputID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out int value) => value = owner.SequenceOutputID;
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003EValueInputID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VariableSetterScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.ValueInputID = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VariableSetterScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.ValueInputID;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VariableSetterScriptNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VariableSetterScriptNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VariableSetterScriptNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VariableSetterScriptNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableSetterScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableSetterScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableSetterScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_VariableSetterScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VariableSetterScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VariableSetterScriptNode();

      MyObjectBuilder_VariableSetterScriptNode IActivator<MyObjectBuilder_VariableSetterScriptNode>.CreateInstance() => new MyObjectBuilder_VariableSetterScriptNode();
    }
  }
}
