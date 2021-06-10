// Decompiled with JetBrains decompiler
// Type: VRage.Meta.MyMetadataContext
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Collections;
using VRage.Library.Collections;

namespace VRage.Meta
{
  public class MyMetadataContext
  {
    protected readonly Dictionary<Type, IMyMetadataIndexer> Indexers = new Dictionary<Type, IMyMetadataIndexer>();
    protected readonly MyListDictionary<Type, IMyAttributeIndexer> AttributeIndexers = new MyListDictionary<Type, IMyAttributeIndexer>();
    protected readonly List<IMyTypeIndexer> TypeIndexers = new List<IMyTypeIndexer>();
    protected readonly HashSet<Assembly> KnownAssemblies = new HashSet<Assembly>();
    public bool RegisterIndexers = true;

    public HashSetReader<Assembly> Known => (HashSetReader<Assembly>) this.KnownAssemblies;

    protected internal virtual void Activate()
    {
      foreach (IMyMetadataIndexer myMetadataIndexer in this.Indexers.Values)
        myMetadataIndexer.Activate();
    }

    protected internal virtual void Close()
    {
      foreach (IMyMetadataIndexer myMetadataIndexer in this.Indexers.Values)
        myMetadataIndexer.Close();
      this.AttributeIndexers.Clear();
      this.KnownAssemblies.Clear();
      this.Indexers.Clear();
    }

    protected internal virtual void Index(Assembly assembly, bool batch = false)
    {
      if (this.KnownAssemblies.Contains(assembly))
        return;
      this.KnownAssemblies.Add(assembly);
      if (this.RegisterIndexers)
        this.PreProcess(assembly);
      foreach (Module loadedModule in assembly.GetLoadedModules())
      {
        foreach (Type type in loadedModule.GetTypes())
        {
          Attribute[] customAttributes = Attribute.GetCustomAttributes((MemberInfo) type);
          for (int index = 0; index < customAttributes.Length; ++index)
          {
            List<IMyAttributeIndexer> list;
            if (this.AttributeIndexers.TryGet(customAttributes[index].GetType(), out list))
            {
              foreach (IMyAttributeIndexer attributeIndexer in list)
                attributeIndexer.Observe(customAttributes[index], type);
            }
          }
          for (int index = 0; index < this.TypeIndexers.Count; ++index)
            this.TypeIndexers[index].Index(type);
        }
      }
      if (batch)
        return;
      this.FinishBatch();
    }

    internal void Index(IEnumerable<Assembly> assemblies, bool batch = false)
    {
      foreach (Assembly assembly in assemblies)
        this.Index(assembly);
      if (batch)
        return;
      this.FinishBatch();
    }

    public void FinishBatch()
    {
      foreach (IMyMetadataIndexer myMetadataIndexer in this.Indexers.Values)
        myMetadataIndexer.Process();
    }

    internal void AddIndexer(Type attributeType, Type indexerType)
    {
      IMyMetadataIndexer metaIndexer = this.GetMetaIndexer(indexerType);
      this.AttributeIndexers.Add(attributeType, (IMyAttributeIndexer) metaIndexer);
      metaIndexer.Activate();
    }

    private IMyMetadataIndexer GetMetaIndexer(Type indexerType)
    {
      IMyMetadataIndexer instance;
      if (!this.Indexers.TryGetValue(indexerType, out instance))
      {
        instance = (IMyMetadataIndexer) Activator.CreateInstance(indexerType);
        this.Indexers.Add(indexerType, instance);
      }
      return instance;
    }

    internal void AddIndexers(
      IEnumerable<KeyValuePair<Type, HashSet<Type>>> indexerTypes)
    {
      foreach (KeyValuePair<Type, HashSet<Type>> indexerType1 in indexerTypes)
      {
        foreach (Type indexerType2 in indexerType1.Value)
          this.AddIndexer(indexerType1.Key, indexerType2);
      }
    }

    internal void AddIndexer(Type typeIndexer)
    {
      IMyMetadataIndexer metaIndexer = this.GetMetaIndexer(typeIndexer);
      this.TypeIndexers.Add((IMyTypeIndexer) metaIndexer);
      metaIndexer.Activate();
    }

    internal void AddIndexers(IEnumerable<Type> typeIndexers)
    {
      foreach (Type typeIndexer in typeIndexers)
        this.AddIndexer(typeIndexer);
    }

    public void Hook(MyMetadataContext parent)
    {
      foreach (KeyValuePair<Type, IMyMetadataIndexer> indexer1 in this.Indexers)
      {
        IMyMetadataIndexer indexer2;
        if (parent.Indexers.TryGetValue(indexer1.Key, out indexer2))
          indexer1.Value.SetParent(indexer2);
      }
    }

    private void PreProcess(Assembly assembly)
    {
      foreach (Module loadedModule in assembly.GetLoadedModules())
      {
        foreach (Type type1 in loadedModule.GetTypes())
        {
          if (type1.HasAttribute<PreloadRequiredAttribute>())
            RuntimeHelpers.RunClassConstructor(type1.TypeHandle);
          foreach (MyAttributeMetadataIndexerAttributeBase customAttribute in type1.GetCustomAttributes<MyAttributeMetadataIndexerAttributeBase>())
          {
            Type attributeType = customAttribute.AttributeType;
            Type type2 = customAttribute.TargetType;
            if ((object) type2 == null)
              type2 = type1;
            Type indexerType = type2;
            MyMetadataSystem.RegisterAttributeIndexer(attributeType, indexerType);
          }
          if (type1.GetCustomAttribute<MyTypeMetadataIndexerAttribute>() != null)
            MyMetadataSystem.RegisterTypeIndexer(type1);
        }
      }
    }

    public bool TryGetIndexer(Type type, out IMyMetadataIndexer indexer) => this.Indexers.TryGetValue(type, out indexer);
  }
}
