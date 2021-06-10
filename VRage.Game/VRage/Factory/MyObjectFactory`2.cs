// Decompiled with JetBrains decompiler
// Type: VRage.Factory.MyObjectFactory`2
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Game.Common;
using VRage.Meta;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Factory
{
  public class MyObjectFactory<TAttribute, TCreatedObjectBase> : IMyAttributeIndexer, IMyMetadataIndexer
    where TAttribute : MyFactoryTagAttribute
    where TCreatedObjectBase : class
  {
    protected Dictionary<Type, TAttribute> AttributesByProducedType = new Dictionary<Type, TAttribute>();
    protected Dictionary<Type, TAttribute> AttributesByObjectBuilder = new Dictionary<Type, TAttribute>();
    protected MyObjectFactory<TAttribute, TCreatedObjectBase> Parent;
    private static MyObjectFactory<TAttribute, TCreatedObjectBase> m_instance;

    protected virtual void RegisterDescriptor(TAttribute descriptor, Type type)
    {
      descriptor.ProducedType = type;
      if (!typeof (TCreatedObjectBase).IsAssignableFrom(type))
      {
        MyLog.Default.Critical("Type {0} cannot have factory tag attribute {1}, because it's not assignable to {2}", (object) type, (object) typeof (TAttribute), (object) typeof (TCreatedObjectBase));
      }
      else
      {
        TAttribute attribute;
        if (descriptor.IsMain)
        {
          if (this.AttributesByProducedType.TryGetValue(descriptor.ProducedType, out attribute))
          {
            MyLog.Default.Critical(string.Format("Duplicate factory tag attribute {0} on type {1}. Either remove the duplicate instances or mark only one of the attributes as the main one main.", (object) typeof (TAttribute), (object) type));
            return;
          }
          this.AttributesByProducedType.Add(descriptor.ProducedType, descriptor);
        }
        if (descriptor.ObjectBuilderType != (Type) null)
        {
          if (this.AttributesByObjectBuilder.TryGetValue(descriptor.ObjectBuilderType, out attribute))
            MyLog.Default.Critical("Cannot associate OB {0} with type {1} because it's already associated with {2}.", (object) descriptor.ObjectBuilderType, (object) descriptor.ProducedType, (object) attribute.ProducedType);
          else
            this.AttributesByObjectBuilder.Add(descriptor.ObjectBuilderType, descriptor);
        }
        else
        {
          if (!typeof (MyObjectBuilder_Base).IsAssignableFrom(descriptor.ProducedType))
            return;
          this.AttributesByObjectBuilder.Add(descriptor.ProducedType, descriptor);
        }
      }
    }

    public TCreatedObjectBase CreateInstance(
      MyObjectBuilderType objectBuilderType,
      params object[] args)
    {
      return this.CreateInstance<TCreatedObjectBase>(objectBuilderType, args);
    }

    public TBase CreateInstance<TBase>(MyObjectBuilderType objectBuilderType, params object[] args) where TBase : class, TCreatedObjectBase
    {
      Type type;
      if (!this.TryGetProducedType(objectBuilderType, out type))
        return default (TBase);
      object instance = Activator.CreateInstance(type, args);
      TBase @base = instance as TBase;
      if (instance == null || (object) @base != null)
        return @base;
      MyLog.Default.Critical("Factory product {0} is not an instance of {1}", (object) ((Type) objectBuilderType).FullName, (object) typeof (TBase).FullName);
      return default (TBase);
    }

    public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(TCreatedObjectBase instance) where TObjectBuilder : MyObjectBuilder_Base => this.CreateObjectBuilder<TObjectBuilder>(instance.GetType());

    public TObjectBuilder CreateObjectBuilder<TObjectBuilder>(Type instanceType) where TObjectBuilder : MyObjectBuilder_Base
    {
      TAttribute attr;
      return !this.TryGetAttribute(instanceType, out attr) ? default (TObjectBuilder) : MyObjectBuilderSerializer.CreateNewObject((MyObjectBuilderType) attr.ObjectBuilderType) as TObjectBuilder;
    }

    public IEnumerable<TAttribute> Attributes => this.Parent != null ? this.AttributesByProducedType.Values.Concat<TAttribute>(this.Parent.Attributes) : (IEnumerable<TAttribute>) this.AttributesByProducedType.Values;

    public Type GetProducedType(MyObjectBuilderType objectBuilderType)
    {
      TAttribute attr;
      return this.TryGetAttribute(objectBuilderType, out attr) ? attr.ProducedType : (Type) null;
    }

    public MyObjectBuilderType GetObjectBuilderType(Type type)
    {
      TAttribute attr;
      return this.TryGetAttribute(type, out attr) ? (MyObjectBuilderType) attr.ObjectBuilderType : (MyObjectBuilderType) (Type) null;
    }

    public bool TryGetProducedType(MyObjectBuilderType objectBuilderType, out Type type)
    {
      TAttribute attr;
      if (this.TryGetAttribute(objectBuilderType, out attr))
      {
        type = attr.ProducedType;
        return true;
      }
      type = (Type) null;
      return false;
    }

    public bool TryGetObjectBuilderType(Type type, out MyObjectBuilderType objectBuilderType)
    {
      TAttribute attr;
      if (this.TryGetAttribute(type, out attr))
      {
        objectBuilderType = (MyObjectBuilderType) attr.ObjectBuilderType;
        return true;
      }
      objectBuilderType = (MyObjectBuilderType) (Type) null;
      return false;
    }

    protected TAttribute GetAttribute(Type instanceType, bool inherited = false)
    {
      if (inherited)
      {
        TAttribute attr = default (TAttribute);
        while (instanceType != (Type) null && !this.TryGetAttribute(instanceType, out attr))
          instanceType = instanceType.BaseType;
        return attr;
      }
      TAttribute attr1;
      this.TryGetAttribute(instanceType, out attr1);
      return attr1;
    }

    public bool TryGetAttribute(Type instanceType, out TAttribute attr)
    {
      if (this.AttributesByProducedType.TryGetValue(instanceType, out attr))
        return true;
      return this.Parent != null && this.Parent.TryGetAttribute(instanceType, out attr);
    }

    public bool TryGetAttribute(MyObjectBuilderType builderType, out TAttribute attr)
    {
      if (this.AttributesByObjectBuilder.TryGetValue((Type) builderType, out attr))
        return true;
      return this.Parent != null && this.Parent.TryGetAttribute(builderType, out attr);
    }

    public static MyObjectFactory<TAttribute, TCreatedObjectBase> Get() => MyObjectFactory<TAttribute, TCreatedObjectBase>.m_instance;

    [Obsolete]
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

    [Obsolete]
    public void RegisterFromCreatedObjectAssembly()
    {
    }

    public virtual void SetParent(IMyMetadataIndexer indexer) => this.Parent = (MyObjectFactory<TAttribute, TCreatedObjectBase>) indexer;

    public virtual void Activate() => MyObjectFactory<TAttribute, TCreatedObjectBase>.m_instance = this;

    public virtual void Observe(Attribute attribute, Type type) => this.RegisterDescriptor((TAttribute) attribute, type);

    public virtual void Close()
    {
      this.AttributesByObjectBuilder.Clear();
      this.AttributesByProducedType.Clear();
    }

    public virtual void Process()
    {
    }
  }
}
