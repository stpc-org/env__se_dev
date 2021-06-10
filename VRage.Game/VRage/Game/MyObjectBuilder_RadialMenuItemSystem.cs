// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_RadialMenuItemSystem
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
  public class MyObjectBuilder_RadialMenuItemSystem : MyObjectBuilder_RadialMenuItem
  {
    public int SystemAction;

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003ESystemAction\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in int value) => owner.SystemAction = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out int value) => value = owner.SystemAction;
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in List<string> value) => this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemSystem owner,
        out List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003ELabelName\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ELabelName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in MyStringId value) => this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out MyStringId value) => this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003ELabelShortcut\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ELabelShortcut\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in MyStringId value) => this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out MyStringId value) => this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003ECloseMenu\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ECloseMenu\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in bool value) => this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out bool value) => this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemSystem owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemSystem owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemSystem, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_RadialMenuItemSystem owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_RadialMenuItemSystem owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_RadialMenuItemSystem\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_RadialMenuItemSystem>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_RadialMenuItemSystem();

      MyObjectBuilder_RadialMenuItemSystem IActivator<MyObjectBuilder_RadialMenuItemSystem>.CreateInstance() => new MyObjectBuilder_RadialMenuItemSystem();
    }
  }
}
