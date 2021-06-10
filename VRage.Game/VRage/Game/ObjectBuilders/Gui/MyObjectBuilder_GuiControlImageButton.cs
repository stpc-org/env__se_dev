// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Gui.MyObjectBuilder_GuiControlImageButton
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ObjectBuilders.Gui
{
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_GuiControlImageButton : MyObjectBuilder_GuiControlBase
  {
    public string Text;
    public string TextEnum;
    public float TextScale;
    public int TextAlignment;
    public bool DrawCrossTextureWhenDisabled;
    public bool DrawRedTextureWhenDisabled;

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlImageButton, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out string value) => value = owner.Text;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003ETextEnum\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlImageButton, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in string value) => owner.TextEnum = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out string value) => value = owner.TextEnum;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003ETextScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlImageButton, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in float value) => owner.TextScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out float value) => value = owner.TextScale;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003ETextAlignment\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlImageButton, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in int value) => owner.TextAlignment = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out int value) => value = owner.TextAlignment;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EDrawCrossTextureWhenDisabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlImageButton, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in bool value) => owner.DrawCrossTextureWhenDisabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out bool value) => value = owner.DrawCrossTextureWhenDisabled;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EDrawRedTextureWhenDisabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlImageButton, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in bool value) => owner.DrawRedTextureWhenDisabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out bool value) => value = owner.DrawRedTextureWhenDisabled;
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EBackgroundColor\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EBackgroundColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in Vector4 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out Vector4 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EControlTexture\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlTexture\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EOriginAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EOriginAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlImageButton owner,
        in MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlImageButton owner,
        out MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlImageButton owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlImageButton owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EControlAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in int value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out int value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlImageButton owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlImageButton owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlImageButton, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlImageButton owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlImageButton owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Gui_MyObjectBuilder_GuiControlImageButton\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlImageButton>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiControlImageButton();

      MyObjectBuilder_GuiControlImageButton IActivator<MyObjectBuilder_GuiControlImageButton>.CreateInstance() => new MyObjectBuilder_GuiControlImageButton();
    }
  }
}
