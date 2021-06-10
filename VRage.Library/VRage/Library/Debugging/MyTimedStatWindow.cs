// Decompiled with JetBrains decompiler
// Type: VRage.Library.Debugging.MyTimedStatWindow
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

namespace VRage.Library.Debugging
{
  public class MyTimedStatWindow
  {
    public static readonly MyTimedStatWindow.IStatArithmetic<int> IntArithmetic = (MyTimedStatWindow.IStatArithmetic<int>) new MyTimedStatWindow.IntArithmeticImpl();
    public static readonly MyTimedStatWindow.IStatArithmetic<float> FloatArithmetic = (MyTimedStatWindow.IStatArithmetic<float>) new MyTimedStatWindow.FloatArithmeticImpl();

    public interface IStatArithmetic<TStats>
    {
      void Add(in TStats lhs, in TStats rhs, out TStats result);

      void Subtract(in TStats lhs, in TStats rhs, out TStats result);
    }

    public class IntArithmeticImpl : MyTimedStatWindow.IStatArithmetic<int>
    {
      public void Add(in int lhs, in int rhs, out int result) => result = lhs + rhs;

      public void Subtract(in int lhs, in int rhs, out int result) => result = lhs - rhs;

      void MyTimedStatWindow.IStatArithmetic<int>.Add(
        in int lhs,
        in int rhs,
        out int result)
      {
        this.Add(in lhs, in rhs, out result);
      }

      void MyTimedStatWindow.IStatArithmetic<int>.Subtract(
        in int lhs,
        in int rhs,
        out int result)
      {
        this.Subtract(in lhs, in rhs, out result);
      }
    }

    public class FloatArithmeticImpl : MyTimedStatWindow.IStatArithmetic<float>
    {
      public void Add(in float lhs, in float rhs, out float result) => result = lhs + rhs;

      public void Subtract(in float lhs, in float rhs, out float result) => result = lhs - rhs;

      void MyTimedStatWindow.IStatArithmetic<float>.Add(
        in float lhs,
        in float rhs,
        out float result)
      {
        this.Add(in lhs, in rhs, out result);
      }

      void MyTimedStatWindow.IStatArithmetic<float>.Subtract(
        in float lhs,
        in float rhs,
        out float result)
      {
        this.Subtract(in lhs, in rhs, out result);
      }
    }
  }
}
