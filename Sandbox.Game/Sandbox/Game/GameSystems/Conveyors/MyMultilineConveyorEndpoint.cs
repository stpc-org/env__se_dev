// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.MyMultilineConveyorEndpoint
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Algorithms;
using VRage.Game;
using VRage.Game.Models;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.GameSystems.Conveyors
{
  public class MyMultilineConveyorEndpoint : IMyConveyorEndpoint, IMyPathVertex<IMyConveyorEndpoint>, IEnumerable<IMyPathEdge<IMyConveyorEndpoint>>, IEnumerable
  {
    protected MyConveyorLine[] m_conveyorLines;
    protected static Dictionary<MyDefinitionId, ConveyorLinePosition[]> m_linePositions = new Dictionary<MyDefinitionId, ConveyorLinePosition[]>();
    private MyCubeBlock m_block;
    private MyPathfindingData m_pathfindingData;
    private ulong m_lastLineUpdateRequested;

    public MyCubeBlock CubeBlock => this.m_block;

    MyPathfindingData IMyPathVertex<IMyConveyorEndpoint>.PathfindingData => this.m_pathfindingData;

    public MyMultilineConveyorEndpoint(MyCubeBlock myBlock)
    {
      this.m_block = myBlock;
      MyConveyorLine.BlockLinePositionInformation[] blockLinePositions = MyConveyorLine.GetBlockLinePositions(myBlock);
      this.m_conveyorLines = new MyConveyorLine[blockLinePositions.Length];
      MyGridConveyorSystem conveyorSystem = myBlock.CubeGrid.GridSystems.ConveyorSystem;
      int index = 0;
      foreach (MyConveyorLine.BlockLinePositionInformation positionInformation in blockLinePositions)
      {
        ConveyorLinePosition gridCoords = this.PositionToGridCoords(positionInformation.Position);
        MyConveyorLine myConveyorLine = conveyorSystem.GetDeserializingLine(gridCoords);
        if (myConveyorLine == null)
        {
          myConveyorLine = new MyConveyorLine();
          myConveyorLine.Init(gridCoords, gridCoords.GetConnectingPosition(), myBlock.CubeGrid, positionInformation.LineType, positionInformation.LineConductivity);
          myConveyorLine.InitEndpoints((IMyConveyorEndpoint) this, (IMyConveyorEndpoint) null);
        }
        else
        {
          ConveyorLinePosition endpointPosition = myConveyorLine.GetEndpointPosition(0);
          if (endpointPosition.Equals(gridCoords))
          {
            myConveyorLine.SetEndpoint(0, (IMyConveyorEndpoint) this);
          }
          else
          {
            endpointPosition = myConveyorLine.GetEndpointPosition(1);
            if (endpointPosition.Equals(gridCoords))
              myConveyorLine.SetEndpoint(1, (IMyConveyorEndpoint) this);
          }
        }
        this.m_conveyorLines[index] = myConveyorLine;
        ++index;
      }
      myBlock.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.UpdateLineFunctionality);
      myBlock.CubeGrid.GridSystems.ConveyorSystem.ResourceSink.IsPoweredChanged += new Action(this.UpdateLineFunctionality);
      this.m_pathfindingData = new MyPathfindingData((object) this);
    }

    public ConveyorLinePosition PositionToGridCoords(
      ConveyorLinePosition position)
    {
      return MyMultilineConveyorEndpoint.PositionToGridCoords(position, this.CubeBlock);
    }

    public static ConveyorLinePosition PositionToGridCoords(
      ConveyorLinePosition position,
      MyCubeBlock cubeBlock)
    {
      ConveyorLinePosition conveyorLinePosition = new ConveyorLinePosition();
      Matrix result = new Matrix();
      cubeBlock.Orientation.GetMatrix(out result);
      Vector3 vector3 = Vector3.Transform(new Vector3(position.LocalGridPosition), result);
      conveyorLinePosition.LocalGridPosition = Vector3I.Round(vector3) + cubeBlock.Position;
      conveyorLinePosition.Direction = cubeBlock.Orientation.TransformDirection(position.Direction);
      return conveyorLinePosition;
    }

    public MyConveyorLine GetConveyorLine(ConveyorLinePosition position)
    {
      ConveyorLinePosition[] linePositions = this.GetLinePositions();
      for (int index = 0; index < linePositions.Length; ++index)
      {
        if (this.PositionToGridCoords(linePositions[index]).Equals(position))
          return this.m_conveyorLines[index];
      }
      return (MyConveyorLine) null;
    }

    public ConveyorLinePosition GetPosition(int index) => this.PositionToGridCoords(this.GetLinePositions()[index]);

    public MyConveyorLine GetConveyorLine(int index)
    {
      if (index >= this.m_conveyorLines.Length)
        throw new IndexOutOfRangeException();
      return this.m_conveyorLines[index];
    }

    public void SetConveyorLine(ConveyorLinePosition position, MyConveyorLine newLine)
    {
      ConveyorLinePosition[] linePositions = this.GetLinePositions();
      for (int index = 0; index < linePositions.Length; ++index)
      {
        if (this.PositionToGridCoords(linePositions[index]).Equals(position))
        {
          this.m_conveyorLines[index] = newLine;
          break;
        }
      }
    }

    public int GetLineCount() => this.m_conveyorLines.Length;

    protected ConveyorLinePosition[] GetLinePositions()
    {
      ConveyorLinePosition[] conveyorLinePositionArray = (ConveyorLinePosition[]) null;
      lock (MyMultilineConveyorEndpoint.m_linePositions)
      {
        if (!MyMultilineConveyorEndpoint.m_linePositions.TryGetValue(this.CubeBlock.BlockDefinition.Id, out conveyorLinePositionArray))
        {
          conveyorLinePositionArray = MyMultilineConveyorEndpoint.GetLinePositions(this.CubeBlock, "detector_conveyor");
          MyMultilineConveyorEndpoint.m_linePositions.Add(this.CubeBlock.BlockDefinition.Id, conveyorLinePositionArray);
        }
      }
      return conveyorLinePositionArray;
    }

    public static ConveyorLinePosition[] GetLinePositions(
      MyCubeBlock cubeBlock,
      string dummyName)
    {
      return MyMultilineConveyorEndpoint.GetLinePositions(cubeBlock, (IDictionary<string, MyModelDummy>) MyModels.GetModelOnlyDummies(cubeBlock.BlockDefinition.Model).Dummies, dummyName);
    }

    public static ConveyorLinePosition[] GetLinePositions(
      MyCubeBlock cubeBlock,
      IDictionary<string, MyModelDummy> dummies,
      string dummyName)
    {
      MyCubeBlockDefinition blockDefinition = cubeBlock.BlockDefinition;
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize);
      Vector3 vector3_1 = new Vector3(blockDefinition.Size) * 0.5f * cubeSize;
      int length = 0;
      foreach (KeyValuePair<string, MyModelDummy> dummy in (IEnumerable<KeyValuePair<string, MyModelDummy>>) dummies)
      {
        if (dummy.Key.ToLower().Contains(dummyName))
          ++length;
      }
      ConveyorLinePosition[] conveyorLinePositionArray = new ConveyorLinePosition[length];
      int index = 0;
      foreach (KeyValuePair<string, MyModelDummy> dummy in (IEnumerable<KeyValuePair<string, MyModelDummy>>) dummies)
      {
        if (dummy.Key.ToLower().Contains(dummyName))
        {
          Matrix matrix = dummy.Value.Matrix;
          ConveyorLinePosition conveyorLinePosition = new ConveyorLinePosition();
          Vector3 vector3_2 = matrix.Translation + blockDefinition.ModelOffset + vector3_1;
          Vector3I vector3I1 = Vector3I.Floor(vector3_2 / cubeSize);
          Vector3I vector3I2 = Vector3I.Max(Vector3I.Zero, vector3I1);
          Vector3I vector3I3 = Vector3I.Min(blockDefinition.Size - Vector3I.One, vector3I2);
          Vector3 vec = Vector3.Normalize(Vector3.DominantAxisProjection(vector3_2 - (new Vector3(vector3I3) + Vector3.Half) * cubeSize));
          conveyorLinePosition.LocalGridPosition = vector3I3 - blockDefinition.Center;
          conveyorLinePosition.Direction = Base6Directions.GetDirection(vec);
          conveyorLinePositionArray[index] = conveyorLinePosition;
          ++index;
        }
      }
      return conveyorLinePositionArray;
    }

    protected void UpdateLineFunctionality()
    {
      if ((long) MySandboxGame.Static.SimulationFrameCounter == (long) this.m_lastLineUpdateRequested && this.m_lastLineUpdateRequested != 0UL)
        return;
      this.m_lastLineUpdateRequested = MySandboxGame.Static.SimulationFrameCounter;
      MySandboxGame.Static.Invoke((Action) (() =>
      {
        for (int index = 0; index < this.m_conveyorLines.Length; ++index)
          this.m_conveyorLines[index].UpdateIsFunctional();
      }), "MyMultilineConveyorEndpoint::UpdateLineFunctionality");
    }

    public ConveyorLineEnumerator GetEnumeratorInternal() => new ConveyorLineEnumerator((IMyConveyorEndpoint) this);

    IEnumerator<IMyPathEdge<IMyConveyorEndpoint>> IEnumerable<IMyPathEdge<IMyConveyorEndpoint>>.GetEnumerator() => (IEnumerator<IMyPathEdge<IMyConveyorEndpoint>>) this.GetEnumeratorInternal();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumeratorInternal();

    float IMyPathVertex<IMyConveyorEndpoint>.EstimateDistanceTo(
      IMyPathVertex<IMyConveyorEndpoint> other)
    {
      MatrixD worldMatrix = (other as IMyConveyorEndpoint).CubeBlock.WorldMatrix;
      Vector3 translation1 = (Vector3) worldMatrix.Translation;
      worldMatrix = this.CubeBlock.WorldMatrix;
      Vector3 translation2 = (Vector3) worldMatrix.Translation;
      return Vector3.RectangularDistance(translation1, translation2);
    }

    int IMyPathVertex<IMyConveyorEndpoint>.GetNeighborCount() => this.GetNeighborCount();

    protected virtual int GetNeighborCount() => this.m_conveyorLines.Length;

    IMyPathVertex<IMyConveyorEndpoint> IMyPathVertex<IMyConveyorEndpoint>.GetNeighbor(
      int index)
    {
      return this.GetNeighbor(index);
    }

    protected virtual IMyPathVertex<IMyConveyorEndpoint> GetNeighbor(
      int index)
    {
      return (IMyPathVertex<IMyConveyorEndpoint>) this.m_conveyorLines[index].GetOtherVertex((IMyConveyorEndpoint) this);
    }

    IMyPathEdge<IMyConveyorEndpoint> IMyPathVertex<IMyConveyorEndpoint>.GetEdge(
      int index)
    {
      return this.GetEdge(index);
    }

    protected virtual IMyPathEdge<IMyConveyorEndpoint> GetEdge(
      int index)
    {
      return (IMyPathEdge<IMyConveyorEndpoint>) this.m_conveyorLines[index];
    }

    public void DebugDraw()
    {
    }
  }
}
