// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_PhysicalObject
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
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
  public class MyObjectBuilder_PhysicalObject : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    [DefaultValue(MyItemFlags.None)]
    public MyItemFlags Flags;
    [ProtoMember(4)]
    [DefaultValue(null)]
    public float? DurabilityHP;

    public bool ShouldSerializeDurabilityHP() => this.DurabilityHP.HasValue;

    public virtual bool CanStack(MyObjectBuilder_PhysicalObject a) => a != null && this.CanStack(a.TypeId, a.SubtypeId, a.Flags);

    public virtual bool CanStack(
      MyObjectBuilderType typeId,
      MyStringHash subtypeId,
      MyItemFlags flags)
    {
      return flags == this.Flags && typeId == this.TypeId && subtypeId == this.SubtypeId;
    }

    public MyObjectBuilder_PhysicalObject()
      : this(MyItemFlags.None)
    {
    }

    public MyObjectBuilder_PhysicalObject(MyItemFlags flags) => this.Flags = flags;

    public virtual MyDefinitionId GetObjectId() => this.GetId();

    protected class VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EFlags\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalObject, MyItemFlags>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicalObject owner, in MyItemFlags value) => owner.Flags = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicalObject owner, out MyItemFlags value) => value = owner.Flags;
    }

    protected class VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EDurabilityHP\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_PhysicalObject, float?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicalObject owner, in float? value) => owner.DurabilityHP = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicalObject owner, out float? value) => value = owner.DurabilityHP;
    }

    protected class VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalObject, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicalObject owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicalObject owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalObject, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicalObject owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicalObject owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalObject, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicalObject owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicalObject owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_PhysicalObject, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_PhysicalObject owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_PhysicalObject owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_PhysicalObject\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_PhysicalObject>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_PhysicalObject();

      MyObjectBuilder_PhysicalObject IActivator<MyObjectBuilder_PhysicalObject>.CreateInstance() => new MyObjectBuilder_PhysicalObject();
    }
  }
}
