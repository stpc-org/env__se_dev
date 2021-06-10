// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_TextStatVisualStyle
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
  public class MyObjectBuilder_TextStatVisualStyle : MyObjectBuilder_StatVisualStyle
  {
    public string Text;
    public float Scale;
    public string Font;
    public Vector4? ColorMask;
    public MyGuiDrawAlignEnum? TextAlign;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out string value) => value = owner.Text;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in float value) => owner.Scale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out float value) => value = owner.Scale;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EFont\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in string value) => owner.Font = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out string value) => value = owner.Font;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EColorMask\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, Vector4?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in Vector4? value) => owner.ColorMask = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out Vector4? value) => value = owner.ColorMask;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003ETextAlign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyGuiDrawAlignEnum?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TextStatVisualStyle owner,
        in MyGuiDrawAlignEnum? value)
      {
        owner.TextAlign = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TextStatVisualStyle owner,
        out MyGuiDrawAlignEnum? value)
      {
        value = owner.TextAlign;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out MyStringHash value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in ConditionBase value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TextStatVisualStyle owner,
        out ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in ConditionBase value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TextStatVisualStyle owner,
        out ConditionBase value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in Vector2 value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out Vector2 value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in Vector2 value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out Vector2 value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in uint? value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out uint? value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in uint? value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out uint? value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in uint? value) => this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out uint? value) => this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyAlphaBlinkBehavior>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TextStatVisualStyle owner,
        in MyAlphaBlinkBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TextStatVisualStyle owner,
        out MyAlphaBlinkBehavior value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor : MyObjectBuilder_StatVisualStyle.VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, VisualStyleCategory?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_TextStatVisualStyle owner,
        in VisualStyleCategory? value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_StatVisualStyle&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_TextStatVisualStyle owner,
        out VisualStyleCategory? value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_StatVisualStyle&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_TextStatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_TextStatVisualStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_TextStatVisualStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_TextStatVisualStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_TextStatVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_TextStatVisualStyle();

      MyObjectBuilder_TextStatVisualStyle IActivator<MyObjectBuilder_TextStatVisualStyle>.CreateInstance() => new MyObjectBuilder_TextStatVisualStyle();
    }
  }
}
