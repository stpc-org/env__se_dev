// Decompiled with JetBrains decompiler
// Type: VRage.MyFixedPoint
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using ProtoBuf;
using System;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VRage.Network;

namespace VRage
{
  [ProtoContract]
  [Serializable]
  public struct MyFixedPoint : IXmlSerializable
  {
    private const int Places = 6;
    private const int Divider = 1000000;
    private static readonly string FormatSpecifier = "D" + (object) 7;
    private static readonly char[] TrimChars = new char[1]
    {
      '0'
    };
    public static readonly MyFixedPoint MinValue = new MyFixedPoint(long.MinValue);
    public static readonly MyFixedPoint MaxValue = new MyFixedPoint(long.MaxValue);
    public static readonly MyFixedPoint Zero = new MyFixedPoint(0L);
    public static readonly MyFixedPoint SmallestPossibleValue = new MyFixedPoint(1L);
    public static readonly MyFixedPoint MaxIntValue = (MyFixedPoint) int.MaxValue;
    public static readonly MyFixedPoint MinIntValue = (MyFixedPoint) int.MinValue;
    [ProtoMember(1)]
    public long RawValue;

    private MyFixedPoint(long rawValue) => this.RawValue = rawValue;

    public string SerializeString()
    {
      string str1 = this.RawValue.ToString(MyFixedPoint.FormatSpecifier);
      string str2 = str1.Substring(0, str1.Length - 6);
      string str3 = str1.Substring(str1.Length - 6).TrimEnd(MyFixedPoint.TrimChars);
      return str3.Length > 0 ? str2 + "." + str3 : str2;
    }

    public static MyFixedPoint DeserializeStringSafe(string text)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        char ch = text[index];
        if ((ch < '0' || ch > '9') && ch != '.' && (ch != '-' || index != 0))
          return (MyFixedPoint) double.Parse(text);
      }
      try
      {
        return MyFixedPoint.DeserializeString(text);
      }
      catch
      {
        return (MyFixedPoint) double.Parse(text);
      }
    }

    public static MyFixedPoint DeserializeString(string text)
    {
      if (string.IsNullOrEmpty(text))
        return new MyFixedPoint();
      int num = text.IndexOf('.');
      if (num == -1)
        return new MyFixedPoint(long.Parse(text) * 1000000L);
      text = text.Replace(".", "");
      text = text.PadRight(num + 1 + 6, '0');
      text = text.Substring(0, num + 6);
      return new MyFixedPoint(long.Parse(text));
    }

    public static explicit operator MyFixedPoint(float d)
    {
      if ((double) d * 1000000.0 + 0.5 >= (double) long.MaxValue)
        return MyFixedPoint.MaxValue;
      return (double) d * 1000000.0 + 0.5 <= (double) long.MinValue ? MyFixedPoint.MinValue : new MyFixedPoint((long) ((double) d * 1000000.0 + 0.5));
    }

    public static explicit operator MyFixedPoint(double d)
    {
      if (d * 1000000.0 + 0.5 >= (double) long.MaxValue)
        return MyFixedPoint.MaxValue;
      return d * 1000000.0 + 0.5 <= (double) long.MinValue ? MyFixedPoint.MinValue : new MyFixedPoint((long) (d * 1000000.0 + 0.5));
    }

    public static explicit operator MyFixedPoint(Decimal d) => new MyFixedPoint((long) (d * 1000000M + 0.5M));

    public static implicit operator MyFixedPoint(int i) => new MyFixedPoint((long) i * 1000000L);

    public static explicit operator Decimal(MyFixedPoint fp) => (Decimal) fp.RawValue / 1000000M;

    public static explicit operator float(MyFixedPoint fp) => (float) fp.RawValue / 1000000f;

    public static explicit operator double(MyFixedPoint fp) => (double) fp.RawValue / 1000000.0;

    public static explicit operator int(MyFixedPoint fp) => (int) (fp.RawValue / 1000000L);

    public static bool IsIntegral(MyFixedPoint fp) => fp.RawValue % 1000000L == 0L;

    public static MyFixedPoint Ceiling(MyFixedPoint a)
    {
      a.RawValue = (a.RawValue + 1000000L - 1L) / 1000000L * 1000000L;
      return a;
    }

    public static MyFixedPoint Floor(MyFixedPoint a)
    {
      a.RawValue = a.RawValue / 1000000L * 1000000L;
      return a;
    }

    public static MyFixedPoint Min(MyFixedPoint a, MyFixedPoint b) => !(a < b) ? b : a;

    public static MyFixedPoint Max(MyFixedPoint a, MyFixedPoint b) => !(a > b) ? b : a;

    public static MyFixedPoint Round(MyFixedPoint a)
    {
      a.RawValue = (a.RawValue + 500000L) / 1000000L;
      return a;
    }

    public static MyFixedPoint operator -(MyFixedPoint a) => new MyFixedPoint(-a.RawValue);

    public static bool operator <(MyFixedPoint a, MyFixedPoint b) => a.RawValue < b.RawValue;

    public static bool operator >(MyFixedPoint a, MyFixedPoint b) => a.RawValue > b.RawValue;

    public static bool operator <=(MyFixedPoint a, MyFixedPoint b) => a.RawValue <= b.RawValue;

    public static bool operator >=(MyFixedPoint a, MyFixedPoint b) => a.RawValue >= b.RawValue;

    public static bool operator ==(MyFixedPoint a, MyFixedPoint b) => a.RawValue == b.RawValue;

    public static bool operator !=(MyFixedPoint a, MyFixedPoint b) => a.RawValue != b.RawValue;

    public static MyFixedPoint operator +(MyFixedPoint a, MyFixedPoint b)
    {
      a.RawValue += b.RawValue;
      return a;
    }

    public static MyFixedPoint operator -(MyFixedPoint a, MyFixedPoint b)
    {
      a.RawValue -= b.RawValue;
      return a;
    }

    public static MyFixedPoint operator *(MyFixedPoint a, MyFixedPoint b)
    {
      long num1 = a.RawValue / 1000000L;
      long num2 = b.RawValue / 1000000L;
      long num3 = a.RawValue % 1000000L;
      long num4 = b.RawValue % 1000000L;
      return new MyFixedPoint(num1 * num2 * 1000000L + num3 * num4 / 1000000L + num1 * num4 + num2 * num3);
    }

    public static MyFixedPoint operator *(MyFixedPoint a, float b) => a * (MyFixedPoint) b;

    public static MyFixedPoint operator *(float a, MyFixedPoint b) => (MyFixedPoint) a * b;

    public static MyFixedPoint operator *(MyFixedPoint a, int b) => a * (MyFixedPoint) b;

    public static MyFixedPoint operator *(int a, MyFixedPoint b) => (MyFixedPoint) a * b;

    public static MyFixedPoint AddSafe(MyFixedPoint a, MyFixedPoint b) => new MyFixedPoint(MyFixedPoint.AddSafeInternal(a.RawValue, b.RawValue));

    public static MyFixedPoint MultiplySafe(MyFixedPoint a, float b) => MyFixedPoint.MultiplySafe(a, (MyFixedPoint) b);

    public static MyFixedPoint MultiplySafe(MyFixedPoint a, int b) => MyFixedPoint.MultiplySafe(a, (MyFixedPoint) b);

    public static MyFixedPoint MultiplySafe(float a, MyFixedPoint b) => MyFixedPoint.MultiplySafe((MyFixedPoint) a, b);

    public static MyFixedPoint MultiplySafe(int a, MyFixedPoint b) => MyFixedPoint.MultiplySafe((MyFixedPoint) a, b);

    public static MyFixedPoint MultiplySafe(MyFixedPoint a, MyFixedPoint b)
    {
      long a1 = a.RawValue / 1000000L;
      long a2 = b.RawValue / 1000000L;
      long b1 = a.RawValue % 1000000L;
      long b2 = b.RawValue % 1000000L;
      long a3 = b1 * b2 / 1000000L;
      long b3 = MyFixedPoint.MultiplySafeInternal(a1, a2 * 1000000L);
      long b4 = MyFixedPoint.MultiplySafeInternal(a1, b2);
      long b5 = MyFixedPoint.MultiplySafeInternal(a2, b1);
      return new MyFixedPoint(MyFixedPoint.AddSafeInternal(MyFixedPoint.AddSafeInternal(MyFixedPoint.AddSafeInternal(a3, b3), b4), b5));
    }

    private static long MultiplySafeInternal(long a, long b)
    {
      long num = a * b;
      if (b == 0L || num / b == a)
        return num;
      return Math.Sign(a) * Math.Sign(b) != 1 ? long.MinValue : long.MaxValue;
    }

    private static long AddSafeInternal(long a, long b)
    {
      int num1 = Math.Sign(a);
      if (num1 * Math.Sign(b) != 1)
        return a + b;
      long num2 = a + b;
      if (Math.Sign(num2) == num1)
        return num2;
      return num1 >= 0 ? long.MaxValue : long.MinValue;
    }

    public int ToIntSafe()
    {
      if (this.RawValue > MyFixedPoint.MaxIntValue.RawValue)
        return (int) MyFixedPoint.MaxIntValue;
      return this.RawValue < MyFixedPoint.MinIntValue.RawValue ? (int) MyFixedPoint.MinIntValue : (int) this;
    }

    public override string ToString() => this.SerializeString();

    public override int GetHashCode() => (int) this.RawValue;

    public override bool Equals(object obj)
    {
      if (obj != null)
      {
        MyFixedPoint? nullable = obj as MyFixedPoint?;
        if (nullable.HasValue)
          return this == nullable.Value;
      }
      return false;
    }

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader) => this.RawValue = MyFixedPoint.DeserializeStringSafe(reader.ReadInnerXml()).RawValue;

    void IXmlSerializable.WriteXml(XmlWriter writer) => writer.WriteString(this.SerializeString());

    protected class VRage_MyFixedPoint\u003C\u003ERawValue\u003C\u003EAccessor : IMemberAccessor<MyFixedPoint, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyFixedPoint owner, in long value) => owner.RawValue = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyFixedPoint owner, out long value) => value = owner.RawValue;
    }
  }
}
