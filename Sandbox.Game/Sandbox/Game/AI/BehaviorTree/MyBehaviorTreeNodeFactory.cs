// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.BehaviorTree.MyBehaviorTreeNodeFactory
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
  internal static class MyBehaviorTreeNodeFactory
  {
    private static readonly MyObjectFactory<MyBehaviorTreeNodeTypeAttribute, MyBehaviorTreeNode> m_objectFactory = new MyObjectFactory<MyBehaviorTreeNodeTypeAttribute, MyBehaviorTreeNode>();

    static MyBehaviorTreeNodeFactory()
    {
      MyBehaviorTreeNodeFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyBehaviorTreeNode)));
      MyBehaviorTreeNodeFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyBehaviorTreeNodeFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyBehaviorTreeNodeFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyBehaviorTreeNode CreateBTNode(
      MyObjectBuilder_BehaviorTreeNode builder)
    {
      return MyBehaviorTreeNodeFactory.m_objectFactory.CreateInstance(builder.TypeId);
    }

    public static Type GetProducedType(MyObjectBuilderType objectBuilderType) => MyBehaviorTreeNodeFactory.m_objectFactory.GetProducedType(objectBuilderType);
  }
}
