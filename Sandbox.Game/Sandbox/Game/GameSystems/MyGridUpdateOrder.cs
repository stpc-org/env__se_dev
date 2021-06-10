﻿// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridUpdateOrder
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

namespace Sandbox.Game.GameSystems
{
  public static class MyGridUpdateOrder
  {
    public const int BeforeSimulation = 1;
    public const int UpdatePredictionFlag = 2;
    public const int EarlyDeactivationCheck = 3;
    public const int GridSystems = 4;
    public const int Control = 4;
    public const int JumpSystem = 5;
    public const int Thrusters = 6;
    public const int Gyros = 7;
    public const int Terminal = 8;
    public const int Wheels = 9;
    public const int Conveyors = 10;
    public const int Cameras = 11;
    public const int GasSystem = 12;
    public const int Sounds = 13;
    public const int ResourceDistributor = 14;
    public const int LazyUpdates = 15;
    public const int SendBones = 16;
    public const int ApplyDamage = 17;
    public const int DetectDisconnects = 18;
    public const int RemoveUnusedBones = 19;
    public const int OwnershipRecalculateOwners = 20;
    public const int Physics = 21;
    public const int PhysicsEffects = 22;
    public const int PhysicsContacts = 23;
    public const int PhysicsDirty = 24;
    public const int PhysicsMass = 25;
    public const int PhysicsToiOptimizer = 26;
    public const int Replay = 27;
    public const int ApplyDeformation = 1;
    public const int SendRemovedBlocks = 2;
    public const int TrashUpdate = 3;
    public const int TestDynamic = 4;
    public const int ClientPredictionStaticCheck = 5;
    public const int InventoryMassUpdate = 6;
    public const int UpdateMergingGrids = 7;
    public const int DirtyRegion = 8;
    public const int HasAnyBlocks = 2147483647;
  }
}
