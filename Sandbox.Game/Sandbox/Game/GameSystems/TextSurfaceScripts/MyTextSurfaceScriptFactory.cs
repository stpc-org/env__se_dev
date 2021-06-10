// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.TextSurfaceScripts.MyTextSurfaceScriptFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.World;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Collections;
using VRage.Game.ModAPI.Ingame;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems.TextSurfaceScripts
{
  public class MyTextSurfaceScriptFactory
  {
    private static MyTextSurfaceScriptFactory m_instance;
    private Dictionary<string, MyTextSurfaceScriptFactory.ScriptInfo> m_scripts = new Dictionary<string, MyTextSurfaceScriptFactory.ScriptInfo>();

    public DictionaryReader<string, MyTextSurfaceScriptFactory.ScriptInfo> Scripts => (DictionaryReader<string, MyTextSurfaceScriptFactory.ScriptInfo>) this.m_scripts;

    public static MyTextSurfaceScriptFactory Instance => MyTextSurfaceScriptFactory.m_instance;

    public static void LoadScripts()
    {
      if (MyTextSurfaceScriptFactory.m_instance == null)
        MyTextSurfaceScriptFactory.m_instance = new MyTextSurfaceScriptFactory();
      MyTextSurfaceScriptFactory.m_instance.m_scripts.Clear();
      MyTextSurfaceScriptFactory.m_instance.RegisterFromAssembly(Assembly.GetExecutingAssembly());
      MyTextSurfaceScriptFactory.m_instance.RegisterFromAssembly((IEnumerable<Assembly>) MySession.Static.ScriptManager.Scripts.Values);
      MyTextSurfaceScriptFactory.m_instance.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyTextSurfaceScriptFactory.m_instance.RegisterFromAssembly((IEnumerable<Assembly>) MyPlugins.UserAssemblies);
    }

    public static void UnloadScripts()
    {
      if (MyTextSurfaceScriptFactory.m_instance == null)
        return;
      MyTextSurfaceScriptFactory.m_instance.m_scripts.Clear();
      MyTextSurfaceScriptFactory.m_instance = (MyTextSurfaceScriptFactory) null;
    }

    public void RegisterFromAssembly(IEnumerable<Assembly> assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        this.RegisterFromAssembly(assembly);
    }

    public void RegisterFromAssembly(Assembly assembly)
    {
      foreach (TypeInfo definedType in assembly.DefinedTypes)
      {
        if (definedType.ImplementedInterfaces.Contains<Type>(typeof (IMyTextSurfaceScript)) && !((Type) definedType == typeof (MyTextSurfaceScriptBase)))
        {
          MyTextSurfaceScriptAttribute customAttribute = definedType.GetCustomAttribute<MyTextSurfaceScriptAttribute>();
          if (customAttribute != null)
            this.m_scripts[customAttribute.Id] = new MyTextSurfaceScriptFactory.ScriptInfo()
            {
              Id = customAttribute.Id,
              DisplayName = MyStringId.GetOrCompute(customAttribute.DisplayName),
              Type = definedType.AsType()
            };
        }
      }
    }

    public static IMyTextSurfaceScript CreateScript(
      string id,
      IMyTextSurface surface,
      IMyCubeBlock block,
      Vector2 size)
    {
      if (MyTextSurfaceScriptFactory.m_instance == null)
        return (IMyTextSurfaceScript) null;
      MyTextSurfaceScriptFactory.ScriptInfo scriptInfo;
      if (!MyTextSurfaceScriptFactory.m_instance.Scripts.TryGetValue(id, out scriptInfo))
        return (IMyTextSurfaceScript) null;
      return (IMyTextSurfaceScript) Activator.CreateInstance(scriptInfo.Type, (object) surface, (object) block, (object) size);
    }

    public struct ScriptInfo
    {
      public string Id;
      public MyStringId DisplayName;
      public Type Type;
    }
  }
}
