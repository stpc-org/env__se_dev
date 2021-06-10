// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.World;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.Components
{
  public class MyDebugRenderComponent : MyDebugRenderComponentBase
  {
    protected MyEntity Entity;

    public MyDebugRenderComponent(IMyEntity entity) => this.Entity = (MyEntity) entity;

    public override void DebugDrawInvalidTriangles()
    {
      if (this.Entity == null)
        return;
      foreach (MyEntityComponentBase child in this.Entity.Hierarchy.Children)
        child.Container.Entity.DebugDrawInvalidTriangles();
      if (this.Entity.Render.GetModel() == null)
        return;
      int trianglesCount = this.Entity.Render.GetModel().GetTrianglesCount();
      for (int triangleIndex = 0; triangleIndex < trianglesCount; ++triangleIndex)
      {
        MyTriangleVertexIndices triangle = this.Entity.Render.GetModel().GetTriangle(triangleIndex);
        if (MyUtils.IsWrongTriangle(this.Entity.Render.GetModel().GetVertex(triangle.I0), this.Entity.Render.GetModel().GetVertex(triangle.I1), this.Entity.Render.GetModel().GetVertex(triangle.I2)))
        {
          Vector3 vector3_1 = (Vector3) Vector3.Transform(this.Entity.Render.GetModel().GetVertex(triangle.I0), this.Entity.PositionComp.WorldMatrixRef);
          Vector3 vector3_2 = (Vector3) Vector3.Transform(this.Entity.Render.GetModel().GetVertex(triangle.I1), this.Entity.PositionComp.WorldMatrixRef);
          Vector3 vector3_3 = (Vector3) Vector3.Transform(this.Entity.Render.GetModel().GetVertex(triangle.I2), this.Entity.PositionComp.WorldMatrixRef);
          MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_1, (Vector3D) vector3_2, Color.Purple, Color.Purple, false);
          MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_2, (Vector3D) vector3_3, Color.Purple, Color.Purple, false);
          MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_3, (Vector3D) vector3_1, Color.Purple, Color.Purple, false);
          Vector3 vector3_4 = (vector3_1 + vector3_2 + vector3_3) / 3f;
          MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_4, (Vector3D) (vector3_4 + Vector3.UnitX), Color.Yellow, Color.Yellow, false);
          MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_4, (Vector3D) (vector3_4 + Vector3.UnitY), Color.Yellow, Color.Yellow, false);
          MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_4, (Vector3D) (vector3_4 + Vector3.UnitZ), Color.Yellow, Color.Yellow, false);
        }
      }
    }

    public override void DebugDraw()
    {
      if (MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES)
        this.DebugDrawDummies(this.Entity.Render.GetModel());
      if (MyDebugDrawSettings.DEBUG_DRAW_ENTITY_IDS && (this.Entity.Parent == null || !MyDebugDrawSettings.DEBUG_DRAW_ENTITY_IDS_ONLY_ROOT))
        MyRenderProxy.DebugDrawText3D(this.Entity.PositionComp.WorldMatrixRef.Translation, this.Entity.EntityId.ToString("X16"), Color.White, 0.6f, false);
      if (!MyDebugDrawSettings.DEBUG_DRAW_PHYSICS || this.Entity.Physics == null)
        return;
      this.Entity.Physics.DebugDraw();
    }

    protected void DebugDrawDummies(MyModel model)
    {
      if (model == null)
        return;
      float num = 0.0f;
      Vector3D vector3D = Vector3D.Zero;
      if (MySector.MainCamera != null)
      {
        num = MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES_DISTANCE * MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES_DISTANCE;
        vector3D = MySector.MainCamera.WorldMatrix.Translation;
      }
      foreach (KeyValuePair<string, MyModelDummy> dummy in model.Dummies)
      {
        MatrixD matrix = (MatrixD) ref dummy.Value.Matrix * this.Entity.PositionComp.WorldMatrixRef;
        if ((double) num == 0.0 || Vector3D.DistanceSquared(vector3D, matrix.Translation) <= (double) num)
        {
          MyRenderProxy.DebugDrawText3D(matrix.Translation, dummy.Key, Color.White, 0.7f, false);
          MyRenderProxy.DebugDrawAxis(MatrixD.Normalize(matrix), 0.1f, false);
          MyRenderProxy.DebugDrawOBB(matrix, (Color) Vector3.One, 0.1f, false, false);
        }
      }
    }
  }
}
