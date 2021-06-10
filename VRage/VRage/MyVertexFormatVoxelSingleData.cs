// Decompiled with JetBrains decompiler
// Type: VRage.MyVertexFormatVoxelSingleData
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System.Runtime.InteropServices;
using VRage.Import;
using VRageMath;
using VRageMath.PackedVector;

namespace VRage
{
  [StructLayout(LayoutKind.Explicit)]
  public struct MyVertexFormatVoxelSingleData
  {
    [FieldOffset(0)]
    public Vector3 Position;
    [FieldOffset(12)]
    public Byte4 Material;
    [FieldOffset(16)]
    public Byte4 PackedNormal;
    [FieldOffset(20)]
    public uint PackedColorShift;

    public Vector3 Normal
    {
      get => VF_Packer.UnpackNormal(ref this.PackedNormal);
      set => this.PackedNormal.PackedValue = VF_Packer.PackNormal(ref value);
    }
  }
}
