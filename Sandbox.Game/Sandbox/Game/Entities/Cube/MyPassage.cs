// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyPassage
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Passage))]
  public class MyPassage : MyCubeBlock, Sandbox.ModAPI.IMyPassage, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyPassage
  {
    public override bool GetIntersectionWithSphere(ref BoundingSphereD sphere) => false;

    private class Sandbox_Game_Entities_Cube_MyPassage\u003C\u003EActor : IActivator, IActivator<MyPassage>
    {
      object IActivator.CreateInstance() => (object) new MyPassage();

      MyPassage IActivator<MyPassage>.CreateInstance() => new MyPassage();
    }
  }
}
