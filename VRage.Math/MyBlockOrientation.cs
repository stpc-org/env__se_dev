// Decompiled with JetBrains decompiler
// Type: VRageMath.MyBlockOrientation
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using ProtoBuf;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [ProtoContract]
  public struct MyBlockOrientation
  {
    public static readonly MyBlockOrientation Identity = new MyBlockOrientation(Base6Directions.Direction.Forward, Base6Directions.Direction.Up);
    [ProtoMember(1)]
    public Base6Directions.Direction Forward;
    [ProtoMember(4)]
    public Base6Directions.Direction Up;

    public Base6Directions.Direction Left => Base6Directions.GetLeft(this.Up, this.Forward);

    public bool IsValid => Base6Directions.IsValidBlockOrientation(this.Forward, this.Up);

    public MyBlockOrientation(Base6Directions.Direction forward, Base6Directions.Direction up)
    {
      this.Forward = forward;
      this.Up = up;
    }

    public MyBlockOrientation(ref Quaternion q)
    {
      this.Forward = Base6Directions.GetForward(q);
      this.Up = Base6Directions.GetUp(q);
    }

    public MyBlockOrientation(ref Matrix m)
    {
      this.Forward = Base6Directions.GetForward(ref m);
      this.Up = Base6Directions.GetUp(ref m);
    }

    public void GetQuaternion(out Quaternion result)
    {
      Matrix result1;
      this.GetMatrix(out result1);
      Quaternion.CreateFromRotationMatrix(ref result1, out result);
    }

    public void GetMatrix(out Matrix result)
    {
      Vector3 result1;
      Base6Directions.GetVector(this.Forward, out result1);
      Vector3 result2;
      Base6Directions.GetVector(this.Up, out result2);
      Matrix.CreateWorld(ref Vector3.Zero, ref result1, ref result2, out result);
    }

    public override int GetHashCode() => (int) ((Base6Directions.Direction) ((int) this.Forward << 16) | this.Up);

    public override bool Equals(object obj)
    {
      if (obj != null)
      {
        MyBlockOrientation? nullable = obj as MyBlockOrientation?;
        if (nullable.HasValue)
          return this == nullable.Value;
      }
      return false;
    }

    public override string ToString() => string.Format("[Forward:{0}, Up:{1}]", (object) this.Forward, (object) this.Up);

    public Base6Directions.Direction TransformDirection(
      Base6Directions.Direction baseDirection)
    {
      Base6Directions.Axis axis = Base6Directions.GetAxis(baseDirection);
      int num = (int) baseDirection % 2;
      switch (axis)
      {
        case Base6Directions.Axis.ForwardBackward:
          return num != 1 ? this.Forward : Base6Directions.GetFlippedDirection(this.Forward);
        case Base6Directions.Axis.LeftRight:
          return num != 1 ? this.Left : Base6Directions.GetFlippedDirection(this.Left);
        default:
          return num != 1 ? this.Up : Base6Directions.GetFlippedDirection(this.Up);
      }
    }

    public Base6Directions.Direction TransformDirectionInverse(
      Base6Directions.Direction baseDirection)
    {
      Base6Directions.Axis axis = Base6Directions.GetAxis(baseDirection);
      return axis == Base6Directions.GetAxis(this.Forward) ? (baseDirection != this.Forward ? Base6Directions.Direction.Backward : Base6Directions.Direction.Forward) : (axis == Base6Directions.GetAxis(this.Left) ? (baseDirection != this.Left ? Base6Directions.Direction.Right : Base6Directions.Direction.Left) : (baseDirection != this.Up ? Base6Directions.Direction.Down : Base6Directions.Direction.Up));
    }

    public static bool operator ==(MyBlockOrientation orientation1, MyBlockOrientation orientation2) => orientation1.Forward == orientation2.Forward && orientation1.Up == orientation2.Up;

    public static bool operator !=(MyBlockOrientation orientation1, MyBlockOrientation orientation2) => orientation1.Forward != orientation2.Forward || orientation1.Up != orientation2.Up;

    protected class VRageMath_MyBlockOrientation\u003C\u003EForward\u003C\u003EAccessor : IMemberAccessor<MyBlockOrientation, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBlockOrientation owner, in Base6Directions.Direction value) => owner.Forward = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBlockOrientation owner, out Base6Directions.Direction value) => value = owner.Forward;
    }

    protected class VRageMath_MyBlockOrientation\u003C\u003EUp\u003C\u003EAccessor : IMemberAccessor<MyBlockOrientation, Base6Directions.Direction>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyBlockOrientation owner, in Base6Directions.Direction value) => owner.Up = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyBlockOrientation owner, out Base6Directions.Direction value) => value = owner.Up;
    }
  }
}
