// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip
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
  [XmlType("RespawnShip")]
  public class MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip : MyObjectBuilder_WorldGeneratorPlayerStartingState
  {
    [ProtoMember(115)]
    [XmlAttribute]
    public bool DampenersEnabled;
    [ProtoMember(118)]
    [XmlAttribute]
    public string RespawnShip;

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip\u003C\u003EDampenersEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        in bool value)
      {
        owner.DampenersEnabled = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        out bool value)
      {
        value = owner.DampenersEnabled;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip\u003C\u003ERespawnShip\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        in string value)
      {
        owner.RespawnShip = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        out string value)
      {
        value = owner.RespawnShip;
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip\u003C\u003EFactionTag\u003C\u003EAccessor : MyObjectBuilder_WorldGeneratorPlayerStartingState.VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState\u003C\u003EFactionTag\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_WorldGeneratorPlayerStartingState&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_WorldGeneratorPlayerStartingState&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip();

      MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip IActivator<MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip>.CreateInstance() => new MyObjectBuilder_WorldGeneratorPlayerStartingState_RespawnShip();
    }
  }
}
