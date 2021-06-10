// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationTree
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

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AnimationTree : MyObjectBuilder_AnimationTreeNode
  {
    [ProtoMember(4)]
    [XmlElement(typeof (MyAbstractXmlSerializer<MyObjectBuilder_AnimationTreeNode>))]
    public MyObjectBuilder_AnimationTreeNode Child;
    [ProtoMember(7)]
    [XmlArrayItem(typeof (MyAbstractXmlSerializer<MyObjectBuilder_AnimationTreeNode>))]
    public MyObjectBuilder_AnimationTreeNode[] Orphans;

    public MyObjectBuilder_AnimationTree DeepCopyWithMask(
      HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes)
    {
      List<MyObjectBuilder_AnimationTreeNode> orphans = new List<MyObjectBuilder_AnimationTreeNode>();
      if (this.Orphans != null)
      {
        foreach (MyObjectBuilder_AnimationTreeNode orphan in this.Orphans)
          orphan.DeepCopyWithMask(selectedNodes, (MyObjectBuilder_AnimationTreeNode) null, orphans);
      }
      return (MyObjectBuilder_AnimationTree) this.DeepCopyWithMask(selectedNodes, (MyObjectBuilder_AnimationTreeNode) null, orphans);
    }

    protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(
      HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes,
      MyObjectBuilder_AnimationTreeNode parentNode,
      List<MyObjectBuilder_AnimationTreeNode> orphans)
    {
      MyObjectBuilder_AnimationTree builderAnimationTree = new MyObjectBuilder_AnimationTree();
      if (this.Child == null)
      {
        builderAnimationTree.Child = (MyObjectBuilder_AnimationTreeNode) null;
        builderAnimationTree.Orphans = orphans.Count > 0 ? orphans.ToArray() : (MyObjectBuilder_AnimationTreeNode[]) null;
        return (MyObjectBuilder_AnimationTreeNode) builderAnimationTree;
      }
      MyObjectBuilder_AnimationTreeNode animationTreeNode = this.Child.DeepCopyWithMask(selectedNodes, (MyObjectBuilder_AnimationTreeNode) builderAnimationTree, orphans);
      builderAnimationTree.EdPos = this.EdPos;
      builderAnimationTree.Child = animationTreeNode;
      builderAnimationTree.Orphans = orphans.Count > 0 ? orphans.ToArray() : (MyObjectBuilder_AnimationTreeNode[]) null;
      return (MyObjectBuilder_AnimationTreeNode) builderAnimationTree;
    }

    public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
    {
      if (this.Child == null)
        return (MyObjectBuilder_AnimationTreeNode[]) null;
      return new MyObjectBuilder_AnimationTreeNode[1]
      {
        this.Child
      };
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003EChild\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTree, MyObjectBuilder_AnimationTreeNode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTree owner,
        in MyObjectBuilder_AnimationTreeNode value)
      {
        owner.Child = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTree owner,
        out MyObjectBuilder_AnimationTreeNode value)
      {
        value = owner.Child;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003EOrphans\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTree, MyObjectBuilder_AnimationTreeNode[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTree owner,
        in MyObjectBuilder_AnimationTreeNode[] value)
      {
        owner.Orphans = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTree owner,
        out MyObjectBuilder_AnimationTreeNode[] value)
      {
        value = owner.Orphans;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003EEdPos\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEdPos\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, Vector2I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTree owner, in Vector2I? value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTree owner, out Vector2I? value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003EEventNames\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventNames\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTree owner, in List<string> value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTree owner, out List<string> value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003EEventTimes\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, List<double>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTree owner, in List<double> value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTree owner, out List<double> value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003EKey\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EKey\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTree owner, in string value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTree owner, out string value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTree owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTree owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTree owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTree owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTree owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTree owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTree, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTree owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTree owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTree\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTree>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationTree();

      MyObjectBuilder_AnimationTree IActivator<MyObjectBuilder_AnimationTree>.CreateInstance() => new MyObjectBuilder_AnimationTree();
    }
  }
}
