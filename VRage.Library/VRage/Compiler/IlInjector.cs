// Decompiled with JetBrains decompiler
// Type: VRage.Compiler.IlInjector
// Assembly: VRage.Library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F6AE17F7-DAEC-41A1-943B-7247EBA3EAA4
// Assembly location: D:\Files\library_development\lib_se\VRage.Library.dll

using System;
using System.Runtime.CompilerServices;

namespace VRage.Compiler
{
  public class IlInjector
  {
    private static IlInjector.InstructionCounterHandle m_instructionCounterHandle = new IlInjector.InstructionCounterHandle();
    private static bool m_isDead;
    private static int m_numInstructions = 0;
    private static int m_numMaxInstructions = 0;
    private static ulong m_minAllowedStackPointer = 0;
    private const int ALLOWED_STACK_CONSUMPTION = 1048576;
    private static int m_numMethodCalls = 0;
    private static int m_maxMethodCalls = 0;
    private static int m_maxCallChainDepth = 1000;
    private static int m_callChainDepth = 0;

    public static int NumInstructions => IlInjector.m_numInstructions;

    public static bool IsWithinRunBlock() => IlInjector.m_instructionCounterHandle.Depth > 0;

    public static IlInjector.ICounterHandle BeginRunBlock(
      int maxInstructions,
      int maxMethodCalls)
    {
      IlInjector.m_instructionCounterHandle.AddRef(maxInstructions, maxMethodCalls);
      return (IlInjector.ICounterHandle) IlInjector.m_instructionCounterHandle;
    }

    private static void RestartCountingInstructions(int maxInstructions)
    {
      IlInjector.m_numInstructions = 0;
      IlInjector.m_numMaxInstructions = maxInstructions;
      IlInjector.m_minAllowedStackPointer = IlInjector.GetStackPointer() - 1048576UL;
    }

    private static void ResetIsDead() => IlInjector.m_isDead = false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CountInstructions()
    {
      ++IlInjector.m_numInstructions;
      if (IlInjector.m_numInstructions > IlInjector.m_numMaxInstructions)
      {
        IlInjector.m_isDead = true;
        throw new ScriptOutOfRangeException();
      }
    }

    public static unsafe ulong GetStackPointer() => (ulong) &0;

    private static void RestartCountingMethods(int maxMethodCalls)
    {
      IlInjector.m_numMethodCalls = 0;
      IlInjector.m_maxMethodCalls = maxMethodCalls;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CountMethodCalls()
    {
      ++IlInjector.m_numMethodCalls;
      if (IlInjector.m_numMethodCalls > IlInjector.m_maxMethodCalls)
      {
        IlInjector.m_isDead = true;
        throw new ScriptOutOfRangeException();
      }
    }

    public static int CallChainDepth => IlInjector.m_callChainDepth;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnterMethod_Profile([CallerMemberName] string member = "") => IlInjector.EnterMethod();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnterMethod()
    {
      ++IlInjector.m_callChainDepth;
      ulong stackPointer = IlInjector.GetStackPointer();
      if (IlInjector.m_callChainDepth > IlInjector.m_maxCallChainDepth || stackPointer < IlInjector.m_minAllowedStackPointer)
      {
        --IlInjector.m_callChainDepth;
        IlInjector.m_isDead = true;
        throw new ScriptOutOfRangeException();
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ExitMethod_Profile() => IlInjector.ExitMethod();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ExitMethod() => --IlInjector.m_callChainDepth;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T YieldGuard_Profile<T>(T value) => IlInjector.YieldGuard<T>(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T YieldGuard<T>(T value)
    {
      IlInjector.ExitMethod();
      return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDead() => IlInjector.m_isDead;

    public interface ICounterHandle : IDisposable
    {
      int InstructionCount { get; }

      int MaxInstructionCount { get; }

      int MethodCallCount { get; }

      int MaxMethodCallCount { get; }

      int Depth { get; }
    }

    private class InstructionCounterHandle : IlInjector.ICounterHandle, IDisposable
    {
      private int m_runDepth;

      public int Depth => this.m_runDepth;

      public void AddRef(int maxInstructions, int maxMethodCount)
      {
        ++this.m_runDepth;
        if (this.m_runDepth != 1)
          return;
        IlInjector.RestartCountingInstructions(maxInstructions);
        IlInjector.RestartCountingMethods(maxMethodCount);
        IlInjector.ResetIsDead();
      }

      public void Dispose()
      {
        if (this.m_runDepth <= 0)
          return;
        --this.m_runDepth;
      }

      public int InstructionCount => IlInjector.m_numInstructions;

      public int MaxInstructionCount => IlInjector.m_numMaxInstructions;

      public int MethodCallCount => IlInjector.m_callChainDepth;

      public int MaxMethodCallCount => IlInjector.m_maxCallChainDepth;
    }
  }
}
