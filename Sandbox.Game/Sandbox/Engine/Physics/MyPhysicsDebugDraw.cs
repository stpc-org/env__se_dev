// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyPhysicsDebugDraw
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Utils;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Engine.Physics
{
  internal static class MyPhysicsDebugDraw
  {
    public static bool DebugDrawFlattenHierarchy = false;
    public static bool HkGridShapeCellDebugDraw = false;
    public static HkGeometry DebugGeometry;
    private static Color[] boxColors = MyUtils.GenerateBoxColors();
    private static List<HkShape> m_tmpShapeList = new List<HkShape>();
    private static Dictionary<string, Vector3D> DebugShapesPositions = new Dictionary<string, Vector3D>();

    private static Color GetShapeColor(
      HkShapeType shapeType,
      ref int shapeIndex,
      bool isPhantom)
    {
      if (isPhantom)
        return Color.LightGreen;
      switch (shapeType)
      {
        case HkShapeType.Sphere:
          return Color.White;
        case HkShapeType.Cylinder:
          return Color.Orange;
        case HkShapeType.Capsule:
          return Color.Yellow;
        case HkShapeType.ConvexVertices:
          return Color.Red;
        default:
          return MyPhysicsDebugDraw.boxColors[++shapeIndex % (MyPhysicsDebugDraw.boxColors.Length - 1)];
      }
    }

    public static void DrawCollisionShape(
      HkShape shape,
      MatrixD worldMatrix,
      float alpha,
      ref int shapeIndex,
      string customText = null,
      bool isPhantom = false)
    {
      Color shapeColor = MyPhysicsDebugDraw.GetShapeColor(shape.ShapeType, ref shapeIndex, isPhantom);
      if (isPhantom)
        alpha *= alpha;
      shapeColor.A = (byte) ((double) alpha * (double) byte.MaxValue);
      bool flag1 = true;
      float num1 = 0.02f;
      float num2 = 1.035f;
      bool flag2 = false;
      switch (shape.ShapeType)
      {
        case HkShapeType.Sphere:
          float radius = ((HkSphereShape) shape).Radius;
          MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, radius, shapeColor, alpha, smooth: flag1);
          if (isPhantom)
          {
            MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, radius, shapeColor);
            MyRenderProxy.DebugDrawSphere(worldMatrix.Translation, radius, shapeColor, cull: false);
          }
          flag2 = true;
          break;
        case HkShapeType.Cylinder:
          HkCylinderShape hkCylinderShape = (HkCylinderShape) shape;
          MyRenderProxy.DebugDrawCylinder(worldMatrix, (Vector3D) hkCylinderShape.VertexA, (Vector3D) hkCylinderShape.VertexB, hkCylinderShape.Radius, shapeColor, alpha, true, flag1);
          flag2 = true;
          break;
        case HkShapeType.Triangle:
          HkTriangleShape hkTriangleShape1 = (HkTriangleShape) shape;
          MyRenderProxy.DebugDrawTriangle((Vector3D) hkTriangleShape1.Pt0, (Vector3D) hkTriangleShape1.Pt1, (Vector3D) hkTriangleShape1.Pt2, Color.Green, false, false);
          break;
        case HkShapeType.Box:
          HkBoxShape hkBoxShape = (HkBoxShape) shape;
          MyRenderProxy.DebugDrawOBB(MatrixD.CreateScale((Vector3D) (hkBoxShape.HalfExtents * 2f + new Vector3(num1))) * worldMatrix, shapeColor, alpha, true, flag1);
          MyRenderProxy.DebugDrawOBB(MatrixD.CreateScale((Vector3D) ((hkBoxShape.HalfExtents + shape.ConvexRadius) * 2f + new Vector3(num1))) * worldMatrix, shapeColor, alpha / 2f, true, flag1);
          if (isPhantom)
          {
            MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(hkBoxShape.HalfExtents * 2f + new Vector3(num1)) * worldMatrix, shapeColor, 1f, true, false);
            MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(hkBoxShape.HalfExtents * 2f + new Vector3(num1)) * worldMatrix, shapeColor, 1f, true, false, false);
          }
          flag2 = true;
          break;
        case HkShapeType.Capsule:
          HkCapsuleShape hkCapsuleShape = (HkCapsuleShape) shape;
          MyRenderProxy.DebugDrawCapsule(Vector3.Transform(hkCapsuleShape.VertexA, worldMatrix), Vector3.Transform(hkCapsuleShape.VertexB, worldMatrix), hkCapsuleShape.Radius, shapeColor, true, flag1);
          flag2 = true;
          break;
        case HkShapeType.ConvexVertices:
          Vector3 center;
          ((HkConvexVerticesShape) shape).GetGeometry(MyPhysicsDebugDraw.DebugGeometry, out center);
          Vector3D vector3D = Vector3D.Transform(center, worldMatrix.GetOrientation());
          MatrixD matrixD = worldMatrix;
          MatrixD worldMatrix1 = MatrixD.CreateScale((double) num2) * matrixD;
          worldMatrix1.Translation -= vector3D * ((double) num2 - 1.0);
          MyPhysicsDebugDraw.DrawGeometry(MyPhysicsDebugDraw.DebugGeometry, worldMatrix1, shapeColor, true, true);
          flag2 = true;
          break;
        case HkShapeType.List:
          HkShapeContainerIterator iterator1 = ((HkListShape) shape).GetIterator();
          while (iterator1.IsValid)
          {
            MyPhysicsDebugDraw.DrawCollisionShape(iterator1.CurrentValue, worldMatrix, alpha, ref shapeIndex, customText);
            iterator1.Next();
          }
          break;
        case HkShapeType.Mopp:
          MyPhysicsDebugDraw.DrawCollisionShape((HkShape) ((HkMoppBvTreeShape) shape).ShapeCollection, worldMatrix, alpha, ref shapeIndex, customText);
          break;
        case HkShapeType.ConvexTranslate:
          HkConvexTranslateShape convexTranslateShape = (HkConvexTranslateShape) shape;
          MyPhysicsDebugDraw.DrawCollisionShape((HkShape) convexTranslateShape.ChildShape, Matrix.CreateTranslation(convexTranslateShape.Translation) * worldMatrix, alpha, ref shapeIndex, customText);
          break;
        case HkShapeType.ConvexTransform:
          HkConvexTransformShape convexTransformShape = (HkConvexTransformShape) shape;
          MyPhysicsDebugDraw.DrawCollisionShape((HkShape) convexTransformShape.ChildShape, convexTransformShape.Transform * worldMatrix, alpha, ref shapeIndex, customText);
          break;
        case HkShapeType.StaticCompound:
          HkStaticCompoundShape staticCompoundShape = (HkStaticCompoundShape) shape;
          if (MyPhysicsDebugDraw.DebugDrawFlattenHierarchy)
          {
            HkShapeContainerIterator iterator2 = staticCompoundShape.GetIterator();
            while (iterator2.IsValid)
            {
              if (staticCompoundShape.IsShapeKeyEnabled(iterator2.CurrentShapeKey))
              {
                string customText1 = (customText ?? string.Empty) + "-" + (object) iterator2.CurrentShapeKey + "-";
                MyPhysicsDebugDraw.DrawCollisionShape(iterator2.CurrentValue, worldMatrix, alpha, ref shapeIndex, customText1);
              }
              iterator2.Next();
            }
            break;
          }
          for (int index = 0; index < staticCompoundShape.InstanceCount; ++index)
          {
            bool flag3 = staticCompoundShape.IsInstanceEnabled(index);
            string customText1;
            if (flag3)
              customText1 = (customText ?? string.Empty) + "<" + (object) index + ">";
            else
              customText1 = (customText ?? string.Empty) + "(" + (object) index + ")";
            if (flag3)
              MyPhysicsDebugDraw.DrawCollisionShape(staticCompoundShape.GetInstance(index), staticCompoundShape.GetInstanceTransform(index) * worldMatrix, alpha, ref shapeIndex, customText1);
          }
          break;
        case HkShapeType.BvCompressedMesh:
          if (MyDebugDrawSettings.DEBUG_DRAW_TRIANGLE_PHYSICS)
          {
            ((HkBvCompressedMeshShape) shape).GetGeometry(MyPhysicsDebugDraw.DebugGeometry);
            MyPhysicsDebugDraw.DrawGeometry(MyPhysicsDebugDraw.DebugGeometry, worldMatrix, Color.Green);
            flag2 = true;
            break;
          }
          break;
        case HkShapeType.BvTree:
          HkGridShape hkGridShape = (HkGridShape) shape;
          if (MyPhysicsDebugDraw.HkGridShapeCellDebugDraw && !hkGridShape.Base.IsZero)
          {
            float cellSize = hkGridShape.CellSize;
            int shapeInfoCount = hkGridShape.GetShapeInfoCount();
            for (int index = 0; index < shapeInfoCount; ++index)
            {
              try
              {
                Vector3S min;
                Vector3S max;
                hkGridShape.GetShapeInfo(index, out min, out max, MyPhysicsDebugDraw.m_tmpShapeList);
                Vector3 vector3_1 = max * cellSize - min * cellSize;
                Vector3 position = (max * cellSize + min * cellSize) / 2f;
                Vector3 vector3_2 = Vector3.One * cellSize;
                Vector3 vector3_3 = vector3_1 + vector3_2;
                Color color = shapeColor;
                if (min == max)
                  color = new Color(1f, 0.2f, 0.1f);
                Vector3 vector3_4 = new Vector3(num1);
                MyRenderProxy.DebugDrawOBB(Matrix.CreateScale(vector3_3 + vector3_4) * Matrix.CreateTranslation(position) * worldMatrix, color, alpha, true, flag1);
              }
              finally
              {
                MyPhysicsDebugDraw.m_tmpShapeList.Clear();
              }
            }
            break;
          }
          MyRenderMessageDebugDrawTriangles debugDrawTriangles = MyRenderProxy.PrepareDebugDrawTriangles();
          try
          {
            using (HkShapeBuffer buffer = new HkShapeBuffer())
            {
              HkShapeContainerIterator iterator2 = ((HkBvTreeShape) shape).GetIterator(buffer);
              while (iterator2.IsValid)
              {
                HkShape currentValue = iterator2.CurrentValue;
                if (currentValue.ShapeType == HkShapeType.Triangle)
                {
                  HkTriangleShape hkTriangleShape2 = (HkTriangleShape) currentValue;
                  debugDrawTriangles.AddTriangle((Vector3D) hkTriangleShape2.Pt0, (Vector3D) hkTriangleShape2.Pt1, (Vector3D) hkTriangleShape2.Pt2);
                }
                else
                  MyPhysicsDebugDraw.DrawCollisionShape(currentValue, worldMatrix, alpha, ref shapeIndex);
                iterator2.Next();
              }
              break;
            }
          }
          finally
          {
            debugDrawTriangles.Color = shapeColor;
            MyRenderProxy.DebugDrawTriangles((IDrawTrianglesMessage) debugDrawTriangles, new MatrixD?(worldMatrix), false, false);
          }
        case HkShapeType.Bv:
          HkBvShape hkBvShape = (HkBvShape) shape;
          MyPhysicsDebugDraw.DrawCollisionShape(hkBvShape.BoundingVolumeShape, worldMatrix, alpha, ref shapeIndex, isPhantom: true);
          MyPhysicsDebugDraw.DrawCollisionShape(hkBvShape.ChildShape, worldMatrix, alpha, ref shapeIndex);
          break;
        case HkShapeType.PhantomCallback:
          MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, "Phantom", Color.Green, 0.75f, false);
          break;
      }
      if (!flag2 || customText == null)
        return;
      shapeColor.A = byte.MaxValue;
      MyRenderProxy.DebugDrawText3D(worldMatrix.Translation, customText, shapeColor, 0.8f, false);
    }

    public static void DrawGeometry(
      HkGeometry geometry,
      MatrixD worldMatrix,
      Color color,
      bool depthRead = false,
      bool shaded = false)
    {
      MyRenderMessageDebugDrawTriangles debugDrawTriangles = MyRenderProxy.PrepareDebugDrawTriangles();
      try
      {
        for (int triangleIndex = 0; triangleIndex < geometry.TriangleCount; ++triangleIndex)
        {
          int i0;
          int i1;
          int i2;
          geometry.GetTriangle(triangleIndex, out i0, out i1, out i2, out int _);
          debugDrawTriangles.AddIndex(i0);
          debugDrawTriangles.AddIndex(i1);
          debugDrawTriangles.AddIndex(i2);
        }
        for (int vertexIndex = 0; vertexIndex < geometry.VertexCount; ++vertexIndex)
          debugDrawTriangles.AddVertex((Vector3D) geometry.GetVertex(vertexIndex));
      }
      finally
      {
        debugDrawTriangles.Color = color;
        MyRenderProxy.DebugDrawTriangles((IDrawTrianglesMessage) debugDrawTriangles, new MatrixD?(worldMatrix), depthRead, shaded);
      }
    }

    public static void DebugDrawBreakable(HkdBreakableBody bb, Vector3 offset)
    {
      MyPhysicsDebugDraw.DebugShapesPositions.Clear();
      if (!((HkReferenceObject) bb != (HkReferenceObject) null))
        return;
      int shapeIndex = 0;
      Matrix rigidBodyMatrix = bb.GetRigidBody().GetRigidBodyMatrix();
      MatrixD world = MatrixD.CreateWorld((Vector3D) (rigidBodyMatrix.Translation + offset), rigidBodyMatrix.Forward, rigidBodyMatrix.Up);
      MyPhysicsDebugDraw.DrawBreakableShape(bb.BreakableShape, world, 0.3f, ref shapeIndex);
      MyPhysicsDebugDraw.DrawConnections(bb.BreakableShape, world, 0.3f, ref shapeIndex);
    }

    private static void DrawBreakableShape(
      HkdBreakableShape breakableShape,
      MatrixD worldMatrix,
      float alpha,
      ref int shapeIndex,
      string customText = null,
      bool isPhantom = false)
    {
      MyPhysicsDebugDraw.DrawCollisionShape(breakableShape.GetShape(), worldMatrix, alpha, ref shapeIndex, breakableShape.Name + " Strength: " + (object) breakableShape.GetStrenght() + " Static:" + breakableShape.IsFixed().ToString());
      if (!string.IsNullOrEmpty(breakableShape.Name) && breakableShape.Name != "PineTree175m_v2_001")
        breakableShape.IsFixed();
      MyPhysicsDebugDraw.DebugShapesPositions[breakableShape.Name] = worldMatrix.Translation;
      List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
      breakableShape.GetChildren(list);
      Vector3 coM = breakableShape.CoM;
      foreach (HkdShapeInstanceInfo shapeInstanceInfo in list)
      {
        MatrixD matrixD = shapeInstanceInfo.GetTransform() * worldMatrix * Matrix.CreateTranslation(Vector3.Right * 2f);
        Matrix matrix = (Matrix) ref matrixD;
        MyPhysicsDebugDraw.DrawBreakableShape(shapeInstanceInfo.Shape, (MatrixD) ref matrix, alpha, ref shapeIndex);
      }
    }

    private static void DrawConnections(
      HkdBreakableShape breakableShape,
      MatrixD worldMatrix,
      float alpha,
      ref int shapeIndex,
      string customText = null,
      bool isPhantom = false)
    {
      List<HkdConnection> resultList = new List<HkdConnection>();
      breakableShape.GetConnectionList(resultList);
      List<HkdShapeInstanceInfo> list = new List<HkdShapeInstanceInfo>();
      breakableShape.GetChildren(list);
      foreach (HkdConnection hkdConnection in resultList)
      {
        Vector3D debugShapesPosition1 = MyPhysicsDebugDraw.DebugShapesPositions[hkdConnection.ShapeAName];
        Vector3D debugShapesPosition2 = MyPhysicsDebugDraw.DebugShapesPositions[hkdConnection.ShapeBName];
        bool flag = false;
        foreach (HkdShapeInstanceInfo shapeInstanceInfo in list)
        {
          if (shapeInstanceInfo.ShapeName == hkdConnection.ShapeAName || shapeInstanceInfo.ShapeName == hkdConnection.ShapeBName)
            flag = true;
        }
        if (flag)
          MyRenderProxy.DebugDrawLine3D(debugShapesPosition1, debugShapesPosition2, Color.White, Color.White, false);
      }
    }

    public static void DebugDrawAddForce(
      MyPhysicsBody physics,
      MyPhysicsForceType type,
      Vector3? force,
      Vector3D? position,
      Vector3? torque,
      bool persistent = false)
    {
      switch (type)
      {
        case MyPhysicsForceType.APPLY_WORLD_IMPULSE_AND_WORLD_ANGULAR_IMPULSE:
          Vector3D pointFrom1 = position.Value + physics.LinearVelocity * 0.01666667f;
          if (force.HasValue)
            MyRenderProxy.DebugDrawArrow3D(pointFrom1, pointFrom1 + force.Value * 0.1f, Color.Blue, new Color?(Color.Red), persistent: persistent);
          if (!torque.HasValue)
            break;
          MyRenderProxy.DebugDrawArrow3D(pointFrom1, pointFrom1 + torque.Value * 0.1f, Color.Blue, new Color?(Color.Purple), persistent: persistent);
          break;
        case MyPhysicsForceType.ADD_BODY_FORCE_AND_BODY_TORQUE:
          if (!((HkReferenceObject) physics.RigidBody != (HkReferenceObject) null))
            break;
          Matrix rigidBodyMatrix = physics.RigidBody.GetRigidBodyMatrix();
          Vector3D pointFrom2 = physics.CenterOfMassWorld + physics.LinearVelocity * 0.01666667f;
          if (force.HasValue)
          {
            Vector3 vector3 = Vector3.TransformNormal(force.Value, rigidBodyMatrix) * 0.1f;
            MyRenderProxy.DebugDrawArrow3D(pointFrom2, pointFrom2 + vector3, Color.Blue, new Color?(Color.Red), persistent: persistent);
          }
          if (!torque.HasValue)
            break;
          Vector3 vector3_1 = Vector3.TransformNormal(torque.Value, rigidBodyMatrix) * 0.1f;
          MyRenderProxy.DebugDrawArrow3D(pointFrom2, pointFrom2 + vector3_1, Color.Blue, new Color?(Color.Purple), persistent: persistent);
          break;
        case MyPhysicsForceType.APPLY_WORLD_FORCE:
          if (!position.HasValue)
            break;
          Vector3D pointFrom3 = position.Value + physics.LinearVelocity * 0.01666667f;
          if (!force.HasValue)
            break;
          MyRenderProxy.DebugDrawArrow3D(pointFrom3, pointFrom3 + force.Value * 0.01666667f * 0.1f, Color.Blue, new Color?(Color.Red), persistent: persistent);
          break;
      }
    }

    public static void DebugDrawCoordinateSystem(
      Vector3? position,
      Vector3? forward,
      Vector3? side,
      Vector3? up,
      float scale = 1f)
    {
      if (!position.HasValue)
        return;
      Vector3D pointFrom = (Vector3D) position.Value;
      if (forward.HasValue)
      {
        Vector3 vector3 = forward.Value * scale;
        MyRenderProxy.DebugDrawArrow3D(pointFrom, pointFrom + vector3, Color.Blue, new Color?(Color.Red));
      }
      if (side.HasValue)
      {
        Vector3 vector3 = side.Value * scale;
        MyRenderProxy.DebugDrawArrow3D(pointFrom, pointFrom + vector3, Color.Blue, new Color?(Color.Green));
      }
      if (!up.HasValue)
        return;
      Vector3 vector3_1 = up.Value * scale;
      MyRenderProxy.DebugDrawArrow3D(pointFrom, pointFrom + vector3_1, Color.Blue, new Color?(Color.Blue));
    }

    public static void DebugDrawVector3(
      Vector3? position,
      Vector3? vector,
      Color color,
      float scale = 0.01f)
    {
      if (!position.HasValue)
        return;
      Vector3D pointFrom = (Vector3D) position.Value;
      if (!vector.HasValue)
        return;
      Vector3 vector3 = vector.Value * scale;
      MyRenderProxy.DebugDrawArrow3D(pointFrom, pointFrom + vector3, color, new Color?(color));
    }
  }
}
