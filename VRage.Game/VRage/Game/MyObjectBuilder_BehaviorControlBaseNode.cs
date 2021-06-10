// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_BehaviorControlBaseNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_BehaviorControlBaseNode : MyObjectBuilder_BehaviorTreeNode
  {
    [ProtoMember(1)]
    [XmlArrayItem("BTNode", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_BehaviorTreeNode>))]
    public MyObjectBuilder_BehaviorTreeNode[] BTNodes;
    [ProtoMember(4)]
    public string Name;
    [ProtoMember(7)]
    [DefaultValue(false)]
    public bool IsMemorable;

    protected class VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003EBTNodes\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorControlBaseNode, MyObjectBuilder_BehaviorTreeNode[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorControlBaseNode owner,
        in MyObjectBuilder_BehaviorTreeNode[] value)
      {
        owner.BTNodes = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorControlBaseNode owner,
        out MyObjectBuilder_BehaviorTreeNode[] value)
      {
        value = owner.BTNodes;
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorControlBaseNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorControlBaseNode owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorControlBaseNode owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003EIsMemorable\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_BehaviorControlBaseNode, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorControlBaseNode owner, in bool value) => owner.IsMemorable = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorControlBaseNode owner, out bool value) => value = owner.IsMemorable;
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorControlBaseNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorControlBaseNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorControlBaseNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorControlBaseNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorControlBaseNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorControlBaseNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorControlBaseNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorControlBaseNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorControlBaseNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorControlBaseNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorControlBaseNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorControlBaseNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorControlBaseNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorControlBaseNode();

      MyObjectBuilder_BehaviorControlBaseNode IActivator<MyObjectBuilder_BehaviorControlBaseNode>.CreateInstance() => new MyObjectBuilder_BehaviorControlBaseNode();
    }
  }
}
