// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ScriptScriptNode
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
  public class MyObjectBuilder_ScriptScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public string Name = string.Empty;
    [ProtoMember(5)]
    public string Path;
    [ProtoMember(10)]
    public int SequenceOutput = -1;
    [ProtoMember(15)]
    public int SequenceInput = -2;
    [ProtoMember(17)]
    public List<int> SequenceInputs = new List<int>();
    [ProtoMember(20)]
    public List<MyInputParameterSerializationData> Inputs = new List<MyInputParameterSerializationData>();
    [ProtoMember(25)]
    public List<MyOutputParameterSerializationData> Outputs = new List<MyOutputParameterSerializationData>();

    public override void AfterDeserialize()
    {
      if (this.SequenceInput == -2)
        return;
      if (this.SequenceInput != -1)
        this.SequenceInputs.Add(this.SequenceInput);
      this.SequenceInput = -2;
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003EPath\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in string value) => owner.Path = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out string value) => value = owner.Path;
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003ESequenceOutput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in int value) => owner.SequenceOutput = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out int value) => value = owner.SequenceOutput;
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003ESequenceInput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in int value) => owner.SequenceInput = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out int value) => value = owner.SequenceInput;
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003ESequenceInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in List<int> value) => owner.SequenceInputs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out List<int> value) => value = owner.SequenceInputs;
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003EInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, List<MyInputParameterSerializationData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptScriptNode owner,
        in List<MyInputParameterSerializationData> value)
      {
        owner.Inputs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptScriptNode owner,
        out List<MyInputParameterSerializationData> value)
      {
        value = owner.Inputs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003EOutputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScriptScriptNode, List<MyOutputParameterSerializationData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScriptScriptNode owner,
        in List<MyOutputParameterSerializationData> value)
      {
        owner.Outputs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScriptScriptNode owner,
        out List<MyOutputParameterSerializationData> value)
      {
        value = owner.Outputs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScriptScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScriptScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScriptScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ScriptScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScriptScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScriptScriptNode();

      MyObjectBuilder_ScriptScriptNode IActivator<MyObjectBuilder_ScriptScriptNode>.CreateInstance() => new MyObjectBuilder_ScriptScriptNode();
    }
  }
}
