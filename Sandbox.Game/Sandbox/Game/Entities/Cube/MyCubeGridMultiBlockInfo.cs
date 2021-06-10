// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Cube.MyCubeGridMultiBlockInfo
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
  public class MyCubeGridMultiBlockInfo
  {
    private static List<MyMultiBlockDefinition.MyMultiBlockPartDefinition> m_tmpPartDefinitions = new List<MyMultiBlockDefinition.MyMultiBlockPartDefinition>();
    public int MultiBlockId;
    public MyMultiBlockDefinition MultiBlockDefinition;
    public MyCubeBlockDefinition MainBlockDefinition;
    public HashSet<MySlimBlock> Blocks = new HashSet<MySlimBlock>();

    public bool GetTransform(out MatrixI transform)
    {
      transform = new MatrixI();
      if (this.Blocks.Count != 0)
      {
        MySlimBlock mySlimBlock = this.Blocks.First<MySlimBlock>();
        if (mySlimBlock.MultiBlockIndex < this.MultiBlockDefinition.BlockDefinitions.Length)
        {
          MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition = this.MultiBlockDefinition.BlockDefinitions[mySlimBlock.MultiBlockIndex];
          transform = MatrixI.CreateRotation(blockDefinition.Forward, blockDefinition.Up, mySlimBlock.Orientation.Forward, mySlimBlock.Orientation.Up);
          transform.Translation = mySlimBlock.Position - Vector3I.TransformNormal(blockDefinition.Min, ref transform);
          return true;
        }
      }
      return false;
    }

    public bool GetBoundingBox(out Vector3I min, out Vector3I max)
    {
      min = new Vector3I();
      max = new Vector3I();
      MatrixI transform;
      if (!this.GetTransform(out transform))
        return false;
      Vector3I vector3I1 = Vector3I.Transform(this.MultiBlockDefinition.Min, transform);
      Vector3I vector3I2 = Vector3I.Transform(this.MultiBlockDefinition.Max, transform);
      min = Vector3I.Min(vector3I1, vector3I2);
      max = Vector3I.Max(vector3I1, vector3I2);
      return true;
    }

    public bool GetMissingBlocks(out MatrixI transform, List<int> multiBlockIndices)
    {
      for (int i = 0; i < this.MultiBlockDefinition.BlockDefinitions.Length; ++i)
      {
        if (!this.Blocks.Any<MySlimBlock>((Func<MySlimBlock, bool>) (b => b.MultiBlockIndex == i)))
          multiBlockIndices.Add(i);
      }
      return this.GetTransform(out transform);
    }

    public bool CanAddBlock(
      ref Vector3I otherGridPositionMin,
      ref Vector3I otherGridPositionMax,
      MyBlockOrientation otherOrientation,
      MyCubeBlockDefinition otherDefinition)
    {
      MatrixI transform;
      if (!this.GetTransform(out transform))
        return true;
      try
      {
        MatrixI result1;
        MatrixI.Invert(ref transform, out result1);
        Vector3I vector3I1 = Vector3I.Transform(otherGridPositionMin, ref result1);
        Vector3I vector3I2 = Vector3I.Transform(otherGridPositionMax, ref result1);
        Vector3I minB = Vector3I.Min(vector3I1, vector3I2);
        Vector3I maxB = Vector3I.Max(vector3I1, vector3I2);
        if (!Vector3I.BoxIntersects(ref this.MultiBlockDefinition.Min, ref this.MultiBlockDefinition.Max, ref minB, ref maxB))
          return true;
        MatrixI leftMatrix = new MatrixI(otherOrientation);
        MatrixI result2;
        MatrixI.Multiply(ref leftMatrix, ref result1, out result2);
        MyBlockOrientation otherOrientation1 = new MyBlockOrientation(result2.Forward, result2.Up);
        MyCubeGridMultiBlockInfo.m_tmpPartDefinitions.Clear();
        foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition blockDefinition in this.MultiBlockDefinition.BlockDefinitions)
        {
          if (Vector3I.BoxIntersects(ref blockDefinition.Min, ref blockDefinition.Max, ref minB, ref maxB))
          {
            if (!(minB == maxB) || !(blockDefinition.Min == blockDefinition.Max))
              return false;
            MyCubeGridMultiBlockInfo.m_tmpPartDefinitions.Add(blockDefinition);
          }
        }
        if (MyCubeGridMultiBlockInfo.m_tmpPartDefinitions.Count == 0)
          return true;
        bool flag = true;
        foreach (MyMultiBlockDefinition.MyMultiBlockPartDefinition tmpPartDefinition in MyCubeGridMultiBlockInfo.m_tmpPartDefinitions)
        {
          MyCubeBlockDefinition blockDefinition;
          if (MyDefinitionManager.Static.TryGetCubeBlockDefinition(tmpPartDefinition.Id, out blockDefinition) && blockDefinition != null)
          {
            flag &= MyCompoundCubeBlock.CanAddBlocks(blockDefinition, new MyBlockOrientation(tmpPartDefinition.Forward, tmpPartDefinition.Up), otherDefinition, otherOrientation1);
            if (!flag)
              break;
          }
        }
        return flag;
      }
      finally
      {
        MyCubeGridMultiBlockInfo.m_tmpPartDefinitions.Clear();
      }
    }

    public bool IsFractured()
    {
      foreach (MySlimBlock block in this.Blocks)
      {
        if (block.GetFractureComponent() != null)
          return true;
      }
      return false;
    }

    public float GetTotalMaxIntegrity()
    {
      float num = 0.0f;
      foreach (MySlimBlock block in this.Blocks)
        num += block.MaxIntegrity;
      return num;
    }
  }
}
