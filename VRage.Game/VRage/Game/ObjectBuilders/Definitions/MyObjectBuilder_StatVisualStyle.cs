// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_StatVisualStyle
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
  public class MyObjectBuilder_StatVisualStyle : MyObjectBuilder_Base
  {
    public MyStringHash StatId;
    [XmlElement(typeof (MyAbstractXmlSerializer<ConditionBase>))]
    public ConditionBase VisibleCondition;
    [XmlElement(typeof (MyAbstractXmlSerializer<ConditionBase>))]
    public ConditionBase BlinkCondition;
    public Vector2 SizePx;
    public Vector2 OffsetPx;
    public uint? FadeInTimeMs;
    public uint? FadeOutTimeMs;
    public uint? MaxOnScreenTimeMs;
    public MyAlphaBlinkBehavior Blink;
    public VisualStyleCategory? Category;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EStatId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in MyStringHash value) => owner.StatId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out MyStringHash value) => value = owner.StatId;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in ConditionBase value) => owner.VisibleCondition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out ConditionBase value) => value = owner.VisibleCondition;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlinkCondition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in ConditionBase value) => owner.BlinkCondition = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out ConditionBase value) => value = owner.BlinkCondition;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in Vector2 value) => owner.SizePx = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out Vector2 value) => value = owner.SizePx;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in Vector2 value) => owner.OffsetPx = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out Vector2 value) => value = owner.OffsetPx;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeInTimeMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in uint? value) => owner.FadeInTimeMs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out uint? value) => value = owner.FadeInTimeMs;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EFadeOutTimeMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in uint? value) => owner.FadeOutTimeMs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out uint? value) => value = owner.FadeOutTimeMs;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EMaxOnScreenTimeMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, uint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in uint? value) => owner.MaxOnScreenTimeMs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out uint? value) => value = owner.MaxOnScreenTimeMs;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EBlink\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, MyAlphaBlinkBehavior>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_StatVisualStyle owner,
        in MyAlphaBlinkBehavior value)
      {
        owner.Blink = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_StatVisualStyle owner,
        out MyAlphaBlinkBehavior value)
      {
        value = owner.Blink;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ECategory\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_StatVisualStyle, VisualStyleCategory?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_StatVisualStyle owner,
        in VisualStyleCategory? value)
      {
        owner.Category = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_StatVisualStyle owner,
        out VisualStyleCategory? value)
      {
        value = owner.Category;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StatVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_StatVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_StatVisualStyle owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_StatVisualStyle owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_StatVisualStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_StatVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_StatVisualStyle();

      MyObjectBuilder_StatVisualStyle IActivator<MyObjectBuilder_StatVisualStyle>.CreateInstance() => new MyObjectBuilder_StatVisualStyle();
    }
  }
}
