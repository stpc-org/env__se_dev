// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyEventTable
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using VRage.Library.Collections;
using VRage.Serialization;

namespace VRage.Network
{
  public class MyEventTable
  {
    private MethodInfo m_createCallSite = typeof (MyEventTable).GetMethod("CreateCallSite", BindingFlags.Instance | BindingFlags.NonPublic);
    private Dictionary<uint, CallSite> m_idToEvent;
    private Dictionary<MethodInfo, CallSite> m_methodInfoLookup;
    private ConcurrentDictionary<object, CallSite> m_associateObjectLookup;
    public readonly MySynchronizedTypeInfo Type;

    public int Count => this.m_idToEvent.Count;

    public MyEventTable(MySynchronizedTypeInfo type)
    {
      this.Type = type;
      if (type != null && type.BaseType != null)
      {
        this.m_idToEvent = new Dictionary<uint, CallSite>((IDictionary<uint, CallSite>) type.BaseType.EventTable.m_idToEvent);
        this.m_methodInfoLookup = new Dictionary<MethodInfo, CallSite>((IDictionary<MethodInfo, CallSite>) type.BaseType.EventTable.m_methodInfoLookup);
        this.m_associateObjectLookup = new ConcurrentDictionary<object, CallSite>((IEnumerable<KeyValuePair<object, CallSite>>) type.BaseType.EventTable.m_associateObjectLookup);
      }
      else
      {
        this.m_idToEvent = new Dictionary<uint, CallSite>();
        this.m_methodInfoLookup = new Dictionary<MethodInfo, CallSite>();
        this.m_associateObjectLookup = new ConcurrentDictionary<object, CallSite>();
      }
      if (this.Type == null)
        return;
      this.RegisterEvents();
    }

    public CallSite Get(uint id) => this.m_idToEvent[id];

    public CallSite Get<T>(object associatedObject, Func<T, Delegate> getter, T arg)
    {
      CallSite orAdd;
      if (!this.m_associateObjectLookup.TryGetValue(associatedObject, out orAdd))
      {
        CallSite callSite = this.m_methodInfoLookup[getter(arg).Method];
        orAdd = this.m_associateObjectLookup.GetOrAdd(associatedObject, callSite);
      }
      return orAdd;
    }

    public bool TryGet<T>(
      object associatedObject,
      Func<T, Delegate> getter,
      T arg,
      out CallSite site)
    {
      if (this.m_associateObjectLookup.TryGetValue(associatedObject, out site))
        return true;
      if (!this.m_methodInfoLookup.TryGetValue(getter(arg).Method, out site))
        return false;
      site = this.m_associateObjectLookup.GetOrAdd(associatedObject, site);
      return true;
    }

    public void AddStaticEvents(System.Type fromType) => this.RegisterEvents(fromType, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    private void RegisterEvents() => this.RegisterEvents(this.Type.Type, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    private void RegisterEvents(System.Type type, BindingFlags flags)
    {
      foreach (MyEventTable.EventReturn eventReturn in (IEnumerable<MyEventTable.EventReturn>) ((IEnumerable<MethodInfo>) type.GetMethods(flags)).Select<MethodInfo, MyEventTable.EventReturn>((Func<MethodInfo, MyEventTable.EventReturn>) (s => new MyEventTable.EventReturn(s.GetCustomAttribute<EventAttribute>(), s))).Where<MyEventTable.EventReturn>((Func<MyEventTable.EventReturn, bool>) (s => s.Event != null)).OrderBy<MyEventTable.EventReturn, int>((Func<MyEventTable.EventReturn, int>) (s => s.Event.Order)))
      {
        MethodInfo method = eventReturn.Method;
        System.Type[] typeArray = new System.Type[7]
        {
          method.IsStatic ? typeof (IMyEventOwner) : method.DeclaringType,
          typeof (DBNull),
          typeof (DBNull),
          typeof (DBNull),
          typeof (DBNull),
          typeof (DBNull),
          typeof (DBNull)
        };
        ParameterInfo[] parameters = method.GetParameters();
        for (int index = 0; index < parameters.Length; ++index)
          typeArray[index + 1] = parameters[index].ParameterType;
        CallSite callSite = (CallSite) this.m_createCallSite.MakeGenericMethod(typeArray).Invoke((object) this, new object[2]
        {
          (object) method,
          (object) (uint) this.m_idToEvent.Count
        });
        if ((callSite.HasBroadcastExceptFlag ? 1 : 0) + (callSite.HasBroadcastFlag ? 1 : 0) + (callSite.HasClientFlag ? 1 : 0) > 1)
          throw new InvalidOperationException(string.Format("Event '{0}' can have only one of [Client], [Broadcast], [BroadcastExcept] attributes", (object) callSite));
        this.m_idToEvent.Add(callSite.Id, callSite);
        this.m_methodInfoLookup.Add(method, callSite);
      }
    }

    private CallSite CreateCallSite<T1, T2, T3, T4, T5, T6, T7>(MethodInfo info, uint id)
    {
      CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7> handler = (CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7>) null;
      ICallSite callSite1 = CodegenUtils.GetCallSite(info);
      if (callSite1 is ICallSite<T1, T2, T3, T4, T5, T6, T7> callSite2)
        handler = new CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7>(callSite2.Invoke);
      if (callSite1 == null)
      {
        ParameterExpression[] array = ((IEnumerable<System.Type>) new System.Type[7]
        {
          typeof (T1),
          typeof (T2),
          typeof (T3),
          typeof (T4),
          typeof (T5),
          typeof (T6),
          typeof (T7)
        }).Select<System.Type, ParameterExpression>((Func<System.Type, ParameterExpression>) (s => Expression.Parameter(s.MakeByRefType()))).ToArray<ParameterExpression>();
        handler = Expression.Lambda<CallSiteInvoker<T1, T2, T3, T4, T5, T6, T7>>(!info.IsStatic ? (Expression) Expression.Call((Expression) ((IEnumerable<ParameterExpression>) array).First<ParameterExpression>(), info, (Expression[]) ((IEnumerable<ParameterExpression>) array).Skip<ParameterExpression>(1).Where<ParameterExpression>((Func<ParameterExpression, bool>) (s => s.Type != typeof (DBNull))).ToArray<ParameterExpression>()) : (Expression) Expression.Call(info, (Expression[]) ((IEnumerable<ParameterExpression>) array).Skip<ParameterExpression>(1).Where<ParameterExpression>((Func<ParameterExpression, bool>) (s => s.Type != typeof (DBNull))).ToArray<ParameterExpression>()), array).Compile();
      }
      EventAttribute customAttribute1 = info.GetCustomAttribute<EventAttribute>();
      ServerAttribute customAttribute2 = info.GetCustomAttribute<ServerAttribute>();
      float distanceRadius = 0.0f;
      CallSiteFlags flags = CallSiteFlags.None;
      if (customAttribute2 != null)
        flags |= CallSiteFlags.Server;
      if (customAttribute2 is ServerInvokedAttribute)
        flags |= CallSiteFlags.ServerInvoked;
      if (info.HasAttribute<ClientAttribute>())
        flags |= CallSiteFlags.Client;
      if (info.HasAttribute<BroadcastAttribute>())
        flags |= CallSiteFlags.Broadcast;
      if (info.HasAttribute<BroadcastExceptAttribute>())
        flags |= CallSiteFlags.BroadcastExcept;
      if (info.HasAttribute<ReliableAttribute>())
        flags |= CallSiteFlags.Reliable;
      if (info.HasAttribute<RefreshReplicableAttribute>())
        flags |= CallSiteFlags.RefreshReplicable;
      if (info.HasAttribute<BlockingAttribute>())
        flags |= CallSiteFlags.Blocking;
      if (info.HasAttribute<DistanceRadiusAttribute>())
      {
        distanceRadius = info.GetCustomAttribute<DistanceRadiusAttribute>().DistanceRadius;
        flags |= CallSiteFlags.DistanceRadius;
      }
      SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> serializeDelegate = (SerializeDelegate<T1, T2, T3, T4, T5, T6, T7>) null;
      Func<T1, T2, T3, T4, T5, T6, T7, bool> func = (Func<T1, T2, T3, T4, T5, T6, T7, bool>) null;
      if (customAttribute1.Serialization != null)
      {
        MethodInfo method = info.DeclaringType.GetMethod(customAttribute1.Serialization, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (method == (MethodInfo) null)
          throw new InvalidOperationException(string.Format("Serialization method '{0}' for event '{1}' defined by type '{2}' not found", (object) customAttribute1.Serialization, (object) info.Name, (object) info.DeclaringType.Name));
        if (!((IEnumerable<ParameterInfo>) method.GetParameters()).Skip<ParameterInfo>(1).All<ParameterInfo>((Func<ParameterInfo, bool>) (s => s.ParameterType.IsByRef)))
          throw new InvalidOperationException(string.Format("Serialization method '{0}' for event '{1}' defined by type '{2}' must have all arguments passed with 'ref' keyword (except BitStream)", (object) customAttribute1.Serialization, (object) info.Name, (object) info.DeclaringType.Name));
        ParameterExpression[] parameterExpressionsFrom = MethodInfoExtensions.ExtractParameterExpressionsFrom<SerializeDelegate<T1, T2, T3, T4, T5, T6, T7>>();
        serializeDelegate = Expression.Lambda<SerializeDelegate<T1, T2, T3, T4, T5, T6, T7>>((Expression) Expression.Call((Expression) ((IEnumerable<ParameterExpression>) parameterExpressionsFrom).First<ParameterExpression>(), method, (Expression[]) ((IEnumerable<ParameterExpression>) parameterExpressionsFrom).Skip<ParameterExpression>(1).Where<ParameterExpression>((Func<ParameterExpression, bool>) (s => s.Type != typeof (DBNull))).ToArray<ParameterExpression>()), parameterExpressionsFrom).Compile();
      }
      if (customAttribute2 != null && customAttribute2.Validation != null)
      {
        MethodInfo method = info.DeclaringType.GetMethod(customAttribute2.Validation, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (method == (MethodInfo) null)
          throw new InvalidOperationException(string.Format("Validation method '{0}' for event '{1}' defined by type '{2}' not found", (object) customAttribute2.Validation, (object) info.Name, (object) info.DeclaringType.Name));
        ParameterExpression[] parameterExpressionsFrom = MethodInfoExtensions.ExtractParameterExpressionsFrom<Func<T1, T2, T3, T4, T5, T6, T7, bool>>();
        func = Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, bool>>((Expression) Expression.Call((Expression) ((IEnumerable<ParameterExpression>) parameterExpressionsFrom).First<ParameterExpression>(), method, (Expression[]) ((IEnumerable<ParameterExpression>) parameterExpressionsFrom).Skip<ParameterExpression>(1).Where<ParameterExpression>((Func<ParameterExpression, bool>) (s => s.Type != typeof (DBNull))).ToArray<ParameterExpression>()), parameterExpressionsFrom).Compile();
      }
      ValidationType validationFlags = ValidationType.None;
      if (customAttribute2 != null)
        validationFlags = customAttribute2.ValidationFlags;
      SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> serializer = serializeDelegate ?? this.CreateSerializer<T1, T2, T3, T4, T5, T6, T7>(info);
      Func<T1, T2, T3, T4, T5, T6, T7, bool> validator = func ?? this.CreateValidator<T1, T2, T3, T4, T5, T6, T7>();
      return (CallSite) new CallSite<T1, T2, T3, T4, T5, T6, T7>(this.Type, id, info, flags, handler, serializer, validator, validationFlags, distanceRadius);
    }

    private SerializeDelegate<T1, T2, T3, T4, T5, T6, T7> CreateSerializer<T1, T2, T3, T4, T5, T6, T7>(
      MethodInfo info)
    {
      MySerializer<T2> s2 = MyFactory.GetSerializer<T2>();
      MySerializer<T3> s3 = MyFactory.GetSerializer<T3>();
      MySerializer<T4> s4 = MyFactory.GetSerializer<T4>();
      MySerializer<T5> s5 = MyFactory.GetSerializer<T5>();
      MySerializer<T6> s6 = MyFactory.GetSerializer<T6>();
      MySerializer<T7> s7 = MyFactory.GetSerializer<T7>();
      ParameterInfo[] parameters = info.GetParameters();
      MySerializeInfo info2 = MySerializeInfo.CreateForParameter(parameters, 0);
      MySerializeInfo info3 = MySerializeInfo.CreateForParameter(parameters, 1);
      MySerializeInfo info4 = MySerializeInfo.CreateForParameter(parameters, 2);
      MySerializeInfo info5 = MySerializeInfo.CreateForParameter(parameters, 3);
      MySerializeInfo info6 = MySerializeInfo.CreateForParameter(parameters, 4);
      MySerializeInfo info7 = MySerializeInfo.CreateForParameter(parameters, 5);
      return (SerializeDelegate<T1, T2, T3, T4, T5, T6, T7>) ((T1 inst, BitStream stream, ref T2 arg2, ref T3 arg3, ref T4 arg4, ref T5 arg5, ref T6 arg6, ref T7 arg7) =>
      {
        if (stream.Reading)
        {
          MySerializationHelpers.CreateAndRead<T2>(stream, out arg2, s2, info2);
          MySerializationHelpers.CreateAndRead<T3>(stream, out arg3, s3, info3);
          MySerializationHelpers.CreateAndRead<T4>(stream, out arg4, s4, info4);
          MySerializationHelpers.CreateAndRead<T5>(stream, out arg5, s5, info5);
          MySerializationHelpers.CreateAndRead<T6>(stream, out arg6, s6, info6);
          MySerializationHelpers.CreateAndRead<T7>(stream, out arg7, s7, info7);
        }
        else
        {
          MySerializationHelpers.Write<T2>(stream, ref arg2, s2, info2);
          MySerializationHelpers.Write<T3>(stream, ref arg3, s3, info3);
          MySerializationHelpers.Write<T4>(stream, ref arg4, s4, info4);
          MySerializationHelpers.Write<T5>(stream, ref arg5, s5, info5);
          MySerializationHelpers.Write<T6>(stream, ref arg6, s6, info6);
          MySerializationHelpers.Write<T7>(stream, ref arg7, s7, info7);
        }
      });
    }

    private Func<T1, T2, T3, T4, T5, T6, T7, bool> CreateValidator<T1, T2, T3, T4, T5, T6, T7>() => (Func<T1, T2, T3, T4, T5, T6, T7, bool>) ((a1, a2, a3, a4, a5, a6, a7) => true);

    private class EventReturn
    {
      public MethodInfo Method;
      public EventAttribute Event;

      public EventReturn(EventAttribute _event, MethodInfo _method)
      {
        this.Event = _event;
        this.Method = _method;
      }
    }
  }
}
