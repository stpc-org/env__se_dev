// Decompiled with JetBrains decompiler
// Type: VRage.MyCubeInstanceData
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using System;
using VRageMath;

namespace VRage
{
  public struct MyCubeInstanceData
  {
    private unsafe fixed byte m_bones[32];
    public Vector4 m_translationAndRot;
    public Vector4 ColorMaskHSV;

    public Matrix ConstructDeformedCubeInstanceMatrix(
      ref Vector4UByte boneIndices,
      ref Vector4 boneWeights,
      out Matrix localMatrix)
    {
      localMatrix = this.LocalMatrix;
      Matrix matrix = localMatrix;
      if (this.EnableSkinning)
      {
        Vector3 boneOffset = this.ComputeBoneOffset(ref boneIndices, ref boneWeights);
        Vector3 vector3 = matrix.Translation + boneOffset;
        matrix.Translation = vector3;
      }
      return matrix;
    }

    public Vector3 ComputeBoneOffset(ref Vector4UByte boneIndices, ref Vector4 boneWeights)
    {
      Matrix matrix = new Matrix();
      Vector4 normalizedBone1 = this.GetNormalizedBone((int) boneIndices[0]);
      Vector4 normalizedBone2 = this.GetNormalizedBone((int) boneIndices[1]);
      Vector4 normalizedBone3 = this.GetNormalizedBone((int) boneIndices[2]);
      Vector4 normalizedBone4 = this.GetNormalizedBone((int) boneIndices[3]);
      matrix.SetRow(0, normalizedBone1);
      matrix.SetRow(1, normalizedBone2);
      matrix.SetRow(2, normalizedBone3);
      matrix.SetRow(3, normalizedBone4);
      return this.Denormalize(Vector4.Transform(boneWeights, matrix), this.BoneRange);
    }

    public unsafe void RetrieveBones(byte* bones)
    {
      fixed (byte* numPtr = this.m_bones)
      {
        for (int index = 0; index < 32; ++index)
          bones[index] = numPtr[index];
      }
    }

    public Matrix LocalMatrix
    {
      get
      {
        Matrix matrix;
        Vector4.UnpackOrthoMatrix(ref this.m_translationAndRot, out matrix);
        return matrix;
      }
      set => this.m_translationAndRot = Vector4.PackOrthoMatrix(ref value);
    }

    public Vector3 Translation => new Vector3(this.m_translationAndRot);

    public Vector4 PackedOrthoMatrix
    {
      get => this.m_translationAndRot;
      set => this.m_translationAndRot = value;
    }

    public unsafe void ResetBones()
    {
      fixed (byte* numPtr = this.m_bones)
      {
        *(ulong*) numPtr = 9259542123273814144UL;
        ((ulong*) numPtr)[1] = 36170086419038336UL;
        ((ulong*) numPtr)[2] = 9259542123273814144UL;
        ((ulong*) numPtr)[3] = 36170086419038336UL;
      }
    }

    public unsafe void SetTextureOffset(Vector4UByte patternOffset)
    {
      fixed (byte* numPtr = this.m_bones)
      {
        ((Vector4UByte*) (numPtr + (new IntPtr(5) * sizeof (Vector4UByte)).ToInt64()))->W = patternOffset.X;
        ((Vector4UByte*) (numPtr + (new IntPtr(6) * sizeof (Vector4UByte)).ToInt64()))->W = patternOffset.Y;
        ((Vector4UByte*) (numPtr + (new IntPtr(7) * sizeof (Vector4UByte)).ToInt64()))->W = (byte) ((int) patternOffset.W - 1 | (int) patternOffset.Z - 1 << 4);
      }
    }

    public unsafe float GetTextureOffset(int index)
    {
      fixed (byte* numPtr = this.m_bones)
      {
        int num1 = (int) ((Vector4UByte*) (numPtr + ((IntPtr) (5 + index) * sizeof (Vector4UByte)).ToInt64()))->W & 15;
        int num2 = (int) ((Vector4UByte*) (numPtr + ((IntPtr) (5 + index) * sizeof (Vector4UByte)).ToInt64()))->W >> 4 & 16;
        return num2 == 0 ? 0.0f : (float) (num1 / num2);
      }
    }

    public unsafe float BoneRange
    {
      get
      {
        fixed (byte* numPtr = this.m_bones)
          return (float) ((Vector4UByte*) (numPtr + (new IntPtr(4) * sizeof (Vector4UByte)).ToInt64()))->W / 10f;
      }
      set
      {
        fixed (byte* numPtr = this.m_bones)
          ((Vector4UByte*) (numPtr + (new IntPtr(4) * sizeof (Vector4UByte)).ToInt64()))->W = (byte) ((double) value * 10.0);
      }
    }

    public unsafe bool EnableSkinning
    {
      get
      {
        fixed (byte* numPtr = this.m_bones)
          return ((int) ((Vector4UByte*) (numPtr + (new IntPtr(3) * sizeof (Vector4UByte)).ToInt64()))->W & 1) > 0;
      }
      set
      {
        fixed (byte* numPtr = this.m_bones)
        {
          if (value)
            ((Vector4UByte*) (numPtr + (new IntPtr(3) * sizeof (Vector4UByte)).ToInt64()))->W |= (byte) 1;
          else
            ((Vector4UByte*) (numPtr + (new IntPtr(3) * sizeof (Vector4UByte)).ToInt64()))->W &= (byte) 254;
        }
      }
    }

    public unsafe void SetColorMaskHSV(Vector4 colorMaskHSV)
    {
      this.ColorMaskHSV = colorMaskHSV;
      fixed (byte* numPtr = this.m_bones)
      {
        if ((double) colorMaskHSV.W < 0.0)
          ((Vector4UByte*) (numPtr + (new IntPtr(3) * sizeof (Vector4UByte)).ToInt64()))->W |= (byte) 2;
        else
          ((Vector4UByte*) (numPtr + (new IntPtr(3) * sizeof (Vector4UByte)).ToInt64()))->W &= (byte) 253;
      }
      this.ColorMaskHSV.W = Math.Abs(this.ColorMaskHSV.W);
    }

    public unsafe Vector3UByte this[int index]
    {
      get
      {
        fixed (byte* numPtr = this.m_bones)
          return index == 8 ? new Vector3UByte(((Vector4UByte*) numPtr)->W, ((Vector4UByte*) (numPtr + sizeof (Vector4UByte)))->W, ((Vector4UByte*) (numPtr + (new IntPtr(2) * sizeof (Vector4UByte)).ToInt64()))->W) : *(Vector3UByte*) (numPtr + ((IntPtr) index * sizeof (Vector4UByte)).ToInt64());
      }
      set
      {
        fixed (byte* numPtr = this.m_bones)
        {
          if (index == 8)
          {
            ((Vector4UByte*) numPtr)->W = value.X;
            ((Vector4UByte*) (numPtr + sizeof (Vector4UByte)))->W = value.Y;
            ((Vector4UByte*) (numPtr + (new IntPtr(2) * sizeof (Vector4UByte)).ToInt64()))->W = value.Z;
          }
          else
            *(Vector3UByte*) (numPtr + ((IntPtr) index * sizeof (Vector4UByte)).ToInt64()) = value;
        }
      }
    }

    public Vector3 GetDenormalizedBone(int index) => this.Denormalize(this.GetNormalizedBone(index), this.BoneRange);

    public unsafe Vector4UByte GetPackedBone(int index)
    {
      fixed (byte* numPtr = this.m_bones)
        return index == 8 ? new Vector4UByte(((Vector4UByte*) numPtr)->W, ((Vector4UByte*) (numPtr + sizeof (Vector4UByte)))->W, ((Vector4UByte*) (numPtr + (new IntPtr(2) * sizeof (Vector4UByte)).ToInt64()))->W, (byte) 0) : *(Vector4UByte*) (numPtr + ((IntPtr) index * sizeof (Vector4UByte)).ToInt64());
    }

    private Vector4 GetNormalizedBone(int index)
    {
      Vector4UByte packedBone = this.GetPackedBone(index);
      return new Vector4((float) packedBone.X, (float) packedBone.Y, (float) packedBone.Z, (float) packedBone.W) / (float) byte.MaxValue;
    }

    private Vector3 Denormalize(Vector4 position, float range) => (new Vector3(position) + 0.001960784f - 0.5f) * range * 2f;
  }
}
