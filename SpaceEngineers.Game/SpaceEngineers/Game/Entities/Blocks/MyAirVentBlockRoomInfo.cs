// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyAirVentBlockRoomInfo
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [Serializable]
  public struct MyAirVentBlockRoomInfo : IEquatable<MyAirVentBlockRoomInfo>
  {
    public bool IsRoomAirtight;
    public float OxygenLevel;
    public float RoomEnvironmentOxygen;

    public MyAirVentBlockRoomInfo(
      bool isRoomAirtight,
      float oxygenLevel,
      float roomEnvironmentOxygen)
    {
      this.IsRoomAirtight = isRoomAirtight;
      this.OxygenLevel = oxygenLevel;
      this.RoomEnvironmentOxygen = roomEnvironmentOxygen;
    }

    public bool Equals(MyAirVentBlockRoomInfo other) => this.IsRoomAirtight == other.IsRoomAirtight && MathHelper.IsEqual(this.OxygenLevel, other.OxygenLevel) && MathHelper.IsEqual(this.RoomEnvironmentOxygen, other.RoomEnvironmentOxygen);

    protected class SpaceEngineers_Game_Entities_Blocks_MyAirVentBlockRoomInfo\u003C\u003EIsRoomAirtight\u003C\u003EAccessor : IMemberAccessor<MyAirVentBlockRoomInfo, bool>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAirVentBlockRoomInfo owner, in bool value) => owner.IsRoomAirtight = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAirVentBlockRoomInfo owner, out bool value) => value = owner.IsRoomAirtight;
    }

    protected class SpaceEngineers_Game_Entities_Blocks_MyAirVentBlockRoomInfo\u003C\u003EOxygenLevel\u003C\u003EAccessor : IMemberAccessor<MyAirVentBlockRoomInfo, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAirVentBlockRoomInfo owner, in float value) => owner.OxygenLevel = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAirVentBlockRoomInfo owner, out float value) => value = owner.OxygenLevel;
    }

    protected class SpaceEngineers_Game_Entities_Blocks_MyAirVentBlockRoomInfo\u003C\u003ERoomEnvironmentOxygen\u003C\u003EAccessor : IMemberAccessor<MyAirVentBlockRoomInfo, float>
    {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Set(ref MyAirVentBlockRoomInfo owner, in float value) => owner.RoomEnvironmentOxygen = value;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public virtual void Get(ref MyAirVentBlockRoomInfo owner, out float value) => value = owner.RoomEnvironmentOxygen;
    }
  }
}
