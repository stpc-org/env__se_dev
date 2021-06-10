// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationTreeNodeAdd
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
  public class MyObjectBuilder_AnimationTreeNodeAdd : MyObjectBuilder_AnimationTreeNode
  {
    [ProtoMember(52)]
    public string ParameterName;
    [ProtoMember(55)]
    public MyParameterAnimTreeNodeMapping BaseNode;
    [ProtoMember(58)]
    public MyParameterAnimTreeNodeMapping AddNode;

    protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(
      HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes,
      MyObjectBuilder_AnimationTreeNode parentNode,
      List<MyObjectBuilder_AnimationTreeNode> orphans)
    {
      bool flag = selectedNodes == null || selectedNodes.Contains((MyObjectBuilder_AnimationTreeNode) this);
      MyObjectBuilder_AnimationTreeNodeAdd animationTreeNodeAdd = new MyObjectBuilder_AnimationTreeNodeAdd();
      animationTreeNodeAdd.EdPos = this.EdPos;
      animationTreeNodeAdd.ParameterName = this.ParameterName;
      animationTreeNodeAdd.BaseNode.Param = this.BaseNode.Param;
      animationTreeNodeAdd.BaseNode.Node = (MyObjectBuilder_AnimationTreeNode) null;
      animationTreeNodeAdd.AddNode.Param = this.AddNode.Param;
      animationTreeNodeAdd.AddNode.Node = (MyObjectBuilder_AnimationTreeNode) null;
      animationTreeNodeAdd.BaseNode.Node = this.BaseNode.Node.DeepCopyWithMask(selectedNodes, flag ? (MyObjectBuilder_AnimationTreeNode) animationTreeNodeAdd : (MyObjectBuilder_AnimationTreeNode) null, orphans);
      animationTreeNodeAdd.AddNode.Node = this.AddNode.Node.DeepCopyWithMask(selectedNodes, flag ? (MyObjectBuilder_AnimationTreeNode) animationTreeNodeAdd : (MyObjectBuilder_AnimationTreeNode) null, orphans);
      if (!flag)
        return (MyObjectBuilder_AnimationTreeNode) null;
      if (parentNode == null)
        orphans.Add((MyObjectBuilder_AnimationTreeNode) animationTreeNodeAdd);
      return (MyObjectBuilder_AnimationTreeNode) animationTreeNodeAdd;
    }

    public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
    {
      List<MyObjectBuilder_AnimationTreeNode> animationTreeNodeList = new List<MyObjectBuilder_AnimationTreeNode>();
      if (this.BaseNode.Node != null)
        animationTreeNodeList.Add(this.BaseNode.Node);
      if (this.AddNode.Node != null)
        animationTreeNodeList.Add(this.AddNode.Node);
      return animationTreeNodeList.Count > 0 ? animationTreeNodeList.ToArray() : (MyObjectBuilder_AnimationTreeNode[]) null;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003EParameterName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in string value) => owner.ParameterName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out string value) => value = owner.ParameterName;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003EBaseNode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, MyParameterAnimTreeNodeMapping>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeAdd owner,
        in MyParameterAnimTreeNodeMapping value)
      {
        owner.BaseNode = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeAdd owner,
        out MyParameterAnimTreeNodeMapping value)
      {
        value = owner.BaseNode;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003EAddNode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, MyParameterAnimTreeNodeMapping>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeAdd owner,
        in MyParameterAnimTreeNodeMapping value)
      {
        owner.AddNode = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeAdd owner,
        out MyParameterAnimTreeNodeMapping value)
      {
        value = owner.AddNode;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003EEdPos\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEdPos\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, Vector2I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in Vector2I? value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out Vector2I? value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003EEventNames\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventNames\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in List<string> value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeAdd owner,
        out List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003EEventTimes\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, List<double>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in List<double> value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeAdd owner,
        out List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003EKey\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EKey\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in string value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out string value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeAdd owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeAdd owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeAdd, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeAdd owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeAdd owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeAdd\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeAdd>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationTreeNodeAdd();

      MyObjectBuilder_AnimationTreeNodeAdd IActivator<MyObjectBuilder_AnimationTreeNodeAdd>.CreateInstance() => new MyObjectBuilder_AnimationTreeNodeAdd();
    }
  }
}
