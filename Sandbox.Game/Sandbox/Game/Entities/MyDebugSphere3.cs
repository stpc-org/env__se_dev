// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyDebugSphere3
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_DebugSphere3))]
  internal class MyDebugSphere3 : MyFunctionalBlock
  {
    private MyDebugSphere3Definition BlockDefinition => (MyDebugSphere3Definition) base.BlockDefinition;

    private class Sandbox_Game_Entities_MyDebugSphere3\u003C\u003EActor : IActivator, IActivator<MyDebugSphere3>
    {
      object IActivator.CreateInstance() => (object) new MyDebugSphere3();

      MyDebugSphere3 IActivator<MyDebugSphere3>.CreateInstance() => new MyDebugSphere3();
    }
  }
}
