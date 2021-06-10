// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GuiControlCompositePanel
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
  public class MyObjectBuilder_GuiControlCompositePanel : MyObjectBuilder_GuiControlPanel
  {
    [ProtoMember(1)]
    public float InnerHeight;

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003EInnerHeight\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in float value) => owner.InnerHeight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out float value) => value = owner.InnerHeight;
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003EBackgroundColor\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EBackgroundColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in Vector4 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out Vector4 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003EControlTexture\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlTexture\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003EOriginAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EOriginAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlCompositePanel owner,
        in MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlCompositePanel owner,
        out MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlCompositePanel owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlCompositePanel owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003EControlAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in int value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out int value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlCompositePanel owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlCompositePanel owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlCompositePanel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlCompositePanel owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlCompositePanel owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GuiControlCompositePanel\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlCompositePanel>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiControlCompositePanel();

      MyObjectBuilder_GuiControlCompositePanel IActivator<MyObjectBuilder_GuiControlCompositePanel>.CreateInstance() => new MyObjectBuilder_GuiControlCompositePanel();
    }
  }
}
