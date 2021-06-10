// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatArtificialGravity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
  public class MyStatArtificialGravity : MyStatBase
  {
    public override float MaxValue => float.MaxValue;

    public MyStatArtificialGravity() => this.Id = MyStringHash.GetOrCompute("artificial_gravity");

    public override void Update()
    {
      Vector3D worldPoint = MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyEntity) ? MySector.MainCamera.Position : (MySession.Static.ControlledEntity as MyEntity).PositionComp.WorldAABB.Center;
      this.CurrentValue = MyGravityProviderSystem.CalculateArtificialGravityInPoint(worldPoint, MyGravityProviderSystem.CalculateArtificialGravityStrengthMultiplier(MyGravityProviderSystem.CalculateHighestNaturalGravityMultiplierInPoint(worldPoint))).Length() / 9.81f;
    }
  }
}
