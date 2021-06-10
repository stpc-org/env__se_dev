// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.MyFactory
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Collections;
using VRage.Network;
using VRage.ObjectBuilder;

namespace VRage.Serialization
{
  public static class MyFactory
  {
    private static ThreadSafeStore<Type, MySerializer> m_serializers = new ThreadSafeStore<Type, MySerializer>(new Func<Type, MySerializer>(MyFactory.CreateSerializerInternal));
    private static Dictionary<Type, Type> m_serializerTypes = new Dictionary<Type, Type>();

    static MyFactory() => MyFactory.RegisterFromAssembly(Assembly.GetExecutingAssembly());

    public static MySerializer<T> GetSerializer<T>() => (MySerializer<T>) MyFactory.GetSerializer(typeof (T));

    public static MySerializer GetSerializer(Type t) => MyFactory.m_serializers.Get(t);

    public static MySerializeInfo CreateInfo(MemberInfo member) => MySerializeInfo.Create((ICustomAttributeProvider) member);

    public static MyMemberSerializer<TOwner> CreateMemberSerializer<TOwner>(
      MemberInfo member)
    {
      return (MyMemberSerializer<TOwner>) MyFactory.CreateMemberSerializer(member, typeof (TOwner));
    }

    public static MyMemberSerializer CreateMemberSerializer(
      MemberInfo member,
      Type ownerType)
    {
      MyMemberSerializer instance = (MyMemberSerializer) Activator.CreateInstance(typeof (MyMemberSerializer<,>).MakeGenericType(ownerType, member.GetMemberType()));
      instance.Init(member, MyFactory.CreateInfo(member));
      return instance;
    }

    private static MySerializer CreateSerializerInternal(Type t)
    {
      Type type;
      lock (MyFactory.m_serializerTypes)
        MyFactory.m_serializerTypes.TryGetValue(t, out type);
      if (type != (Type) null)
        return (MySerializer) Activator.CreateInstance(type);
      if (t.IsEnum)
        return (MySerializer) Activator.CreateInstance(typeof (MySerializerEnum<>).MakeGenericType(t));
      if (t.IsArray)
        return (MySerializer) Activator.CreateInstance(typeof (MySerializerArray<>).MakeGenericType(t.GetElementType()));
      if (typeof (IMyNetObject).IsAssignableFrom(t))
        return (MySerializer) Activator.CreateInstance(typeof (MySerializerNetObject<>).MakeGenericType(t));
      if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof (Nullable<>))
        return (MySerializer) Activator.CreateInstance(typeof (MySerializerNullable<>).MakeGenericType(t.GetGenericArguments()[0]));
      if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof (MySerializableList<>))
        return (MySerializer) Activator.CreateInstance(typeof (MySerializerObList<>).MakeGenericType(t.GetGenericArguments()[0]));
      if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof (List<>))
        return (MySerializer) Activator.CreateInstance(typeof (MySerializerList<>).MakeGenericType(t.GetGenericArguments()[0]));
      if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof (HashSet<>))
        return (MySerializer) Activator.CreateInstance(typeof (MySerializerHashSet<>).MakeGenericType(t.GetGenericArguments()[0]));
      if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof (Dictionary<,>))
      {
        Type[] genericArguments = t.GetGenericArguments();
        return (MySerializer) Activator.CreateInstance(typeof (MySerializerDictionary<,>).MakeGenericType(genericArguments[0], genericArguments[1]));
      }
      return t.IsClass || t.IsStruct() ? (MySerializer) Activator.CreateInstance(typeof (MySerializerObject<>).MakeGenericType(t)) : throw new InvalidOperationException("No serializer found for type: " + t.Name);
    }

    public static void Register(Type serializedType, Type serializer)
    {
      lock (MyFactory.m_serializerTypes)
        MyFactory.m_serializerTypes.Add(serializedType, serializer);
    }

    public static void RegisterFromAssembly(Assembly assembly)
    {
      foreach (Type type in assembly.GetTypes())
      {
        if (!type.IsGenericType && type.BaseType != (Type) null && (type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof (MySerializer<>)))
          MyFactory.Register(type.BaseType.GetGenericArguments()[0], type);
      }
    }
  }
}
