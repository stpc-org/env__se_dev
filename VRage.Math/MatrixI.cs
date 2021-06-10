// Decompiled with JetBrains decompiler
// Type: VRageMath.MatrixI
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  public struct MatrixI
  {
    public Base6Directions.Direction Right;
    public Base6Directions.Direction Up;
    public Base6Directions.Direction Backward;
    public Vector3I Translation;

    public Base6Directions.Direction Left
    {
      get => Base6Directions.GetFlippedDirection(this.Right);
      set => this.Right = Base6Directions.GetFlippedDirection(value);
    }

    public Base6Directions.Direction Down
    {
      get => Base6Directions.GetFlippedDirection(this.Up);
      set => this.Up = Base6Directions.GetFlippedDirection(value);
    }

    public Base6Directions.Direction Forward
    {
      get => Base6Directions.GetFlippedDirection(this.Backward);
      set => this.Backward = Base6Directions.GetFlippedDirection(value);
    }

    public Base6Directions.Direction GetDirection(Base6Directions.Direction direction)
    {
      switch (direction)
      {
        case Base6Directions.Direction.Backward:
          return this.Backward;
        case Base6Directions.Direction.Left:
          return this.Left;
        case Base6Directions.Direction.Right:
          return this.Right;
        case Base6Directions.Direction.Up:
          return this.Up;
        case Base6Directions.Direction.Down:
          return this.Down;
        default:
          return this.Forward;
      }
    }

    public void SetDirection(
      Base6Directions.Direction dirToSet,
      Base6Directions.Direction newDirection)
    {
      switch (dirToSet)
      {
        case Base6Directions.Direction.Forward:
          this.Forward = newDirection;
          break;
        case Base6Directions.Direction.Backward:
          this.Backward = newDirection;
          break;
        case Base6Directions.Direction.Left:
          this.Left = newDirection;
          break;
        case Base6Directions.Direction.Right:
          this.Right = newDirection;
          break;
        case Base6Directions.Direction.Up:
          this.Up = newDirection;
          break;
        case Base6Directions.Direction.Down:
          this.Down = newDirection;
          break;
      }
    }

    public Vector3I RightVector
    {
      get => Base6Directions.GetIntVector(this.Right);
      set => this.Right = Base6Directions.GetDirection(value);
    }

    public Vector3I LeftVector
    {
      get => Base6Directions.GetIntVector(this.Left);
      set => this.Left = Base6Directions.GetDirection(value);
    }

    public Vector3I UpVector
    {
      get => Base6Directions.GetIntVector(this.Up);
      set => this.Up = Base6Directions.GetDirection(value);
    }

    public Vector3I DownVector
    {
      get => Base6Directions.GetIntVector(this.Down);
      set => this.Down = Base6Directions.GetDirection(value);
    }

    public Vector3I BackwardVector
    {
      get => Base6Directions.GetIntVector(this.Backward);
      set => this.Backward = Base6Directions.GetDirection(value);
    }

    public Vector3I ForwardVector
    {
      get => Base6Directions.GetIntVector(this.Forward);
      set => this.Forward = Base6Directions.GetDirection(value);
    }

    public MatrixI(
      ref Vector3I position,
      Base6Directions.Direction forward,
      Base6Directions.Direction up)
    {
      this.Translation = position;
      this.Right = Base6Directions.GetFlippedDirection(Base6Directions.GetLeft(up, forward));
      this.Up = up;
      this.Backward = Base6Directions.GetFlippedDirection(forward);
    }

    public MatrixI(
      Vector3I position,
      Base6Directions.Direction forward,
      Base6Directions.Direction up)
    {
      this.Translation = position;
      this.Right = Base6Directions.GetFlippedDirection(Base6Directions.GetLeft(up, forward));
      this.Up = up;
      this.Backward = Base6Directions.GetFlippedDirection(forward);
    }

    public MatrixI(Base6Directions.Direction forward, Base6Directions.Direction up)
      : this(Vector3I.Zero, forward, up)
    {
    }

    public MatrixI(ref Vector3I position, ref Vector3I forward, ref Vector3I up)
      : this(ref position, Base6Directions.GetDirection(ref forward), Base6Directions.GetDirection(ref up))
    {
    }

    public MatrixI(ref Vector3I position, ref Vector3 forward, ref Vector3 up)
      : this(ref position, Base6Directions.GetDirection(ref forward), Base6Directions.GetDirection(ref up))
    {
    }

    public MatrixI(MyBlockOrientation orientation)
      : this(Vector3I.Zero, orientation.Forward, orientation.Up)
    {
    }

    public MyBlockOrientation GetBlockOrientation() => new MyBlockOrientation(this.Forward, this.Up);

    public Matrix GetFloatMatrix() => Matrix.CreateWorld(new Vector3(this.Translation), Base6Directions.GetVector(this.Forward), Base6Directions.GetVector(this.Up));

    public static MatrixI CreateRotation(
      Base6Directions.Direction oldA,
      Base6Directions.Direction oldB,
      Base6Directions.Direction newA,
      Base6Directions.Direction newB)
    {
      MatrixI matrixI = new MatrixI();
      matrixI.Translation = Vector3I.Zero;
      Base6Directions.Direction cross1 = Base6Directions.GetCross(oldA, oldB);
      Base6Directions.Direction cross2 = Base6Directions.GetCross(newA, newB);
      matrixI.SetDirection(oldA, newA);
      matrixI.SetDirection(oldB, newB);
      matrixI.SetDirection(cross1, cross2);
      return matrixI;
    }

    public static void Invert(ref MatrixI matrix, out MatrixI result)
    {
      result = new MatrixI();
      switch (matrix.Right)
      {
        case Base6Directions.Direction.Forward:
          result.Backward = Base6Directions.Direction.Left;
          break;
        case Base6Directions.Direction.Backward:
          result.Backward = Base6Directions.Direction.Right;
          break;
        case Base6Directions.Direction.Up:
          result.Up = Base6Directions.Direction.Right;
          break;
        case Base6Directions.Direction.Down:
          result.Up = Base6Directions.Direction.Left;
          break;
        default:
          result.Right = matrix.Right;
          break;
      }
      switch (matrix.Up)
      {
        case Base6Directions.Direction.Forward:
          result.Backward = Base6Directions.Direction.Down;
          break;
        case Base6Directions.Direction.Backward:
          result.Backward = Base6Directions.Direction.Up;
          break;
        case Base6Directions.Direction.Left:
          result.Right = Base6Directions.Direction.Down;
          break;
        case Base6Directions.Direction.Right:
          result.Right = Base6Directions.Direction.Up;
          break;
        default:
          result.Up = matrix.Up;
          break;
      }
      switch (matrix.Backward)
      {
        case Base6Directions.Direction.Left:
          result.Right = Base6Directions.Direction.Forward;
          break;
        case Base6Directions.Direction.Right:
          result.Right = Base6Directions.Direction.Backward;
          break;
        case Base6Directions.Direction.Up:
          result.Up = Base6Directions.Direction.Backward;
          break;
        case Base6Directions.Direction.Down:
          result.Up = Base6Directions.Direction.Forward;
          break;
        default:
          result.Backward = matrix.Backward;
          break;
      }
      Vector3I.TransformNormal(ref matrix.Translation, ref result, out result.Translation);
      result.Translation = -result.Translation;
    }

    public static void Multiply(
      ref MatrixI leftMatrix,
      ref MatrixI rightMatrix,
      out MatrixI result)
    {
      result = new MatrixI();
      Vector3I rightVector = leftMatrix.RightVector;
      Vector3I upVector = leftMatrix.UpVector;
      Vector3I backwardVector = leftMatrix.BackwardVector;
      Vector3I result1;
      Vector3I.TransformNormal(ref rightVector, ref rightMatrix, out result1);
      Vector3I result2;
      Vector3I.TransformNormal(ref upVector, ref rightMatrix, out result2);
      Vector3I result3;
      Vector3I.TransformNormal(ref backwardVector, ref rightMatrix, out result3);
      Vector3I.Transform(ref leftMatrix.Translation, ref rightMatrix, out result.Translation);
      result.RightVector = result1;
      result.UpVector = result2;
      result.BackwardVector = result3;
    }

    public static MyBlockOrientation Transform(
      ref MyBlockOrientation orientation,
      ref MatrixI transform)
    {
      return new MyBlockOrientation(transform.GetDirection(orientation.Forward), transform.GetDirection(orientation.Up));
    }

    protected class VRageMath_MatrixI\u003C\u003ERight\u003C\u003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Base6Directions.Direction value) => owner.Right = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Base6Directions.Direction value) => value = owner.Right;
    }

    protected class VRageMath_MatrixI\u003C\u003EUp\u003C\u003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Base6Directions.Direction value) => owner.Up = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Base6Directions.Direction value) => value = owner.Up;
    }

    protected class VRageMath_MatrixI\u003C\u003EBackward\u003C\u003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Base6Directions.Direction value) => owner.Backward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Base6Directions.Direction value) => value = owner.Backward;
    }

    protected class VRageMath_MatrixI\u003C\u003ETranslation\u003C\u003EAccessor : IMemberAccessor<MatrixI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Vector3I value) => owner.Translation = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Vector3I value) => value = owner.Translation;
    }

    protected class VRageMath_MatrixI\u003C\u003ELeft\u003C\u003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Base6Directions.Direction value) => owner.Left = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Base6Directions.Direction value) => value = owner.Left;
    }

    protected class VRageMath_MatrixI\u003C\u003EDown\u003C\u003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Base6Directions.Direction value) => owner.Down = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Base6Directions.Direction value) => value = owner.Down;
    }

    protected class VRageMath_MatrixI\u003C\u003EForward\u003C\u003EAccessor : IMemberAccessor<MatrixI, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Base6Directions.Direction value) => owner.Forward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Base6Directions.Direction value) => value = owner.Forward;
    }

    protected class VRageMath_MatrixI\u003C\u003ERightVector\u003C\u003EAccessor : IMemberAccessor<MatrixI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Vector3I value) => owner.RightVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Vector3I value) => value = owner.RightVector;
    }

    protected class VRageMath_MatrixI\u003C\u003ELeftVector\u003C\u003EAccessor : IMemberAccessor<MatrixI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Vector3I value) => owner.LeftVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Vector3I value) => value = owner.LeftVector;
    }

    protected class VRageMath_MatrixI\u003C\u003EUpVector\u003C\u003EAccessor : IMemberAccessor<MatrixI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Vector3I value) => owner.UpVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Vector3I value) => value = owner.UpVector;
    }

    protected class VRageMath_MatrixI\u003C\u003EDownVector\u003C\u003EAccessor : IMemberAccessor<MatrixI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Vector3I value) => owner.DownVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Vector3I value) => value = owner.DownVector;
    }

    protected class VRageMath_MatrixI\u003C\u003EBackwardVector\u003C\u003EAccessor : IMemberAccessor<MatrixI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Vector3I value) => owner.BackwardVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Vector3I value) => value = owner.BackwardVector;
    }

    protected class VRageMath_MatrixI\u003C\u003EForwardVector\u003C\u003EAccessor : IMemberAccessor<MatrixI, Vector3I>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MatrixI owner, in Vector3I value) => owner.ForwardVector = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MatrixI owner, out Vector3I value) => value = owner.ForwardVector;
    }
  }
}
