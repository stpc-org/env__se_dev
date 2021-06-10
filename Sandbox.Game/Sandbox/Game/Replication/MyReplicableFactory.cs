// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Replication.MyReplicableFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage;
using VRage.Collections;
using VRage.Plugins;

namespace Sandbox.Game.Replication
{
  public class MyReplicableFactory
  {
    private readonly MyConcurrentDictionary<Type, Type> m_objTypeToExternalReplicableType = new MyConcurrentDictionary<Type, Type>(32);

    public MyReplicableFactory()
    {
      Assembly[] assemblyArray = new Assembly[4]
      {
        typeof (MySandboxGame).Assembly,
        MyPlugins.GameAssembly,
        MyPlugins.SandboxAssembly,
        MyPlugins.SandboxGameAssembly
      };
      if (MyPlugins.UserAssemblies != null)
        assemblyArray = ((IEnumerable<Assembly>) assemblyArray).Union<Assembly>((IEnumerable<Assembly>) MyPlugins.UserAssemblies).ToArray<Assembly>();
      this.RegisterFromAssemblies((IEnumerable<Assembly>) assemblyArray);
    }

    private void RegisterFromAssemblies(IEnumerable<Assembly> assemblies)
    {
      foreach (Assembly assembly in assemblies.Where<Assembly>((Func<Assembly, bool>) (x => x != (Assembly) null)))
        this.RegisterFromAssembly(assembly);
    }

    private void RegisterFromAssembly(Assembly assembly)
    {
      foreach (Type type in ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => typeof (MyExternalReplicable).IsAssignableFrom(t) && !t.IsAbstract)))
      {
        Type baseTypeArgument = type.FindGenericBaseTypeArgument(typeof (MyExternalReplicable<>));
        if (baseTypeArgument != (Type) null && !this.m_objTypeToExternalReplicableType.ContainsKey(baseTypeArgument))
          this.m_objTypeToExternalReplicableType.TryAdd(baseTypeArgument, type);
      }
    }

    public Type FindTypeFor(object obj)
    {
      Type type1 = obj.GetType();
      if (type1.IsValueType)
        throw new InvalidOperationException("obj cannot be value type");
      Type type2 = (Type) null;
      Type key = type1;
      while (key != typeof (object) && !this.m_objTypeToExternalReplicableType.TryGetValue(key, out type2))
        key = key.BaseType;
      if (type1 != key)
        this.m_objTypeToExternalReplicableType.TryAdd(type1, type2);
      return type2;
    }
  }
}
