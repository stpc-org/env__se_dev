// Decompiled with JetBrains decompiler
// Type: VRage.MyInstanceData
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using VRageMath;
using VRageMath.PackedVector;

namespace VRage
{
  public struct MyInstanceData
  {
    public HalfVector4 m_row0;
    public HalfVector4 m_row1;
    public HalfVector4 m_row2;
    public HalfVector4 ColorMaskHSV;

    public MyInstanceData(Matrix m)
    {
      this.m_row0 = new HalfVector4(m.M11, m.M21, m.M31, m.M41);
      this.m_row1 = new HalfVector4(m.M12, m.M22, m.M32, m.M42);
      this.m_row2 = new HalfVector4(m.M13, m.M23, m.M33, m.M43);
      this.ColorMaskHSV = new HalfVector4();
    }

    public Matrix LocalMatrix
    {
      get
      {
        Vector4 vector4_1 = this.m_row0.ToVector4();
        Vector4 vector4_2 = this.m_row1.ToVector4();
        Vector4 vector4_3 = this.m_row2.ToVector4();
        return new Matrix(vector4_1.X, vector4_2.X, vector4_3.X, 0.0f, vector4_1.Y, vector4_2.Y, vector4_3.Y, 0.0f, vector4_1.Z, vector4_2.Z, vector4_3.Z, 0.0f, vector4_1.W, vector4_2.W, vector4_3.W, 1f);
      }
      set
      {
        this.m_row0 = new HalfVector4(value.M11, value.M21, value.M31, value.M41);
        this.m_row1 = new HalfVector4(value.M12, value.M22, value.M32, value.M42);
        this.m_row2 = new HalfVector4(value.M13, value.M23, value.M33, value.M43);
      }
    }

    public Vector3 Translation
    {
      get => new Vector3(HalfUtils.Unpack((ushort) (this.m_row0.PackedValue >> 48)), HalfUtils.Unpack((ushort) (this.m_row1.PackedValue >> 48)), HalfUtils.Unpack((ushort) (this.m_row2.PackedValue >> 48)));
      set
      {
        this.m_row0.PackedValue = (ulong) ((long) this.m_row0.PackedValue & 281474976710655L | (long) HalfUtils.Pack(value.X) << 48);
        this.m_row1.PackedValue = (ulong) ((long) this.m_row1.PackedValue & 281474976710655L | (long) HalfUtils.Pack(value.Y) << 48);
        this.m_row2.PackedValue = (ulong) ((long) this.m_row2.PackedValue & 281474976710655L | (long) HalfUtils.Pack(value.Z) << 48);
      }
    }
  }
}
