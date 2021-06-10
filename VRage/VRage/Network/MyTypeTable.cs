// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyTypeTable
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Library.Collections;
using VRage.Utils;

namespace VRage.Network
{
  public class MyTypeTable
  {
    private List<MySynchronizedTypeInfo> m_idToType = new List<MySynchronizedTypeInfo>();
    private Dictionary<Type, MySynchronizedTypeInfo> m_typeLookup = new Dictionary<Type, MySynchronizedTypeInfo>();
    private Dictionary<int, MySynchronizedTypeInfo> m_hashLookup = new Dictionary<int, MySynchronizedTypeInfo>();
    private MyEventTable m_staticEventTable = new MyEventTable((MySynchronizedTypeInfo) null);

    public MyEventTable StaticEventTable => this.m_staticEventTable;

    public bool Contains(Type type) => this.m_typeLookup.ContainsKey(type);

    public MySynchronizedTypeInfo Get(TypeId id)
    {
      if ((long) id.Value >= (long) this.m_idToType.Count)
        MyLog.Default.WriteLine("Invalid replication type ID: " + (object) id.Value);
      return this.m_idToType[(int) id.Value];
    }

    public MySynchronizedTypeInfo Get(Type type) => this.m_typeLookup[type];

    public bool TryGet(Type type, out MySynchronizedTypeInfo typeInfo) => this.m_typeLookup.TryGetValue(type, out typeInfo);

    public MySynchronizedTypeInfo Register(Type type)
    {
      MySynchronizedTypeInfo synchronizedTypeInfo;
      if (!this.m_typeLookup.TryGetValue(type, out synchronizedTypeInfo))
      {
        MySynchronizedTypeInfo baseType = this.CreateBaseType(type);
        bool isReplicated = MyTypeTable.IsReplicated(type);
        bool flag1 = MyTypeTable.HasEvents(type);
        bool flag2 = MyTypeTable.IsSerializableClass(type);
        if (isReplicated | flag1 | flag2)
        {
          synchronizedTypeInfo = new MySynchronizedTypeInfo(type, new TypeId((uint) this.m_idToType.Count), baseType, isReplicated);
          this.m_idToType.Add(synchronizedTypeInfo);
          this.m_hashLookup.Add(synchronizedTypeInfo.TypeHash, synchronizedTypeInfo);
          this.m_typeLookup.Add(type, synchronizedTypeInfo);
          if (isReplicated | flag1)
            this.m_staticEventTable.AddStaticEvents(type);
        }
        else if (baseType != null)
        {
          synchronizedTypeInfo = baseType;
          this.m_typeLookup.Add(type, synchronizedTypeInfo);
        }
        else
          synchronizedTypeInfo = (MySynchronizedTypeInfo) null;
      }
      return synchronizedTypeInfo;
    }

    public static bool ShouldRegister(Type type) => MyTypeTable.IsReplicated(type) || MyTypeTable.CanHaveEvents(type) || MyTypeTable.IsSerializableClass(type);

    private static bool IsSerializableClass(Type type) => type.HasAttribute<SerializableAttribute>() && !type.HasAttribute<CompilerGeneratedAttribute>() || type.IsEnum || typeof (MulticastDelegate).IsAssignableFrom(type.BaseType);

    private static bool IsReplicated(Type type) => !type.IsAbstract && typeof (IMyReplicable).IsAssignableFrom(type) && !type.HasAttribute<NotReplicableAttribute>();

    private static bool CanHaveEvents(Type type) => Attribute.IsDefined((MemberInfo) type, typeof (StaticEventOwnerAttribute)) || typeof (IMyEventOwner).IsAssignableFrom(type);

    private static bool HasEvents(Type type) => ((IEnumerable<MemberInfo>) type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)).Any<MemberInfo>((Func<MemberInfo, bool>) (s => s.HasAttribute<EventAttribute>()));

    private MySynchronizedTypeInfo CreateBaseType(Type type)
    {
      for (; type.BaseType != (Type) null && type.BaseType != typeof (object); type = type.BaseType)
      {
        if (MyTypeTable.ShouldRegister(type.BaseType))
          return this.Register(type.BaseType);
      }
      return (MySynchronizedTypeInfo) null;
    }

    public void Serialize(BitStream stream)
    {
      if (stream.Writing)
      {
        stream.WriteVariant((uint) this.m_idToType.Count);
        for (int index = 0; index < this.m_idToType.Count; ++index)
          stream.WriteInt32(this.m_idToType[index].TypeHash);
      }
      else
      {
        int num = (int) stream.ReadUInt32Variant();
        if (this.m_idToType.Count != num)
          MyLog.Default.WriteLine(string.Format("Bad number of types from server. Recieved {0}, have {1}", (object) num, (object) this.m_idToType.Count));
        this.m_staticEventTable = new MyEventTable((MySynchronizedTypeInfo) null);
        for (int index = 0; index < num; ++index)
        {
          int key = stream.ReadInt32();
          if (!this.m_hashLookup.ContainsKey(key))
            MyLog.Default.WriteLine("Type hash not found! Value: " + (object) key);
          MySynchronizedTypeInfo synchronizedTypeInfo = this.m_hashLookup[key];
          this.m_idToType[index] = synchronizedTypeInfo;
          this.m_staticEventTable.AddStaticEvents(synchronizedTypeInfo.Type);
        }
      }
    }
  }
}
