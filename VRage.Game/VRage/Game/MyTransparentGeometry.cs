// Decompiled with JetBrains decompiler
// Type: VRage.Game.MyTransparentGeometry
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Game.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game
{
  public class MyTransparentGeometry
  {
    private static MyCamera m_camera;
    private const int MAX_TRANSPARENT_GEOMETRY_COUNT = 4000;
    private const int MAX_NEW_PARTICLES_COUNT = 2800;

    public static bool HasCamera => MyTransparentGeometry.m_camera != null;

    public static MatrixD Camera => MyTransparentGeometry.m_camera.WorldMatrix;

    public static MatrixD CameraView => MyTransparentGeometry.m_camera.ViewMatrix;

    public static void SetCamera(MyCamera camera) => MyTransparentGeometry.m_camera = camera;

    private static bool IsEnabled => MyRenderProxy.DebugOverrides.BillboardsStatic;

    public static void AddLineBillboard(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      Vector3 directionNormalized,
      float length,
      float thickness,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      int customViewProjection = -1,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      MyTransparentGeometry.AddLineBillboard(material, color, origin, uint.MaxValue, ref MatrixD.Identity, directionNormalized, length, thickness, blendType, customViewProjection, intensity, persistentBillboards);
    }

    public static void AddLineBillboard(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      uint renderObjectID,
      ref MatrixD worldToLocal,
      Vector3 directionNormalized,
      float length,
      float thickness,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      int customViewProjection = -1,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      if (!MyTransparentGeometry.IsEnabled)
        return;
      MyBillboard billboard;
      if (persistentBillboards == null)
      {
        MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out billboard);
      }
      else
      {
        billboard = MyRenderProxy.AddPersistentBillboard();
        persistentBillboards.Add(billboard);
      }
      billboard.BlendType = blendType;
      billboard.UVOffset = Vector2.Zero;
      billboard.UVSize = Vector2.One;
      billboard.LocalType = MyBillboard.LocalTypeEnum.Custom;
      MyPolyLineD polyLine;
      polyLine.LineDirectionNormalized = directionNormalized;
      polyLine.Point0 = origin;
      polyLine.Point1 = origin + directionNormalized * length;
      polyLine.Thickness = thickness;
      Vector3D cameraPosition = customViewProjection == -1 ? MyTransparentGeometry.Camera.Translation : MyRenderProxy.BillboardsViewProjectionWrite[customViewProjection].CameraPosition;
      if (Vector3D.IsZero(cameraPosition - polyLine.Point0, 1E-06))
        return;
      MyQuadD retQuad;
      MyUtils.GetPolyLineQuad(out retQuad, ref polyLine, cameraPosition);
      MyTransparentGeometry.CreateBillboard(billboard, ref retQuad, material, ref color, ref origin, customViewProjection);
      if (renderObjectID != uint.MaxValue)
      {
        Vector3D.Transform(ref billboard.Position0, ref worldToLocal, out billboard.Position0);
        Vector3D.Transform(ref billboard.Position1, ref worldToLocal, out billboard.Position1);
        Vector3D.Transform(ref billboard.Position2, ref worldToLocal, out billboard.Position2);
        Vector3D.Transform(ref billboard.Position3, ref worldToLocal, out billboard.Position3);
        billboard.ParentID = renderObjectID;
      }
      billboard.ColorIntensity = intensity;
      MyRenderProxy.AddBillboard(billboard);
    }

    public static void AddLocalLineBillboard(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      uint renderObjectID,
      Vector3 directionNormalized,
      float length,
      float thickness,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      int customViewProjection = -1,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      if (!MyTransparentGeometry.IsEnabled)
        return;
      MyBillboard billboard;
      if (persistentBillboards == null)
      {
        MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out billboard);
      }
      else
      {
        billboard = MyRenderProxy.AddPersistentBillboard();
        persistentBillboards.Add(billboard);
      }
      billboard.BlendType = blendType;
      billboard.UVOffset = Vector2.Zero;
      billboard.UVSize = Vector2.One;
      MyQuadD quad = new MyQuadD();
      MyTransparentGeometry.CreateBillboard(billboard, ref quad, material, ref color, ref origin, customViewProjection);
      billboard.Position0 = origin;
      billboard.Position1 = (Vector3D) directionNormalized;
      billboard.Position2 = new Vector3D((double) length, (double) thickness, 0.0);
      billboard.ParentID = renderObjectID;
      billboard.LocalType = MyBillboard.LocalTypeEnum.Line;
      billboard.ColorIntensity = intensity;
      MyRenderProxy.AddBillboard(billboard);
    }

    public static void AddLocalPointBillboard(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      uint renderObjectID,
      float radius,
      float angle,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      int customViewProjection = -1,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      if (!MyTransparentGeometry.IsEnabled)
        return;
      MyBillboard billboard;
      if (persistentBillboards == null)
      {
        MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out billboard);
      }
      else
      {
        billboard = MyRenderProxy.AddPersistentBillboard();
        persistentBillboards.Add(billboard);
      }
      billboard.BlendType = blendType;
      billboard.UVOffset = Vector2.Zero;
      billboard.UVSize = Vector2.One;
      MyQuadD quad = new MyQuadD();
      MyTransparentGeometry.CreateBillboard(billboard, ref quad, material, ref color, ref origin, customViewProjection);
      billboard.ColorIntensity = intensity;
      billboard.Position0 = origin;
      billboard.Position2 = new Vector3D((double) radius, (double) angle, 0.0);
      billboard.ParentID = renderObjectID;
      billboard.LocalType = MyBillboard.LocalTypeEnum.Point;
      MyRenderProxy.AddBillboard(billboard);
    }

    public static void AddPointBillboard(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      float radius,
      float angle,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
    {
      MyTransparentGeometry.AddPointBillboard(material, color, origin, uint.MaxValue, ref MatrixD.Identity, radius, angle, customViewProjection, blendType);
    }

    public static void AddPointBillboard(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      uint renderObjectID,
      ref MatrixD worldToLocal,
      float radius,
      float angle,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      float intensity = 1f,
      List<MyBillboard> persistentBillboards = null)
    {
      if (!MyTransparentGeometry.IsEnabled)
        return;
      Vector3D vector3D = MyTransparentGeometry.Camera.Translation - origin;
      MyQuadD quad;
      if (!MyUtils.GetBillboardQuadAdvancedRotated(out quad, origin, radius, radius, angle, origin + vector3D))
        return;
      MyBillboard billboard;
      if (persistentBillboards == null)
      {
        MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out billboard);
      }
      else
      {
        billboard = MyRenderProxy.AddPersistentBillboard();
        persistentBillboards.Add(billboard);
      }
      MyTransparentGeometry.CreateBillboard(billboard, ref quad, material, ref color, ref origin, customViewProjection);
      billboard.BlendType = blendType;
      if (renderObjectID != uint.MaxValue)
      {
        Vector3D.Transform(ref billboard.Position0, ref worldToLocal, out billboard.Position0);
        Vector3D.Transform(ref billboard.Position1, ref worldToLocal, out billboard.Position1);
        Vector3D.Transform(ref billboard.Position2, ref worldToLocal, out billboard.Position2);
        Vector3D.Transform(ref billboard.Position3, ref worldToLocal, out billboard.Position3);
        billboard.ParentID = renderObjectID;
      }
      billboard.ColorIntensity = intensity;
      MyRenderProxy.AddBillboard(billboard);
    }

    public static void AddBillboardOrientedCull(
      Vector3 cameraPos,
      MyStringId material,
      Vector4 color,
      Vector3 origin,
      Vector3 leftVector,
      Vector3 upVector,
      float radius,
      int customViewProjection = -1,
      float reflection = 0.0f)
    {
      if ((double) Vector3.Dot(Vector3.Cross(leftVector, upVector), origin - cameraPos) <= 0.0)
        return;
      MyTransparentGeometry.AddBillboardOriented(material, color, (Vector3D) origin, leftVector, upVector, radius, customViewProjection: customViewProjection, reflection: reflection);
    }

    [Obsolete("Only for modders")]
    public static void AddTriangleBillboard(
      Vector3D p0,
      Vector3D p1,
      Vector3D p2,
      Vector3 n0,
      Vector3 n1,
      Vector3 n2,
      Vector2 uv0,
      Vector2 uv1,
      Vector2 uv2,
      MyStringId material,
      uint parentID,
      Vector3D worldPosition,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
    {
      MyTriangleBillboard triangleBillboard;
      MyRenderProxy.TriangleBillboardsPoolWrite.AllocateOrCreate(out triangleBillboard);
      MyTransparentMaterial material1 = MyTransparentMaterials.GetMaterial(material);
      triangleBillboard.BlendType = blendType;
      triangleBillboard.Position0 = p0;
      triangleBillboard.Position1 = p1;
      triangleBillboard.Position2 = p2;
      triangleBillboard.Position3 = p0;
      triangleBillboard.UV0 = uv0;
      triangleBillboard.UV1 = uv1;
      triangleBillboard.UV2 = uv2;
      triangleBillboard.Normal0 = n0;
      triangleBillboard.Normal1 = n1;
      triangleBillboard.Normal2 = n2;
      triangleBillboard.DistanceSquared = (float) Vector3D.DistanceSquared(MyTransparentGeometry.Camera.Translation, worldPosition);
      triangleBillboard.Material = material;
      triangleBillboard.Color = material1.Color;
      triangleBillboard.ColorIntensity = 1f;
      triangleBillboard.CustomViewProjection = -1;
      triangleBillboard.Reflectivity = material1.Reflectivity;
      triangleBillboard.LocalType = MyBillboard.LocalTypeEnum.Custom;
      triangleBillboard.ParentID = parentID;
      MyRenderProxy.AddBillboard((MyBillboard) triangleBillboard);
    }

    [Obsolete("Only for modders")]
    public static void AddTriangleBillboard(
      Vector3D p0,
      Vector3D p1,
      Vector3D p2,
      Vector3 n0,
      Vector3 n1,
      Vector3 n2,
      Vector2 uv0,
      Vector2 uv1,
      Vector2 uv2,
      MyStringId material,
      uint parentID,
      Vector3D worldPosition,
      Vector4 color,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
    {
      MyTriangleBillboard triangleBillboard;
      MyRenderProxy.TriangleBillboardsPoolWrite.AllocateOrCreate(out triangleBillboard);
      MyTransparentMaterial material1 = MyTransparentMaterials.GetMaterial(material);
      triangleBillboard.BlendType = blendType;
      triangleBillboard.Position0 = p0;
      triangleBillboard.Position1 = p1;
      triangleBillboard.Position2 = p2;
      triangleBillboard.Position3 = p0;
      triangleBillboard.UV0 = uv0;
      triangleBillboard.UV1 = uv1;
      triangleBillboard.UV2 = uv2;
      triangleBillboard.Normal0 = n0;
      triangleBillboard.Normal1 = n1;
      triangleBillboard.Normal2 = n2;
      triangleBillboard.DistanceSquared = (float) Vector3D.DistanceSquared(MyTransparentGeometry.Camera.Translation, worldPosition);
      triangleBillboard.Material = material;
      triangleBillboard.Color = color;
      triangleBillboard.ColorIntensity = 1f;
      triangleBillboard.CustomViewProjection = -1;
      triangleBillboard.Reflectivity = material1.Reflectivity;
      triangleBillboard.ParentID = parentID;
      MyRenderProxy.AddBillboard((MyBillboard) triangleBillboard);
    }

    public static void AddBillboardOriented(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      Vector3 leftVector,
      Vector3 upVector,
      float radius,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      int customViewProjection = -1,
      float reflection = 0.0f)
    {
      if (!MyTransparentGeometry.IsEnabled)
        return;
      MyTransparentGeometry.AddBillboardOriented(material, color, origin, leftVector, upVector, radius, radius, Vector2.Zero, blendType, customViewProjection, reflection);
    }

    public static void CreateBillboard(
      MyBillboard billboard,
      ref MyQuadD quad,
      MyStringId material,
      ref Vector4 color,
      ref Vector3D origin,
      int customViewProjection = -1,
      float reflection = 0.0f)
    {
      MyTransparentGeometry.CreateBillboard(billboard, ref quad, material, ref color, ref origin, Vector2.Zero, customViewProjection, reflection);
    }

    public static void CreateBillboard(
      MyBillboard billboard,
      ref MyQuadD quad,
      MyStringId material,
      ref Vector4 color,
      ref Vector3D origin,
      Vector2 uvOffset,
      int customViewProjection = -1,
      float reflectivity = 0.0f)
    {
      MyTransparentMaterial material1 = MyTransparentMaterials.GetMaterial(material);
      if (material1.Id == MyTransparentMaterials.ErrorMaterial.Id)
        color = Vector4.One;
      billboard.Material = material;
      billboard.LocalType = MyBillboard.LocalTypeEnum.Custom;
      billboard.Position0 = quad.Point0;
      billboard.Position1 = quad.Point1;
      billboard.Position2 = quad.Point2;
      billboard.Position3 = quad.Point3;
      billboard.UVOffset = uvOffset;
      billboard.UVSize = Vector2.One;
      Vector3D vector3D = customViewProjection == -1 ? MyTransparentGeometry.Camera.Translation : MyRenderProxy.BillboardsViewProjectionWrite[customViewProjection].CameraPosition;
      billboard.DistanceSquared = (float) Vector3D.DistanceSquared(vector3D, origin);
      billboard.Color = color.ToLinearRGB();
      billboard.ColorIntensity = 1f;
      billboard.Reflectivity = reflectivity;
      billboard.CustomViewProjection = customViewProjection;
      billboard.ParentID = uint.MaxValue;
      billboard.SoftParticleDistanceScale = 1f;
      if (material1.AlphaMistingEnable)
        billboard.Color *= MathHelper.Clamp((float) ((Math.Sqrt((double) billboard.DistanceSquared) - (double) material1.AlphaMistingStart) / ((double) material1.AlphaMistingEnd - (double) material1.AlphaMistingStart)), 0.0f, 1f);
      billboard.Color *= material1.Color;
    }

    public static void AddBillboardOriented(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      Vector3 leftVector,
      Vector3 upVector,
      float width,
      float height)
    {
      MyTransparentGeometry.AddBillboardOriented(material, color, origin, leftVector, upVector, width, height, Vector2.Zero);
    }

    public static void AddBillboardOriented(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      Vector3 leftVector,
      Vector3 upVector,
      float radius,
      int customProjection,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard)
    {
      MyTransparentGeometry.AddBillboardOriented(material, color, origin, leftVector, upVector, radius, blendType, customProjection);
    }

    public static void AddBillboardOriented(
      MyStringId material,
      Vector4 color,
      Vector3D origin,
      Vector3 leftVector,
      Vector3 upVector,
      float width,
      float height,
      Vector2 uvOffset,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      int customViewProjection = -1,
      float reflection = 0.0f,
      List<MyBillboard> persistentBillboards = null)
    {
      if (!MyTransparentGeometry.IsEnabled)
        return;
      MyBillboard billboard;
      if (persistentBillboards == null)
      {
        MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out billboard);
      }
      else
      {
        billboard = MyRenderProxy.AddPersistentBillboard();
        persistentBillboards.Add(billboard);
      }
      MyQuadD quad;
      MyUtils.GetBillboardQuadOriented(out quad, ref origin, width, height, ref leftVector, ref upVector);
      MyTransparentGeometry.CreateBillboard(billboard, ref quad, material, ref color, ref origin, uvOffset, customViewProjection);
      billboard.BlendType = blendType;
      billboard.Reflectivity = reflection;
      MyRenderProxy.AddBillboard(billboard);
    }

    public static bool AddQuad(
      MyStringId material,
      ref MyQuadD quad,
      Vector4 color,
      ref Vector3D vctPos,
      int customViewProjection = -1,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      List<MyBillboard> persistentBillboards = null)
    {
      if (!MyTransparentGeometry.IsEnabled)
        return false;
      MyBillboard billboard;
      if (persistentBillboards == null)
      {
        MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out billboard);
      }
      else
      {
        billboard = MyRenderProxy.AddPersistentBillboard();
        persistentBillboards.Add(billboard);
      }
      MyTransparentGeometry.CreateBillboard(billboard, ref quad, material, ref color, ref vctPos, customViewProjection);
      billboard.BlendType = blendType;
      MyRenderProxy.AddBillboard(billboard);
      return true;
    }

    public static bool AddAttachedQuad(
      MyStringId material,
      ref MyQuadD quad,
      Vector4 color,
      ref Vector3D vctPos,
      uint renderObjectID,
      MyBillboard.BlendTypeEnum blendType = MyBillboard.BlendTypeEnum.Standard,
      List<MyBillboard> persistentBillboards = null)
    {
      if (!MyTransparentGeometry.IsEnabled)
        return false;
      MyBillboard billboard;
      if (persistentBillboards == null)
      {
        MyRenderProxy.BillboardsPoolWrite.AllocateOrCreate(out billboard);
      }
      else
      {
        billboard = MyRenderProxy.AddPersistentBillboard();
        persistentBillboards.Add(billboard);
      }
      MyTransparentGeometry.CreateBillboard(billboard, ref quad, material, ref color, ref vctPos);
      billboard.ParentID = renderObjectID;
      billboard.BlendType = blendType;
      MyRenderProxy.AddBillboard(billboard);
      return true;
    }

    [Conditional("PARTICLE_PROFILING")]
    public static void StartParticleProfilingBlock(string name)
    {
    }

    [Conditional("PARTICLE_PROFILING")]
    public static void EndParticleProfilingBlock()
    {
    }
  }
}
