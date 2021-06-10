// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ImageStatVisualStyle
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
  public class MyObjectBuilder_ImageStatVisualStyle : MyObjectBuilder_StatVisualStyle
  {
    public MyStringHash Texture;
    public Vector4? ColorMask;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003ETexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyStringHash value) => owner.Texture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        out MyStringHash value)
      {
        value = owner.Texture;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EColorMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, Vector4?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in Vector4? value) => owner.ColorMask = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out Vector4? value) => value = owner.ColorMask;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        in ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        out ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        in ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        out ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in Vector2 value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out Vector2 value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in Vector2 value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out Vector2 value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in uint? value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out uint? value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in uint? value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out uint? value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in uint? value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out uint? value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyAlphaBlinkBehavior>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        in MyAlphaBlinkBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        out MyAlphaBlinkBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, VisualStyleCategory?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        in VisualStyleCategory? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        out VisualStyleCategory? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ImageStatVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ImageStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_ImageStatVisualStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_ImageStatVisualStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ImageStatVisualStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ImageStatVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ImageStatVisualStyle();

      MyObjectBuilder_ImageStatVisualStyle IActivator<MyObjectBuilder_ImageStatVisualStyle>.CreateInstance() => new MyObjectBuilder_ImageStatVisualStyle();
    }
  }
}
