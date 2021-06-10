// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationSMNode
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.Animation;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AnimationSMNode : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public string Name;
    [ProtoMember(4)]
    public string StateMachineName;
    [ProtoMember(7)]
    public MyObjectBuilder_AnimationTree AnimationTree;
    [ProtoMember(10)]
    public Vector2I? EdPos;
    [ProtoMember(13)]
    public MyObjectBuilder_AnimationSMNode.MySMNodeType Type;
    [ProtoMember(16)]
    [XmlArrayItem("Variable")]
    public List<MyObjectBuilder_AnimationSMVariable> Variables = new List<MyObjectBuilder_AnimationSMVariable>();

    public enum MySMNodeType
    {
      Normal,
      PassThrough,
      Any,
      AnyExceptTarget,
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMNode owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMNode owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003EStateMachineName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMNode owner, in string value) => owner.StateMachineName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMNode owner, out string value) => value = owner.StateMachineName;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003EAnimationTree\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, MyObjectBuilder_AnimationTree>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMNode owner,
        in MyObjectBuilder_AnimationTree value)
      {
        owner.AnimationTree = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMNode owner,
        out MyObjectBuilder_AnimationTree value)
      {
        value = owner.AnimationTree;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003EEdPos\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, Vector2I?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMNode owner, in Vector2I? value) => owner.EdPos = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMNode owner, out Vector2I? value) => value = owner.EdPos;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003EType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, MyObjectBuilder_AnimationSMNode.MySMNodeType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMNode owner,
        in MyObjectBuilder_AnimationSMNode.MySMNodeType value)
      {
        owner.Type = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMNode owner,
        out MyObjectBuilder_AnimationSMNode.MySMNodeType value)
      {
        value = owner.Type;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003EVariables\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMNode, List<MyObjectBuilder_AnimationSMVariable>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMNode owner,
        in List<MyObjectBuilder_AnimationSMVariable> value)
      {
        owner.Variables = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMNode owner,
        out List<MyObjectBuilder_AnimationSMVariable> value)
      {
        value = owner.Variables;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMNode, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMNode owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMNode owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMNode, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMNode owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMNode owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMNode\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMNode>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationSMNode();

      MyObjectBuilder_AnimationSMNode IActivator<MyObjectBuilder_AnimationSMNode>.CreateInstance() => new MyObjectBuilder_AnimationSMNode();
    }
  }
}
