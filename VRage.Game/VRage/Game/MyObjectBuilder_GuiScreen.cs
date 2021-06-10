// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GuiScreen
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
  public class MyObjectBuilder_GuiScreen : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public MyObjectBuilder_GuiControls Controls;
    [ProtoMember(4)]
    public Vector4? BackgroundColor;
    [ProtoMember(7)]
    public string BackgroundTexture;
    [ProtoMember(10)]
    public Vector2? Size;
    [ProtoMember(13)]
    public bool CloseButtonEnabled;
    [ProtoMember(16)]
    public Vector2 CloseButtonOffset;

    public bool ShouldSerializeCloseButtonOffset() => this.CloseButtonEnabled;

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003EControls\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, MyObjectBuilder_GuiControls>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiScreen owner,
        in MyObjectBuilder_GuiControls value)
      {
        owner.Controls = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiScreen owner,
        out MyObjectBuilder_GuiControls value)
      {
        value = owner.Controls;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003EBackgroundColor\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, Vector4?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in Vector4? value) => owner.BackgroundColor = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out Vector4? value) => value = owner.BackgroundColor;
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003EBackgroundTexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in string value) => owner.BackgroundTexture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out string value) => value = owner.BackgroundTexture;
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, Vector2?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in Vector2? value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out Vector2? value) => value = owner.Size;
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003ECloseButtonEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in bool value) => owner.CloseButtonEnabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out bool value) => value = owner.CloseButtonEnabled;
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003ECloseButtonOffset\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GuiScreen, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in Vector2 value) => owner.CloseButtonOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out Vector2 value) => value = owner.CloseButtonOffset;
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiScreen, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiScreen, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiScreen, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiScreen, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiScreen owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiScreen owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GuiScreen\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiScreen>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiScreen();

      MyObjectBuilder_GuiScreen IActivator<MyObjectBuilder_GuiScreen>.CreateInstance() => new MyObjectBuilder_GuiScreen();
    }
  }
}
