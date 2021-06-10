// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.Reputation.MyObjectBuilder_ReputationSettingsDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Definitions.Reputation
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ReputationSettingsDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    public MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings DamageSettings;
    [ProtoMember(3)]
    public MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings PirateDamageSettings;
    [ProtoMember(5)]
    public int MaxReputationGainInTime;
    [ProtoMember(7)]
    public int ResetTimeMinForRepGain;

    [ProtoContract]
    public struct MyReputationDamageSettings
    {
      [ProtoMember(1)]
      public int GrindingWelding;
      [ProtoMember(3)]
      public int Damaging;
      [ProtoMember(5)]
      public int Stealing;
      [ProtoMember(7)]
      public int Killing;

      protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EMyReputationDamageSettings\u003C\u003EGrindingWelding\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings owner,
          in int value)
        {
          owner.GrindingWelding = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings owner,
          out int value)
        {
          value = owner.GrindingWelding;
        }
      }

      protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EMyReputationDamageSettings\u003C\u003EDamaging\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings owner,
          in int value)
        {
          owner.Damaging = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings owner,
          out int value)
        {
          value = owner.Damaging;
        }
      }

      protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EMyReputationDamageSettings\u003C\u003EStealing\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings owner,
          in int value)
        {
          owner.Stealing = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings owner,
          out int value)
        {
          value = owner.Stealing;
        }
      }

      protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EMyReputationDamageSettings\u003C\u003EKilling\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings owner,
          in int value)
        {
          owner.Killing = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings owner,
          out int value)
        {
          value = owner.Killing;
        }
      }

      private class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EMyReputationDamageSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings();

        MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings IActivator<MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings>.CreateInstance() => new MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings();
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EDamageSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings value)
      {
        owner.DamageSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings value)
      {
        value = owner.DamageSettings;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EPirateDamageSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings value)
      {
        owner.PirateDamageSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out MyObjectBuilder_ReputationSettingsDefinition.MyReputationDamageSettings value)
      {
        value = owner.PirateDamageSettings;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EMaxReputationGainInTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in int value)
      {
        owner.MaxReputationGainInTime = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out int value)
      {
        value = owner.MaxReputationGainInTime;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EResetTimeMinForRepGain\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in int value)
      {
        owner.ResetTimeMinForRepGain = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out int value)
      {
        value = owner.ResetTimeMinForRepGain;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ReputationSettingsDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ReputationSettingsDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_Reputation_MyObjectBuilder_ReputationSettingsDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ReputationSettingsDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ReputationSettingsDefinition();

      MyObjectBuilder_ReputationSettingsDefinition IActivator<MyObjectBuilder_ReputationSettingsDefinition>.CreateInstance() => new MyObjectBuilder_ReputationSettingsDefinition();
    }
  }
}
