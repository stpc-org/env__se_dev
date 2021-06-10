// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Components.MyDebugRenderComponentCubeGrid
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Components
{
  public class MyDebugRenderComponentCubeGrid : MyDebugRenderComponent
  {
    private MyCubeGrid m_cubeGrid;
    private Dictionary<Vector3I, MyTimeSpan> m_dirtyBlocks = new Dictionary<Vector3I, MyTimeSpan>();
    private List<Vector3I> m_tmpRemoveList = new List<Vector3I>();
    private List<HkBodyCollision> m_penetrations = new List<HkBodyCollision>();
    private readonly List<MyCubeGrid.DebugUpdateRecord> m_gridDebugUpdateInfo = new List<MyCubeGrid.DebugUpdateRecord>();

    public MyDebugRenderComponentCubeGrid(MyCubeGrid cubeGrid)
      : base((IMyEntity) cubeGrid)
      => this.m_cubeGrid = cubeGrid;

    public override void PrepareForDraw()
    {
      base.PrepareForDraw();
      if (!MyDebugDrawSettings.DEBUG_DRAW_GRID_DIRTY_BLOCKS)
        return;
      MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(1500.0);
      using (this.m_tmpRemoveList.GetClearToken<Vector3I>())
      {
        foreach (KeyValuePair<Vector3I, MyTimeSpan> dirtyBlock in this.m_dirtyBlocks)
        {
          if (MySandboxGame.Static.TotalTime - dirtyBlock.Value > myTimeSpan)
            this.m_tmpRemoveList.Add(dirtyBlock.Key);
        }
        foreach (Vector3I tmpRemove in this.m_tmpRemoveList)
          this.m_dirtyBlocks.Remove(tmpRemove);
      }
      foreach (Vector3I dirtyBlock in this.m_cubeGrid.DirtyBlocks)
        this.m_dirtyBlocks[dirtyBlock] = MySandboxGame.Static.TotalTime;
    }

    public override void DebugDraw()
    {
      if (MyPhysicsConfig.EnableGridSpeedDebugDraw && this.Entity.Physics != null)
      {
        Color color1 = (double) this.Entity.Physics.RigidBody.MaxLinearVelocity <= 190.0 ? Color.Red : Color.Green;
        Vector3D position = this.Entity.PositionComp.GetPosition();
        float num = this.Entity.Physics.LinearVelocity.Length();
        string text1 = num.ToString("F2");
        Color color2 = color1;
        MyRenderProxy.DebugDrawText3D(position, text1, color2, 1f, false);
        Vector3D worldCoord = this.Entity.PositionComp.GetPosition() + Vector3.One * 3f;
        num = this.Entity.Physics.AngularVelocity.Length();
        string text2 = num.ToString("F2");
        Color color3 = color1;
        MyRenderProxy.DebugDrawText3D(worldCoord, text2, color3, 1f, false);
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_GRID_UPDATES)
      {
        Vector3D worldCoord = Vector3D.Transform(this.Entity.PositionComp.LocalAABB.Center, this.Entity.PositionComp.WorldMatrixRef);
        float scale = (float) MathHelper.Clamp(this.Entity.PositionComp.WorldVolume.Radius * 2.0 / Vector3D.Distance(MySector.MainCamera.Position, worldCoord), 0.01, 1.0);
        this.m_gridDebugUpdateInfo.Clear();
        this.m_cubeGrid.GetDebugUpdateInfo(this.m_gridDebugUpdateInfo);
        if (this.m_gridDebugUpdateInfo.Count == 0)
          MyRenderProxy.DebugDrawText3D(worldCoord, "No Updates", Color.Gray, scale, false);
        else
          MyRenderProxy.DebugDrawText3D(worldCoord, string.Join<MyCubeGrid.DebugUpdateRecord>("\n", (IEnumerable<MyCubeGrid.DebugUpdateRecord>) this.m_gridDebugUpdateInfo), Color.Wheat, scale, false);
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_GRID_AABB)
      {
        BoundingBox localAabb = this.m_cubeGrid.PositionComp.LocalAABB;
        MatrixD matrixD = this.m_cubeGrid.PositionComp.WorldMatrixRef;
        MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD((BoundingBoxD) localAabb, matrixD), Color.Yellow, 0.2f, false, true);
        MyRenderProxy.DebugDrawAxis(matrixD, 1f, false);
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_FIXED_BLOCK_QUERIES)
      {
        foreach (MySlimBlock block in this.m_cubeGrid.GetBlocks())
        {
          BoundingBox geometryLocalBox = block.FatBlock.GetGeometryLocalBox();
          Vector3 halfExtents = geometryLocalBox.Size / 2f;
          Vector3D scaledCenter;
          block.ComputeScaledCenter(out scaledCenter);
          scaledCenter += geometryLocalBox.Center;
          scaledCenter = Vector3D.Transform(scaledCenter, this.m_cubeGrid.WorldMatrix);
          Matrix result;
          block.Orientation.GetMatrix(out result);
          Matrix matrix1 = result;
          MatrixD matrix2 = this.m_cubeGrid.WorldMatrix;
          MatrixD orientation = matrix2.GetOrientation();
          matrix2 = matrix1 * orientation;
          Quaternion fromRotationMatrix = Quaternion.CreateFromRotationMatrix(in matrix2);
          MyPhysics.GetPenetrationsBox(ref halfExtents, ref scaledCenter, ref fromRotationMatrix, this.m_penetrations, 14);
          bool flag = false;
          foreach (HkBodyCollision penetration in this.m_penetrations)
          {
            IMyEntity collisionEntity = penetration.GetCollisionEntity();
            if (collisionEntity != null && collisionEntity is MyVoxelMap)
            {
              flag = true;
              break;
            }
          }
          this.m_penetrations.Clear();
          MyRenderProxy.DebugDrawOBB(new MyOrientedBoundingBoxD(scaledCenter, (Vector3D) halfExtents, fromRotationMatrix), flag ? Color.Green : Color.Red, 0.1f, false, false);
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_GRID_NAMES || MyDebugDrawSettings.DEBUG_DRAW_GRID_CONTROL)
      {
        string text = "";
        Color color = Color.White;
        if (MyDebugDrawSettings.DEBUG_DRAW_GRID_NAMES)
          text = text + this.m_cubeGrid.ToString() + " ";
        if (MyDebugDrawSettings.DEBUG_DRAW_GRID_CONTROL)
        {
          MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer((MyEntity) this.m_cubeGrid);
          if (controllingPlayer != null)
          {
            text = text + "Controlled by: " + controllingPlayer.DisplayName;
            color = Color.LightGreen;
          }
        }
        MyRenderProxy.DebugDrawText3D(this.m_cubeGrid.PositionComp.WorldAABB.Center, text, color, 0.7f, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      }
      MyRenderComponentCubeGrid render = this.m_cubeGrid.Render;
      if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS)
      {
        Vector3D translation = this.m_cubeGrid.PositionComp.WorldMatrixRef.Translation;
        foreach (MyBlockGroup blockGroup in this.m_cubeGrid.BlockGroups)
        {
          MyRenderProxy.DebugDrawText3D(translation, blockGroup.Name.ToString(), Color.Red, 1f, false);
          translation += this.m_cubeGrid.PositionComp.WorldMatrixRef.Right * (double) blockGroup.Name.Length * 0.100000001490116;
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_GRID_DIRTY_BLOCKS)
      {
        foreach (KeyValuePair<Vector3I, MyTimeSpan> dirtyBlock in this.m_dirtyBlocks)
        {
          Color color;
          Vector3 vector3_1;
          if (this.m_cubeGrid.GetCubeBlock(dirtyBlock.Key) == null)
          {
            color = Color.Yellow;
            vector3_1 = color.ToVector3();
          }
          else
          {
            color = Color.Red;
            vector3_1 = color.ToVector3();
          }
          Vector3 vector3_2 = vector3_1;
          MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(this.m_cubeGrid.GridSize) * Matrix.CreateTranslation(dirtyBlock.Key * this.m_cubeGrid.GridSize) * this.m_cubeGrid.WorldMatrix, (Color) vector3_2, 0.15f, false, true);
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_VERTICES_CACHE && (MySector.MainCamera.Position - this.m_cubeGrid.PositionComp.WorldMatrix.Translation).LengthSquared() < 600.0)
      {
        List<Vector3> verts = new List<Vector3>();
        int i = 0;
        foreach (MySlimBlock cubeBlock in this.m_cubeGrid.CubeBlocks)
        {
          ++i;
          verts.Clear();
          if (cubeBlock.BlockDefinition.CubeDefinition != null)
          {
            Vector3 vector3_1 = cubeBlock.Position * this.m_cubeGrid.GridSize;
            Matrix result1;
            cubeBlock.Orientation.GetMatrix(out result1);
            result1.Translation = vector3_1;
            MyBlockVerticesCache.GetTopologySwitch(cubeBlock.BlockDefinition.CubeDefinition.CubeTopology, verts);
            MyCubeGridDefinitions.TableEntry topologyInfo = MyCubeGridDefinitions.GetTopologyInfo(cubeBlock.BlockDefinition.CubeDefinition.CubeTopology);
            foreach (MyEdgeDefinition edge in topologyInfo.Edges)
            {
              Vector3 vector3_2 = (Vector3.TransformNormal(edge.Point0, cubeBlock.Orientation) + Vector3.TransformNormal(edge.Point1, cubeBlock.Orientation)) * 0.5f;
              Vector3.Transform(edge.Point0 * this.m_cubeGrid.GridSizeHalf, ref result1);
              Vector3.Transform(edge.Point1 * this.m_cubeGrid.GridSizeHalf, ref result1);
              if (edge.Side0 < topologyInfo.Tiles.Length && edge.Side1 < topologyInfo.Tiles.Length && (edge.Side0 >= 0 && edge.Side1 >= 0))
              {
                Vector3 vector3_3 = Vector3.TransformNormal(topologyInfo.Tiles[edge.Side0].Normal, cubeBlock.Orientation);
                Vector3.TransformNormal(topologyInfo.Tiles[edge.Side1].Normal, cubeBlock.Orientation);
                MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_2, (Vector3D) (vector3_2 + vector3_3), MyDebugDrawSettings.GetColor(i), MyDebugDrawSettings.GetColor(i), true);
              }
            }
            Matrix result2;
            cubeBlock.Orientation.GetMatrix(out result2);
            for (int index = 0; index < verts.Count - 1; ++index)
              MyRenderProxy.DebugDrawArrow3D(Vector3.Transform(Vector3.Transform(verts[index], result2) + cubeBlock.Position * this.m_cubeGrid.GridSize, this.m_cubeGrid.WorldMatrix), Vector3.Transform(Vector3.Transform(verts[index + 1], result2) + cubeBlock.Position * this.m_cubeGrid.GridSize, this.m_cubeGrid.WorldMatrix), Color.Violet, depthRead: true);
            for (int index = 0; index < verts.Count; ++index)
            {
              Vector3D vector3D = Vector3.Transform(Vector3.Transform(verts[index], result2) + cubeBlock.Position * this.m_cubeGrid.GridSize, this.m_cubeGrid.WorldMatrix);
              MyRenderProxy.DebugDrawSphere(vector3D, 0.025f, Color.Green, 0.5f);
              MyRenderProxy.DebugDrawText3D(vector3D, index.ToString(), Color.Green, 0.5f, true);
            }
          }
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_SKETELON_CUBE_BONES && ((MySector.MainCamera.Position - this.m_cubeGrid.PositionComp.WorldMatrix.Translation).LengthSquared() < 900.0 && MySession.Static.GameplayFrameCounter % 10 == 0))
      {
        Dictionary<MyCubeTopology, HashSet<Vector3I>> dictionary = new Dictionary<MyCubeTopology, HashSet<Vector3I>>();
        foreach (MySlimBlock cubeBlock in this.m_cubeGrid.CubeBlocks)
        {
          if (cubeBlock.BlockDefinition.CubeDefinition != null)
          {
            MyCubeTopology cubeTopology = cubeBlock.BlockDefinition.CubeDefinition.CubeTopology;
            MyCube cube;
            if (this.m_cubeGrid.TryGetCube(cubeBlock.Position, out cube) && !dictionary.ContainsKey(cubeTopology))
            {
              MyCubeGridDefinitions.GetCubeTiles(cubeBlock.BlockDefinition);
              cube.CubeBlock.Orientation.GetMatrix(out Matrix _);
              dictionary.Add(cubeTopology, new HashSet<Vector3I>());
              foreach (MyCubePart part in cube.Parts)
              {
                if (part.Model.BoneMapping != null)
                {
                  Vector3D zero = Vector3D.Zero;
                  for (int index = 0; index < Math.Min(part.Model.BoneMapping.Length, 9); ++index)
                  {
                    Vector3I vector3I1 = part.Model.BoneMapping[index];
                    Matrix orientation = part.InstanceData.LocalMatrix.GetOrientation();
                    Vector3 vector3 = vector3I1 * 1f - Vector3.One;
                    Vector3I vector3I2 = Vector3I.Round(Vector3.Transform(vector3 * 1f, orientation));
                    Vector3I.Round(Vector3.Transform(vector3 * 1f, orientation) + Vector3.One);
                    dictionary[cubeTopology].Add(vector3I2 + Vector3I.One);
                  }
                }
              }
            }
          }
        }
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<MyCubeTopology, HashSet<Vector3I>> keyValuePair in dictionary)
        {
          stringBuilder.AppendLine(string.Format("      <Skeleton><!--{0}-->", (object) keyValuePair.Key));
          foreach (Vector3I vector3I in keyValuePair.Value)
          {
            stringBuilder.AppendLine("        <BoneInfo>");
            stringBuilder.AppendLine(string.Format("          <BonePosition x=\"{0}\" y=\"{1}\" z=\"{2}\" />", (object) vector3I.X, (object) vector3I.Y, (object) vector3I.Z));
            stringBuilder.AppendLine("          <BoneOffset x=\"127\" y=\"127\" z=\"127\" />");
            stringBuilder.AppendLine("        </BoneInfo>");
          }
          stringBuilder.AppendLine("      </Skeleton>");
          stringBuilder.AppendLine();
          stringBuilder.AppendLine();
          stringBuilder.AppendLine();
        }
        MyLog.Default.WriteLine(stringBuilder.ToString());
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_DISPLACED_BONES && (MySector.MainCamera.Position - this.m_cubeGrid.PositionComp.WorldMatrix.Translation).LengthSquared() < 600.0)
      {
        Vector3D position1 = MySector.MainCamera.Position;
        foreach (MySlimBlock cubeBlock in this.m_cubeGrid.CubeBlocks)
        {
          MyCube cube;
          if (this.m_cubeGrid.TryGetCube(cubeBlock.Position, out cube))
          {
            int index1 = 0;
            MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(cubeBlock.BlockDefinition);
            Matrix result;
            cube.CubeBlock.Orientation.GetMatrix(out result);
            foreach (MyCubePart part in cube.Parts)
            {
              if (part.Model.BoneMapping != null && (index1 == MyPetaInputComponent.DEBUG_INDEX || index1 == 7))
              {
                Vector3D zero = Vector3D.Zero;
                for (int index2 = 0; index2 < Math.Min(part.Model.BoneMapping.Length, 9); ++index2)
                {
                  Vector3I vector3I1 = part.Model.BoneMapping[index2];
                  Matrix orientation = part.InstanceData.LocalMatrix.GetOrientation();
                  Vector3 vector3 = vector3I1 * 1f - Vector3.One;
                  Vector3I vector3I2 = Vector3I.Round(Vector3.Transform(vector3 * 1f, orientation));
                  Vector3I bonePos = Vector3I.Round(Vector3.Transform(vector3 * 1f, orientation) + Vector3.One);
                  Vector3 position2 = this.m_cubeGrid.GridSize * ((Vector3) cubeBlock.Position + vector3I2 / 2f);
                  Vector3 bone = this.m_cubeGrid.Skeleton.GetBone(cubeBlock.Position, bonePos);
                  MatrixD matrix = this.m_cubeGrid.PositionComp.WorldMatrixRef;
                  Vector3D vector3D1 = Vector3D.Transform(position2, matrix);
                  Vector3D vector3D2 = Vector3D.TransformNormal(bone, this.m_cubeGrid.PositionComp.WorldMatrixRef);
                  MyRenderProxy.DebugDrawSphere(vector3D1, 0.025f, Color.Green, 0.5f, false, true);
                  MyRenderProxy.DebugDrawText3D(vector3D1, index2.ToString() + "  (" + (object) vector3I1.X + "," + (object) vector3I1.Y + "," + (object) vector3I1.Z + ")", Color.Green, 0.5f, false);
                  MyRenderProxy.DebugDrawArrow3D(vector3D1, vector3D1 + vector3D2, Color.Red);
                  zero += vector3D1;
                }
                Vector3D pointFrom = zero / (double) Math.Min(part.Model.BoneMapping.Length, 9);
                try
                {
                  Vector3 vector3 = Vector3.TransformNormal(cubeTiles[index1].Normal, result);
                  MyRenderProxy.DebugDrawArrow3D(pointFrom, pointFrom + vector3, Color.Purple);
                }
                catch (Exception ex)
                {
                }
              }
              ++index1;
            }
          }
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_CUBES)
      {
        foreach (MySlimBlock cubeBlock in this.m_cubeGrid.CubeBlocks)
        {
          Matrix localMatrix;
          cubeBlock.GetLocalMatrix(out localMatrix);
          MyRenderProxy.DebugDrawAxis(localMatrix * this.m_cubeGrid.WorldMatrix, 1f, false);
          cubeBlock.FatBlock?.DebugDraw();
        }
      }
      this.m_cubeGrid.GridSystems.DebugDraw();
      int num1 = MyDebugDrawSettings.DEBUG_DRAW_GRID_TERMINAL_SYSTEMS ? 1 : 0;
      if (MyDebugDrawSettings.DEBUG_DRAW_GRID_ORIGINS)
        MyRenderProxy.DebugDrawAxis(this.m_cubeGrid.PositionComp.WorldMatrixRef, 1f, false);
      if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS_ALL)
      {
        foreach (MySlimBlock block in this.m_cubeGrid.GetBlocks())
        {
          if ((this.m_cubeGrid.GridIntegerToWorld(block.Position) - MySector.MainCamera.Position).LengthSquared() < 200.0)
            this.DebugDrawMountPoints(block);
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_ARMOR_BLOCK_TILE_NORMALS)
      {
        foreach (MySlimBlock block in this.m_cubeGrid.GetBlocks())
        {
          MyCube cube;
          if ((this.m_cubeGrid.GridIntegerToWorld(block.Position) - MySector.MainCamera.Position).LengthSquared() < 200.0 && this.m_cubeGrid.TryGetCube(block.Position, out cube))
          {
            Matrix result;
            cube.CubeBlock.Orientation.GetMatrix(out result);
            MyTileDefinition[] cubeTiles = MyCubeGridDefinitions.GetCubeTiles(block.BlockDefinition);
            for (int index = 0; index < cube.Parts.Length; ++index)
            {
              MyCubePart part = cube.Parts[index];
              Vector3 translation = part.InstanceData.Translation;
              if (!this.m_cubeGrid.Render.RenderData.GetCell(ref translation, false).HasCubePart(part))
              {
                MyTileDefinition myTileDefinition = cubeTiles[index];
                Vector3 vector3_1 = Vector3.TransformNormal(myTileDefinition.Normal, result);
                Vector3 vector3_2 = Vector3.TransformNormal(myTileDefinition.Up, result);
                MyRenderProxy.DebugDrawLine3D(block.WorldPosition, block.WorldPosition + vector3_2, Color.Red, Color.Red, true);
                MyRenderProxy.DebugDrawLine3D(block.WorldPosition, block.WorldPosition + vector3_1, Color.Green, Color.Green, true);
              }
            }
          }
        }
      }
      if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_INTEGRITY && MySector.MainCamera != null && (MySector.MainCamera.Position - this.m_cubeGrid.PositionComp.WorldVolume.Center).Length() < 16.0 + this.m_cubeGrid.PositionComp.WorldVolume.Radius)
      {
        foreach (MySlimBlock cubeBlock in this.m_cubeGrid.CubeBlocks)
        {
          Vector3D world = this.m_cubeGrid.GridIntegerToWorld(cubeBlock.Position);
          if (this.m_cubeGrid.GridSizeEnum == MyCubeSize.Large || MySector.MainCamera != null && (MySector.MainCamera.Position - world).LengthSquared() < 9.0)
          {
            float num2 = 0.0f;
            if (cubeBlock.FatBlock is MyCompoundCubeBlock)
            {
              foreach (MySlimBlock block in (cubeBlock.FatBlock as MyCompoundCubeBlock).GetBlocks())
                num2 += block.Integrity * block.BlockDefinition.MaxIntegrityRatio;
            }
            else
              num2 = cubeBlock.Integrity * cubeBlock.BlockDefinition.MaxIntegrityRatio;
            MyRenderProxy.DebugDrawText3D(this.m_cubeGrid.GridIntegerToWorld(cubeBlock.Position), ((int) num2).ToString(), Color.White, this.m_cubeGrid.GridSizeEnum == MyCubeSize.Large ? 0.65f : 0.5f, false);
          }
        }
      }
      base.DebugDraw();
    }

    private void DebugDrawMountPoints(MySlimBlock block)
    {
      if (block.FatBlock is MyCompoundCubeBlock)
      {
        foreach (MySlimBlock block1 in (block.FatBlock as MyCompoundCubeBlock).GetBlocks())
          this.DebugDrawMountPoints(block1);
      }
      else
      {
        Matrix localMatrix;
        block.GetLocalMatrix(out localMatrix);
        MatrixD drawMatrix = localMatrix * this.m_cubeGrid.WorldMatrix;
        MyCubeBlockDefinition blockDefinition;
        MyDefinitionManager.Static.TryGetCubeBlockDefinition(block.BlockDefinition.Id, out blockDefinition);
        if (MyFakes.ENABLE_FRACTURE_COMPONENT && block.FatBlock != null && block.FatBlock.Components.Has<MyFractureComponentBase>())
        {
          MyFractureComponentCubeBlock fractureComponent = block.GetFractureComponent();
          if (fractureComponent == null)
            return;
          MyCubeBuilder.DrawMountPoints(this.m_cubeGrid.GridSize, blockDefinition, drawMatrix, ((IEnumerable<MyCubeBlockDefinition.MountPoint>) fractureComponent.MountPoints).ToArray<MyCubeBlockDefinition.MountPoint>());
        }
        else
          MyCubeBuilder.DrawMountPoints(this.m_cubeGrid.GridSize, blockDefinition, ref drawMatrix);
      }
    }

    public override void DebugDrawInvalidTriangles()
    {
      base.DebugDrawInvalidTriangles();
      foreach (KeyValuePair<Vector3I, MyCubeGridRenderCell> cell in this.m_cubeGrid.Render.RenderData.Cells)
      {
        foreach (KeyValuePair<MyCubePart, ConcurrentDictionary<uint, bool>> cubePart in cell.Value.CubeParts)
        {
          MyModel model = cubePart.Key.Model;
          if (model != null)
          {
            int trianglesCount = model.GetTrianglesCount();
            for (int triangleIndex = 0; triangleIndex < trianglesCount; ++triangleIndex)
            {
              MyTriangleVertexIndices triangle = model.GetTriangle(triangleIndex);
              if (MyUtils.IsWrongTriangle(model.GetVertex(triangle.I0), model.GetVertex(triangle.I1), model.GetVertex(triangle.I2)))
              {
                Vector3 vector3_1 = Vector3.Transform(model.GetVertex(triangle.I0), (Matrix) ref this.m_cubeGrid.PositionComp.WorldMatrixRef);
                Vector3 vector3_2 = Vector3.Transform(model.GetVertex(triangle.I1), (Matrix) ref this.m_cubeGrid.PositionComp.WorldMatrixRef);
                Vector3 vector3_3 = Vector3.Transform(model.GetVertex(triangle.I2), (Matrix) ref this.m_cubeGrid.PositionComp.WorldMatrixRef);
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
        }
      }
    }
  }
}
