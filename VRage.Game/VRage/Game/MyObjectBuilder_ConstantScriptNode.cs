// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ConstantScriptNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
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
  public class MyObjectBuilder_ConstantScriptNode : MyObjectBuilder_ScriptNode
  {
    [ProtoMember(1)]
    public string Value = string.Empty;
    [ProtoMember(5)]
    public string Type = string.Empty;
    [ProtoMember(10)]
    public IdentifierList OutputIds = new IdentifierList();
    [ProtoMember(15)]
    public Vector3D Vector;
    [ProtoMember(20)]
    public IdentifierList OutputIdsX = new IdentifierList();
    [ProtoMember(25)]
    public IdentifierList OutputIdsY = new IdentifierList();
    [ProtoMember(30)]
    public IdentifierList OutputIdsZ = new IdentifierList();

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in string value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out string value) => value = owner.Value;
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in string value) => owner.Type = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out string value) => value = owner.Type;
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EOutputIds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, IdentifierList>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in IdentifierList value) => owner.OutputIds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConstantScriptNode owner,
        out IdentifierList value)
      {
        value = owner.OutputIds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EVector\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, Vector3D>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in Vector3D value) => owner.Vector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out Vector3D value) => value = owner.Vector;
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EOutputIdsX\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, IdentifierList>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in IdentifierList value) => owner.OutputIdsX = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConstantScriptNode owner,
        out IdentifierList value)
      {
        value = owner.OutputIdsX;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EOutputIdsY\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, IdentifierList>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in IdentifierList value) => owner.OutputIdsY = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConstantScriptNode owner,
        out IdentifierList value)
      {
        value = owner.OutputIdsY;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EOutputIdsZ\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ConstantScriptNode, IdentifierList>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in IdentifierList value) => owner.OutputIdsZ = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ConstantScriptNode owner,
        out IdentifierList value)
      {
        value = owner.OutputIdsZ;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EID\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EID\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in int value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out int value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_ScriptNode.VRage_Game_MyObjectBuilder_ScriptNode\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in Vector2 value) => this.Set((MyObjectBuilder_ScriptNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out Vector2 value) => this.Get((MyObjectBuilder_ScriptNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ConstantScriptNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ConstantScriptNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ConstantScriptNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ConstantScriptNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ConstantScriptNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ConstantScriptNode();

      MyObjectBuilder_ConstantScriptNode IActivator<MyObjectBuilder_ConstantScriptNode>.CreateInstance() => new MyObjectBuilder_ConstantScriptNode();
    }
  }
}
