// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.Conveyors.MyConveyorPacket
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities.Debris;
using System;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Network;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.GameSystems.Conveyors
{
  [MyEntityType(typeof (MyObjectBuilder_ConveyorPacket), true)]
  public class MyConveyorPacket : MyEntity
  {
    public MyPhysicalInventoryItem Item;
    public int LinePosition;
    private float m_segmentLength;
    private Base6Directions.Direction m_segmentDirection;

    public void Init(MyObjectBuilder_ConveyorPacket builder, MyEntity parent)
    {
      this.Item = new MyPhysicalInventoryItem(builder.Item);
      this.LinePosition = builder.LinePosition;
      MyPhysicalItemDefinition physicalItemDefinition = MyDefinitionManager.Static.GetPhysicalItemDefinition((MyObjectBuilder_Base) this.Item.Content);
      MyObjectBuilder_Ore content = this.Item.Content as MyObjectBuilder_Ore;
      string model = physicalItemDefinition.Model;
      float num1 = 1f;
      if (content != null)
      {
        foreach (MyVoxelMaterialDefinition materialDefinition in MyDefinitionManager.Static.GetVoxelMaterialDefinitions())
        {
          if (materialDefinition.MinedOre == content.SubtypeName)
          {
            model = MyDebris.GetRandomDebrisVoxel();
            num1 = (float) Math.Pow((double) (float) this.Item.Amount * (double) physicalItemDefinition.Volume / (double) MyDebris.VoxelDebrisModelVolume, 0.333000004291534);
            break;
          }
        }
      }
      if ((double) num1 < 0.0500000007450581)
        num1 = 0.05f;
      else if ((double) num1 > 1.0)
        num1 = 1f;
      int num2 = MyEntityIdentifier.AllocationSuspended ? 1 : 0;
      MyEntityIdentifier.AllocationSuspended = false;
      this.Init((StringBuilder) null, model, parent, new float?());
      MyEntityIdentifier.AllocationSuspended = num2 != 0;
      this.PositionComp.Scale = new float?(num1);
      this.Save = false;
    }

    public void SetSegmentLength(float length) => this.m_segmentLength = length;

    public void SetLocalPosition(
      Vector3I sectionStart,
      int sectionStartPosition,
      float cubeSize,
      Base6Directions.Direction forward,
      Base6Directions.Direction offset)
    {
      int num = this.LinePosition - sectionStartPosition;
      Matrix localMatrix = this.PositionComp.LocalMatrixRef;
      Matrix matrix = this.PositionComp.LocalMatrixRef;
      Vector3 vector3_1 = matrix.GetDirectionVector(forward) * (float) num;
      matrix = this.PositionComp.LocalMatrixRef;
      Vector3 vector3_2 = matrix.GetDirectionVector(offset) * 0.1f;
      Vector3 vector3_3 = vector3_1 + vector3_2;
      localMatrix.Translation = ((Vector3) sectionStart + vector3_3 / this.PositionComp.Scale.Value) * cubeSize;
      this.PositionComp.SetLocalMatrix(ref localMatrix);
      this.m_segmentDirection = forward;
    }

    public void MoveRelative(float linePositionFraction)
    {
      this.PrepareForDraw();
      Matrix localMatrix = this.PositionComp.LocalMatrixRef;
      localMatrix.Translation += this.PositionComp.LocalMatrixRef.GetDirectionVector(this.m_segmentDirection) * this.m_segmentLength * linePositionFraction / this.PositionComp.Scale.Value;
      this.PositionComp.SetLocalMatrix(ref localMatrix);
    }

    private class Sandbox_Game_GameSystems_Conveyors_MyConveyorPacket\u003C\u003EActor : IActivator, IActivator<MyConveyorPacket>
    {
      object IActivator.CreateInstance() => (object) new MyConveyorPacket();

      MyConveyorPacket IActivator<MyConveyorPacket>.CreateInstance() => new MyConveyorPacket();
    }
  }
}
