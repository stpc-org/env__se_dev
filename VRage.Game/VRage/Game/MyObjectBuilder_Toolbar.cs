// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_Toolbar
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;

namespace VRage.Game
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_Toolbar : MyObjectBuilder_Base
  {
    [ProtoMember(10)]
    public MyToolbarType ToolbarType;
    [ProtoMember(13)]
    [DefaultValue(null)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public int? SelectedSlot;
    [ProtoMember(16)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<MyObjectBuilder_Toolbar.Slot> Slots;
    [ProtoMember(17)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public List<MyObjectBuilder_Toolbar.Slot> SlotsGamepad;
    [ProtoMember(19)]
    [DefaultValue(null)]
    [NoSerialize]
    public List<Vector3> ColorMaskHSVList;

    public bool ShouldSerializeColorMaskHSVList() => false;

    public MyObjectBuilder_Toolbar()
    {
      this.Slots = new List<MyObjectBuilder_Toolbar.Slot>();
      this.SlotsGamepad = new List<MyObjectBuilder_Toolbar.Slot>();
    }

    public void Remap(IMyRemapHelper remapHelper)
    {
      if (this.Slots != null)
      {
        foreach (MyObjectBuilder_Toolbar.Slot slot in this.Slots)
          slot.Data.Remap(remapHelper);
      }
      if (this.SlotsGamepad == null)
        return;
      foreach (MyObjectBuilder_Toolbar.Slot slot in this.SlotsGamepad)
        slot.Data.Remap(remapHelper);
    }

    [ProtoContract]
    public struct Slot
    {
      [ProtoMember(1)]
      public int Index;
      [ProtoMember(4)]
      public string Item;
      [ProtoMember(7)]
      [DynamicObjectBuilder(false)]
      [XmlElement(Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_ToolbarItem>))]
      public MyObjectBuilder_ToolbarItem Data;

      protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003ESlot\u003C\u003EIndex\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar.Slot, int>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Toolbar.Slot owner, in int value) => owner.Index = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Toolbar.Slot owner, out int value) => value = owner.Index;
      }

      protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003ESlot\u003C\u003EItem\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar.Slot, string>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(ref MyObjectBuilder_Toolbar.Slot owner, in string value) => owner.Item = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(ref MyObjectBuilder_Toolbar.Slot owner, out string value) => value = owner.Item;
      }

      protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003ESlot\u003C\u003EData\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar.Slot, MyObjectBuilder_ToolbarItem>
      {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Set(
          ref MyObjectBuilder_Toolbar.Slot owner,
          in MyObjectBuilder_ToolbarItem value)
        {
          owner.Data = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Get(
          ref MyObjectBuilder_Toolbar.Slot owner,
          out MyObjectBuilder_ToolbarItem value)
        {
          value = owner.Data;
        }
      }

      private class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003ESlot\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Toolbar.Slot>
      {
        object IActivator.CreateInstance() => (object) new MyObjectBuilder_Toolbar.Slot();

        MyObjectBuilder_Toolbar.Slot IActivator<MyObjectBuilder_Toolbar.Slot>.CreateInstance() => new MyObjectBuilder_Toolbar.Slot();
      }
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003EToolbarType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, MyToolbarType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Toolbar owner, in MyToolbarType value) => owner.ToolbarType = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Toolbar owner, out MyToolbarType value) => value = owner.ToolbarType;
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003ESelectedSlot\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, int?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Toolbar owner, in int? value) => owner.SelectedSlot = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Toolbar owner, out int? value) => value = owner.SelectedSlot;
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003ESlots\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, List<MyObjectBuilder_Toolbar.Slot>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Toolbar owner,
        in List<MyObjectBuilder_Toolbar.Slot> value)
      {
        owner.Slots = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Toolbar owner,
        out List<MyObjectBuilder_Toolbar.Slot> value)
      {
        value = owner.Slots;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003ESlotsGamepad\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, List<MyObjectBuilder_Toolbar.Slot>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_Toolbar owner,
        in List<MyObjectBuilder_Toolbar.Slot> value)
      {
        owner.SlotsGamepad = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_Toolbar owner,
        out List<MyObjectBuilder_Toolbar.Slot> value)
      {
        value = owner.SlotsGamepad;
      }
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003EColorMaskHSVList\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Toolbar, List<Vector3>>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Toolbar owner, in List<Vector3> value) => owner.ColorMaskHSVList = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Toolbar owner, out List<Vector3> value) => value = owner.ColorMaskHSVList;
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Toolbar, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Toolbar owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Toolbar owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Toolbar, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Toolbar owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Toolbar owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Toolbar, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Toolbar owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Toolbar owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_Toolbar, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Toolbar owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Toolbar owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_Toolbar\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_Toolbar>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_Toolbar();

      MyObjectBuilder_Toolbar IActivator<MyObjectBuilder_Toolbar>.CreateInstance() => new MyObjectBuilder_Toolbar();
    }
  }
}
