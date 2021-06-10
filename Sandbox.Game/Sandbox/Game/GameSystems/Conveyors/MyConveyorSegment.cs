// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.MyConveyorSegment
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using System;
using VRage.Game;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GameSystems.Conveyors
{
  public class MyConveyorSegment
  {
    public MyConveyorLine ConveyorLine { get; private set; }

    public ConveyorLinePosition ConnectingPosition1 { get; private set; }

    public ConveyorLinePosition ConnectingPosition2 { get; private set; }

    public MyCubeBlock CubeBlock { get; private set; }

    public bool IsCorner
    {
      get
      {
        Vector3I vectorDirection1 = this.ConnectingPosition1.VectorDirection;
        Vector3I vectorDirection2 = this.ConnectingPosition2.VectorDirection;
        return Vector3I.Dot(ref vectorDirection1, ref vectorDirection2) != -1;
      }
    }

    public void Init(
      MyCubeBlock myBlock,
      ConveyorLinePosition a,
      ConveyorLinePosition b,
      MyObjectBuilder_ConveyorLine.LineType type,
      MyObjectBuilder_ConveyorLine.LineConductivity conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL)
    {
      this.CubeBlock = myBlock;
      this.ConnectingPosition1 = a;
      this.ConnectingPosition2 = b;
      Vector3I neighbourGridPosition = (myBlock as IMyConveyorSegmentBlock).ConveyorSegment.ConnectingPosition1.NeighbourGridPosition;
      this.ConveyorLine = myBlock.CubeGrid.GridSystems.ConveyorSystem.GetDeserializingLine(neighbourGridPosition);
      if (this.ConveyorLine == null)
      {
        this.ConveyorLine = new MyConveyorLine();
        if (this.IsCorner)
          this.ConveyorLine.Init(a, b, myBlock.CubeGrid, type, conductivity, new Vector3I?(this.CalculateCornerPosition()));
        else
          this.ConveyorLine.Init(a, b, myBlock.CubeGrid, type, conductivity);
      }
      myBlock.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.CubeBlock_IsFunctionalChanged);
    }

    public void SetConveyorLine(MyConveyorLine newLine) => this.ConveyorLine = newLine;

    public bool CanConnectTo(
      ConveyorLinePosition connectingPosition,
      MyObjectBuilder_ConveyorLine.LineType type)
    {
      if (type == this.ConveyorLine.Type)
      {
        ref ConveyorLinePosition local1 = ref connectingPosition;
        ConveyorLinePosition conveyorLinePosition = this.ConnectingPosition1;
        ConveyorLinePosition connectingPosition1 = conveyorLinePosition.GetConnectingPosition();
        if (!local1.Equals(connectingPosition1))
        {
          ref ConveyorLinePosition local2 = ref connectingPosition;
          conveyorLinePosition = this.ConnectingPosition2;
          ConveyorLinePosition connectingPosition2 = conveyorLinePosition.GetConnectingPosition();
          if (!local2.Equals(connectingPosition2))
            goto label_4;
        }
        return true;
      }
label_4:
      return false;
    }

    private void CubeBlock_IsFunctionalChanged() => this.ConveyorLine.UpdateIsFunctional();

    public Base6Directions.Direction CalculateConnectingDirection(
      Vector3I connectingPosition)
    {
      Vector3 vector3 = new Vector3(this.CubeBlock.Max - this.CubeBlock.Min + Vector3I.One) * 0.5f;
      Vector3 vec = Vector3.DominantAxisProjection(Vector3.Multiply(new Vector3(this.CubeBlock.Max + this.CubeBlock.Min) * 0.5f - (Vector3) connectingPosition, vector3));
      double num = (double) vec.Normalize();
      return Base6Directions.GetDirection(vec);
    }

    private Vector3I CalculateCornerPosition()
    {
      Vector3I vector3I = this.ConnectingPosition2.LocalGridPosition - this.ConnectingPosition1.LocalGridPosition;
      switch (Base6Directions.GetAxis(this.ConnectingPosition1.Direction))
      {
        case Base6Directions.Axis.ForwardBackward:
          return this.ConnectingPosition1.LocalGridPosition + new Vector3I(0, 0, vector3I.Z);
        case Base6Directions.Axis.LeftRight:
          return this.ConnectingPosition1.LocalGridPosition + new Vector3I(vector3I.X, 0, 0);
        case Base6Directions.Axis.UpDown:
          return this.ConnectingPosition1.LocalGridPosition + new Vector3I(0, vector3I.Y, 0);
        default:
          return Vector3I.Zero;
      }
    }

    public void DebugDraw()
    {
      if (!MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS)
        return;
      Vector3 position1 = ((Vector3) this.ConnectingPosition1.LocalGridPosition + this.ConnectingPosition1.VectorDirection * 0.5f) * this.CubeBlock.CubeGrid.GridSize;
      Vector3 position2 = ((Vector3) this.ConnectingPosition2.LocalGridPosition + this.ConnectingPosition2.VectorDirection * 0.5f) * this.CubeBlock.CubeGrid.GridSize;
      Vector3 vector3_1 = (Vector3) Vector3.Transform(position1, this.CubeBlock.CubeGrid.WorldMatrix);
      Vector3 vector3_2 = (Vector3) Vector3.Transform(position2, this.CubeBlock.CubeGrid.WorldMatrix);
      Color color1 = this.ConveyorLine.IsFunctional ? Color.Orange : Color.DarkRed;
      Color color2 = this.ConveyorLine.IsWorking ? Color.GreenYellow : color1;
      MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_1, (Vector3D) vector3_2, color2, color2, false);
      if (this.ConveyorLine == null || !MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS_LINE_IDS)
        return;
      MyRenderProxy.DebugDrawText3D((Vector3D) ((vector3_1 + vector3_2) * 0.5f), this.ConveyorLine.GetHashCode().ToString(), color2, 0.5f, false);
    }
  }
}
