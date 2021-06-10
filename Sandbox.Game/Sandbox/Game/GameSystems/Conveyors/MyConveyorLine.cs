// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.MyConveyorLine
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.World;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VRage;
using VRage.Algorithms;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Import;

namespace Sandbox.Game.GameSystems.Conveyors
{
  public class MyConveyorLine : IEnumerable<Vector3I>, IEnumerable, IMyPathEdge<IMyConveyorEndpoint>
  {
    private static ConcurrentDictionary<MyDefinitionId, MyConveyorLine.BlockLinePositionInformation[]> m_blockLinePositions = new ConcurrentDictionary<MyDefinitionId, MyConveyorLine.BlockLinePositionInformation[]>();
    private static readonly float CONVEYOR_PER_LINE_PENALTY = 1f;
    private const int FRAMES_PER_BIG_UPDATE = 64;
    private const float BIG_UPDATE_FRACTION = 0.015625f;
    private ConveyorLinePosition m_endpointPosition1;
    private ConveyorLinePosition m_endpointPosition2;
    private IMyConveyorEndpoint m_endpoint1;
    private IMyConveyorEndpoint m_endpoint2;
    private MyObjectBuilder_ConveyorLine.LineType m_type;
    private MyObjectBuilder_ConveyorLine.LineConductivity m_conductivity;
    private int m_length;
    private MyCubeGrid m_cubeGrid;
    [ThreadStatic]
    private static bool m_invertedConductivity = false;
    private MySinglyLinkedList<MyConveyorPacket> m_queue1;
    private MySinglyLinkedList<MyConveyorPacket> m_queue2;
    private List<MyConveyorLine.SectionInformation> m_sections;
    private static List<MyConveyorLine.SectionInformation> m_tmpSections1 = new List<MyConveyorLine.SectionInformation>();
    private static List<MyConveyorLine.SectionInformation> m_tmpSections2 = new List<MyConveyorLine.SectionInformation>();
    private bool m_stopped1;
    private bool m_stopped2;
    private float m_queuePosition;
    private bool m_isFunctional;
    private bool m_isWorking;
    private MyConveyorLine.LinePositionEnumerator m_enumerator;

    public bool IsFunctional => this.m_isFunctional;

    public bool IsWorking => this.m_isWorking;

    public int Length => this.m_length;

    public bool IsDegenerate => this.Length == 1 && this.HasNullEndpoints;

    public bool IsCircular => this.Length != 1 && this.m_endpointPosition1.GetConnectingPosition().Equals(this.m_endpointPosition2);

    public bool HasNullEndpoints => this.m_endpoint1 == null && this.m_endpoint2 == null;

    public bool IsDisconnected => this.m_endpoint1 == null || this.m_endpoint2 == null;

    public bool IsEmpty => this.m_queue1.Count == 0 && this.m_queue2.Count == 0;

    public MyObjectBuilder_ConveyorLine.LineType Type => this.m_type;

    public MyObjectBuilder_ConveyorLine.LineConductivity Conductivity => this.m_conductivity;

    public MyConveyorLine()
    {
      this.m_queue1 = new MySinglyLinkedList<MyConveyorPacket>();
      this.m_queue2 = new MySinglyLinkedList<MyConveyorPacket>();
      this.m_length = 0;
      this.m_queuePosition = 0.0f;
      this.m_stopped1 = false;
      this.m_stopped2 = false;
      this.m_sections = (List<MyConveyorLine.SectionInformation>) null;
      this.m_isFunctional = false;
      this.m_isWorking = false;
    }

    public MyObjectBuilder_ConveyorLine GetObjectBuilder()
    {
      MyObjectBuilder_ConveyorLine builderConveyorLine = new MyObjectBuilder_ConveyorLine();
      foreach (MyConveyorPacket myConveyorPacket in this.m_queue1)
        builderConveyorLine.PacketsForward.Add(new MyObjectBuilder_ConveyorPacket()
        {
          Item = myConveyorPacket.Item.GetObjectBuilder(),
          LinePosition = myConveyorPacket.LinePosition
        });
      foreach (MyConveyorPacket myConveyorPacket in this.m_queue2)
        builderConveyorLine.PacketsBackward.Add(new MyObjectBuilder_ConveyorPacket()
        {
          Item = myConveyorPacket.Item.GetObjectBuilder(),
          LinePosition = myConveyorPacket.LinePosition
        });
      builderConveyorLine.StartPosition = (SerializableVector3I) this.m_endpointPosition1.LocalGridPosition;
      builderConveyorLine.StartDirection = this.m_endpointPosition1.Direction;
      builderConveyorLine.EndPosition = (SerializableVector3I) this.m_endpointPosition2.LocalGridPosition;
      builderConveyorLine.EndDirection = this.m_endpointPosition2.Direction;
      if (this.m_sections != null)
      {
        builderConveyorLine.Sections = new List<SerializableLineSectionInformation>(this.m_sections.Count);
        foreach (MyConveyorLine.SectionInformation section in this.m_sections)
          builderConveyorLine.Sections.Add(new SerializableLineSectionInformation()
          {
            Direction = section.Direction,
            Length = section.Length
          });
      }
      builderConveyorLine.ConveyorLineType = this.m_type;
      builderConveyorLine.ConveyorLineConductivity = this.m_conductivity;
      return builderConveyorLine;
    }

    private void InitializeSectionList(int size = -1)
    {
      if (this.m_sections != null)
      {
        this.m_sections.Clear();
        if (size == -1)
          return;
        this.m_sections.Capacity = size;
      }
      else if (size != -1)
        this.m_sections = new List<MyConveyorLine.SectionInformation>(size);
      else
        this.m_sections = new List<MyConveyorLine.SectionInformation>();
    }

    public void Init(MyObjectBuilder_ConveyorLine objectBuilder, MyCubeGrid cubeGrid)
    {
      this.m_cubeGrid = cubeGrid;
      foreach (MyObjectBuilder_ConveyorPacket builder in objectBuilder.PacketsForward)
      {
        MyConveyorPacket myConveyorPacket = new MyConveyorPacket();
        myConveyorPacket.Init(builder, (MyEntity) this.m_cubeGrid);
        this.m_queue1.Append(myConveyorPacket);
      }
      foreach (MyObjectBuilder_ConveyorPacket builder in objectBuilder.PacketsBackward)
      {
        MyConveyorPacket myConveyorPacket = new MyConveyorPacket();
        myConveyorPacket.Init(builder, (MyEntity) this.m_cubeGrid);
        this.m_queue2.Append(myConveyorPacket);
      }
      this.m_endpointPosition1 = new ConveyorLinePosition((Vector3I) objectBuilder.StartPosition, objectBuilder.StartDirection);
      this.m_endpointPosition2 = new ConveyorLinePosition((Vector3I) objectBuilder.EndPosition, objectBuilder.EndDirection);
      this.m_length = 0;
      if (objectBuilder.Sections != null && objectBuilder.Sections.Count != 0)
      {
        this.InitializeSectionList(objectBuilder.Sections.Count);
        foreach (SerializableLineSectionInformation section in objectBuilder.Sections)
        {
          MyConveyorLine.SectionInformation sectionInformation = new MyConveyorLine.SectionInformation();
          sectionInformation.Direction = section.Direction;
          sectionInformation.Length = section.Length;
          this.m_sections.Add(sectionInformation);
          this.m_length += sectionInformation.Length;
        }
      }
      if (this.m_length == 0)
        this.m_length = this.m_endpointPosition2.LocalGridPosition.RectangularDistance(this.m_endpointPosition1.LocalGridPosition);
      this.m_type = objectBuilder.ConveyorLineType;
      if (this.m_type == MyObjectBuilder_ConveyorLine.LineType.DEFAULT_LINE)
      {
        if (cubeGrid.GridSizeEnum == MyCubeSize.Small)
          this.m_type = MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE;
        else if (cubeGrid.GridSizeEnum == MyCubeSize.Large)
          this.m_type = MyObjectBuilder_ConveyorLine.LineType.LARGE_LINE;
      }
      this.m_conductivity = objectBuilder.ConveyorLineConductivity;
      this.StopQueuesIfNeeded();
      this.RecalculatePacketPositions();
    }

    public void Init(
      ConveyorLinePosition endpoint1,
      ConveyorLinePosition endpoint2,
      MyCubeGrid cubeGrid,
      MyObjectBuilder_ConveyorLine.LineType type,
      MyObjectBuilder_ConveyorLine.LineConductivity conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL,
      Vector3I? corner = null)
    {
      this.m_cubeGrid = cubeGrid;
      this.m_type = type;
      this.m_conductivity = conductivity;
      this.m_endpointPosition1 = endpoint1;
      this.m_endpointPosition2 = endpoint2;
      this.m_isFunctional = false;
      if (corner.HasValue)
      {
        this.InitializeSectionList(2);
        Vector3I vector3I1 = corner.Value - endpoint1.LocalGridPosition;
        int num1 = vector3I1.RectangularLength();
        Vector3I vec1 = vector3I1 / num1;
        Vector3I vector3I2 = endpoint2.LocalGridPosition - corner.Value;
        int num2 = vector3I2.RectangularLength();
        Vector3I vec2 = vector3I2 / num2;
        Base6Directions.Direction direction1 = Base6Directions.GetDirection(vec1);
        Base6Directions.Direction direction2 = Base6Directions.GetDirection(vec2);
        MyConveyorLine.SectionInformation sectionInformation1 = new MyConveyorLine.SectionInformation()
        {
          Direction = direction1,
          Length = num1
        };
        MyConveyorLine.SectionInformation sectionInformation2 = new MyConveyorLine.SectionInformation()
        {
          Direction = direction2,
          Length = num2
        };
        this.m_sections.Add(sectionInformation1);
        this.m_sections.Add(sectionInformation2);
      }
      this.m_length = endpoint1.LocalGridPosition.RectangularDistance(endpoint2.LocalGridPosition);
    }

    private void InitAfterSplit(
      ConveyorLinePosition endpoint1,
      ConveyorLinePosition endpoint2,
      List<MyConveyorLine.SectionInformation> sections,
      int newLength,
      MyCubeGrid cubeGrid,
      MyObjectBuilder_ConveyorLine.LineType lineType)
    {
      this.m_endpointPosition1 = endpoint1;
      this.m_endpointPosition2 = endpoint2;
      this.m_sections = sections;
      this.m_length = newLength;
      this.m_cubeGrid = cubeGrid;
      this.m_type = lineType;
    }

    public void InitEndpoints(IMyConveyorEndpoint endpoint1, IMyConveyorEndpoint endpoint2)
    {
      this.m_endpoint1 = endpoint1;
      this.m_endpoint2 = endpoint2;
      this.UpdateIsFunctional();
    }

    private void RecalculatePacketPositions()
    {
      int sectionStartPosition1 = 0;
      Vector3I sectionStart1 = this.m_endpointPosition1.LocalGridPosition;
      Base6Directions.Direction direction1 = this.m_endpointPosition1.Direction;
      int index1 = 0;
      int length1 = this.Length;
      if (this.m_sections != null)
      {
        index1 = this.m_sections.Count - 1;
        sectionStartPosition1 = this.Length - this.m_sections[index1].Length;
        sectionStart1 = this.m_endpointPosition2.LocalGridPosition - Base6Directions.GetIntVector(direction1) * this.m_sections[index1].Length;
        direction1 = this.m_sections[index1].Direction;
        length1 = this.m_sections[index1].Length;
      }
      Base6Directions.Direction perpendicular = Base6Directions.GetPerpendicular(direction1);
      MySinglyLinkedList<MyConveyorPacket>.Enumerator enumerator1 = this.m_queue1.GetEnumerator();
      bool flag1 = enumerator1.MoveNext();
      while (sectionStartPosition1 >= 0)
      {
        for (; flag1 && enumerator1.Current.LinePosition >= sectionStartPosition1; flag1 = enumerator1.MoveNext())
        {
          enumerator1.Current.SetLocalPosition(sectionStart1, sectionStartPosition1, this.m_cubeGrid.GridSize, direction1, perpendicular);
          enumerator1.Current.SetSegmentLength(this.m_cubeGrid.GridSize);
        }
        if (this.m_sections != null && flag1)
        {
          --index1;
          if (index1 >= 0)
          {
            direction1 = this.m_sections[index1].Direction;
            int length2 = this.m_sections[index1].Length;
            sectionStartPosition1 -= length2;
            sectionStart1 -= Base6Directions.GetIntVector(direction1) * length2;
          }
          else
            break;
        }
        else
          break;
      }
      int sectionStartPosition2 = 0;
      Vector3I sectionStart2 = this.m_endpointPosition2.LocalGridPosition;
      Base6Directions.Direction direction2 = this.m_endpointPosition2.Direction;
      Base6Directions.Direction flippedDirection = Base6Directions.GetFlippedDirection(perpendicular);
      int index2 = 0;
      length1 = this.Length;
      if (this.m_sections != null)
      {
        int length2 = this.m_sections[index2].Length;
        sectionStartPosition2 = this.Length - length2;
        direction2 = Base6Directions.GetFlippedDirection(this.m_sections[index2].Direction);
        sectionStart2 = this.m_endpointPosition1.LocalGridPosition - Base6Directions.GetIntVector(direction2) * length2;
      }
      MySinglyLinkedList<MyConveyorPacket>.Enumerator enumerator2 = this.m_queue2.GetEnumerator();
      bool flag2 = enumerator2.MoveNext();
      while (sectionStartPosition2 >= 0)
      {
        for (; flag2 && enumerator2.Current.LinePosition >= sectionStartPosition2; flag2 = enumerator2.MoveNext())
        {
          enumerator2.Current.SetLocalPosition(sectionStart2, sectionStartPosition2, this.m_cubeGrid.GridSize, direction2, flippedDirection);
          enumerator2.Current.SetSegmentLength(this.m_cubeGrid.GridSize);
        }
        if (this.m_sections == null || !flag2)
          break;
        ++index2;
        if (index2 >= this.m_sections.Count)
          break;
        int length2 = this.m_sections[index2].Length;
        sectionStartPosition2 -= length2;
        direction2 = Base6Directions.GetFlippedDirection(this.m_sections[index2].Direction);
        sectionStart2 -= Base6Directions.GetIntVector(direction2) * length2;
      }
    }

    public IMyConveyorEndpoint GetEndpoint(int index)
    {
      if (index == 0)
        return this.m_endpoint1;
      if (index == 1)
        return this.m_endpoint2;
      throw new IndexOutOfRangeException();
    }

    public void SetEndpoint(int index, IMyConveyorEndpoint endpoint)
    {
      if (index == 0)
      {
        this.m_endpoint1 = endpoint;
      }
      else
      {
        if (index != 1)
          throw new IndexOutOfRangeException();
        this.m_endpoint2 = endpoint;
      }
    }

    public ConveyorLinePosition GetEndpointPosition(int index)
    {
      if (index == 0)
        return this.m_endpointPosition1;
      if (index == 1)
        return this.m_endpointPosition2;
      throw new IndexOutOfRangeException();
    }

    public static MyConveyorLine.BlockLinePositionInformation[] GetBlockLinePositions(
      MyCubeBlock block)
    {
      MyConveyorLine.BlockLinePositionInformation[] positionInformationArray1;
      if (MyConveyorLine.m_blockLinePositions.TryGetValue(block.BlockDefinition.Id, out positionInformationArray1))
        return positionInformationArray1;
      MyCubeBlockDefinition blockDefinition = block.BlockDefinition;
      float cubeSize = MyDefinitionManager.Static.GetCubeSize(blockDefinition.CubeSize);
      Vector3 vector3_1 = new Vector3(blockDefinition.Size) * 0.5f * cubeSize;
      MyModel modelOnlyDummies = MyModels.GetModelOnlyDummies(block.BlockDefinition.Model);
      int length = 0;
      foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
      {
        string[] strArray = dummy.Key.ToLower().Split('_');
        if (strArray.Length >= 2 && strArray[0] == "detector" && strArray[1].StartsWith("conveyor"))
          ++length;
      }
      MyConveyorLine.BlockLinePositionInformation[] positionInformationArray2 = new MyConveyorLine.BlockLinePositionInformation[length];
      int index = 0;
      foreach (KeyValuePair<string, MyModelDummy> dummy in modelOnlyDummies.Dummies)
      {
        string[] strArray = dummy.Key.ToLower().Split('_');
        if (strArray.Length >= 2 && !(strArray[0] != "detector") && strArray[1].StartsWith("conveyor"))
        {
          int num = strArray.Length <= 2 ? 0 : (strArray[2] == "small" ? 1 : 0);
          positionInformationArray2[index].LineType = num == 0 ? MyObjectBuilder_ConveyorLine.LineType.LARGE_LINE : MyObjectBuilder_ConveyorLine.LineType.SMALL_LINE;
          positionInformationArray2[index].LineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
          if ((strArray.Length <= 2 || !(strArray[2] == "in") ? (strArray.Length <= 3 ? 0 : (strArray[3] == "in" ? 1 : 0)) : 1) != 0)
            positionInformationArray2[index].LineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
          if ((strArray.Length <= 2 || !(strArray[2] == "out") ? (strArray.Length <= 3 ? 0 : (strArray[3] == "out" ? 1 : 0)) : 1) != 0)
            positionInformationArray2[index].LineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
          Matrix matrix = dummy.Value.Matrix;
          ConveyorLinePosition conveyorLinePosition = new ConveyorLinePosition();
          Vector3 vector3_2 = matrix.Translation + blockDefinition.ModelOffset + vector3_1;
          Vector3I vector3I1 = Vector3I.Floor(vector3_2 / cubeSize);
          Vector3I vector3I2 = Vector3I.Max(Vector3I.Zero, vector3I1);
          Vector3I vector3I3 = Vector3I.Min(blockDefinition.Size - Vector3I.One, vector3I2);
          Vector3 vec = Vector3.Normalize(Vector3.DominantAxisProjection(Vector3.Divide(vector3_2 - (new Vector3(vector3I3) + Vector3.Half) * cubeSize, cubeSize)));
          conveyorLinePosition.LocalGridPosition = vector3I3 - blockDefinition.Center;
          conveyorLinePosition.Direction = Base6Directions.GetDirection(vec);
          positionInformationArray2[index].Position = conveyorLinePosition;
          ++index;
        }
      }
      MyConveyorLine.m_blockLinePositions.TryAdd(blockDefinition.Id, positionInformationArray2);
      return positionInformationArray2;
    }

    public void RecalculateConductivity()
    {
      this.m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
      MyObjectBuilder_ConveyorLine.LineConductivity lineConductivity1 = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
      MyObjectBuilder_ConveyorLine.LineConductivity lineConductivity2 = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
      ConveyorLinePosition gridCoords;
      if (this.m_endpoint1 != null && this.m_endpoint1 is MyMultilineConveyorEndpoint)
      {
        MyMultilineConveyorEndpoint endpoint1 = this.m_endpoint1 as MyMultilineConveyorEndpoint;
        foreach (MyConveyorLine.BlockLinePositionInformation blockLinePosition in MyConveyorLine.GetBlockLinePositions(endpoint1.CubeBlock))
        {
          gridCoords = endpoint1.PositionToGridCoords(blockLinePosition.Position);
          if (gridCoords.Equals(this.m_endpointPosition1))
          {
            lineConductivity1 = blockLinePosition.LineConductivity;
            break;
          }
        }
      }
      if (this.m_endpoint2 != null && this.m_endpoint2 is MyMultilineConveyorEndpoint)
      {
        MyMultilineConveyorEndpoint endpoint2 = this.m_endpoint2 as MyMultilineConveyorEndpoint;
        foreach (MyConveyorLine.BlockLinePositionInformation blockLinePosition in MyConveyorLine.GetBlockLinePositions(endpoint2.CubeBlock))
        {
          gridCoords = endpoint2.PositionToGridCoords(blockLinePosition.Position);
          if (gridCoords.Equals(this.m_endpointPosition2))
          {
            lineConductivity2 = blockLinePosition.LineConductivity;
            switch (lineConductivity2)
            {
              case MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD:
                lineConductivity2 = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
                goto label_14;
              case MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD:
                lineConductivity2 = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
                goto label_14;
              default:
                goto label_14;
            }
          }
        }
      }
label_14:
      if (lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.NONE || lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.NONE || lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD || lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD)
        this.m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.NONE;
      else if (lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.FULL && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD || lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FULL || lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD)
        this.m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
      else if (lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.FULL && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD || lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.FULL || lineConductivity1 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD && lineConductivity2 == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD)
        this.m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
      else
        this.m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FULL;
    }

    public void Merge(MyConveyorLine mergingLine, IMyConveyorSegmentBlock newlyAddedBlock = null)
    {
      ConveyorLinePosition connectingPosition1 = this.m_endpointPosition2.GetConnectingPosition();
      if (mergingLine.m_endpointPosition1.Equals(connectingPosition1))
        this.MergeInternal(mergingLine, newlyAddedBlock);
      else if (mergingLine.m_endpointPosition2.Equals(connectingPosition1))
      {
        mergingLine.Reverse();
        this.MergeInternal(mergingLine, newlyAddedBlock);
      }
      else
      {
        this.Reverse();
        ConveyorLinePosition connectingPosition2 = this.m_endpointPosition2.GetConnectingPosition();
        if (mergingLine.m_endpointPosition1.Equals(connectingPosition2))
          this.MergeInternal(mergingLine, newlyAddedBlock);
        else if (mergingLine.m_endpointPosition2.Equals(connectingPosition2))
        {
          mergingLine.Reverse();
          this.MergeInternal(mergingLine, newlyAddedBlock);
        }
      }
      mergingLine.RecalculateConductivity();
    }

    public void MergeInternal(MyConveyorLine mergingLine, IMyConveyorSegmentBlock newlyAddedBlock = null)
    {
      this.m_endpointPosition2 = mergingLine.m_endpointPosition2;
      this.m_endpoint2 = mergingLine.m_endpoint2;
      if (mergingLine.m_sections != null)
      {
        if (this.m_sections == null)
        {
          this.InitializeSectionList(mergingLine.m_sections.Count);
          this.m_sections.AddRange((IEnumerable<MyConveyorLine.SectionInformation>) mergingLine.m_sections);
          MyConveyorLine.SectionInformation section = this.m_sections[0];
          section.Length += this.m_length - 1;
          this.m_sections[0] = section;
        }
        else
        {
          this.m_sections.Capacity = this.m_sections.Count + mergingLine.m_sections.Count - 1;
          MyConveyorLine.SectionInformation section = this.m_sections[this.m_sections.Count - 1];
          section.Length += mergingLine.m_sections[0].Length - 1;
          this.m_sections[this.m_sections.Count - 1] = section;
          for (int index = 1; index < mergingLine.m_sections.Count; ++index)
            this.m_sections.Add(mergingLine.m_sections[index]);
        }
      }
      else if (this.m_sections != null)
      {
        MyConveyorLine.SectionInformation section = this.m_sections[this.m_sections.Count - 1];
        section.Length += mergingLine.m_length - 1;
        this.m_sections[this.m_sections.Count - 1] = section;
      }
      this.m_length = this.m_length + mergingLine.m_length - 1;
      this.UpdateIsFunctional();
      if (newlyAddedBlock == null)
        return;
      this.m_isFunctional &= newlyAddedBlock.ConveyorSegment.CubeBlock.IsFunctional;
      this.m_isWorking &= this.m_isFunctional;
    }

    public bool CheckSectionConsistency()
    {
      if (this.m_sections == null)
        return true;
      Base6Directions.Direction? nullable = new Base6Directions.Direction?();
      foreach (MyConveyorLine.SectionInformation section in this.m_sections)
      {
        if (nullable.HasValue && nullable.Value == section.Direction)
          return false;
        nullable = new Base6Directions.Direction?(section.Direction);
      }
      return true;
    }

    public void Reverse()
    {
      ConveyorLinePosition endpointPosition1 = this.m_endpointPosition1;
      this.m_endpointPosition1 = this.m_endpointPosition2;
      this.m_endpointPosition2 = endpointPosition1;
      IMyConveyorEndpoint endpoint1 = this.m_endpoint1;
      this.m_endpoint1 = this.m_endpoint2;
      this.m_endpoint2 = endpoint1;
      if (this.m_conductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD)
        this.m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
      else if (this.m_conductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD)
        this.m_conductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
      if (this.m_sections == null)
        return;
      for (int index1 = 0; index1 < (this.m_sections.Count + 1) / 2; ++index1)
      {
        int index2 = this.m_sections.Count - index1 - 1;
        MyConveyorLine.SectionInformation section1 = this.m_sections[index1];
        section1.Direction = Base6Directions.GetFlippedDirection(section1.Direction);
        MyConveyorLine.SectionInformation section2 = this.m_sections[index2];
        section2.Direction = Base6Directions.GetFlippedDirection(section2.Direction);
        this.m_sections[index1] = section2;
        this.m_sections[index2] = section1;
      }
    }

    public void DisconnectEndpoint(IMyConveyorEndpoint endpoint)
    {
      if (endpoint == this.m_endpoint1)
        this.m_endpoint1 = (IMyConveyorEndpoint) null;
      if (endpoint == this.m_endpoint2)
        this.m_endpoint2 = (IMyConveyorEndpoint) null;
      this.UpdateIsFunctional();
    }

    public IEnumerator<Vector3I> GetEnumerator() => (IEnumerator<Vector3I>) new MyConveyorLine.LinePositionEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new MyConveyorLine.LinePositionEnumerator(this);

    public float GetWeight() => (float) this.Length + MyConveyorLine.CONVEYOR_PER_LINE_PENALTY;

    public IMyConveyorEndpoint GetOtherVertex(IMyConveyorEndpoint endpoint)
    {
      if (!this.m_isWorking)
        return (IMyConveyorEndpoint) null;
      MyObjectBuilder_ConveyorLine.LineConductivity lineConductivity = this.m_conductivity;
      if (MyConveyorLine.m_invertedConductivity)
      {
        if (this.m_conductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD)
          lineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD;
        else if (this.m_conductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD)
          lineConductivity = MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD;
      }
      if (endpoint == this.m_endpoint1)
        return lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FULL || lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.BACKWARD ? this.m_endpoint2 : (IMyConveyorEndpoint) null;
      if (endpoint != this.m_endpoint2)
        return (IMyConveyorEndpoint) null;
      return lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FULL || lineConductivity == MyObjectBuilder_ConveyorLine.LineConductivity.FORWARD ? this.m_endpoint1 : (IMyConveyorEndpoint) null;
    }

    public override string ToString() => this.m_endpointPosition1.LocalGridPosition.ToString() + " <-> " + this.m_endpointPosition2.LocalGridPosition.ToString();

    public MyConveyorLine RemovePortion(Vector3I startPosition, Vector3I endPosition)
    {
      if (this.IsCircular)
        this.RotateCircularLine(startPosition);
      if (startPosition != endPosition)
      {
        bool flag1 = false;
        if (this.m_sections != null)
        {
          Vector3I localGridPosition = this.m_endpointPosition1.LocalGridPosition;
          foreach (MyConveyorLine.SectionInformation section in this.m_sections)
          {
            int sectionLength1;
            bool flag2 = MyConveyorLine.PositionIsInSection(startPosition, localGridPosition, section, out sectionLength1);
            int sectionLength2;
            bool flag3 = MyConveyorLine.PositionIsInSection(endPosition, localGridPosition, section, out sectionLength2);
            if (flag2 & flag3)
            {
              if (sectionLength2 < sectionLength1)
              {
                flag1 = true;
                break;
              }
              break;
            }
            if (flag3)
            {
              flag1 = true;
              break;
            }
            if (!flag2)
              localGridPosition += Base6Directions.GetIntVector(section.Direction) * section.Length;
            else
              break;
          }
        }
        else if (Vector3I.DistanceManhattan(this.m_endpointPosition1.LocalGridPosition, endPosition) < Vector3I.DistanceManhattan(this.m_endpointPosition1.LocalGridPosition, startPosition))
          flag1 = true;
        if (flag1)
        {
          Vector3I vector3I = startPosition;
          startPosition = endPosition;
          endPosition = vector3I;
        }
      }
      List<MyConveyorLine.SectionInformation> sections1 = (List<MyConveyorLine.SectionInformation>) null;
      List<MyConveyorLine.SectionInformation> sections2 = (List<MyConveyorLine.SectionInformation>) null;
      ConveyorLinePosition newPosition1_1 = new ConveyorLinePosition(startPosition, this.m_endpointPosition2.Direction);
      ConveyorLinePosition newPosition2 = new ConveyorLinePosition(endPosition, this.m_endpointPosition1.Direction);
      ConveyorLinePosition newPosition1_2 = new ConveyorLinePosition();
      int line1Length1 = 0;
      int num;
      if (this.m_sections != null)
      {
        MyConveyorLine.m_tmpSections1.Clear();
        MyConveyorLine.m_tmpSections2.Clear();
        MyConveyorLine.SplitSections(this.m_sections, this.Length, this.m_endpointPosition1.LocalGridPosition, startPosition, MyConveyorLine.m_tmpSections1, MyConveyorLine.m_tmpSections2, out newPosition1_1, out newPosition2, out line1Length1);
        num = this.Length - line1Length1;
        if (MyConveyorLine.m_tmpSections1.Count > 1)
        {
          sections1 = new List<MyConveyorLine.SectionInformation>();
          sections1.AddRange((IEnumerable<MyConveyorLine.SectionInformation>) MyConveyorLine.m_tmpSections1);
        }
        if (startPosition != endPosition)
        {
          MyConveyorLine.m_tmpSections1.Clear();
          int line1Length2;
          MyConveyorLine.SplitSections(MyConveyorLine.m_tmpSections2, num, newPosition2.LocalGridPosition, endPosition, (List<MyConveyorLine.SectionInformation>) null, MyConveyorLine.m_tmpSections1, out newPosition1_2, out newPosition2, out line1Length2);
          num -= line1Length2;
          if (MyConveyorLine.m_tmpSections1.Count > 1)
          {
            sections2 = new List<MyConveyorLine.SectionInformation>();
            sections2.AddRange((IEnumerable<MyConveyorLine.SectionInformation>) MyConveyorLine.m_tmpSections1);
          }
        }
        else if (MyConveyorLine.m_tmpSections2.Count > 1)
        {
          sections2 = new List<MyConveyorLine.SectionInformation>();
          sections2.AddRange((IEnumerable<MyConveyorLine.SectionInformation>) MyConveyorLine.m_tmpSections2);
        }
        MyConveyorLine.m_tmpSections1.Clear();
        MyConveyorLine.m_tmpSections2.Clear();
      }
      else
      {
        line1Length1 = startPosition.RectangularDistance(this.m_endpointPosition1.LocalGridPosition);
        num = endPosition.RectangularDistance(this.m_endpointPosition2.LocalGridPosition);
      }
      MyConveyorLine myConveyorLine = (MyConveyorLine) null;
      if (line1Length1 <= 1 || line1Length1 < num)
      {
        if (line1Length1 > 1 || line1Length1 > 0 && this.m_endpoint1 != null)
        {
          myConveyorLine = new MyConveyorLine();
          myConveyorLine.InitAfterSplit(this.m_endpointPosition1, newPosition1_1, sections1, line1Length1, this.m_cubeGrid, this.m_type);
          myConveyorLine.InitEndpoints(this.m_endpoint1, (IMyConveyorEndpoint) null);
        }
        this.InitAfterSplit(newPosition2, this.m_endpointPosition2, sections2, num, this.m_cubeGrid, this.m_type);
        this.InitEndpoints((IMyConveyorEndpoint) null, this.m_endpoint2);
      }
      else
      {
        if (num > 1 || num > 0 && this.m_endpoint2 != null)
        {
          myConveyorLine = new MyConveyorLine();
          myConveyorLine.InitAfterSplit(newPosition2, this.m_endpointPosition2, sections2, num, this.m_cubeGrid, this.m_type);
          myConveyorLine.InitEndpoints((IMyConveyorEndpoint) null, this.m_endpoint2);
        }
        this.InitAfterSplit(this.m_endpointPosition1, newPosition1_1, sections1, line1Length1, this.m_cubeGrid, this.m_type);
        this.InitEndpoints(this.m_endpoint1, (IMyConveyorEndpoint) null);
      }
      this.RecalculateConductivity();
      myConveyorLine?.RecalculateConductivity();
      return myConveyorLine;
    }

    private static void SplitSections(
      List<MyConveyorLine.SectionInformation> sections,
      int lengthLimit,
      Vector3I startPosition,
      Vector3I splittingPosition,
      List<MyConveyorLine.SectionInformation> sections1,
      List<MyConveyorLine.SectionInformation> sections2,
      out ConveyorLinePosition newPosition1,
      out ConveyorLinePosition newPosition2,
      out int line1Length)
    {
      bool flag = false;
      line1Length = 0;
      Vector3I sectionStart = startPosition;
      MyConveyorLine.SectionInformation section = new MyConveyorLine.SectionInformation();
      int sectionLength = 0;
      int index1;
      for (index1 = 0; index1 < sections.Count; ++index1)
      {
        section = sections[index1];
        if (MyConveyorLine.PositionIsInSection(splittingPosition, sectionStart, section, out sectionLength))
        {
          line1Length += sectionLength;
          if (sectionLength == 0)
          {
            flag = true;
            break;
          }
          break;
        }
        line1Length += section.Length;
        sectionStart += Base6Directions.GetIntVector(section.Direction) * section.Length;
      }
      newPosition2 = new ConveyorLinePosition(splittingPosition, section.Direction);
      newPosition1 = !flag ? new ConveyorLinePosition(splittingPosition, Base6Directions.GetFlippedDirection(section.Direction)) : new ConveyorLinePosition(splittingPosition, Base6Directions.GetFlippedDirection(sections[index1 - 1].Direction));
      int num1 = flag ? index1 : index1 + 1;
      int num2 = sections.Count - index1;
      MyConveyorLine.SectionInformation sectionInformation = new MyConveyorLine.SectionInformation();
      if (line1Length >= lengthLimit)
      {
        MyLog.Default.Error("Conveyor line splitting failed. If modded conveyors are used, they must be straight, not curved.");
      }
      else
      {
        if (sections1 != null)
        {
          for (int index2 = 0; index2 < num1 - 1; ++index2)
            sections1.Add(sections[index2]);
          if (flag)
          {
            sections1.Add(sections[num1 - 1]);
          }
          else
          {
            sectionInformation.Direction = sections[num1 - 1].Direction;
            sectionInformation.Length = sectionLength;
            sections1.Add(sectionInformation);
          }
        }
        sectionInformation.Direction = sections[index1].Direction;
        sectionInformation.Length = sections[index1].Length - sectionLength;
        sections2.Add(sectionInformation);
        for (int index2 = 1; index2 < num2; ++index2)
          sections2.Add(sections[index1 + index2]);
      }
    }

    private static bool PositionIsInSection(
      Vector3I position,
      Vector3I sectionStart,
      MyConveyorLine.SectionInformation section,
      out int sectionLength)
    {
      sectionLength = 0;
      Vector3I intVector = Base6Directions.GetIntVector(section.Direction);
      Vector3I vector3I = position - sectionStart;
      switch (Base6Directions.GetAxis(section.Direction))
      {
        case Base6Directions.Axis.ForwardBackward:
          sectionLength = intVector.Z * vector3I.Z;
          break;
        case Base6Directions.Axis.LeftRight:
          sectionLength = intVector.X * vector3I.X;
          break;
        case Base6Directions.Axis.UpDown:
          sectionLength = intVector.Y * vector3I.Y;
          break;
      }
      return sectionLength >= 0 && sectionLength < section.Length && vector3I.RectangularLength() == sectionLength;
    }

    private void RotateCircularLine(Vector3I position)
    {
      List<MyConveyorLine.SectionInformation> sectionInformationList = new List<MyConveyorLine.SectionInformation>(this.m_sections.Count + 1);
      Vector3I localGridPosition = this.m_endpointPosition1.LocalGridPosition;
      for (int index1 = 0; index1 < this.m_sections.Count; ++index1)
      {
        MyConveyorLine.SectionInformation section = this.m_sections[index1];
        int num = 0;
        Vector3I intVector = Base6Directions.GetIntVector(section.Direction);
        Vector3I vector3I = position - localGridPosition;
        switch (Base6Directions.GetAxis(section.Direction))
        {
          case Base6Directions.Axis.ForwardBackward:
            num = intVector.Z * vector3I.Z;
            break;
          case Base6Directions.Axis.LeftRight:
            num = intVector.X * vector3I.X;
            break;
          case Base6Directions.Axis.UpDown:
            num = intVector.Y * vector3I.Y;
            break;
        }
        if (num > 0 && num <= section.Length && vector3I.RectangularLength() == num)
        {
          sectionInformationList.Add(new MyConveyorLine.SectionInformation()
          {
            Direction = this.m_sections[index1].Direction,
            Length = this.m_sections[index1].Length - num + 1
          });
          for (int index2 = index1 + 1; index2 < this.m_sections.Count - 1; ++index2)
            sectionInformationList.Add(this.m_sections[index2]);
          sectionInformationList.Add(new MyConveyorLine.SectionInformation()
          {
            Direction = this.m_sections[0].Direction,
            Length = this.m_sections[0].Length + this.m_sections[this.m_sections.Count - 1].Length - 1
          });
          for (int index2 = 1; index2 < index1; ++index2)
            sectionInformationList.Add(this.m_sections[index2]);
          sectionInformationList.Add(new MyConveyorLine.SectionInformation()
          {
            Direction = this.m_sections[index1].Direction,
            Length = num
          });
          break;
        }
        localGridPosition += Base6Directions.GetIntVector(section.Direction) * section.Length;
      }
      this.m_sections = sectionInformationList;
      this.m_endpointPosition2 = new ConveyorLinePosition(position, Base6Directions.GetFlippedDirection(this.m_sections[0].Direction));
      this.m_endpointPosition1 = this.m_endpointPosition2.GetConnectingPosition();
    }

    private MyCubeGrid GetGrid() => this.m_endpoint1 == null || this.m_endpoint2 == null ? (MyCubeGrid) null : this.m_endpoint1.CubeBlock.CubeGrid;

    public void StopQueuesIfNeeded()
    {
      if ((double) this.m_queuePosition != 0.0)
        return;
      if (!this.m_stopped1 && this.m_queue1.Count != 0 && this.m_queue1.First().LinePosition >= this.Length - 1)
        this.m_stopped1 = true;
      if (this.m_stopped2 || this.m_queue2.Count == 0 || this.m_queue2.First().LinePosition < this.Length - 1)
        return;
      this.m_stopped2 = true;
    }

    public void Update()
    {
      this.m_queuePosition += 1f / 64f;
      if ((double) this.m_queuePosition < 1.0)
        return;
      this.BigUpdate();
    }

    public void BigUpdate()
    {
      this.StopQueuesIfNeeded();
      if (!this.m_stopped1)
      {
        foreach (MyConveyorPacket myConveyorPacket in this.m_queue1)
          ++myConveyorPacket.LinePosition;
      }
      if (!this.m_stopped2)
      {
        foreach (MyConveyorPacket myConveyorPacket in this.m_queue2)
          ++myConveyorPacket.LinePosition;
      }
      if (!this.m_isWorking)
      {
        this.m_stopped1 = true;
        this.m_stopped2 = true;
      }
      this.m_queuePosition = 0.0f;
      if (this.m_stopped1 && this.m_stopped2)
        return;
      this.RecalculatePacketPositions();
    }

    public void UpdateIsFunctional()
    {
      this.m_isFunctional = this.UpdateIsFunctionalInternal();
      int num = (int) this.UpdateIsWorking();
    }

    public MyResourceStateEnum UpdateIsWorking()
    {
      MyResourceStateEnum resourceStateEnum = MyResourceStateEnum.Disconnected;
      if (!this.m_isFunctional)
      {
        this.m_isWorking = false;
        return resourceStateEnum;
      }
      if (this.IsDisconnected)
      {
        this.m_isWorking = false;
        return resourceStateEnum;
      }
      if (MySession.Static == null)
      {
        this.m_isWorking = false;
        return resourceStateEnum;
      }
      MyCubeGrid grid = this.GetGrid();
      if (grid.GridSystems.ResourceDistributor != null)
      {
        resourceStateEnum = grid.GridSystems.ResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
        bool flag = resourceStateEnum != MyResourceStateEnum.NoPower;
        if (this.m_isWorking != flag)
        {
          this.m_isWorking = flag;
          grid.GridSystems.ConveyorSystem.FlagForRecomputation();
        }
      }
      return resourceStateEnum;
    }

    private bool UpdateIsFunctionalInternal()
    {
      if (this.m_endpoint1 == null || this.m_endpoint2 == null || (!this.m_endpoint1.CubeBlock.IsFunctional || !this.m_endpoint2.CubeBlock.IsFunctional))
        return false;
      MyCubeGrid cubeGrid = this.m_endpoint1.CubeBlock.CubeGrid;
      foreach (Vector3I pos in this)
      {
        MySlimBlock cubeBlock = cubeGrid.GetCubeBlock(pos);
        if (cubeBlock != null && cubeBlock.FatBlock != null && !cubeBlock.FatBlock.IsFunctional)
          return false;
      }
      return true;
    }

    public void PrepareForDraw(MyCubeGrid grid)
    {
      if (this.m_queue1.Count == 0 && this.m_queue2.Count == 0)
        return;
      if (!this.m_stopped1)
      {
        foreach (MyConveyorPacket myConveyorPacket in this.m_queue1)
          myConveyorPacket.MoveRelative(1f / 64f);
      }
      if (this.m_stopped2)
        return;
      foreach (MyConveyorPacket myConveyorPacket in this.m_queue2)
        myConveyorPacket.MoveRelative(1f / 64f);
    }

    public void DebugDraw(MyCubeGrid grid)
    {
      Vector3 position1 = new Vector3(this.m_endpointPosition1.LocalGridPosition) * grid.GridSize;
      Vector3 position2 = new Vector3(this.m_endpointPosition2.LocalGridPosition) * grid.GridSize;
      MatrixD worldMatrix = grid.WorldMatrix;
      Vector3 vector3_1 = (Vector3) Vector3.Transform(position1, worldMatrix);
      Vector3 vector3_2 = (Vector3) Vector3.Transform(position2, grid.WorldMatrix);
      string text = (this.m_endpoint1 == null ? "- " : "# ") + this.m_length.ToString() + " " + this.m_type.ToString() + (this.m_endpoint2 == null ? " -" : " #") + " " + this.m_conductivity.ToString();
      MyRenderProxy.DebugDrawText3D((Vector3D) ((vector3_1 + vector3_2) * 0.5f), text, Color.Blue, 1f, false);
      Color color = this.IsFunctional ? Color.Green : Color.Red;
      MyRenderProxy.DebugDrawLine3D((Vector3D) vector3_1, (Vector3D) vector3_2, color, color, false);
    }

    public void DebugDrawPackets()
    {
      foreach (MyConveyorPacket myConveyorPacket in this.m_queue1)
      {
        MyRenderProxy.DebugDrawSphere(myConveyorPacket.WorldMatrix.Translation, 0.2f, (Color) Color.Red.ToVector3(), depthRead: false);
        MyRenderProxy.DebugDrawText3D(myConveyorPacket.WorldMatrix.Translation, myConveyorPacket.LinePosition.ToString(), Color.White, 1f, false);
      }
      foreach (MyConveyorPacket myConveyorPacket in this.m_queue2)
      {
        MyRenderProxy.DebugDrawSphere(myConveyorPacket.WorldMatrix.Translation, 0.2f, (Color) Color.Red.ToVector3(), depthRead: false);
        MyRenderProxy.DebugDrawText3D(myConveyorPacket.WorldMatrix.Translation, myConveyorPacket.LinePosition.ToString(), Color.White, 1f, false);
      }
    }

    public struct LinePositionEnumerator : IEnumerator<Vector3I>, IEnumerator, IDisposable
    {
      private MyConveyorLine m_line;
      private Vector3I m_currentPosition;
      private Vector3I m_direction;
      private int m_index;
      private int m_sectionIndex;
      private int m_sectionLength;

      public LinePositionEnumerator(MyConveyorLine line)
      {
        this.m_line = line;
        this.m_currentPosition = line.m_endpointPosition1.LocalGridPosition;
        this.m_direction = line.m_endpointPosition1.VectorDirection;
        this.m_index = 0;
        this.m_sectionIndex = 0;
        this.m_sectionLength = this.m_line.m_length;
        this.UpdateSectionLength();
      }

      public Vector3I Current => this.m_currentPosition;

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        ++this.m_index;
        this.m_currentPosition += this.m_direction;
        if (this.m_index >= this.m_sectionLength)
        {
          this.m_index = 0;
          ++this.m_sectionIndex;
          if (this.m_line.m_sections == null || this.m_sectionIndex >= this.m_line.m_sections.Count)
            return false;
          this.m_direction = Base6Directions.GetIntVector(this.m_line.m_sections[this.m_sectionIndex].Direction);
          this.UpdateSectionLength();
        }
        return true;
      }

      public void Reset()
      {
        this.m_currentPosition = this.m_line.m_endpointPosition1.LocalGridPosition;
        this.m_direction = this.m_line.m_endpointPosition1.VectorDirection;
        this.m_index = 0;
        this.m_sectionIndex = 0;
        this.m_sectionLength = this.m_line.m_length;
        this.UpdateSectionLength();
      }

      object IEnumerator.Current => (object) this.Current;

      private void UpdateSectionLength()
      {
        if (this.m_line.m_sections == null || this.m_line.m_sections.Count == 0)
          return;
        this.m_sectionLength = this.m_line.m_sections[this.m_sectionIndex].Length;
      }
    }

    private struct SectionInformation
    {
      public Base6Directions.Direction Direction;
      public int Length;

      public void Reverse() => this.Direction = Base6Directions.GetFlippedDirection(this.Direction);

      public override string ToString() => this.Length.ToString() + " -> " + this.Direction.ToString();
    }

    public struct BlockLinePositionInformation
    {
      public ConveyorLinePosition Position;
      public MyObjectBuilder_ConveyorLine.LineType LineType;
      public MyObjectBuilder_ConveyorLine.LineConductivity LineConductivity;
    }

    public class InvertedConductivity : IDisposable
    {
      public InvertedConductivity() => MyConveyorLine.m_invertedConductivity = !MyConveyorLine.m_invertedConductivity;

      public void Dispose() => MyConveyorLine.m_invertedConductivity = !MyConveyorLine.m_invertedConductivity;
    }
  }
}
