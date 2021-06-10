// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationLayer
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AnimationLayer : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public string Name;
    [ProtoMember(4)]
    public MyObjectBuilder_AnimationLayer.MyLayerMode Mode;
    [ProtoMember(7)]
    public string StateMachine;
    [ProtoMember(10)]
    public string InitialSMNode;
    [ProtoMember(13)]
    public string BoneMask;

    [ProtoContract]
    public enum MyLayerMode
    {
      Replace,
      Add,
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationLayer owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationLayer owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003EMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, MyObjectBuilder_AnimationLayer.MyLayerMode>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationLayer owner,
        in MyObjectBuilder_AnimationLayer.MyLayerMode value)
      {
        owner.Mode = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationLayer owner,
        out MyObjectBuilder_AnimationLayer.MyLayerMode value)
      {
        value = owner.Mode;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003EStateMachine\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationLayer owner, in string value) => owner.StateMachine = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationLayer owner, out string value) => value = owner.StateMachine;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003EInitialSMNode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationLayer owner, in string value) => owner.InitialSMNode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationLayer owner, out string value) => value = owner.InitialSMNode;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003EBoneMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationLayer owner, in string value) => owner.BoneMask = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationLayer owner, out string value) => value = owner.BoneMask;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationLayer, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationLayer owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationLayer owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationLayer owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationLayer owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationLayer, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationLayer owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationLayer owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationLayer, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationLayer owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationLayer owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationLayer\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationLayer>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationLayer();

      MyObjectBuilder_AnimationLayer IActivator<MyObjectBuilder_AnimationLayer>.CreateInstance() => new MyObjectBuilder_AnimationLayer();
    }
  }
}
