// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyPlanter
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using VRage.Network;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Planter))]
  public class MyPlanter : MyCubeBlock
  {
    private class Sandbox_Game_Entities_MyPlanter\u003C\u003EActor : IActivator, IActivator<MyPlanter>
    {
      object IActivator.CreateInstance() => (object) new MyPlanter();

      MyPlanter IActivator<MyPlanter>.CreateInstance() => new MyPlanter();
    }
  }
}
