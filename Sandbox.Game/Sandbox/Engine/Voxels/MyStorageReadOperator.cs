﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Voxels.MyStorageReadOperator
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using VRage.ModAPI;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Engine.Voxels
{
  public struct MyStorageReadOperator : IVoxelOperator
  {
    private readonly MyStorageData m_data;

    public VoxelOperatorFlags Flags => VoxelOperatorFlags.Read;

    public MyStorageReadOperator(MyStorageData data) => this.m_data = data;

    public void Op(ref Vector3I position, MyStorageDataTypeEnum dataType, ref byte inData) => this.m_data.Set(dataType, ref position, inData);
  }
}
