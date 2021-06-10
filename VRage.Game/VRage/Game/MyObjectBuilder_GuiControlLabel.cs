// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GuiControlLabel
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
  public class MyObjectBuilder_GuiControlLabel : MyObjectBuilder_GuiControlBase
  {
    [ProtoMember(1)]
    public string TextEnum;
    [ProtoMember(4)]
    public string Text;
    [ProtoMember(7)]
    public float TextScale;
    [ProtoMember(10)]
    public string Font;

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003ETextEnum\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlLabel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in string value) => owner.TextEnum = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out string value) => value = owner.TextEnum;
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EText\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlLabel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in string value) => owner.Text = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out string value) => value = owner.Text;
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003ETextScale\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlLabel, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in float value) => owner.TextScale = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out float value) => value = owner.TextScale;
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EFont\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiControlLabel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in string value) => owner.Font = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out string value) => value = owner.Font;
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EBackgroundColor\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EBackgroundColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in Vector4 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out Vector4 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EControlTexture\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlTexture\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EOriginAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EOriginAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlLabel owner,
        in MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlLabel owner,
        out MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EControlAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in int value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out int value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlLabel, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlLabel owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlLabel owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GuiControlLabel\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlLabel>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiControlLabel();

      MyObjectBuilder_GuiControlLabel IActivator<MyObjectBuilder_GuiControlLabel>.CreateInstance() => new MyObjectBuilder_GuiControlLabel();
    }
  }
}
