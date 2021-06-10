// Decompiled with JetBrains decompiler
// Type: VRage.MyDeltaTransform
// Assembly: VRage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A58B56E2-8FB0-4C25-B2B4-ADDA35DEFA91
// Assembly location: D:\Files\library_development\lib_se\VRage.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace VRage
{
  [ProtoContract]
  [Serializable]
  public struct MyDeltaTransform
  {
    [NoSerialize]
    public Quaternion OrientationOffset;
    [Serialize]
    [ProtoMember(4)]
    public Vector3 PositionOffset;

    [Serialize]
    [ProtoMember(1)]
    public Vector4 OrientationAsVector
    {
      get => this.OrientationOffset.ToVector4();
      set => this.OrientationOffset = Quaternion.FromVector4(value);
    }

    public bool IsZero => this.PositionOffset == Vector3.Zero && this.OrientationOffset == Quaternion.Zero;

    public static implicit operator Matrix(MyDeltaTransform transform)
    {
      Matrix result;
      Matrix.CreateFromQuaternion(ref transform.OrientationOffset, out result);
      result.Translation = transform.PositionOffset;
      return result;
    }

    public static implicit operator MyDeltaTransform(Matrix matrix)
    {
      MyDeltaTransform myDeltaTransform;
      myDeltaTransform.PositionOffset = matrix.Translation;
      Quaternion.CreateFromRotationMatrix(ref matrix, out myDeltaTransform.OrientationOffset);
      return myDeltaTransform;
    }

    public static implicit operator MatrixD(MyDeltaTransform transform)
    {
      MatrixD result;
      MatrixD.CreateFromQuaternion(ref transform.OrientationOffset, out result);
      result.Translation = (Vector3D) transform.PositionOffset;
      return result;
    }

    public static implicit operator MyDeltaTransform(MatrixD matrix)
    {
      MyDeltaTransform myDeltaTransform;
      myDeltaTransform.PositionOffset = (Vector3) matrix.Translation;
      Quaternion.CreateFromRotationMatrix(ref matrix, out myDeltaTransform.OrientationOffset);
      return myDeltaTransform;
    }

    public static implicit operator MyPositionAndOrientation(
      MyDeltaTransform deltaTransform)
    {
      return new MyPositionAndOrientation((Vector3D) deltaTransform.PositionOffset, deltaTransform.OrientationOffset.Forward, deltaTransform.OrientationOffset.Up);
    }

    public static implicit operator MyDeltaTransform(MyPositionAndOrientation value) => new MyDeltaTransform()
    {
      PositionOffset = (Vector3) (Vector3D) value.Position,
      OrientationOffset = Quaternion.CreateFromForwardUp((Vector3) value.Forward, (Vector3) value.Up)
    };

    protected class VRage_MyDeltaTransform\u003C\u003EOrientationOffset\u003C\u003EAccessor : IMemberAccessor<MyDeltaTransform, Quaternion>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDeltaTransform owner, in Quaternion value) => owner.OrientationOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDeltaTransform owner, out Quaternion value) => value = owner.OrientationOffset;
    }

    protected class VRage_MyDeltaTransform\u003C\u003EPositionOffset\u003C\u003EAccessor : IMemberAccessor<MyDeltaTransform, Vector3>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDeltaTransform owner, in Vector3 value) => owner.PositionOffset = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDeltaTransform owner, out Vector3 value) => value = owner.PositionOffset;
    }

    protected class VRage_MyDeltaTransform\u003C\u003EOrientationAsVector\u003C\u003EAccessor : IMemberAccessor<MyDeltaTransform, Vector4>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyDeltaTransform owner, in Vector4 value) => owner.OrientationAsVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyDeltaTransform owner, out Vector4 value) => value = owner.OrientationAsVector;
    }
  }
}
