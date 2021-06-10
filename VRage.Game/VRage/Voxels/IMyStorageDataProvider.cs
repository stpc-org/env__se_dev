// Decompiled with JetBrains decompiler
// Type: VRage.Voxels.IMyStorageDataProvider
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.IO;
using VRageMath;

namespace VRage.Voxels
{
  public interface IMyStorageDataProvider
  {
    int SerializedSize { get; }

    void WriteTo(Stream stream);

    void ReadFrom(int storageVersion, Stream stream, int size, ref bool isOldFormat);

    void ReadRange(
      MyStorageData target,
      MyStorageDataTypeFlags dataType,
      ref Vector3I writeOffset,
      int lodIndex,
      ref Vector3I minInLod,
      ref Vector3I maxInLod);

    void ReadRange(ref MyVoxelDataRequest request, bool detectOnly = false);

    void DebugDraw(ref MatrixD worldMatrix);

    void ReindexMaterials(Dictionary<byte, byte> oldToNewIndexMap);

    ContainmentType Intersect(BoundingBoxI box, int lod);

    bool Intersect(ref LineD line, out double startOffset, out double endOffset);

    void Close();

    void PostProcess(VrVoxelMesh mesh, MyStorageDataTypeFlags dataTypes);
  }
}
