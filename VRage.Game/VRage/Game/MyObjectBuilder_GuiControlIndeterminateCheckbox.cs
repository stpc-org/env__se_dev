// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GuiControlIndeterminateCheckbox
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
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
  public class MyObjectBuilder_GuiControlIndeterminateCheckbox : MyObjectBuilder_GuiControlBase
  {
    [ProtoMember(1)]
    public CheckStateEnum State;
    [ProtoMember(4)]
    public MyGuiControlIndeterminateCheckboxStyleEnum VisualStyle;

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EState\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, CheckStateEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in CheckStateEnum value)
      {
        owner.State = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out CheckStateEnum value)
      {
        value = owner.State;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EVisualStyle\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, MyGuiControlIndeterminateCheckboxStyleEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in MyGuiControlIndeterminateCheckboxStyleEnum value)
      {
        owner.VisualStyle = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out MyGuiControlIndeterminateCheckboxStyleEnum value)
      {
        value = owner.VisualStyle;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out Vector2 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EBackgroundColor\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EBackgroundColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in Vector4 value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out Vector4 value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EControlTexture\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlTexture\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EOriginAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EOriginAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EControlAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in int value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out int value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlIndeterminateCheckbox, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlIndeterminateCheckbox owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_MyObjectBuilder_GuiControlIndeterminateCheckbox\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlIndeterminateCheckbox>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiControlIndeterminateCheckbox();

      MyObjectBuilder_GuiControlIndeterminateCheckbox IActivator<MyObjectBuilder_GuiControlIndeterminateCheckbox>.CreateInstance() => new MyObjectBuilder_GuiControlIndeterminateCheckbox();
    }
  }
}
