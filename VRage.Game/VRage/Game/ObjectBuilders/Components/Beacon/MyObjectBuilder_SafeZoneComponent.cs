// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilders.Components.Beacon.MyObjectBuilder_SafeZoneComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.Game.ObjectBuilders.Components.Beacon
{
  [ProtoContract]
  [MyObjectBuilderDefinition(null, null)]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public class MyObjectBuilder_SafeZoneComponent : MyObjectBuilder_ComponentBase
  {
    [ProtoMember(1)]
    public double UpkeepTime;
    [ProtoMember(4)]
    public bool Activating;
    [ProtoMember(7)]
    public long ActivationTime;
    [ProtoMember(10)]
    [XmlElement(Type = typeof (MyAbstractXmlSerializer<MyObjectBuilder_EntityBase>))]
    [DynamicObjectBuilder(false)]
    [Serialize(MyObjectFlags.DefaultZero)]
    public MyObjectBuilder_EntityBase SafeZoneOb;

    protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003EUpkeepTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneComponent, double>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SafeZoneComponent owner, in double value) => owner.UpkeepTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SafeZoneComponent owner, out double value) => value = owner.UpkeepTime;
    }

    protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003EActivating\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneComponent, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SafeZoneComponent owner, in bool value) => owner.Activating = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SafeZoneComponent owner, out bool value) => value = owner.Activating;
    }

    protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003EActivationTime\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneComponent, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SafeZoneComponent owner, in long value) => owner.ActivationTime = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SafeZoneComponent owner, out long value) => value = owner.ActivationTime;
    }

    protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003ESafeZoneOb\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_SafeZoneComponent, MyObjectBuilder_EntityBase>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_SafeZoneComponent owner,
        in MyObjectBuilder_EntityBase value)
      {
        owner.SafeZoneOb = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_SafeZoneComponent owner,
        out MyObjectBuilder_EntityBase value)
      {
        value = owner.SafeZoneOb;
      }
    }

    protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SafeZoneComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SafeZoneComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SafeZoneComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SafeZoneComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneComponent, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SafeZoneComponent owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SafeZoneComponent owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_SafeZoneComponent, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_SafeZoneComponent owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_SafeZoneComponent owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_ObjectBuilders_Components_Beacon_MyObjectBuilder_SafeZoneComponent\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_SafeZoneComponent>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_SafeZoneComponent();

      MyObjectBuilder_SafeZoneComponent IActivator<MyObjectBuilder_SafeZoneComponent>.CreateInstance() => new MyObjectBuilder_SafeZoneComponent();
    }
  }
}
