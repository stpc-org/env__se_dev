// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_SequenceScriptNode
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
  public class MyObjectBuilder_SequenceScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public int SequenceInput = -2;
    [ProtoMember(3)]
    public List<int> SequenceInputs = new List<int>();
    [ProtoMember(5)]
    public List<int> SequenceOutputs = new List<int>();

    public override void AfterDeserialize()
    {
      if (this.SequenceInput == -2)
        return;
      if (this.SequenceInput != -1)
        this.SequenceInputs.Add(this.SequenceInput);
      this.SequenceInput = -2;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003ESequenceInput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SequenceScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in int value) => owner.SequenceInput = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out int value) => value = owner.SequenceInput;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003ESequenceInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SequenceScriptNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in List<int> value) => owner.SequenceInputs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out List<int> value) => value = owner.SequenceInputs;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003ESequenceOutputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SequenceScriptNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in List<int> value) => owner.SequenceOutputs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out List<int> value) => value = owner.SequenceOutputs;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SequenceScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SequenceScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SequenceScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SequenceScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SequenceScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SequenceScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SequenceScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SequenceScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SequenceScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SequenceScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SequenceScriptNode();

      MyObjectBuilder_SequenceScriptNode IActivator<MyObjectBuilder_SequenceScriptNode>.CreateInstance() => new MyObjectBuilder_SequenceScriptNode();
    }
  }
}
