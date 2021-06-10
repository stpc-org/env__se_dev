// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.MyStatNaturalGravity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.GameSystems;
using Sandbox.Game.World;
using VRage.Game.Entity;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
  public class MyStatNaturalGravity : MyStatBase
  {
    public override float MaxValue => float.MaxValue;

    public MyStatNaturalGravity() => this.Id = MyStringHash.GetOrCompute("natural_gravity");

    public override void Update() => this.CurrentValue = MyGravityProviderSystem.CalculateNaturalGravityInPoint(MySession.Static.ControlledEntity == null || !(MySession.Static.ControlledEntity is MyEntity) ? MySector.MainCamera.Position : (MySession.Static.ControlledEntity as MyEntity).WorldMatrix.Translation).Length() / 9.81f;
  }
}
