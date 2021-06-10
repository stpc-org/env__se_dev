// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyGridSkeleton
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities.Cube;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using VRage;
using VRageMath;

namespace Sandbox.Game.Entities
{
  internal class MyGridSkeleton
  {
    public readonly ConcurrentDictionary<Vector3I, Vector3> Bones = new ConcurrentDictionary<Vector3I, Vector3>();
    private List<Vector3I> m_tmpRemovedCubes = new List<Vector3I>();
    private HashSet<Vector3I> m_usedBones = new HashSet<Vector3I>();
    private HashSet<Vector3I> m_testedCubes = new HashSet<Vector3I>();
    private static readonly float MAX_BONE_ERROR;
    [ThreadStatic]
    private static List<Vector3I> m_tempAffectedCubes = new List<Vector3I>();
    public const int BoneDensity = 2;
    public static readonly Vector3I[] BoneOffsets;

    static MyGridSkeleton()
    {
      MyGridSkeleton.MAX_BONE_ERROR = Vector3UByte.Denormalize(new Vector3UByte((byte) 128, (byte) 128, (byte) 128), 1f).X * 0.75f;
      MyGridSkeleton.BoneOffsets = new Vector3I[(int) Math.Pow(3.0, 3.0)];
      int num = 0;
      Vector3I vector3I;
      for (vector3I.X = 0; vector3I.X <= 1; ++vector3I.X)
      {
        for (vector3I.Y = 0; vector3I.Y <= 1; ++vector3I.Y)
        {
          for (vector3I.Z = 0; vector3I.Z <= 1; ++vector3I.Z)
            MyGridSkeleton.BoneOffsets[num++] = vector3I * 2;
        }
      }
      for (vector3I.X = 0; vector3I.X <= 2; ++vector3I.X)
      {
        for (vector3I.Y = 0; vector3I.Y <= 2; ++vector3I.Y)
        {
          for (vector3I.Z = 0; vector3I.Z <= 2; ++vector3I.Z)
          {
            if (vector3I.X == 1 || vector3I.Y == 1 || vector3I.Z == 1)
              MyGridSkeleton.BoneOffsets[num++] = vector3I;
          }
        }
      }
    }

    public static float GetMaxBoneError(float gridSize) => MyGridSkeleton.MAX_BONE_ERROR * gridSize;

    public void Reset() => this.Bones.Clear();

    public void CopyTo(MyGridSkeleton target, Vector3I fromGridPosition)
    {
      Vector3I vector3I = fromGridPosition * 2;
      foreach (Vector3I boneOffset in MyGridSkeleton.BoneOffsets)
      {
        Vector3I key = vector3I + boneOffset;
        Vector3 vector3;
        if (this.Bones.TryGetValue(key, out vector3))
          target.Bones[key] = vector3;
      }
    }

    public void CopyTo(MyGridSkeleton target, Vector3I fromGridPosition, Vector3I toGridPosition)
    {
      Vector3I vector3I1 = fromGridPosition * 2;
      Vector3I vector3I2 = (toGridPosition - fromGridPosition + Vector3I.One) * 2;
      Vector3I vector3I3;
      for (vector3I3.X = 0; vector3I3.X <= vector3I2.X; ++vector3I3.X)
      {
        for (vector3I3.Y = 0; vector3I3.Y <= vector3I2.Y; ++vector3I3.Y)
        {
          for (vector3I3.Z = 0; vector3I3.Z <= vector3I2.Z; ++vector3I3.Z)
          {
            Vector3I key = vector3I1 + vector3I3;
            Vector3 vector3;
            if (this.Bones.TryGetValue(key, out vector3))
              target.Bones[key] = vector3;
            else
              target.Bones.Remove<Vector3I, Vector3>(key);
          }
        }
      }
    }

    public void CopyTo(MyGridSkeleton target, MatrixI transformationMatrix, MyCubeGrid targetGrid)
    {
      MatrixI rightMatrix = new MatrixI(new Vector3I(1, 1, 1), Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
      MatrixI leftMatrix = new MatrixI(new Vector3I(-1, -1, -1), Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
      transformationMatrix.Translation *= 2;
      MatrixI result1;
      MatrixI.Multiply(ref leftMatrix, ref transformationMatrix, out result1);
      MatrixI.Multiply(ref result1, ref rightMatrix, out transformationMatrix);
      Matrix result2;
      transformationMatrix.GetBlockOrientation().GetMatrix(out result2);
      foreach (KeyValuePair<Vector3I, Vector3> bone in this.Bones)
      {
        Vector3I key = bone.Key;
        Vector3I result3;
        Vector3I.Transform(ref key, ref transformationMatrix, out result3);
        Vector3 vector3_1 = Vector3.Transform(bone.Value, result2);
        Vector3 vector3_2;
        if (target.Bones.TryGetValue(result3, out vector3_2))
        {
          Vector3 vector3_3 = (vector3_2 + vector3_1) * 0.5f;
          target.Bones[result3] = vector3_3;
        }
        else
          target.Bones[result3] = vector3_1;
        Vector3I vector3I = result3 / 2;
        for (int x = -1; x <= 1; ++x)
        {
          for (int y = -1; y <= 1; ++y)
          {
            for (int z = -1; z <= 1; ++z)
              targetGrid.SetCubeDirty(vector3I + new Vector3I(x, y, z));
          }
        }
      }
    }

    public void FixBone(
      Vector3I gridPosition,
      Vector3I boneOffset,
      float gridSize,
      float minBoneDist = 0.05f)
    {
      this.FixBone(gridPosition * 2 + boneOffset, minBoneDist);
    }

    private void FixBone(Vector3I bonePosition, float gridSize, float minBoneDist = 0.05f)
    {
      Vector3 defaultBone1 = -Vector3.One * gridSize;
      Vector3 defaultBone2 = Vector3.One * gridSize;
      Vector3 vector3_1;
      vector3_1.X = this.TryGetBone(bonePosition - Vector3I.UnitX, ref defaultBone1).X;
      vector3_1.Y = this.TryGetBone(bonePosition - Vector3I.UnitY, ref defaultBone1).Y;
      vector3_1.Z = this.TryGetBone(bonePosition - Vector3I.UnitZ, ref defaultBone1).Z;
      Vector3 min = vector3_1 - new Vector3(gridSize / 2f) + new Vector3(minBoneDist);
      Vector3 vector3_2;
      vector3_2.X = this.TryGetBone(bonePosition + Vector3I.UnitX, ref defaultBone2).X;
      vector3_2.Y = this.TryGetBone(bonePosition + Vector3I.UnitY, ref defaultBone2).Y;
      vector3_2.Z = this.TryGetBone(bonePosition + Vector3I.UnitZ, ref defaultBone2).Z;
      Vector3 max = vector3_2 + new Vector3(gridSize / 2f) - new Vector3(minBoneDist);
      this.Bones[bonePosition] = Vector3.Clamp(this.Bones[bonePosition], min, max);
    }

    private Vector3 TryGetBone(Vector3I bonePosition, ref Vector3 defaultBone)
    {
      Vector3 vector3;
      return this.Bones.TryGetValue(bonePosition, out vector3) ? vector3 : defaultBone;
    }

    public void Serialize(List<VRage.Game.BoneInfo> result, float boneRange, MyCubeGrid grid)
    {
      VRage.Game.BoneInfo boneInfo = new VRage.Game.BoneInfo();
      float maxBoneError = MyGridSkeleton.GetMaxBoneError(grid.GridSize);
      float num = maxBoneError * maxBoneError;
      foreach (KeyValuePair<Vector3I, Vector3> bone in this.Bones)
      {
        Vector3I? cubeFromBone = this.GetCubeFromBone(bone.Key, grid);
        if (cubeFromBone.HasValue && (double) Math.Abs(this.GetDefinitionOffsetWithNeighbours(cubeFromBone.Value, bone.Key, grid).LengthSquared() - bone.Value.LengthSquared()) > (double) num)
        {
          boneInfo.BonePosition = (SerializableVector3I) bone.Key;
          boneInfo.BoneOffset = (SerializableVector3UByte) Vector3UByte.Normalize(bone.Value, boneRange);
          if (!Vector3UByte.IsMiddle((Vector3UByte) boneInfo.BoneOffset))
            result.Add(boneInfo);
        }
      }
    }

    private Vector3I? GetCubeFromBone(Vector3I bone, MyCubeGrid grid)
    {
      Vector3I zero = Vector3I.Zero;
      Vector3I pos1 = bone / 2;
      if (grid.CubeExists(pos1))
        return new Vector3I?(pos1);
      for (int x = -1; x <= 1; ++x)
      {
        for (int y = -1; y <= 1; ++y)
        {
          for (int z = -1; z <= 1; ++z)
          {
            Vector3I pos2 = pos1 + new Vector3I(x, y, z);
            Vector3I vector3I = bone - pos2 * 2;
            if (vector3I.X <= 2 && vector3I.Y <= 2 && (vector3I.Z <= 2 && grid.CubeExists(pos2)))
              return new Vector3I?(pos2);
          }
        }
      }
      return new Vector3I?();
    }

    public void Deserialize(List<VRage.Game.BoneInfo> data, float boneRange, float gridSize, bool clear = false)
    {
      if (clear)
        this.Bones.Clear();
      foreach (VRage.Game.BoneInfo boneInfo in data)
        this.Bones[(Vector3I) boneInfo.BonePosition] = Vector3UByte.Denormalize((Vector3UByte) boneInfo.BoneOffset, boneRange);
    }

    public bool SerializePart(
      Vector3I minBone,
      Vector3I maxBone,
      float boneRange,
      List<byte> result)
    {
      bool flag = false;
      minBone.ToBytes(result);
      maxBone.ToBytes(result);
      int count = result.Count;
      result.Add((byte) 1);
      Vector3I key;
      for (key.X = minBone.X; key.X <= maxBone.X; ++key.X)
      {
        for (key.Y = minBone.Y; key.Y <= maxBone.Y; ++key.Y)
        {
          for (key.Z = minBone.Z; key.Z <= maxBone.Z; ++key.Z)
          {
            Vector3 vec;
            flag |= this.Bones.TryGetValue(key, out vec);
            Vector3UByte vector3Ubyte = Vector3UByte.Normalize(vec, boneRange);
            result.Add(vector3Ubyte.X);
            result.Add(vector3Ubyte.Y);
            result.Add(vector3Ubyte.Z);
          }
        }
      }
      if (!flag)
      {
        result.RemoveRange(count, result.Count - count);
        result.Add((byte) 0);
      }
      return flag;
    }

    public int DeserializePart(
      float boneRange,
      byte[] data,
      ref int dataIndex,
      out Vector3I minBone,
      out Vector3I maxBone)
    {
      minBone = new Vector3I(data, dataIndex);
      dataIndex += 12;
      maxBone = new Vector3I(data, dataIndex);
      dataIndex += 12;
      bool flag = data[dataIndex] > (byte) 0;
      ++dataIndex;
      Vector3I vector3I1 = maxBone - minBone + Vector3I.One;
      if (flag && dataIndex + vector3I1.Size * 3 > data.Length)
        return dataIndex;
      Vector3I vector3I2;
      for (vector3I2.X = minBone.X; vector3I2.X <= maxBone.X; ++vector3I2.X)
      {
        for (vector3I2.Y = minBone.Y; vector3I2.Y <= maxBone.Y; ++vector3I2.Y)
        {
          for (vector3I2.Z = minBone.Z; vector3I2.Z <= maxBone.Z; ++vector3I2.Z)
          {
            if (flag)
            {
              this[vector3I2] = Vector3UByte.Denormalize(new Vector3UByte(data[dataIndex], data[dataIndex + 1], data[dataIndex + 2]), boneRange);
              dataIndex += 3;
            }
            else
              this.Bones.Remove<Vector3I, Vector3>(vector3I2);
          }
        }
      }
      return dataIndex;
    }

    public Vector3 GetBone(Vector3I cubePos, Vector3I bonePos)
    {
      Vector3 vector3;
      return !this.Bones.TryGetValue(cubePos * 2 + bonePos, out vector3) ? Vector3.Zero : vector3;
    }

    public void GetBone(ref Vector3I pos, out Vector3 bone)
    {
      if (this.Bones.TryGetValue(pos, out bone))
        return;
      bone = Vector3.Zero;
    }

    public bool TryGetBone(ref Vector3I pos, out Vector3 bone) => this.Bones.TryGetValue(pos, out bone);

    public void SetBone(ref Vector3I pos, ref Vector3 bone) => this.Bones[pos] = bone;

    public void SetOrClearBone(ref Vector3I pos, ref Vector3 bone)
    {
      if (bone == Vector3.Zero)
        this.Bones.Remove<Vector3I, Vector3>(pos);
      else
        this.Bones[pos] = bone;
    }

    public void ClearBone(ref Vector3I pos) => this.Bones.Remove<Vector3I, Vector3>(pos);

    public bool MultiplyBone(
      ref Vector3I pos,
      float factor,
      ref Vector3I cubePos,
      MyCubeGrid cubeGrid,
      float epsilon = 0.005f)
    {
      Vector3 vector3_1;
      if (!this.Bones.TryGetValue(pos, out vector3_1))
        return false;
      Vector3 offsetWithNeighbours = this.GetDefinitionOffsetWithNeighbours(cubePos, pos, cubeGrid);
      factor = 1f - factor;
      if ((double) factor < 0.100000001490116)
        factor = 0.1f;
      Vector3 vector3_2 = Vector3.Lerp(vector3_1, offsetWithNeighbours, factor);
      if ((double) vector3_2.LengthSquared() < (double) epsilon * (double) epsilon)
        this.Bones.Remove<Vector3I, Vector3>(pos);
      else
        this.Bones[pos] = vector3_2;
      return true;
    }

    public Vector3 this[Vector3I pos]
    {
      get
      {
        Vector3 vector3;
        return this.Bones.TryGetValue(pos, out vector3) ? vector3 : Vector3.Zero;
      }
      set => this.Bones[pos] = value;
    }

    [Conditional("DEBUG")]
    private void AssertBone(Vector3 value, float range)
    {
    }

    public bool IsDeformed(
      Vector3I cube,
      float ignoredDeformation,
      MyCubeGrid cubeGrid,
      bool checkBlockDefinition)
    {
      float num1 = ignoredDeformation * ignoredDeformation;
      float maxBoneError = MyGridSkeleton.GetMaxBoneError(cubeGrid.GridSize);
      float num2 = maxBoneError * maxBoneError;
      foreach (Vector3I boneOffset in MyGridSkeleton.BoneOffsets)
      {
        Vector3 vector3;
        if (this.Bones.TryGetValue(cube * 2 + boneOffset, out vector3))
        {
          if (checkBlockDefinition)
          {
            if ((double) Math.Abs(this.GetDefinitionOffsetWithNeighbours(cube, cube * 2 + boneOffset, cubeGrid).LengthSquared() - vector3.LengthSquared()) > (double) num2)
              return true;
          }
          else if ((double) vector3.LengthSquared() > (double) num1)
            return true;
        }
      }
      return false;
    }

    public float MaxDeformation(Vector3I cube, MyCubeGrid cubeGrid)
    {
      float num1 = 0.0f;
      float maxBoneError = MyGridSkeleton.GetMaxBoneError(cubeGrid.GridSize);
      float num2 = maxBoneError * maxBoneError;
      foreach (Vector3I boneOffset in MyGridSkeleton.BoneOffsets)
      {
        Vector3I key = cube * 2 + boneOffset;
        Vector3 offset;
        int num3 = this.Bones.TryGetValue(key, out offset) ? 1 : 0;
        float num4 = Math.Abs(this.GetDefinitionOffsetWithNeighbours(cube, cube * 2 + boneOffset, cubeGrid).LengthSquared() - offset.LengthSquared());
        if ((double) num4 > (double) num1)
          num1 = num4;
        if (num3 == 0 && (double) num4 > (double) num2)
        {
          this.Bones.AddOrUpdate(key, offset, (Func<Vector3I, Vector3, Vector3>) ((k, v) => v = offset));
          cubeGrid.AddDirtyBone(cube, boneOffset);
        }
      }
      return (float) Math.Sqrt((double) num1);
    }

    public void Wrap(ref Vector3I cube, ref Vector3I boneOffset)
    {
      Vector3I vector3I = cube * 2 + boneOffset;
      cube = Vector3I.Floor((Vector3D) (vector3I / 2));
      boneOffset = vector3I - cube * 2;
    }

    public void GetAffectedCubes(
      Vector3I cube,
      Vector3I boneOffset,
      List<Vector3I> resultList,
      MyCubeGrid grid)
    {
      Vector3I vector3I1 = boneOffset - Vector3I.One;
      Vector3I vector3I2 = Vector3I.Sign(vector3I1);
      Vector3I vector3I3 = vector3I1 * vector3I2;
      Vector3I vector3I4;
      for (vector3I4.X = 0; vector3I4.X <= vector3I3.X; ++vector3I4.X)
      {
        for (vector3I4.Y = 0; vector3I4.Y <= vector3I3.Y; ++vector3I4.Y)
        {
          for (vector3I4.Z = 0; vector3I4.Z <= vector3I3.Z; ++vector3I4.Z)
          {
            Vector3I pos = cube + vector3I4 * vector3I2;
            if (grid.CubeExists(pos))
              resultList.Add(pos);
          }
        }
      }
    }

    public bool HasUnusedBones => this.m_tmpRemovedCubes.Count > 0;

    public void MarkCubeRemoved(ref Vector3I pos) => this.m_tmpRemovedCubes.Add(pos);

    public void RemoveUnusedBones(MyCubeGrid grid)
    {
      if (this.m_tmpRemovedCubes.Count == 0)
        return;
      foreach (Vector3I tmpRemovedCube in this.m_tmpRemovedCubes)
      {
        if (grid.CubeExists(tmpRemovedCube))
        {
          if (!this.m_testedCubes.Contains(tmpRemovedCube))
          {
            this.m_testedCubes.Add(tmpRemovedCube);
            this.AddUsedBones(tmpRemovedCube);
          }
        }
        else
        {
          Vector3I vector3I1 = tmpRemovedCube * 2 + Vector3I.One;
          for (int index1 = -1; index1 <= 1; ++index1)
          {
            for (int index2 = -1; index2 <= 1; ++index2)
            {
              for (int index3 = -1; index3 <= 1; ++index3)
              {
                Vector3I vector3I2;
                vector3I2.X = index1;
                vector3I2.Y = index2;
                vector3I2.Z = index3;
                Vector3I pos = tmpRemovedCube + vector3I2;
                if (grid.CubeExists(pos) && !this.m_testedCubes.Contains(pos))
                {
                  this.m_testedCubes.Add(pos);
                  this.AddUsedBones(pos);
                }
              }
            }
          }
        }
      }
      foreach (Vector3I tmpRemovedCube in this.m_tmpRemovedCubes)
      {
        Vector3I pos = tmpRemovedCube * 2;
        for (int index1 = 0; index1 <= 2; ++index1)
        {
          for (int index2 = 0; index2 <= 2; ++index2)
          {
            for (int index3 = 0; index3 <= 2; ++index3)
            {
              if (!this.m_usedBones.Contains(pos))
                this.ClearBone(ref pos);
              ++pos.Z;
            }
            ++pos.Y;
            pos.Z -= 3;
          }
          ++pos.X;
          pos.Y -= 3;
        }
      }
      this.m_testedCubes.Clear();
      this.m_usedBones.Clear();
      this.m_tmpRemovedCubes.Clear();
    }

    private void AddUsedBones(Vector3I pos)
    {
      pos *= 2;
      for (int index1 = 0; index1 <= 2; ++index1)
      {
        for (int index2 = 0; index2 <= 2; ++index2)
        {
          for (int index3 = 0; index3 <= 2; ++index3)
          {
            this.m_usedBones.Add(pos);
            ++pos.Z;
          }
          ++pos.Y;
          pos.Z -= 3;
        }
        ++pos.X;
        pos.Y -= 3;
      }
    }

    public Vector3 GetDefinitionOffsetWithNeighbours(
      Vector3I cubePos,
      Vector3I bonePos,
      MyCubeGrid grid)
    {
      Vector3I cubeBoneOffset1 = this.GetCubeBoneOffset(cubePos, bonePos);
      if (MyGridSkeleton.m_tempAffectedCubes == null)
        MyGridSkeleton.m_tempAffectedCubes = new List<Vector3I>();
      MyGridSkeleton.m_tempAffectedCubes.Clear();
      this.GetAffectedCubes(cubePos, cubeBoneOffset1, MyGridSkeleton.m_tempAffectedCubes, grid);
      Vector3 zero = Vector3.Zero;
      int num = 0;
      foreach (Vector3I tempAffectedCube in MyGridSkeleton.m_tempAffectedCubes)
      {
        MySlimBlock cubeBlock = grid.GetCubeBlock(tempAffectedCube);
        if (cubeBlock != null && cubeBlock.BlockDefinition.Skeleton != null)
        {
          Vector3I cubeBoneOffset2 = this.GetCubeBoneOffset(tempAffectedCube, bonePos);
          Vector3? definitionOffset = this.GetDefinitionOffset(cubeBlock, cubeBoneOffset2);
          if (definitionOffset.HasValue)
          {
            zero += definitionOffset.Value;
            ++num;
          }
        }
      }
      return num == 0 ? zero : zero / (float) num;
    }

    private Vector3I GetCubeBoneOffset(Vector3I cubePos, Vector3I boneOffset)
    {
      Vector3I zero = Vector3I.Zero;
      if (boneOffset.X % 2 != 0)
        zero.X = 1;
      else if (boneOffset.X / 2 != cubePos.X)
        zero.X = 2;
      if (boneOffset.Y % 2 != 0)
        zero.Y = 1;
      else if (boneOffset.Y / 2 != cubePos.Y)
        zero.Y = 2;
      if (boneOffset.Z % 2 != 0)
        zero.Z = 1;
      else if (boneOffset.Z / 2 != cubePos.Z)
        zero.Z = 2;
      return zero;
    }

    private Vector3? GetDefinitionOffset(MySlimBlock cubeBlock, Vector3I bonePos)
    {
      Vector3I position1 = bonePos - Vector3I.One;
      Matrix result1;
      cubeBlock.Orientation.GetMatrix(out result1);
      Matrix result2;
      Matrix.Transpose(ref result1, out result2);
      Vector3I result3;
      Vector3I.Transform(ref position1, ref result2, out result3);
      Vector3I key = result3 + Vector3I.One;
      Vector3 position2;
      if (!cubeBlock.BlockDefinition.Bones.TryGetValue(key, out position2))
        return new Vector3?();
      Vector3 result4;
      Vector3.Transform(ref position2, ref result1, out result4);
      return new Vector3?(result4);
    }
  }
}
