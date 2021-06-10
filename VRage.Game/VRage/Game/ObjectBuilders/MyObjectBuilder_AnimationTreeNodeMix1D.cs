// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationTreeNodeMix1D
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
  public class MyObjectBuilder_AnimationTreeNodeMix1D : MyObjectBuilder_AnimationTreeNode
  {
    [ProtoMember(37)]
    public string ParameterName;
    [ProtoMember(40)]
    public bool Circular;
    [ProtoMember(43)]
    public float Sensitivity = 1f;
    [ProtoMember(46)]
    public float? MaxChange;
    [ProtoMember(49)]
    [XmlElement("Child")]
    public MyParameterAnimTreeNodeMapping[] Children;

    protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(
      HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes,
      MyObjectBuilder_AnimationTreeNode parentNode,
      List<MyObjectBuilder_AnimationTreeNode> orphans)
    {
      bool flag = selectedNodes == null || selectedNodes.Contains((MyObjectBuilder_AnimationTreeNode) this);
      MyObjectBuilder_AnimationTreeNodeMix1D animationTreeNodeMix1D = new MyObjectBuilder_AnimationTreeNodeMix1D();
      animationTreeNodeMix1D.ParameterName = this.ParameterName;
      animationTreeNodeMix1D.Circular = this.Circular;
      animationTreeNodeMix1D.Sensitivity = this.Sensitivity;
      animationTreeNodeMix1D.MaxChange = this.MaxChange;
      animationTreeNodeMix1D.Children = (MyParameterAnimTreeNodeMapping[]) null;
      animationTreeNodeMix1D.EdPos = this.EdPos;
      if (this.Children != null)
      {
        animationTreeNodeMix1D.Children = new MyParameterAnimTreeNodeMapping[this.Children.Length];
        for (int index = 0; index < this.Children.Length; ++index)
        {
          MyObjectBuilder_AnimationTreeNode animationTreeNode = (MyObjectBuilder_AnimationTreeNode) null;
          if (this.Children[index].Node != null)
            animationTreeNode = this.Children[index].Node.DeepCopyWithMask(selectedNodes, flag ? (MyObjectBuilder_AnimationTreeNode) animationTreeNodeMix1D : (MyObjectBuilder_AnimationTreeNode) null, orphans);
          animationTreeNodeMix1D.Children[index].Param = this.Children[index].Param;
          animationTreeNodeMix1D.Children[index].Node = animationTreeNode;
        }
      }
      if (!flag)
        return (MyObjectBuilder_AnimationTreeNode) null;
      if (parentNode == null)
        orphans.Add((MyObjectBuilder_AnimationTreeNode) animationTreeNodeMix1D);
      return (MyObjectBuilder_AnimationTreeNode) animationTreeNodeMix1D;
    }

    public override MyObjectBuilder_AnimationTreeNode[] GetChildren()
    {
      if (this.Children != null)
      {
        List<MyObjectBuilder_AnimationTreeNode> animationTreeNodeList = new List<MyObjectBuilder_AnimationTreeNode>();
        for (int index = 0; index < this.Children.Length; ++index)
        {
          if (this.Children[index].Node != null)
            animationTreeNodeList.Add(this.Children[index].Node);
        }
        if (animationTreeNodeList.Count > 0)
          return animationTreeNodeList.ToArray();
      }
      return (MyObjectBuilder_AnimationTreeNode[]) null;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003EParameterName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in string value) => owner.ParameterName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out string value) => value = owner.ParameterName;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003ECircular\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in bool value) => owner.Circular = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out bool value) => value = owner.Circular;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003ESensitivity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in float value) => owner.Sensitivity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out float value) => value = owner.Sensitivity;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003EMaxChange\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in float? value) => owner.MaxChange = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out float? value) => value = owner.MaxChange;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003EChildren\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, MyParameterAnimTreeNodeMapping[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        in MyParameterAnimTreeNodeMapping[] value)
      {
        owner.Children = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        out MyParameterAnimTreeNodeMapping[] value)
      {
        value = owner.Children;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003EEdPos\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEdPos\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, Vector2I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in Vector2I? value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out Vector2I? value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003EEventNames\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventNames\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        in List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        out List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003EEventTimes\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, List<double>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        in List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        out List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003EKey\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EKey\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in string value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out string value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeMix1D owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeMix1D, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeMix1D owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeMix1D\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeMix1D>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationTreeNodeMix1D();

      MyObjectBuilder_AnimationTreeNodeMix1D IActivator<MyObjectBuilder_AnimationTreeNodeMix1D>.CreateInstance() => new MyObjectBuilder_AnimationTreeNodeMix1D();
    }
  }
}
