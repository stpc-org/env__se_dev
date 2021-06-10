// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractModelFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Reflection;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Screens.Models
{
  public class MyContractModelFactory
  {
    private static MyObjectFactory<MyContractModelDescriptor, MyContractModel> m_objectFactory = new MyObjectFactory<MyContractModelDescriptor, MyContractModel>();

    static MyContractModelFactory()
    {
      MyContractModelFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyContractModel)));
      MyContractModelFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyContractModelFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyContractModelFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyContractModel CreateInstance(
      MyObjectBuilder_Contract data,
      bool showFactionIcons = true)
    {
      MyContractModel instance = MyContractModelFactory.m_objectFactory.CreateInstance(data.TypeId);
      instance.Init(data, showFactionIcons);
      return instance;
    }
  }
}
