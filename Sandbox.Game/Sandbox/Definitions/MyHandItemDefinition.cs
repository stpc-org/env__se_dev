// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyHandItemDefinition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_HandItemDefinition), null)]
  public class MyHandItemDefinition : MyDefinitionBase
  {
    public Matrix LeftHand;
    public Matrix RightHand;
    public Matrix ItemLocation;
    public Matrix ItemLocation3rd;
    public Matrix ItemWalkingLocation;
    public Matrix ItemWalkingLocation3rd;
    public Matrix ItemShootLocation;
    public Matrix ItemShootLocation3rd;
    public Matrix ItemIronsightLocation;
    public float BlendTime;
    public float XAmplitudeOffset;
    public float YAmplitudeOffset;
    public float ZAmplitudeOffset;
    public float XAmplitudeScale;
    public float YAmplitudeScale;
    public float ZAmplitudeScale;
    public float RunMultiplier;
    public float AmplitudeMultiplier3rd = 1f;
    public bool SimulateLeftHand = true;
    public bool SimulateRightHand = true;
    public bool SimulateLeftHandFps = true;
    public bool SimulateRightHandFps = true;
    public string FingersAnimation;
    public float ShootBlend;
    public Vector3 MuzzlePosition;
    public Vector3 ShootScatter;
    public float ScatterSpeed;
    public MyDefinitionId PhysicalItemId;
    public Vector4 LightColor;
    public float LightFalloff;
    public float LightRadius;
    public float LightGlareSize;
    public float LightGlareIntensity;
    public float LightIntensityLower;
    public float LightIntensityUpper;
    public float ShakeAmountTarget;
    public float ShakeAmountNoTarget;
    public MyItemPositioningEnum ItemPositioning;
    public MyItemPositioningEnum ItemPositioning3rd;
    public MyItemPositioningEnum ItemPositioningWalk;
    public MyItemPositioningEnum ItemPositioningWalk3rd;
    public MyItemPositioningEnum ItemPositioningShoot;
    public MyItemPositioningEnum ItemPositioningShoot3rd;
    public MyItemPositioningEnum ItemPositioningIronsight;
    public MyItemPositioningEnum ItemPositioningIronsight3rd;
    public List<ToolSound> ToolSounds;
    public MyStringHash ToolMaterial;
    public float SprintSpeedMultiplier = 1f;
    public float RunSpeedMultiplier = 1f;
    public float BackrunSpeedMultiplier = 1f;
    public float RunStrafingSpeedMultiplier = 1f;
    public float WalkSpeedMultiplier = 1f;
    public float BackwalkSpeedMultiplier = 1f;
    public float WalkStrafingSpeedMultiplier = 1f;
    public float CrouchWalkSpeedMultiplier = 1f;
    public float CrouchBackwalkSpeedMultiplier = 1f;
    public float CrouchStrafingSpeedMultiplier = 1f;
    public float AimingSpeedMultiplier = 1f;
    public MyItemWeaponType WeaponType;

    protected override void Init(MyObjectBuilder_DefinitionBase builder)
    {
      base.Init(builder);
      MyObjectBuilder_HandItemDefinition handItemDefinition = builder as MyObjectBuilder_HandItemDefinition;
      this.Id = (MyDefinitionId) builder.Id;
      this.LeftHand = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.LeftHandOrientation));
      this.LeftHand.Translation = handItemDefinition.LeftHandPosition;
      this.RightHand = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.RightHandOrientation));
      this.RightHand.Translation = handItemDefinition.RightHandPosition;
      this.ItemLocation = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.ItemOrientation));
      this.ItemLocation.Translation = handItemDefinition.ItemPosition;
      this.ItemWalkingLocation = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.ItemWalkingOrientation));
      this.ItemWalkingLocation.Translation = handItemDefinition.ItemWalkingPosition;
      this.BlendTime = handItemDefinition.BlendTime;
      this.XAmplitudeOffset = handItemDefinition.XAmplitudeOffset;
      this.YAmplitudeOffset = handItemDefinition.YAmplitudeOffset;
      this.ZAmplitudeOffset = handItemDefinition.ZAmplitudeOffset;
      this.XAmplitudeScale = handItemDefinition.XAmplitudeScale;
      this.YAmplitudeScale = handItemDefinition.YAmplitudeScale;
      this.ZAmplitudeScale = handItemDefinition.ZAmplitudeScale;
      this.RunMultiplier = handItemDefinition.RunMultiplier;
      this.ItemLocation3rd = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.ItemOrientation3rd));
      this.ItemLocation3rd.Translation = handItemDefinition.ItemPosition3rd;
      this.ItemWalkingLocation3rd = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.ItemWalkingOrientation3rd));
      this.ItemWalkingLocation3rd.Translation = handItemDefinition.ItemWalkingPosition3rd;
      this.AmplitudeMultiplier3rd = handItemDefinition.AmplitudeMultiplier3rd;
      this.SimulateLeftHand = handItemDefinition.SimulateLeftHand;
      this.SimulateRightHand = handItemDefinition.SimulateRightHand;
      bool? nullable = handItemDefinition.SimulateLeftHandFps;
      this.SimulateLeftHandFps = nullable.HasValue ? nullable.GetValueOrDefault() : this.SimulateLeftHand;
      nullable = handItemDefinition.SimulateRightHandFps;
      this.SimulateRightHandFps = nullable.HasValue ? nullable.GetValueOrDefault() : this.SimulateRightHand;
      this.FingersAnimation = MyDefinitionManager.Static.GetAnimationDefinitionCompatibility(handItemDefinition.FingersAnimation);
      this.ItemShootLocation = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.ItemShootOrientation));
      this.ItemShootLocation.Translation = handItemDefinition.ItemShootPosition;
      this.ItemShootLocation3rd = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.ItemShootOrientation3rd));
      this.ItemShootLocation3rd.Translation = handItemDefinition.ItemShootPosition3rd;
      this.ShootBlend = handItemDefinition.ShootBlend;
      this.ItemIronsightLocation = Matrix.CreateFromQuaternion(Quaternion.Normalize(handItemDefinition.ItemIronsightOrientation));
      this.ItemIronsightLocation.Translation = handItemDefinition.ItemIronsightPosition;
      this.MuzzlePosition = handItemDefinition.MuzzlePosition;
      this.ShootScatter = handItemDefinition.ShootScatter;
      this.ScatterSpeed = handItemDefinition.ScatterSpeed;
      this.PhysicalItemId = (MyDefinitionId) handItemDefinition.PhysicalItemId;
      this.LightColor = handItemDefinition.LightColor;
      this.LightFalloff = handItemDefinition.LightFalloff;
      this.LightRadius = handItemDefinition.LightRadius;
      this.LightGlareSize = handItemDefinition.LightGlareSize;
      this.LightGlareIntensity = handItemDefinition.LightGlareIntensity;
      this.LightIntensityLower = handItemDefinition.LightIntensityLower;
      this.LightIntensityUpper = handItemDefinition.LightIntensityUpper;
      this.ShakeAmountTarget = handItemDefinition.ShakeAmountTarget;
      this.ShakeAmountNoTarget = handItemDefinition.ShakeAmountNoTarget;
      this.ToolSounds = handItemDefinition.ToolSounds;
      this.ToolMaterial = MyStringHash.GetOrCompute(handItemDefinition.ToolMaterial);
      this.ItemPositioning = handItemDefinition.ItemPositioning;
      this.ItemPositioning3rd = handItemDefinition.ItemPositioning3rd;
      this.ItemPositioningWalk = handItemDefinition.ItemPositioningWalk;
      this.ItemPositioningWalk3rd = handItemDefinition.ItemPositioningWalk3rd;
      this.ItemPositioningShoot = handItemDefinition.ItemPositioningShoot;
      this.ItemPositioningShoot3rd = handItemDefinition.ItemPositioningShoot3rd;
      this.ItemPositioningIronsight = handItemDefinition.ItemPositioningIronsight;
      this.ItemPositioningIronsight3rd = handItemDefinition.ItemPositioningIronsight3rd;
      this.WeaponType = handItemDefinition.WeaponType;
    }

    public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
    {
      MyObjectBuilder_HandItemDefinition objectBuilder = (MyObjectBuilder_HandItemDefinition) base.GetObjectBuilder();
      objectBuilder.Id = (SerializableDefinitionId) this.Id;
      objectBuilder.LeftHandOrientation = Quaternion.CreateFromRotationMatrix(this.LeftHand);
      objectBuilder.LeftHandPosition = this.LeftHand.Translation;
      objectBuilder.RightHandOrientation = Quaternion.CreateFromRotationMatrix(this.RightHand);
      objectBuilder.RightHandPosition = this.RightHand.Translation;
      objectBuilder.ItemOrientation = Quaternion.CreateFromRotationMatrix(this.ItemLocation);
      objectBuilder.ItemPosition = this.ItemLocation.Translation;
      objectBuilder.ItemWalkingOrientation = Quaternion.CreateFromRotationMatrix(this.ItemWalkingLocation);
      objectBuilder.ItemWalkingPosition = this.ItemWalkingLocation.Translation;
      objectBuilder.BlendTime = this.BlendTime;
      objectBuilder.XAmplitudeOffset = this.XAmplitudeOffset;
      objectBuilder.YAmplitudeOffset = this.YAmplitudeOffset;
      objectBuilder.ZAmplitudeOffset = this.ZAmplitudeOffset;
      objectBuilder.XAmplitudeScale = this.XAmplitudeScale;
      objectBuilder.YAmplitudeScale = this.YAmplitudeScale;
      objectBuilder.ZAmplitudeScale = this.ZAmplitudeScale;
      objectBuilder.RunMultiplier = this.RunMultiplier;
      objectBuilder.ItemWalkingOrientation3rd = Quaternion.CreateFromRotationMatrix(this.ItemWalkingLocation3rd);
      objectBuilder.ItemWalkingPosition3rd = this.ItemWalkingLocation3rd.Translation;
      objectBuilder.ItemOrientation3rd = Quaternion.CreateFromRotationMatrix(this.ItemLocation3rd);
      objectBuilder.ItemPosition3rd = this.ItemLocation3rd.Translation;
      objectBuilder.AmplitudeMultiplier3rd = this.AmplitudeMultiplier3rd;
      objectBuilder.SimulateLeftHand = this.SimulateLeftHand;
      objectBuilder.SimulateRightHand = this.SimulateRightHand;
      objectBuilder.SimulateLeftHandFps = this.SimulateLeftHandFps != this.SimulateLeftHand ? new bool?(this.SimulateLeftHandFps) : new bool?();
      objectBuilder.SimulateRightHandFps = this.SimulateRightHandFps != this.SimulateRightHand ? new bool?(this.SimulateRightHandFps) : new bool?();
      objectBuilder.FingersAnimation = this.FingersAnimation;
      objectBuilder.ItemShootOrientation = Quaternion.CreateFromRotationMatrix(this.ItemShootLocation);
      objectBuilder.ItemShootPosition = this.ItemShootLocation.Translation;
      objectBuilder.ItemShootOrientation3rd = Quaternion.CreateFromRotationMatrix(this.ItemShootLocation3rd);
      objectBuilder.ItemShootPosition3rd = this.ItemShootLocation3rd.Translation;
      objectBuilder.ShootBlend = this.ShootBlend;
      objectBuilder.ItemIronsightOrientation = Quaternion.CreateFromRotationMatrix(this.ItemIronsightLocation);
      objectBuilder.ItemIronsightPosition = this.ItemIronsightLocation.Translation;
      objectBuilder.MuzzlePosition = this.MuzzlePosition;
      objectBuilder.ShootScatter = this.ShootScatter;
      objectBuilder.ScatterSpeed = this.ScatterSpeed;
      objectBuilder.PhysicalItemId = (SerializableDefinitionId) this.PhysicalItemId;
      objectBuilder.LightColor = this.LightColor;
      objectBuilder.LightFalloff = this.LightFalloff;
      objectBuilder.LightRadius = this.LightRadius;
      objectBuilder.LightGlareSize = this.LightGlareSize;
      objectBuilder.LightGlareIntensity = this.LightGlareIntensity;
      objectBuilder.LightIntensityLower = this.LightIntensityLower;
      objectBuilder.LightIntensityUpper = this.LightIntensityUpper;
      objectBuilder.ShakeAmountTarget = this.ShakeAmountTarget;
      objectBuilder.ShakeAmountNoTarget = this.ShakeAmountNoTarget;
      objectBuilder.ToolSounds = this.ToolSounds;
      objectBuilder.ToolMaterial = this.ToolMaterial.ToString();
      objectBuilder.ItemPositioning = this.ItemPositioning;
      objectBuilder.ItemPositioning3rd = this.ItemPositioning3rd;
      objectBuilder.ItemPositioningWalk = this.ItemPositioningWalk;
      objectBuilder.ItemPositioningWalk3rd = this.ItemPositioningWalk3rd;
      objectBuilder.ItemPositioningShoot = this.ItemPositioningShoot;
      objectBuilder.ItemPositioningShoot3rd = this.ItemPositioningShoot3rd;
      objectBuilder.ItemPositioningIronsight = this.ItemPositioningIronsight;
      objectBuilder.ItemPositioningIronsight3rd = this.ItemPositioningIronsight3rd;
      objectBuilder.WeaponType = this.WeaponType;
      return (MyObjectBuilder_DefinitionBase) objectBuilder;
    }

    private class Sandbox_Definitions_MyHandItemDefinition\u003C\u003EActor : IActivator, IActivator<MyHandItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyHandItemDefinition();

      MyHandItemDefinition IActivator<MyHandItemDefinition>.CreateInstance() => new MyHandItemDefinition();
    }
  }
}
