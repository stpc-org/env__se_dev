// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Reflection;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Contracts
{
  public static class MyContractFactory
  {
    private static MyObjectFactory<MyContractDescriptor, MyContract> m_objectFactory = new MyObjectFactory<MyContractDescriptor, MyContract>();

    static MyContractFactory()
    {
      MyContractFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyContract)));
      MyContractFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyContractFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyContractFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyContract CreateInstance(MyObjectBuilder_Contract data)
    {
      MyContract instance = MyContractFactory.m_objectFactory.CreateInstance(data.TypeId);
      instance.Init(data);
      return instance;
    }

    public static MyObjectBuilder_Contract CreateObjectBuilder(
      MyContract cont)
    {
      return MyContractFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_Contract>(cont);
    }
  }
}
