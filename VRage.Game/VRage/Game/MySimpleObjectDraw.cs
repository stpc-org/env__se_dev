// Decompiled with JetBrains decompiler
// Type: VRage.Game.MySimpleObjectDraw
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
  public static class MySimpleObjectDraw
  {
    private static readonly MyStringId ID_CONTAINER_BORDER = MyStringId.GetOrCompute("ContainerBorder");
    private static readonly MyStringId ID_GIZMO_DRAW_LINE = MyStringId.GetOrCompute("GizmoDrawLine");
    private const int MaxLinesPerDraw = 2000;

    public static void DrawTransparentBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color color,
      MySimpleObjectRasterizer rasterization,
      int wireDivideRatio,
      float lineWidth = 0.001f,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, ref color, rasterization, new Vector3I(wireDivideRatio), lineWidth, faceMaterial, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
    }

    public static void DrawTransparentBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color color,
      ref Color frontFaceColor,
      MySimpleObjectRasterizer rasterization,
      int wireDivideRatio,
      float lineWidth = 0.001f,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref color, ref frontFaceColor, rasterization, new Vector3I(wireDivideRatio), lineWidth, faceMaterial, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
    }

    public static void DrawTransparentBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color faceX_P,
      ref Color faceY_P,
      ref Color faceZ_P,
      ref Color faceX_N,
      ref Color faceY_N,
      ref Color faceZ_N,
      ref Color wire,
      MySimpleObjectRasterizer rasterization,
      int wireDivideRatio,
      float lineWidth = 0.001f,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      MySimpleObjectDraw.DrawTransparentBox(ref worldMatrix, ref localbox, ref faceX_P, ref faceY_P, ref faceZ_P, ref faceX_N, ref faceY_N, ref faceZ_N, ref wire, rasterization, new Vector3I(wireDivideRatio), lineWidth, faceMaterial, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
    }

    public static void DrawAttachedTransparentBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color color,
      uint renderObjectID,
      ref MatrixD worldToLocal,
      MySimpleObjectRasterizer rasterization,
      int wireDivideRatio,
      float lineWidth = 0.001f,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false)
    {
      MySimpleObjectDraw.DrawAttachedTransparentBox(ref worldMatrix, ref localbox, ref color, renderObjectID, ref worldToLocal, rasterization, new Vector3I(wireDivideRatio), lineWidth, faceMaterial, lineMaterial, onlyFrontFaces);
    }

    public static bool FaceVisible(Vector3D center, Vector3D normal) => Vector3D.Dot(Vector3D.Normalize(center - MyTransparentGeometry.Camera.Translation), normal) < 0.0;

    public static bool FaceVisibleRelative(Vector3D center, Vector3D normal) => Vector3D.Dot(Vector3D.Normalize(center), normal) < 0.0;

    public static void DrawTransparentBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color color,
      ref Color frontFaceColor,
      MySimpleObjectRasterizer rasterization,
      Vector3I wireDivideRatio,
      float lineWidth = 0.001f,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      if (faceMaterial.HasValue)
      {
        MyStringId? nullable = faceMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      faceMaterial = new MyStringId?(MySimpleObjectDraw.ID_CONTAINER_BORDER);
label_3:
      if (rasterization == MySimpleObjectRasterizer.Solid || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
      {
        Vector3 min = (Vector3) localbox.Min;
        Vector3 max = (Vector3) localbox.Max;
        MatrixD identity = MatrixD.Identity;
        identity.Forward = worldMatrix.Forward;
        identity.Up = worldMatrix.Up;
        identity.Right = worldMatrix.Right;
        Vector3D vector3D = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
        float width = (float) (localbox.Max.X - localbox.Min.X) / 2f;
        float height = (float) (localbox.Max.Y - localbox.Min.Y) / 2f;
        float num = (float) (localbox.Max.Z - localbox.Min.Z) / 2f;
        Vector3D normal1 = Vector3D.TransformNormal(Vector3.Forward, identity) * (double) num;
        Vector3D center1 = vector3D + normal1;
        MyQuadD quad;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center1, normal1))
        {
          MyUtils.GenerateQuad(out quad, ref center1, width, height, ref worldMatrix);
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) frontFaceColor, ref center1, customViewProjection, blendType, persistentBillboards);
        }
        Vector3D center2 = vector3D - normal1;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, -normal1))
        {
          MyUtils.GenerateQuad(out quad, ref center2, width, height, ref worldMatrix);
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center2, customViewProjection, blendType, persistentBillboards);
        }
        MatrixD matrix = MatrixD.CreateRotationY((double) MathHelper.ToRadians(90f)) * worldMatrix;
        Vector3D normal2 = (Vector3D) Vector3.TransformNormal(Vector3.Left, worldMatrix) * (double) width;
        Vector3D center3 = vector3D + normal2;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center3, normal2))
        {
          MyUtils.GenerateQuad(out quad, ref center3, num, height, ref matrix);
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center3, customViewProjection, blendType, persistentBillboards);
        }
        Vector3D center4 = vector3D - normal2;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center4, -normal2))
        {
          MyUtils.GenerateQuad(out quad, ref center4, num, height, ref matrix);
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center4, customViewProjection, blendType, persistentBillboards);
        }
        Matrix rotationX = Matrix.CreateRotationX(MathHelper.ToRadians(90f));
        matrix = (MatrixD) ref rotationX * worldMatrix;
        Vector3D normal3 = (Vector3D) Vector3.TransformNormal(Vector3.Up, worldMatrix) * ((localbox.Max.Y - localbox.Min.Y) / 2.0);
        Vector3D center5 = vector3D + normal3;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center5, normal3))
        {
          MyUtils.GenerateQuad(out quad, ref center5, width, num, ref matrix);
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center5, customViewProjection, blendType, persistentBillboards);
        }
        Vector3D center6 = vector3D - normal3;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center6, -normal3))
        {
          MyUtils.GenerateQuad(out quad, ref center6, width, num, ref matrix);
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center6, customViewProjection, blendType, persistentBillboards);
        }
      }
      if (rasterization != MySimpleObjectRasterizer.Wireframe && rasterization != MySimpleObjectRasterizer.SolidAndWireframe)
        return;
      Color color1 = color * 1.3f;
      MySimpleObjectDraw.DrawWireFramedBox(ref worldMatrix, ref localbox, ref color1, lineWidth, wireDivideRatio, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
    }

    public static unsafe void DrawTransparentBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color faceX_P,
      ref Color faceY_P,
      ref Color faceZ_P,
      ref Color faceX_N,
      ref Color faceY_N,
      ref Color faceZ_N,
      ref Color wire,
      MySimpleObjectRasterizer rasterization,
      Vector3I wireDivideRatio,
      float lineWidth = 0.001f,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      if (faceMaterial.HasValue)
      {
        MyStringId? nullable = faceMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      faceMaterial = new MyStringId?(MySimpleObjectDraw.ID_CONTAINER_BORDER);
label_3:
      if (rasterization == MySimpleObjectRasterizer.Solid || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
      {
        Vector3 min = (Vector3) localbox.Min;
        Vector3 max = (Vector3) localbox.Max;
        MatrixD identity = MatrixD.Identity;
        identity.Forward = worldMatrix.Forward;
        identity.Up = worldMatrix.Up;
        identity.Right = worldMatrix.Right;
        Vector3D vector3D = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
        float width = (float) (localbox.Max.X - localbox.Min.X) / 2f;
        float height = (float) (localbox.Max.Y - localbox.Min.Y) / 2f;
        float num = (float) (localbox.Max.Z - localbox.Min.Z) / 2f;
        Vector3D normal1 = Vector3D.TransformNormal(Vector3.Forward, identity) * (double) num;
        Vector3D position1 = vector3D + normal1;
        // ISSUE: untyped stack allocation
        Span<MySimpleObjectDraw.FaceInfo> span = new Span<MySimpleObjectDraw.FaceInfo>((void*) __untypedstackalloc(checked (new IntPtr(6) * sizeof (MySimpleObjectDraw.FaceInfo))), 6);
        bool flag1 = MySimpleObjectDraw.FaceVisibleRelative(position1, normal1);
        MyQuadD quad;
        if (!onlyFrontFaces | flag1)
        {
          MyUtils.GenerateQuad(out quad, ref position1, width, height, ref worldMatrix);
          span[0].Front = flag1;
          span[0].Quad = quad;
          span[0].Pos = position1;
          span[0].Col = faceZ_N;
        }
        else
          span[0].FrontSet = false;
        Vector3D position2 = vector3D - normal1;
        bool flag2 = MySimpleObjectDraw.FaceVisibleRelative(position2, -normal1);
        if (!onlyFrontFaces | flag2)
        {
          MyUtils.GenerateQuad(out quad, ref position2, width, height, ref worldMatrix);
          span[1].Front = flag2;
          span[1].Quad = quad;
          span[1].Pos = position2;
          span[1].Col = faceZ_P;
        }
        else
          span[1].FrontSet = false;
        MatrixD matrix = MatrixD.CreateRotationY((double) MathHelper.ToRadians(90f)) * worldMatrix;
        Vector3D normal2 = (Vector3D) Vector3.TransformNormal(Vector3.Left, worldMatrix) * (double) width;
        Vector3D position3 = vector3D + normal2;
        bool flag3 = MySimpleObjectDraw.FaceVisibleRelative(position3, normal2);
        if (!onlyFrontFaces | flag3)
        {
          MyUtils.GenerateQuad(out quad, ref position3, num, height, ref matrix);
          span[2].Front = flag3;
          span[2].Quad = quad;
          span[2].Pos = position3;
          span[2].Col = faceX_N;
        }
        else
          span[2].FrontSet = false;
        Vector3D position4 = vector3D - normal2;
        bool flag4 = MySimpleObjectDraw.FaceVisibleRelative(position4, -normal2);
        if (!onlyFrontFaces | flag4)
        {
          MyUtils.GenerateQuad(out quad, ref position4, num, height, ref matrix);
          span[3].Front = flag4;
          span[3].Quad = quad;
          span[3].Pos = position4;
          span[3].Col = faceX_P;
        }
        else
          span[3].FrontSet = false;
        Matrix rotationX = Matrix.CreateRotationX(MathHelper.ToRadians(90f));
        matrix = (MatrixD) ref rotationX * worldMatrix;
        Vector3D normal3 = (Vector3D) Vector3.TransformNormal(Vector3.Up, worldMatrix) * ((localbox.Max.Y - localbox.Min.Y) / 2.0);
        Vector3D position5 = vector3D + normal3;
        bool flag5 = MySimpleObjectDraw.FaceVisibleRelative(position5, normal3);
        if (!onlyFrontFaces | flag5)
        {
          MyUtils.GenerateQuad(out quad, ref position5, width, num, ref matrix);
          span[4].Front = flag5;
          span[4].Quad = quad;
          span[4].Pos = position5;
          span[4].Col = faceY_P;
        }
        else
          span[4].FrontSet = false;
        Vector3D position6 = vector3D - normal3;
        bool flag6 = MySimpleObjectDraw.FaceVisibleRelative(position6, -normal3);
        if (!onlyFrontFaces | flag6)
        {
          MyUtils.GenerateQuad(out quad, ref position6, width, num, ref matrix);
          span[5].Front = flag6;
          span[5].Quad = quad;
          span[5].Pos = position6;
          span[5].Col = faceY_N;
        }
        else
          span[5].FrontSet = false;
        for (int index = 0; index < 6; ++index)
        {
          if (span[index].FrontSet && !span[index].Front)
            MyTransparentGeometry.AddQuad(faceMaterial.Value, ref span[index].Quad, (Vector4) span[index].Col, ref span[index].Pos, customViewProjection, blendType, persistentBillboards);
        }
        for (int index = 0; index < 6; ++index)
        {
          if (span[index].FrontSet && span[index].Front)
            MyTransparentGeometry.AddQuad(faceMaterial.Value, ref span[index].Quad, (Vector4) span[index].Col, ref span[index].Pos, customViewProjection, blendType, persistentBillboards);
        }
      }
      if (rasterization != MySimpleObjectRasterizer.Wireframe && rasterization != MySimpleObjectRasterizer.SolidAndWireframe)
        return;
      Color color = wire * 1.3f;
      MySimpleObjectDraw.DrawWireFramedBox(ref worldMatrix, ref localbox, ref color, lineWidth, wireDivideRatio, lineMaterial, onlyFrontFaces, customViewProjection, blendType, intensity, persistentBillboards);
    }

    public static void DrawTransparentRamp(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color color,
      MyStringId? faceMaterial = null,
      bool onlyFrontFaces = false,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
    {
      if (faceMaterial.HasValue)
      {
        MyStringId? nullable = faceMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      faceMaterial = new MyStringId?(MySimpleObjectDraw.ID_CONTAINER_BORDER);
label_3:
      MatrixD identity = MatrixD.Identity;
      identity.Forward = worldMatrix.Forward;
      identity.Up = worldMatrix.Up;
      identity.Right = worldMatrix.Right;
      Vector3D vector3D1 = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
      float width = (float) (localbox.Max.X - localbox.Min.X) / 2f;
      float height = (float) (localbox.Max.Y - localbox.Min.Y) / 2f;
      float num = (float) (localbox.Max.Z - localbox.Min.Z) / 2f;
      Vector3D vector3D2 = Vector3D.TransformNormal(Vector3D.Forward, identity) * (double) num;
      Vector3D center1 = vector3D1 - vector3D2;
      MyQuadD quad;
      if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center1, -vector3D2))
      {
        MyUtils.GenerateQuad(out quad, ref center1, width, height, ref worldMatrix);
        MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center1, customViewProjection, blendType);
      }
      MatrixD matrix = MatrixD.CreateRotationY((double) MathHelper.ToRadians(90f)) * worldMatrix;
      Vector3D normal1 = (Vector3D) Vector3.TransformNormal(Vector3.Left, worldMatrix) * (double) width;
      Vector3D center2 = vector3D1 + normal1;
      if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, normal1))
      {
        MyUtils.GenerateQuad(out quad, ref center2, num, height, ref matrix);
        quad.Point3 = quad.Point0;
        MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center2, customViewProjection, blendType);
      }
      center2 = vector3D1 - normal1;
      if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, -normal1))
      {
        MyUtils.GenerateQuad(out quad, ref center2, num, height, ref matrix);
        quad.Point3 = quad.Point0;
        MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center2, customViewProjection, blendType);
      }
      Vector3D vector3D3 = Vector3D.One;
      Vector3D vector3D4 = Vector3D.One;
      Matrix rotationX = Matrix.CreateRotationX(MathHelper.ToRadians(90f));
      matrix = (MatrixD) ref rotationX * worldMatrix;
      Vector3D normal2 = (Vector3D) Vector3.TransformNormal(Vector3.Up, worldMatrix) * ((localbox.Max.Y - localbox.Min.Y) / 2.0);
      center2 = vector3D1 - normal2;
      if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, -normal2))
      {
        MyUtils.GenerateQuad(out quad, ref center2, width, num, ref matrix);
        vector3D3 = quad.Point1;
        vector3D4 = quad.Point2;
        MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center2, customViewProjection, blendType);
      }
      center2 = vector3D1 + normal2;
      if (onlyFrontFaces && !MySimpleObjectDraw.FaceVisible(center2, normal2))
        return;
      MyUtils.GenerateQuad(out quad, ref center2, width, num, ref matrix);
      quad.Point1 = vector3D3;
      quad.Point2 = vector3D4;
      MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center2, customViewProjection, blendType);
    }

    public static void DrawTransparentRoundedCorner(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color color,
      MyStringId? faceMaterial = null,
      int customViewProjection = -1)
    {
      if (faceMaterial.HasValue)
      {
        MyStringId? nullable = faceMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      faceMaterial = new MyStringId?(MySimpleObjectDraw.ID_CONTAINER_BORDER);
label_3:
      MyQuadD quad;
      quad.Point0 = localbox.Min;
      quad.Point0.Z = localbox.Max.Z;
      quad.Point1 = localbox.Max;
      quad.Point1.Y = localbox.Min.Y;
      quad.Point2 = localbox.Max;
      quad.Point3 = localbox.Max;
      quad.Point3.X = localbox.Min.X;
      quad.Point0 = Vector3D.Transform(quad.Point0, worldMatrix);
      quad.Point1 = Vector3D.Transform(quad.Point1, worldMatrix);
      quad.Point2 = Vector3D.Transform(quad.Point2, worldMatrix);
      quad.Point3 = Vector3D.Transform(quad.Point3, worldMatrix);
      Vector3D vctPos1 = (quad.Point0 + quad.Point1 + quad.Point2 + quad.Point3) * 0.25;
      MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref vctPos1, customViewProjection);
      quad.Point0 = localbox.Min;
      quad.Point0.X = localbox.Max.X;
      quad.Point1 = localbox.Max;
      quad.Point1.Z = localbox.Min.Z;
      quad.Point2 = localbox.Max;
      quad.Point3 = localbox.Max;
      quad.Point3.Y = localbox.Min.Y;
      quad.Point0 = Vector3D.Transform(quad.Point0, worldMatrix);
      quad.Point1 = Vector3D.Transform(quad.Point1, worldMatrix);
      quad.Point2 = Vector3D.Transform(quad.Point2, worldMatrix);
      quad.Point3 = Vector3D.Transform(quad.Point3, worldMatrix);
      Vector3D vctPos2 = (quad.Point0 + quad.Point1 + quad.Point2 + quad.Point3) * 0.25;
      MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref vctPos2, customViewProjection);
      float num1 = 0.1570796f;
      float num2 = (float) (localbox.Max.X - localbox.Min.X);
      float num3 = num2 * 0.5f;
      Vector3D vector3D = (quad.Point2 + quad.Point3) * 0.5;
      Vector3D translation = worldMatrix.Translation;
      worldMatrix.Translation = vector3D;
      for (int index = 20; index < 30; ++index)
      {
        float num4 = (float) index * num1;
        float num5 = num2 * (float) Math.Cos((double) num4);
        float num6 = num2 * (float) Math.Sin((double) num4);
        quad.Point0.X = (double) num5;
        quad.Point0.Z = (double) num6;
        quad.Point3.X = (double) num5;
        quad.Point3.Z = (double) num6;
        float num7 = (float) (index + 1) * num1;
        float num8 = num2 * (float) Math.Cos((double) num7);
        float num9 = num2 * (float) Math.Sin((double) num7);
        quad.Point1.X = (double) num8;
        quad.Point1.Z = (double) num9;
        quad.Point2.X = (double) num8;
        quad.Point2.Z = (double) num9;
        quad.Point0.Y = -(double) num3;
        quad.Point1.Y = -(double) num3;
        quad.Point2.Y = (double) num3;
        quad.Point3.Y = (double) num3;
        quad.Point0 = Vector3D.Transform(quad.Point0, worldMatrix);
        quad.Point1 = Vector3D.Transform(quad.Point1, worldMatrix);
        quad.Point2 = Vector3D.Transform(quad.Point2, worldMatrix);
        quad.Point3 = Vector3D.Transform(quad.Point3, worldMatrix);
        Vector3D vctPos3 = (quad.Point0 + quad.Point1 + quad.Point2 + quad.Point3) * 0.25;
        MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref vctPos3, customViewProjection);
      }
      worldMatrix.Translation = translation;
    }

    public static void DrawAttachedTransparentBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color color,
      uint renderObjectID,
      ref MatrixD worldToLocal,
      MySimpleObjectRasterizer rasterization,
      Vector3I wireDivideRatio,
      float lineWidth = 0.001f,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
    {
      if (faceMaterial.HasValue)
      {
        MyStringId? nullable = faceMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      faceMaterial = new MyStringId?(MySimpleObjectDraw.ID_CONTAINER_BORDER);
label_3:
      if (rasterization == MySimpleObjectRasterizer.Solid || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
      {
        Vector3 min = (Vector3) localbox.Min;
        Vector3 max = (Vector3) localbox.Max;
        MatrixD identity = MatrixD.Identity;
        identity.Forward = worldMatrix.Forward;
        identity.Up = worldMatrix.Up;
        identity.Right = worldMatrix.Right;
        Vector3D vector3D = worldMatrix.Translation + Vector3D.Transform(localbox.Center, identity);
        float width = (float) (localbox.Max.X - localbox.Min.X) / 2f;
        float height = (float) (localbox.Max.Y - localbox.Min.Y) / 2f;
        float num = (float) (localbox.Max.Z - localbox.Min.Z) / 2f;
        Vector3D normal1 = Vector3D.TransformNormal(Vector3D.Forward, identity);
        Vector3D center1 = vector3D + normal1 * (double) num;
        MyQuadD quad;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center1, normal1))
        {
          MyUtils.GenerateQuad(out quad, ref center1, width, height, ref worldMatrix);
          Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
          Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
          Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
          Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
          MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center1, renderObjectID, blendType);
        }
        Vector3D center2 = vector3D - normal1 * (double) num;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, -normal1))
        {
          MyUtils.GenerateQuad(out quad, ref center2, width, height, ref worldMatrix);
          Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
          Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
          Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
          Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
          MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center2, renderObjectID, blendType);
        }
        Matrix rotationY = Matrix.CreateRotationY(MathHelper.ToRadians(90f));
        MatrixD matrix = (MatrixD) ref rotationY * worldMatrix;
        Vector3D normal2 = Vector3D.TransformNormal(Vector3D.Left, worldMatrix);
        Vector3D center3 = vector3D + normal2 * (double) width;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center3, normal2))
        {
          MyUtils.GenerateQuad(out quad, ref center3, num, height, ref matrix);
          Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
          Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
          Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
          Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
          MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center3, renderObjectID, blendType);
        }
        Vector3D center4 = vector3D - normal2 * (double) width;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center4, -normal2))
        {
          MyUtils.GenerateQuad(out quad, ref center4, num, height, ref matrix);
          Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
          Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
          Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
          Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
          MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center4, renderObjectID, blendType);
        }
        matrix = MatrixD.CreateRotationX((double) MathHelper.ToRadians(90f)) * worldMatrix;
        Vector3D normal3 = Vector3D.TransformNormal(Vector3D.Up, worldMatrix);
        Vector3D center5 = vector3D + normal3 * (double) height;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center5, normal3))
        {
          MyUtils.GenerateQuad(out quad, ref center5, width, num, ref matrix);
          Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
          Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
          Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
          Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
          MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center5, renderObjectID, blendType);
        }
        Vector3D center6 = vector3D - normal3 * (double) height;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center6, -normal3))
        {
          MyUtils.GenerateQuad(out quad, ref center6, width, num, ref matrix);
          Vector3D.Transform(ref quad.Point0, ref worldToLocal, out quad.Point0);
          Vector3D.Transform(ref quad.Point1, ref worldToLocal, out quad.Point1);
          Vector3D.Transform(ref quad.Point2, ref worldToLocal, out quad.Point2);
          Vector3D.Transform(ref quad.Point3, ref worldToLocal, out quad.Point3);
          MyTransparentGeometry.AddAttachedQuad(faceMaterial.Value, ref quad, (Vector4) color, ref center6, renderObjectID, blendType);
        }
      }
      if (rasterization != MySimpleObjectRasterizer.Wireframe && rasterization != MySimpleObjectRasterizer.SolidAndWireframe)
        return;
      Vector4 vctColor = (Vector4) color * 1.3f;
      MySimpleObjectDraw.DrawAttachedWireFramedBox(ref worldMatrix, ref localbox, renderObjectID, ref worldToLocal, ref vctColor, lineWidth, wireDivideRatio, lineMaterial, onlyFrontFaces, blendType);
    }

    private static void DrawWireFramedBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      ref Color color,
      float fThickRatio,
      Vector3I wireDivideRatio,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      if (lineMaterial.HasValue)
      {
        MyStringId? nullable = lineMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      lineMaterial = new MyStringId?(MyTransparentMaterials.ErrorMaterial.Id);
label_3:
      List<LineD> poolObject;
      using (PoolManager.Get<List<LineD>>(out poolObject))
      {
        MatrixD identity = MatrixD.Identity;
        identity.Forward = worldMatrix.Forward;
        identity.Up = worldMatrix.Up;
        identity.Right = worldMatrix.Right;
        Vector3D.Dot(identity.Forward, MyTransparentGeometry.Camera.Forward);
        Vector3D.Dot(identity.Right, MyTransparentGeometry.Camera.Forward);
        Vector3D up1 = identity.Up;
        MatrixD matrixD = MyTransparentGeometry.Camera;
        Vector3D forward1 = matrixD.Forward;
        Vector3D.Dot(up1, forward1);
        Vector3D forward2 = identity.Forward;
        Vector3D right = identity.Right;
        Vector3D up2 = identity.Up;
        float x = (float) localbox.Size.X;
        float y = (float) localbox.Size.Y;
        float z = (float) localbox.Size.Z;
        Vector3D vector3D = Vector3D.Transform(localbox.Center, worldMatrix);
        Vector3D center1 = vector3D + forward2 * ((double) z * 0.5);
        Vector3D center2 = vector3D - forward2 * ((double) z * 0.5);
        Vector3D min1 = localbox.Min;
        Vector3D vctEnd1 = min1 + Vector3.Up * y;
        Vector3D vctSideStep = (Vector3D) (Vector3.Right * (x / (float) wireDivideRatio.X));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center1, forward2))
          MySimpleObjectDraw.GenerateLines(min1, vctEnd1, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.X);
        Vector3D vctStart1 = min1 + Vector3.Backward * z;
        Vector3D vctEnd2 = vctStart1 + Vector3.Up * y;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, -forward2))
          MySimpleObjectDraw.GenerateLines(vctStart1, vctEnd2, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.X);
        Vector3D min2 = localbox.Min;
        Vector3D vctEnd3 = min2 + Vector3.Right * x;
        vctSideStep = (Vector3D) (Vector3.Up * (y / (float) wireDivideRatio.Y));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center1, forward2))
          MySimpleObjectDraw.GenerateLines(min2, vctEnd3, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Y);
        Vector3D vctStart2 = min2 + Vector3.Backward * z;
        Vector3D vctEnd4 = vctEnd3 + Vector3.Backward * z;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, -forward2))
          MySimpleObjectDraw.GenerateLines(vctStart2, vctEnd4, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Y);
        matrixD = Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * worldMatrix;
        Matrix matrix = (Matrix) ref matrixD;
        Vector3D center3 = vector3D - right * ((double) x * 0.5);
        Vector3D center4 = vector3D + right * ((double) x * 0.5);
        Vector3D min3 = localbox.Min;
        Vector3D vctEnd5 = min3 + Vector3.Backward * z;
        vctSideStep = (Vector3D) (Vector3.Up * (y / (float) wireDivideRatio.Y));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center3, -right))
          MySimpleObjectDraw.GenerateLines(min3, vctEnd5, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Y);
        Vector3D vctStart3 = localbox.Min + Vector3.Right * x;
        Vector3D vctEnd6 = vctStart3 + Vector3.Backward * z;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center4, right))
          MySimpleObjectDraw.GenerateLines(vctStart3, vctEnd6, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Y);
        Vector3D min4 = localbox.Min;
        Vector3D vctEnd7 = min4 + Vector3.Up * y;
        vctSideStep = (Vector3D) (Vector3.Backward * (z / (float) wireDivideRatio.Z));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center3, -right))
          MySimpleObjectDraw.GenerateLines(min4, vctEnd7, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Z);
        Vector3D vctStart4 = min4 + Vector3.Right * x;
        Vector3D vctEnd8 = vctEnd7 + Vector3.Right * x;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center4, right))
          MySimpleObjectDraw.GenerateLines(vctStart4, vctEnd8, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Z);
        Vector3D center5 = vector3D - up2 * ((double) y * 0.5);
        Vector3D center6 = vector3D + up2 * ((double) y * 0.5);
        Vector3D min5 = localbox.Min;
        Vector3D vctEnd9 = min5 + Vector3.Right * x;
        vctSideStep = (Vector3D) (Vector3.Backward * (z / (float) wireDivideRatio.Z));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center5, -up2))
          MySimpleObjectDraw.GenerateLines(min5, vctEnd9, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Z);
        Vector3D vctStart5 = min5 + Vector3.Up * y;
        Vector3D vctEnd10 = vctEnd9 + Vector3.Up * y;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center6, up2))
          MySimpleObjectDraw.GenerateLines(vctStart5, vctEnd10, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Z);
        Vector3D min6 = localbox.Min;
        Vector3D vctEnd11 = min6 + Vector3.Backward * z;
        vctSideStep = (Vector3D) (Vector3.Right * (x / (float) wireDivideRatio.X));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center5, -up2))
          MySimpleObjectDraw.GenerateLines(min6, vctEnd11, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.X);
        Vector3D vctStart6 = min6 + Vector3.Up * y;
        Vector3D vctEnd12 = vctEnd11 + Vector3.Up * y;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center6, up2))
          MySimpleObjectDraw.GenerateLines(vctStart6, vctEnd12, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.X);
        Vector3 vector3 = new Vector3(localbox.Max.X - localbox.Min.X, localbox.Max.Y - localbox.Min.Y, localbox.Max.Z - localbox.Min.Z);
        float thickness = MathHelper.Max(1f, MathHelper.Min(MathHelper.Min(vector3.X, vector3.Y), vector3.Z)) * fThickRatio;
        foreach (LineD lineD in poolObject)
          MyTransparentGeometry.AddLineBillboard(lineMaterial.Value, (Vector4) color, lineD.From, (Vector3) lineD.Direction, (float) lineD.Length, thickness, blendType, customViewProjection, intensity, persistentBillboards);
      }
    }

    private static void DrawAttachedWireFramedBox(
      ref MatrixD worldMatrix,
      ref BoundingBoxD localbox,
      uint renderObjectID,
      ref MatrixD worldToLocal,
      ref Vector4 vctColor,
      float fThickRatio,
      Vector3I wireDivideRatio,
      MyStringId? lineMaterial = null,
      bool onlyFrontFaces = false,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
    {
      if (lineMaterial.HasValue)
      {
        MyStringId? nullable = lineMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      lineMaterial = new MyStringId?(MyTransparentMaterials.ErrorMaterial.Id);
label_3:
      List<LineD> poolObject;
      using (PoolManager.Get<List<LineD>>(out poolObject))
      {
        MatrixD identity = MatrixD.Identity;
        identity.Forward = worldMatrix.Forward;
        identity.Up = worldMatrix.Up;
        identity.Right = worldMatrix.Right;
        Vector3D.Dot(identity.Forward, MyTransparentGeometry.Camera.Forward);
        Vector3D.Dot(identity.Right, MyTransparentGeometry.Camera.Forward);
        Vector3D up1 = identity.Up;
        MatrixD matrixD = MyTransparentGeometry.Camera;
        Vector3D forward1 = matrixD.Forward;
        Vector3D.Dot(up1, forward1);
        Vector3D forward2 = identity.Forward;
        Vector3D right = identity.Right;
        Vector3D up2 = identity.Up;
        float x = (float) localbox.Size.X;
        float y = (float) localbox.Size.Y;
        float z = (float) localbox.Size.Z;
        Vector3D vector3D = Vector3D.Transform(localbox.Center, worldMatrix);
        Vector3D center1 = vector3D + forward2 * ((double) z * 0.5);
        Vector3D center2 = vector3D - forward2 * ((double) z * 0.5);
        Vector3D min1 = localbox.Min;
        Vector3D vctEnd1 = min1 + Vector3.Up * y;
        Vector3D vctSideStep = (Vector3D) (Vector3.Right * (x / (float) wireDivideRatio.X));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center1, forward2))
          MySimpleObjectDraw.GenerateLines(min1, vctEnd1, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.X);
        Vector3D vctStart1 = min1 + Vector3.Backward * z;
        Vector3D vctEnd2 = vctStart1 + Vector3.Up * y;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, -forward2))
          MySimpleObjectDraw.GenerateLines(vctStart1, vctEnd2, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.X);
        Vector3D min2 = localbox.Min;
        Vector3D vctEnd3 = min2 + Vector3.Right * x;
        vctSideStep = (Vector3D) (Vector3.Up * (y / (float) wireDivideRatio.Y));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center1, forward2))
          MySimpleObjectDraw.GenerateLines(min2, vctEnd3, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Y);
        Vector3D vctStart2 = min2 + Vector3.Backward * z;
        Vector3D vctEnd4 = vctEnd3 + Vector3.Backward * z;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center2, -forward2))
          MySimpleObjectDraw.GenerateLines(vctStart2, vctEnd4, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Y);
        matrixD = Matrix.CreateRotationY(MathHelper.ToRadians(90f)) * worldMatrix;
        Matrix matrix = (Matrix) ref matrixD;
        Vector3D center3 = vector3D - right * ((double) x * 0.5);
        Vector3D center4 = vector3D + right * ((double) x * 0.5);
        Vector3D min3 = localbox.Min;
        Vector3D vctEnd5 = min3 + Vector3.Backward * z;
        vctSideStep = (Vector3D) (Vector3.Up * (y / (float) wireDivideRatio.Y));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center3, -right))
          MySimpleObjectDraw.GenerateLines(min3, vctEnd5, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Y);
        Vector3D vctStart3 = localbox.Min + Vector3.Right * x;
        Vector3D vctEnd6 = vctStart3 + Vector3.Backward * z;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center4, right))
          MySimpleObjectDraw.GenerateLines(vctStart3, vctEnd6, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Y);
        Vector3D min4 = localbox.Min;
        Vector3D vctEnd7 = min4 + Vector3.Up * y;
        vctSideStep = (Vector3D) (Vector3.Backward * (z / (float) wireDivideRatio.Z));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center3, -right))
          MySimpleObjectDraw.GenerateLines(min4, vctEnd7, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Z);
        Vector3D vctStart4 = min4 + Vector3.Right * x;
        Vector3D vctEnd8 = vctEnd7 + Vector3.Right * x;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center4, right))
          MySimpleObjectDraw.GenerateLines(vctStart4, vctEnd8, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Z);
        Vector3D center5 = vector3D - up2 * ((double) y * 0.5);
        Vector3D center6 = vector3D + up2 * ((double) y * 0.5);
        Vector3D min5 = localbox.Min;
        Vector3D vctEnd9 = min5 + Vector3.Right * x;
        vctSideStep = (Vector3D) (Vector3.Backward * (z / (float) wireDivideRatio.Z));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center5, -up2))
          MySimpleObjectDraw.GenerateLines(min5, vctEnd9, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Z);
        Vector3D vctStart5 = min5 + Vector3.Up * y;
        Vector3D vctEnd10 = vctEnd9 + Vector3.Up * y;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center6, up2))
          MySimpleObjectDraw.GenerateLines(vctStart5, vctEnd10, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.Z);
        Vector3D min6 = localbox.Min;
        Vector3D vctEnd11 = min6 + Vector3.Backward * z;
        vctSideStep = (Vector3D) (Vector3.Right * (x / (float) wireDivideRatio.X));
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center5, -up2))
          MySimpleObjectDraw.GenerateLines(min6, vctEnd11, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.X);
        Vector3D vctStart6 = min6 + Vector3.Up * y;
        Vector3D vctEnd12 = vctEnd11 + Vector3.Up * y;
        if (!onlyFrontFaces || MySimpleObjectDraw.FaceVisible(center6, up2))
          MySimpleObjectDraw.GenerateLines(vctStart6, vctEnd12, ref vctSideStep, ref worldMatrix, ref poolObject, wireDivideRatio.X);
        Vector3 vector3 = new Vector3(localbox.Max.X - localbox.Min.X, localbox.Max.Y - localbox.Min.Y, localbox.Max.Z - localbox.Min.Z);
        float thickness = MathHelper.Max(1f, MathHelper.Min(MathHelper.Min(vector3.X, vector3.Y), vector3.Z)) * fThickRatio;
        foreach (LineD lineD in poolObject)
          MyTransparentGeometry.AddLineBillboard(lineMaterial.Value, vctColor, lineD.From, renderObjectID, ref worldToLocal, (Vector3) lineD.Direction, (float) lineD.Length, thickness, blendType);
      }
    }

    public static void DrawTransparentSphere(
      List<Vector3D> verticesBuffer,
      float radius,
      ref Color color,
      MySimpleObjectRasterizer rasterization,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      float lineThickness = -1f,
      int customViewProjectionMatrix = -1,
      List<MyBillboard> persistentBillboards = null,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f)
    {
      Vector3D zero = Vector3D.Zero;
      float thickness = radius * 0.01f;
      if ((double) lineThickness > -1.0)
        thickness = lineThickness;
      for (int index = 0; index < verticesBuffer.Count; index += 4)
      {
        MyQuadD myQuadD;
        myQuadD.Point0 = verticesBuffer[index + 1];
        myQuadD.Point1 = verticesBuffer[index + 3];
        myQuadD.Point2 = verticesBuffer[index + 2];
        myQuadD.Point3 = verticesBuffer[index];
        if (rasterization == MySimpleObjectRasterizer.Solid || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
        {
          MyStringId material = faceMaterial ?? MySimpleObjectDraw.ID_CONTAINER_BORDER;
          ref MyQuadD local1 = ref myQuadD;
          Vector4 color1 = (Vector4) color;
          ref Vector3D local2 = ref zero;
          int customViewProjection = customViewProjectionMatrix;
          List<MyBillboard> myBillboardList = persistentBillboards;
          int num = (int) blendType;
          List<MyBillboard> persistentBillboards1 = myBillboardList;
          MyTransparentGeometry.AddQuad(material, ref local1, color1, ref local2, customViewProjection, (MyBillboard.BlendTypeEnum) num, persistentBillboards1);
        }
        if (rasterization == MySimpleObjectRasterizer.Wireframe || rasterization == MySimpleObjectRasterizer.SolidAndWireframe)
        {
          Vector3D point0 = myQuadD.Point0;
          Vector3 vec1 = (Vector3) (myQuadD.Point1 - point0);
          float length1 = vec1.Length();
          if ((double) length1 > 0.100000001490116)
          {
            Vector3 directionNormalized = MyUtils.Normalize(vec1);
            MyTransparentGeometry.AddLineBillboard(lineMaterial.Value, (Vector4) color, point0, directionNormalized, length1, thickness, blendType, customViewProjectionMatrix, intensity, persistentBillboards);
          }
          Vector3D point1 = myQuadD.Point1;
          Vector3 vec2 = (Vector3) (myQuadD.Point2 - point1);
          float length2 = vec2.Length();
          if ((double) length2 > 0.100000001490116)
          {
            Vector3 directionNormalized = MyUtils.Normalize(vec2);
            MyTransparentGeometry.AddLineBillboard(lineMaterial.Value, (Vector4) color, point1, directionNormalized, length2, thickness, blendType, customViewProjectionMatrix, intensity, persistentBillboards);
          }
        }
      }
    }

    public static void DrawTransparentSphere(
      ref MatrixD worldMatrix,
      float radius,
      ref Color color,
      MySimpleObjectRasterizer rasterization,
      int wireDivideRatio,
      MyStringId? faceMaterial = null,
      MyStringId? lineMaterial = null,
      float lineThickness = -1f,
      int customViewProjectionMatrix = -1,
      List<MyBillboard> persistentBillboards = null,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f)
    {
      if (lineMaterial.HasValue)
      {
        MyStringId? nullable = lineMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      lineMaterial = new MyStringId?(MyTransparentMaterials.ErrorMaterial.Id);
label_3:
      List<Vector3D> poolObject;
      using (PoolManager.Get<List<Vector3D>>(out poolObject))
      {
        MyMeshHelper.GenerateSphere(ref worldMatrix, radius, wireDivideRatio, poolObject);
        MySimpleObjectDraw.DrawTransparentSphere(poolObject, radius, ref color, rasterization, faceMaterial, lineMaterial, lineThickness, customViewProjectionMatrix, persistentBillboards, blendType, intensity);
      }
    }

    public static void DrawTransparentCapsule(
      ref MatrixD worldMatrix,
      float radius,
      float height,
      ref Color color,
      int wireDivideRatio,
      MyStringId? faceMaterial = null,
      int customViewProjectionMatrix = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
    {
      if (faceMaterial.HasValue)
      {
        MyStringId? nullable = faceMaterial;
        MyStringId nullOrEmpty = MyStringId.NullOrEmpty;
        if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == nullOrEmpty ? 1 : 0) : 1) : 0) == 0)
          goto label_3;
      }
      faceMaterial = new MyStringId?(MySimpleObjectDraw.ID_CONTAINER_BORDER);
label_3:
      float num1 = height * 0.5f;
      Vector3D translation = worldMatrix.Translation;
      MatrixD rotationX1 = MatrixD.CreateRotationX(-1.57079601287842);
      rotationX1.Translation = new Vector3D(0.0, (double) num1, 0.0);
      rotationX1 *= worldMatrix;
      List<Vector3D> poolObject;
      using (PoolManager.Get<List<Vector3D>>(out poolObject))
      {
        MyMeshHelper.GenerateSphere(ref rotationX1, radius, wireDivideRatio, poolObject);
        Vector3D zero = Vector3D.Zero;
        int num2 = poolObject.Count / 2;
        for (int index = 0; index < num2; index += 4)
        {
          MyQuadD quad;
          quad.Point0 = poolObject[index + 1];
          quad.Point1 = poolObject[index + 3];
          quad.Point2 = poolObject[index + 2];
          quad.Point3 = poolObject[index];
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref zero, customViewProjectionMatrix, blendType);
        }
        MatrixD rotationX2 = MatrixD.CreateRotationX(-1.57079601287842);
        rotationX2.Translation = new Vector3D(0.0, -(double) num1, 0.0);
        MatrixD worldMatrix1 = rotationX2 * worldMatrix;
        poolObject.Clear();
        MyMeshHelper.GenerateSphere(ref worldMatrix1, radius, wireDivideRatio, poolObject);
        for (int index = num2; index < poolObject.Count; index += 4)
        {
          MyQuadD quad;
          quad.Point0 = poolObject[index + 1];
          quad.Point1 = poolObject[index + 3];
          quad.Point2 = poolObject[index + 2];
          quad.Point3 = poolObject[index];
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref zero, customViewProjectionMatrix, blendType);
        }
        float num3 = 6.283185f / (float) wireDivideRatio;
        for (int index = 0; index < wireDivideRatio; ++index)
        {
          float num4 = (float) index * num3;
          float num5 = radius * (float) Math.Cos((double) num4);
          float num6 = radius * (float) Math.Sin((double) num4);
          MyQuadD quad;
          quad.Point0.X = (double) num5;
          quad.Point0.Z = (double) num6;
          quad.Point3.X = (double) num5;
          quad.Point3.Z = (double) num6;
          float num7 = (float) (index + 1) * num3;
          float num8 = radius * (float) Math.Cos((double) num7);
          float num9 = radius * (float) Math.Sin((double) num7);
          quad.Point1.X = (double) num8;
          quad.Point1.Z = (double) num9;
          quad.Point2.X = (double) num8;
          quad.Point2.Z = (double) num9;
          quad.Point0.Y = -(double) num1;
          quad.Point1.Y = -(double) num1;
          quad.Point2.Y = (double) num1;
          quad.Point3.Y = (double) num1;
          quad.Point0 = Vector3D.Transform(quad.Point0, worldMatrix);
          quad.Point1 = Vector3D.Transform(quad.Point1, worldMatrix);
          quad.Point2 = Vector3D.Transform(quad.Point2, worldMatrix);
          quad.Point3 = Vector3D.Transform(quad.Point3, worldMatrix);
          Vector3D vctPos = (quad.Point0 + quad.Point1 + quad.Point2 + quad.Point3) * 0.25;
          MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref vctPos, customViewProjectionMatrix, blendType);
        }
      }
    }

    public static void DrawTransparentCone(
      ref MatrixD worldMatrix,
      float radius,
      float height,
      ref Color color,
      int wireDivideRatio,
      MyStringId? faceMaterial = null,
      int customViewProjectionMatrix = -1)
    {
      MySimpleObjectDraw.DrawTransparentCone(worldMatrix.Translation, (Vector3) worldMatrix.Forward * height, (Vector3) worldMatrix.Up * radius, color, wireDivideRatio, faceMaterial, customViewProjectionMatrix);
    }

    private static void DrawTransparentCone(
      Vector3D apexPosition,
      Vector3 directionVector,
      Vector3 baseVector,
      Color color,
      int wireDivideRatio,
      MyStringId? faceMaterial = null,
      int customViewProjectionMatrix = -1)
    {
      if (!faceMaterial.HasValue || faceMaterial.Value == MyStringId.NullOrEmpty)
        faceMaterial = new MyStringId?(MySimpleObjectDraw.ID_CONTAINER_BORDER);
      Vector3 axis = directionVector;
      double num1 = (double) axis.Normalize();
      Vector3D vector3D1 = apexPosition;
      float num2 = 6.283185f / (float) wireDivideRatio;
      for (int index = 0; index < wireDivideRatio; ++index)
      {
        float angle1 = (float) index * num2;
        float angle2 = (float) (index + 1) * num2;
        Vector3D vector3D2 = apexPosition + directionVector + Vector3.Transform(baseVector, Matrix.CreateFromAxisAngle(axis, angle1));
        Vector3D vector3D3 = apexPosition + directionVector + Vector3.Transform(baseVector, Matrix.CreateFromAxisAngle(axis, angle2));
        MyQuadD quad;
        quad.Point0 = vector3D2;
        quad.Point1 = vector3D3;
        quad.Point2 = vector3D1;
        quad.Point3 = vector3D1;
        MyTransparentGeometry.AddQuad(faceMaterial.Value, ref quad, (Vector4) color, ref Vector3D.Zero);
      }
    }

    public static void DrawTransparentCuboid(
      ref MatrixD worldMatrix,
      MyCuboid cuboid,
      ref Vector4 vctColor,
      bool bWireFramed,
      float thickness,
      MyStringId? lineMaterial = null)
    {
      foreach (Line uniqueLine in cuboid.UniqueLines)
        MySimpleObjectDraw.DrawLine(Vector3D.Transform(uniqueLine.From, worldMatrix), Vector3D.Transform(uniqueLine.To, worldMatrix), new MyStringId?(lineMaterial ?? MySimpleObjectDraw.ID_GIZMO_DRAW_LINE), ref vctColor, thickness);
    }

    public static void DrawLine(
      Vector3D start,
      Vector3D end,
      MyStringId? material,
      ref Vector4 color,
      float thickness,
      MyBillboard.BlendTypeEnum blendtype = MyBillboard.BlendTypeEnum.Standard)
    {
      Vector3 vec = (Vector3) (end - start);
      float length = vec.Length();
      if ((double) length <= 0.100000001490116)
        return;
      Vector3 directionNormalized = MyUtils.Normalize(vec);
      MyTransparentGeometry.AddLineBillboard(material ?? MySimpleObjectDraw.ID_GIZMO_DRAW_LINE, color, start, directionNormalized, length, thickness, blendtype);
    }

    public static void DrawTransparentCylinder(
      ref MatrixD worldMatrix,
      float radius1,
      float radius2,
      float length,
      ref Vector4 vctColor,
      bool bWireFramed,
      int wireDivideRatio,
      float thickness,
      MyStringId? lineMaterial = null)
    {
      Vector3D position1 = (Vector3D) Vector3.Zero;
      Vector3D position2 = (Vector3D) Vector3.Zero;
      Vector3D vector3D1 = (Vector3D) Vector3.Zero;
      Vector3D vector3D2 = (Vector3D) Vector3.Zero;
      float num1 = 360f / (float) wireDivideRatio;
      for (int index = 0; index <= wireDivideRatio; ++index)
      {
        float degrees = (float) index * num1;
        position1.X = (double) (radius1 * (float) Math.Cos((double) MathHelper.ToRadians(degrees)));
        position1.Y = (double) length / 2.0;
        position1.Z = (double) (radius1 * (float) Math.Sin((double) MathHelper.ToRadians(degrees)));
        position2.X = (double) (radius2 * (float) Math.Cos((double) MathHelper.ToRadians(degrees)));
        position2.Y = -(double) length / 2.0;
        position2.Z = (double) (radius2 * (float) Math.Sin((double) MathHelper.ToRadians(degrees)));
        position1 = Vector3D.Transform(position1, worldMatrix);
        position2 = Vector3D.Transform(position2, worldMatrix);
        Vector3D start1 = position2;
        Vector3D end1 = position1;
        MyStringId? nullable = lineMaterial;
        MyStringId? material1 = new MyStringId?(nullable ?? MySimpleObjectDraw.ID_GIZMO_DRAW_LINE);
        ref Vector4 local1 = ref vctColor;
        double num2 = (double) thickness;
        MySimpleObjectDraw.DrawLine(start1, end1, material1, ref local1, (float) num2);
        if (index > 0)
        {
          Vector3D start2 = vector3D2;
          Vector3D end2 = position2;
          nullable = lineMaterial;
          MyStringId? material2 = new MyStringId?(nullable ?? MySimpleObjectDraw.ID_GIZMO_DRAW_LINE);
          ref Vector4 local2 = ref vctColor;
          double num3 = (double) thickness;
          MySimpleObjectDraw.DrawLine(start2, end2, material2, ref local2, (float) num3);
          Vector3D start3 = vector3D1;
          Vector3D end3 = position1;
          nullable = lineMaterial;
          MyStringId? material3 = new MyStringId?(nullable ?? MySimpleObjectDraw.ID_GIZMO_DRAW_LINE);
          ref Vector4 local3 = ref vctColor;
          double num4 = (double) thickness;
          MySimpleObjectDraw.DrawLine(start3, end3, material3, ref local3, (float) num4);
        }
        vector3D2 = position2;
        vector3D1 = position1;
      }
    }

    public static void DrawTransparentPyramid(
      ref Vector3D start,
      ref MyQuad backQuad,
      ref Vector4 vctColor,
      int divideRatio,
      float thickness,
      MyStringId? lineMaterial = null)
    {
      List<LineD> poolObject;
      using (PoolManager.Get<List<LineD>>(out poolObject))
      {
        Vector3 zero = Vector3.Zero;
        MySimpleObjectDraw.GenerateLines(start, (Vector3D) backQuad.Point0, (Vector3D) backQuad.Point1, ref poolObject, divideRatio);
        MySimpleObjectDraw.GenerateLines(start, (Vector3D) backQuad.Point1, (Vector3D) backQuad.Point2, ref poolObject, divideRatio);
        MySimpleObjectDraw.GenerateLines(start, (Vector3D) backQuad.Point2, (Vector3D) backQuad.Point3, ref poolObject, divideRatio);
        MySimpleObjectDraw.GenerateLines(start, (Vector3D) backQuad.Point3, (Vector3D) backQuad.Point0, ref poolObject, divideRatio);
        foreach (LineD lineD in poolObject)
        {
          Vector3 vec = (Vector3) (lineD.To - lineD.From);
          float length = vec.Length();
          if ((double) length > 0.100000001490116)
          {
            Vector3 directionNormalized = MyUtils.Normalize(vec);
            MyTransparentGeometry.AddLineBillboard(lineMaterial ?? MySimpleObjectDraw.ID_GIZMO_DRAW_LINE, vctColor, lineD.From, directionNormalized, length, thickness);
          }
        }
      }
    }

    private static void GenerateLines(
      Vector3D start,
      Vector3D end1,
      Vector3D end2,
      ref List<LineD> lineBuffer,
      int divideRatio)
    {
      Vector3D vector3D = (end2 - end1) / (double) divideRatio;
      for (int index = 0; index < divideRatio; ++index)
      {
        LineD lineD = new LineD(start, end1 + (double) index * vector3D);
        lineBuffer.Add(lineD);
      }
    }

    private static void GenerateLines(
      Vector3D vctStart,
      Vector3D vctEnd,
      ref Vector3D vctSideStep,
      ref MatrixD worldMatrix,
      ref List<LineD> lineBuffer,
      int divideRatio)
    {
      for (int index = 0; index <= divideRatio; ++index)
      {
        Vector3D from = Vector3D.Transform(vctStart, worldMatrix);
        Vector3D to = Vector3D.Transform(vctEnd, worldMatrix);
        if (lineBuffer.Count < 2000)
        {
          LineD lineD = new LineD(from, to);
          lineBuffer.Add(lineD);
          vctStart += vctSideStep;
          vctEnd += vctSideStep;
        }
      }
    }

    private struct FaceInfo
    {
      private bool m_front;
      public bool FrontSet;
      public MyQuadD Quad;
      public Vector3D Pos;
      public Color Col;

      public bool Front
      {
        get => this.m_front;
        set
        {
          this.FrontSet = true;
          this.m_front = value;
        }
      }
    }
  }
}
