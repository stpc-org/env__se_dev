// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyCompositeShapeProviderBase
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using VRage.Game;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  internal abstract class MyCompositeShapeProviderBase : IMyStorageDataProvider
  {
    protected IMyCompositionInfoProvider m_infoProvider;
    [ThreadStatic]
    private static List<IMyCompositeDeposit> m_overlappedDeposits;
    [ThreadStatic]
    private static List<IMyCompositeShape> m_overlappedFilledShapes;
    [ThreadStatic]
    private static List<IMyCompositeShape> m_overlappedRemovedShapes;
    [ThreadStatic]
    private static MyStorageData m_storageCache;

    public abstract int SerializedSize { get; }

    public abstract void WriteTo(Stream stream);

    public abstract void ReadFrom(
      int storageVersion,
      Stream stream,
      int size,
      ref bool isOldFormat);

    bool IMyStorageDataProvider.Intersect(
      ref LineD line,
      out double startOffset,
      out double endOffset)
    {
      startOffset = 0.0;
      endOffset = 0.0;
      return true;
    }

    public ContainmentType Intersect(BoundingBoxI box, int lod) => MyCompositeShapeProviderBase.Intersect(this.m_infoProvider, box, lod);

    public static ContainmentType Intersect(
      IMyCompositionInfoProvider infoProvider,
      BoundingBoxI box,
      int lod)
    {
      ContainmentType containmentType1 = ContainmentType.Disjoint;
      BoundingBox queryBox = new BoundingBox(box);
      BoundingSphere querySphere = new BoundingSphere(queryBox.Center, queryBox.Extents.Length() / 2f);
      foreach (IMyCompositeShape filledShape in infoProvider.FilledShapes)
      {
        ContainmentType containmentType2 = filledShape.Contains(ref queryBox, ref querySphere, 1);
        switch (containmentType2)
        {
          case ContainmentType.Contains:
            containmentType1 = containmentType2;
            goto label_6;
          case ContainmentType.Intersects:
            containmentType1 = ContainmentType.Intersects;
            break;
        }
      }
label_6:
      if (containmentType1 != ContainmentType.Disjoint)
      {
        foreach (IMyCompositeShape removedShape in infoProvider.RemovedShapes)
        {
          switch (removedShape.Contains(ref queryBox, ref querySphere, 1))
          {
            case ContainmentType.Contains:
              containmentType1 = ContainmentType.Disjoint;
              goto label_13;
            case ContainmentType.Intersects:
              containmentType1 = ContainmentType.Intersects;
              break;
          }
        }
      }
label_13:
      return containmentType1;
    }

    void IMyStorageDataProvider.ReadRange(
      ref MyVoxelDataRequest req,
      bool detectOnly = false)
    {
      req.Flags = !req.RequestedData.Requests(MyStorageDataTypeEnum.Content) ? this.ReadMaterialRange(req.Target, ref req.Offset, req.Lod, ref req.MinInLod, ref req.MaxInLod, detectOnly, (req.RequestFlags & MyVoxelRequestFlags.ConsiderContent) > (MyVoxelRequestFlags) 0) : this.ReadContentRange(req.Target, ref req.Offset, req.Lod, ref req.MinInLod, ref req.MaxInLod, detectOnly);
      req.Flags |= req.RequestFlags & MyVoxelRequestFlags.RequestFlags;
    }

    void IMyStorageDataProvider.ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataType,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod)
    {
      if (dataType.Requests(MyStorageDataTypeEnum.Content))
      {
        int num1 = (int) this.ReadContentRange(target, ref writeOffset, lodIndex, ref minInLod, ref maxInLod, false);
      }
      else
      {
        int num2 = (int) this.ReadMaterialRange(target, ref writeOffset, lodIndex, ref minInLod, ref maxInLod, false, false);
      }
    }

    public virtual void DebugDraw(ref MatrixD worldMatrix)
    {
      Color green = Color.Green;
      Color red = Color.Red;
      Color cornflowerBlue = Color.CornflowerBlue;
      foreach (IMyCompositeShape filledShape in this.m_infoProvider.FilledShapes)
        filledShape.DebugDraw(ref worldMatrix, green);
      foreach (IMyCompositeShape removedShape in this.m_infoProvider.RemovedShapes)
        removedShape.DebugDraw(ref worldMatrix, red);
      foreach (IMyCompositeShape deposit in this.m_infoProvider.Deposits)
        deposit.DebugDraw(ref worldMatrix, cornflowerBlue);
    }

    void IMyStorageDataProvider.ReindexMaterials(
      Dictionary<byte, byte> oldToNewIndexMap)
    {
    }

    void IMyStorageDataProvider.PostProcess(
      VrVoxelMesh mesh,
      MyStorageDataTypeFlags dataTypes)
    {
    }

    void IMyStorageDataProvider.Close()
    {
      foreach (IMyCompositeShape myCompositeShape in ((IEnumerable<IMyCompositeShape>) this.m_infoProvider.Deposits).Concat<IMyCompositeShape>((IEnumerable<IMyCompositeShape>) this.m_infoProvider.FilledShapes).Concat<IMyCompositeShape>((IEnumerable<IMyCompositeShape>) this.m_infoProvider.RemovedShapes))
        myCompositeShape.Close();
      this.m_infoProvider.Close();
      this.m_infoProvider = (IMyCompositionInfoProvider) null;
    }

    private static MyStorageData GetTempStorage(ref Vector3I min, ref Vector3I max)
    {
      MyStorageData myStorageData = MyCompositeShapeProviderBase.m_storageCache;
      if (myStorageData == null)
        MyCompositeShapeProviderBase.m_storageCache = myStorageData = new MyStorageData(MyStorageDataTypeFlags.Content);
      myStorageData.Resize(min, max);
      return myStorageData;
    }

    internal MyVoxelRequestFlags ReadContentRange(
      MyStorageData target,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      bool detectOnly)
    {
      int lodVoxelSize;
      BoundingBox queryBox;
      BoundingSphere querySphere;
      MyCompositeShapeProviderBase.SetupReading(lodIndex, ref minInLod, ref maxInLod, out lodVoxelSize, out queryBox, out querySphere);
      using (MyUtils.ReuseCollection<IMyCompositeShape>(ref MyCompositeShapeProviderBase.m_overlappedFilledShapes))
      {
        using (MyUtils.ReuseCollection<IMyCompositeShape>(ref MyCompositeShapeProviderBase.m_overlappedRemovedShapes))
        {
          List<IMyCompositeShape> overlappedFilledShapes = MyCompositeShapeProviderBase.m_overlappedFilledShapes;
          List<IMyCompositeShape> overlappedRemovedShapes = MyCompositeShapeProviderBase.m_overlappedRemovedShapes;
          ContainmentType containmentType1 = ContainmentType.Disjoint;
          foreach (IMyCompositeShape removedShape in this.m_infoProvider.RemovedShapes)
          {
            switch (removedShape.Contains(ref queryBox, ref querySphere, lodVoxelSize))
            {
              case ContainmentType.Contains:
                containmentType1 = ContainmentType.Contains;
                goto label_8;
              case ContainmentType.Intersects:
                containmentType1 = ContainmentType.Intersects;
                overlappedRemovedShapes.Add(removedShape);
                break;
            }
          }
label_8:
          if (containmentType1 == ContainmentType.Contains)
          {
            if (!detectOnly)
              target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), (byte) 0);
            return MyVoxelRequestFlags.EmptyData;
          }
          ContainmentType containmentType2 = ContainmentType.Disjoint;
          foreach (IMyCompositeShape filledShape in this.m_infoProvider.FilledShapes)
          {
            switch (filledShape.Contains(ref queryBox, ref querySphere, lodVoxelSize))
            {
              case ContainmentType.Contains:
                overlappedFilledShapes.Clear();
                containmentType2 = ContainmentType.Contains;
                goto label_18;
              case ContainmentType.Intersects:
                overlappedFilledShapes.Add(filledShape);
                containmentType2 = ContainmentType.Intersects;
                break;
            }
          }
label_18:
          if (containmentType2 == ContainmentType.Disjoint)
          {
            if (!detectOnly)
              target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), (byte) 0);
            return MyVoxelRequestFlags.EmptyData;
          }
          if (containmentType1 == ContainmentType.Disjoint && containmentType2 == ContainmentType.Contains)
          {
            if (!detectOnly)
              target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), byte.MaxValue);
            return MyVoxelRequestFlags.FullContent;
          }
          if (detectOnly)
            return (MyVoxelRequestFlags) 0;
          MyStorageData tempStorage = MyCompositeShapeProviderBase.GetTempStorage(ref minInLod, ref maxInLod);
          bool flag = containmentType2 == ContainmentType.Contains;
          target.BlockFillContent(writeOffset, writeOffset + (maxInLod - minInLod), flag ? byte.MaxValue : (byte) 0);
          if (!flag)
          {
            foreach (IMyCompositeShape myCompositeShape in overlappedFilledShapes)
            {
              myCompositeShape.ComputeContent(tempStorage, lodIndex, minInLod, maxInLod, lodVoxelSize);
              target.OpRange<MyCompositeShapeProviderBase.MaxOp>(tempStorage, Vector3I.Zero, maxInLod - minInLod, writeOffset, MyStorageDataTypeEnum.Content);
            }
          }
          if (containmentType1 != ContainmentType.Disjoint)
          {
            foreach (IMyCompositeShape myCompositeShape in overlappedRemovedShapes)
            {
              myCompositeShape.ComputeContent(tempStorage, lodIndex, minInLod, maxInLod, lodVoxelSize);
              target.OpRange<MyCompositeShapeProviderBase.DiffOp>(tempStorage, Vector3I.Zero, maxInLod - minInLod, writeOffset, MyStorageDataTypeEnum.Content);
            }
          }
        }
      }
      return (MyVoxelRequestFlags) 0;
    }

    internal virtual MyVoxelRequestFlags ReadMaterialRange(
      MyStorageData target,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      bool detectOnly,
      bool considerContent)
    {
      int lodVoxelSize;
      BoundingBox queryBox;
      BoundingSphere querySphere;
      MyCompositeShapeProviderBase.SetupReading(lodIndex, ref minInLod, ref maxInLod, out lodVoxelSize, out queryBox, out querySphere);
      using (MyUtils.ReuseCollection<IMyCompositeDeposit>(ref MyCompositeShapeProviderBase.m_overlappedDeposits))
      {
        List<IMyCompositeDeposit> overlappedDeposits = MyCompositeShapeProviderBase.m_overlappedDeposits;
        MyVoxelMaterialDefinition defaultMaterial = this.m_infoProvider.DefaultMaterial;
        ContainmentType containmentType = ContainmentType.Disjoint;
        foreach (IMyCompositeDeposit deposit in this.m_infoProvider.Deposits)
        {
          if (deposit.Contains(ref queryBox, ref querySphere, lodVoxelSize) != ContainmentType.Disjoint)
          {
            overlappedDeposits.Add(deposit);
            containmentType = ContainmentType.Intersects;
          }
        }
        if (containmentType == ContainmentType.Disjoint)
        {
          if (!detectOnly)
          {
            if (considerContent)
              target.BlockFillMaterialConsiderContent(writeOffset, writeOffset + (maxInLod - minInLod), defaultMaterial.Index);
            else
              target.BlockFillMaterial(writeOffset, writeOffset + (maxInLod - minInLod), defaultMaterial.Index);
          }
          return MyVoxelRequestFlags.EmptyData;
        }
        if (detectOnly)
          return (MyVoxelRequestFlags) 0;
        Vector3I vector3I;
        for (vector3I.Z = minInLod.Z; vector3I.Z <= maxInLod.Z; ++vector3I.Z)
        {
          for (vector3I.Y = minInLod.Y; vector3I.Y <= maxInLod.Y; ++vector3I.Y)
          {
            for (vector3I.X = minInLod.X; vector3I.X <= maxInLod.X; ++vector3I.X)
            {
              Vector3I p = vector3I - minInLod + writeOffset;
              if (considerContent && target.Content(ref p) == (byte) 0)
              {
                target.Material(ref p, byte.MaxValue);
              }
              else
              {
                Vector3 localPos = (Vector3) (vector3I * lodVoxelSize);
                float num1 = 1f;
                byte materialIdx = defaultMaterial.Index;
                if (!MyFakes.DISABLE_COMPOSITE_MATERIAL)
                {
                  foreach (IMyCompositeDeposit compositeDeposit in overlappedDeposits)
                  {
                    float num2 = compositeDeposit.SignedDistance(ref localPos, 1);
                    if ((double) num2 < 0.0 && (double) num2 <= (double) num1)
                    {
                      num1 = num2;
                      MyVoxelMaterialDefinition materialForPosition = compositeDeposit.GetMaterialForPosition(ref localPos, (float) lodVoxelSize);
                      materialIdx = materialForPosition == null ? defaultMaterial.Index : materialForPosition.Index;
                    }
                  }
                }
                target.Material(ref p, materialIdx);
              }
            }
          }
        }
      }
      return (MyVoxelRequestFlags) 0;
    }

    protected static void SetupReading(
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      out int lodVoxelSize,
      out BoundingBox queryBox,
      out BoundingSphere querySphere)
    {
      float num = 0.5f * (float) (1 << lodIndex);
      lodVoxelSize = (int) ((double) num * 2.0);
      Vector3I voxelCoord1 = minInLod << lodIndex;
      Vector3I voxelCoord2 = maxInLod << lodIndex;
      Vector3 localPosition;
      MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord1, out localPosition);
      Vector3 vector3_1 = localPosition;
      MyVoxelCoordSystems.VoxelCoordToLocalPosition(ref voxelCoord2, out localPosition);
      Vector3 vector3_2 = localPosition;
      Vector3 min = vector3_1 - num;
      Vector3 max = vector3_2 + num;
      queryBox = new BoundingBox(min, max);
      BoundingSphere.CreateFromBoundingBox(ref queryBox, out querySphere);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static float ContentToSignedDistance(byte content) => (float) (((double) content / (double) byte.MaxValue - 0.5) * -2.0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static byte SignedDistanceToContent(float signedDistance)
    {
      signedDistance = MathHelper.Clamp(signedDistance, -1f, 1f);
      return (byte) (((double) signedDistance / -2.0 + 0.5) * (double) byte.MaxValue);
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct MaxOp : MyStorageData.IOperator
    {
      public void Op(ref byte a, byte b) => a = Math.Max(a, b);
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct DiffOp : MyStorageData.IOperator
    {
      public void Op(ref byte a, byte b) => a = (byte) Math.Min((int) a, (int) byte.MaxValue - (int) b);
    }
  }
}
