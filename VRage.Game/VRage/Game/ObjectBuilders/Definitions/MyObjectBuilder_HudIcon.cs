// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Definitions.MyObjectBuilder_HudIcon
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
  public class MyObjectBuilder_HudIcon : MyObjectBuilder_Base
  {
    public Vector2 Position;
    public MyGuiDrawAlignEnum? OriginAlign;
    public Vector2? Size;
    public MyStringHash Texture;
    public MyAlphaBlinkBehavior Blink;

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in Vector2 value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out Vector2 value) => value = owner.Position;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003EOriginAlign\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, MyGuiDrawAlignEnum?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in MyGuiDrawAlignEnum? value) => owner.OriginAlign = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out MyGuiDrawAlignEnum? value) => value = owner.OriginAlign;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, Vector2?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in Vector2? value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out Vector2? value) => value = owner.Size;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003ETexture\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in MyStringHash value) => owner.Texture = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out MyStringHash value) => value = owner.Texture;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003EBlink\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_HudIcon, MyAlphaBlinkBehavior>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in MyAlphaBlinkBehavior value) => owner.Blink = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out MyAlphaBlinkBehavior value) => value = owner.Blink;
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HudIcon, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HudIcon, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HudIcon, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_HudIcon, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_HudIcon owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_HudIcon owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Definitions_MyObjectBuilder_HudIcon\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_HudIcon>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_HudIcon();

      MyObjectBuilder_HudIcon IActivator<MyObjectBuilder_HudIcon>.CreateInstance() => new MyObjectBuilder_HudIcon();
    }
  }
}
