// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyStringId
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Utils
{
  [ProtoContract]
  public struct MyStringId : IXmlSerializable
  {
    public static readonly MyStringId NullOrEmpty;
    [ProtoMember(1)]
    public int m_id;
    public static readonly MyStringId.IdComparerType Comparer = new MyStringId.IdComparerType();
    private static readonly FastResourceLock m_lock = new FastResourceLock();
    private static Dictionary<string, MyStringId> m_stringToId = new Dictionary<string, MyStringId>(50);
    private static Dictionary<MyStringId, string> m_idToString = new Dictionary<MyStringId, string>(50, (IEqualityComparer<MyStringId>) MyStringId.Comparer);

    private MyStringId(int id) => this.m_id = id;

    public int Id => this.m_id;

    public string String
    {
      get
      {
        using (MyStringId.m_lock.AcquireSharedUsing())
          return MyStringId.m_idToString[this];
      }
    }

    public override string ToString() => this.String;

    public override int GetHashCode() => this.m_id;

    public override bool Equals(object obj) => obj is MyStringId id && this.Equals(id);

    public bool Equals(MyStringId id) => this.m_id == id.m_id;

    public static bool operator ==(MyStringId lhs, MyStringId rhs) => lhs.m_id == rhs.m_id;

    public static bool operator !=(MyStringId lhs, MyStringId rhs) => lhs.m_id != rhs.m_id;

    public static explicit operator int(MyStringId id) => id.m_id;

    static MyStringId() => MyStringId.NullOrEmpty = MyStringId.GetOrCompute("");

    public static MyStringId GetOrCompute(string str)
    {
      if (str == null)
        return MyStringId.NullOrEmpty;
      MyStringId key;
      using (MyStringId.m_lock.AcquireSharedUsing())
      {
        if (MyStringId.m_stringToId.TryGetValue(str, out key))
          return key;
      }
      using (MyStringId.m_lock.AcquireExclusiveUsing())
      {
        if (!MyStringId.m_stringToId.TryGetValue(str, out key))
        {
          key = new MyStringId(MyStringId.m_stringToId.Count);
          MyStringId.m_idToString.Add(key, str);
          MyStringId.m_stringToId.Add(str, key);
        }
        return key;
      }
    }

    public static MyStringId Get(string str)
    {
      using (MyStringId.m_lock.AcquireSharedUsing())
        return MyStringId.m_stringToId[str];
    }

    public static bool TryGet(string str, out MyStringId id)
    {
      using (MyStringId.m_lock.AcquireSharedUsing())
        return MyStringId.m_stringToId.TryGetValue(str, out id);
    }

    public static MyStringId TryGet(string str)
    {
      using (MyStringId.m_lock.AcquireSharedUsing())
      {
        MyStringId myStringId;
        MyStringId.m_stringToId.TryGetValue(str, out myStringId);
        return myStringId;
      }
    }

    public static bool IsKnown(MyStringId id)
    {
      using (MyStringId.m_lock.AcquireSharedUsing())
        return MyStringId.m_idToString.ContainsKey(id);
    }

    public XmlSchema GetSchema() => (XmlSchema) null;

    public void ReadXml(XmlReader reader) => this.m_id = MyStringId.GetOrCompute(reader.ReadInnerXml()).Id;

    public void WriteXml(XmlWriter writer) => writer.WriteString(this.String);

    public class IdComparerType : IComparer<MyStringId>, IEqualityComparer<MyStringId>
    {
      public int Compare(MyStringId x, MyStringId y) => x.m_id - y.m_id;

      public bool Equals(MyStringId x, MyStringId y) => x.m_id == y.m_id;

      public int GetHashCode(MyStringId obj) => obj.m_id;
    }

    protected class VRage_Utils_MyStringId\u003C\u003Em_id\u003C\u003EAccessor : IMemberAccessor<MyStringId, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStringId owner, in int value) => owner.m_id = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStringId owner, out int value) => value = owner.m_id;
    }
  }
}
