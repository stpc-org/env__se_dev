// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("Transform")]
  public class MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform : MyObjectBuilder_WorldGeneratorPlayerStartingState
  {
    [ProtoMember(106)]
    public MyPositionAndOrientation? Transform;
    [ProtoMember(109)]
    [XmlAttribute]
    public bool JetpackEnabled;
    [ProtoMember(112)]
    [XmlAttribute]
    public bool DampenersEnabled;

    public bool ShouldSerializeTransform() => this.Transform.HasValue;

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003ETransform\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        in MyPositionAndOrientation? value)
      {
        owner.Transform = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        out MyPositionAndOrientation? value)
      {
        value = owner.Transform;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003EJetpackEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        in bool value)
      {
        owner.JetpackEnabled = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        out bool value)
      {
        value = owner.JetpackEnabled;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003EDampenersEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        in bool value)
      {
        owner.DampenersEnabled = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        out bool value)
      {
        value = owner.DampenersEnabled;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorPlayerStartingState.VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorPlayerStartingState&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorPlayerStartingState&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform();

      MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform>.CreateInstance() => new MyObjectBuilder_WorldGeneratorPlayerStartingState_Transform();
    }
  }
}
