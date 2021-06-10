// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_ScenarioDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlType("ScenarioDefinition")]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_ScenarioDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    public SerializableDefinitionId GameDefinition = (SerializableDefinitionId) MyGameDefinition.Default;
    [ProtoMember(4)]
    public SerializableDefinitionId EnvironmentDefinition = new SerializableDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_EnvironmentDefinition), "Default");
    [ProtoMember(7)]
    public MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings AsteroidClusters;
    [ProtoMember(10)]
    public MyEnvironmentHostilityEnum DefaultEnvironment = MyEnvironmentHostilityEnum.NORMAL;
    [ProtoMember(13)]
    [XmlArrayItem("StartingState", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_WorldGeneratorPlayerStartingState>))]
    public MyObjectBuilder_WorldGeneratorPlayerStartingState[] PossibleStartingStates;
    [ProtoMember(16)]
    [XmlArrayItem("Operation", Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_WorldGeneratorOperation>))]
    public MyObjectBuilder_WorldGeneratorOperation[] WorldGeneratorOperations;
    [ProtoMember(19)]
    [XmlArrayItem("Weapon")]
    public string[] CreativeModeWeapons;
    [ProtoMember(22)]
    [XmlArrayItem("Component")]
    public MyObjectBuilder_ScenarioDefinition.StartingItem[] CreativeModeComponents;
    [ProtoMember(25)]
    [XmlArrayItem("PhysicalItem")]
    public MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem[] CreativeModePhysicalItems;
    [ProtoMember(28)]
    [XmlArrayItem("AmmoItem")]
    public MyObjectBuilder_ScenarioDefinition.StartingItem[] CreativeModeAmmoItems;
    [ProtoMember(31)]
    [XmlArrayItem("Weapon")]
    public string[] SurvivalModeWeapons;
    [ProtoMember(34)]
    [XmlArrayItem("Component")]
    public MyObjectBuilder_ScenarioDefinition.StartingItem[] SurvivalModeComponents;
    [ProtoMember(37)]
    [XmlArrayItem("PhysicalItem")]
    public MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem[] SurvivalModePhysicalItems;
    [ProtoMember(40)]
    [XmlArrayItem("AmmoItem")]
    public MyObjectBuilder_ScenarioDefinition.StartingItem[] SurvivalModeAmmoItems;
    [ProtoMember(43)]
    public MyObjectBuilder_InventoryItem[] CreativeInventoryItems;
    [ProtoMember(46)]
    public MyObjectBuilder_InventoryItem[] SurvivalInventoryItems;
    [ProtoMember(49)]
    public SerializableBoundingBoxD? WorldBoundaries;
    private MyObjectBuilder_Toolbar m_creativeDefaultToolbar;
    [ProtoMember(58)]
    public MyObjectBuilder_Toolbar SurvivalDefaultToolbar;
    [ProtoMember(61)]
    public MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings Battle;
    [ProtoMember(64)]
    public string MainCharacterModel;
    [ProtoMember(67)]
    public long GameDate = 656385372000000000;
    [ProtoMember(70)]
    public SerializableVector3 SunDirection = (SerializableVector3) Vector3.Invalid;

    [ProtoMember(52)]
    public MyObjectBuilder_Toolbar DefaultToolbar
    {
      get => (MyObjectBuilder_Toolbar) null;
      set => this.CreativeDefaultToolbar = this.SurvivalDefaultToolbar = value;
    }

    public bool ShouldSerializeDefaultToolbar() => false;

    [ProtoMember(55)]
    public MyObjectBuilder_Toolbar CreativeDefaultToolbar
    {
      get => this.m_creativeDefaultToolbar;
      set => this.m_creativeDefaultToolbar = value;
    }

    [ProtoContract]
    public struct AsteroidClustersSettings
    {
      [ProtoMember(73)]
      [XmlAttribute]
      public bool Enabled;
      [ProtoMember(76)]
      [XmlAttribute]
      public float Offset;
      [ProtoMember(79)]
      [XmlAttribute]
      public bool CentralCluster;

      public bool ShouldSerializeOffset() => this.Enabled;

      public bool ShouldSerializeCentralCluster() => this.Enabled;

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EAsteroidClustersSettings\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings owner,
          in bool value)
        {
          owner.Enabled = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings owner,
          out bool value)
        {
          value = owner.Enabled;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EAsteroidClustersSettings\u003C\u003EOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings owner,
          in float value)
        {
          owner.Offset = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings owner,
          out float value)
        {
          value = owner.Offset;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EAsteroidClustersSettings\u003C\u003ECentralCluster\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings, bool>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings owner,
          in bool value)
        {
          owner.CentralCluster = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings owner,
          out bool value)
        {
          value = owner.CentralCluster;
        }
      }

      private class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EAsteroidClustersSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings();

        MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings IActivator<MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings>.CreateInstance() => new MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings();
      }
    }

    [ProtoContract]
    public struct StartingItem
    {
      [ProtoMember(82)]
      [XmlAttribute]
      public float amount;
      [ProtoMember(85)]
      [XmlText]
      public string itemName;

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EStartingItem\u003C\u003Eamount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.StartingItem, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.StartingItem owner,
          in float value)
        {
          owner.amount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.StartingItem owner,
          out float value)
        {
          value = owner.amount;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EStartingItem\u003C\u003EitemName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.StartingItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.StartingItem owner,
          in string value)
        {
          owner.itemName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.StartingItem owner,
          out string value)
        {
          value = owner.itemName;
        }
      }

      private class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EStartingItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScenarioDefinition.StartingItem>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScenarioDefinition.StartingItem();

        MyObjectBuilder_ScenarioDefinition.StartingItem IActivator<MyObjectBuilder_ScenarioDefinition.StartingItem>.CreateInstance() => new MyObjectBuilder_ScenarioDefinition.StartingItem();
      }
    }

    [ProtoContract]
    public struct StartingPhysicalItem
    {
      [ProtoMember(88)]
      [XmlAttribute]
      public float amount;
      [ProtoMember(91)]
      [XmlText]
      public string itemName;
      [ProtoMember(94)]
      [XmlAttribute]
      public string itemType;

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EStartingPhysicalItem\u003C\u003Eamount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem, float>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem owner,
          in float value)
        {
          owner.amount = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem owner,
          out float value)
        {
          value = owner.amount;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EStartingPhysicalItem\u003C\u003EitemName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem owner,
          in string value)
        {
          owner.itemName = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem owner,
          out string value)
        {
          value = owner.itemName;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EStartingPhysicalItem\u003C\u003EitemType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem owner,
          in string value)
        {
          owner.itemType = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem owner,
          out string value)
        {
          value = owner.itemType;
        }
      }

      private class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EStartingPhysicalItem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem();

        MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem IActivator<MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem>.CreateInstance() => new MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem();
      }
    }

    [ProtoContract]
    public class MyOBBattleSettings
    {
      [ProtoMember(97)]
      [XmlArrayItem("Slot")]
      public SerializableBoundingBoxD[] AttackerSlots;
      [ProtoMember(100)]
      public SerializableBoundingBoxD DefenderSlot;
      [ProtoMember(103)]
      public long DefenderEntityId;

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EMyOBBattleSettings\u003C\u003EAttackerSlots\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings, SerializableBoundingBoxD[]>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings owner,
          in SerializableBoundingBoxD[] value)
        {
          owner.AttackerSlots = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings owner,
          out SerializableBoundingBoxD[] value)
        {
          value = owner.AttackerSlots;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EMyOBBattleSettings\u003C\u003EDefenderSlot\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings, SerializableBoundingBoxD>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings owner,
          in SerializableBoundingBoxD value)
        {
          owner.DefenderSlot = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings owner,
          out SerializableBoundingBoxD value)
        {
          value = owner.DefenderSlot;
        }
      }

      protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EMyOBBattleSettings\u003C\u003EDefenderEntityId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings, long>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings owner,
          in long value)
        {
          owner.DefenderEntityId = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings owner,
          out long value)
        {
          value = owner.DefenderEntityId;
        }
      }

      private class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EMyOBBattleSettings\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings();

        MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings IActivator<MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings>.CreateInstance() => new MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings();
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EGameDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.GameDefinition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.GameDefinition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EEnvironmentDefinition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.EnvironmentDefinition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.EnvironmentDefinition;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EAsteroidClusters\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings value)
      {
        owner.AsteroidClusters = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_ScenarioDefinition.AsteroidClustersSettings value)
      {
        value = owner.AsteroidClusters;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EDefaultEnvironment\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyEnvironmentHostilityEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyEnvironmentHostilityEnum value)
      {
        owner.DefaultEnvironment = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyEnvironmentHostilityEnum value)
      {
        value = owner.DefaultEnvironment;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EPossibleStartingStates\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_WorldGeneratorPlayerStartingState[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_WorldGeneratorPlayerStartingState[] value)
      {
        owner.PossibleStartingStates = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_WorldGeneratorPlayerStartingState[] value)
      {
        value = owner.PossibleStartingStates;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EWorldGeneratorOperations\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_WorldGeneratorOperation[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_WorldGeneratorOperation[] value)
      {
        owner.WorldGeneratorOperations = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_WorldGeneratorOperation[] value)
      {
        value = owner.WorldGeneratorOperations;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ECreativeModeWeapons\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string[] value) => owner.CreativeModeWeapons = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string[] value) => value = owner.CreativeModeWeapons;
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ECreativeModeComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_ScenarioDefinition.StartingItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_ScenarioDefinition.StartingItem[] value)
      {
        owner.CreativeModeComponents = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_ScenarioDefinition.StartingItem[] value)
      {
        value = owner.CreativeModeComponents;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ECreativeModePhysicalItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem[] value)
      {
        owner.CreativeModePhysicalItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem[] value)
      {
        value = owner.CreativeModePhysicalItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ECreativeModeAmmoItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_ScenarioDefinition.StartingItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_ScenarioDefinition.StartingItem[] value)
      {
        owner.CreativeModeAmmoItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_ScenarioDefinition.StartingItem[] value)
      {
        value = owner.CreativeModeAmmoItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ESurvivalModeWeapons\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string[] value) => owner.SurvivalModeWeapons = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string[] value) => value = owner.SurvivalModeWeapons;
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ESurvivalModeComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_ScenarioDefinition.StartingItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_ScenarioDefinition.StartingItem[] value)
      {
        owner.SurvivalModeComponents = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_ScenarioDefinition.StartingItem[] value)
      {
        value = owner.SurvivalModeComponents;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ESurvivalModePhysicalItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem[] value)
      {
        owner.SurvivalModePhysicalItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_ScenarioDefinition.StartingPhysicalItem[] value)
      {
        value = owner.SurvivalModePhysicalItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ESurvivalModeAmmoItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_ScenarioDefinition.StartingItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_ScenarioDefinition.StartingItem[] value)
      {
        owner.SurvivalModeAmmoItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_ScenarioDefinition.StartingItem[] value)
      {
        value = owner.SurvivalModeAmmoItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ECreativeInventoryItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_InventoryItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_InventoryItem[] value)
      {
        owner.CreativeInventoryItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_InventoryItem[] value)
      {
        value = owner.CreativeInventoryItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ESurvivalInventoryItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_InventoryItem[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_InventoryItem[] value)
      {
        owner.SurvivalInventoryItems = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_InventoryItem[] value)
      {
        value = owner.SurvivalInventoryItems;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EWorldBoundaries\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableBoundingBoxD?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in SerializableBoundingBoxD? value)
      {
        owner.WorldBoundaries = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out SerializableBoundingBoxD? value)
      {
        value = owner.WorldBoundaries;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003Em_creativeDefaultToolbar\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_Toolbar>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_Toolbar value)
      {
        owner.m_creativeDefaultToolbar = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_Toolbar value)
      {
        value = owner.m_creativeDefaultToolbar;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ESurvivalDefaultToolbar\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_Toolbar>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_Toolbar value)
      {
        owner.SurvivalDefaultToolbar = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_Toolbar value)
      {
        value = owner.SurvivalDefaultToolbar;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EBattle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings value)
      {
        owner.Battle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_ScenarioDefinition.MyOBBattleSettings value)
      {
        value = owner.Battle;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EMainCharacterModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value) => owner.MainCharacterModel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value) => value = owner.MainCharacterModel;
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EGameDate\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in long value) => owner.GameDate = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out long value) => value = owner.GameDate;
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ESunDirection\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableVector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in SerializableVector3 value)
      {
        owner.SunDirection = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out SerializableVector3 value)
      {
        value = owner.SunDirection;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EDefaultToolbar\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_Toolbar>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_Toolbar value)
      {
        owner.DefaultToolbar = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_Toolbar value)
      {
        value = owner.DefaultToolbar;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ECreativeDefaultToolbar\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyObjectBuilder_Toolbar>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ScenarioDefinition owner,
        in MyObjectBuilder_Toolbar value)
      {
        owner.CreativeDefaultToolbar = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ScenarioDefinition owner,
        out MyObjectBuilder_Toolbar value)
      {
        value = owner.CreativeDefaultToolbar;
      }
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ScenarioDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ScenarioDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ScenarioDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_ScenarioDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ScenarioDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ScenarioDefinition();

      MyObjectBuilder_ScenarioDefinition IActivator<MyObjectBuilder_ScenarioDefinition>.CreateInstance() => new MyObjectBuilder_ScenarioDefinition();
    }
  }
}
