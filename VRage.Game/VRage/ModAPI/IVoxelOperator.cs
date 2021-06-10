// Decompiled with JetBrains decompiler
// Type: VRage.ModAPI.IVoxelOperator
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using VRage.Voxels;
using VRageMath;

namespace VRage.ModAPI
{
  public interface IVoxelOperator
  {
    VoxelOperatorFlags Flags { get; }

    void Op(ref Vector3I position, MyStorageDataTypeEnum dataType, ref byte inOutContent);
  }
}
