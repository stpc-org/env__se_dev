// Decompiled with JetBrains decompiler
// Type: VRage.Meta.MyMetadataSystem
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using VRage.Library.Collections;
using VRage.Utils;

namespace VRage.Meta
{
  public static class MyMetadataSystem
  {
    private static readonly MyHashSetDictionary<Type, Type> AttributeIndexers = new MyHashSetDictionary<Type, Type>();
    private static readonly List<Type> TypeIndexers = new List<Type>();
    private static readonly List<MyMetadataContext> Stack = new List<MyMetadataContext>();

    public static void LoadAssembly(Assembly assembly, bool batch = false)
    {
      if (MyMetadataSystem.ActiveContext != null)
        MyMetadataSystem.ActiveContext.Index(assembly, batch);
      else
        MyMetadataSystem.Log.Error("Assembly {0} will not be indexed because there are no registered indexers.");
    }

    public static void FinishBatch()
    {
      if (MyMetadataSystem.ActiveContext == null)
        return;
      MyMetadataSystem.ActiveContext.FinishBatch();
    }

    public static Type GetType(string fullName, bool throwOnError)
    {
      Type type1 = Type.GetType(fullName, false);
      if (type1 != (Type) null)
        return type1;
      for (int index = 0; index < MyMetadataSystem.Stack.Count; ++index)
      {
        foreach (Assembly assembly in MyMetadataSystem.Stack[index].Known)
        {
          Type type2;
          if ((type2 = assembly.GetType(fullName, false)) != (Type) null)
            return type2;
        }
      }
      if (throwOnError)
        throw new TypeLoadException(string.Format("Type {0} was not found in any registered assembly!", (object) fullName));
      return (Type) null;
    }

    public static MyMetadataContext ActiveContext => MyMetadataSystem.Stack.LastOrDefault<MyMetadataContext>();

    public static MyLog Log => MyLog.Default;

    public static void PushMetadataContext(MyMetadataContext context)
    {
      context.AddIndexers((IEnumerable<KeyValuePair<Type, HashSet<Type>>>) MyMetadataSystem.AttributeIndexers);
      context.AddIndexers((IEnumerable<Type>) MyMetadataSystem.TypeIndexers);
      MyMetadataContext activeContext = MyMetadataSystem.ActiveContext;
      if (activeContext != null)
        context.Hook(activeContext);
      MyMetadataSystem.Stack.Add(context);
    }

    public static MyMetadataContext PushMetadataContext()
    {
      MyMetadataContext context = new MyMetadataContext();
      MyMetadataSystem.PushMetadataContext(context);
      return context;
    }

    public static void PopContext()
    {
      if (MyMetadataSystem.Stack.Count == 0)
        MyMetadataSystem.Log.Error("When popping metadata context: No context set.");
      else
        MyMetadataSystem.Stack.Pop<MyMetadataContext>().Close();
    }

    public static void RegisterAttributeIndexer(Type attributeType, Type indexerType)
    {
      if (!typeof (IMyAttributeIndexer).IsAssignableFrom(indexerType))
        MyMetadataSystem.Log.Error("Cannot register metadata indexer {0}, the type is not a IMyMetadataIndexer.", (object) indexerType);
      else if (!indexerType.HasDefaultConstructor())
        MyMetadataSystem.Log.Error("Cannot register metadata indexer {0}, the type does not define a parameterless constructor.", (object) indexerType);
      else if (!typeof (Attribute).IsAssignableFrom(attributeType))
      {
        MyMetadataSystem.Log.Error("Cannot register metadata indexer {0}, the indexed attribute {1} is not actually an attribute.", (object) indexerType, (object) attributeType);
      }
      else
      {
        MyMetadataSystem.AttributeIndexers.Add(attributeType, indexerType);
        foreach (MyMetadataContext myMetadataContext in MyMetadataSystem.Stack)
          myMetadataContext.AddIndexer(attributeType, indexerType);
      }
    }

    public static void RegisterTypeIndexer(Type indexerType)
    {
      if (!typeof (IMyTypeIndexer).IsAssignableFrom(indexerType))
        MyMetadataSystem.Log.Error("Cannot register metadata indexer {0}, the type is not a IMyMetadataIndexer.", (object) indexerType);
      else if (!indexerType.HasDefaultConstructor())
      {
        MyMetadataSystem.Log.Error("Cannot register metadata indexer {0}, the type does not define a parameterless constructor.", (object) indexerType);
      }
      else
      {
        MyMetadataSystem.TypeIndexers.Add(indexerType);
        foreach (MyMetadataContext myMetadataContext in MyMetadataSystem.Stack)
          myMetadataContext.AddIndexer(indexerType);
      }
    }
  }
}
