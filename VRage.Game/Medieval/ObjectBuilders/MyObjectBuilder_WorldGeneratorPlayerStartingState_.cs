// Decompiled with JetBrains decompiler
// Type: Medieval.ObjectBuilders.MyObjectBuilder_WorldGeneratorPlayerStartingState_Range
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace Medieval.ObjectBuilders
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  [XmlType("Range")]
  public class MyObjectBuilder_WorldGeneratorPlayerStartingState_Range : MyObjectBuilder_WorldGeneratorPlayerStartingState
  {
    [ProtoMember(1)]
    public SerializableVector3 MinPosition;
    [ProtoMember(4)]
    public SerializableVector3 MaxPosition;

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range\u003C\u003EMinPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        in SerializableVector3 value)
      {
        owner.MinPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        out SerializableVector3 value)
      {
        value = owner.MinPosition;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range\u003C\u003EMaxPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        in SerializableVector3 value)
      {
        owner.MaxPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        out SerializableVector3 value)
      {
        value = owner.MaxPosition;
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorPlayerStartingState.VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorPlayerStartingState&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorPlayerStartingState&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Range owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class Medieval_ObjectBuilders_MyObjectBuilder_WorldGeneratorPlayerStartingState_Range\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorPlayerStartingState_Range();

      MyObjectBuilder_WorldGeneratorPlayerStartingState_Range IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_Range>.CreateInstance() => new MyObjectBuilder_WorldGeneratorPlayerStartingState_Range();
    }
  }
}
