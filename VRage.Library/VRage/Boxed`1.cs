// Decompiled with JetBrains decompiler
// Type: VRage.Boxed`1
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage
{
  public sealed class Boxed<T> where T : struct
  {
    public T BoxedValue;

    public Boxed(T value) => this.BoxedValue = value;

    public override int GetHashCode() => this.BoxedValue.GetHashCode();

    public override string ToString() => this.BoxedValue.ToString();

    public static implicit operator T(Boxed<T> box) => box.BoxedValue;

    public static explicit operator Boxed<T>(T value) => new Boxed<T>(value);
  }
}
