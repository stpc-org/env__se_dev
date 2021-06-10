// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationTreeNodeSetter
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
  public class MyObjectBuilder_AnimationTreeNodeSetter : MyObjectBuilder_AnimationTreeNode
  {
    [ProtoMember(76)]
    [XmlElement(typeof (MyAbstractXmlSerializer<MyObjectBuilder_AnimationTreeNode>))]
    public MyObjectBuilder_AnimationTreeNode Child;
    [ProtoMember(79)]
    public float Time;
    [ProtoMember(82)]
    public MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment Value;
    [ProtoMember(85)]
    public bool ResetValueEnabled;
    [ProtoMember(88)]
    public float ResetValue;

    protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(
      HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes,
      MyObjectBuilder_AnimationTreeNode parentNode,
      List<MyObjectBuilder_AnimationTreeNode> orphans)
    {
      bool flag = selectedNodes == null || selectedNodes.Contains((MyObjectBuilder_AnimationTreeNode) this);
      MyObjectBuilder_AnimationTreeNodeSetter animationTreeNodeSetter = new MyObjectBuilder_AnimationTreeNodeSetter();
      animationTreeNodeSetter.Value = this.Value;
      animationTreeNodeSetter.ResetValue = this.ResetValue;
      animationTreeNodeSetter.Time = this.Time;
      animationTreeNodeSetter.EdPos = this.EdPos;
      animationTreeNodeSetter.ResetValueEnabled = this.ResetValueEnabled;
      if (this.Child != null)
        animationTreeNodeSetter.Child = this.Child.DeepCopyWithMask(selectedNodes, flag ? (MyObjectBuilder_AnimationTreeNode) animationTreeNodeSetter : (MyObjectBuilder_AnimationTreeNode) null, orphans);
      if (!flag)
        return (MyObjectBuilder_AnimationTreeNode) null;
      if (parentNode == null)
        orphans.Add((MyObjectBuilder_AnimationTreeNode) animationTreeNodeSetter);
      return (MyObjectBuilder_AnimationTreeNode) animationTreeNodeSetter;
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

    [ProtoContract]
    public struct ValueAssignment
    {
      [ProtoMember(70)]
      public string Name;
      [ProtoMember(73)]
      public float Value;

      protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EValueAssignment\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment owner,
          in string value)
        {
          owner.Name = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment owner,
          out string value)
        {
          value = owner.Name;
        }
      }

      protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EValueAssignment\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment owner,
          in float value)
        {
          owner.Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment owner,
          out float value)
        {
          value = owner.Value;
        }
      }

      private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EValueAssignment\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment();

        MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment IActivator<MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment>.CreateInstance() => new MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment();
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EChild\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, MyObjectBuilder_AnimationTreeNode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        in MyObjectBuilder_AnimationTreeNode value)
      {
        owner.Child = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        out MyObjectBuilder_AnimationTreeNode value)
      {
        value = owner.Child;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003ETime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in float value) => owner.Time = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out float value) => value = owner.Time;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        in MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment value)
      {
        owner.Value = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        out MyObjectBuilder_AnimationTreeNodeSetter.ValueAssignment value)
      {
        value = owner.Value;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EResetValueEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in bool value) => owner.ResetValueEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out bool value) => value = owner.ResetValueEnabled;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EResetValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in float value) => owner.ResetValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out float value) => value = owner.ResetValue;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EEdPos\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEdPos\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, Vector2I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in Vector2I? value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        out Vector2I? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EEventNames\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventNames\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        in List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        out List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EEventTimes\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, List<double>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        in List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        out List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EKey\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EKey\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in string value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out string value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeSetter owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeSetter, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeSetter owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeSetter owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeSetter\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeSetter>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationTreeNodeSetter();

      MyObjectBuilder_AnimationTreeNodeSetter IActivator<MyObjectBuilder_AnimationTreeNodeSetter>.CreateInstance() => new MyObjectBuilder_AnimationTreeNodeSetter();
    }
  }
}
