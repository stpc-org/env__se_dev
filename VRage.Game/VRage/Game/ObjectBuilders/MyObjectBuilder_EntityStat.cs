// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_EntityStat
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_EntityStat : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public float Value = 1f;
    [ProtoMember(4)]
    public float MaxValue = 1f;
    [ProtoMember(7)]
    public float StatRegenAmountMultiplier = 1f;
    [ProtoMember(10)]
    public float StatRegenAmountMultiplierDuration;
    [ProtoMember(13)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_EntityStatRegenEffect[] Effects;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003EValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStat owner, in float value) => owner.Value = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStat owner, out float value) => value = owner.Value;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003EMaxValue\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStat owner, in float value) => owner.MaxValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStat owner, out float value) => value = owner.MaxValue;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003EStatRegenAmountMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStat owner, in float value) => owner.StatRegenAmountMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStat owner, out float value) => value = owner.StatRegenAmountMultiplier;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003EStatRegenAmountMultiplierDuration\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStat owner, in float value) => owner.StatRegenAmountMultiplierDuration = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStat owner, out float value) => value = owner.StatRegenAmountMultiplierDuration;
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003EEffects\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EntityStat, MyObjectBuilder_EntityStatRegenEffect[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EntityStat owner,
        in MyObjectBuilder_EntityStatRegenEffect[] value)
      {
        owner.Effects = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EntityStat owner,
        out MyObjectBuilder_EntityStatRegenEffect[] value)
      {
        value = owner.Effects;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStat, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStat owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStat owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStat, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStat owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStat owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStat, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStat owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStat owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EntityStat, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_EntityStat owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_EntityStat owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_EntityStat\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EntityStat>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EntityStat();

      MyObjectBuilder_EntityStat IActivator<MyObjectBuilder_EntityStat>.CreateInstance() => new MyObjectBuilder_EntityStat();
    }
  }
}
