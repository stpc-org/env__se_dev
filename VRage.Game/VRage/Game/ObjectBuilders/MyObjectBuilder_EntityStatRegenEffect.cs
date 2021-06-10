// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_EntityStatRegenEffect
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
  public class MyObjectBuilder_EntityStatRegenEffect : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public float TickAmount;
    [ProtoMember(4)]
    public float Interval = 1f;
    [ProtoMember(7)]
    public float MaxRegenRatio = 1f;
    [ProtoMember(10)]
    public float MinRegenRatio;
    [ProtoMember(13)]
    public float AliveTime;
    [ProtoMember(16)]
    public float Duration = -1f;
    [ProtoMember(18)]
    public bool RemoveWhenReachedMaxRegenRatio;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003ETickAmount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value) => owner.TickAmount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value) => value = owner.TickAmount;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003EInterval\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value) => owner.Interval = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value) => value = owner.Interval;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003EMaxRegenRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value) => owner.MaxRegenRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value) => value = owner.MaxRegenRatio;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003EMinRegenRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value) => owner.MinRegenRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value) => value = owner.MinRegenRatio;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003EAliveTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value) => owner.AliveTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value) => value = owner.AliveTime;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003EDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in float value) => owner.Duration = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out float value) => value = owner.Duration;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003ERemoveWhenReachedMaxRegenRatio\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in bool value) => owner.RemoveWhenReachedMaxRegenRatio = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out bool value) => value = owner.RemoveWhenReachedMaxRegenRatio;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStatRegenEffect owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStatRegenEffect owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStatRegenEffect owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStatRegenEffect owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStatRegenEffect, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStatRegenEffect owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStatRegenEffect owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStatRegenEffect\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EntityStatRegenEffect>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EntityStatRegenEffect();

      MyObjectBuilder_EntityStatRegenEffect IActivator<MyObjectBuilder_EntityStatRegenEffect>.CreateInstance() => new MyObjectBuilder_EntityStatRegenEffect();
    }
  }
}
