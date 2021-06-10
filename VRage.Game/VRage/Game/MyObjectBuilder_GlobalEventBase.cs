// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyObjectBuilder_GlobalEventBase
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
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
  public class MyObjectBuilder_GlobalEventBase : MyObjectBuilder_Base
  {
    [ProtoMember(1)]
    public SerializableDefinitionId? DefinitionId;
    [ProtoMember(4)]
    public bool Enabled;
    [ProtoMember(7)]
    public long ActivationTimeMs;
    [ProtoMember(10)]
    public MyGlobalEventTypeEnum EventType;

    public bool ShouldSerializeDefinitionId() => false;

    public bool ShouldSerializeEventType() => false;

    protected class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003EDefinitionId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalEventBase, SerializableDefinitionId?>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GlobalEventBase owner,
        in SerializableDefinitionId? value)
      {
        owner.DefinitionId = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GlobalEventBase owner,
        out SerializableDefinitionId? value)
      {
        value = owner.DefinitionId;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalEventBase, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalEventBase owner, in bool value) => owner.Enabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalEventBase owner, out bool value) => value = owner.Enabled;
    }

    protected class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003EActivationTimeMs\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalEventBase, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalEventBase owner, in long value) => owner.ActivationTimeMs = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalEventBase owner, out long value) => value = owner.ActivationTimeMs;
    }

    protected class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003EEventType\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_GlobalEventBase, MyGlobalEventTypeEnum>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(
        ref MyObjectBuilder_GlobalEventBase owner,
        in MyGlobalEventTypeEnum value)
      {
        owner.EventType = value;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(
        ref MyObjectBuilder_GlobalEventBase owner,
        out MyGlobalEventTypeEnum value)
      {
        value = owner.EventType;
      }
    }

    protected class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003Em_subtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalEventBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalEventBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalEventBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003Em_subtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalEventBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalEventBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalEventBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalEventBase, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalEventBase owner, in MyStringHash value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalEventBase owner, out MyStringHash value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    protected class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003ESubtypeName\u003C\u003EAccessor : MyObjectBuilder_Base.VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor, IMemberAccessor<MyObjectBuilder_GlobalEventBase, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_GlobalEventBase owner, in string value) => this.Set((MyObjectBuilder_Base&) ref owner, in value);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_GlobalEventBase owner, out string value) => this.Get((MyObjectBuilder_Base&) ref owner, out value);
    }

    private class VRage_Game_MyObjectBuilder_GlobalEventBase\u003C\u003EActor : IActivator, IActivator<MyObjectBuilder_GlobalEventBase>
    {
      object IActivator.CreateInstance() => (object) new MyObjectBuilder_GlobalEventBase();

      MyObjectBuilder_GlobalEventBase IActivator<MyObjectBuilder_GlobalEventBase>.CreateInstance() => new MyObjectBuilder_GlobalEventBase();
    }
  }
}
