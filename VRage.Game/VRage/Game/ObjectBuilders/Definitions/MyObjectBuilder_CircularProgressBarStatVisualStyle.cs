// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_CircularProgressBarStatVisualStyle
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.GUI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_CircularProgressBarStatVisualStyle : MyObjectBuilder_StatVisualStyle
  {
    public Vector2 SegmentSizePx;
    public MyStringHash SegmentTexture;
    public MyStringHash? BackgroudTexture;
    public MyStringHash? FirstSegmentTexture;
    public MyStringHash? LastSegmentTexture;
    public Vector2? SegmentOrigin;
    public float? SpacingAngle;
    public float? AngleOffset;
    public bool? Animate;
    public int? NumberOfSegments;
    public bool? ShowEmptySegments;
    public Vector4? EmptySegmentColorMask;
    public Vector4? FullSegmentColorMask;
    public Vector4? AnimatedSegmentColorMask;
    public double? AnimationDelayMs;
    public double? AnimationSegmentDelayMs;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ESegmentSizePx\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in Vector2 value)
      {
        owner.SegmentSizePx = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out Vector2 value)
      {
        value = owner.SegmentSizePx;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ESegmentTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in MyStringHash value)
      {
        owner.SegmentTexture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out MyStringHash value)
      {
        value = owner.SegmentTexture;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EBackgroudTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in MyStringHash? value)
      {
        owner.BackgroudTexture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out MyStringHash? value)
      {
        value = owner.BackgroudTexture;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EFirstSegmentTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in MyStringHash? value)
      {
        owner.FirstSegmentTexture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out MyStringHash? value)
      {
        value = owner.FirstSegmentTexture;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ELastSegmentTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in MyStringHash? value)
      {
        owner.LastSegmentTexture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out MyStringHash? value)
      {
        value = owner.LastSegmentTexture;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ESegmentOrigin\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector2?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in Vector2? value)
      {
        owner.SegmentOrigin = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out Vector2? value)
      {
        value = owner.SegmentOrigin;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ESpacingAngle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in float? value)
      {
        owner.SpacingAngle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out float? value)
      {
        value = owner.SpacingAngle;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EAngleOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in float? value)
      {
        owner.AngleOffset = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out float? value)
      {
        value = owner.AngleOffset;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EAnimate\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in bool? value)
      {
        owner.Animate = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out bool? value)
      {
        value = owner.Animate;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ENumberOfSegments\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in int? value)
      {
        owner.NumberOfSegments = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out int? value)
      {
        value = owner.NumberOfSegments;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EShowEmptySegments\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in bool? value)
      {
        owner.ShowEmptySegments = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out bool? value)
      {
        value = owner.ShowEmptySegments;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EEmptySegmentColorMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector4?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in Vector4? value)
      {
        owner.EmptySegmentColorMask = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out Vector4? value)
      {
        value = owner.EmptySegmentColorMask;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EFullSegmentColorMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector4?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in Vector4? value)
      {
        owner.FullSegmentColorMask = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out Vector4? value)
      {
        value = owner.FullSegmentColorMask;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EAnimatedSegmentColorMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector4?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in Vector4? value)
      {
        owner.AnimatedSegmentColorMask = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out Vector4? value)
      {
        value = owner.AnimatedSegmentColorMask;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EAnimationDelayMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, double?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in double? value)
      {
        owner.AnimationDelayMs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out double? value)
      {
        value = owner.AnimationDelayMs;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EAnimationSegmentDelayMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, double?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in double? value)
      {
        owner.AnimationSegmentDelayMs = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out double? value)
      {
        value = owner.AnimationSegmentDelayMs;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in uint? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out uint? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in uint? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out uint? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in uint? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out uint? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyAlphaBlinkBehavior>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in MyAlphaBlinkBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out MyAlphaBlinkBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, VisualStyleCategory?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in VisualStyleCategory? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out VisualStyleCategory? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_CircularProgressBarStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_CircularProgressBarStatVisualStyle owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_CircularProgressBarStatVisualStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_CircularProgressBarStatVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_CircularProgressBarStatVisualStyle();

      MyObjectBuilder_CircularProgressBarStatVisualStyle IActivator<MyObjectBuilder_CircularProgressBarStatVisualStyle>.CreateInstance() => new MyObjectBuilder_CircularProgressBarStatVisualStyle();
    }
  }
}
