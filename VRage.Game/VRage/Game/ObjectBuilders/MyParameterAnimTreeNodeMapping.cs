// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyParameterAnimTreeNodeMapping
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public struct MyParameterAnimTreeNodeMapping
  {
    [ProtoMember(31)]
    public float Param;
    [ProtoMember(34)]
    [XmlElement(typeof (MyAbstractXmlSerializer<MyObjectBuilder_AnimationTreeNode>))]
    public MyObjectBuilder_AnimationTreeNode Node;

    protected class VRage_Game_ObjectBuilders_MyParameterAnimTreeNodeMapping\u003C\u003EParam\u003C\u003EAccessor : IMemberAccessor<MyParameterAnimTreeNodeMapping, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyParameterAnimTreeNodeMapping owner, in float value) => owner.Param = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyParameterAnimTreeNodeMapping owner, out float value) => value = owner.Param;
    }

    protected class VRage_Game_ObjectBuilders_MyParameterAnimTreeNodeMapping\u003C\u003ENode\u003C\u003EAccessor : IMemberAccessor<MyParameterAnimTreeNodeMapping, MyObjectBuilder_AnimationTreeNode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyParameterAnimTreeNodeMapping owner,
        in MyObjectBuilder_AnimationTreeNode value)
      {
        owner.Node = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyParameterAnimTreeNodeMapping owner,
        out MyObjectBuilder_AnimationTreeNode value)
      {
        value = owner.Node;
      }
    }

    private class VRage_Game_ObjectBuilders_MyParameterAnimTreeNodeMapping\u003C\u003EActor : IActivator, IActivator<MyParameterAnimTreeNodeMapping>
    {
      object IActivator.CreateInstance() => (object) new MyParameterAnimTreeNodeMapping();

      MyParameterAnimTreeNodeMapping IActivator<MyParameterAnimTreeNodeMapping>.CreateInstance() => new MyParameterAnimTreeNodeMapping();
    }
  }
}
