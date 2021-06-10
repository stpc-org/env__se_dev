// Decompiled with JetBrains decompiler
// Type: VRage.Game.Utils.MyDebugDrawHelper
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Utils
{
  public static class MyDebugDrawHelper
  {
    public static void DrawNamedPoint(
      Vector3D pos,
      string name,
      Color? color = null,
      MatrixD? cameraViewMatrix = null)
    {
      if (!cameraViewMatrix.HasValue)
        cameraViewMatrix = new MatrixD?(MatrixD.Identity);
      if (!color.HasValue)
        color = new Color?(Color.White);
      MatrixD matrix = cameraViewMatrix.Value;
      matrix = MatrixD.Invert(ref matrix);
      int hashCode = name.GetHashCode();
      Vector3D vector3D = 0.5 * matrix.Right * Math.Cos((double) hashCode) + matrix.Up * (0.75 + 0.25 * Math.Abs(Math.Sin((double) hashCode)));
      MyRenderProxy.DebugDrawText3D(pos + vector3D, name, color.Value, 0.5f, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM);
      MyDebugDrawHelper.DrawDashedLine(pos + vector3D, pos, color.Value);
      MyRenderProxy.DebugDrawSphere(pos, 0.025f, color.Value, depthRead: false);
    }

    public static void DrawDashedLine(Vector3D pos1, Vector3D pos2, Color colorValue)
    {
      float num1 = (float) (0.1 / (pos1 - pos2).Length());
      for (float num2 = 0.0f; (double) num2 < 1.0; num2 += num1)
      {
        Vector3D result1;
        Vector3D.Lerp(ref pos1, ref pos2, (double) num2, out result1);
        Vector3D result2;
        Vector3D.Lerp(ref pos1, ref pos2, (double) num2 + 0.300000011920929 * (double) num1, out result2);
        MyRenderProxy.DebugDrawLine3D(result1, result2, colorValue, colorValue, false);
      }
    }

    public static void DrawNamedColoredAxis(
      MatrixD matrix,
      float axisLengthScale = 1f,
      string name = null,
      Color? color = null)
    {
      if (!color.HasValue)
        color = new Color?(Color.White);
      MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + matrix.Right * ((double) axisLengthScale * 0.800000011920929), color.Value, color.Value, false);
      MyRenderProxy.DebugDrawLine3D(matrix.Translation + matrix.Right * ((double) axisLengthScale * 0.800000011920929), matrix.Translation + matrix.Right * (double) axisLengthScale, Color.Red, Color.Red, false);
      MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + matrix.Up * ((double) axisLengthScale * 0.800000011920929), color.Value, color.Value, false);
      MyRenderProxy.DebugDrawLine3D(matrix.Translation + matrix.Up * ((double) axisLengthScale * 0.800000011920929), matrix.Translation + matrix.Up * (double) axisLengthScale, Color.Green, Color.Green, false);
      MyRenderProxy.DebugDrawLine3D(matrix.Translation, matrix.Translation + matrix.Forward * ((double) axisLengthScale * 0.800000011920929), color.Value, color.Value, false);
      MyRenderProxy.DebugDrawLine3D(matrix.Translation + matrix.Forward * ((double) axisLengthScale * 0.800000011920929), matrix.Translation + matrix.Forward * (double) axisLengthScale, Color.Blue, Color.Blue, false);
      MyDebugDrawHelper.DrawNamedPoint(matrix.Translation, name, color);
    }
  }
}
