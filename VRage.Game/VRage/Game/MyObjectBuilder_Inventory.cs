// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Inventory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Inventory : MyObjectBuilder_InventoryBase
  {
    [ProtoMember(1)]
    public List<MyObjectBuilder_InventoryItem> Items = new List<MyObjectBuilder_InventoryItem>();
    [ProtoMember(4)]
    public uint nextItemId;
    [ProtoMember(7)]
    [DefaultValue(null)]
    public MyFixedPoint? Volume;
    [ProtoMember(10)]
    [DefaultValue(null)]
    public MyFixedPoint? Mass;
    [ProtoMember(13)]
    [DefaultValue(null)]
    public int? MaxItemCount;
    [ProtoMember(16)]
    [DefaultValue(null)]
    public SerializableVector3? Size;
    [ProtoMember(19)]
    [DefaultValue(null)]
    public MyInventoryFlags? InventoryFlags;
    [ProtoMember(22)]
    public bool RemoveEntityOnEmpty;

    public bool ShouldSerializeMaxItemCount() => this.MaxItemCount.HasValue;

    public override void Clear()
    {
      this.Items.Clear();
      this.nextItemId = 0U;
      base.Clear();
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003EItems\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, List<MyObjectBuilder_InventoryItem>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Inventory owner,
        in List<MyObjectBuilder_InventoryItem> value)
      {
        owner.Items = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Inventory owner,
        out List<MyObjectBuilder_InventoryItem> value)
      {
        value = owner.Items;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003EnextItemId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, uint>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in uint value) => owner.nextItemId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out uint value) => value = owner.nextItemId;
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003EVolume\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, MyFixedPoint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in MyFixedPoint? value) => owner.Volume = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out MyFixedPoint? value) => value = owner.Volume;
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003EMass\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, MyFixedPoint?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in MyFixedPoint? value) => owner.Mass = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out MyFixedPoint? value) => value = owner.Mass;
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003EMaxItemCount\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in int? value) => owner.MaxItemCount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out int? value) => value = owner.MaxItemCount;
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, SerializableVector3?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in SerializableVector3? value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out SerializableVector3? value) => value = owner.Size;
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003EInventoryFlags\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, MyInventoryFlags?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in MyInventoryFlags? value) => owner.InventoryFlags = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out MyInventoryFlags? value) => value = owner.InventoryFlags;
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003ERemoveEntityOnEmpty\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Inventory, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in bool value) => owner.RemoveEntityOnEmpty = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out bool value) => value = owner.RemoveEntityOnEmpty;
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003EInventoryId\u003C\u003EAccessor : MyObjectBuilder_InventoryBase.VRage_Game_ObjectBuilders_ComponentSystem_MyObjectBuilder_InventoryBase\u003C\u003EInventoryId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in string value) => this.Set((MyObjectBuilder_InventoryBase&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out string value) => this.Get((MyObjectBuilder_InventoryBase&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Inventory\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Inventory, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Inventory owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Inventory owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Inventory\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Inventory>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Inventory();

      MyObjectBuilder_Inventory IActivator<MyObjectBuilder_Inventory>.CreateInstance() => new MyObjectBuilder_Inventory();
    }
  }
}
