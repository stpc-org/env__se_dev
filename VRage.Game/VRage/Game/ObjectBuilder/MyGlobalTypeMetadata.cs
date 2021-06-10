// Decompiled with JetBrains decompiler
// Type: VRage.Game.ObjectBuilder.MyGlobalTypeMetadata
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace VRage.Game.ObjectBuilder
{
  public class MyGlobalTypeMetadata
  {
    public static MyGlobalTypeMetadata Static = new MyGlobalTypeMetadata();
    private HashSet<Assembly> m_assemblies = new HashSet<Assembly>();
    private bool m_ready;

    public void RegisterAssembly(Assembly[] assemblies)
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        this.RegisterAssembly(assembly);
    }

    public void RegisterAssembly(Assembly assembly)
    {
      if (assembly == (Assembly) null)
        return;
      this.m_assemblies.Add(assembly);
      MyObjectBuilderSerializer.RegisterFromAssembly(assembly);
      MyObjectBuilderType.RegisterFromAssembly(assembly, true);
      MyXmlSerializerManager.RegisterFromAssembly(assembly);
      MyDefinitionManagerBase.RegisterTypesFromAssembly(assembly);
    }

    public Type GetType(string fullName, bool throwOnError)
    {
      foreach (Assembly assembly in this.m_assemblies)
      {
        Type type;
        if ((type = assembly.GetType(fullName, false)) != (Type) null)
          return type;
      }
      if (throwOnError)
        throw new TypeLoadException(string.Format("Type {0} was not found in any registered assembly!", (object) fullName));
      return (Type) null;
    }

    public void Init(bool loadSerializersAsync = true)
    {
      if (this.m_ready)
        return;
      MyXmlSerializerManager.RegisterSerializableBaseType(typeof (MyObjectBuilder_Base));
      this.RegisterAssembly(this.GetType().Assembly);
      this.RegisterAssembly(MyPlugins.GameAssembly);
      this.RegisterAssembly(MyPlugins.SandboxGameAssembly);
      this.RegisterAssembly(MyPlugins.SandboxAssembly);
      this.RegisterAssembly(MyPlugins.UserAssemblies);
      this.RegisterAssembly(MyPlugins.GameBaseObjectBuildersAssembly);
      this.RegisterAssembly(MyPlugins.GameObjectBuildersAssembly);
      foreach (object plugin in MyPlugins.Plugins)
        this.RegisterAssembly(plugin.GetType().Assembly);
      if (loadSerializersAsync)
        Task.Factory.StartNew((Action) (() => MyObjectBuilderSerializer.LoadSerializers()));
      else
        MyObjectBuilderSerializer.LoadSerializers();
      this.m_ready = true;
    }
  }
}
