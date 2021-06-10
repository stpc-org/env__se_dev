// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyKitchen
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities.Cube;
using VRage.Network;

namespace Sandbox.Game.Entities
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Kitchen))]
  public class MyKitchen : MyCubeBlock
  {
    private class Sandbox_Game_Entities_MyKitchen\u003C\u003EActor : IActivator, IActivator<MyKitchen>
    {
      object IActivator.CreateInstance() => (object) new MyKitchen();

      MyKitchen IActivator<MyKitchen>.CreateInstance() => new MyKitchen();
    }
  }
}
