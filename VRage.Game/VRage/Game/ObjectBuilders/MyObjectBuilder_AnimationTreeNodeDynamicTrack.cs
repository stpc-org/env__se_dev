// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationTreeNodeDynamicTrack
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
  public class MyObjectBuilder_AnimationTreeNodeDynamicTrack : MyObjectBuilder_AnimationTreeNodeTrack
  {
    [ProtoMember(10)]
    public string DefaultAnimation;

    protected internal override MyObjectBuilder_AnimationTreeNode DeepCopyWithMask(
      HashSet<MyObjectBuilder_AnimationTreeNode> selectedNodes,
      MyObjectBuilder_AnimationTreeNode parentNode,
      List<MyObjectBuilder_AnimationTreeNode> orphans)
    {
      int num = selectedNodes == null ? 1 : (selectedNodes.Contains((MyObjectBuilder_AnimationTreeNode) this) ? 1 : 0);
      MyObjectBuilder_AnimationTreeNodeDynamicTrack nodeDynamicTrack1 = new MyObjectBuilder_AnimationTreeNodeDynamicTrack();
      nodeDynamicTrack1.DefaultAnimation = this.DefaultAnimation;
      nodeDynamicTrack1.PathToModel = this.PathToModel;
      nodeDynamicTrack1.AnimationName = this.AnimationName;
      nodeDynamicTrack1.Loop = this.Loop;
      nodeDynamicTrack1.Speed = this.Speed;
      nodeDynamicTrack1.Interpolate = this.Interpolate;
      nodeDynamicTrack1.SynchronizeWithLayer = this.SynchronizeWithLayer;
      nodeDynamicTrack1.EdPos = this.EdPos;
      MyObjectBuilder_AnimationTreeNodeDynamicTrack nodeDynamicTrack2 = nodeDynamicTrack1;
      if (num == 0)
        return (MyObjectBuilder_AnimationTreeNode) null;
      if (parentNode == null)
        orphans.Add((MyObjectBuilder_AnimationTreeNode) nodeDynamicTrack2);
      return (MyObjectBuilder_AnimationTreeNode) nodeDynamicTrack2;
    }

    public override MyObjectBuilder_AnimationTreeNode[] GetChildren() => (MyObjectBuilder_AnimationTreeNode[]) null;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EDefaultAnimation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in string value)
      {
        owner.DefaultAnimation = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out string value)
      {
        value = owner.DefaultAnimation;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EPathToModel\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNodeTrack.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EPathToModel\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EAnimationName\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNodeTrack.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EAnimationName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003ELoop\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNodeTrack.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003ELoop\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003ESpeed\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNodeTrack.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003ESpeed\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in double value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out double value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EInterpolate\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNodeTrack.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003EInterpolate\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003ESynchronizeWithLayer\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNodeTrack.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeTrack\u003C\u003ESynchronizeWithLayer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNodeTrack&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EEdPos\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEdPos\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, Vector2I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in Vector2I? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out Vector2I? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EEventNames\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventNames\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EEventTimes\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EEventTimes\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, List<double>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out List<double> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EKey\u003C\u003EAccessor : MyObjectBuilder_AnimationTreeNode.VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNode\u003C\u003EKey\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_AnimationTreeNode&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_AnimationTreeNode&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationTreeNodeDynamicTrack, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationTreeNodeDynamicTrack owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationTreeNodeDynamicTrack\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationTreeNodeDynamicTrack>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationTreeNodeDynamicTrack();

      MyObjectBuilder_AnimationTreeNodeDynamicTrack IActivator<MyObjectBuilder_AnimationTreeNodeDynamicTrack>.CreateInstance() => new MyObjectBuilder_AnimationTreeNodeDynamicTrack();
    }
  }
}
