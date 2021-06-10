// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.MyEnvironmentalParticleLogicFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Reflection;
using VRage.Game.ObjectBuilders;
using VRage.ObjectBuilders;
using VRage.Plugins;

namespace Sandbox.Game.World
{
  public class MyEnvironmentalParticleLogicFactory
  {
    private static MyObjectFactory<MyEnvironmentalParticleLogicTypeAttribute, MyEnvironmentalParticleLogic> m_objectFactory = new MyObjectFactory<MyEnvironmentalParticleLogicTypeAttribute, MyEnvironmentalParticleLogic>();

    static MyEnvironmentalParticleLogicFactory()
    {
      MyEnvironmentalParticleLogicFactory.m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof (MyEnvironmentalParticleLogic)));
      MyEnvironmentalParticleLogicFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.GameAssembly);
      MyEnvironmentalParticleLogicFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.SandboxAssembly);
      MyEnvironmentalParticleLogicFactory.m_objectFactory.RegisterFromAssembly(MyPlugins.UserAssemblies);
    }

    public static MyEnvironmentalParticleLogic CreateEnvironmentalParticleLogic(
      MyObjectBuilder_EnvironmentalParticleLogic builder)
    {
      return MyEnvironmentalParticleLogicFactory.m_objectFactory.CreateInstance(builder.TypeId);
    }
  }
}
