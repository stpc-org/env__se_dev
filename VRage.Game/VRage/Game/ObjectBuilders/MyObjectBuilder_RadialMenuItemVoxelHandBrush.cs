// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.MyObjectBuilder_RadialMenuItemVoxelHandBrush
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

namespace VRage.Game.ObjectBuilders
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_RadialMenuItemVoxelHandBrush : MyObjectBuilder_RadialMenuItem
  {
    public string BrushSubtypeName;

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003EBrushSubtypeName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in string value)
      {
        owner.BrushSubtypeName = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out string value)
      {
        value = owner.BrushSubtypeName;
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003EIcons\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003EIcons\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, List<string>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out List<string> value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003ELabelName\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ELabelName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in MyStringId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out MyStringId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003ELabelShortcut\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ELabelShortcut\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, MyStringId>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in MyStringId value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out MyStringId value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003ECloseMenu\u003C\u003EAccessor : MyObjectBuilder_RadialMenuItem.VRage_Game_MyObjectBuilder_RadialMenuItem\u003C\u003ECloseMenu\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in bool value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_RadialMenuItem&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out bool value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_RadialMenuItem&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out MyStringHash value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    protected class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_RadialMenuItemVoxelHandBrush, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        in string value)
      {
        // ISSUE: cast to a reference type
        this.Set((MyObjectBuilder_Base&) ref owner, in value);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_RadialMenuItemVoxelHandBrush owner,
        out string value)
      {
        // ISSUE: cast to a reference type
        this.Get((MyObjectBuilder_Base&) ref owner, out value);
      }
    }

    private class VRage_Game_ObjectBuilders_MyObjectBuilder_RadialMenuItemVoxelHandBrush\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_RadialMenuItemVoxelHandBrush>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_RadialMenuItemVoxelHandBrush();

      MyObjectBuilder_RadialMenuItemVoxelHandBrush IActivator<MyObjectBuilder_RadialMenuItemVoxelHandBrush>.CreateInstance() => new MyObjectBuilder_RadialMenuItemVoxelHandBrush();
    }
  }
}
