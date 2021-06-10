// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_ToolbarControlVisualStyle
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
  public class MyObjectBuilder_ToolbarControlVisualStyle : MyObjectBuilder_Base
  {
    [XmlElement(typeof (MyAbstractXmlSerializer<ConditionBase>))]
    public ConditionBase VisibleCondition;
    public MyObjectBuilder_ToolbarControlVisualStyle.ColorStyle ColorPanelStyle;
    public Vector2 CenterPosition;
    public Vector2 SelectedItemPosition;
    public float? SelectedItemTextScale;
    public MyGuiDrawAlignEnum OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM;
    public MyObjectBuilder_ToolbarControlVisualStyle.ToolbarItemStyle ItemStyle;
    public MyObjectBuilder_ToolbarControlVisualStyle.ToolbarPageStyle PageStyle;
    [XmlArrayItem("StatControl")]
    public MyObjectBuilder_StatControls[] StatControls;

    public class ToolbarItemStyle
    {
      public MyStringHash Texture = MyStringHash.GetOrCompute("Textures\\GUI\\Controls\\grid_item.dds");
      public MyStringHash TextureHighlight = MyStringHash.GetOrCompute("Textures\\GUI\\Controls\\grid_item_highlight.dds");
      public MyStringHash TextureFocus = MyStringHash.GetOrCompute("Textures\\GUI\\Controls\\grid_item_focus.dds");
      public MyStringHash TextureActive = MyStringHash.GetOrCompute("Textures\\GUI\\Controls\\grid_item_active.dds");
      public MyStringHash VariantTexture = MyStringHash.GetOrCompute("Textures\\GUI\\Icons\\VariantsAvailable.dds");
      public Vector2? VariantOffset;
      public string FontNormal = "Blue";
      public string FontHighlight = "White";
      public float TextScale = 0.75f;
      public Vector2 ItemTextureScale = Vector2.Zero;
      public MyGuiOffset? Margin;
    }

    public class ToolbarPageStyle
    {
      public MyStringHash PageCompositeTexture;
      public MyStringHash PageHighlightCompositeTexture;
      public Vector2 PagesOffset;
      public float? NumberSize;
    }

    public class ColorStyle
    {
      public Vector2 Offset;
      public Vector2 VoxelHandPosition;
      public Vector2 Size;
      public MyStringHash Texture;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003EVisibleCondition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, ConditionBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in ConditionBase value)
      {
        owner.VisibleCondition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out ConditionBase value)
      {
        value = owner.VisibleCondition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003EColorPanelStyle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyObjectBuilder_ToolbarControlVisualStyle.ColorStyle>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in MyObjectBuilder_ToolbarControlVisualStyle.ColorStyle value)
      {
        owner.ColorPanelStyle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out MyObjectBuilder_ToolbarControlVisualStyle.ColorStyle value)
      {
        value = owner.ColorPanelStyle;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003ECenterPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in Vector2 value)
      {
        owner.CenterPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out Vector2 value)
      {
        value = owner.CenterPosition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003ESelectedItemPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in Vector2 value)
      {
        owner.SelectedItemPosition = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out Vector2 value)
      {
        value = owner.SelectedItemPosition;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003ESelectedItemTextScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in float? value)
      {
        owner.SelectedItemTextScale = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out float? value)
      {
        value = owner.SelectedItemTextScale;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003EOriginAlign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in MyGuiDrawAlignEnum value)
      {
        owner.OriginAlign = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out MyGuiDrawAlignEnum value)
      {
        value = owner.OriginAlign;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003EItemStyle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyObjectBuilder_ToolbarControlVisualStyle.ToolbarItemStyle>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in MyObjectBuilder_ToolbarControlVisualStyle.ToolbarItemStyle value)
      {
        owner.ItemStyle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out MyObjectBuilder_ToolbarControlVisualStyle.ToolbarItemStyle value)
      {
        value = owner.ItemStyle;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003EPageStyle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyObjectBuilder_ToolbarControlVisualStyle.ToolbarPageStyle>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in MyObjectBuilder_ToolbarControlVisualStyle.ToolbarPageStyle value)
      {
        owner.PageStyle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out MyObjectBuilder_ToolbarControlVisualStyle.ToolbarPageStyle value)
      {
        value = owner.PageStyle;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003EStatControls\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyObjectBuilder_StatControls[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in MyObjectBuilder_StatControls[] value)
      {
        owner.StatControls = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out MyObjectBuilder_StatControls[] value)
      {
        value = owner.StatControls;
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_ToolbarControlVisualStyle, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_ToolbarControlVisualStyle owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_ToolbarControlVisualStyle\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_ToolbarControlVisualStyle>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_ToolbarControlVisualStyle();

      MyObjectBuilder_ToolbarControlVisualStyle IActivator<MyObjectBuilder_ToolbarControlVisualStyle>.CreateInstance() => new MyObjectBuilder_ToolbarControlVisualStyle();
    }
  }
}
