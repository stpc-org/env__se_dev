// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyDebugSphere1
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using VRage.Game;
using VRage.Network;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_DebugSphere1))]
  internal class MyDebugSphere1 : MyFunctionalBlock
  {
    private MyDebugSphere1Definition BlockDefinition => (MyDebugSphere1Definition) base.BlockDefinition;

    private class Sandbox_Game_Entities_MyDebugSphere1\u003C\u003EActor : IActivator, IActivator<MyDebugSphere1>
    {
      object IActivator.CreateInstance() => (object) new MyDebugSphere1();

      MyDebugSphere1 IActivator<MyDebugSphere1>.CreateInstance() => new MyDebugSphere1();
    }
  }
}
