// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyObjectBuilder_Base
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using ProtoBuf;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
  [ProtoContract]
  [XmlSerializerAssembly("VRage.Game.XmlSerializers")]
  public abstract class MyObjectBuilder_Base
  {
    private MyStringHash m_subtypeId;
    private string m_subtypeName;

    [DefaultValue(0)]
    public MyStringHash SubtypeId => this.m_subtypeId;

    public bool ShouldSerializeSubtypeId() => false;

    [Serialize]
    private MyStringHash m_serializableSubtypeId
    {
      get => this.m_subtypeId;
      set
      {
        this.m_subtypeId = value;
        this.m_subtypeName = value.String;
      }
    }

    [ProtoMember(1)]
    [DefaultValue(null)]
    [NoSerialize]
    public string SubtypeName
    {
      get => this.m_subtypeName;
      set
      {
        this.m_subtypeName = value;
        this.m_subtypeId = MyStringHash.GetOrCompute(value);
      }
    }

    [XmlIgnore]
    public MyObjectBuilderType TypeId => (MyObjectBuilderType) this.GetType();

    public void Save(string filepath) => MyObjectBuilderSerializer.SerializeXML(filepath, false, this);

    public virtual MyObjectBuilder_Base Clone() => MyObjectBuilderSerializer.Clone(this);

    public virtual bool Equals(MyObjectBuilder_Base obj2)
    {
      string str1 = "";
      string str2 = "";
      using (MemoryStream memoryStream = new MemoryStream())
      {
        MyObjectBuilderSerializer.SerializeXML((Stream) memoryStream, obj2);
        str2 = Encoding.Unicode.GetString(memoryStream.ToArray());
      }
      using (MemoryStream memoryStream = new MemoryStream())
      {
        MyObjectBuilderSerializer.SerializeXML((Stream) memoryStream, this);
        str1 = Encoding.Unicode.GetString(memoryStream.ToArray());
      }
      return str1 == str2;
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Base, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Base owner, in MyStringHash value) => owner.m_subtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Base owner, out MyStringHash value) => value = owner.m_subtypeId;
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_subtypeName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Base, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Base owner, in string value) => owner.m_subtypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Base owner, out string value) => value = owner.m_subtypeName;
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003Em_serializableSubtypeId\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Base, MyStringHash>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Base owner, in MyStringHash value) => owner.m_serializableSubtypeId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Base owner, out MyStringHash value) => value = owner.m_serializableSubtypeId;
    }

    protected class VRage_ObjectBuilders_MyObjectBuilder_Base\u003C\u003ESubtypeName\u003C\u003EAccessor : IMemberAccessor<MyObjectBuilder_Base, string>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyObjectBuilder_Base owner, in string value) => owner.SubtypeName = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyObjectBuilder_Base owner, out string value) => value = owner.SubtypeName;
    }
  }
}
