// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_RadialMenuItemEmpty
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_RadialMenuItemEmpty : MyObjectBuilder_RadialMenuItem
  {
    protected class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemEmpty, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemEmpty owner, in List<string> value) => this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemEmpty owner, out List<string> value) => this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003ELabelName\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ELabelName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemEmpty, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemEmpty owner, in MyStringId value) => this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemEmpty owner, out MyStringId value) => this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003ELabelShortcut\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ELabelShortcut\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemEmpty, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemEmpty owner, in MyStringId value) => this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemEmpty owner, out MyStringId value) => this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003ECloseMenu\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ECloseMenu\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemEmpty, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemEmpty owner, in bool value) => this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemEmpty owner, out bool value) => this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemEmpty, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemEmpty owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemEmpty owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemEmpty, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemEmpty owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemEmpty owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemEmpty, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemEmpty owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemEmpty owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemEmpty, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemEmpty owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemEmpty owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_RadialMenuItemEmpty\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_RadialMenuItemEmpty>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_RadialMenuItemEmpty();

      MyObjectBuilder_RadialMenuItemEmpty IActivator<MyObjectBuilder_RadialMenuItemEmpty>.CreateInstance() => new MyObjectBuilder_RadialMenuItemEmpty();
    }
  }
}
