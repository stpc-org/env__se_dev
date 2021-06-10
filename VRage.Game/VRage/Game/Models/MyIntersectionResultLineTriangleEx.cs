// Decompiled with JetBrains decompiler
// Type: VRage.Game.Models.MyIntersectionResultLineTriangleEx
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.Models
{
  public struct MyIntersectionResultLineTriangleEx
  {
    public MyIntersectionResultLineTriangle Triangle;
    public Vector3 IntersectionPointInObjectSpace;
    public Vector3D IntersectionPointInWorldSpace;
    public IMyEntity Entity;
    public object UserObject;
    public Vector3 NormalInWorldSpace;
    public Vector3 NormalInObjectSpace;
    public Line InputLineInObjectSpace;

    public MyIntersectionResultLineTriangleEx(
      MyIntersectionResultLineTriangle triangle,
      IMyEntity entity,
      ref Line line)
    {
      this.Triangle = triangle;
      this.Entity = entity;
      this.InputLineInObjectSpace = line;
      this.UserObject = (object) null;
      this.NormalInObjectSpace = MyUtils.GetNormalVectorFromTriangle(ref this.Triangle.InputTriangle);
      if (!this.NormalInObjectSpace.IsValid())
        this.NormalInObjectSpace = new Vector3(0.0f, 0.0f, 1f);
      this.IntersectionPointInObjectSpace = line.From + line.Direction * this.Triangle.Distance;
      if (this.Entity is IMyVoxelBase)
      {
        this.IntersectionPointInWorldSpace = (Vector3D) this.IntersectionPointInObjectSpace + ((IMyVoxelBase) this.Entity).PositionLeftBottomCorner;
        this.NormalInWorldSpace = this.NormalInObjectSpace;
      }
      else
      {
        MatrixD worldMatrix = this.Entity.WorldMatrix;
        this.NormalInWorldSpace = (Vector3) MyUtils.GetTransformNormalNormalized((Vector3D) this.NormalInObjectSpace, ref worldMatrix);
        this.IntersectionPointInWorldSpace = Vector3D.Transform((Vector3D) this.IntersectionPointInObjectSpace, ref worldMatrix);
      }
    }

    public MyIntersectionResultLineTriangleEx(
      MyIntersectionResultLineTriangle triangle,
      IMyEntity entity,
      ref Line line,
      Vector3D intersectionPointInWorldSpace,
      Vector3 normalInWorldSpace)
    {
      this.Triangle = triangle;
      this.Entity = entity;
      this.InputLineInObjectSpace = line;
      this.UserObject = (object) null;
      this.NormalInObjectSpace = this.NormalInWorldSpace = normalInWorldSpace;
      this.IntersectionPointInWorldSpace = intersectionPointInWorldSpace;
      this.IntersectionPointInObjectSpace = (Vector3) this.IntersectionPointInWorldSpace;
    }

    public VertexBoneIndicesWeights? GetAffectingBoneIndicesWeights(
      ref List<VertexArealBoneIndexWeight> tmpStorage)
    {
      if (!this.Triangle.BoneWeights.HasValue)
        return new VertexBoneIndicesWeights?();
      if (tmpStorage == null)
        tmpStorage = new List<VertexArealBoneIndexWeight>(4);
      tmpStorage.Clear();
      MyTriangle_BoneIndicesWeigths boneIndicesWeigths = this.Triangle.BoneWeights.Value;
      float u;
      float v;
      float w;
      Vector3.Barycentric(this.IntersectionPointInObjectSpace, this.Triangle.InputTriangle.Vertex0, this.Triangle.InputTriangle.Vertex1, this.Triangle.InputTriangle.Vertex2, out u, out v, out w);
      this.FillIndicesWeightsStorage(tmpStorage, ref boneIndicesWeigths.Vertex0, u);
      this.FillIndicesWeightsStorage(tmpStorage, ref boneIndicesWeigths.Vertex1, v);
      this.FillIndicesWeightsStorage(tmpStorage, ref boneIndicesWeigths.Vertex2, w);
      tmpStorage.Sort(new System.Comparison<VertexArealBoneIndexWeight>(this.Comparison));
      VertexBoneIndicesWeights indicesWeights = new VertexBoneIndicesWeights();
      this.FillIndicesWeights(ref indicesWeights, 0, tmpStorage);
      this.FillIndicesWeights(ref indicesWeights, 1, tmpStorage);
      this.FillIndicesWeights(ref indicesWeights, 2, tmpStorage);
      this.FillIndicesWeights(ref indicesWeights, 3, tmpStorage);
      this.NormalizeBoneWeights(ref indicesWeights);
      return new VertexBoneIndicesWeights?(indicesWeights);
    }

    private int Comparison(VertexArealBoneIndexWeight x, VertexArealBoneIndexWeight y)
    {
      if ((double) x.Weight > (double) y.Weight)
        return -1;
      return (double) x.Weight == (double) y.Weight ? 0 : 1;
    }

    private void FillIndicesWeights(
      ref VertexBoneIndicesWeights indicesWeights,
      int index,
      List<VertexArealBoneIndexWeight> tmpStorage)
    {
      if (index >= tmpStorage.Count)
        return;
      indicesWeights.Indices[index] = tmpStorage[index].Index;
      indicesWeights.Weights[index] = tmpStorage[index].Weight;
    }

    private void FillIndicesWeightsStorage(
      List<VertexArealBoneIndexWeight> tmpStorage,
      ref MyVertex_BoneIndicesWeights indicesWeights,
      float arealCoord)
    {
      this.HandleAddBoneIndexWeight(tmpStorage, ref indicesWeights, 0, arealCoord);
      this.HandleAddBoneIndexWeight(tmpStorage, ref indicesWeights, 1, arealCoord);
      this.HandleAddBoneIndexWeight(tmpStorage, ref indicesWeights, 2, arealCoord);
      this.HandleAddBoneIndexWeight(tmpStorage, ref indicesWeights, 3, arealCoord);
    }

    private void HandleAddBoneIndexWeight(
      List<VertexArealBoneIndexWeight> tmpStorage,
      ref MyVertex_BoneIndicesWeights indicesWeights,
      int index,
      float arealCoord)
    {
      float weight = indicesWeights.Weights[index];
      if ((double) weight == 0.0)
        return;
      byte index1 = indicesWeights.Indices[index];
      int exsistingBoneIndexWeight = this.FindExsistingBoneIndexWeight(tmpStorage, (int) index1);
      if (exsistingBoneIndexWeight == -1)
      {
        tmpStorage.Add(new VertexArealBoneIndexWeight()
        {
          Index = index1,
          Weight = weight * arealCoord
        });
      }
      else
      {
        VertexArealBoneIndexWeight arealBoneIndexWeight = tmpStorage[exsistingBoneIndexWeight];
        arealBoneIndexWeight.Weight += weight * arealCoord;
        tmpStorage[exsistingBoneIndexWeight] = arealBoneIndexWeight;
      }
    }

    private int FindExsistingBoneIndexWeight(
      List<VertexArealBoneIndexWeight> tmpStorage,
      int boneIndex)
    {
      int num = -1;
      for (int index = 0; index < tmpStorage.Count; ++index)
      {
        if ((int) tmpStorage[index].Index == boneIndex)
        {
          num = index;
          break;
        }
      }
      return num;
    }

    private void NormalizeBoneWeights(ref VertexBoneIndicesWeights indicesWeights)
    {
      float num = 0.0f;
      for (int index = 0; index < 4; ++index)
        num += indicesWeights.Weights[index];
      for (int index = 0; index < 4; ++index)
        indicesWeights.Weights[index] /= num;
    }

    public static MyIntersectionResultLineTriangleEx? GetCloserIntersection(
      ref MyIntersectionResultLineTriangleEx? a,
      ref MyIntersectionResultLineTriangleEx? b)
    {
      return !a.HasValue && b.HasValue || a.HasValue && b.HasValue && (double) b.Value.Triangle.Distance < (double) a.Value.Triangle.Distance ? b : a;
    }

    public static bool IsDistanceLessThanTolerance(
      ref MyIntersectionResultLineTriangleEx? a,
      ref MyIntersectionResultLineTriangleEx? b,
      float distanceTolerance)
    {
      return !a.HasValue && b.HasValue || a.HasValue && b.HasValue && (double) Math.Abs(b.Value.Triangle.Distance - a.Value.Triangle.Distance) <= (double) distanceTolerance;
    }
  }
}
