﻿// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GuiControlTextbox
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
  public class MyObjectBuilder_GuiControlTextbox : MyObjectBuilder_GuiControlBase
  {
    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003EPosition\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EPosition\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003ESize\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003ESize\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in Vector2 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out Vector2 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003EName\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003EBackgroundColor\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EBackgroundColor\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in Vector4 value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out Vector4 value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003EControlTexture\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlTexture\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in string value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out string value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003EOriginAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EOriginAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, MyGuiDrawAlignEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GuiControlTextbox owner,
        in MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GuiControlTextbox owner,
        out MyGuiDrawAlignEnum value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003EControlAlign\u003C\u003EAccessor : MyObjectBuilder_GuiControlBase.VRage_Game_MyObjectBuilder_GuiControlBase\u003C\u003EControlAlign\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in int value) => this.Set((MyObjectBuilder_GuiControlBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out int value) => this.Get((MyObjectBuilder_GuiControlBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GuiControlTextbox, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GuiControlTextbox owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GuiControlTextbox owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GuiControlTextbox\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GuiControlTextbox>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GuiControlTextbox();

      MyObjectBuilder_GuiControlTextbox IActivator<MyObjectBuilder_GuiControlTextbox>.CreateInstance() => new MyObjectBuilder_GuiControlTextbox();
    }
  }
}