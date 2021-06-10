// Decompiled with JetBrains decompiler
// Type: VRageMath.CompressedPositionOrientation
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath.PackedVector;

namespace VRageMath
{
  [ProtoContract]
  [Serializable]
  public struct CompressedPositionOrientation
  {
    public Vector3 Position;
    public HalfVector4 Orientation;

    public Matrix Matrix
    {
      get
      {
        Matrix result;
        this.ToMatrix(out result);
        return result;
      }
      set => this.FromMatrix(ref value);
    }

    public CompressedPositionOrientation(ref Matrix matrix)
    {
      this.Position = matrix.Translation;
      Quaternion result;
      Quaternion.CreateFromRotationMatrix(ref matrix, out result);
      this.Orientation = new HalfVector4(result.ToVector4());
    }

    public void FromMatrix(ref Matrix matrix)
    {
      this.Position = matrix.Translation;
      Quaternion result;
      Quaternion.CreateFromRotationMatrix(ref matrix, out result);
      this.Orientation = new HalfVector4(result.ToVector4());
    }

    public void ToMatrix(out Matrix result)
    {
      Quaternion quaternion = Quaternion.FromVector4(this.Orientation.ToVector4());
      Matrix.CreateFromQuaternion(ref quaternion, out result);
      result.Translation = this.Position;
    }

    protected class VRageMath_CompressedPositionOrientation\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<CompressedPositionOrientation, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CompressedPositionOrientation owner, in Vector3 value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CompressedPositionOrientation owner, out Vector3 value) => value = owner.Position;
    }

    protected class VRageMath_CompressedPositionOrientation\u003C\u003EOrientation\u003C\u003EAccessor : IMemberAccessor<CompressedPositionOrientation, HalfVector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CompressedPositionOrientation owner, in HalfVector4 value) => owner.Orientation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CompressedPositionOrientation owner, out HalfVector4 value) => value = owner.Orientation;
    }

    protected class VRageMath_CompressedPositionOrientation\u003C\u003EMatrix\u003C\u003EAccessor : IMemberAccessor<CompressedPositionOrientation, Matrix>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CompressedPositionOrientation owner, in Matrix value) => owner.Matrix = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CompressedPositionOrientation owner, out Matrix value) => value = owner.Matrix;
    }
  }
}
