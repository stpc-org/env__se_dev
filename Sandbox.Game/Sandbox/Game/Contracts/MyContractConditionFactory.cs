// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractConditionFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Reflection;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Contracts
{
  public static class MyContractConditionFactory
  {
    private static MyObjectFactory<MyContractConditionDescriptor, MyContractCondition> m_objectFactory = new MyObjectFactory<MyContractConditionDescriptor, MyContractCondition>();

    static MyContractConditionFactory()
    {
      MyContractConditionFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyContractCondition)));
      MyContractConditionFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyContractConditionFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyContractConditionFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyContractCondition CreateInstance(
      MyObjectBuilder_ContractCondition data)
    {
      MyContractCondition instance = MyContractConditionFactory.m_objectFactory.CreateInstance(data.TypeId);
      instance.Init(data);
      return instance;
    }

    public static MyObjectBuilder_ContractCondition CreateObjectBuilder(
      MyContractCondition cont)
    {
      return MyContractConditionFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_ContractCondition>(cont);
    }
  }
}
