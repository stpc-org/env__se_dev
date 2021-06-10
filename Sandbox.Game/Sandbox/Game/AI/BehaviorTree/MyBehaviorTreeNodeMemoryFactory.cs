// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeNodeMemoryFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Reflection;
using VRage.Game;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.AI.BehaviorTree
{
  internal static class MyBehaviorTreeNodeMemoryFactory
  {
    private static readonly MyObjectFactory<MyBehaviorTreeNodeMemoryTypeAttribute, MyBehaviorTreeNodeMemory> m_objectFactory = new MyObjectFactory<MyBehaviorTreeNodeMemoryTypeAttribute, MyBehaviorTreeNodeMemory>();

    static MyBehaviorTreeNodeMemoryFactory()
    {
      MyBehaviorTreeNodeMemoryFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyBehaviorTreeNodeMemory)));
      MyBehaviorTreeNodeMemoryFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyBehaviorTreeNodeMemoryFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyBehaviorTreeNodeMemoryFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyBehaviorTreeNodeMemory CreateNodeMemory(
      MyObjectBuilder_BehaviorTreeNodeMemory builder)
    {
      return MyBehaviorTreeNodeMemoryFactory.m_objectFactory.CreateInstance(builder.TypeId);
    }

    public static MyObjectBuilder_BehaviorTreeNodeMemory CreateObjectBuilder(
      MyBehaviorTreeNodeMemory cubeBlock)
    {
      return MyBehaviorTreeNodeMemoryFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_BehaviorTreeNodeMemory>(cubeBlock);
    }

    public static Type GetProducedType(MyObjectBuilderType objectBuilderType) => MyBehaviorTreeNodeMemoryFactory.m_objectFactory.GetProducedType(objectBuilderType);
  }
}
