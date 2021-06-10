// Decompiled with JetBrains decompiler
// Type: VRage.Game.Entity.UseObject.MyUseObjectFactory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using VRage.FileSystem;
using VRage.ModAPI;
using VRage.Plugins;
using VRageRender.Import;

namespace VRage.Game.Entity.UseObject
{
  [PreloadRequired]
  public static class MyUseObjectFactory
  {
    private static Dictionary<string, Type> m_useObjectTypesByDummyName = new Dictionary<string, Type>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    static MyUseObjectFactory()
    {
      MyUseObjectFactory.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
      MyUseObjectFactory.RegisterAssemblyTypes(MyPlugins.GameAssembly);
      MyUseObjectFactory.RegisterAssemblyTypes(MyPlugins.SandboxAssembly);
      MyUseObjectFactory.RegisterAssemblyTypes(MyPlugins.UserAssemblies);
      MyUseObjectFactory.RegisterAssemblyTypes(Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, "Sandbox.Game.dll")));
    }

    private static void RegisterAssemblyTypes(Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MyUseObjectFactory.RegisterAssemblyTypes(assembly);
    }

    private static void RegisterAssemblyTypes(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      Type type1 = typeof (IMyUseObject);
      foreach (Type type2 in assembly.GetTypes())
      {
        if (type1.IsAssignableFrom(type2))
        {
          MyUseObjectAttribute[] customAttributes = (MyUseObjectAttribute[]) type2.GetCustomAttributes(typeof (MyUseObjectAttribute), false);
          if (!customAttributes.IsNullOrEmpty<MyUseObjectAttribute>())
          {
            foreach (MyUseObjectAttribute useObjectAttribute in customAttributes)
              MyUseObjectFactory.m_useObjectTypesByDummyName[useObjectAttribute.DummyName] = type2;
          }
        }
      }
    }

    [Conditional("DEBUG")]
    private static void AssertHasCorrectCtor(Type type)
    {
      foreach (MethodBase constructor in type.GetConstructors())
      {
        ParameterInfo[] parameters = constructor.GetParameters();
        if (parameters.Length == 4 && parameters[0].ParameterType == typeof (IMyEntity) && (parameters[1].ParameterType == typeof (string) && parameters[2].ParameterType == typeof (MyModelDummy)) && parameters[3].ParameterType == typeof (uint))
          break;
      }
    }

    public static IMyUseObject CreateUseObject(
      string detectorName,
      IMyEntity owner,
      string dummyName,
      MyModelDummy dummyData,
      uint shapeKey)
    {
      Type type;
      if (!MyUseObjectFactory.m_useObjectTypesByDummyName.TryGetValue(detectorName, out type) || type == (Type) null)
        return (IMyUseObject) null;
      return (IMyUseObject) Activator.CreateInstance(type, (object) owner, (object) dummyName, (object) dummyData, (object) shapeKey);
    }
  }
}
