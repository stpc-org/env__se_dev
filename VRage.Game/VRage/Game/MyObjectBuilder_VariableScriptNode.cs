// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_VariableScriptNode
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
  public class MyObjectBuilder_VariableScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public string VariableName = "Default";
    [ProtoMember(5)]
    public string VariableType = string.Empty;
    [ProtoMember(10)]
    public string VariableValue = string.Empty;
    [ProtoMember(15)]
    public List<MyVariableIdentifier> OutputNodeIds = new List<MyVariableIdentifier>();
    [ProtoMember(20)]
    public Vector3D Vector;
    [ProtoMember(25)]
    public List<MyVariableIdentifier> OutputNodeIdsX = new List<MyVariableIdentifier>();
    [ProtoMember(30)]
    public List<MyVariableIdentifier> OutputNodeIdsY = new List<MyVariableIdentifier>();
    [ProtoMember(35)]
    public List<MyVariableIdentifier> OutputNodeIdsZ = new List<MyVariableIdentifier>();

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EVariableName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value) => owner.VariableName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value) => value = owner.VariableName;
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EVariableType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value) => owner.VariableType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value) => value = owner.VariableType;
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EVariableValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value) => owner.VariableValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value) => value = owner.VariableValue;
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EOutputNodeIds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VariableScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.OutputNodeIds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VariableScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.OutputNodeIds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EVector\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in Vector3D value) => owner.Vector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out Vector3D value) => value = owner.Vector;
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EOutputNodeIdsX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VariableScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.OutputNodeIdsX = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VariableScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.OutputNodeIdsX;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EOutputNodeIdsY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VariableScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.OutputNodeIdsY = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VariableScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.OutputNodeIdsY;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EOutputNodeIdsZ\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_VariableScriptNode, List<MyVariableIdentifier>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_VariableScriptNode owner,
        in List<MyVariableIdentifier> value)
      {
        owner.OutputNodeIdsZ = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_VariableScriptNode owner,
        out List<MyVariableIdentifier> value)
      {
        value = owner.OutputNodeIdsZ;
      }
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_VariableScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_VariableScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_VariableScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_VariableScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_VariableScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_VariableScriptNode();

      MyObjectBuilder_VariableScriptNode IActivator<MyObjectBuilder_VariableScriptNode>.CreateInstance() => new MyObjectBuilder_VariableScriptNode();
    }
  }
}
