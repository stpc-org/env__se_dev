// Decompiled with JetBrains decompiler
// Type: VRage.MyServiceManager
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;

namespace VRage
{
  public sealed class MyServiceManager
  {
    private static readonly MyServiceManager m_singleton = new MyServiceManager();
    private readonly Dictionary<Type, object> m_services;
    private readonly object m_lockObject;

    public event Action<object> OnChanged;

    public static MyServiceManager Instance => MyServiceManager.m_singleton;

    private MyServiceManager()
    {
      this.m_lockObject = new object();
      this.m_services = new Dictionary<Type, object>();
    }

    public void AddService<T>(T serviceInstance) where T : class
    {
      lock (this.m_lockObject)
      {
        this.m_services[typeof (T)] = (object) serviceInstance;
        Action<object> onChanged = this.OnChanged;
        if (onChanged == null)
          return;
        onChanged((object) serviceInstance);
      }
    }

    public T GetService<T>() where T : class
    {
      object obj;
      lock (this.m_lockObject)
        this.m_services.TryGetValue(typeof (T), out obj);
      return obj as T;
    }

    public void RemoveService<T>() where T : class
    {
      lock (this.m_lockObject)
      {
        T service = this.GetService<T>();
        if (!this.m_services.Remove(typeof (T)))
          return;
        Action<object> onChanged = this.OnChanged;
        if (onChanged == null)
          return;
        onChanged((object) service);
      }
    }
  }
}
