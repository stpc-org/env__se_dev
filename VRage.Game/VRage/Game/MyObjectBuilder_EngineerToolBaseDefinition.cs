// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_EngineerToolBaseDefinition
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
  public class MyObjectBuilder_EngineerToolBaseDefinition : MyObjectBuilder_HandItemDefinition
  {
    [ProtoMember(1)]
    [DefaultValue(1)]
    public float SpeedMultiplier = 1f;
    [ProtoMember(4)]
    [DefaultValue(1)]
    public float DistanceMultiplier = 1f;
    public string Flare = "Welder";

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ESpeedMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        owner.SpeedMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        value = owner.SpeedMultiplier;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EDistanceMultiplier\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        owner.DistanceMultiplier = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        value = owner.DistanceMultiplier;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EFlare\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string value)
      {
        owner.Flare = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string value)
      {
        value = owner.Flare;
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELeftHandOrientation\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELeftHandOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELeftHandPosition\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELeftHandPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ERightHandOrientation\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERightHandOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ERightHandPosition\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERightHandPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemOrientation\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPosition\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemWalkingOrientation\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemWalkingOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemWalkingPosition\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemWalkingPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemShootOrientation\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemShootOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemShootPosition\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemShootPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemIronsightOrientation\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemIronsightOrientation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemIronsightPosition\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemIronsightPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemOrientation3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemOrientation3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPosition3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPosition3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemWalkingOrientation3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemWalkingOrientation3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemWalkingPosition3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemWalkingPosition3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemShootOrientation3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemShootOrientation3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Quaternion value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemShootPosition3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemShootPosition3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EBlendTime\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EBlendTime\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EShootBlend\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EShootBlend\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EXAmplitudeOffset\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EXAmplitudeOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EYAmplitudeOffset\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EYAmplitudeOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EZAmplitudeOffset\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EZAmplitudeOffset\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EXAmplitudeScale\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EXAmplitudeScale\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EYAmplitudeScale\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EYAmplitudeScale\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EZAmplitudeScale\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EZAmplitudeScale\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ERunMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERunMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EAmplitudeMultiplier3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EAmplitudeMultiplier3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ESimulateLeftHand\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESimulateLeftHand\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ESimulateRightHand\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESimulateRightHand\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ESimulateLeftHandFps\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESimulateLeftHandFps\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in bool? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out bool? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ESimulateRightHandFps\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESimulateRightHandFps\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in bool? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out bool? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EFingersAnimation\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EFingersAnimation\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EMuzzlePosition\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EMuzzlePosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EShootScatter\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EShootScatter\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector3 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EScatterSpeed\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EScatterSpeed\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EPhysicalItemId\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EPhysicalItemId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELightColor\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in Vector4 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out Vector4 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELightFalloff\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightFalloff\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELightRadius\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightRadius\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELightGlareSize\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightGlareSize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELightGlareIntensity\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightGlareIntensity\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELightIntensityLower\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightIntensityLower\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ELightIntensityUpper\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ELightIntensityUpper\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EShakeAmountTarget\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EShakeAmountTarget\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EShakeAmountNoTarget\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EShakeAmountNoTarget\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EToolSounds\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EToolSounds\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, List<ToolSound>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in List<ToolSound> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out List<ToolSound> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EToolMaterial\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EToolMaterial\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPositioning\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioning\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPositioning3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioning3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPositioningWalk\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningWalk\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPositioningWalk3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningWalk3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPositioningShoot\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningShoot\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPositioningShoot3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningShoot3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPositioningIronsight\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningIronsight\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EItemPositioningIronsight3rd\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EItemPositioningIronsight3rd\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemPositioningEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemPositioningEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ESprintSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ESprintSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ERunSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERunSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EBackrunSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EBackrunSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ERunStrafingSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ERunStrafingSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EWalkSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EWalkSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EBackwalkSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EBackwalkSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EWalkStrafingSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EWalkStrafingSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ECrouchWalkSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ECrouchWalkSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ECrouchBackwalkSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ECrouchBackwalkSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ECrouchStrafingSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003ECrouchStrafingSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EAimingSpeedMultiplier\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EAimingSpeedMultiplier\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in float value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out float value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EWeaponType\u003C\u003EAccessor : MyObjectBuilder_HandItemDefinition.VRage_Game_MyObjectBuilder_HandItemDefinition\u003C\u003EWeaponType\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyItemWeaponType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyItemWeaponType value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_HandItemDefinition&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyItemWeaponType value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_HandItemDefinition&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EId\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, SerializableDefinitionId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out SerializableDefinitionId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EDisplayName\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDisplayName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EDescription\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescription\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EPublic\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EPublic\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EEnabled\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EEnabled\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EAvailableInSurvival\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EAvailableInSurvival\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EDescriptionArgs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDescriptionArgs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EDLCs\u003C\u003EAccessor : MyObjectBuilder_DefinitionBase.VRage_Game_MyObjectBuilder_DefinitionBase\u003C\u003EDLCs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string[] value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_DefinitionBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string[] value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_DefinitionBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_EngineerToolBaseDefinition, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_EngineerToolBaseDefinition owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_EngineerToolBaseDefinition\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_EngineerToolBaseDefinition>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_EngineerToolBaseDefinition();

      MyObjectBuilder_EngineerToolBaseDefinition IActivator<MyObjectBuilder_EngineerToolBaseDefinition>.CreateInstance() => new MyObjectBuilder_EngineerToolBaseDefinition();
    }
  }
}
