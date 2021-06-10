// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.SerializableDefinitionId
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
  [ProtoContract]
  public struct SerializableDefinitionId
  {
    [XmlIgnore]
    [NoSerialize]
    public MyObjectBuilderType TypeId;
    [XmlIgnore]
    [NoSerialize]
    public string SubtypeName;

    [ProtoMember(1)]
    [XmlAttribute("Type")]
    [NoSerialize]
    public string TypeIdStringAttribute
    {
      get => this.TypeId.IsNull ? "(null)" : this.TypeId.ToString();
      set
      {
        if (value == null)
          return;
        this.TypeIdString = value;
      }
    }

    [ProtoMember(4)]
    [XmlElement("TypeId")]
    [NoSerialize]
    public string TypeIdString
    {
      get => this.TypeId.IsNull ? "(null)" : this.TypeId.ToString();
      set => this.TypeId = MyObjectBuilderType.ParseBackwardsCompatible(value);
    }

    public bool ShouldSerializeTypeIdString() => false;

    [ProtoMember(7)]
    [XmlAttribute("Subtype")]
    [NoSerialize]
    public string SubtypeIdAttribute
    {
      get => this.SubtypeName;
      set => this.SubtypeName = value;
    }

    [ProtoMember(10)]
    [NoSerialize]
    public string SubtypeId
    {
      get => this.SubtypeName;
      set => this.SubtypeName = value;
    }

    public bool ShouldSerializeSubtypeId() => false;

    [Serialize]
    private ushort m_binaryTypeId
    {
      get => ((MyRuntimeObjectBuilderId) this.TypeId).Value;
      set => this.TypeId = (MyObjectBuilderType) new MyRuntimeObjectBuilderId(value);
    }

    [Serialize]
    private MyStringHash m_binarySubtypeId
    {
      get => MyStringHash.TryGet(this.SubtypeId);
      set => this.SubtypeName = value.String;
    }

    public SerializableDefinitionId(MyObjectBuilderType typeId, string subtypeName)
    {
      this.TypeId = typeId;
      this.SubtypeName = subtypeName;
    }

    public override string ToString() => string.Format("{0}/{1}", (object) this.TypeId, (object) this.SubtypeName);

    public bool IsNull() => this.TypeId.IsNull;

    protected class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003ETypeId\u003C\u003EAccessor : IMemberAccessor<SerializableDefinitionId, MyObjectBuilderType>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDefinitionId owner, in MyObjectBuilderType value) => owner.TypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDefinitionId owner, out MyObjectBuilderType value) => value = owner.TypeId;
    }

    protected class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003ESubtypeName\u003C\u003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDefinitionId owner, in string value) => owner.SubtypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDefinitionId owner, out string value) => value = owner.SubtypeName;
    }

    protected class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003ETypeIdStringAttribute\u003C\u003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDefinitionId owner, in string value) => owner.TypeIdStringAttribute = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDefinitionId owner, out string value) => value = owner.TypeIdStringAttribute;
    }

    protected class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003ETypeIdString\u003C\u003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDefinitionId owner, in string value) => owner.TypeIdString = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDefinitionId owner, out string value) => value = owner.TypeIdString;
    }

    protected class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003ESubtypeIdAttribute\u003C\u003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDefinitionId owner, in string value) => owner.SubtypeIdAttribute = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDefinitionId owner, out string value) => value = owner.SubtypeIdAttribute;
    }

    protected class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003ESubtypeId\u003C\u003EAccessor : IMemberAccessor<SerializableDefinitionId, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDefinitionId owner, in string value) => owner.SubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDefinitionId owner, out string value) => value = owner.SubtypeId;
    }

    protected class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003Em_binaryTypeId\u003C\u003EAccessor : IMemberAccessor<SerializableDefinitionId, ushort>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDefinitionId owner, in ushort value) => owner.m_binaryTypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDefinitionId owner, out ushort value) => value = owner.m_binaryTypeId;
    }

    protected class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003Em_binarySubtypeId\u003C\u003EAccessor : IMemberAccessor<SerializableDefinitionId, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref SerializableDefinitionId owner, in MyStringHash value) => owner.m_binarySubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref SerializableDefinitionId owner, out MyStringHash value) => value = owner.m_binarySubtypeId;
    }

    private class VRage_ObjectBuilders_SerializableDefinitionId\u003C\u003EActor : IActivator, IActivator<SerializableDefinitionId>
    {
      object IActivator.CreateInstance() => (object) new SerializableDefinitionId();

      SerializableDefinitionId IActivator<SerializableDefinitionId>.CreateInstance() => new SerializableDefinitionId();
    }
  }
}
