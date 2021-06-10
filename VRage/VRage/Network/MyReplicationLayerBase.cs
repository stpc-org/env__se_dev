// Decompiled with JetBrains decompiler
// Type: VRage.Network.MyReplicationLayerBase
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using VRageMath;

namespace VRage.Network
{
  public abstract class MyReplicationLayerBase
  {
    private static DBNull e = DBNull.Value;
    protected readonly MyTypeTable m_typeTable = new MyTypeTable();
    protected EndpointId m_localEndpoint;

    public void SetLocalEndpoint(EndpointId localEndpoint) => this.m_localEndpoint = localEndpoint;

    public Type GetType(TypeId id) => this.m_typeTable.Get(id).Type;

    public TypeId GetTypeId(Type id) => this.m_typeTable.Get(id).TypeId;

    public DateTime LastMessageFromServer { get; protected set; }

    protected static bool ShouldServerInvokeLocally(
      CallSite site,
      EndpointId localClientEndpoint,
      EndpointId recipient)
    {
      if (site.HasServerFlag)
        return true;
      if (!(recipient == localClientEndpoint))
        return false;
      return site.HasClientFlag || site.HasBroadcastFlag;
    }

    private bool TryGetInstanceCallSite<T>(
      Func<T, Delegate> callSiteGetter,
      T arg,
      out CallSite site)
    {
      return this.m_typeTable.Get(arg.GetType()).EventTable.TryGet<T>((object) callSiteGetter, callSiteGetter, arg, out site);
    }

    private bool TryGetStaticCallSite<T>(Func<T, Delegate> callSiteGetter, out CallSite site) => this.m_typeTable.StaticEventTable.TryGet<T>((object) callSiteGetter, callSiteGetter, default (T), out site);

    private CallSite GetCallSite<T>(Func<T, Delegate> callSiteGetter, T arg)
    {
      CallSite site;
      if ((object) arg == null)
        this.TryGetStaticCallSite<T>(callSiteGetter, out site);
      else
        this.TryGetInstanceCallSite<T>(callSiteGetter, arg, out site);
      if (site != null)
        return site;
      MethodInfo method = callSiteGetter(arg).Method;
      if (!method.HasAttribute<EventAttribute>())
        throw new InvalidOperationException(string.Format("Event '{0}' in type '{1}' is missing attribute '{2}'", (object) method.Name, (object) method.DeclaringType.Name, (object) typeof (EventAttribute).Name));
      if (!method.DeclaringType.HasAttribute<StaticEventOwnerAttribute>() && !typeof (IMyEventProxy).IsAssignableFrom(method.DeclaringType) && !typeof (IMyNetObject).IsAssignableFrom(method.DeclaringType))
        throw new InvalidOperationException(string.Format("Event '{0}' is defined in type '{1}', which does not implement '{2}' or '{3}' or has attribute '{4}'", (object) method.Name, (object) method.DeclaringType.Name, (object) typeof (IMyEventOwner).Name, (object) typeof (IMyNetObject).Name, (object) typeof (StaticEventOwnerAttribute).Name));
      throw new InvalidOperationException(string.Format("Event '{0}' not found, is declaring type '{1}' registered within replication layer?", (object) method.Name, (object) method.DeclaringType.Name));
    }

    public void RaiseEvent<T1, T2>(
      T1 arg1,
      T2 arg2,
      Func<T1, Action> action,
      EndpointId endpointId = default (EndpointId),
      Vector3D? position = null)
      where T1 : IMyEventOwner
      where T2 : IMyEventOwner
    {
      this.DispatchEvent<T1, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull, T2>(this.GetCallSite<T1>((Func<T1, Delegate>) action, arg1), endpointId, position, ref arg1, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref arg2);
    }

    public void RaiseEvent<T1, T2, T3>(
      T1 arg1,
      T3 arg3,
      Func<T1, Action<T2>> action,
      T2 arg2,
      EndpointId endpointId = default (EndpointId),
      Vector3D? position = null)
      where T1 : IMyEventOwner
      where T3 : IMyEventOwner
    {
      this.DispatchEvent<T1, T2, DBNull, DBNull, DBNull, DBNull, DBNull, T3>(this.GetCallSite<T1>((Func<T1, Delegate>) action, arg1), endpointId, position, ref arg1, ref arg2, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref arg3);
    }

    public void RaiseEvent<T1, T2, T3, T4>(
      T1 arg1,
      T4 arg4,
      Func<T1, Action<T2, T3>> action,
      T2 arg2,
      T3 arg3,
      EndpointId endpointId = default (EndpointId),
      Vector3D? position = null)
      where T1 : IMyEventOwner
      where T4 : IMyEventOwner
    {
      this.DispatchEvent<T1, T2, T3, DBNull, DBNull, DBNull, DBNull, T4>(this.GetCallSite<T1>((Func<T1, Delegate>) action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref arg4);
    }

    public void RaiseEvent<T1, T2, T3, T4, T5>(
      T1 arg1,
      T5 arg5,
      Func<T1, Action<T2, T3, T4>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      EndpointId endpointId = default (EndpointId),
      Vector3D? position = null)
      where T1 : IMyEventOwner
      where T5 : IMyEventOwner
    {
      this.DispatchEvent<T1, T2, T3, T4, DBNull, DBNull, DBNull, T5>(this.GetCallSite<T1>((Func<T1, Delegate>) action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref arg4, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref arg5);
    }

    public void RaiseEvent<T1, T2, T3, T4, T5, T6>(
      T1 arg1,
      T6 arg6,
      Func<T1, Action<T2, T3, T4, T5>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      EndpointId endpointId = default (EndpointId),
      Vector3D? position = null)
      where T1 : IMyEventOwner
      where T6 : IMyEventOwner
    {
      this.DispatchEvent<T1, T2, T3, T4, T5, DBNull, DBNull, T6>(this.GetCallSite<T1>((Func<T1, Delegate>) action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref MyReplicationLayerBase.e, ref MyReplicationLayerBase.e, ref arg6);
    }

    public void RaiseEvent<T1, T2, T3, T4, T5, T6, T7>(
      T1 arg1,
      T7 arg7,
      Func<T1, Action<T2, T3, T4, T5, T6>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      T6 arg6,
      EndpointId endpointId = default (EndpointId),
      Vector3D? position = null)
      where T1 : IMyEventOwner
      where T7 : IMyEventOwner
    {
      this.DispatchEvent<T1, T2, T3, T4, T5, T6, DBNull, T7>(this.GetCallSite<T1>((Func<T1, Delegate>) action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref MyReplicationLayerBase.e, ref arg7);
    }

    public void RaiseEvent<T1, T2, T3, T4, T5, T6, T7, T8>(
      T1 arg1,
      T8 arg8,
      Func<T1, Action<T2, T3, T4, T5, T6, T7>> action,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      T6 arg6,
      T7 arg7,
      EndpointId endpointId = default (EndpointId),
      Vector3D? position = null)
      where T1 : IMyEventOwner
      where T8 : IMyEventOwner
    {
      this.DispatchEvent<T1, T2, T3, T4, T5, T6, T7, T8>(this.GetCallSite<T1>((Func<T1, Delegate>) action, arg1), endpointId, position, ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7, ref arg8);
    }

    protected abstract void DispatchEvent<T1, T2, T3, T4, T5, T6, T7, T8>(
      CallSite callSite,
      EndpointId recipient,
      Vector3D? position,
      ref T1 arg1,
      ref T2 arg2,
      ref T3 arg3,
      ref T4 arg4,
      ref T5 arg5,
      ref T6 arg6,
      ref T7 arg7,
      ref T8 arg8)
      where T1 : IMyEventOwner
      where T8 : IMyEventOwner;

    internal void InvokeLocally<T1, T2, T3, T4, T5, T6, T7>(
      CallSite<T1, T2, T3, T4, T5, T6, T7> site,
      T1 arg1,
      T2 arg2,
      T3 arg3,
      T4 arg4,
      T5 arg5,
      T6 arg6,
      T7 arg7)
    {
      using (MyEventContext.Set(this.m_localEndpoint, (MyClientStateBase) null, true))
        site.Invoke(in arg1, in arg2, in arg3, in arg4, in arg5, in arg6, in arg7);
    }

    public void RegisterFromAssembly(IEnumerable<Assembly> assemblies)
    {
      foreach (Assembly assembly in assemblies)
        this.RegisterFromAssembly(assembly);
    }

    public void RegisterFromAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      foreach (Type type in assembly.GetTypes())
      {
        if (MyTypeTable.ShouldRegister(type))
          this.m_typeTable.Register(type);
      }
    }

    public abstract void AdvanceSyncTime();
  }
}
