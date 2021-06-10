// Decompiled with JetBrains decompiler
// Type: VRage.Utils.MyStringHash
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage.Utils
{
  [ProtoContract]
  public struct MyStringHash : IEquatable<MyStringHash>, IXmlSerializable
  {
    public static readonly MyStringHash NullOrEmpty;
    [ProtoMember(1)]
    public int m_hash;
    public static readonly MyStringHash.HashComparerType Comparer = new MyStringHash.HashComparerType();
    private static readonly FastResourceLock m_lock = new FastResourceLock();
    private static Dictionary<string, MyStringHash> m_stringToHash = new Dictionary<string, MyStringHash>(50);
    private static Dictionary<MyStringHash, string> m_hashToString = new Dictionary<MyStringHash, string>(50, (IEqualityComparer<MyStringHash>) MyStringHash.Comparer);

    private MyStringHash(int hash) => this.m_hash = hash;

    public string String
    {
      get
      {
        using (MyStringHash.m_lock.AcquireSharedUsing())
          return MyStringHash.m_hashToString[this];
      }
    }

    public override string ToString() => this.String;

    public override int GetHashCode() => this.m_hash;

    public override bool Equals(object obj) => obj is MyStringHash id && this.Equals(id);

    public bool Equals(MyStringHash id) => this.m_hash == id.m_hash;

    public static bool operator ==(MyStringHash lhs, MyStringHash rhs) => lhs.m_hash == rhs.m_hash;

    public static bool operator !=(MyStringHash lhs, MyStringHash rhs) => lhs.m_hash != rhs.m_hash;

    public static explicit operator int(MyStringHash id) => id.m_hash;

    static MyStringHash() => MyStringHash.NullOrEmpty = MyStringHash.GetOrCompute("");

    public static MyStringHash GetOrCompute(string str)
    {
      if (str == null)
        return MyStringHash.NullOrEmpty;
      MyStringHash key;
      using (MyStringHash.m_lock.AcquireSharedUsing())
      {
        if (MyStringHash.m_stringToHash.TryGetValue(str, out key))
          return key;
      }
      using (MyStringHash.m_lock.AcquireExclusiveUsing())
      {
        if (!MyStringHash.m_stringToHash.TryGetValue(str, out key))
        {
          key = new MyStringHash(MyUtils.GetHash(str, 0));
          MyStringHash.m_hashToString.Add(key, str);
          MyStringHash.m_stringToHash.Add(str, key);
        }
        return key;
      }
    }

    public static MyStringHash Get(string str)
    {
      using (MyStringHash.m_lock.AcquireSharedUsing())
        return MyStringHash.m_stringToHash[str];
    }

    public static bool TryGet(string str, out MyStringHash id)
    {
      using (MyStringHash.m_lock.AcquireSharedUsing())
        return MyStringHash.m_stringToHash.TryGetValue(str, out id);
    }

    public static MyStringHash TryGet(string str)
    {
      using (MyStringHash.m_lock.AcquireSharedUsing())
      {
        MyStringHash myStringHash;
        MyStringHash.m_stringToHash.TryGetValue(str, out myStringHash);
        return myStringHash;
      }
    }

    public static MyStringHash TryGet(int id)
    {
      using (MyStringHash.m_lock.AcquireSharedUsing())
      {
        MyStringHash key = new MyStringHash(id);
        return MyStringHash.m_hashToString.ContainsKey(key) ? key : MyStringHash.NullOrEmpty;
      }
    }

    public static bool IsKnown(MyStringHash id)
    {
      using (MyStringHash.m_lock.AcquireSharedUsing())
        return MyStringHash.m_hashToString.ContainsKey(id);
    }

    public XmlSchema GetSchema() => (XmlSchema) null;

    public void ReadXml(XmlReader reader) => this.m_hash = MyStringHash.GetOrCompute(reader.ReadInnerXml()).m_hash;

    public void WriteXml(XmlWriter writer) => writer.WriteString(this.String);

    public class HashComparerType : IComparer<MyStringHash>, IEqualityComparer<MyStringHash>
    {
      public int Compare(MyStringHash x, MyStringHash y) => x.m_hash - y.m_hash;

      public bool Equals(MyStringHash x, MyStringHash y) => x.m_hash == y.m_hash;

      public int GetHashCode(MyStringHash obj) => obj.m_hash;
    }

    protected class VRage_Utils_MyStringHash\u003C\u003Em_hash\u003C\u003EAccessor : IMemberAccessor<MyStringHash, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStringHash owner, in int value) => owner.m_hash = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStringHash owner, out int value) => value = owner.m_hash;
    }
  }
}
