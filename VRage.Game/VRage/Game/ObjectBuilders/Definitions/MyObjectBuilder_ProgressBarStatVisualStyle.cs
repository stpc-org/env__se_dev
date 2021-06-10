// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ProgressBarStatVisualStyle
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
  public class MyObjectBuilder_ProgressBarStatVisualStyle : MyObjectBuilder_StatVisualStyle
  {
    public MyObjectBuilder_ProgressBarStatVisualStyle.SimpleBarData? SimpleStyle;
    public MyObjectBuilder_ProgressBarStatVisualStyle.NineTiledData? NineTiledStyle;
    public bool? Inverted;

    public struct SimpleBarData
    {
      public MyStringHash BackgroundTexture;
      public MyStringHash ProgressTexture;
      public Vector4? BackgroundColorMask;
      public Vector4? ProgressColorMask;
      public Vector2I ProgressTextureOffsetPx;
    }

    public struct NineTiledData
    {
      public MyStringHash Texture;
      public Vector4? ColorMask;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003ESimpleStyle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyObjectBuilder_ProgressBarStatVisualStyle.SimpleBarData?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in MyObjectBuilder_ProgressBarStatVisualStyle.SimpleBarData? value)
      {
        owner.SimpleStyle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out MyObjectBuilder_ProgressBarStatVisualStyle.SimpleBarData? value)
      {
        value = owner.SimpleStyle;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003ENineTiledStyle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyObjectBuilder_ProgressBarStatVisualStyle.NineTiledData?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in MyObjectBuilder_ProgressBarStatVisualStyle.NineTiledData? value)
      {
        owner.NineTiledStyle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out MyObjectBuilder_ProgressBarStatVisualStyle.NineTiledData? value)
      {
        value = owner.NineTiledStyle;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EInverted\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, bool?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in bool? value)
      {
        owner.Inverted = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out bool? value)
      {
        value = owner.Inverted;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in uint? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out uint? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in uint? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out uint? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in uint? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out uint? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyAlphaBlinkBehavior>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in MyAlphaBlinkBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out MyAlphaBlinkBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, VisualStyleCategory?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in VisualStyleCategory? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out VisualStyleCategory? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ProgressBarStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ProgressBarStatVisualStyle owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ProgressBarStatVisualStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ProgressBarStatVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ProgressBarStatVisualStyle();

      MyObjectBuilder_ProgressBarStatVisualStyle IActivator<MyObjectBuilder_ProgressBarStatVisualStyle>.CreateInstance() => new MyObjectBuilder_ProgressBarStatVisualStyle();
    }
  }
}
