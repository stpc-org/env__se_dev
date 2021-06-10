// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_RadialMenuItem
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
  public abstract class MyObjectBuilder_RadialMenuItem : MyObjectBuilder_Base
  {
    public List<string> Icons;
    public MyStringId LabelName;
    public MyStringId LabelShortcut;
    public bool CloseMenu = true;

    protected class VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003EIcons\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItem, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItem owner, in List<string> value) => owner.Icons = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItem owner, out List<string> value) => value = owner.Icons;
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ELabelName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItem, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItem owner, in MyStringId value) => owner.LabelName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItem owner, out MyStringId value) => value = owner.LabelName;
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ELabelShortcut\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItem, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItem owner, in MyStringId value) => owner.LabelShortcut = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItem owner, out MyStringId value) => value = owner.LabelShortcut;
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ECloseMenu\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItem, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItem owner, in bool value) => owner.CloseMenu = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItem owner, out bool value) => value = owner.CloseMenu;
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItem owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }
  }
}
