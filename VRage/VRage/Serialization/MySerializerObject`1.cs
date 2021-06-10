// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MySerializerObject`1
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Reflection;
using VRage.Library.Collections;

namespace VRage.Serialization
{
  public class MySerializerObject<T> : MySerializer<T>
  {
    private MyMemberSerializer<T>[] m_memberSerializers;

    public MySerializerObject() => this.m_memberSerializers = typeof (T).GetDataMembers(true, true, true, true, false, true, true, true).Where<MemberInfo>((Func<MemberInfo, bool>) (s => !Attribute.IsDefined(s, typeof (NoSerializeAttribute)))).Where<MemberInfo>((Func<MemberInfo, bool>) (s => Attribute.IsDefined(s, typeof (SerializeAttribute)) || s.IsMemberPublic())).Where<MemberInfo>(new Func<MemberInfo, bool>(this.Filter)).Select<MemberInfo, MyMemberSerializer<T>>((Func<MemberInfo, MyMemberSerializer<T>>) (s => MyFactory.CreateMemberSerializer<T>(s))).ToArray<MyMemberSerializer<T>>();

    private bool Filter(MemberInfo info)
    {
      if (info.MemberType == MemberTypes.Field)
        return true;
      if (info.MemberType != MemberTypes.Property)
        return false;
      PropertyInfo propertyInfo = (PropertyInfo) info;
      return propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.GetIndexParameters().Length == 0;
    }

    public override void Clone(ref T value)
    {
      T instance = Activator.CreateInstance<T>();
      foreach (MyMemberSerializer<T> memberSerializer in this.m_memberSerializers)
        memberSerializer.Clone(ref value, ref instance);
      value = instance;
    }

    public override bool Equals(ref T a, ref T b)
    {
      if (!typeof (T).IsValueType)
      {
        if ((object) a == (object) b)
          return true;
        if (MySerializer.AnyNull((object) a, (object) b))
          return false;
      }
      foreach (MyMemberSerializer<T> memberSerializer in this.m_memberSerializers)
      {
        if (!memberSerializer.Equals(ref a, ref b))
          return false;
      }
      return true;
    }

    public override void Read(BitStream stream, out T value, MySerializeInfo info)
    {
      value = Activator.CreateInstance<T>();
      foreach (MyMemberSerializer<T> memberSerializer in this.m_memberSerializers)
        memberSerializer.Read(stream, ref value, info.ItemInfo);
    }

    public override void Write(BitStream stream, ref T value, MySerializeInfo info)
    {
      foreach (MyMemberSerializer<T> memberSerializer in this.m_memberSerializers)
        memberSerializer.Write(stream, ref value, info.ItemInfo);
    }
  }
}
