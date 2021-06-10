// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_TriggerBlockDestroyed
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

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_TriggerBlockDestroyed : MyObjectBuilder_Trigger
  {
    [ProtoMember(1)]
    public List<long> BlockIds;
    [ProtoMember(4)]
    public string SingleMessage;

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003EBlockIds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, List<long>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in List<long> value) => owner.BlockIds = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out List<long> value) => value = owner.BlockIds;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003ESingleMessage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value) => owner.SingleMessage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value) => value = owner.SingleMessage;
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003EIsTrue\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EIsTrue\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in bool value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out bool value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003EMessage\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EMessage\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003EWwwLink\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003EWwwLink\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003ENextMission\u003C\u003EAccessor : MyObjectBuilder_Trigger.VRage_Game_MyObjectBuilder_Trigger\u003C\u003ENextMission\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value) => this.Set((MyObjectBuilder_Trigger&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value) => this.Get((MyObjectBuilder_Trigger&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TriggerBlockDestroyed owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TriggerBlockDestroyed owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TriggerBlockDestroyed owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TriggerBlockDestroyed owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TriggerBlockDestroyed, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TriggerBlockDestroyed owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TriggerBlockDestroyed owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_TriggerBlockDestroyed\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TriggerBlockDestroyed>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TriggerBlockDestroyed();

      MyObjectBuilder_TriggerBlockDestroyed IActivator<MyObjectBuilder_TriggerBlockDestroyed>.CreateInstance() => new MyObjectBuilder_TriggerBlockDestroyed();
    }
  }
}
