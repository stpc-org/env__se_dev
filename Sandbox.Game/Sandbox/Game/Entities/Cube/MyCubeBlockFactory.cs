// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeBlockFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Reflection;
using VRage.Game;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.Entities.Cube
{
  internal static class MyCubeBlockFactory
  {
    private static MyObjectFactory<MyCubeBlockTypeAttribute, object> m_objectFactory = new MyObjectFactory<MyCubeBlockTypeAttribute, object>();

    static MyCubeBlockFactory()
    {
      MyCubeBlockFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyCubeBlock)));
      MyCubeBlockFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyCubeBlockFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyCubeBlockFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static object CreateCubeBlock(MyObjectBuilder_CubeBlock builder)
    {
      object instance = MyCubeBlockFactory.m_objectFactory.CreateInstance(builder.TypeId);
      if (!(instance is MyEntity entity))
        return instance;
      MyEntityFactory.AddScriptGameLogic(entity, builder.TypeId, builder.SubtypeName);
      return instance;
    }

    public static MyObjectBuilder_CubeBlock CreateObjectBuilder(
      MyCubeBlock cubeBlock)
    {
      return (MyObjectBuilder_CubeBlock) MyObjectBuilderSerializer.CreateNewObject((SerializableDefinitionId) cubeBlock.BlockDefinition.Id);
    }

    public static Type GetProducedType(MyObjectBuilderType objectBuilderType) => MyCubeBlockFactory.m_objectFactory.GetProducedType(objectBuilderType);
  }
}
