// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_SwitchScriptNode
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
  public class MyObjectBuilder_SwitchScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public int SequenceInput = -2;
    [ProtoMember(3)]
    public List<int> SequenceInputs = new List<int>();
    [ProtoMember(5)]
    public readonly List<MyObjectBuilder_SwitchScriptNode.OptionData> Options = new List<MyObjectBuilder_SwitchScriptNode.OptionData>();
    [ProtoMember(10)]
    public MyVariableIdentifier ValueInput;
    [ProtoMember(15)]
    public string NodeType = string.Empty;

    public override void AfterDeserialize()
    {
      if (this.SequenceInput == -2)
        return;
      if (this.SequenceInput != -1)
        this.SequenceInputs.Add(this.SequenceInput);
      this.SequenceInput = -2;
    }

    [ProtoContract]
    public struct OptionData
    {
      [ProtoMember(100)]
      public string Option;
      [ProtoMember(110)]
      public int SequenceOutput;

      protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003EOptionData\u003C\u003EOption\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SwitchScriptNode.OptionData, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SwitchScriptNode.OptionData owner,
          in string value)
        {
          owner.Option = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SwitchScriptNode.OptionData owner,
          out string value)
        {
          value = owner.Option;
        }
      }

      protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003EOptionData\u003C\u003ESequenceOutput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SwitchScriptNode.OptionData, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_SwitchScriptNode.OptionData owner,
          in int value)
        {
          owner.SequenceOutput = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_SwitchScriptNode.OptionData owner,
          out int value)
        {
          value = owner.SequenceOutput;
        }
      }

      private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003EOptionData\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SwitchScriptNode.OptionData>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_SwitchScriptNode.OptionData();

        MyObjectBuilder_SwitchScriptNode.OptionData IActivator<MyObjectBuilder_SwitchScriptNode.OptionData>.CreateInstance() => new MyObjectBuilder_SwitchScriptNode.OptionData();
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003ESequenceInput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SwitchScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in int value) => owner.SequenceInput = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out int value) => value = owner.SequenceInput;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003ESequenceInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SwitchScriptNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in List<int> value) => owner.SequenceInputs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out List<int> value) => value = owner.SequenceInputs;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003EOptions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SwitchScriptNode, List<MyObjectBuilder_SwitchScriptNode.OptionData>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SwitchScriptNode owner,
        in List<MyObjectBuilder_SwitchScriptNode.OptionData> value)
      {
        owner.Options = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SwitchScriptNode owner,
        out List<MyObjectBuilder_SwitchScriptNode.OptionData> value)
      {
        value = owner.Options;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003EValueInput\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SwitchScriptNode, MyVariableIdentifier>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SwitchScriptNode owner,
        in MyVariableIdentifier value)
      {
        owner.ValueInput = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SwitchScriptNode owner,
        out MyVariableIdentifier value)
      {
        value = owner.ValueInput;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003ENodeType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SwitchScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in string value) => owner.NodeType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out string value) => value = owner.NodeType;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SwitchScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SwitchScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SwitchScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SwitchScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SwitchScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SwitchScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SwitchScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SwitchScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_SwitchScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SwitchScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SwitchScriptNode();

      MyObjectBuilder_SwitchScriptNode IActivator<MyObjectBuilder_SwitchScriptNode>.CreateInstance() => new MyObjectBuilder_SwitchScriptNode();
    }
  }
}
