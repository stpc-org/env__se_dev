﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_HandItemDefinition
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_HandItemDefinition : MyObjectBuilder_DefinitionBase
  {
    [ProtoMember(1)]
    public Quaternion LeftHandOrientation = Quaternion.Identity;
    [ProtoMember(4)]
    public Vector3 LeftHandPosition;
    [ProtoMember(7)]
    public Quaternion RightHandOrientation = Quaternion.Identity;
    [ProtoMember(10)]
    public Vector3 RightHandPosition;
    [ProtoMember(13)]
    public Quaternion ItemOrientation = Quaternion.Identity;
    [ProtoMember(16)]
    public Vector3 ItemPosition;
    [ProtoMember(19)]
    public Quaternion ItemWalkingOrientation = Quaternion.Identity;
    [ProtoMember(22)]
    public Vector3 ItemWalkingPosition;
    [ProtoMember(25)]
    public Quaternion ItemShootOrientation = Quaternion.Identity;
    [ProtoMember(28)]
    public Vector3 ItemShootPosition;
    [ProtoMember(31)]
    public Quaternion ItemIronsightOrientation = Quaternion.Identity;
    [ProtoMember(34)]
    public Vector3 ItemIronsightPosition;
    [ProtoMember(37)]
    public Quaternion ItemOrientation3rd = Quaternion.Identity;
    [ProtoMember(40)]
    public Vector3 ItemPosition3rd;
    [ProtoMember(43)]
    public Quaternion ItemWalkingOrientation3rd = Quaternion.Identity;
    [ProtoMember(46)]
    public Vector3 ItemWalkingPosition3rd;
    [ProtoMember(49)]
    public Quaternion ItemShootOrientation3rd = Quaternion.Identity;
    [ProtoMember(52)]
    public Vector3 ItemShootPosition3rd;
    [ProtoMember(55)]
    public float BlendTime;
    [ProtoMember(58)]
    public float ShootBlend;
    [ProtoMember(61)]
    public float XAmplitudeOffset;
    [ProtoMember(64)]
    public float YAmplitudeOffset;
    [ProtoMember(67)]
    public float ZAmplitudeOffset;
    [ProtoMember(70)]
    public float XAmplitudeScale;
    [ProtoMember(73)]
    public float YAmplitudeScale;
    [ProtoMember(76)]
    public float ZAmplitudeScale;
    [ProtoMember(79)]
    public float RunMultiplier;
    [ProtoMember(82)]
    public float AmplitudeMultiplier3rd;
    [ProtoMember(85)]
    [DefaultValue(true)]
    public bool SimulateLeftHand = true;
    [ProtoMember(88)]
    [DefaultValue(true)]
    public bool SimulateRightHand = true;
    [ProtoMember(91)]
    public bool? SimulateLeftHandFps;
    [ProtoMember(94)]
    public bool? SimulateRightHandFps;
    [ProtoMember(97)]
    public string FingersAnimation;
    [ProtoMember(100)]
    public Vector3 MuzzlePosition;
    [ProtoMember(103)]
    public Vector3 ShootScatter;
    [ProtoMember(106)]
    public float ScatterSpeed;
    [ProtoMember(109)]
    public SerializableDefinitionId PhysicalItemId;
    [ProtoMember(112)]
    public Vector4 LightColor;
    [ProtoMember(115)]
    public float LightFalloff;
    [ProtoMember(118)]
    public float LightRadius;
    [ProtoMember(121)]
    public float LightGlareSize;
    [ProtoMember(124)]
    public float LightGlareIntensity = 1f;
    [ProtoMember(127)]
    public float LightIntensityLower;
    [ProtoMember(130)]
    public float LightIntensityUpper;
    [ProtoMember(133)]
    public float ShakeAmountTarget;
    [ProtoMember(136)]
    public float ShakeAmountNoTarget;
    [ProtoMember(139)]
    public List<ToolSound> ToolSounds;
    [ProtoMember(142)]
    public string ToolMaterial = "Grinder";
    [ProtoMember(145)]
    public MyItemPositioningEnum ItemPositioning;
    [ProtoMember(148)]
    public MyItemPositioningEnum ItemPositioning3rd;
    [ProtoMember(151)]
    public MyItemPositioningEnum ItemPositioningWalk;
    [ProtoMember(154)]
    public MyItemPositioningEnum ItemPositioningWalk3rd;
    [ProtoMember(157)]
    public MyItemPositioningEnum ItemPositioningShoot;
    [ProtoMember(160)]
    public MyItemPositioningEnum ItemPositioningShoot3rd;
    [ProtoMember(163)]
    public MyItemPositioningEnum ItemPositioningIronsight;
    [ProtoMember(166)]
    public MyItemPositioningEnum ItemPositioningIronsight3rd;
    [ProtoMember(170)]
    public float SprintSpeedMultiplier = 1f;
    [ProtoMember(171)]
    public float RunSpeedMultiplier = 1f;
    [ProtoMember(172)]
    public float BackrunSpeedMultiplier = 1f;
    [ProtoMember(173)]
    public float RunStrafingSpeedMultiplier = 1f;
    [ProtoMember(174)]
    public float WalkSpeedMultiplier = 1f;
    [ProtoMember(175)]
    public float BackwalkSpeedMultiplier = 1f;
    [ProtoMember(176)]
    public float WalkStrafingSpeedMultiplier = 1f;
    [ProtoMember(177)]
    public float CrouchWalkSpeedMultiplier = 1f;
    [ProtoMember(178)]
    public float CrouchBackwalkSpeedMultiplier = 1f;
    [ProtoMember(179)]
    public float CrouchStrafingSpeedMultiplier = 1f;
    [ProtoMember(180)]
    public float AimingSpeedMultiplier = 1f;
    [ProtoMember(182)]
    public MyItemWeaponType WeaponType;

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELeftHandOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.LeftHandOrientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.LeftHandOrientation;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELeftHandPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.LeftHandPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.LeftHandPosition;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERightHandOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.RightHandOrientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.RightHandOrientation;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERightHandPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.RightHandPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.RightHandPosition;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.ItemOrientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.ItemOrientation;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.ItemPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.ItemPosition;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemWalkingOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.ItemWalkingOrientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.ItemWalkingOrientation;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemWalkingPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.ItemWalkingPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.ItemWalkingPosition;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemShootOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.ItemShootOrientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.ItemShootOrientation;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemShootPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.ItemShootPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.ItemShootPosition;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemIronsightOrientation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.ItemIronsightOrientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.ItemIronsightOrientation;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemIronsightPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.ItemIronsightPosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.ItemIronsightPosition;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemOrientation3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.ItemOrientation3rd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.ItemOrientation3rd;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPosition3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.ItemPosition3rd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.ItemPosition3rd;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemWalkingOrientation3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.ItemWalkingOrientation3rd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.ItemWalkingOrientation3rd;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemWalkingPosition3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.ItemWalkingPosition3rd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.ItemWalkingPosition3rd;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemShootOrientation3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Quaternion value) => owner.ItemShootOrientation3rd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Quaternion value) => value = owner.ItemShootOrientation3rd;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemShootPosition3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.ItemShootPosition3rd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.ItemShootPosition3rd;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EBlendTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.BlendTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.BlendTime;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EShootBlend\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.ShootBlend = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.ShootBlend;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EXAmplitudeOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.XAmplitudeOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.XAmplitudeOffset;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EYAmplitudeOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.YAmplitudeOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.YAmplitudeOffset;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EZAmplitudeOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.ZAmplitudeOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.ZAmplitudeOffset;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EXAmplitudeScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.XAmplitudeScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.XAmplitudeScale;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EYAmplitudeScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.YAmplitudeScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.YAmplitudeScale;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EZAmplitudeScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.ZAmplitudeScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.ZAmplitudeScale;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERunMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.RunMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.RunMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EAmplitudeMultiplier3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.AmplitudeMultiplier3rd = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.AmplitudeMultiplier3rd;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESimulateLeftHand\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value) => owner.SimulateLeftHand = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value) => value = owner.SimulateLeftHand;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESimulateRightHand\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value) => owner.SimulateRightHand = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value) => value = owner.SimulateRightHand;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESimulateLeftHandFps\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool? value) => owner.SimulateLeftHandFps = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool? value) => value = owner.SimulateLeftHandFps;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESimulateRightHandFps\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool? value) => owner.SimulateRightHandFps = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool? value) => value = owner.SimulateRightHandFps;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EFingersAnimation\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value) => owner.FingersAnimation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value) => value = owner.FingersAnimation;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EMuzzlePosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.MuzzlePosition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.MuzzlePosition;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EShootScatter\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector3 value) => owner.ShootScatter = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector3 value) => value = owner.ShootScatter;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EScatterSpeed\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.ScatterSpeed = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.ScatterSpeed;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EPhysicalItemId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in SerializableDefinitionId value)
      {
        owner.PhysicalItemId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out SerializableDefinitionId value)
      {
        value = owner.PhysicalItemId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in Vector4 value) => owner.LightColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out Vector4 value) => value = owner.LightColor;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightFalloff\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.LightFalloff = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.LightFalloff;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightRadius\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.LightRadius = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.LightRadius;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightGlareSize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.LightGlareSize = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.LightGlareSize;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightGlareIntensity\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.LightGlareIntensity = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.LightGlareIntensity;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightIntensityLower\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.LightIntensityLower = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.LightIntensityLower;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightIntensityUpper\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.LightIntensityUpper = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.LightIntensityUpper;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EShakeAmountTarget\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.ShakeAmountTarget = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.ShakeAmountTarget;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EShakeAmountNoTarget\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.ShakeAmountNoTarget = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.ShakeAmountNoTarget;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EToolSounds\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, List<ToolSound>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in List<ToolSound> value)
      {
        owner.ToolSounds = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out List<ToolSound> value)
      {
        value = owner.ToolSounds;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EToolMaterial\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value) => owner.ToolMaterial = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value) => value = owner.ToolMaterial;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioning\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemPositioningEnum value)
      {
        owner.ItemPositioning = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemPositioningEnum value)
      {
        value = owner.ItemPositioning;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioning3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemPositioningEnum value)
      {
        owner.ItemPositioning3rd = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemPositioningEnum value)
      {
        value = owner.ItemPositioning3rd;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningWalk\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemPositioningEnum value)
      {
        owner.ItemPositioningWalk = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemPositioningEnum value)
      {
        value = owner.ItemPositioningWalk;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningWalk3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemPositioningEnum value)
      {
        owner.ItemPositioningWalk3rd = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemPositioningEnum value)
      {
        value = owner.ItemPositioningWalk3rd;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningShoot\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemPositioningEnum value)
      {
        owner.ItemPositioningShoot = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemPositioningEnum value)
      {
        value = owner.ItemPositioningShoot;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningShoot3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemPositioningEnum value)
      {
        owner.ItemPositioningShoot3rd = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemPositioningEnum value)
      {
        value = owner.ItemPositioningShoot3rd;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningIronsight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemPositioningEnum value)
      {
        owner.ItemPositioningIronsight = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemPositioningEnum value)
      {
        value = owner.ItemPositioningIronsight;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningIronsight3rd\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemPositioningEnum value)
      {
        owner.ItemPositioningIronsight3rd = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemPositioningEnum value)
      {
        value = owner.ItemPositioningIronsight3rd;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESprintSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.SprintSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.SprintSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERunSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.RunSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.RunSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EBackrunSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.BackrunSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.BackrunSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERunStrafingSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.RunStrafingSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.RunStrafingSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EWalkSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.WalkSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.WalkSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EBackwalkSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.BackwalkSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.BackwalkSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EWalkStrafingSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.WalkStrafingSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.WalkStrafingSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ECrouchWalkSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.CrouchWalkSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.CrouchWalkSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ECrouchBackwalkSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.CrouchBackwalkSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.CrouchBackwalkSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ECrouchStrafingSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.CrouchStrafingSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.CrouchStrafingSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EAimingSpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in float value) => owner.AimingSpeedMultiplier = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out float value) => value = owner.AimingSpeedMultiplier;
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EWeaponType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyItemWeaponType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in MyItemWeaponType value)
      {
        owner.WeaponType = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out MyItemWeaponType value)
      {
        value = owner.WeaponType;
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_HandItemDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_HandItemDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in bool value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out bool value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string[] value) => this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string[] value) => this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HandItemDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HandItemDefinition owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HandItemDefinition owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_HandItemDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_HandItemDefinition();

      MyObjectBuilder_HandItemDefinition IActivator<MyObjectBuilder_HandItemDefinition>.CreateInstance() => new MyObjectBuilder_HandItemDefinition();
    }
  }
}
