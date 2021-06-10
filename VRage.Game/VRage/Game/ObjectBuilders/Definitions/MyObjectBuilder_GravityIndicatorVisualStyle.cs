// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_GravityIndicatorVisualStyle
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Definitions
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_GravityIndicatorVisualStyle : MyObjectBuilder_Base
  {
    public Vector2 OffsetPx;
    public Vector2 SizePx;
    public Vector2 VelocitySizePx;
    public MyStringHash FillTexture;
    public MyStringHash OverlayTexture;
    public MyStringHash VelocityTexture;
    public MyGuiDrawAlignEnum OriginAlign;
    [XmlElement(typeof (MyAbstractXmlSerializer<ConditionBase>))]
    public ConditionBase VisibleCondition;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003EOffsetPx\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in Vector2 value)
      {
        owner.OffsetPx = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out Vector2 value)
      {
        value = owner.OffsetPx;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003ESizePx\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in Vector2 value)
      {
        owner.SizePx = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out Vector2 value)
      {
        value = owner.SizePx;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003EVelocitySizePx\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in Vector2 value)
      {
        owner.VelocitySizePx = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out Vector2 value)
      {
        value = owner.VelocitySizePx;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003EFillTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in MyStringHash value)
      {
        owner.FillTexture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out MyStringHash value)
      {
        value = owner.FillTexture;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003EOverlayTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in MyStringHash value)
      {
        owner.OverlayTexture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out MyStringHash value)
      {
        value = owner.OverlayTexture;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003EVelocityTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in MyStringHash value)
      {
        owner.VelocityTexture = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out MyStringHash value)
      {
        value = owner.VelocityTexture;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003EOriginAlign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in MyGuiDrawAlignEnum value)
      {
        owner.OriginAlign = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out MyGuiDrawAlignEnum value)
      {
        value = owner.OriginAlign;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in ConditionBase value)
      {
        owner.VisibleCondition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out ConditionBase value)
      {
        value = owner.VisibleCondition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GravityIndicatorVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GravityIndicatorVisualStyle owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_GravityIndicatorVisualStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GravityIndicatorVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GravityIndicatorVisualStyle();

      MyObjectBuilder_GravityIndicatorVisualStyle IActivator<MyObjectBuilder_GravityIndicatorVisualStyle>.CreateInstance() => new MyObjectBuilder_GravityIndicatorVisualStyle();
    }
  }
}
