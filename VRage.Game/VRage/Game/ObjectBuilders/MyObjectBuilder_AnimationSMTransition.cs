// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_AnimationSMTransition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageRender.Animations;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_AnimationSMTransition : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    [XmlAttribute]
    public string Name;
    [ProtoMember(4)]
    [XmlAttribute]
    public string From;
    [ProtoMember(7)]
    [XmlAttribute]
    public string To;
    [ProtoMember(10)]
    [XmlAttribute]
    public double TimeInSec;
    [ProtoMember(13)]
    [XmlAttribute]
    public MyAnimationTransitionSyncType Sync;
    [ProtoMember(16)]
    [XmlArrayItem("Conjunction")]
    public MyObjectBuilder_AnimationSMConditionsConjunction[] Conditions;
    [ProtoMember(19)]
    public int? Priority;
    [ProtoMember(22)]
    [XmlAttribute]
    public MyAnimationTransitionCurve Curve;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003EFrom\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value) => owner.From = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value) => value = owner.From;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003ETo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value) => owner.To = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value) => value = owner.To;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003ETimeInSec\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMTransition owner, in double value) => owner.TimeInSec = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMTransition owner, out double value) => value = owner.TimeInSec;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003ESync\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyAnimationTransitionSyncType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMTransition owner,
        in MyAnimationTransitionSyncType value)
      {
        owner.Sync = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMTransition owner,
        out MyAnimationTransitionSyncType value)
      {
        value = owner.Sync;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003EConditions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyObjectBuilder_AnimationSMConditionsConjunction[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMTransition owner,
        in MyObjectBuilder_AnimationSMConditionsConjunction[] value)
      {
        owner.Conditions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMTransition owner,
        out MyObjectBuilder_AnimationSMConditionsConjunction[] value)
      {
        value = owner.Conditions;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003EPriority\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMTransition owner, in int? value) => owner.Priority = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMTransition owner, out int? value) => value = owner.Priority;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003ECurve\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyAnimationTransitionCurve>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMTransition owner,
        in MyAnimationTransitionCurve value)
      {
        owner.Curve = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMTransition owner,
        out MyAnimationTransitionCurve value)
      {
        value = owner.Curve;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMTransition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMTransition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMTransition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_AnimationSMTransition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_AnimationSMTransition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_AnimationSMTransition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_AnimationSMTransition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_AnimationSMTransition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_AnimationSMTransition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_AnimationSMTransition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_AnimationSMTransition();

      MyObjectBuilder_AnimationSMTransition IActivator<MyObjectBuilder_AnimationSMTransition>.CreateInstance() => new MyObjectBuilder_AnimationSMTransition();
    }
  }
}
