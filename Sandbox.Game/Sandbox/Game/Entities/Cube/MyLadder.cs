// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyLadder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Character;
using System;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity.UseObject;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Network;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Ladder2))]
  public class MyLadder : MyCubeBlock
  {
    private Matrix m_detectorBox = Matrix.Identity;

    public event Action<MyCubeGrid> CubeGridChanged;

    public Matrix StartMatrix { get; private set; }

    public Matrix StopMatrix { get; private set; }

    public float DistanceBetweenPoles { get; private set; }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.StartMatrix = Matrix.Identity;
      this.OnModelChange();
      this.AddDebugRenderComponent((MyDebugRenderComponentBase) new MyDebugRenderComponentLadder((IMyEntity) this));
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      MyModelDummy myModelDummy1;
      if (this.Model.Dummies.TryGetValue("astronaut", out myModelDummy1))
        this.StartMatrix = myModelDummy1.Matrix;
      if (this.Model.Dummies.TryGetValue("detector_ladder_01", out myModelDummy1))
        this.m_detectorBox = myModelDummy1.Matrix;
      if (this.Model.Dummies.TryGetValue("TopLadder", out myModelDummy1))
        this.StopMatrix = myModelDummy1.Matrix;
      MyModelDummy myModelDummy2;
      if (!this.Model.Dummies.TryGetValue("pole_1", out myModelDummy1) || !this.Model.Dummies.TryGetValue("pole_2", out myModelDummy2))
        return;
      this.DistanceBetweenPoles = Math.Abs(myModelDummy1.Matrix.Translation.Y - myModelDummy2.Matrix.Translation.Y);
    }

    public override bool GetIntersectionWithLine(
      ref LineD line,
      out MyIntersectionResultLineTriangleEx? t,
      IntersectionFlags flags = IntersectionFlags.ALL_TRIANGLES)
    {
      MyOrientedBoundingBoxD orientedBoundingBoxD = new MyOrientedBoundingBoxD((MatrixD) ref this.m_detectorBox * this.PositionComp.WorldMatrixRef);
      t = new MyIntersectionResultLineTriangleEx?();
      double? nullable = orientedBoundingBoxD.Intersects(ref line);
      if (nullable.HasValue)
      {
        MyIntersectionResultLineTriangleEx resultLineTriangleEx = new MyIntersectionResultLineTriangleEx()
        {
          Entity = (IMyEntity) this,
          IntersectionPointInWorldSpace = line.From + (nullable.Value + 0.2) * line.Direction
        };
        resultLineTriangleEx.IntersectionPointInObjectSpace = (Vector3) Vector3D.Transform(resultLineTriangleEx.IntersectionPointInWorldSpace, this.PositionComp.WorldMatrixInvScaled);
        resultLineTriangleEx.NormalInWorldSpace = (Vector3) -line.Direction;
        resultLineTriangleEx.NormalInObjectSpace = (Vector3) Vector3D.TransformNormal(resultLineTriangleEx.NormalInWorldSpace, this.PositionComp.WorldMatrixInvScaled);
        t = new MyIntersectionResultLineTriangleEx?(resultLineTriangleEx);
      }
      return t.HasValue;
    }

    public void Use(UseActionEnum actionEnum, IMyEntity entity)
    {
      if (!(entity is MyCharacter myCharacter) || !Sandbox.Game.Entities.MyEntities.IsInsideWorld(myCharacter.PositionComp.GetPosition()) || !Sandbox.Game.Entities.MyEntities.IsInsideWorld(this.PositionComp.GetPosition()))
        return;
      myCharacter.GetOnLadder(this);
    }

    public override void OnCubeGridChanged(MyCubeGrid oldGrid)
    {
      base.OnCubeGridChanged(oldGrid);
      if (this.CubeGridChanged == null)
        return;
      this.CubeGridChanged(oldGrid);
    }

    private class Sandbox_Game_Entities_Cube_MyLadder\u003C\u003EActor : IActivator, IActivator<MyLadder>
    {
      object IActivator.CreateInstance() => (object) new MyLadder();

      MyLadder IActivator<MyLadder>.CreateInstance() => new MyLadder();
    }
  }
}
