// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.CyclingOptions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Game.Entities
{
  [Serializable]
  public struct CyclingOptions
  {
    public bool Enabled;
    public bool OnlySmallGrids;
    public bool OnlyLargeGrids;

    protected class Sandbox_Game_Entities_CyclingOptions\u003C\u003EEnabled\u003C\u003EAccessor : IMemberAccessor<CyclingOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CyclingOptions owner, in bool value) => owner.Enabled = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CyclingOptions owner, out bool value) => value = owner.Enabled;
    }

    protected class Sandbox_Game_Entities_CyclingOptions\u003C\u003EOnlySmallGrids\u003C\u003EAccessor : IMemberAccessor<CyclingOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CyclingOptions owner, in bool value) => owner.OnlySmallGrids = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CyclingOptions owner, out bool value) => value = owner.OnlySmallGrids;
    }

    protected class Sandbox_Game_Entities_CyclingOptions\u003C\u003EOnlyLargeGrids\u003C\u003EAccessor : IMemberAccessor<CyclingOptions, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref CyclingOptions owner, in bool value) => owner.OnlyLargeGrids = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref CyclingOptions owner, out bool value) => value = owner.OnlyLargeGrids;
    }
  }
}
