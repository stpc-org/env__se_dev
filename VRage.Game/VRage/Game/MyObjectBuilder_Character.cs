// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Character
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Character : MyObjectBuilder_EntityBase
  {
    public static Dictionary<string, SerializableVector3> CharacterModels = new Dictionary<string, SerializableVector3>()
    {
      {
        "Soldier",
        new SerializableVector3(0.0f, 0.0f, 0.05f)
      },
      {
        "Astronaut",
        new SerializableVector3(0.0f, -1f, 0.0f)
      },
      {
        "Astronaut_Black",
        new SerializableVector3(0.0f, -0.96f, -0.5f)
      },
      {
        "Astronaut_Blue",
        new SerializableVector3(0.575f, 0.15f, 0.2f)
      },
      {
        "Astronaut_Green",
        new SerializableVector3(0.333f, -0.33f, -0.05f)
      },
      {
        "Astronaut_Red",
        new SerializableVector3(0.0f, 0.0f, 0.05f)
      },
      {
        "Astronaut_White",
        new SerializableVector3(0.0f, -0.8f, 0.6f)
      },
      {
        "Astronaut_Yellow",
        new SerializableVector3(0.122f, 0.05f, 0.46f)
      },
      {
        "Engineer_suit_no_helmet",
        new SerializableVector3(-100f, -100f, -100f)
      }
    };
    [ProtoMember(7)]
    public string CharacterModel;
    [ProtoMember(10)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_Inventory Inventory;
    [ProtoMember(13)]
    [XmlElement("HandWeapon", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_EntityBase>))]
    [Nullable]
    [DynamicObjectBuilder(false)]
    public MyObjectBuilder_EntityBase HandWeapon;
    [ProtoMember(16)]
    public MyObjectBuilder_Battery Battery;
    [ProtoMember(19)]
    public bool LightEnabled;
    [ProtoMember(22)]
    [DefaultValue(true)]
    public bool DampenersEnabled = true;
    [ProtoMember(25)]
    [DefaultValue(1f)]
    public float CharacterGeneralDamageModifier = 1f;
    [ProtoMember(28)]
    public long? UsingLadder;
    [ProtoMember(31)]
    public SerializableVector2 HeadAngle;
    [ProtoMember(34)]
    public SerializableVector3 LinearVelocity;
    [ProtoMember(37)]
    public float AutoenableJetpackDelay;
    [ProtoMember(40)]
    public bool JetpackEnabled;
    [ProtoMember(43)]
    [NoSerialize]
    public float? Health;
    [ProtoMember(46)]
    [DefaultValue(false)]
    public bool AIMode;
    [ProtoMember(49)]
    public SerializableVector3 ColorMaskHSV;
    [ProtoMember(52)]
    public float LootingCounter;
    [ProtoMember(55)]
    public string DisplayName;
    [ProtoMember(58)]
    public bool IsInFirstPersonView = true;
    [ProtoMember(61)]
    public bool EnableBroadcasting = true;
    [ProtoMember(64)]
    public float OxygenLevel = 1f;
    [ProtoMember(67)]
    public float EnvironmentOxygenLevel = 1f;
    [ProtoMember(70)]
    [Nullable]
    public List<MyObjectBuilder_Character.StoredGas> StoredGases;
    [ProtoMember(73)]
    public MyCharacterMovementEnum MovementState;
    [ProtoMember(76)]
    [Nullable]
    public List<string> EnabledComponents;
    [ProtoMember(85)]
    public bool NeedsOxygenFromSuit;
    [ProtoMember(88, IsRequired = false)]
    public long? OwningPlayerIdentityId;
    [ProtoMember(91, IsRequired = false)]
    public bool IsPersistenceCharacter;
    [ProtoMember(92, IsRequired = false)]
    public bool IsStartingCharacterForLobby;
    [ProtoMember(94)]
    public long RelativeDampeningEntity;
    [ProtoMember(195)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<MyObjectBuilder_Character.BuildPlanItem> BuildPlanner;
    [ProtoMember(210)]
    public MyObjectBuilder_Character.LadderInfo? UsingLadderInfo;
    [ProtoMember(212)]
    public bool EnableBroadcastingPlayerToggle = true;

    public bool ShouldSerializeHealth() => false;

    public bool ShouldSerializeMovementState() => (uint) this.MovementState > 0U;

    public MyObjectBuilder_Character()
    {
      this.StoredGases = new List<MyObjectBuilder_Character.StoredGas>();
      this.EnabledComponents = new List<string>();
    }

    [ProtoContract]
    public struct StoredGas
    {
      [ProtoMember(1)]
      public SerializableDefinitionId Id;
      [ProtoMember(4)]
      public float FillLevel;

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EStoredGas\u003C\u003EId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.StoredGas, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Character.StoredGas owner,
          in SerializableDefinitionId value)
        {
          owner.Id = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Character.StoredGas owner,
          out SerializableDefinitionId value)
        {
          value = owner.Id;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EStoredGas\u003C\u003EFillLevel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.StoredGas, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Character.StoredGas owner, in float value) => owner.FillLevel = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Character.StoredGas owner, out float value) => value = owner.FillLevel;
      }

      private class VRage_Game_MyObjectBuilder_Character\u003C\u003EStoredGas\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Character.StoredGas>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Character.StoredGas();

        MyObjectBuilder_Character.StoredGas IActivator<MyObjectBuilder_Character.StoredGas>.CreateInstance() => new MyObjectBuilder_Character.StoredGas();
      }
    }

    [ProtoContract]
    public struct ComponentItem
    {
      [ProtoMember(197)]
      public SerializableDefinitionId ComponentId;
      [ProtoMember(199)]
      public int Count;

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EComponentItem\u003C\u003EComponentId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.ComponentItem, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Character.ComponentItem owner,
          in SerializableDefinitionId value)
        {
          owner.ComponentId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Character.ComponentItem owner,
          out SerializableDefinitionId value)
        {
          value = owner.ComponentId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EComponentItem\u003C\u003ECount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.ComponentItem, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Character.ComponentItem owner, in int value) => owner.Count = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Character.ComponentItem owner, out int value) => value = owner.Count;
      }

      private class VRage_Game_MyObjectBuilder_Character\u003C\u003EComponentItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Character.ComponentItem>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Character.ComponentItem();

        MyObjectBuilder_Character.ComponentItem IActivator<MyObjectBuilder_Character.ComponentItem>.CreateInstance() => new MyObjectBuilder_Character.ComponentItem();
      }
    }

    [ProtoContract]
    public struct BuildPlanItem
    {
      [ProtoMember(185)]
      public SerializableDefinitionId BlockId;
      [ProtoMember(188)]
      public bool IsInProgress;
      [ProtoMember(190)]
      public List<MyObjectBuilder_Character.ComponentItem> Components;

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EBuildPlanItem\u003C\u003EBlockId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.BuildPlanItem, SerializableDefinitionId>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Character.BuildPlanItem owner,
          in SerializableDefinitionId value)
        {
          owner.BlockId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Character.BuildPlanItem owner,
          out SerializableDefinitionId value)
        {
          value = owner.BlockId;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EBuildPlanItem\u003C\u003EIsInProgress\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.BuildPlanItem, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Character.BuildPlanItem owner, in bool value) => owner.IsInProgress = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Character.BuildPlanItem owner, out bool value) => value = owner.IsInProgress;
      }

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EBuildPlanItem\u003C\u003EComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.BuildPlanItem, List<MyObjectBuilder_Character.ComponentItem>>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Character.BuildPlanItem owner,
          in List<MyObjectBuilder_Character.ComponentItem> value)
        {
          owner.Components = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Character.BuildPlanItem owner,
          out List<MyObjectBuilder_Character.ComponentItem> value)
        {
          value = owner.Components;
        }
      }

      private class VRage_Game_MyObjectBuilder_Character\u003C\u003EBuildPlanItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Character.BuildPlanItem>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Character.BuildPlanItem();

        MyObjectBuilder_Character.BuildPlanItem IActivator<MyObjectBuilder_Character.BuildPlanItem>.CreateInstance() => new MyObjectBuilder_Character.BuildPlanItem();
      }
    }

    [ProtoContract]
    public struct LadderInfo
    {
      [ProtoMember(200)]
      public MyPositionAndOrientation BaseMatrix;
      [ProtoMember(205)]
      public SerializableVector3 IncrementToBase;

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ELadderInfo\u003C\u003EBaseMatrix\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.LadderInfo, MyPositionAndOrientation>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Character.LadderInfo owner,
          in MyPositionAndOrientation value)
        {
          owner.BaseMatrix = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Character.LadderInfo owner,
          out MyPositionAndOrientation value)
        {
          value = owner.BaseMatrix;
        }
      }

      protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ELadderInfo\u003C\u003EIncrementToBase\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character.LadderInfo, SerializableVector3>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Character.LadderInfo owner,
          in SerializableVector3 value)
        {
          owner.IncrementToBase = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Character.LadderInfo owner,
          out SerializableVector3 value)
        {
          value = owner.IncrementToBase;
        }
      }

      private class VRage_Game_MyObjectBuilder_Character\u003C\u003ELadderInfo\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Character.LadderInfo>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Character.LadderInfo();

        MyObjectBuilder_Character.LadderInfo IActivator<MyObjectBuilder_Character.LadderInfo>.CreateInstance() => new MyObjectBuilder_Character.LadderInfo();
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ECharacterModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in string value) => owner.CharacterModel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out string value) => value = owner.CharacterModel;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EInventory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_Inventory>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in MyObjectBuilder_Inventory value)
      {
        owner.Inventory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyObjectBuilder_Inventory value)
      {
        value = owner.Inventory;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EHandWeapon\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_EntityBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in MyObjectBuilder_EntityBase value)
      {
        owner.HandWeapon = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyObjectBuilder_EntityBase value)
      {
        value = owner.HandWeapon;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EBattery\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_Battery>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in MyObjectBuilder_Battery value) => owner.Battery = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyObjectBuilder_Battery value)
      {
        value = owner.Battery;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ELightEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.LightEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.LightEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EDampenersEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.DampenersEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.DampenersEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ECharacterGeneralDamageModifier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in float value) => owner.CharacterGeneralDamageModifier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out float value) => value = owner.CharacterGeneralDamageModifier;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EUsingLadder\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in long? value) => owner.UsingLadder = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out long? value) => value = owner.UsingLadder;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EHeadAngle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, SerializableVector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in SerializableVector2 value) => owner.HeadAngle = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out SerializableVector2 value) => value = owner.HeadAngle;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ELinearVelocity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in SerializableVector3 value) => owner.LinearVelocity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out SerializableVector3 value) => value = owner.LinearVelocity;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EAutoenableJetpackDelay\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in float value) => owner.AutoenableJetpackDelay = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out float value) => value = owner.AutoenableJetpackDelay;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EJetpackEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.JetpackEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.JetpackEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EHealth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in float? value) => owner.Health = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out float? value) => value = owner.Health;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EAIMode\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.AIMode = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.AIMode;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EColorMaskHSV\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in SerializableVector3 value) => owner.ColorMaskHSV = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out SerializableVector3 value) => value = owner.ColorMaskHSV;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ELootingCounter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in float value) => owner.LootingCounter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out float value) => value = owner.LootingCounter;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EDisplayName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in string value) => owner.DisplayName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out string value) => value = owner.DisplayName;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EIsInFirstPersonView\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.IsInFirstPersonView = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.IsInFirstPersonView;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EEnableBroadcasting\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.EnableBroadcasting = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.EnableBroadcasting;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EOxygenLevel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in float value) => owner.OxygenLevel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out float value) => value = owner.OxygenLevel;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EEnvironmentOxygenLevel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in float value) => owner.EnvironmentOxygenLevel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out float value) => value = owner.EnvironmentOxygenLevel;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EStoredGases\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, List<MyObjectBuilder_Character.StoredGas>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in List<MyObjectBuilder_Character.StoredGas> value)
      {
        owner.StoredGases = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out List<MyObjectBuilder_Character.StoredGas> value)
      {
        value = owner.StoredGases;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EMovementState\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyCharacterMovementEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in MyCharacterMovementEnum value) => owner.MovementState = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyCharacterMovementEnum value)
      {
        value = owner.MovementState;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EEnabledComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in List<string> value) => owner.EnabledComponents = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out List<string> value) => value = owner.EnabledComponents;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ENeedsOxygenFromSuit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.NeedsOxygenFromSuit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.NeedsOxygenFromSuit;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EOwningPlayerIdentityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, long?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in long? value) => owner.OwningPlayerIdentityId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out long? value) => value = owner.OwningPlayerIdentityId;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EIsPersistenceCharacter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.IsPersistenceCharacter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.IsPersistenceCharacter;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EIsStartingCharacterForLobby\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.IsStartingCharacterForLobby = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.IsStartingCharacterForLobby;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ERelativeDampeningEntity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in long value) => owner.RelativeDampeningEntity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out long value) => value = owner.RelativeDampeningEntity;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EBuildPlanner\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, List<MyObjectBuilder_Character.BuildPlanItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in List<MyObjectBuilder_Character.BuildPlanItem> value)
      {
        owner.BuildPlanner = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out List<MyObjectBuilder_Character.BuildPlanItem> value)
      {
        value = owner.BuildPlanner;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EUsingLadderInfo\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_Character.LadderInfo?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in MyObjectBuilder_Character.LadderInfo? value)
      {
        owner.UsingLadderInfo = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyObjectBuilder_Character.LadderInfo? value)
      {
        value = owner.UsingLadderInfo;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EEnableBroadcastingPlayerToggle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Character, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in bool value) => owner.EnableBroadcastingPlayerToggle = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out bool value) => value = owner.EnableBroadcastingPlayerToggle;
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EEntityId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in long value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out long value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EPersistentFlags\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPersistentFlags\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyPersistentEntityFlags2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyPersistentEntityFlags2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in string value) => this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out string value) => this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003ELocalPositionAndOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyPositionAndOrientation?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyPositionAndOrientation? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EComponentContainer\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EComponentContainer\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyObjectBuilder_ComponentContainer>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out MyObjectBuilder_ComponentContainer value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003EEntityDefinitionId\u003C\u003EAccessor : MyObjectBuilder_EntityBase.VRage_ObjectBuilders_MyObjectBuilder_EntityBase\u003C\u003EEntityDefinitionId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Character owner,
        in SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_EntityBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Character owner,
        out SerializableDefinitionId? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_EntityBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Character\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Character, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Character owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Character owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Character\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Character>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Character();

      MyObjectBuilder_Character IActivator<MyObjectBuilder_Character>.CreateInstance() => new MyObjectBuilder_Character();
    }
  }
}
