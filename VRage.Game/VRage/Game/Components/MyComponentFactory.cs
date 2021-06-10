// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyComponentFactory
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Reflection;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace VRage.Game.Components
{
  [PreloadRequired]
  public static class MyComponentFactory
  {
    private static MyObjectFactory<MyComponentBuilderAttribute, MyComponentBase> m_objectFactory = new MyObjectFactory<MyComponentBuilderAttribute, MyComponentBase>();

    static MyComponentFactory()
    {
      MyComponentFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetExecutingAssembly());
      MyComponentFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyComponentFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxGameAssembly);
      MyComponentFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyComponentBase CreateInstanceByTypeId(MyObjectBuilderType type) => MyComponentFactory.m_objectFactory.CreateInstance(type);

    public static MyObjectBuilder_ComponentBase CreateObjectBuilder(
      MyComponentBase instance)
    {
      return MyComponentFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_ComponentBase>(instance);
    }

    public static MyComponentBase CreateInstanceByType(Type type) => type.IsAssignableFrom(typeof (MyComponentBase)) ? Activator.CreateInstance(type) as MyComponentBase : (MyComponentBase) null;

    public static Type GetCreatedInstanceType(MyObjectBuilderType type) => MyComponentFactory.m_objectFactory.GetProducedType(type);

    public static Type TryGetCreatedInstanceType(MyObjectBuilderType type) => MyComponentFactory.m_objectFactory.TryGetProducedType(type);
  }
}
