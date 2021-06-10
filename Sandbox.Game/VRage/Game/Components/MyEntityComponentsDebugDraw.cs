// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyEntityComponentsDebugDraw
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace VRage.Game.Components
{
  [PreloadRequired]
  public class MyEntityComponentsDebugDraw
  {
    public static void DebugDraw()
    {
      if (!MyDebugDrawSettings.ENABLE_DEBUG_DRAW || !MyDebugDrawSettings.DEBUG_DRAW_ENTITY_COMPONENTS || MySector.MainCamera == null)
        return;
      double num1 = 1.5;
      double lineSize = num1 * 0.045;
      double num2 = 0.5;
      Vector3D position1 = MySector.MainCamera.Position;
      Vector3D upVector = MySector.MainCamera.WorldMatrix.Up;
      Vector3D right = MySector.MainCamera.WorldMatrix.Right;
      Vector3D forwardVector = (Vector3D) MySector.MainCamera.ForwardVector;
      BoundingSphereD boundingSphere = new BoundingSphereD(position1, 5.0);
      List<MyEntity> entitiesInSphere = MyEntities.GetEntitiesInSphere(ref boundingSphere);
      Vector3D vector3D1 = Vector3D.Zero;
      Vector3D zero = Vector3D.Zero;
      MatrixD projectionMatrix = MySector.MainCamera.ViewProjectionMatrix;
      Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
      float num3 = (float) safeGuiRectangle.Height / (float) safeGuiRectangle.Width;
      float num4 = 600f;
      float num5 = num4 * num3;
      Vector3D position2 = position1 + 1.0 * forwardVector;
      Vector3D vector3D2 = Vector3D.Transform(position2, projectionMatrix);
      Vector3D vector3D3 = Vector3D.Transform(position2 + Vector3D.Right * 0.100000001490116, projectionMatrix);
      Vector3D vector3D4 = Vector3D.Transform(position2 + Vector3D.Up * 0.100000001490116, projectionMatrix);
      Vector3D vector3D5 = Vector3D.Transform(position2 + Vector3D.Backward * 0.100000001490116, projectionMatrix);
      Vector2 vector2_1 = new Vector2((float) vector3D2.X * num4, (float) vector3D2.Y * -num5 * num3);
      Vector2 vector2_2 = new Vector2((float) vector3D3.X * num4, (float) vector3D3.Y * -num5 * num3) - vector2_1;
      Vector2 vector2_3 = new Vector2((float) vector3D4.X * num4, (float) vector3D4.Y * -num5 * num3) - vector2_1;
      Vector2 vector2_4 = new Vector2((float) vector3D5.X * num4, (float) vector3D5.Y * -num5 * num3) - vector2_1;
      float num6 = 150f;
      Vector2 normalizedCoordinate = MyGuiManager.GetScreenCoordinateFromNormalizedCoordinate(new Vector2(1f, 1f));
      Vector2 vector2_5 = normalizedCoordinate + new Vector2(-num6, 0.0f);
      Vector2 vector2_6 = normalizedCoordinate + new Vector2(0.0f, -num6);
      Vector2 pointFrom1 = (normalizedCoordinate + (normalizedCoordinate + new Vector2(-num6, -num6))) * 0.5f;
      MyRenderProxy.DebugDrawLine2D(pointFrom1, pointFrom1 + vector2_2, Color.Red, Color.Red);
      MyRenderProxy.DebugDrawLine2D(pointFrom1, pointFrom1 + vector2_3, Color.Green, Color.Green);
      MyRenderProxy.DebugDrawLine2D(pointFrom1, pointFrom1 + vector2_4, Color.Blue, Color.Blue);
      MyRenderProxy.DebugDrawText2D(pointFrom1 + vector2_2, "World X", Color.Red, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyRenderProxy.DebugDrawText2D(pointFrom1 + vector2_3, "World Y", Color.Green, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyRenderProxy.DebugDrawText2D(pointFrom1 + vector2_4, "World Z", Color.Blue, 0.5f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
      MyComponentsDebugInputComponent.DetectedEntities.Clear();
      foreach (MyEntity myEntity in entitiesInSphere)
      {
        if (myEntity.PositionComp != null)
        {
          Vector3D position3 = myEntity.PositionComp.GetPosition();
          Vector3D pointTo = position3 + upVector * 0.100000001490116;
          Vector3D pointFrom2 = pointTo - right * num2;
          if (Vector3D.Dot(Vector3D.Normalize(position3 - position1), forwardVector) < 0.9995)
          {
            MatrixD matrixD = myEntity.PositionComp.WorldMatrixRef;
            Vector3D vector3D6 = matrixD.Right * 0.300000011920929;
            matrixD = myEntity.PositionComp.WorldMatrixRef;
            Vector3D vector3D7 = matrixD.Up * 0.300000011920929;
            matrixD = myEntity.PositionComp.WorldMatrixRef;
            Vector3D vector3D8 = matrixD.Backward * 0.300000011920929;
            MyRenderProxy.DebugDrawSphere(position3, 0.01f, Color.White, depthRead: false);
            MyRenderProxy.DebugDrawArrow3D(position3, position3 + vector3D6, Color.Red, new Color?(Color.Red), text: "X");
            MyRenderProxy.DebugDrawArrow3D(position3, position3 + vector3D7, Color.Green, new Color?(Color.Green), text: "Y");
            MyRenderProxy.DebugDrawArrow3D(position3, position3 + vector3D8, Color.Blue, new Color?(Color.Blue), text: "Z");
          }
          else
          {
            if (Vector3D.Distance(position3, vector3D1) < 0.01)
            {
              zero += right * 0.300000011920929;
              upVector = -upVector;
              pointTo = position3 + upVector * 0.100000001490116;
              pointFrom2 = pointTo - right * num2;
            }
            vector3D1 = position3;
            double val1 = Vector3D.Distance(pointFrom2, position1);
            double num7 = Math.Atan(num1 / Math.Max(val1, 0.001));
            float num8 = 0.0f;
            Dictionary<Type, MyComponentBase>.ValueCollection.Enumerator enumerator = myEntity.Components.GetEnumerator();
            MyComponentBase component1 = (MyComponentBase) null;
            while (enumerator.MoveNext())
            {
              component1 = enumerator.Current;
              num8 += (float) MyEntityComponentsDebugDraw.GetComponentLines(component1);
            }
            float num9 = num8 + 1f - (float) MyEntityComponentsDebugDraw.GetComponentLines(component1);
            enumerator.Dispose();
            Vector3D vector3D6 = pointFrom2 + ((double) num9 + 0.5) * upVector * lineSize;
            Vector3D worldCoord = pointFrom2 + ((double) num9 + 1.0) * upVector * lineSize + 0.00999999977648258 * right;
            MyRenderProxy.DebugDrawLine3D(position3, pointTo, Color.White, Color.White, false);
            MyRenderProxy.DebugDrawLine3D(pointFrom2, pointTo, Color.White, Color.White, false);
            MyRenderProxy.DebugDrawLine3D(pointFrom2, vector3D6, Color.White, Color.White, false);
            MyRenderProxy.DebugDrawLine3D(vector3D6, vector3D6 + right * 1.0, Color.White, Color.White, false);
            MyRenderProxy.DebugDrawText3D(worldCoord, myEntity.GetType().ToString() + " - " + myEntity.DisplayName, Color.Orange, (float) num7, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
            MyComponentsDebugInputComponent.DetectedEntities.Add(myEntity);
            foreach (MyComponentBase component2 in (MyComponentContainer) myEntity.Components)
            {
              Vector3D origin = pointFrom2 + (double) num9 * upVector * lineSize;
              MyEntityComponentsDebugDraw.DebugDrawComponent(component2, origin, right, upVector, lineSize, (float) num7);
              string text = !(component2 is MyEntityComponentBase entityComponentBase) ? "" : entityComponentBase.ComponentTypeDebugString;
              MyRenderProxy.DebugDrawText3D(origin - 0.0199999995529652 * right, text, Color.Yellow, (float) num7, false, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
              num9 -= (float) MyEntityComponentsDebugDraw.GetComponentLines(component2);
            }
          }
        }
      }
      entitiesInSphere.Clear();
    }

    private static int GetComponentLines(MyComponentBase component, bool countAll = true)
    {
      int num1 = 1;
      if (component is IMyComponentAggregate)
      {
        int count = (component as IMyComponentAggregate).ChildList.Reader.Count;
        int num2 = 0;
        foreach (MyComponentBase component1 in (component as IMyComponentAggregate).ChildList.Reader)
        {
          ++num2;
          if (num2 < count | countAll)
            num1 += MyEntityComponentsDebugDraw.GetComponentLines(component1);
          else
            ++num1;
        }
      }
      return num1;
    }

    private static void DebugDrawComponent(
      MyComponentBase component,
      Vector3D origin,
      Vector3D rightVector,
      Vector3D upVector,
      double lineSize,
      float textSize)
    {
      Vector3D vector3D1 = rightVector * 0.025000000372529;
      Vector3D vector3D2 = origin + vector3D1 * 3.5;
      Vector3D worldCoord = origin + 2.0 * vector3D1 + rightVector * 0.0149999996647239;
      MyRenderProxy.DebugDrawLine3D(origin, origin + 2.0 * vector3D1, Color.White, Color.White, false);
      string text = component.ToString();
      Color white = Color.White;
      double num1 = (double) textSize;
      MyRenderProxy.DebugDrawText3D(worldCoord, text, white, (float) num1, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
      if (!(component is IMyComponentAggregate))
        return;
      ListReader<MyComponentBase> reader = (component as IMyComponentAggregate).ChildList.Reader;
      if (reader.Count == 0)
        return;
      int num2 = MyEntityComponentsDebugDraw.GetComponentLines(component, false) - 1;
      MyRenderProxy.DebugDrawLine3D(vector3D2 - 0.5 * lineSize * upVector, vector3D2 - (double) num2 * lineSize * upVector, Color.White, Color.White, false);
      Vector3D origin1 = vector3D2 - 1.0 * lineSize * upVector;
      reader = (component as IMyComponentAggregate).ChildList.Reader;
      foreach (MyComponentBase component1 in reader)
      {
        int componentLines = MyEntityComponentsDebugDraw.GetComponentLines(component1);
        MyEntityComponentsDebugDraw.DebugDrawComponent(component1, origin1, rightVector, upVector, lineSize, textSize);
        origin1 -= (double) componentLines * lineSize * upVector;
      }
    }
  }
}
