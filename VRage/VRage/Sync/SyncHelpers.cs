// Decompiled with JetBrains decompiler
// Type: VRage.Sync.SyncHelpers
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Sync
{
  public static class SyncHelpers
  {
    private static Dictionary<Type, List<Tuple<FieldInfo, SyncHelpers.Composer, MySerializeInfo>>> m_composers = new Dictionary<Type, List<Tuple<FieldInfo, SyncHelpers.Composer, MySerializeInfo>>>();
    private static FastResourceLock m_composersLock = new FastResourceLock();

    public static SyncType Compose(object obj, int firstId = 0)
    {
      List<SyncBase> syncBaseList = new List<SyncBase>();
      SyncHelpers.Compose(obj, firstId, syncBaseList);
      return new SyncType(syncBaseList);
    }

    public static void Compose(object obj, int startingId, List<SyncBase> resultList)
    {
      Type type = obj.GetType();
      List<Tuple<FieldInfo, SyncHelpers.Composer, MySerializeInfo>> composer;
      using (SyncHelpers.m_composersLock.AcquireExclusiveUsing())
      {
        if (!SyncHelpers.m_composers.TryGetValue(type, out composer))
        {
          composer = SyncHelpers.CreateComposer(type);
          SyncHelpers.m_composers.Add(type, composer);
        }
      }
      foreach (Tuple<FieldInfo, SyncHelpers.Composer, MySerializeInfo> tuple in composer)
      {
        SyncBase syncBase = (SyncBase) tuple.Item2(obj, startingId++, (ISerializerInfo) tuple.Item3);
        syncBase.DebugName = tuple.Item1.Name;
        resultList.Add(syncBase);
      }
    }

    private static List<Tuple<FieldInfo, SyncHelpers.Composer, MySerializeInfo>> CreateComposer(
      Type type)
    {
      List<Tuple<FieldInfo, SyncHelpers.Composer, MySerializeInfo>> tupleList = new List<Tuple<FieldInfo, SyncHelpers.Composer, MySerializeInfo>>();
      foreach (FieldInfo field in type.GetDataMembers(true, false, true, true, false, true, true, true).OfType<FieldInfo>())
      {
        if (typeof (SyncBase).IsAssignableFrom(field.FieldType))
          tupleList.Add(new Tuple<FieldInfo, SyncHelpers.Composer, MySerializeInfo>(field, SyncHelpers.CreateFieldComposer(field), MyFactory.CreateInfo((MemberInfo) field)));
      }
      return tupleList;
    }

    private static SyncHelpers.Composer CreateFieldComposer(FieldInfo field)
    {
      ISyncComposer syncComposer = CodegenUtils.GetSyncComposer(field);
      if (syncComposer != null)
        return new SyncHelpers.Composer(syncComposer.Compose);
      ConstructorInfo constructor = field.FieldType.GetConstructor(new Type[2]
      {
        typeof (int),
        typeof (ISerializerInfo)
      });
      if (field.IsInitOnly)
        return (SyncHelpers.Composer) ((instance, id, info) =>
        {
          object instance1 = Activator.CreateInstance(field.FieldType, (object) id, (object) info);
          field.SetValue(instance, instance1);
          return (ISyncType) instance1;
        });
      ParameterExpression parameterExpression4 = Expression.Parameter(typeof (object), "instance");
      ParameterExpression parameterExpression5 = Expression.Parameter(typeof (int), "id");
      ParameterExpression parameterExpression6 = Expression.Parameter(typeof (ISerializerInfo), "serializeInfo");
      UnaryExpression unaryExpression = Expression.Convert((Expression) parameterExpression4, field.DeclaringType);
      ParameterExpression parameterExpression7 = Expression.Parameter(field.FieldType, "syncInstance");
      NewExpression newExpression = Expression.New(constructor, (Expression) parameterExpression5, (Expression) parameterExpression6);
      return ((Expression<SyncHelpers.Composer>) ((parameterExpression1, parameterExpression2, parameterExpression3) => Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
      {
        parameterExpression7
      }, (Expression) Expression.Assign((Expression) parameterExpression7, (Expression) newExpression), (Expression) Expression.Assign((Expression) Expression.Field((Expression) unaryExpression, field), (Expression) parameterExpression7), (Expression) parameterExpression7))).Compile();
    }

    internal delegate ISyncType Composer(
      object instance,
      int id,
      ISerializerInfo serializeInfo);
  }
}
