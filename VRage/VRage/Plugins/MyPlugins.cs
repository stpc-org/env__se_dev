// Decompiled with JetBrains decompiler
// Type: VRage.Plugins.MyPlugins
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using VRage.Collections;
using VRage.FileSystem;

namespace VRage.Plugins
{
  public class MyPlugins : IDisposable
  {
    private static List<IPlugin> m_plugins = new List<IPlugin>();
    private static List<IHandleInputPlugin> m_handleInputPlugins = new List<IHandleInputPlugin>();
    private static Assembly m_gamePluginAssembly;
    private static List<Assembly> m_userPluginAssemblies;
    private static Assembly m_sandboxAssembly;
    private static Assembly m_sandboxGameAssembly;
    private static Assembly m_gameObjBuildersPlugin;
    private static Assembly m_gameBaseObjBuildersPlugin;
    private static MyPlugins m_instance;

    public static bool Loaded => MyPlugins.m_instance != null;

    public static ListReader<IPlugin> Plugins => (ListReader<IPlugin>) MyPlugins.m_plugins;

    public static ListReader<IHandleInputPlugin> HandleInputPlugins => (ListReader<IHandleInputPlugin>) MyPlugins.m_handleInputPlugins;

    public static Assembly GameAssembly => !MyPlugins.GameAssemblyReady ? (Assembly) null : MyPlugins.m_gamePluginAssembly;

    public static Assembly GameObjectBuildersAssembly => !MyPlugins.GameObjectBuildersAssemblyReady ? (Assembly) null : MyPlugins.m_gameObjBuildersPlugin;

    public static Assembly GameBaseObjectBuildersAssembly => !MyPlugins.GameBaseObjectBuildersAssemblyReady ? (Assembly) null : MyPlugins.m_gameBaseObjBuildersPlugin;

    public static Assembly[] UserAssemblies => !MyPlugins.UserAssembliesReady ? (Assembly[]) null : MyPlugins.m_userPluginAssemblies.ToArray();

    public static Assembly SandboxAssembly => !MyPlugins.SandboxAssemblyReady ? (Assembly) null : MyPlugins.m_sandboxAssembly;

    public static Assembly SandboxGameAssembly => MyPlugins.m_sandboxGameAssembly == (Assembly) null ? (Assembly) null : MyPlugins.m_sandboxGameAssembly;

    public static bool GameAssemblyReady => MyPlugins.m_gamePluginAssembly != (Assembly) null;

    public static bool GameObjectBuildersAssemblyReady => MyPlugins.m_gameObjBuildersPlugin != (Assembly) null;

    public static bool GameBaseObjectBuildersAssemblyReady => MyPlugins.m_gameBaseObjBuildersPlugin != (Assembly) null;

    public static bool UserAssembliesReady => MyPlugins.m_userPluginAssemblies != null;

    public static bool SandboxAssemblyReady => MyPlugins.m_sandboxAssembly != (Assembly) null;

    public static void RegisterFromArgs(string[] args)
    {
      MyPlugins.m_userPluginAssemblies = (List<Assembly>) null;
      if (args == null)
        return;
      List<string> stringList = new List<string>();
      if (args.Contains<string>("-plugin"))
      {
        for (int index = ((IEnumerable<string>) args).ToList<string>().IndexOf("-plugin"); index + 1 < args.Length && !args[index + 1].StartsWith("-"); ++index)
          stringList.Add(args[index + 1]);
      }
      if (stringList.Count <= 0)
        return;
      MyPlugins.m_userPluginAssemblies = new List<Assembly>(stringList.Count);
      for (int index = 0; index < stringList.Count; ++index)
        MyPlugins.m_userPluginAssemblies.Add(Assembly.LoadFrom(stringList[index]));
    }

    public static void RegisterUserAssemblyFiles(List<string> userAssemblyFiles)
    {
      if (userAssemblyFiles == null)
        return;
      if (MyPlugins.m_userPluginAssemblies == null)
        MyPlugins.m_userPluginAssemblies = new List<Assembly>(userAssemblyFiles.Count);
      foreach (string userAssemblyFile in userAssemblyFiles)
      {
        if (!string.IsNullOrEmpty(userAssemblyFile))
          MyPlugins.m_userPluginAssemblies.Add(Assembly.LoadFrom(userAssemblyFile));
      }
    }

    public static void RegisterGameAssemblyFile(string gameAssemblyFile)
    {
      if (gameAssemblyFile == null)
        return;
      MyPlugins.m_gamePluginAssembly = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, gameAssemblyFile));
    }

    public static void RegisterGameObjectBuildersAssemblyFile(string gameObjBuildersAssemblyFile)
    {
      if (gameObjBuildersAssemblyFile == null)
        return;
      MyPlugins.m_gameObjBuildersPlugin = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, gameObjBuildersAssemblyFile));
    }

    public static void RegisterBaseGameObjectBuildersAssemblyFile(
      string gameBaseObjBuildersAssemblyFile)
    {
      if (gameBaseObjBuildersAssemblyFile == null)
        return;
      MyPlugins.m_gameBaseObjBuildersPlugin = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, gameBaseObjBuildersAssemblyFile));
    }

    public static void RegisterSandboxAssemblyFile(string sandboxAssemblyFile)
    {
      if (sandboxAssemblyFile == null)
        return;
      MyPlugins.m_sandboxAssembly = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, sandboxAssemblyFile));
    }

    public static void RegisterSandboxGameAssemblyFile(string sandboxAssemblyFile)
    {
      if (sandboxAssemblyFile == null)
        return;
      MyPlugins.m_sandboxGameAssembly = Assembly.LoadFrom(Path.Combine(MyFileSystem.ExePath, sandboxAssemblyFile));
    }

    public static void Load()
    {
      try
      {
        if (MyPlugins.m_gamePluginAssembly != (Assembly) null)
          MyPlugins.LoadPlugins(new List<Assembly>()
          {
            MyPlugins.m_gamePluginAssembly
          });
        if (MyPlugins.m_userPluginAssemblies != null)
          MyPlugins.LoadPlugins(MyPlugins.m_userPluginAssemblies);
      }
      catch (Exception ex)
      {
        if (ex is ReflectionTypeLoadException)
        {
          Exception[] loaderExceptions = (ex as ReflectionTypeLoadException).LoaderExceptions;
        }
        throw;
      }
      MyPlugins.m_instance = new MyPlugins();
    }

    private static void LoadPlugins(List<Assembly> assemblies)
    {
      foreach (Assembly assembly in assemblies)
      {
        foreach (Type type in ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (s => s.GetInterfaces().Contains<Type>(typeof (IPlugin)) && !s.IsAbstract)))
        {
          try
          {
            IPlugin instance = (IPlugin) Activator.CreateInstance(type);
            MyPlugins.m_plugins.Add(instance);
            if (instance is IHandleInputPlugin)
              MyPlugins.m_handleInputPlugins.Add((IHandleInputPlugin) instance);
          }
          catch (Exception ex)
          {
            Trace.Fail("Cannot create instance of '" + type.FullName + "': " + ex.ToString());
          }
        }
      }
    }

    public static void Unload()
    {
      foreach (IDisposable plugin in MyPlugins.m_plugins)
        plugin.Dispose();
      MyPlugins.m_plugins.Clear();
      MyPlugins.m_handleInputPlugins.Clear();
      MyPlugins.m_instance.Dispose();
      MyPlugins.m_instance = (MyPlugins) null;
      MyPlugins.m_gamePluginAssembly = (Assembly) null;
      MyPlugins.m_userPluginAssemblies = (List<Assembly>) null;
      MyPlugins.m_sandboxAssembly = (Assembly) null;
      MyPlugins.m_sandboxGameAssembly = (Assembly) null;
      MyPlugins.m_gameObjBuildersPlugin = (Assembly) null;
      MyPlugins.m_gameBaseObjBuildersPlugin = (Assembly) null;
    }

    private MyPlugins()
    {
    }

    ~MyPlugins()
    {
    }

    public void Dispose() => GC.SuppressFinalize((object) this);
  }
}
