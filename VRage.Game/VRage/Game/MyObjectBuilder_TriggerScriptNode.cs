// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TriggerScriptNode
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
  public class MyObjectBuilder_TriggerScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public string TriggerName = string.Empty;
    [ProtoMember(5)]
    public int SequenceInputID = -2;
    [ProtoMember(7)]
    public List<int> SequenceInputs = new List<int>();
    [ProtoMember(10)]
    public List<MyVariableIdentifier> InputIDs = new List<MyVariableIdentifier>();
    [ProtoMember(15)]
    public List<string> InputNames = new List<string>();
    [ProtoMember(20)]
    public List<string> InputTypes = new List<string>();

    public override void AfterDeserialize()
    {
      if (this.SequenceInputID == -2)
        return;
      if (this.SequenceInputID != -1)
        this.SequenceInputs.Add(this.SequenceInputID);
      this.SequenceInputID = -2;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003ETriggerName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in string value) => owner.TriggerName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out string value) => value = owner.TriggerName;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003ESequenceInputID\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in int value) => owner.SequenceInputID = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out int value) => value = owner.SequenceInputID;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003ESequenceInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerScriptNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in List<int> value) => owner.SequenceInputs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out List<int> value) => value = owner.SequenceInputs;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003EInputIDs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TriggerScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.InputIDs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TriggerScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.InputIDs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003EInputNames\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerScriptNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in List<string> value) => owner.InputNames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out List<string> value) => value = owner.InputNames;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003EInputTypes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerScriptNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in List<string> value) => owner.InputTypes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out List<string> value) => value = owner.InputTypes;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_TriggerScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TriggerScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TriggerScriptNode();

      MyObjectBuilder_TriggerScriptNode IActivator<MyObjectBuilder_TriggerScriptNode>.CreateInstance() => new MyObjectBuilder_TriggerScriptNode();
    }
  }
}
