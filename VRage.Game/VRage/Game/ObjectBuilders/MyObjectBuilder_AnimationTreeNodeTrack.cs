// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationTreeNodeTrack
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
  public class MyObjectBuilder_AnimationTreeNodeTrack : MyObjectBuilder_AnimationTreeNode
  {
    [ProtoMember(13)]
    public string PathToModel;
    [ProtoMember(16)]
    public string AnimationName;
    [ProtoMember(19)]
    public bool Loop = true;
    [ProtoMember(22)]
    public double Speed = 1.0;
    [ProtoMember(25)]
    public bool Interpolate = true;
    [ProtoMember(28)]
    public string SynchronizeWithLayer;

    protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(
      HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes,
      MyObjectBuilder_AnimationTreeNode parentNode,
      List<MyObjectBuilder_AnimationTreeNode> orphans)
    {
      int num = selectedNodes == null ? 1 : (selectedNodes.Contains((MyObjectBuilder_AnimationTreeNode) this) ? 1 : 0);
      MyObjectBuilder_AnimationTreeNodeTrack animationTreeNodeTrack1 = new MyObjectBuilder_AnimationTreeNodeTrack();
      animationTreeNodeTrack1.PathToModel = this.PathToModel;
      animationTreeNodeTrack1.AnimationName = this.AnimationName;
      animationTreeNodeTrack1.Loop = this.Loop;
      animationTreeNodeTrack1.Speed = this.Speed;
      animationTreeNodeTrack1.Interpolate = this.Interpolate;
      animationTreeNodeTrack1.SynchronizeWithLayer = this.SynchronizeWithLayer;
      animationTreeNodeTrack1.EdPos = this.EdPos;
      MyObjectBuilder_AnimationTreeNodeTrack animationTreeNodeTrack2 = animationTreeNodeTrack1;
      if (num == 0)
        return (MyObjectBuilder_AnimationTreeNode) null;
      if (parentNode == null)
        orphans.Add((MyObjectBuilder_AnimationTreeNode) animationTreeNodeTrack2);
      return (MyObjectBuilder_AnimationTreeNode) animationTreeNodeTrack2;
    }

    public override MyObjectBuilder_AnimationTreeNode[] GetChildren() => (MyObjectBuilder_AnimationTreeNode[]) null;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EPathToModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value) => owner.PathToModel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value) => value = owner.PathToModel;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EAnimationName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value) => owner.AnimationName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value) => value = owner.AnimationName;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003ELoop\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in bool value) => owner.Loop = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out bool value) => value = owner.Loop;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003ESpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in double value) => owner.Speed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out double value) => value = owner.Speed;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EInterpolate\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in bool value) => owner.Interpolate = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out bool value) => value = owner.Interpolate;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003ESynchronizeWithLayer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value) => owner.SynchronizeWithLayer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value) => value = owner.SynchronizeWithLayer;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EEdPos\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEdPos\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, Vector2I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in Vector2I? value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out Vector2I? value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EEventNames\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventNames\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeTrack owner,
        in List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeTrack owner,
        out List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EEventTimes\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, List<double>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeTrack owner,
        in List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeTrack owner,
        out List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EKey\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EKey\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value) => this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value) => this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeTrack owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeTrack owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeTrack owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeTrack owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationTreeNodeTrack owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationTreeNodeTrack owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeTrack>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationTreeNodeTrack();

      MyObjectBuilder_AnimationTreeNodeTrack IActivator<MyObjectBuilder_AnimationTreeNodeTrack>.CreateInstance() => new MyObjectBuilder_AnimationTreeNodeTrack();
    }
  }
}
