// Decompiled with JetBrains decompiler
// Type: VRage.ObjectBuilders.MyObjectFactory`2
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using VRage.Collections;
using VRage.Game.Common;

namespace VRage.ObjectBuilders
{
  public class MyObjectFactory<TAttribute, TCreatedObjectBase>
    where TAttribute : MyFactoryTagAttribute
    where TCreatedObjectBase : class
  {
    private readonly Dictionary<Type, TAttribute> m_attributesByProducedType = new Dictionary<Type, TAttribute>();
    private readonly Dictionary<Type, TAttribute> m_attributesByObjectBuilder = new Dictionary<Type, TAttribute>();
    private readonly FastResourceLock m_activatorsLock = new FastResourceLock();
    private readonly Dictionary<Type, Func<object>> m_activators = new Dictionary<Type, Func<object>>();

    public DictionaryValuesReader<Type, TAttribute> Attributes => new DictionaryValuesReader<Type, TAttribute>(this.m_attributesByProducedType);

    public void RegisterFromCreatedObjectAssembly() => this.RegisterFromAssembly(Assembly.GetAssembly(typeof (TCreatedObjectBase)));

    public void RegisterDescriptor(TAttribute descriptor, Type type)
    {
      descriptor.ProducedType = type;
      if (descriptor.IsMain)
        this.m_attributesByProducedType.Add(descriptor.ProducedType, descriptor);
      if (descriptor.ObjectBuilderType != (Type) null)
      {
        this.m_attributesByObjectBuilder.Add(descriptor.ObjectBuilderType, descriptor);
      }
      else
      {
        if (!typeof (MyObjectBuilder_Base).IsAssignableFrom(descriptor.ProducedType))
          return;
        this.m_attributesByObjectBuilder.Add(descriptor.ProducedType, descriptor);
      }
    }

    public void RegisterFromAssembly(Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        this.RegisterFromAssembly(assembly);
    }

    public void RegisterFromAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      foreach (Type type in assembly.GetTypes())
      {
        foreach (TAttribute customAttribute in type.GetCustomAttributes(typeof (TAttribute), false))
          this.RegisterDescriptor(customAttribute, type);
      }
    }

    public TCreatedObjectBase CreateInstance(MyObjectBuilderType objectBuilderType) => this.CreateInstance<TCreatedObjectBase>(objectBuilderType);

    public TBase CreateInstance<TBase>(MyObjectBuilderType objectBuilderType) where TBase : class, TCreatedObjectBase
    {
      TAttribute attribute;
      if (!this.m_attributesByObjectBuilder.TryGetValue((Type) objectBuilderType, out attribute))
        return default (TBase);
      Func<object> activator;
      using (this.m_activatorsLock.AcquireSharedUsing())
        this.m_activators.TryGetValue(attribute.ProducedType, out activator);
      if (activator == null)
      {
        using (this.m_activatorsLock.AcquireExclusiveUsing())
        {
          if (!this.m_activators.TryGetValue(attribute.ProducedType, out activator))
          {
            activator = ExpressionExtension.CreateActivator<object>(attribute.ProducedType);
            this.m_activators.Add(attribute.ProducedType, activator);
          }
        }
      }
      return activator() as TBase;
    }

    public TBase CreateInstance<TBase>() where TBase : class, TCreatedObjectBase, new() => this.CreateInstance<TBase>((MyObjectBuilderType) typeof (TBase));

    public Type GetProducedType(MyObjectBuilderType objectBuilderType) => this.m_attributesByObjectBuilder[(Type) objectBuilderType].ProducedType;

    public Type TryGetProducedType(MyObjectBuilderType objectBuilderType)
    {
      TAttribute attribute = default (TAttribute);
      return !this.m_attributesByObjectBuilder.TryGetValue((Type) objectBuilderType, out attribute) ? (Type) null : attribute.ProducedType;
    }

    public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(TCreatedObjectBase instance) where TObjectBuilder : MyObjectBuilder_Base => this.CreateObjectBuilder<TObjectBuilder>(instance.GetType());

    public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(Type instanceType) where TObjectBuilder : MyObjectBuilder_Base
    {
      TAttribute attribute;
      return !this.m_attributesByProducedType.TryGetValue(instanceType, out attribute) ? default (TObjectBuilder) : MyObjectBuilderSerializer.CreateNewObject((MyObjectBuilderType) attribute.ObjectBuilderType) as TObjectBuilder;
    }
  }
}
