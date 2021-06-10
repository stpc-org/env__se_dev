// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Pathfinding.Obsolete.MyBasicObstacle
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using VRage.Game.Entity;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.AI.Pathfinding.Obsolete
{
  public class MyBasicObstacle : IMyObstacle
  {
    public MatrixD m_worldInv;
    public Vector3D m_halfExtents;
    private MyEntity m_entity;

    public bool Valid { get; private set; }

    public MyBasicObstacle(MyEntity entity)
    {
      this.m_entity = entity;
      this.m_entity.OnClosing += new Action<MyEntity>(this.OnEntityClosing);
      this.Update();
      this.Valid = true;
    }

    public bool Contains(ref Vector3D point)
    {
      Vector3D result;
      Vector3D.Transform(ref point, ref this.m_worldInv, out result);
      return Math.Abs(result.X) < this.m_halfExtents.X && Math.Abs(result.Y) < this.m_halfExtents.Y && Math.Abs(result.Z) < this.m_halfExtents.Z;
    }

    public void Update()
    {
      this.m_worldInv = this.m_entity.PositionComp.WorldMatrixNormalizedInv;
      this.m_halfExtents = (Vector3D) this.m_entity.PositionComp.LocalAABB.Extents;
    }

    public void DebugDraw()
    {
      MatrixD matrixD = MatrixD.Invert(this.m_worldInv);
      MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(MatrixD.CreateScale(this.m_halfExtents) * matrixD), Color.Red, 0.3f, false, false);
    }

    private void OnEntityClosing(MyEntity entity)
    {
      this.Valid = false;
      this.m_entity = (MyEntity) null;
    }
  }
}
