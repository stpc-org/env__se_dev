// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Autopilots.MyAutopilotFactory
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.AI.Autopilots
{
  internal static class MyAutopilotFactory
  {
    private static readonly MyObjectFactory<MyAutopilotTypeAttribute, MyAutopilotBase> m_objectFactory = new MyObjectFactory<MyAutopilotTypeAttribute, MyAutopilotBase>();

    static MyAutopilotFactory() => MyAutopilotFactory.m_objectFactory.RegisterFromCreatedObjectAssembly();

    public static MyAutopilotBase CreateAutopilot(
      MyObjectBuilder_AutopilotBase builder)
    {
      return MyAutopilotFactory.m_objectFactory.CreateInstance(builder.TypeId);
    }

    public static MyObjectBuilder_AutopilotBase CreateObjectBuilder(
      MyAutopilotBase autopilot)
    {
      return MyAutopilotFactory.m_objectFactory.CreateObjectBuilder<MyObjectBuilder_AutopilotBase>(autopilot);
    }
  }
}
