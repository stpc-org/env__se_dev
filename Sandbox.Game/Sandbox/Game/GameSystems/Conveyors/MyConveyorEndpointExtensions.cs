// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.MyConveyorEndpointExtensions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.Conveyors
{
  internal static class MyConveyorEndpointExtensions
  {
    public static void DebugDraw(this IMyConveyorEndpoint endpoint)
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS)
        return;
      Vector3 vector3_1 = new Vector3();
      for (int index = 0; index < endpoint.GetLineCount(); ++index)
      {
        ConveyorLinePosition position = endpoint.GetPosition(index);
        Vector3 vector3_2 = new Vector3(position.LocalGridPosition) + 0.5f * new Vector3(position.VectorDirection);
        vector3_1 += vector3_2;
      }
      Vector3 vector3_3 = (Vector3) Vector3.Transform(vector3_1 * endpoint.CubeBlock.CubeGrid.GridSize / (float) endpoint.GetLineCount(), endpoint.CubeBlock.CubeGrid.WorldMatrix);
      for (int index = 0; index < endpoint.GetLineCount(); ++index)
      {
        ConveyorLinePosition position1 = endpoint.GetPosition(index);
        MyConveyorLine conveyorLine = endpoint.GetConveyorLine(index);
        Vector3 position2 = (new Vector3(position1.LocalGridPosition) + 0.5f * new Vector3(position1.VectorDirection)) * endpoint.CubeBlock.CubeGrid.GridSize;
        Vector3 position3 = (new Vector3(position1.LocalGridPosition) + 0.4f * new Vector3(position1.VectorDirection)) * endpoint.CubeBlock.CubeGrid.GridSize;
        Vector3 vector3_2 = (Vector3) Vector3.Transform(position2, endpoint.CubeBlock.CubeGrid.WorldMatrix);
        Vector3 vector3_4 = (Vector3) Vector3.Transform(position3, endpoint.CubeBlock.CubeGrid.WorldMatrix);
        Vector3 vector3_5 = Vector3.TransformNormal(position1.VectorDirection * endpoint.CubeBlock.CubeGrid.GridSize * 0.5f, endpoint.CubeBlock.CubeGrid.WorldMatrix);
        Color color1 = conveyorLine.IsFunctional ? Color.Orange : Color.DarkRed;
        Color color2 = conveyorLine.IsWorking ? Color.GreenYellow : color1;
        float num1;
        float num2;
        MyConveyorEndpointExtensions.EndpointDebugShape endpointDebugShape;
        if (conveyorLine.GetEndpoint(0) == null || conveyorLine.GetEndpoint(1) == null)
        {
          if (conveyorLine.Type == MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE)
          {
            num1 = 0.2f;
            num2 = 0.015f;
            endpointDebugShape = MyConveyorEndpointExtensions.EndpointDebugShape.SHAPE_SPHERE;
          }
          else
          {
            num1 = 0.1f;
            num2 = 0.015f;
            endpointDebugShape = MyConveyorEndpointExtensions.EndpointDebugShape.SHAPE_CAPSULE;
          }
        }
        else if (conveyorLine.Type == MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE)
        {
          num1 = 1f;
          num2 = 0.05f;
          endpointDebugShape = MyConveyorEndpointExtensions.EndpointDebugShape.SHAPE_SPHERE;
        }
        else
        {
          num1 = 0.2f;
          num2 = 0.05f;
          endpointDebugShape = MyConveyorEndpointExtensions.EndpointDebugShape.SHAPE_CAPSULE;
        }
        MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_2, (Vector3D) (vector3_2 + vector3_5 * num1), color2, color2, true);
        switch (endpointDebugShape)
        {
          case MyConveyorEndpointExtensions.EndpointDebugShape.SHAPE_SPHERE:
            MyRenderProxy.DebugDrawSphere((Vector3D) vector3_2, num2 * endpoint.CubeBlock.CubeGrid.GridSize, (Color) color2.ToVector3(), depthRead: false);
            break;
          case MyConveyorEndpointExtensions.EndpointDebugShape.SHAPE_CAPSULE:
            MyRenderProxy.DebugDrawCapsule((Vector3D) (vector3_2 - vector3_5 * num1), (Vector3D) (vector3_2 + vector3_5 * num1), num2 * endpoint.CubeBlock.CubeGrid.GridSize, color2, false);
            break;
        }
        if (MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_IDS)
          MyRenderProxy.DebugDrawText3D((Vector3D) vector3_4, conveyorLine.GetHashCode().ToString(), color2, 0.6f, false);
        MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_2, (Vector3D) vector3_3, color2, color2, false);
      }
    }

    private enum EndpointDebugShape
    {
      SHAPE_SPHERE,
      SHAPE_CAPSULE,
    }
  }
}
