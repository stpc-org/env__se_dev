// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.VisualScripting.MyObjectBuilder_InterfaceMethodNode
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
  public class MyObjectBuilder_InterfaceMethodNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public string MethodName;
    [ProtoMember(5)]
    public List<int> SequenceOutputIDs = new List<int>();
    [ProtoMember(10)]
    public List<IdentifierList> OutputIDs = new List<IdentifierList>();
    [ProtoMember(15)]
    public List<string> OutputNames = new List<string>();
    [ProtoMember(20)]
    public List<string> OuputTypes = new List<string>();

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003EMethodName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in string value) => owner.MethodName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out string value) => value = owner.MethodName;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003ESequenceOutputIDs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, List<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in List<int> value) => owner.SequenceOutputIDs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out List<int> value) => value = owner.SequenceOutputIDs;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003EOutputIDs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, List<IdentifierList>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_InterfaceMethodNode owner,
        in List<IdentifierList> value)
      {
        owner.OutputIDs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_InterfaceMethodNode owner,
        out List<IdentifierList> value)
      {
        value = owner.OutputIDs;
      }
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003EOutputNames\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in List<string> value) => owner.OutputNames = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out List<string> value) => value = owner.OutputNames;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003EOuputTypes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in List<string> value) => owner.OuputTypes = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out List<string> value) => value = owner.OuputTypes;
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_InterfaceMethodNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_InterfaceMethodNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_InterfaceMethodNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_VisualScripting_MyObjectBuilder_InterfaceMethodNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_InterfaceMethodNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_InterfaceMethodNode();

      MyObjectBuilder_InterfaceMethodNode IActivator<MyObjectBuilder_InterfaceMethodNode>.CreateInstance() => new MyObjectBuilder_InterfaceMethodNode();
    }
  }
}
