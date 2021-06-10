// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Models.MyContractConditionModelFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Reflection;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Screens.Models
{
  public class MyContractConditionModelFactory
  {
    private static MyObjectFactory<MyContractConditionModelDescriptor, MyContractConditionModel> m_objectFactory = new MyObjectFactory<MyContractConditionModelDescriptor, MyContractConditionModel>();

    static MyContractConditionModelFactory()
    {
      MyContractConditionModelFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyContractConditionModel)));
      MyContractConditionModelFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyContractConditionModelFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyContractConditionModelFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyContractConditionModel CreateInstance(
      MyObjectBuilder_ContractCondition data)
    {
      MyContractConditionModel instance = MyContractConditionModelFactory.m_objectFactory.CreateInstance(data.TypeId);
      instance.Init(data);
      return instance;
    }
  }
}
