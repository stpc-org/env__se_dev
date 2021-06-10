// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyCharacterDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_CharacterDefinition), null)]
  public class MyCharacterDefinition : MyDefinitionBase
  {
    public string Name;
    public string Model;
    public string ReflectorTexture;
    public string LeftGlare;
    public string RightGlare;
    public string LeftLightBone;
    public string RightLightBone;
    public Vector3 LightOffset;
    public float LightGlareSize;
    public string HeadBone;
    public string Camera3rdBone;
    public string LeftHandIKStartBone;
    public string LeftHandIKEndBone;
    public string RightHandIKStartBone;
    public string RightHandIKEndBone;
    public string WeaponBone;
    public string LeftHandItemBone;
    public string Skeleton;
    public string LeftForearmBone;
    public string LeftUpperarmBone;
    public string RightForearmBone;
    public string RightUpperarmBone;
    public string SpineBone;
    public float BendMultiplier1st;
    public float BendMultiplier3rd;
    public bool UsesAtmosphereDetector;
    public bool UsesReverbDetector;
    [Obsolete("Dont ever use again.")]
    public bool NeedsOxygen;
    public float OxygenConsumptionMultiplier;
    public float OxygenConsumption;
    public float OxygenSuitRefillTime;
    public float MinOxygenLevelForSuitRefill;
    public float PressureLevelForLowDamage;
    public float DamageAmountAtZeroPressure;
    public MyStringHash FootprintDecal;
    public MyStringHash FootprintMirroredDecal;
    public bool LoopingFootsteps;
    public bool VisibleOnHud;
    public bool UsableByPlayer;
    public string PhysicalMaterial;
    public float JumpForce;
    public bool EnableFirstPersonView;
    public string JumpSoundName;
    public string JetpackIdleSoundName;
    public string JetpackRunSoundName;
    public string CrouchDownSoundName;
    public string CrouchUpSoundName;
    public string PainSoundName;
    public string SuffocateSoundName;
    public string DeathSoundName;
    public string DeathBySuffocationSoundName;
    public string IronsightActSoundName;
    public string IronsightDeactSoundName;
    public string FastFlySoundName;
    public string HelmetOxygenNormalSoundName;
    public string HelmetOxygenLowSoundName;
    public string HelmetOxygenCriticalSoundName;
    public string HelmetOxygenNoneSoundName;
    public string MovementSoundName;
    public string MagnetBootsStartSoundName;
    public string MagnetBootsEndSoundName;
    public string MagnetBootsStepsSoundName;
    public string MagnetBootsProximitySoundName;
    public string BreathCalmSoundName;
    public string BreathHeavySoundName;
    public string OxygenChokeNormalSoundName;
    public string OxygenChokeLowSoundName;
    public string OxygenChokeCriticalSoundName;
    public int StepSoundDelay;
    public float AnkleHeightWhileStanding;
    public bool FeetIKEnabled;
    public string ModelRootBoneName;
    public string LeftHipBoneName;
    public string LeftKneeBoneName;
    public string LeftAnkleBoneName;
    public string RightHipBoneName;
    public string RightKneeBoneName;
    public string RightAnkleBoneName;
    public string RagdollDataFile;
    public Dictionary<string, MyCharacterDefinition.RagdollBoneSet> RagdollBonesMappings = new Dictionary<string, MyCharacterDefinition.RagdollBoneSet>();
    public Dictionary<string, string[]> RagdollPartialSimulations = new Dictionary<string, string[]>();
    public HashSet<int> WeakPointBoneIndices = new HashSet<int>();
    public string RagdollRootBody;
    public Dictionary<MyCharacterMovementEnum, MyFeetIKSettings> FeetIKSettings;
    public List<SuitResourceDefinition> SuitResourceStorage;
    public MyObjectBuilder_JetpackDefinition Jetpack;
    public Dictionary<string, string[]> BoneSets = new Dictionary<string, string[]>();
    public Dictionary<float, string[]> BoneLODs = new Dictionary<float, string[]>();
    public Dictionary<string, string> AnimationNameToSubtypeName = new Dictionary<string, string>();
    public string[] MaterialsDisabledIn1st;
    public float Mass;
    public float ImpulseLimit;
    public string RighHandItemBone;
    public bool VerticalPositionFlyingOnly;
    public bool UseOnlyWalking;
    public float MaxSlope;
    public float MaxSprintSpeed;
    public float MaxRunSpeed;
    public float MaxBackrunSpeed;
    public float MaxRunStrafingSpeed;
    public float MaxWalkSpeed;
    public float MaxBackwalkSpeed;
    public float MaxWalkStrafingSpeed;
    public float MaxCrouchWalkSpeed;
    public float MaxCrouchBackwalkSpeed;
    public float MaxCrouchStrafingSpeed;
    public float CharacterHeadSize;
    public float CharacterHeadHeight;
    public float CharacterCollisionScale;
    public float CharacterCollisionHeight;
    public float CharacterCollisionWidth;
    public float CharacterCollisionCrouchHeight;
    public float CharacterWidth;
    public float CharacterHeight;
    public float CharacterLength;
    public bool CanCrouch;
    public bool CanIronsight;
    public float CrouchHeadServerOffset;
    public MyObjectBuilder_InventoryDefinition InventoryDefinition;
    public bool EnableSpawnInventoryAsContainer;
    public MyDefinitionId? InventorySpawnContainerId;
    public bool SpawnInventoryOnBodyRemoval;
    [Obsolete("Use MyComponentDefinitionBase and MyContainerDefinition to define enabled types of components on entities")]
    public List<string> EnabledComponents = new List<string>();
    public float LootingTime;
    public string InitialAnimation;
    public MyObjectBuilder_DeadBodyShape DeadBodyShape;
    public string AnimationController;
    public float? MaxForce;
    public MyEnumCharacterRotationToSupport RotationToSupport;
    public string HUD;
    public float SuitConsumptionInTemperatureExtreme;
    public float RecoilJetpackDampeningRadPerFrame;
    public List<MyObjectBuilder_FootsPosition> FootOnGroundPostions;

    public bool UseNewAnimationSystem => this.AnimationController != null;

    protected override void Init(MyObjectBuilder_DefinitionBase objectBuilder)
    {
      base.Init(objectBuilder);
      MyObjectBuilder_CharacterDefinition characterDefinition = (MyObjectBuilder_CharacterDefinition) objectBuilder;
      this.Name = characterDefinition.Name;
      this.Model = characterDefinition.Model;
      this.ReflectorTexture = characterDefinition.ReflectorTexture;
      this.LeftGlare = characterDefinition.LeftGlare;
      this.RightGlare = characterDefinition.RightGlare;
      this.LeftLightBone = characterDefinition.LeftLightBone;
      this.RightLightBone = characterDefinition.RightLightBone;
      this.LightOffset = characterDefinition.LightOffset;
      this.LightGlareSize = characterDefinition.LightGlareSize;
      this.FootprintDecal = MyStringHash.GetOrCompute(characterDefinition.FootprintDecal);
      this.FootprintMirroredDecal = MyStringHash.GetOrCompute(characterDefinition.FootprintMirroredDecal);
      this.HeadBone = characterDefinition.HeadBone;
      this.Camera3rdBone = characterDefinition.Camera3rdBone;
      this.LeftHandIKStartBone = characterDefinition.LeftHandIKStartBone;
      this.LeftHandIKEndBone = characterDefinition.LeftHandIKEndBone;
      this.RightHandIKStartBone = characterDefinition.RightHandIKStartBone;
      this.RightHandIKEndBone = characterDefinition.RightHandIKEndBone;
      this.WeaponBone = characterDefinition.WeaponBone;
      this.LeftHandItemBone = characterDefinition.LeftHandItemBone;
      this.RighHandItemBone = characterDefinition.RightHandItemBone;
      this.Skeleton = characterDefinition.Skeleton;
      this.LeftForearmBone = characterDefinition.LeftForearmBone;
      this.LeftUpperarmBone = characterDefinition.LeftUpperarmBone;
      this.RightForearmBone = characterDefinition.RightForearmBone;
      this.RightUpperarmBone = characterDefinition.RightUpperarmBone;
      this.SpineBone = characterDefinition.SpineBone;
      this.BendMultiplier1st = characterDefinition.BendMultiplier1st;
      this.BendMultiplier3rd = characterDefinition.BendMultiplier3rd;
      this.MaterialsDisabledIn1st = characterDefinition.MaterialsDisabledIn1st;
      this.FeetIKEnabled = characterDefinition.FeetIKEnabled;
      this.ModelRootBoneName = characterDefinition.ModelRootBoneName;
      this.LeftHipBoneName = characterDefinition.LeftHipBoneName;
      this.LeftKneeBoneName = characterDefinition.LeftKneeBoneName;
      this.LeftAnkleBoneName = characterDefinition.LeftAnkleBoneName;
      this.RightHipBoneName = characterDefinition.RightHipBoneName;
      this.RightKneeBoneName = characterDefinition.RightKneeBoneName;
      this.RightAnkleBoneName = characterDefinition.RightAnkleBoneName;
      this.UsesAtmosphereDetector = characterDefinition.UsesAtmosphereDetector;
      this.UsesReverbDetector = characterDefinition.UsesReverbDetector;
      this.NeedsOxygen = characterDefinition.NeedsOxygen;
      this.OxygenConsumptionMultiplier = characterDefinition.OxygenConsumptionMultiplier;
      this.OxygenConsumption = characterDefinition.OxygenConsumption;
      this.OxygenSuitRefillTime = characterDefinition.OxygenSuitRefillTime;
      this.MinOxygenLevelForSuitRefill = characterDefinition.MinOxygenLevelForSuitRefill;
      this.PressureLevelForLowDamage = characterDefinition.PressureLevelForLowDamage;
      this.DamageAmountAtZeroPressure = characterDefinition.DamageAmountAtZeroPressure;
      this.RagdollDataFile = characterDefinition.RagdollDataFile;
      this.JumpSoundName = characterDefinition.JumpSoundName;
      this.JetpackIdleSoundName = characterDefinition.JetpackIdleSoundName;
      this.JetpackRunSoundName = characterDefinition.JetpackRunSoundName;
      this.CrouchDownSoundName = characterDefinition.CrouchDownSoundName;
      this.CrouchUpSoundName = characterDefinition.CrouchUpSoundName;
      this.PainSoundName = characterDefinition.PainSoundName;
      this.SuffocateSoundName = characterDefinition.SuffocateSoundName;
      this.DeathSoundName = characterDefinition.DeathSoundName;
      this.DeathBySuffocationSoundName = characterDefinition.DeathBySuffocationSoundName;
      this.IronsightActSoundName = characterDefinition.IronsightActSoundName;
      this.IronsightDeactSoundName = characterDefinition.IronsightDeactSoundName;
      this.FastFlySoundName = characterDefinition.FastFlySoundName;
      this.HelmetOxygenNormalSoundName = characterDefinition.HelmetOxygenNormalSoundName;
      this.HelmetOxygenLowSoundName = characterDefinition.HelmetOxygenLowSoundName;
      this.HelmetOxygenCriticalSoundName = characterDefinition.HelmetOxygenCriticalSoundName;
      this.HelmetOxygenNoneSoundName = characterDefinition.HelmetOxygenNoneSoundName;
      this.MovementSoundName = characterDefinition.MovementSoundName;
      this.MagnetBootsStartSoundName = characterDefinition.MagnetBootsStartSoundName;
      this.MagnetBootsStepsSoundName = characterDefinition.MagnetBootsStepsSoundName;
      this.MagnetBootsEndSoundName = characterDefinition.MagnetBootsEndSoundName;
      this.MagnetBootsProximitySoundName = characterDefinition.MagnetBootsProximitySoundName;
      this.LoopingFootsteps = characterDefinition.LoopingFootsteps;
      this.VisibleOnHud = characterDefinition.VisibleOnHud;
      this.UsableByPlayer = characterDefinition.UsableByPlayer;
      this.RagdollRootBody = characterDefinition.RagdollRootBody;
      this.InitialAnimation = characterDefinition.InitialAnimation;
      this.PhysicalMaterial = characterDefinition.PhysicalMaterial;
      this.JumpForce = characterDefinition.JumpForce;
      this.RotationToSupport = characterDefinition.RotationToSupport;
      this.HUD = characterDefinition.HUD;
      this.EnableFirstPersonView = characterDefinition.EnableFirstPersonView;
      this.StepSoundDelay = characterDefinition.StepSoundDelay;
      this.AnkleHeightWhileStanding = characterDefinition.AnkleHeightWhileStanding;
      this.BreathCalmSoundName = characterDefinition.BreathCalmSoundName;
      this.BreathHeavySoundName = characterDefinition.BreathHeavySoundName;
      this.OxygenChokeNormalSoundName = characterDefinition.OxygenChokeNormalSoundName;
      this.OxygenChokeLowSoundName = characterDefinition.OxygenChokeLowSoundName;
      this.OxygenChokeCriticalSoundName = characterDefinition.OxygenChokeCriticalSoundName;
      this.CrouchHeadServerOffset = characterDefinition.CrouchHeadServerOffset;
      this.RecoilJetpackDampeningRadPerFrame = (float) ((double) characterDefinition.RecoilJetpackDampeningDegPerS * (Math.PI / 180.0) / 60.0);
      this.FeetIKSettings = new Dictionary<MyCharacterMovementEnum, MyFeetIKSettings>();
      if (characterDefinition.IKSettings != null)
      {
        foreach (MyObjectBuilder_MyFeetIKSettings ikSetting in characterDefinition.IKSettings)
        {
          string movementState = ikSetting.MovementState;
          char[] chArray = new char[1]{ ',' };
          foreach (string str1 in movementState.Split(chArray))
          {
            string str2 = str1.Trim();
            MyCharacterMovementEnum result;
            if (str2 != "" && Enum.TryParse<MyCharacterMovementEnum>(str2, true, out result))
              this.FeetIKSettings.Add(result, new MyFeetIKSettings()
              {
                Enabled = ikSetting.Enabled,
                AboveReachableDistance = ikSetting.AboveReachableDistance,
                BelowReachableDistance = ikSetting.BelowReachableDistance,
                VerticalShiftDownGain = ikSetting.VerticalShiftDownGain,
                VerticalShiftUpGain = ikSetting.VerticalShiftUpGain,
                FootSize = new Vector3(ikSetting.FootWidth, ikSetting.AnkleHeight, ikSetting.FootLenght)
              });
          }
        }
      }
      this.SuitResourceStorage = characterDefinition.SuitResourceStorage;
      this.Jetpack = characterDefinition.Jetpack;
      if (characterDefinition.BoneSets != null)
        this.BoneSets = ((IEnumerable<MyBoneSetDefinition>) characterDefinition.BoneSets).ToDictionary<MyBoneSetDefinition, string, string[]>((Func<MyBoneSetDefinition, string>) (x => x.Name), (Func<MyBoneSetDefinition, string[]>) (x => x.Bones.Split(' ')));
      if (characterDefinition.BoneLODs != null)
        this.BoneLODs = ((IEnumerable<MyBoneSetDefinition>) characterDefinition.BoneLODs).ToDictionary<MyBoneSetDefinition, float, string[]>((Func<MyBoneSetDefinition, float>) (x => Convert.ToSingle(x.Name)), (Func<MyBoneSetDefinition, string[]>) (x => x.Bones.Split(' ')));
      if (characterDefinition.AnimationMappings != null)
        this.AnimationNameToSubtypeName = ((IEnumerable<MyMovementAnimationMapping>) characterDefinition.AnimationMappings).ToDictionary<MyMovementAnimationMapping, string, string>((Func<MyMovementAnimationMapping, string>) (mapping => mapping.Name), (Func<MyMovementAnimationMapping, string>) (mapping => mapping.AnimationSubtypeName));
      if (characterDefinition.RagdollBonesMappings != null)
        this.RagdollBonesMappings = ((IEnumerable<MyRagdollBoneSetDefinition>) characterDefinition.RagdollBonesMappings).ToDictionary<MyRagdollBoneSetDefinition, string, MyCharacterDefinition.RagdollBoneSet>((Func<MyRagdollBoneSetDefinition, string>) (x => x.Name), (Func<MyRagdollBoneSetDefinition, MyCharacterDefinition.RagdollBoneSet>) (x => new MyCharacterDefinition.RagdollBoneSet(x.Bones, x.CollisionRadius)));
      if (characterDefinition.WeakPointBoneIndices != null)
      {
        foreach (int weakPointBoneIndex in (List<int>) characterDefinition.WeakPointBoneIndices)
          this.WeakPointBoneIndices.Add(weakPointBoneIndex);
      }
      if (characterDefinition.RagdollPartialSimulations != null)
        this.RagdollPartialSimulations = ((IEnumerable<MyBoneSetDefinition>) characterDefinition.RagdollPartialSimulations).ToDictionary<MyBoneSetDefinition, string, string[]>((Func<MyBoneSetDefinition, string>) (x => x.Name), (Func<MyBoneSetDefinition, string[]>) (x => x.Bones.Split(' ')));
      this.Mass = characterDefinition.Mass;
      this.ImpulseLimit = characterDefinition.ImpulseLimit;
      this.VerticalPositionFlyingOnly = characterDefinition.VerticalPositionFlyingOnly;
      this.UseOnlyWalking = characterDefinition.UseOnlyWalking;
      this.MaxSlope = characterDefinition.MaxSlope;
      this.MaxSprintSpeed = characterDefinition.MaxSprintSpeed;
      this.MaxRunSpeed = characterDefinition.MaxRunSpeed;
      this.MaxBackrunSpeed = characterDefinition.MaxBackrunSpeed;
      this.MaxRunStrafingSpeed = characterDefinition.MaxRunStrafingSpeed;
      this.MaxWalkSpeed = characterDefinition.MaxWalkSpeed;
      this.MaxBackwalkSpeed = characterDefinition.MaxBackwalkSpeed;
      this.MaxWalkStrafingSpeed = characterDefinition.MaxWalkStrafingSpeed;
      this.MaxCrouchWalkSpeed = characterDefinition.MaxCrouchWalkSpeed;
      this.MaxCrouchBackwalkSpeed = characterDefinition.MaxCrouchBackwalkSpeed;
      this.MaxCrouchStrafingSpeed = characterDefinition.MaxCrouchStrafingSpeed;
      this.CharacterHeadSize = characterDefinition.CharacterHeadSize;
      this.CharacterHeadHeight = characterDefinition.CharacterHeadHeight;
      this.CharacterCollisionScale = characterDefinition.CharacterCollisionScale;
      this.CharacterCollisionWidth = characterDefinition.CharacterCollisionWidth;
      this.CharacterCollisionHeight = characterDefinition.CharacterCollisionHeight;
      this.CharacterCollisionCrouchHeight = characterDefinition.CharacterCollisionCrouchHeight;
      this.CanCrouch = characterDefinition.CanCrouch;
      this.CanIronsight = characterDefinition.CanIronsight;
      this.InventoryDefinition = characterDefinition.Inventory != null ? characterDefinition.Inventory : new MyObjectBuilder_InventoryDefinition();
      if (characterDefinition.EnabledComponents != null)
        this.EnabledComponents = ((IEnumerable<string>) characterDefinition.EnabledComponents.Split(' ')).ToList<string>();
      this.EnableSpawnInventoryAsContainer = characterDefinition.EnableSpawnInventoryAsContainer;
      if (this.EnableSpawnInventoryAsContainer)
      {
        if (characterDefinition.InventorySpawnContainerId.HasValue)
          this.InventorySpawnContainerId = new MyDefinitionId?((MyDefinitionId) characterDefinition.InventorySpawnContainerId.Value);
        this.SpawnInventoryOnBodyRemoval = characterDefinition.SpawnInventoryOnBodyRemoval;
      }
      this.LootingTime = characterDefinition.LootingTime;
      this.DeadBodyShape = characterDefinition.DeadBodyShape;
      this.AnimationController = characterDefinition.AnimationController;
      this.MaxForce = characterDefinition.MaxForce;
      this.SuitConsumptionInTemperatureExtreme = characterDefinition.SuitConsumptionInTemperatureExtreme;
      this.FootOnGroundPostions = characterDefinition.FootOnGroundPostions;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_CharacterDefinition objectBuilder = (MyObjectBuilder_CharacterDefinition) base.GetObjectBuilder();
      objectBuilder.Name = this.Name;
      objectBuilder.Model = this.Model;
      objectBuilder.ReflectorTexture = this.ReflectorTexture;
      objectBuilder.LeftGlare = this.LeftGlare;
      objectBuilder.RightGlare = this.RightGlare;
      objectBuilder.LightGlareSize = this.LightGlareSize;
      objectBuilder.Skeleton = this.Skeleton;
      objectBuilder.LeftForearmBone = this.LeftForearmBone;
      objectBuilder.LeftUpperarmBone = this.LeftUpperarmBone;
      objectBuilder.RightForearmBone = this.RightForearmBone;
      objectBuilder.RightUpperarmBone = this.RightUpperarmBone;
      objectBuilder.SpineBone = this.SpineBone;
      objectBuilder.MaterialsDisabledIn1st = this.MaterialsDisabledIn1st;
      objectBuilder.UsesAtmosphereDetector = this.UsesAtmosphereDetector;
      objectBuilder.UsesReverbDetector = this.UsesReverbDetector;
      objectBuilder.NeedsOxygen = this.NeedsOxygen;
      objectBuilder.OxygenConsumptionMultiplier = this.OxygenConsumptionMultiplier;
      objectBuilder.OxygenConsumption = this.OxygenConsumption;
      objectBuilder.OxygenSuitRefillTime = this.OxygenSuitRefillTime;
      objectBuilder.MinOxygenLevelForSuitRefill = this.MinOxygenLevelForSuitRefill;
      objectBuilder.PressureLevelForLowDamage = this.PressureLevelForLowDamage;
      objectBuilder.DamageAmountAtZeroPressure = this.DamageAmountAtZeroPressure;
      objectBuilder.JumpSoundName = this.JumpSoundName;
      objectBuilder.JetpackIdleSoundName = this.JetpackIdleSoundName;
      objectBuilder.JetpackRunSoundName = this.JetpackRunSoundName;
      objectBuilder.CrouchDownSoundName = this.CrouchDownSoundName;
      objectBuilder.CrouchUpSoundName = this.CrouchUpSoundName;
      objectBuilder.SuffocateSoundName = this.SuffocateSoundName;
      objectBuilder.PainSoundName = this.PainSoundName;
      objectBuilder.DeathSoundName = this.DeathSoundName;
      objectBuilder.DeathBySuffocationSoundName = this.DeathBySuffocationSoundName;
      objectBuilder.IronsightActSoundName = this.IronsightActSoundName;
      objectBuilder.IronsightDeactSoundName = this.IronsightDeactSoundName;
      objectBuilder.LoopingFootsteps = this.LoopingFootsteps;
      objectBuilder.MagnetBootsStartSoundName = this.MagnetBootsStartSoundName;
      objectBuilder.MagnetBootsEndSoundName = this.MagnetBootsEndSoundName;
      objectBuilder.MagnetBootsStepsSoundName = this.MagnetBootsStepsSoundName;
      objectBuilder.MagnetBootsProximitySoundName = this.MagnetBootsProximitySoundName;
      objectBuilder.StepSoundDelay = this.StepSoundDelay;
      objectBuilder.AnkleHeightWhileStanding = this.AnkleHeightWhileStanding;
      objectBuilder.VisibleOnHud = this.VisibleOnHud;
      objectBuilder.UsableByPlayer = this.UsableByPlayer;
      objectBuilder.SuitResourceStorage = this.SuitResourceStorage;
      objectBuilder.Jetpack = this.Jetpack;
      objectBuilder.VerticalPositionFlyingOnly = this.VerticalPositionFlyingOnly;
      objectBuilder.UseOnlyWalking = this.UseOnlyWalking;
      objectBuilder.MaxSlope = this.MaxSlope;
      objectBuilder.MaxSprintSpeed = this.MaxSprintSpeed;
      objectBuilder.MaxRunSpeed = this.MaxRunSpeed;
      objectBuilder.MaxBackrunSpeed = this.MaxBackrunSpeed;
      objectBuilder.MaxRunStrafingSpeed = this.MaxRunStrafingSpeed;
      objectBuilder.MaxWalkSpeed = this.MaxWalkSpeed;
      objectBuilder.MaxBackwalkSpeed = this.MaxBackwalkSpeed;
      objectBuilder.MaxWalkStrafingSpeed = this.MaxWalkStrafingSpeed;
      objectBuilder.MaxCrouchWalkSpeed = this.MaxCrouchWalkSpeed;
      objectBuilder.MaxCrouchBackwalkSpeed = this.MaxCrouchBackwalkSpeed;
      objectBuilder.MaxCrouchStrafingSpeed = this.MaxCrouchStrafingSpeed;
      objectBuilder.CharacterHeadSize = this.CharacterHeadSize;
      objectBuilder.CharacterHeadHeight = this.CharacterHeadHeight;
      objectBuilder.CharacterCollisionScale = this.CharacterCollisionScale;
      objectBuilder.CharacterCollisionWidth = this.CharacterCollisionWidth;
      objectBuilder.CharacterCollisionHeight = this.CharacterCollisionHeight;
      objectBuilder.CharacterCollisionCrouchHeight = this.CharacterCollisionCrouchHeight;
      objectBuilder.CanCrouch = this.CanCrouch;
      objectBuilder.CanIronsight = this.CanIronsight;
      objectBuilder.Inventory = this.InventoryDefinition;
      objectBuilder.PhysicalMaterial = this.PhysicalMaterial;
      objectBuilder.EnabledComponents = string.Join(" ", (IEnumerable<string>) this.EnabledComponents);
      objectBuilder.EnableSpawnInventoryAsContainer = this.EnableSpawnInventoryAsContainer;
      if (this.EnableSpawnInventoryAsContainer)
      {
        if (this.InventorySpawnContainerId.HasValue)
          objectBuilder.InventorySpawnContainerId = new SerializableDefinitionId?((SerializableDefinitionId) this.InventorySpawnContainerId.Value);
        objectBuilder.SpawnInventoryOnBodyRemoval = this.SpawnInventoryOnBodyRemoval;
      }
      objectBuilder.LootingTime = this.LootingTime;
      objectBuilder.DeadBodyShape = this.DeadBodyShape;
      objectBuilder.AnimationController = this.AnimationController;
      objectBuilder.MaxForce = this.MaxForce;
      objectBuilder.RotationToSupport = this.RotationToSupport;
      objectBuilder.BreathCalmSoundName = this.BreathCalmSoundName;
      objectBuilder.BreathHeavySoundName = this.BreathHeavySoundName;
      objectBuilder.OxygenChokeNormalSoundName = this.OxygenChokeNormalSoundName;
      objectBuilder.OxygenChokeLowSoundName = this.OxygenChokeLowSoundName;
      objectBuilder.OxygenChokeCriticalSoundName = this.OxygenChokeCriticalSoundName;
      objectBuilder.SuitConsumptionInTemperatureExtreme = this.SuitConsumptionInTemperatureExtreme;
      objectBuilder.FootOnGroundPostions = this.FootOnGroundPostions;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    public class RagdollBoneSet
    {
      public string[] Bones;
      public float CollisionRadius;

      public RagdollBoneSet(string bones, float radius)
      {
        this.Bones = bones.Split(' ');
        this.CollisionRadius = radius;
      }
    }

    private class Sandbox_Definitions_MyCharacterDefinition\u003C\u003EActor : IActivator, IActivator<MyCharacterDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyCharacterDefinition();

      MyCharacterDefinition IActivator<MyCharacterDefinition>.CreateInstance() => new MyCharacterDefinition();
    }
  }
}
