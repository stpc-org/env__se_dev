// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyBlockBuilderRotationHints
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
  public class MyBlockBuilderRotationHints
  {
    private static readonly MyStringId ID_SQUARE_FULL_COLOR = MyStringId.GetOrCompute("SquareFullColor");
    private static readonly MyStringId ID_ARROW_LEFT_GREEN = MyStringId.GetOrCompute("ArrowLeftGreen");
    private static readonly MyStringId ID_ARROW_RIGHT_GREEN = MyStringId.GetOrCompute("ArrowRightGreen");
    private static readonly MyStringId ID_ARROW_GREEN = MyStringId.GetOrCompute("ArrowGreen");
    private static readonly MyStringId ID_ARROW_LEFT_RED = MyStringId.GetOrCompute("ArrowLeftRed");
    private static readonly MyStringId ID_ARROW_RIGHT_RED = MyStringId.GetOrCompute("ArrowRightRed");
    private static readonly MyStringId ID_ARROW_RED = MyStringId.GetOrCompute("ArrowRed");
    private static readonly MyStringId ID_ARROW_LEFT_BLUE = MyStringId.GetOrCompute("ArrowLeftBlue");
    private static readonly MyStringId ID_ARROW_RIGHT_BLUE = MyStringId.GetOrCompute("ArrowRightBlue");
    private static readonly MyStringId ID_ARROW_BLUE = MyStringId.GetOrCompute("ArrowBlue");
    private Vector3D[] m_cubeVertices = new Vector3D[8];
    private List<MyBlockBuilderRotationHints.BoxEdge> m_cubeEdges = new List<MyBlockBuilderRotationHints.BoxEdge>(3);
    private MyBillboardViewProjection m_viewProjection;
    private const MyBillboard.BlendTypeEnum HINT_CUBE_BLENDTYPE = MyBillboard.BlendTypeEnum.LDR;

    public int RotationRightAxis { get; private set; }

    public int RotationRightDirection { get; private set; }

    public int RotationUpAxis { get; private set; }

    public int RotationUpDirection { get; private set; }

    public int RotationForwardAxis { get; private set; }

    public int RotationForwardDirection { get; private set; }

    public MyBlockBuilderRotationHints() => this.Clear();

    private static int GetBestAxis(
      List<MyBlockBuilderRotationHints.BoxEdge> edgeList,
      Vector3D fitVector,
      out int direction)
    {
      double num1 = double.MaxValue;
      int index1 = -1;
      direction = 0;
      for (int index2 = 0; index2 < edgeList.Count; ++index2)
      {
        double num2 = Vector3D.Dot(fitVector, edgeList[index2].Edge.Direction);
        int num3 = Math.Sign(num2);
        double num4 = 1.0 - Math.Abs(num2);
        if (num4 < num1)
        {
          num1 = num4;
          index1 = index2;
          direction = num3;
        }
      }
      int axis = edgeList[index1].Axis;
      edgeList.RemoveAt(index1);
      return axis;
    }

    private static void GetClosestCubeEdge(
      Vector3D[] vertices,
      Vector3D cameraPosition,
      int[] startIndices,
      int[] endIndices,
      out int edgeIndex,
      out int edgeIndex2)
    {
      edgeIndex = -1;
      edgeIndex2 = -1;
      float num1 = float.MaxValue;
      float num2 = float.MaxValue;
      for (int index = 0; index < 4; ++index)
      {
        Vector3D vector3D = (vertices[startIndices[index]] + vertices[endIndices[index]]) * 0.5;
        float num3 = (float) Vector3D.Distance(cameraPosition, vector3D);
        if ((double) num3 < (double) num1)
        {
          edgeIndex2 = edgeIndex;
          edgeIndex = index;
          num2 = num1;
          num1 = num3;
        }
        else if ((double) num3 < (double) num2)
        {
          edgeIndex2 = index;
          num2 = num3;
        }
      }
    }

    public void Clear()
    {
      this.RotationRightAxis = -1;
      this.RotationRightDirection = -1;
      this.RotationUpAxis = -1;
      this.RotationUpDirection = -1;
      this.RotationForwardAxis = -1;
      this.RotationForwardDirection = -1;
    }

    public void ReleaseRenderData() => MyRenderProxy.RemoveBillboardViewProjection(0);

    public void CalculateRotationHints(
      MatrixD drawMatrix,
      bool draw,
      bool fixedAxes = false,
      bool hideForwardAndUpArrows = false)
    {
      Matrix matrix1 = (Matrix) ref MySector.MainCamera.ViewMatrix;
      MatrixD matrix2 = MatrixD.Invert((MatrixD) ref matrix1);
      if (!drawMatrix.IsValid() || !matrix1.IsValid())
        return;
      matrix2.Translation = drawMatrix.Translation - 7.0 * matrix2.Forward + 1.0 * matrix2.Left - 0.600000023841858 * matrix2.Up;
      drawMatrix.Translation -= matrix2.Translation;
      this.m_viewProjection.CameraPosition = matrix2.Translation;
      matrix2.Translation = Vector3D.Zero;
      MatrixD matrixD = MatrixD.Transpose(matrix2);
      this.m_viewProjection.ViewAtZero = (Matrix) ref matrixD;
      Vector2 fromNormalizedSize = MyGuiManager.GetScreenSizeFromNormalizedSize(Vector2.One);
      float num1 = 2.75f;
      int num2 = (int) ((double) fromNormalizedSize.X / (double) num1);
      int num3 = (int) ((double) fromNormalizedSize.Y / (double) num1);
      int num4 = 0;
      int num5 = 0;
      this.m_viewProjection.Viewport = new MyViewport((float) ((int) MySector.MainCamera.Viewport.Width - num2 - num4), (float) num5, (float) num2, (float) num3);
      this.m_viewProjection.Projection = Matrix.CreatePerspectiveFieldOfView(0.7853982f, (float) num2 / (float) num3, 0.1f, 10f);
      BoundingBoxD localbox = new BoundingBoxD((Vector3D) -new Vector3(MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large) * 0.5f), (Vector3D) (new Vector3(MyDefinitionManager.Static.GetCubeSize(MyCubeSize.Large)) * 0.5f));
      int num6 = 0;
      MyRenderProxy.AddBillboardViewProjection(num6, this.m_viewProjection);
      if (draw)
      {
        Color red = Color.Red;
        Color green = Color.Green;
        Color blue = Color.Blue;
        Color white1 = Color.White;
        Color white2 = Color.White;
        Color white3 = Color.White;
        Color white4 = Color.White;
        MySimpleObjectDraw.DrawTransparentBox(ref drawMatrix, ref localbox, ref red, ref green, ref blue, ref white1, ref white2, ref white3, ref white4, MySimpleObjectRasterizer.Solid, 1, 0.04f, new MyStringId?(MyBlockBuilderRotationHints.ID_SQUARE_FULL_COLOR), customViewProjection: num6, blendType: MyBillboard.BlendTypeEnum.LDR);
      }
      new MyOrientedBoundingBoxD(Vector3D.Transform(localbox.Center, drawMatrix), localbox.HalfExtents, Quaternion.CreateFromRotationMatrix(in drawMatrix)).GetCorners(this.m_cubeVertices, 0);
      int edgeIndex1;
      int edgeIndex2_1;
      MyBlockBuilderRotationHints.GetClosestCubeEdge(this.m_cubeVertices, Vector3D.Zero, MyOrientedBoundingBox.StartXVertices, MyOrientedBoundingBox.EndXVertices, out edgeIndex1, out edgeIndex2_1);
      Vector3D cubeVertex1 = this.m_cubeVertices[MyOrientedBoundingBox.StartXVertices[edgeIndex1]];
      Vector3D cubeVertex2 = this.m_cubeVertices[MyOrientedBoundingBox.EndXVertices[edgeIndex1]];
      Vector3D cubeVertex3 = this.m_cubeVertices[MyOrientedBoundingBox.StartXVertices[edgeIndex2_1]];
      Vector3D cubeVertex4 = this.m_cubeVertices[MyOrientedBoundingBox.EndXVertices[edgeIndex2_1]];
      int edgeIndex2;
      int edgeIndex2_2;
      MyBlockBuilderRotationHints.GetClosestCubeEdge(this.m_cubeVertices, Vector3D.Zero, MyOrientedBoundingBox.StartYVertices, MyOrientedBoundingBox.EndYVertices, out edgeIndex2, out edgeIndex2_2);
      Vector3D cubeVertex5 = this.m_cubeVertices[MyOrientedBoundingBox.StartYVertices[edgeIndex2]];
      Vector3D cubeVertex6 = this.m_cubeVertices[MyOrientedBoundingBox.EndYVertices[edgeIndex2]];
      Vector3D cubeVertex7 = this.m_cubeVertices[MyOrientedBoundingBox.StartYVertices[edgeIndex2_2]];
      Vector3D cubeVertex8 = this.m_cubeVertices[MyOrientedBoundingBox.EndYVertices[edgeIndex2_2]];
      int edgeIndex3;
      int edgeIndex2_3;
      MyBlockBuilderRotationHints.GetClosestCubeEdge(this.m_cubeVertices, Vector3D.Zero, MyOrientedBoundingBox.StartZVertices, MyOrientedBoundingBox.EndZVertices, out edgeIndex3, out edgeIndex2_3);
      Vector3D cubeVertex9 = this.m_cubeVertices[MyOrientedBoundingBox.StartZVertices[edgeIndex3]];
      Vector3D cubeVertex10 = this.m_cubeVertices[MyOrientedBoundingBox.EndZVertices[edgeIndex3]];
      Vector3D cubeVertex11 = this.m_cubeVertices[MyOrientedBoundingBox.StartZVertices[edgeIndex2_3]];
      Vector3D cubeVertex12 = this.m_cubeVertices[MyOrientedBoundingBox.EndZVertices[edgeIndex2_3]];
      this.m_cubeEdges.Clear();
      this.m_cubeEdges.Add(new MyBlockBuilderRotationHints.BoxEdge()
      {
        Axis = 0,
        Edge = new LineD(cubeVertex1, cubeVertex2)
      });
      List<MyBlockBuilderRotationHints.BoxEdge> cubeEdges1 = this.m_cubeEdges;
      MyBlockBuilderRotationHints.BoxEdge boxEdge1 = new MyBlockBuilderRotationHints.BoxEdge();
      boxEdge1.Axis = 1;
      boxEdge1.Edge = new LineD(cubeVertex5, cubeVertex6);
      MyBlockBuilderRotationHints.BoxEdge boxEdge2 = boxEdge1;
      cubeEdges1.Add(boxEdge2);
      List<MyBlockBuilderRotationHints.BoxEdge> cubeEdges2 = this.m_cubeEdges;
      boxEdge1 = new MyBlockBuilderRotationHints.BoxEdge();
      boxEdge1.Axis = 2;
      boxEdge1.Edge = new LineD(cubeVertex9, cubeVertex10);
      MyBlockBuilderRotationHints.BoxEdge boxEdge3 = boxEdge1;
      cubeEdges2.Add(boxEdge3);
      if (!fixedAxes)
      {
        int direction;
        this.RotationRightAxis = MyBlockBuilderRotationHints.GetBestAxis(this.m_cubeEdges, MySector.MainCamera.WorldMatrix.Right, out direction);
        this.RotationRightDirection = direction;
        this.RotationUpAxis = MyBlockBuilderRotationHints.GetBestAxis(this.m_cubeEdges, MySector.MainCamera.WorldMatrix.Up, out direction);
        this.RotationUpDirection = direction;
        this.RotationForwardAxis = MyBlockBuilderRotationHints.GetBestAxis(this.m_cubeEdges, MySector.MainCamera.WorldMatrix.Forward, out direction);
        this.RotationForwardDirection = direction;
      }
      string controlButtonName1 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
      string controlButtonName2 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
      string controlButtonName3 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
      string controlButtonName4 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
      string controlButtonName5 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
      string controlButtonName6 = MyInput.Static.GetGameControl(MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard);
      if (MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed)
      {
        controlButtonName1 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_HORISONTAL_POSITIVE).ToString();
        controlButtonName2 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_HORISONTAL_NEGATIVE).ToString();
        controlButtonName3 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_VERTICAL_POSITIVE).ToString();
        controlButtonName4 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_VERTICAL_NEGATIVE).ToString();
        controlButtonName5 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_ROLL_POSITIVE).ToString();
        controlButtonName6 = MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.AX_BUILD, MyControlsSpace.CUBE_ROTATE_ROLL_NEGATIVE).ToString();
      }
      Vector3D vector3D1 = Vector3D.Zero;
      Vector3D vector3D2 = Vector3D.Zero;
      Vector3D vector3D3 = Vector3D.Zero;
      Vector3D vector3D4 = Vector3D.Zero;
      Vector3D vector3D5 = Vector3D.Zero;
      Vector3D vector3D6 = Vector3D.Zero;
      Vector3D vector3D7 = Vector3D.Zero;
      Vector3D vector3D8 = Vector3D.Zero;
      Vector3D vector3D9 = Vector3D.Zero;
      Vector3D vector3D10 = Vector3D.Zero;
      Vector3D vector3D11 = Vector3D.Zero;
      Vector3D vector3D12 = Vector3D.Zero;
      int axis1 = -1;
      int axis2 = -1;
      int axis3 = -1;
      int num7 = -1;
      int num8 = -1;
      int num9 = -1;
      int num10 = -1;
      int num11 = -1;
      int num12 = -1;
      if (this.RotationRightAxis == 0)
      {
        vector3D1 = cubeVertex1;
        vector3D2 = cubeVertex2;
        vector3D7 = cubeVertex3;
        vector3D8 = cubeVertex4;
        axis1 = 0;
        num7 = edgeIndex1;
        num10 = edgeIndex2_1;
      }
      else if (this.RotationRightAxis == 1)
      {
        vector3D1 = cubeVertex5;
        vector3D2 = cubeVertex6;
        vector3D7 = cubeVertex7;
        vector3D8 = cubeVertex8;
        axis1 = 1;
        num7 = edgeIndex2;
        num10 = edgeIndex2_2;
      }
      else if (this.RotationRightAxis == 2)
      {
        vector3D1 = cubeVertex9;
        vector3D2 = cubeVertex10;
        vector3D7 = cubeVertex11;
        vector3D8 = cubeVertex12;
        axis1 = 2;
        num7 = edgeIndex3;
        num10 = edgeIndex2_3;
      }
      if (this.RotationUpAxis == 0)
      {
        vector3D3 = cubeVertex1;
        vector3D4 = cubeVertex2;
        vector3D9 = cubeVertex3;
        vector3D10 = cubeVertex4;
        axis2 = 0;
        num8 = edgeIndex1;
        num11 = edgeIndex2_1;
      }
      else if (this.RotationUpAxis == 1)
      {
        vector3D3 = cubeVertex5;
        vector3D4 = cubeVertex6;
        vector3D9 = cubeVertex7;
        vector3D10 = cubeVertex8;
        axis2 = 1;
        num8 = edgeIndex2;
        num11 = edgeIndex2_2;
      }
      else if (this.RotationUpAxis == 2)
      {
        vector3D3 = cubeVertex9;
        vector3D4 = cubeVertex10;
        vector3D9 = cubeVertex11;
        vector3D10 = cubeVertex12;
        axis2 = 2;
        num8 = edgeIndex3;
        num11 = edgeIndex2_3;
      }
      if (this.RotationForwardAxis == 0)
      {
        vector3D5 = cubeVertex1;
        vector3D6 = cubeVertex2;
        vector3D11 = cubeVertex3;
        vector3D12 = cubeVertex4;
        axis3 = 0;
        num9 = edgeIndex1;
        num12 = edgeIndex2_1;
      }
      else if (this.RotationForwardAxis == 1)
      {
        vector3D5 = cubeVertex5;
        vector3D6 = cubeVertex6;
        vector3D11 = cubeVertex7;
        vector3D12 = cubeVertex8;
        axis3 = 1;
        num9 = edgeIndex2;
        num12 = edgeIndex2_2;
      }
      else if (this.RotationForwardAxis == 2)
      {
        vector3D5 = cubeVertex9;
        vector3D6 = cubeVertex10;
        vector3D11 = cubeVertex11;
        vector3D12 = cubeVertex12;
        axis3 = 2;
        num9 = edgeIndex3;
        num12 = edgeIndex2_3;
      }
      float scale = 0.5448648f;
      if (!draw)
        return;
      Vector3D forwardVector = (Vector3D) MySector.MainCamera.ForwardVector;
      Vector3D vector3D13 = (Vector3D) Vector3.Normalize(vector3D2 - vector3D1);
      Vector3D vector3D14 = (Vector3D) Vector3.Normalize(vector3D4 - vector3D3);
      Vector3D vector3D15 = (Vector3D) Vector3.Normalize(vector3D6 - vector3D5);
      float num13 = Math.Abs(Vector3.Dot((Vector3) forwardVector, (Vector3) vector3D13));
      float num14 = Math.Abs(Vector3.Dot((Vector3) forwardVector, (Vector3) vector3D14));
      float num15 = Math.Abs(Vector3.Dot((Vector3) forwardVector, (Vector3) vector3D15));
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      float num16 = 0.4f;
      if ((double) num13 < (double) num16)
      {
        if ((double) num14 < (double) num16)
        {
          flag6 = true;
          flag1 = true;
          flag2 = true;
        }
        else if ((double) num15 < (double) num16)
        {
          flag5 = true;
          flag1 = true;
          flag3 = true;
        }
        else
        {
          flag2 = true;
          flag3 = true;
        }
      }
      else if ((double) num14 < (double) num16)
      {
        if ((double) num13 < (double) num16)
        {
          flag6 = true;
          flag1 = true;
          flag2 = true;
        }
        else if ((double) num15 < (double) num16)
        {
          flag4 = true;
          flag2 = true;
          flag3 = true;
        }
        else
        {
          flag1 = true;
          flag3 = true;
        }
      }
      else if ((double) num15 < (double) num16)
      {
        if ((double) num13 < (double) num16)
        {
          flag5 = true;
          flag1 = true;
          flag3 = true;
        }
        else if ((double) num14 < (double) num16)
        {
          flag5 = true;
          flag1 = true;
          flag3 = true;
        }
        else
        {
          flag2 = true;
          flag1 = true;
        }
      }
      if (!hideForwardAndUpArrows || this.RotationRightAxis == 1)
      {
        if (flag4)
        {
          Vector3D vector3D16 = (vector3D5 + vector3D6 + vector3D11 + vector3D12) * 0.25;
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_LEFT_GREEN, Vector4.One, vector3D16 - (double) this.RotationForwardDirection * vector3D15 * 0.200000002980232 - (double) this.RotationRightDirection * vector3D13 * 0.00999999977648258, (Vector3) ((double) -this.RotationUpDirection * vector3D14), (Vector3) ((double) -this.RotationForwardDirection * vector3D15), 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_RIGHT_GREEN, Vector4.One, vector3D16 + (double) this.RotationForwardDirection * vector3D15 * 0.200000002980232 - (double) this.RotationRightDirection * vector3D13 * 0.00999999977648258, (Vector3) ((double) this.RotationUpDirection * vector3D14), (Vector3) ((double) this.RotationForwardDirection * vector3D15), 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
          if (this.ShouldDrawText())
          {
            MyRenderProxy.DebugDrawText3D(vector3D16 - (double) this.RotationForwardDirection * vector3D15 * 0.200000002980232 - (double) this.RotationRightDirection * vector3D13 * 0.00999999977648258, controlButtonName2, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
            MyRenderProxy.DebugDrawText3D(vector3D16 + (double) this.RotationForwardDirection * vector3D15 * 0.200000002980232 - (double) this.RotationRightDirection * vector3D13 * 0.00999999977648258, controlButtonName1, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
          }
        }
        else if (flag1)
        {
          Vector3 normal1;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis1, num7, num10, out normal1);
          Vector3D vector3D16 = (vector3D1 + vector3D2) * 0.5;
          Vector3D vector3D17 = Vector3D.TransformNormal(normal1, drawMatrix);
          Vector3 normal2;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis1, num10, num7, out normal2);
          Vector3D vector3D18 = (vector3D7 + vector3D8) * 0.5;
          Vector3D vector3D19 = Vector3D.TransformNormal(normal2, drawMatrix);
          bool flag7 = false;
          int edge1;
          if (num7 == 0 && num10 == 3)
            edge1 = num7 + 1;
          else if (num7 < num10 || num7 == 3 && num10 == 0)
          {
            edge1 = num7 - 1;
            flag7 = true;
          }
          else
            edge1 = num7 + 1;
          if (this.RotationRightDirection < 0)
            flag7 = !flag7;
          Vector3 normal3;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis1, num7, edge1, out normal3);
          Vector3D vector3D20 = Vector3D.TransformNormal(normal3, drawMatrix);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_GREEN, Vector4.One, vector3D16 + vector3D17 * 0.400000005960464 - vector3D20 * 0.00999999977648258, (Vector3) vector3D13, (Vector3) vector3D19, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_GREEN, Vector4.One, vector3D18 + vector3D19 * 0.400000005960464 - vector3D20 * 0.00999999977648258, (Vector3) vector3D13, (Vector3) vector3D17, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
          if (this.ShouldDrawText())
          {
            MyRenderProxy.DebugDrawText3D(vector3D16 + vector3D17 * 0.300000011920929 - vector3D20 * 0.00999999977648258, flag7 ? controlButtonName1 : controlButtonName2, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
            MyRenderProxy.DebugDrawText3D(vector3D18 + vector3D19 * 0.300000011920929 - vector3D20 * 0.00999999977648258, flag7 ? controlButtonName2 : controlButtonName1, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
          }
        }
        else
        {
          Vector3 normal1;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis1, num7, num7 + 1, out normal1);
          Vector3 normal2;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis1, num7, num7 - 1, out normal2);
          Vector3D vector3D16 = (vector3D1 + vector3D2) * 0.5;
          Vector3D vector3D17 = Vector3D.TransformNormal(normal1, drawMatrix);
          Vector3D vector3D18 = Vector3D.TransformNormal(normal2, drawMatrix);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_GREEN, Vector4.One, vector3D16 + vector3D17 * 0.300000011920929 - vector3D18 * 0.00999999977648258, (Vector3) vector3D13, (Vector3) vector3D17, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_GREEN, Vector4.One, vector3D16 + vector3D18 * 0.300000011920929 - vector3D17 * 0.00999999977648258, (Vector3) vector3D13, (Vector3) vector3D18, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
          if (this.ShouldDrawText())
          {
            MyRenderProxy.DebugDrawText3D(vector3D16 + vector3D17 * 0.300000011920929 - vector3D18 * 0.00999999977648258, this.RotationRightDirection < 0 ? controlButtonName1 : controlButtonName2, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
            MyRenderProxy.DebugDrawText3D(vector3D16 + vector3D18 * 0.300000011920929 - vector3D17 * 0.00999999977648258, this.RotationRightDirection < 0 ? controlButtonName2 : controlButtonName1, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
          }
        }
      }
      if (!hideForwardAndUpArrows || this.RotationUpAxis == 1)
      {
        if (flag5)
        {
          Vector3D vector3D16 = (vector3D5 + vector3D6 + vector3D11 + vector3D12) * 0.25;
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_LEFT_RED, Vector4.One, vector3D16 - (double) this.RotationRightDirection * vector3D13 * 0.200000002980232 - (double) this.RotationUpDirection * vector3D14 * 0.00999999977648258, (Vector3) ((double) -this.RotationForwardDirection * vector3D15), (Vector3) ((double) -this.RotationRightDirection * vector3D13), 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_RIGHT_RED, Vector4.One, vector3D16 + (double) this.RotationRightDirection * vector3D13 * 0.200000002980232 - (double) this.RotationUpDirection * vector3D14 * 0.00999999977648258, (Vector3) ((double) this.RotationForwardDirection * vector3D15), (Vector3) ((double) this.RotationRightDirection * vector3D13), 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
          if (this.ShouldDrawText())
          {
            MyRenderProxy.DebugDrawText3D(vector3D16 - (double) this.RotationRightDirection * vector3D13 * 0.200000002980232 - (double) this.RotationUpDirection * vector3D14 * 0.00999999977648258, controlButtonName3, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
            MyRenderProxy.DebugDrawText3D(vector3D16 + (double) this.RotationRightDirection * vector3D13 * 0.200000002980232 - (double) this.RotationUpDirection * vector3D14 * 0.00999999977648258, controlButtonName4, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
          }
        }
        else if (flag2)
        {
          Vector3 normal1;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num8, num11, out normal1);
          Vector3D vector3D16 = (vector3D3 + vector3D4) * 0.5;
          Vector3 upVector1 = Vector3.TransformNormal(normal1, drawMatrix);
          Vector3 normal2;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num11, num8, out normal2);
          Vector3D vector3D17 = (vector3D9 + vector3D10) * 0.5;
          Vector3 upVector2 = Vector3.TransformNormal(normal2, drawMatrix);
          bool flag7 = false;
          int edge1;
          if (num8 == 0 && num11 == 3)
            edge1 = num8 + 1;
          else if (num8 < num11 || num8 == 3 && num11 == 0)
          {
            edge1 = num8 - 1;
            flag7 = true;
          }
          else
            edge1 = num8 + 1;
          if (this.RotationUpDirection < 0)
            flag7 = !flag7;
          Vector3 normal3;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num8, edge1, out normal3);
          Vector3 vector3 = Vector3.TransformNormal(normal3, drawMatrix);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_RED, Vector4.One, vector3D16 + upVector1 * 0.4f - vector3 * 0.01f, (Vector3) vector3D14, upVector2, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_RED, Vector4.One, vector3D17 + upVector2 * 0.4f - vector3 * 0.01f, (Vector3) vector3D14, upVector1, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
          if (this.ShouldDrawText())
          {
            MyRenderProxy.DebugDrawText3D(vector3D16 + upVector1 * 0.3f - vector3 * 0.01f, flag7 ? controlButtonName4 : controlButtonName3, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
            MyRenderProxy.DebugDrawText3D(vector3D17 + upVector2 * 0.3f - vector3 * 0.01f, flag7 ? controlButtonName3 : controlButtonName4, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
          }
        }
        else
        {
          Vector3 normal1;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num8, num8 + 1, out normal1);
          Vector3 normal2;
          MyOrientedBoundingBox.GetNormalBetweenEdges(axis2, num8, num8 - 1, out normal2);
          Vector3D vector3D16 = (vector3D3 + vector3D4) * 0.5;
          Vector3 upVector1 = Vector3.TransformNormal(normal1, drawMatrix);
          Vector3 upVector2 = Vector3.TransformNormal(normal2, drawMatrix);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_RED, Vector4.One, vector3D16 + upVector1 * 0.3f - upVector2 * 0.01f, (Vector3) vector3D14, upVector1, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
          MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_RED, Vector4.One, vector3D16 + upVector2 * 0.3f - upVector1 * 0.01f, (Vector3) vector3D14, upVector2, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
          if (this.ShouldDrawText())
          {
            MyRenderProxy.DebugDrawText3D(vector3D16 + upVector1 * 0.6f - upVector2 * 0.01f, this.RotationUpDirection > 0 ? controlButtonName3 : controlButtonName4, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
            MyRenderProxy.DebugDrawText3D(vector3D16 + upVector2 * 0.6f - upVector1 * 0.01f, this.RotationUpDirection > 0 ? controlButtonName4 : controlButtonName3, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
          }
        }
      }
      if (hideForwardAndUpArrows && this.RotationForwardAxis != 1)
        return;
      if (flag6)
      {
        Vector3D vector3D16 = (vector3D1 + vector3D2 + vector3D7 + vector3D8) * 0.25;
        MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_LEFT_BLUE, Vector4.One, vector3D16 + (double) this.RotationUpDirection * vector3D14 * 0.200000002980232 - (double) this.RotationForwardDirection * vector3D15 * 0.00999999977648258, (Vector3) ((double) -this.RotationRightDirection * vector3D13), (Vector3) ((double) this.RotationUpDirection * vector3D14), 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
        MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_RIGHT_BLUE, Vector4.One, vector3D16 - (double) this.RotationUpDirection * vector3D14 * 0.200000002980232 - (double) this.RotationForwardDirection * vector3D15 * 0.00999999977648258, (Vector3) ((double) this.RotationRightDirection * vector3D13), (Vector3) ((double) -this.RotationUpDirection * vector3D14), 0.2f, num6, MyBillboard.BlendTypeEnum.LDR);
        if (!this.ShouldDrawText())
          return;
        MyRenderProxy.DebugDrawText3D(vector3D16 + (double) this.RotationUpDirection * vector3D14 * 0.200000002980232 - (double) this.RotationForwardDirection * vector3D15 * 0.00999999977648258, controlButtonName5, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, num6);
        MyRenderProxy.DebugDrawText3D(vector3D16 - (double) this.RotationUpDirection * vector3D14 * 0.200000002980232 - (double) this.RotationForwardDirection * vector3D15 * 0.00999999977648258, controlButtonName6, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, num6);
      }
      else if (flag3)
      {
        Vector3 normal1;
        MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num9, num12, out normal1);
        Vector3D vector3D16 = (vector3D5 + vector3D6) * 0.5;
        Vector3 upVector1 = Vector3.TransformNormal(normal1, drawMatrix);
        Vector3 normal2;
        MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num12, num9, out normal2);
        Vector3D vector3D17 = (vector3D11 + vector3D12) * 0.5;
        Vector3 upVector2 = Vector3.TransformNormal(normal2, drawMatrix);
        bool flag7 = false;
        int edge1;
        if (num9 == 0 && num12 == 3)
          edge1 = num9 + 1;
        else if (num9 < num12 || num9 == 3 && num12 == 0)
        {
          edge1 = num9 - 1;
          flag7 = true;
        }
        else
          edge1 = num9 + 1;
        if (this.RotationForwardDirection < 0)
          flag7 = !flag7;
        Vector3 normal3;
        MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num9, edge1, out normal3);
        Vector3 vector3 = Vector3.TransformNormal(normal3, drawMatrix);
        MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_BLUE, Vector4.One, vector3D16 + upVector1 * 0.4f - vector3 * 0.01f, (Vector3) vector3D15, upVector2, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
        MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_BLUE, Vector4.One, vector3D17 + upVector2 * 0.4f - vector3 * 0.01f, (Vector3) vector3D15, upVector1, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
        if (!this.ShouldDrawText())
          return;
        MyRenderProxy.DebugDrawText3D(vector3D16 + upVector1 * 0.3f - vector3 * 0.01f, flag7 ? controlButtonName5 : controlButtonName6, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
        MyRenderProxy.DebugDrawText3D(vector3D17 + upVector2 * 0.3f - vector3 * 0.01f, flag7 ? controlButtonName6 : controlButtonName5, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
      }
      else
      {
        Vector3 normal1;
        MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num9, num9 + 1, out normal1);
        Vector3 normal2;
        MyOrientedBoundingBox.GetNormalBetweenEdges(axis3, num9, num9 - 1, out normal2);
        Vector3D vector3D16 = (vector3D5 + vector3D6) * 0.5;
        Vector3 upVector1 = Vector3.TransformNormal(normal1, drawMatrix);
        Vector3 upVector2 = Vector3.TransformNormal(normal2, drawMatrix);
        MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_BLUE, Vector4.One, vector3D16 + upVector1 * 0.3f - upVector2 * 0.01f, (Vector3) vector3D15, upVector1, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
        MyTransparentGeometry.AddBillboardOriented(MyBlockBuilderRotationHints.ID_ARROW_BLUE, Vector4.One, vector3D16 + upVector2 * 0.3f - upVector1 * 0.01f, (Vector3) vector3D15, upVector2, 0.5f, num6, MyBillboard.BlendTypeEnum.LDR);
        if (!this.ShouldDrawText())
          return;
        MyRenderProxy.DebugDrawText3D(vector3D16 + upVector1 * 0.3f - upVector2 * 0.01f, this.RotationForwardDirection < 0 ? controlButtonName5 : controlButtonName6, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
        MyRenderProxy.DebugDrawText3D(vector3D16 + upVector2 * 0.3f - upVector1 * 0.01f, this.RotationForwardDirection < 0 ? controlButtonName6 : controlButtonName5, Color.White, scale, false, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, num6);
      }
    }

    private bool ShouldDrawText() => MyScreenManager.GetScreenWithFocus() is MyGuiScreenGamePlay;

    private struct BoxEdge
    {
      public int Axis;
      public LineD Edge;
    }
  }
}
