// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyStoreSellItemResult
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Game.Entities.Blocks
{
  [Serializable]
  public class MyStoreSellItemResult
  {
    public long ItemId { get; set; }

    public int Amount { get; set; }

    public MyStoreSellItemResults Result { get; set; }

    protected class Sandbox_Game_Entities_Blocks_MyStoreSellItemResult\u003C\u003EItemId\u003C\u003EAccessor : IMemberAccessor<MyStoreSellItemResult, long>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreSellItemResult owner, in long value) => owner.ItemId = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreSellItemResult owner, out long value) => value = owner.ItemId;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreSellItemResult\u003C\u003EAmount\u003C\u003EAccessor : IMemberAccessor<MyStoreSellItemResult, int>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreSellItemResult owner, in int value) => owner.Amount = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreSellItemResult owner, out int value) => value = owner.Amount;
    }

    protected class Sandbox_Game_Entities_Blocks_MyStoreSellItemResult\u003C\u003EResult\u003C\u003EAccessor : IMemberAccessor<MyStoreSellItemResult, MyStoreSellItemResults>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyStoreSellItemResult owner, in MyStoreSellItemResults value) => owner.Result = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyStoreSellItemResult owner, out MyStoreSellItemResults value) => value = owner.Result;
    }
  }
}
