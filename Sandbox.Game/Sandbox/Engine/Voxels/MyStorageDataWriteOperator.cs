// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyStorageDataWriteOperator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public struct MyStorageDataWriteOperator : IVoxelOperator
  {
    private readonly MyStorageData m_data;

    public VoxelOperatorFlags Flags => VoxelOperatorFlags.WriteAll;

    public MyStorageDataWriteOperator(MyStorageData data) => this.m_data = data;

    public void Op(ref Vector3I position, MyStorageDataTypeEnum dataType, ref byte outData) => outData = this.m_data.Get(dataType, ref position);
  }
}
