﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_LocalizationScriptNode
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
  public class MyObjectBuilder_LocalizationScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public string Context = string.Empty;
    [ProtoMember(5)]
    public string MessageId = string.Empty;
    [ProtoMember(10)]
    public ulong ResourceId = ulong.MaxValue;
    [ProtoMember(15)]
    public List<MyVariableIdentifier> ParameterInputs = new List<MyVariableIdentifier>();
    [ProtoMember(20)]
    public List<MyVariableIdentifier> ValueOutputs = new List<MyVariableIdentifier>();

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003EContext\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LocalizationScriptNode owner, in string value) => owner.Context = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LocalizationScriptNode owner, out string value) => value = owner.Context;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003EMessageId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LocalizationScriptNode owner, in string value) => owner.MessageId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LocalizationScriptNode owner, out string value) => value = owner.MessageId;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003EResourceId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, ulong>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LocalizationScriptNode owner, in ulong value) => owner.ResourceId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LocalizationScriptNode owner, out ulong value) => value = owner.ResourceId;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003EParameterInputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.ParameterInputs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.ParameterInputs;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003EValueOutputs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.ValueOutputs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.ValueOutputs;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LocalizationScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LocalizationScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LocalizationScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LocalizationScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationScriptNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationScriptNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LocalizationScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LocalizationScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_LocalizationScriptNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_LocalizationScriptNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_LocalizationScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_LocalizationScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_LocalizationScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_LocalizationScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_LocalizationScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_LocalizationScriptNode();

      MyObjectBuilder_LocalizationScriptNode IActivator<MyObjectBuilder_LocalizationScriptNode>.CreateInstance() => new MyObjectBuilder_LocalizationScriptNode();
    }
  }
}
