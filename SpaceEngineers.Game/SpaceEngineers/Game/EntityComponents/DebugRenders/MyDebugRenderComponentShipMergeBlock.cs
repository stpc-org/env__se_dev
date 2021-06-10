// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.EntityComponents.DebugRenders.MyDebugRenderComponentShipMergeBlock
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using SpaceEngineers.Game.Entities.Blocks;
using System;
using VRage.ModAPI;
using VRageMath;
using VRageRender;

namespace SpaceEngineers.Game.EntityComponents.DebugRenders
{
  public class MyDebugRenderComponentShipMergeBlock : MyDebugRenderComponent
  {
    private MyShipMergeBlock m_shipMergeBlock;

    public MyDebugRenderComponentShipMergeBlock(MyShipMergeBlock shipConnector)
      : base((IMyEntity) shipConnector)
      => this.m_shipMergeBlock = shipConnector;

    public override void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_CONNECTORS_AND_MERGE_BLOCKS)
        return;
      Matrix matrix1 = (Matrix) ref this.m_shipMergeBlock.PositionComp.WorldMatrixRef;
      MyRenderProxy.DebugDrawLine3D((Vector3D) this.m_shipMergeBlock.Physics.RigidBody.Position, this.m_shipMergeBlock.Physics.RigidBody.Position + this.m_shipMergeBlock.WorldMatrix.Right, Color.Green, Color.Green, false);
      Vector3 position = this.m_shipMergeBlock.Position * this.m_shipMergeBlock.CubeGrid.GridSize;
      MatrixD worldMatrix1 = this.m_shipMergeBlock.WorldMatrix;
      Matrix matrix2 = Matrix.Invert((Matrix) ref worldMatrix1);
      MyRenderProxy.DebugDrawSphere((Vector3D) Vector3.Transform(position, matrix2), 1f, Color.Green, depthRead: false);
      MyRenderProxy.DebugDrawSphere(this.m_shipMergeBlock.WorldMatrix.Translation, 0.2f, this.m_shipMergeBlock.InConstraint ? Color.Yellow : Color.Orange, depthRead: false);
      if (this.m_shipMergeBlock.InConstraint)
      {
        MyRenderProxy.DebugDrawSphere(this.m_shipMergeBlock.Other.WorldMatrix.Translation, 0.2f, Color.Yellow, depthRead: false);
        MyRenderProxy.DebugDrawLine3D(this.m_shipMergeBlock.WorldMatrix.Translation, this.m_shipMergeBlock.Other.WorldMatrix.Translation, Color.Yellow, Color.Yellow, false);
      }
      MyRenderProxy.DebugDrawLine3D((Vector3D) matrix1.Translation, matrix1.Translation + this.m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(Base6Directions.GetDirection(this.m_shipMergeBlock.PositionComp.LocalMatrixRef.Right)), Color.Red, Color.Red, false);
      MyRenderProxy.DebugDrawLine3D((Vector3D) matrix1.Translation, matrix1.Translation + this.m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(Base6Directions.GetDirection(this.m_shipMergeBlock.PositionComp.LocalMatrixRef.Up)), Color.Green, Color.Green, false);
      MyRenderProxy.DebugDrawLine3D((Vector3D) matrix1.Translation, matrix1.Translation + this.m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(Base6Directions.GetDirection(this.m_shipMergeBlock.PositionComp.LocalMatrixRef.Backward)), Color.Blue, Color.Blue, false);
      MyRenderProxy.DebugDrawLine3D((Vector3D) matrix1.Translation, matrix1.Translation + this.m_shipMergeBlock.CubeGrid.WorldMatrix.GetDirectionVector(this.m_shipMergeBlock.OtherRight), Color.Violet, Color.Violet, false);
      MyRenderProxy.DebugDrawText3D((Vector3D) matrix1.Translation, "Bodies: " + (object) this.m_shipMergeBlock.GridCount, Color.White, 1f, false);
      if (this.m_shipMergeBlock.Other == null)
        return;
      Vector3 translation1 = matrix1.Translation;
      MatrixD worldMatrix2 = this.m_shipMergeBlock.Other.WorldMatrix;
      Vector3D translation2 = worldMatrix2.Translation;
      float num = (float) Math.Exp(-((translation1 - translation2).Length() - (double) this.m_shipMergeBlock.CubeGrid.GridSize) * 6.0);
      Vector3 translation3 = matrix1.Translation;
      worldMatrix2 = this.m_shipMergeBlock.CubeGrid.WorldMatrix;
      Vector3D vector3D = worldMatrix2.GetDirectionVector(Base6Directions.GetDirection(this.m_shipMergeBlock.PositionComp.LocalMatrixRef.Up)) * 0.5;
      MyRenderProxy.DebugDrawText3D(translation3 + vector3D, num.ToString("0.00"), Color.Red, 1f, false);
    }
  }
}
