// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_BehaviorTreeSelectorNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
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
  public class MyObjectBuilder_BehaviorTreeSelectorNode : MyObjectBuilder_BehaviorControlBaseNode
  {
    protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode\u003C\u003EBTNodes\u003C\u003EAccessor : MyObjectBuilder_BehaviorControlBaseNode.VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003EBTNodes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, MyObjectBuilder_BehaviorTreeNode[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeSelectorNode owner,
        in MyObjectBuilder_BehaviorTreeNode[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_BehaviorControlBaseNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeSelectorNode owner,
        out MyObjectBuilder_BehaviorTreeNode[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_BehaviorControlBaseNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_BehaviorControlBaseNode.VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in string value) => this.Set((MyObjectBuilder_BehaviorControlBaseNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out string value) => this.Get((MyObjectBuilder_BehaviorControlBaseNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode\u003C\u003EIsMemorable\u003C\u003EAccessor : MyObjectBuilder_BehaviorControlBaseNode.VRage_Game_MyObjectBuilder_BehaviorControlBaseNode\u003C\u003EIsMemorable\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in bool value) => this.Set((MyObjectBuilder_BehaviorControlBaseNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out bool value) => this.Get((MyObjectBuilder_BehaviorControlBaseNode&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeSelectorNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeSelectorNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_BehaviorTreeSelectorNode owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_BehaviorTreeSelectorNode owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_BehaviorTreeSelectorNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_BehaviorTreeSelectorNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_BehaviorTreeSelectorNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_BehaviorTreeSelectorNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_BehaviorTreeSelectorNode();

      MyObjectBuilder_BehaviorTreeSelectorNode IActivator<MyObjectBuilder_BehaviorTreeSelectorNode>.CreateInstance() => new MyObjectBuilder_BehaviorTreeSelectorNode();
    }
  }
}
