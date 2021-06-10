// Decompiled with JetBrains decompiler
// Type: Sandbox.Definitions.MyDebugSphere2Definition
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
  [MyDefinitionType(typeof (MyObjectBuilder_DebugSphere2Definition), null)]
  public class MyDebugSphere2Definition : MyCubeBlockDefinition
  {
    private class Sandbox_Definitions_MyDebugSphere2Definition\u003C\u003EActor : IActivator, IActivator<MyDebugSphere2Definition>
    {
      object IActivator.CreateInstance() => (object) new MyDebugSphere2Definition();

      MyDebugSphere2Definition IActivator<MyDebugSphere2Definition>.CreateInstance() => new MyDebugSphere2Definition();
    }
  }
}
