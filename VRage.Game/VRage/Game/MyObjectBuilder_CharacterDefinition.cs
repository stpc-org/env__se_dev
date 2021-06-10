// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_CharacterDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Data;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CharacterDefinition : MyObjectBuilder_DefinitionBase
  {
    private static readonly Vector3 DefaultLightOffset = new Vector3(0.0f, 0.0f, -0.5f);
    [ProtoMember(27)]
    public string Name;
    [ProtoMember(28)]
    [ModdableContentFile("mwm")]
    public string Model;
    [ProtoMember(29)]
    [ModdableContentFile("dds")]
    public string ReflectorTexture = "Textures\\Lights\\reflector.dds";
    [ProtoMember(30)]
    public string LeftGlare;
    [ProtoMember(31)]
    public string RightGlare;
    [ProtoMember(32)]
    public string Skeleton = "Humanoid";
    [ProtoMember(33)]
    public float LightGlareSize = 0.02f;
    [ProtoMember(34)]
    public MyObjectBuilder_JetpackDefinition Jetpack;
    [ProtoMember(35)]
    [XmlArrayItem("Resource")]
    public List<SuitResourceDefinition> SuitResourceStorage;
    [ProtoMember(36)]
    [XmlArrayItem("BoneSet")]
    public MyBoneSetDefinition[] BoneSets;
    [ProtoMember(37)]
    [XmlArrayItem("BoneSet")]
    public MyBoneSetDefinition[] BoneLODs;
    [ProtoMember(38)]
    public string LeftLightBone;
    [ProtoMember(39)]
    public string RightLightBone;
    [ProtoMember(40)]
    public Vector3 LightOffset = MyObjectBuilder_CharacterDefinition.DefaultLightOffset;
    [ProtoMember(41)]
    public string HeadBone;
    [ProtoMember(42)]
    public string LeftHandIKStartBone;
    [ProtoMember(43)]
    public string LeftHandIKEndBone;
    [ProtoMember(44)]
    public string RightHandIKStartBone;
    [ProtoMember(45)]
    public string RightHandIKEndBone;
    [ProtoMember(46)]
    public string WeaponBone;
    [ProtoMember(47)]
    public string Camera3rdBone;
    [ProtoMember(48)]
    public string LeftHandItemBone;
    [ProtoMember(49)]
    public string LeftForearmBone;
    [ProtoMember(50)]
    public string LeftUpperarmBone;
    [ProtoMember(51)]
    public string RightForearmBone;
    [ProtoMember(52)]
    public string RightUpperarmBone;
    [ProtoMember(53)]
    public string SpineBone;
    [ProtoMember(54)]
    public float BendMultiplier1st = 1f;
    [ProtoMember(55)]
    public float BendMultiplier3rd = 1f;
    [ProtoMember(56)]
    [XmlArrayItem("Material")]
    public string[] MaterialsDisabledIn1st;
    [ProtoMember(57)]
    [XmlArrayItem("Mapping")]
    public MyMovementAnimationMapping[] AnimationMappings;
    [ProtoMember(58)]
    public float Mass = 100f;
    [ProtoMember(59)]
    public string ModelRootBoneName;
    [ProtoMember(60)]
    public string LeftHipBoneName;
    [ProtoMember(61)]
    public string LeftKneeBoneName;
    [ProtoMember(62)]
    public string LeftAnkleBoneName;
    [ProtoMember(63)]
    public string RightHipBoneName;
    [ProtoMember(64)]
    public string RightKneeBoneName;
    [ProtoMember(65)]
    public string RightAnkleBoneName;
    [ProtoMember(66)]
    public bool FeetIKEnabled;
    [ProtoMember(67)]
    [XmlArrayItem("FeetIKSettings")]
    public MyObjectBuilder_MyFeetIKSettings[] IKSettings;
    [ProtoMember(68)]
    public string RightHandItemBone;
    [ProtoMember(69)]
    public bool UsesAtmosphereDetector;
    [ProtoMember(70)]
    public bool UsesReverbDetector;
    [ProtoMember(71)]
    public bool NeedsOxygen;
    [ProtoMember(72)]
    public string RagdollDataFile;
    [ProtoMember(73)]
    [XmlArrayItem("BoneSet")]
    public MyRagdollBoneSetDefinition[] RagdollBonesMappings;
    [ProtoMember(74)]
    [XmlArrayItem("BoneSet")]
    public MyBoneSetDefinition[] RagdollPartialSimulations;
    [ProtoMember(75)]
    public float OxygenConsumptionMultiplier = 1f;
    [ProtoMember(76)]
    public float OxygenConsumption = 10f;
    [ProtoMember(77)]
    public float PressureLevelForLowDamage = 0.5f;
    [ProtoMember(78)]
    public float DamageAmountAtZeroPressure = 7f;
    [ProtoMember(79)]
    public bool VerticalPositionFlyingOnly;
    [ProtoMember(80)]
    public bool UseOnlyWalking = true;
    [ProtoMember(81)]
    public float MaxSlope = 60f;
    [ProtoMember(82)]
    public float MaxSprintSpeed = 11f;
    [ProtoMember(83)]
    public float MaxRunSpeed = 11f;
    [ProtoMember(84)]
    public float MaxBackrunSpeed = 11f;
    [ProtoMember(85)]
    public float MaxRunStrafingSpeed = 11f;
    [ProtoMember(86)]
    public float MaxWalkSpeed = 6f;
    [ProtoMember(87)]
    public float MaxBackwalkSpeed = 6f;
    [ProtoMember(88)]
    public float MaxWalkStrafingSpeed = 6f;
    [ProtoMember(89)]
    public float MaxCrouchWalkSpeed = 4f;
    [ProtoMember(90)]
    public float MaxCrouchBackwalkSpeed = 4f;
    [ProtoMember(91)]
    public float MaxCrouchStrafingSpeed = 4f;
    [ProtoMember(92)]
    public float CharacterHeadSize = 0.55f;
    [ProtoMember(93)]
    public float CharacterHeadHeight = 0.25f;
    [ProtoMember(94)]
    public float CharacterCollisionScale = 1f;
    [ProtoMember(95)]
    public float CharacterCollisionWidth = 1f;
    [ProtoMember(96)]
    public float CharacterCollisionHeight = 1.8f;
    [ProtoMember(97)]
    public float CharacterCollisionCrouchHeight = 1.25f;
    [ProtoMember(98)]
    public bool CanCrouch = true;
    [ProtoMember(99)]
    public bool CanIronsight = true;
    [ProtoMember(100)]
    public string JumpSoundName = "";
    [ProtoMember(101)]
    public float JumpForce = 2.5f;
    [ProtoMember(102)]
    public string JetpackIdleSoundName = "";
    [ProtoMember(103)]
    public string JetpackRunSoundName = "";
    [ProtoMember(104)]
    public string CrouchDownSoundName = "";
    [ProtoMember(105)]
    public string CrouchUpSoundName = "";
    [ProtoMember(106)]
    public string MovementSoundName = "";
    [ProtoMember(107)]
    public string PainSoundName = "";
    [ProtoMember(108)]
    public string SuffocateSoundName = "";
    [ProtoMember(109)]
    public string DeathSoundName = "";
    [ProtoMember(110)]
    public string DeathBySuffocationSoundName = "";
    [ProtoMember(111)]
    public string IronsightActSoundName = "";
    [ProtoMember(112)]
    public string IronsightDeactSoundName = "";
    [ProtoMember(113)]
    public string FastFlySoundName = "";
    [ProtoMember(114)]
    public string HelmetOxygenNormalSoundName = "";
    [ProtoMember(115)]
    public string HelmetOxygenLowSoundName = "";
    [ProtoMember(116)]
    public string HelmetOxygenCriticalSoundName = "";
    [ProtoMember(117)]
    public string HelmetOxygenNoneSoundName = "";
    [ProtoMember(118, IsRequired = false)]
    public string MagnetBootsStartSoundName = "";
    [ProtoMember(119, IsRequired = false)]
    public string MagnetBootsEndSoundName = "";
    [ProtoMember(120, IsRequired = false)]
    public string MagnetBootsStepsSoundName = "";
    [ProtoMember(121, IsRequired = false)]
    public string MagnetBootsProximitySoundName = "";
    [ProtoMember(122)]
    public bool LoopingFootsteps;
    [ProtoMember(123)]
    public bool VisibleOnHud = true;
    [ProtoMember(124)]
    public bool UsableByPlayer = true;
    [ProtoMember(125)]
    public string RagdollRootBody = string.Empty;
    [ProtoMember(126)]
    [DefaultValue(null)]
    public MyObjectBuilder_InventoryDefinition Inventory;
    [ProtoMember(127)]
    public string EnabledComponents;
    [ProtoMember(128)]
    public bool EnableSpawnInventoryAsContainer;
    [ProtoMember(129)]
    [DefaultValue(null)]
    public SerializableDefinitionId? InventorySpawnContainerId;
    [ProtoMember(130)]
    public bool SpawnInventoryOnBodyRemoval;
    [ProtoMember(131)]
    public float LootingTime = 300f;
    [ProtoMember(132)]
    public string InitialAnimation = "Idle";
    [ProtoMember(133)]
    public float ImpulseLimit = float.PositiveInfinity;
    [ProtoMember(134)]
    public string PhysicalMaterial = "Character";
    [ProtoMember(135)]
    public MyObjectBuilder_DeadBodyShape DeadBodyShape;
    [ProtoMember(136)]
    public string AnimationController;
    [ProtoMember(137)]
    public float? MaxForce;
    [ProtoMember(138)]
    public MyEnumCharacterRotationToSupport RotationToSupport;
    [ProtoMember(139)]
    public string HUD;
    [ProtoMember(140)]
    [DefaultValue(true)]
    public bool EnableFirstPersonView = true;
    [ProtoMember(141)]
    public string BreathCalmSoundName = "";
    [ProtoMember(142)]
    public string BreathHeavySoundName = "";
    [ProtoMember(143)]
    public string OxygenChokeNormalSoundName = "";
    [ProtoMember(144)]
    public string OxygenChokeLowSoundName = "";
    [ProtoMember(145)]
    public string OxygenChokeCriticalSoundName = "";
    [ProtoMember(146)]
    [DefaultValue(0)]
    public float OxygenSuitRefillTime;
    [ProtoMember(147)]
    [DefaultValue(0.75f)]
    public float MinOxygenLevelForSuitRefill = 0.75f;
    [ProtoMember(148)]
    [DefaultValue(3)]
    public float SuitConsumptionInTemperatureExtreme = 3f;
    [ProtoMember(149)]
    public string FootprintDecal = "";
    [ProtoMember(150)]
    public string FootprintMirroredDecal = "";
    [ProtoMember(151)]
    public MySerializableList<int> WeakPointBoneIndices;
    [ProtoMember(153)]
    public float RecoilJetpackDampeningDegPerS;
    [ProtoMember(160)]
    [XmlArrayItem("Item")]
    public List<MyObjectBuilder_FootsPosition> FootOnGroundPostions;
    [ProtoMember(170, IsRequired = false)]
    public int StepSoundDelay = 100;
    [ProtoMember(171, IsRequired = false)]
    public float AnkleHeightWhileStanding = 0.187f;
    [ProtoMember(173)]
    public float CrouchHeadServerOffset = 1f;

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.Name = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.Name;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EModel\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.Model = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.Model;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EReflectorTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.ReflectorTexture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.ReflectorTexture;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftGlare\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftGlare = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftGlare;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightGlare\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightGlare = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightGlare;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ESkeleton\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.Skeleton = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.Skeleton;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELightGlareSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.LightGlareSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.LightGlareSize;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EJetpack\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyObjectBuilder_JetpackDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyObjectBuilder_JetpackDefinition value)
      {
        owner.Jetpack = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyObjectBuilder_JetpackDefinition value)
      {
        value = owner.Jetpack;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ESuitResourceStorage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, List<SuitResourceDefinition>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in List<SuitResourceDefinition> value)
      {
        owner.SuitResourceStorage = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out List<SuitResourceDefinition> value)
      {
        value = owner.SuitResourceStorage;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EBoneSets\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyBoneSetDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyBoneSetDefinition[] value)
      {
        owner.BoneSets = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyBoneSetDefinition[] value)
      {
        value = owner.BoneSets;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EBoneLODs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyBoneSetDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyBoneSetDefinition[] value)
      {
        owner.BoneLODs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyBoneSetDefinition[] value)
      {
        value = owner.BoneLODs;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftLightBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftLightBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftLightBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightLightBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightLightBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightLightBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELightOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in Vector3 value) => owner.LightOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out Vector3 value) => value = owner.LightOffset;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EHeadBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.HeadBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.HeadBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftHandIKStartBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftHandIKStartBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftHandIKStartBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftHandIKEndBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftHandIKEndBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftHandIKEndBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightHandIKStartBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightHandIKStartBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightHandIKStartBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightHandIKEndBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightHandIKEndBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightHandIKEndBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EWeaponBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.WeaponBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.WeaponBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECamera3rdBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.Camera3rdBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.Camera3rdBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftHandItemBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftHandItemBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftHandItemBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftForearmBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftForearmBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftForearmBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftUpperarmBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftUpperarmBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftUpperarmBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightForearmBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightForearmBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightForearmBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightUpperarmBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightUpperarmBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightUpperarmBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ESpineBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.SpineBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.SpineBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EBendMultiplier1st\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.BendMultiplier1st = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.BendMultiplier1st;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EBendMultiplier3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.BendMultiplier3rd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.BendMultiplier3rd;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaterialsDisabledIn1st\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string[] value) => owner.MaterialsDisabledIn1st = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string[] value) => value = owner.MaterialsDisabledIn1st;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EAnimationMappings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyMovementAnimationMapping[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyMovementAnimationMapping[] value)
      {
        owner.AnimationMappings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyMovementAnimationMapping[] value)
      {
        value = owner.AnimationMappings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMass\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.Mass = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.Mass;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EModelRootBoneName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.ModelRootBoneName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.ModelRootBoneName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftHipBoneName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftHipBoneName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftHipBoneName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftKneeBoneName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftKneeBoneName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftKneeBoneName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELeftAnkleBoneName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.LeftAnkleBoneName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.LeftAnkleBoneName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightHipBoneName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightHipBoneName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightHipBoneName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightKneeBoneName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightKneeBoneName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightKneeBoneName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightAnkleBoneName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightAnkleBoneName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightAnkleBoneName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EFeetIKEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.FeetIKEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.FeetIKEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EIKSettings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyObjectBuilder_MyFeetIKSettings[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyObjectBuilder_MyFeetIKSettings[] value)
      {
        owner.IKSettings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyObjectBuilder_MyFeetIKSettings[] value)
      {
        value = owner.IKSettings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERightHandItemBone\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RightHandItemBone = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RightHandItemBone;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EUsesAtmosphereDetector\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.UsesAtmosphereDetector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.UsesAtmosphereDetector;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EUsesReverbDetector\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.UsesReverbDetector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.UsesReverbDetector;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ENeedsOxygen\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.NeedsOxygen = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.NeedsOxygen;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERagdollDataFile\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RagdollDataFile = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RagdollDataFile;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERagdollBonesMappings\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyRagdollBoneSetDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyRagdollBoneSetDefinition[] value)
      {
        owner.RagdollBonesMappings = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyRagdollBoneSetDefinition[] value)
      {
        value = owner.RagdollBonesMappings;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERagdollPartialSimulations\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyBoneSetDefinition[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyBoneSetDefinition[] value)
      {
        owner.RagdollPartialSimulations = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyBoneSetDefinition[] value)
      {
        value = owner.RagdollPartialSimulations;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EOxygenConsumptionMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.OxygenConsumptionMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.OxygenConsumptionMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EOxygenConsumption\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.OxygenConsumption = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.OxygenConsumption;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EPressureLevelForLowDamage\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.PressureLevelForLowDamage = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.PressureLevelForLowDamage;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EDamageAmountAtZeroPressure\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.DamageAmountAtZeroPressure = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.DamageAmountAtZeroPressure;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EVerticalPositionFlyingOnly\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.VerticalPositionFlyingOnly = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.VerticalPositionFlyingOnly;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EUseOnlyWalking\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.UseOnlyWalking = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.UseOnlyWalking;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxSlope\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxSlope = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxSlope;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxSprintSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxSprintSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxSprintSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxRunSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxRunSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxRunSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxBackrunSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxBackrunSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxBackrunSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxRunStrafingSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxRunStrafingSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxRunStrafingSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxWalkSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxWalkSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxWalkSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxBackwalkSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxBackwalkSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxBackwalkSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxWalkStrafingSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxWalkStrafingSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxWalkStrafingSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxCrouchWalkSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxCrouchWalkSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxCrouchWalkSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxCrouchBackwalkSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxCrouchBackwalkSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxCrouchBackwalkSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxCrouchStrafingSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MaxCrouchStrafingSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MaxCrouchStrafingSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECharacterHeadSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.CharacterHeadSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.CharacterHeadSize;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECharacterHeadHeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.CharacterHeadHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.CharacterHeadHeight;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECharacterCollisionScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.CharacterCollisionScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.CharacterCollisionScale;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECharacterCollisionWidth\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.CharacterCollisionWidth = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.CharacterCollisionWidth;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECharacterCollisionHeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.CharacterCollisionHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.CharacterCollisionHeight;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECharacterCollisionCrouchHeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.CharacterCollisionCrouchHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.CharacterCollisionCrouchHeight;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECanCrouch\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.CanCrouch = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.CanCrouch;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECanIronsight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.CanIronsight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.CanIronsight;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EJumpSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.JumpSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.JumpSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EJumpForce\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.JumpForce = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.JumpForce;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EJetpackIdleSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.JetpackIdleSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.JetpackIdleSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EJetpackRunSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.JetpackRunSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.JetpackRunSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECrouchDownSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.CrouchDownSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.CrouchDownSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECrouchUpSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.CrouchUpSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.CrouchUpSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMovementSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.MovementSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.MovementSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EPainSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.PainSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.PainSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ESuffocateSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.SuffocateSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.SuffocateSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EDeathSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.DeathSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.DeathSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EDeathBySuffocationSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.DeathBySuffocationSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.DeathBySuffocationSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EIronsightActSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.IronsightActSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.IronsightActSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EIronsightDeactSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.IronsightDeactSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.IronsightDeactSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EFastFlySoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.FastFlySoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.FastFlySoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EHelmetOxygenNormalSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.HelmetOxygenNormalSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.HelmetOxygenNormalSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EHelmetOxygenLowSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.HelmetOxygenLowSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.HelmetOxygenLowSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EHelmetOxygenCriticalSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.HelmetOxygenCriticalSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.HelmetOxygenCriticalSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EHelmetOxygenNoneSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.HelmetOxygenNoneSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.HelmetOxygenNoneSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMagnetBootsStartSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.MagnetBootsStartSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.MagnetBootsStartSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMagnetBootsEndSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.MagnetBootsEndSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.MagnetBootsEndSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMagnetBootsStepsSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.MagnetBootsStepsSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.MagnetBootsStepsSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMagnetBootsProximitySoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.MagnetBootsProximitySoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.MagnetBootsProximitySoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELoopingFootsteps\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.LoopingFootsteps = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.LoopingFootsteps;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EVisibleOnHud\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.VisibleOnHud = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.VisibleOnHud;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EUsableByPlayer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.UsableByPlayer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.UsableByPlayer;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERagdollRootBody\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.RagdollRootBody = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.RagdollRootBody;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EInventory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyObjectBuilder_InventoryDefinition>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyObjectBuilder_InventoryDefinition value)
      {
        owner.Inventory = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyObjectBuilder_InventoryDefinition value)
      {
        value = owner.Inventory;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EEnabledComponents\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.EnabledComponents = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.EnabledComponents;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EEnableSpawnInventoryAsContainer\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.EnableSpawnInventoryAsContainer = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.EnableSpawnInventoryAsContainer;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EInventorySpawnContainerId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in SerializableDefinitionId? value)
      {
        owner.InventorySpawnContainerId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out SerializableDefinitionId? value)
      {
        value = owner.InventorySpawnContainerId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ESpawnInventoryOnBodyRemoval\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.SpawnInventoryOnBodyRemoval = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.SpawnInventoryOnBodyRemoval;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ELootingTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.LootingTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.LootingTime;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EInitialAnimation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.InitialAnimation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.InitialAnimation;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EImpulseLimit\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.ImpulseLimit = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.ImpulseLimit;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EPhysicalMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.PhysicalMaterial = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.PhysicalMaterial;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EDeadBodyShape\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyObjectBuilder_DeadBodyShape>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyObjectBuilder_DeadBodyShape value)
      {
        owner.DeadBodyShape = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyObjectBuilder_DeadBodyShape value)
      {
        value = owner.DeadBodyShape;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EAnimationController\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.AnimationController = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.AnimationController;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMaxForce\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float? value) => owner.MaxForce = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float? value) => value = owner.MaxForce;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERotationToSupport\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyEnumCharacterRotationToSupport>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MyEnumCharacterRotationToSupport value)
      {
        owner.RotationToSupport = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MyEnumCharacterRotationToSupport value)
      {
        value = owner.RotationToSupport;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EHUD\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.HUD = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.HUD;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EEnableFirstPersonView\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => owner.EnableFirstPersonView = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => value = owner.EnableFirstPersonView;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EBreathCalmSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.BreathCalmSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.BreathCalmSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EBreathHeavySoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.BreathHeavySoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.BreathHeavySoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EOxygenChokeNormalSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.OxygenChokeNormalSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.OxygenChokeNormalSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EOxygenChokeLowSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.OxygenChokeLowSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.OxygenChokeLowSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EOxygenChokeCriticalSoundName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.OxygenChokeCriticalSoundName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.OxygenChokeCriticalSoundName;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EOxygenSuitRefillTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.OxygenSuitRefillTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.OxygenSuitRefillTime;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EMinOxygenLevelForSuitRefill\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.MinOxygenLevelForSuitRefill = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.MinOxygenLevelForSuitRefill;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ESuitConsumptionInTemperatureExtreme\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.SuitConsumptionInTemperatureExtreme = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.SuitConsumptionInTemperatureExtreme;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EFootprintDecal\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.FootprintDecal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.FootprintDecal;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EFootprintMirroredDecal\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => owner.FootprintMirroredDecal = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => value = owner.FootprintMirroredDecal;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EWeakPointBoneIndices\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, MySerializableList<int>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in MySerializableList<int> value)
      {
        owner.WeakPointBoneIndices = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out MySerializableList<int> value)
      {
        value = owner.WeakPointBoneIndices;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ERecoilJetpackDampeningDegPerS\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.RecoilJetpackDampeningDegPerS = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.RecoilJetpackDampeningDegPerS;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EFootOnGroundPostions\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, List<MyObjectBuilder_FootsPosition>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in List<MyObjectBuilder_FootsPosition> value)
      {
        owner.FootOnGroundPostions = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out List<MyObjectBuilder_FootsPosition> value)
      {
        value = owner.FootOnGroundPostions;
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EStepSoundDelay\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in int value) => owner.StepSoundDelay = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out int value) => value = owner.StepSoundDelay;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EAnkleHeightWhileStanding\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.AnkleHeightWhileStanding = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.AnkleHeightWhileStanding;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ECrouchHeadServerOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CharacterDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in float value) => owner.CrouchHeadServerOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out float value) => value = owner.CrouchHeadServerOffset;
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CharacterDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CharacterDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CharacterDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_CharacterDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_CharacterDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_CharacterDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CharacterDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CharacterDefinition();

      MyObjectBuilder_CharacterDefinition IActivator<MyObjectBuilder_CharacterDefinition>.CreateInstance() => new MyObjectBuilder_CharacterDefinition();
    }
  }
}
