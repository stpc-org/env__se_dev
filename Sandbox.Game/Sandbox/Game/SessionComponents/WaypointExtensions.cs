// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.WaypointExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using System;
using VRage.Game;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
  internal static class WaypointExtensions
  {
    public static MyEntity GetWaypoint(string name)
    {
      MyEntity entity;
      return MyEntities.TryGetEntityByName(name, out entity) ? entity : (MyEntity) null;
    }

    public static MatrixD GetWorldMatrix(this CutsceneSequenceNodeWaypoint waypoint)
    {
      MatrixD matrixD = MatrixD.Identity;
      MyEntity waypoint1 = WaypointExtensions.GetWaypoint(waypoint.Name);
      if (waypoint1 != null)
        matrixD = waypoint1.PositionComp.WorldMatrixRef;
      return matrixD;
    }

    public static Vector3D GetPosition(this CutsceneSequenceNodeWaypoint waypoint)
    {
      MyEntity waypoint1 = WaypointExtensions.GetWaypoint(waypoint.Name);
      return waypoint1 != null ? waypoint1.PositionComp.GetPosition() : Vector3D.Zero;
    }

    public static Vector3D GetBezierPosition(
      this CutsceneSequenceNode cutsceneNode,
      float timeRatio)
    {
      Vector3D vector3D = Vector3D.Zero;
      if (cutsceneNode.Waypoints.Count > 2)
      {
        float num1 = 1f / (float) (cutsceneNode.Waypoints.Count - 1);
        int index = (int) Math.Floor((double) timeRatio / (double) num1);
        float num2 = (timeRatio - (float) index * num1) / num1;
        if (index == cutsceneNode.Waypoints.Count - 1)
        {
          vector3D = cutsceneNode.Waypoints[cutsceneNode.Waypoints.Count - 1].GetPosition();
        }
        else
        {
          if (index > cutsceneNode.Waypoints.Count - 2)
            index = cutsceneNode.Waypoints.Count - 2;
          vector3D = index != 0 ? (index <= cutsceneNode.Waypoints.Count - 3 ? MathHelper.CalculateBezierPoint((double) num2, cutsceneNode.Waypoints[index].GetPosition(), cutsceneNode.Waypoints[index].GetPosition() + (cutsceneNode.Waypoints[index + 1].GetPosition() - cutsceneNode.Waypoints[index - 1].GetPosition()) / 4.0, cutsceneNode.Waypoints[index + 1].GetPosition() - (cutsceneNode.Waypoints[index + 2].GetPosition() - cutsceneNode.Waypoints[index].GetPosition()) / 4.0, cutsceneNode.Waypoints[index + 1].GetPosition()) : MathHelper.CalculateBezierPoint((double) num2, cutsceneNode.Waypoints[index].GetPosition(), cutsceneNode.Waypoints[index].GetPosition() + (cutsceneNode.Waypoints[index + 1].GetPosition() - cutsceneNode.Waypoints[index - 1].GetPosition()) / 4.0, cutsceneNode.Waypoints[index + 1].GetPosition(), cutsceneNode.Waypoints[index + 1].GetPosition())) : MathHelper.CalculateBezierPoint((double) num2, cutsceneNode.Waypoints[index].GetPosition(), cutsceneNode.Waypoints[index].GetPosition(), cutsceneNode.Waypoints[index + 1].GetPosition() - (cutsceneNode.Waypoints[index + 2].GetPosition() - cutsceneNode.Waypoints[index].GetPosition()) / 4.0, cutsceneNode.Waypoints[index + 1].GetPosition());
        }
      }
      return vector3D;
    }

    public static MatrixD GetBezierOrientation(
      this CutsceneSequenceNode cutsceneNode,
      float timeRatio)
    {
      if (cutsceneNode.Waypoints.Count <= 2)
        return MatrixD.Identity;
      Vector3 forward1 = Vector3.Forward;
      Vector3 up1 = Vector3.Up;
      float num1 = 1f / (float) (cutsceneNode.Waypoints.Count - 1);
      int index = (int) Math.Floor((double) timeRatio / (double) num1);
      float num2 = (timeRatio - (float) index * num1) / num1;
      Vector3 forward2;
      Vector3 up2;
      if (index == cutsceneNode.Waypoints.Count - 1)
      {
        forward2 = (Vector3) cutsceneNode.Waypoints[cutsceneNode.Waypoints.Count - 1].GetWorldMatrix().Forward;
        up2 = (Vector3) cutsceneNode.Waypoints[cutsceneNode.Waypoints.Count - 1].GetWorldMatrix().Up;
      }
      else
      {
        if (index > cutsceneNode.Waypoints.Count - 2)
          index = cutsceneNode.Waypoints.Count - 2;
        if (index == 0)
        {
          double t1 = (double) num2;
          MatrixD worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D forward3 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D forward4 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D forward5 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index + 2].GetWorldMatrix();
          Vector3D forward6 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D forward7 = worldMatrix.Forward;
          Vector3D vector3D1 = (forward6 - forward7) / 4.0;
          Vector3D p2_1 = forward5 - vector3D1;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D forward8 = worldMatrix.Forward;
          forward2 = (Vector3) MathHelper.CalculateBezierPoint(t1, forward3, forward4, p2_1, forward8);
          double t2 = (double) num2;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D up3 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D up4 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D up5 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index + 2].GetWorldMatrix();
          Vector3D up6 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D up7 = worldMatrix.Up;
          Vector3D vector3D2 = (up6 - up7) / 4.0;
          Vector3D p2_2 = up5 - vector3D2;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D up8 = worldMatrix.Up;
          up2 = (Vector3) MathHelper.CalculateBezierPoint(t2, up3, up4, p2_2, up8);
        }
        else if (index > cutsceneNode.Waypoints.Count - 3)
        {
          double t1 = (double) num2;
          MatrixD worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D forward3 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D forward4 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D forward5 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index - 1].GetWorldMatrix();
          Vector3D forward6 = worldMatrix.Forward;
          Vector3D vector3D1 = (forward5 - forward6) / 4.0;
          Vector3D p1_1 = forward4 + vector3D1;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D forward7 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D forward8 = worldMatrix.Forward;
          forward2 = (Vector3) MathHelper.CalculateBezierPoint(t1, forward3, p1_1, forward7, forward8);
          double t2 = (double) num2;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D up3 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D up4 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D up5 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index - 1].GetWorldMatrix();
          Vector3D up6 = worldMatrix.Up;
          Vector3D vector3D2 = (up5 - up6) / 4.0;
          Vector3D p1_2 = up4 + vector3D2;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D up7 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D up8 = worldMatrix.Up;
          up2 = (Vector3) MathHelper.CalculateBezierPoint(t2, up3, p1_2, up7, up8);
        }
        else
        {
          double t1 = (double) num2;
          MatrixD worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D forward3 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D forward4 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D forward5 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index - 1].GetWorldMatrix();
          Vector3D forward6 = worldMatrix.Forward;
          Vector3D vector3D1 = (forward5 - forward6) / 4.0;
          Vector3D p1_1 = forward4 + vector3D1;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D forward7 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index + 2].GetWorldMatrix();
          Vector3D forward8 = worldMatrix.Forward;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D forward9 = worldMatrix.Forward;
          Vector3D vector3D2 = (forward8 - forward9) / 4.0;
          Vector3D p2_1 = forward7 - vector3D2;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D forward10 = worldMatrix.Forward;
          forward2 = (Vector3) MathHelper.CalculateBezierPoint(t1, forward3, p1_1, p2_1, forward10);
          double t2 = (double) num2;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D up3 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D up4 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D up5 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index - 1].GetWorldMatrix();
          Vector3D up6 = worldMatrix.Up;
          Vector3D vector3D3 = (up5 - up6) / 4.0;
          Vector3D p1_2 = up4 + vector3D3;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D up7 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index + 2].GetWorldMatrix();
          Vector3D up8 = worldMatrix.Up;
          worldMatrix = cutsceneNode.Waypoints[index].GetWorldMatrix();
          Vector3D up9 = worldMatrix.Up;
          Vector3D vector3D4 = (up8 - up9) / 4.0;
          Vector3D p2_2 = up7 - vector3D4;
          worldMatrix = cutsceneNode.Waypoints[index + 1].GetWorldMatrix();
          Vector3D up10 = worldMatrix.Up;
          up2 = (Vector3) MathHelper.CalculateBezierPoint(t2, up3, p1_2, p2_2, up10);
        }
      }
      return MatrixD.CreateWorld(Vector3D.Zero, forward2, up2);
    }
  }
}
