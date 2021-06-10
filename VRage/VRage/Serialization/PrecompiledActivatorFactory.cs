// Decompiled with JetBrains decompiler
// Type: VRage.Serialization.PrecompiledActivatorFactory
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Library.Extensions;
using VRage.Network;

namespace VRage.Serialization
{
  public sealed class PrecompiledActivatorFactory : IActivatorFactory
  {
    private readonly IActivatorFactory m_fallback = (IActivatorFactory) new ExpressionBaseActivatorFactory();

    public Func<T> CreateActivator<T>()
    {
      IActivator<T> activator = CodegenUtils.GetActivator<T>();
      return activator != null ? new Func<T>(activator.CreateInstance) : this.m_fallback.CreateActivator<T>();
    }

    public Func<T> CreateActivator<T>(Type subtype)
    {
      IActivator activator = CodegenUtils.GetActivator(subtype);
      if (activator == null)
        return this.m_fallback.CreateActivator<T>(subtype);
      return new Func<T>(((PrecompiledActivatorFactory.ActivatorWrapper<T>) Activator.CreateInstance(typeof (PrecompiledActivatorFactory.ActivatorWrapper<,>).MakeGenericType(typeof (T), subtype), (object) activator)).Create);
    }

    private abstract class ActivatorWrapper<TBase>
    {
      public abstract TBase Create();
    }

    private class ActivatorWrapper<TBase, TInstance> : PrecompiledActivatorFactory.ActivatorWrapper<TBase>
      where TInstance : TBase
    {
      private readonly IActivator<TInstance> m_instance;

      public ActivatorWrapper(IActivator<TInstance> instance) => this.m_instance = instance;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public override TBase Create() => (TBase) this.m_instance.CreateInstance();
    }
  }
}
