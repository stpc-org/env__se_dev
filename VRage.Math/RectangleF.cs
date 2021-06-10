// Decompiled with JetBrains decompiler
// Type: VRageMath.RectangleF
// Assembly: VRage.Math, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B60E95EA-339C-4CC9-9413-1B8A10CB206E
// Assembly location: D:\Files\library_development\lib_se\VRage.Math.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace VRageMath
{
  [Serializable]
  public struct RectangleF : IEquatable<RectangleF>
  {
    public Vector2 Position;
    public Vector2 Size;

    public RectangleF(Vector2 position, Vector2 size)
    {
      this.Position = position;
      this.Size = size;
    }

    public RectangleF(float x, float y, float width, float height)
    {
      this.Position = new Vector2(x, y);
      this.Size = new Vector2(width, height);
    }

    public bool Contains(int x, int y) => (double) x >= (double) this.X && (double) x <= (double) this.X + (double) this.Width && ((double) y >= (double) this.Y && (double) y <= (double) this.Y + (double) this.Height);

    public bool Contains(float x, float y) => (double) x >= (double) this.X && (double) x <= (double) this.X + (double) this.Width && ((double) y >= (double) this.Y && (double) y <= (double) this.Y + (double) this.Height);

    public bool Contains(Vector2 vector2D) => (double) vector2D.X >= (double) this.X && (double) vector2D.X <= (double) this.X + (double) this.Width && ((double) vector2D.Y >= (double) this.Y && (double) vector2D.Y <= (double) this.Y + (double) this.Height);

    public bool Contains(Point point) => (double) point.X >= (double) this.X && (double) point.X <= (double) this.X + (double) this.Width && ((double) point.Y >= (double) this.Y && (double) point.Y <= (double) this.Y + (double) this.Height);

    public float X
    {
      get => this.Position.X;
      set => this.Position.X = value;
    }

    public float Y
    {
      get => this.Position.Y;
      set => this.Position.Y = value;
    }

    public float Width
    {
      get => this.Size.X;
      set => this.Size.X = value;
    }

    public float Height
    {
      get => this.Size.Y;
      set => this.Size.Y = value;
    }

    public float Right => this.Position.X + this.Size.X;

    public float Bottom => this.Position.Y + this.Size.Y;

    public Vector2 Center => this.Position + this.Size / 2f;

    public bool Equals(RectangleF other) => (double) other.X == (double) this.X && (double) other.Y == (double) this.Y && (double) other.Width == (double) this.Width && (double) other.Height == (double) this.Height;

    public static bool Intersect(
      ref RectangleF value1,
      ref RectangleF value2,
      out RectangleF result)
    {
      float num1 = value1.X + value1.Width;
      float num2 = value2.X + value2.Width;
      float num3 = value1.Y + value1.Height;
      float num4 = value2.Y + value2.Height;
      float x = (double) value1.X > (double) value2.X ? value1.X : value2.X;
      float y = (double) value1.Y > (double) value2.Y ? value1.Y : value2.Y;
      float num5 = (double) num1 < (double) num2 ? num1 : num2;
      float num6 = (double) num3 < (double) num4 ? num3 : num4;
      if ((double) num5 > (double) x && (double) num6 > (double) y)
      {
        result = new RectangleF(x, y, num5 - x, num6 - y);
        return true;
      }
      result = new RectangleF(0.0f, 0.0f, 0.0f, 0.0f);
      return false;
    }

    public override bool Equals(object obj) => obj != null && !(obj.GetType() != typeof (RectangleF)) && this.Equals((RectangleF) obj);

    public override int GetHashCode()
    {
      float num1 = this.X;
      int num2 = num1.GetHashCode() * 397;
      num1 = this.Y;
      int hashCode1 = num1.GetHashCode();
      int num3 = (num2 ^ hashCode1) * 397;
      num1 = this.Width;
      int hashCode2 = num1.GetHashCode();
      int num4 = (num3 ^ hashCode2) * 397;
      num1 = this.Height;
      int hashCode3 = num1.GetHashCode();
      return num4 ^ hashCode3;
    }

    public static bool operator ==(RectangleF left, RectangleF right) => left.Equals(right);

    public static bool operator !=(RectangleF left, RectangleF right) => !left.Equals(right);

    public override string ToString() => string.Format("(X: {0} Y: {1} W: {2} H: {3})", (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height);

    public static RectangleF Min(RectangleF? rectangle, RectangleF? scissors)
    {
      if (rectangle.HasValue)
      {
        if (!scissors.HasValue)
          return rectangle.Value;
        Vector2 position;
        ref Vector2 local1 = ref position;
        double x1 = (double) rectangle.Value.X;
        RectangleF rectangleF1 = scissors.Value;
        double x2 = (double) rectangleF1.X;
        double num1 = (double) Math.Max((float) x1, (float) x2);
        rectangleF1 = rectangle.Value;
        double y1 = (double) rectangleF1.Y;
        rectangleF1 = scissors.Value;
        double y2 = (double) rectangleF1.Y;
        double num2 = (double) Math.Max((float) y1, (float) y2);
        local1 = new Vector2((float) num1, (float) num2);
        Vector2 vector2;
        ref Vector2 local2 = ref vector2;
        rectangleF1 = rectangle.Value;
        double right1 = (double) rectangleF1.Right;
        RectangleF rectangleF2 = scissors.Value;
        double right2 = (double) rectangleF2.Right;
        double num3 = (double) Math.Min((float) right1, (float) right2);
        rectangleF2 = rectangle.Value;
        double bottom1 = (double) rectangleF2.Bottom;
        rectangleF2 = scissors.Value;
        double bottom2 = (double) rectangleF2.Bottom;
        double num4 = (double) Math.Min((float) bottom1, (float) bottom2);
        local2 = new Vector2((float) num3, (float) num4);
        return new RectangleF(position, vector2 - position);
      }
      return scissors.HasValue ? scissors.Value : new RectangleF();
    }

    protected class VRageMath_RectangleF\u003C\u003EPosition\u003C\u003EAccessor : IMemberAccessor<RectangleF, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RectangleF owner, in Vector2 value) => owner.Position = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RectangleF owner, out Vector2 value) => value = owner.Position;
    }

    protected class VRageMath_RectangleF\u003C\u003ESize\u003C\u003EAccessor : IMemberAccessor<RectangleF, Vector2>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RectangleF owner, in Vector2 value) => owner.Size = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RectangleF owner, out Vector2 value) => value = owner.Size;
    }

    protected class VRageMath_RectangleF\u003C\u003EX\u003C\u003EAccessor : IMemberAccessor<RectangleF, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RectangleF owner, in float value) => owner.X = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RectangleF owner, out float value) => value = owner.X;
    }

    protected class VRageMath_RectangleF\u003C\u003EY\u003C\u003EAccessor : IMemberAccessor<RectangleF, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RectangleF owner, in float value) => owner.Y = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RectangleF owner, out float value) => value = owner.Y;
    }

    protected class VRageMath_RectangleF\u003C\u003EWidth\u003C\u003EAccessor : IMemberAccessor<RectangleF, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RectangleF owner, in float value) => owner.Width = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RectangleF owner, out float value) => value = owner.Width;
    }

    protected class VRageMath_RectangleF\u003C\u003EHeight\u003C\u003EAccessor : IMemberAccessor<RectangleF, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref RectangleF owner, in float value) => owner.Height = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref RectangleF owner, out float value) => value = owner.Height;
    }
  }
}
