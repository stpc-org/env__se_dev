// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.World.Generator.MyPredefinedDataProvider
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Voxels;
using System;
using System.IO;
using VRage.FileSystem;
using VRage.Game;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
  [MyStorageDataProvider(10102)]
  internal sealed class MyPredefinedDataProvider : MyCompositeShapeProviderBase, IMyStorageDataProvider
  {
    private const int CURRENT_VERSION = 0;
    private string m_storageName;
    private string m_voxelMaterial = "";
    private MyStorageBase m_storageBase;

    public override int SerializedSize => 4 + this.m_storageName.Get7bitEncodedSize() + this.m_voxelMaterial.Get7bitEncodedSize() + 4;

    public MyStorageBase Storage => this.m_storageBase;

    private void Init(string storageName, string voxelMaterial)
    {
      this.m_storageName = storageName;
      this.m_voxelMaterial = string.IsNullOrEmpty(voxelMaterial) ? "" : voxelMaterial;
      MyVoxelMapStorageDefinition definition;
      if (MyDefinitionManager.Static.TryGetVoxelMapStorageDefinition(storageName, out definition))
        this.m_storageBase = MyStorageBase.Load(Path.Combine(definition.Context.IsBaseGame ? MyFileSystem.ContentPath : definition.Context.ModPath, definition.StorageFile), local: true);
      this.m_infoProvider = (IMyCompositionInfoProvider) new MyPredefinedDataProvider.SingleShapeProvider((IMyCompositeShape) this.m_storageBase, (IMyCompositeDeposit) new MyPredefinedDataProvider.SingleDeposit(this.m_storageBase));
    }

    public override void WriteTo(Stream stream)
    {
      stream.WriteNoAlloc(0);
      stream.WriteNoAlloc(this.m_storageName);
      stream.WriteNoAlloc(this.m_voxelMaterial);
    }

    public override void ReadFrom(
      int storageVersion,
      Stream stream,
      int size,
      ref bool isOldFormat)
    {
      stream.ReadInt32();
      this.Init(stream.ReadString(), stream.ReadString());
    }

    public static MyPredefinedDataProvider CreatePredefinedShape(
      string storageName,
      string voxelMaterial)
    {
      MyPredefinedDataProvider predefinedDataProvider = new MyPredefinedDataProvider();
      predefinedDataProvider.Init(storageName, voxelMaterial);
      return predefinedDataProvider;
    }

    internal override MyVoxelRequestFlags ReadMaterialRange(
      MyStorageData target,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod,
      bool detectOnly,
      bool considerContent)
    {
      if (string.IsNullOrEmpty(this.m_voxelMaterial))
        return base.ReadMaterialRange(target, ref writeOffset, lodIndex, ref minInLod, ref maxInLod, detectOnly, considerContent);
      MyVoxelMaterialDefinition definition = (MyVoxelMaterialDefinition) null;
      if (!string.IsNullOrEmpty(this.m_voxelMaterial))
      {
        MyDefinitionManager.Static.TryGetVoxelMaterialDefinition(this.m_voxelMaterial, out definition);
        target.BlockFillMaterialConsiderContent(writeOffset, writeOffset + (maxInLod - minInLod), definition.Index);
      }
      return (MyVoxelRequestFlags) 0;
    }

    private class SingleShapeProvider : IMyCompositionInfoProvider
    {
      public IMyCompositeShape[] FilledShapes { get; }

      public IMyCompositeDeposit[] Deposits { get; }

      public IMyCompositeShape[] RemovedShapes => Array.Empty<IMyCompositeShape>();

      public MyVoxelMaterialDefinition DefaultMaterial => MyDefinitionManager.Static.GetVoxelMaterialDefinition("Stone");

      public SingleShapeProvider(IMyCompositeShape shape, IMyCompositeDeposit deposit)
      {
        this.FilledShapes = new IMyCompositeShape[1]
        {
          shape
        };
        this.Deposits = new IMyCompositeDeposit[1]
        {
          deposit
        };
      }

      public void Close()
      {
      }
    }

    private class SingleDeposit : IMyCompositeDeposit, IMyCompositeShape
    {
      private MyStorageBase m_storageBase;

      public SingleDeposit(MyStorageBase storage) => this.m_storageBase = storage;

      public ContainmentType Contains(
        ref BoundingBox queryBox,
        ref BoundingSphere querySphere,
        int lodVoxelSize)
      {
        return ContainmentType.Contains;
      }

      public float SignedDistance(ref Vector3 localPos, int lodVoxelSize) => -1f;

      public void ComputeContent(
        MyStorageData storage,
        int lodIndex,
        Vector3I lodVoxelRangeMin,
        Vector3I lodVoxelRangeMax,
        int lodVoxelSize)
      {
        this.m_storageBase.ReadRange(storage, MyStorageDataTypeFlags.Material, lodIndex, lodVoxelRangeMin, lodVoxelRangeMax);
      }

      public void DebugDraw(ref MatrixD worldMatrix, Color color)
      {
      }

      public void Close()
      {
      }

      public MyVoxelMaterialDefinition GetMaterialForPosition(
        ref Vector3 localPos,
        float lodVoxelSize)
      {
        Vector3D localCoords = (Vector3D) localPos;
        return this.m_storageBase.GetMaterialAt(ref localCoords);
      }
    }
  }
}
