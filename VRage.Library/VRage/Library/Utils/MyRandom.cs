// Decompiled with JetBrains decompiler
// Type: VRage.Library.Utils.MyRandom
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using VRage.Network;

namespace VRage.Library.Utils
{
  [Serializable]
  public class MyRandom
  {
    [ThreadStatic]
    private static MyRandom m_instance;
    private int inext;
    private int inextp;
    private const int MBIG = 2147483647;
    private const int MSEED = 161803398;
    private const int MZ = 0;
    private int[] SeedArray;
    private byte[] m_tmpLongArray = new byte[8];
    internal static bool EnableDeterminism;

    public static MyRandom Instance
    {
      get
      {
        if (MyRandom.m_instance == null)
          MyRandom.m_instance = new MyRandom();
        return MyRandom.m_instance;
      }
    }

    public MyRandom()
      : this(MyEnvironment.TickCount + Thread.CurrentThread.ManagedThreadId)
    {
    }

    public MyRandom(int Seed)
    {
      this.SeedArray = new int[56];
      this.SetSeed(Seed);
    }

    public MyRandom.StateToken PushSeed(int newSeed) => new MyRandom.StateToken(this, newSeed);

    public unsafe void GetState(out MyRandom.State state)
    {
      state.Inext = this.inext;
      state.Inextp = this.inextp;
      fixed (int* numPtr = state.Seed)
        Marshal.Copy(this.SeedArray, 0, new IntPtr((void*) numPtr), 56);
    }

    public unsafe void SetState(ref MyRandom.State state)
    {
      this.inext = state.Inext;
      this.inextp = state.Inextp;
      fixed (int* numPtr = state.Seed)
        Marshal.Copy(new IntPtr((void*) numPtr), this.SeedArray, 0, 56);
    }

    public int CreateRandomSeed() => MyEnvironment.TickCount ^ this.Next();

    public void SetSeed(int Seed)
    {
      int num1 = 161803398 - (Seed == int.MinValue ? int.MaxValue : Math.Abs(Seed));
      this.SeedArray[55] = num1;
      int num2 = 1;
      for (int index1 = 1; index1 < 55; ++index1)
      {
        int index2 = 21 * index1 % 55;
        this.SeedArray[index2] = num2;
        num2 = num1 - num2;
        if (num2 < 0)
          num2 += int.MaxValue;
        num1 = this.SeedArray[index2];
      }
      for (int index1 = 1; index1 < 5; ++index1)
      {
        for (int index2 = 1; index2 < 56; ++index2)
        {
          this.SeedArray[index2] -= this.SeedArray[1 + (index2 + 30) % 55];
          if (this.SeedArray[index2] < 0)
            this.SeedArray[index2] += int.MaxValue;
        }
      }
      this.inext = 0;
      this.inextp = 21;
      Seed = 1;
    }

    private double GetSampleForLargeRange()
    {
      int num = this.InternalSample();
      if (this.InternalSample() % 2 == 0)
        num = -num;
      return ((double) num + 2147483646.0) / 4294967293.0;
    }

    private int InternalSample()
    {
      int inext = this.inext;
      int inextp = this.inextp;
      int index1;
      if ((index1 = inext + 1) >= 56)
        index1 = 1;
      int index2;
      if ((index2 = inextp + 1) >= 56)
        index2 = 1;
      int num = this.SeedArray[index1] - this.SeedArray[index2];
      if (num == int.MaxValue)
        --num;
      if (num < 0)
        num += int.MaxValue;
      this.SeedArray[index1] = num;
      this.inext = index1;
      this.inextp = index2;
      return num;
    }

    public int Next() => this.InternalSample();

    public int Next(int maxValue)
    {
      if (maxValue < 0)
        throw new ArgumentOutOfRangeException(nameof (maxValue));
      return (int) (this.Sample() * (double) maxValue);
    }

    public int Next(int minValue, int maxValue)
    {
      if (minValue > maxValue)
        throw new ArgumentOutOfRangeException(nameof (minValue));
      long num = (long) (maxValue - minValue);
      return num <= (long) int.MaxValue ? (int) (this.Sample() * (double) num) + minValue : (int) (long) (this.GetSampleForLargeRange() * (double) num) + minValue;
    }

    public long NextLong()
    {
      this.NextBytes(this.m_tmpLongArray);
      return BitConverter.ToInt64(this.m_tmpLongArray, 0);
    }

    public void NextBytes(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      for (int index = 0; index < buffer.Length; ++index)
        buffer[index] = (byte) (this.InternalSample() % 256);
    }

    public float NextFloat() => (float) this.NextDouble();

    public double NextDouble() => this.Sample();

    protected double Sample() => (double) this.InternalSample() * 4.6566128752458E-10;

    public float GetRandomSign() => (float) Math.Sign((float) this.NextDouble() - 0.5f);

    public float GetRandomFloat(float minValue, float maxValue) => this.NextFloat() * (maxValue - minValue) + minValue;

    public struct State
    {
      public int Inext;
      public int Inextp;
      public unsafe fixed int Seed[56];
    }

    public struct StateToken : IDisposable
    {
      private MyRandom m_random;
      private MyRandom.State m_state;

      public StateToken(MyRandom random)
      {
        this.m_random = random;
        random.GetState(out this.m_state);
      }

      public StateToken(MyRandom random, int newSeed)
      {
        this.m_random = random;
        random.GetState(out this.m_state);
        random.SetSeed(newSeed);
      }

      public void Dispose()
      {
        if (this.m_random == null)
          return;
        this.m_random.SetState(ref this.m_state);
      }
    }

    protected class VRage_Library_Utils_MyRandom\u003C\u003Einext\u003C\u003EAccessor : IMemberAccessor<MyRandom, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyRandom owner, in int value) => owner.inext = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyRandom owner, out int value) => value = owner.inext;
    }

    protected class VRage_Library_Utils_MyRandom\u003C\u003Einextp\u003C\u003EAccessor : IMemberAccessor<MyRandom, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyRandom owner, in int value) => owner.inextp = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyRandom owner, out int value) => value = owner.inextp;
    }

    protected class VRage_Library_Utils_MyRandom\u003C\u003ESeedArray\u003C\u003EAccessor : IMemberAccessor<MyRandom, int[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyRandom owner, in int[] value) => owner.SeedArray = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyRandom owner, out int[] value) => value = owner.SeedArray;
    }

    protected class VRage_Library_Utils_MyRandom\u003C\u003Em_tmpLongArray\u003C\u003EAccessor : IMemberAccessor<MyRandom, byte[]>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyRandom owner, in byte[] value) => owner.m_tmpLongArray = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyRandom owner, out byte[] value) => value = owner.m_tmpLongArray;
    }
  }
}
