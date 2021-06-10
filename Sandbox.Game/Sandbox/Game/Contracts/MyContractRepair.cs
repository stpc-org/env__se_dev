// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Contracts.MyContractRepair
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Components.Contracts;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Utils;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Contracts
{
  [MyContractDescriptor(typeof (MyObjectBuilder_ContractRepair))]
  public class MyContractRepair : MyContract
  {
    public static readonly int DISPOSE_TIME_IN_S = 10;
    private bool m_isBeingDisposed;
    private float m_disposeTime;
    private MyTimeSpan? DisposeTime;

    public Vector3D GridPosition { get; private set; }

    public long GridId { get; private set; }

    public string PrefabName { get; private set; }

    public HashSet<Vector3I> BlocksToRepair { get; private set; }

    public int UnrepairedBlockCount { get; private set; }

    public bool KeepGridAtTheEnd { get; private set; }

    public override MyObjectBuilder_Contract GetObjectBuilder()
    {
      MyObjectBuilder_Contract objectBuilder = base.GetObjectBuilder();
      MyObjectBuilder_ContractRepair builderContractRepair = objectBuilder as MyObjectBuilder_ContractRepair;
      builderContractRepair.GridPosition = (SerializableVector3D) this.GridPosition;
      builderContractRepair.GridId = this.GridId;
      builderContractRepair.PrefabName = this.PrefabName;
      builderContractRepair.KeepGridAtTheEnd = this.KeepGridAtTheEnd;
      builderContractRepair.UnrepairedBlockCount = this.UnrepairedBlockCount;
      builderContractRepair.BlocksToRepair = new MySerializableList<Vector3I>();
      foreach (Vector3I vector3I in this.BlocksToRepair)
        builderContractRepair.BlocksToRepair.Add(vector3I);
      return objectBuilder;
    }

    public override void Init(MyObjectBuilder_Contract ob)
    {
      base.Init(ob);
      if (!(ob is MyObjectBuilder_ContractRepair builderContractRepair))
        return;
      this.GridPosition = (Vector3D) builderContractRepair.GridPosition;
      this.GridId = builderContractRepair.GridId;
      this.PrefabName = builderContractRepair.PrefabName;
      this.KeepGridAtTheEnd = builderContractRepair.KeepGridAtTheEnd;
      this.UnrepairedBlockCount = builderContractRepair.UnrepairedBlockCount;
      this.BlocksToRepair = new HashSet<Vector3I>();
      foreach (Vector3I vector3I in (List<Vector3I>) builderContractRepair.BlocksToRepair)
        this.BlocksToRepair.Add(vector3I);
    }

    public override void BeforeStart()
    {
      base.BeforeStart();
      if (this.State != MyContractStateEnum.Active || !(Sandbox.Game.Entities.MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return;
      this.SubscribeToBlocks(entityById);
      entityById.OnBlockRemoved += new Action<MySlimBlock>(this.BlockRemoved);
      if (!this.CanBeFinished)
        return;
      int num = (int) this.Finish();
    }

    public override bool CanBeFinished_Internal() => base.CanBeFinished_Internal() && this.UnrepairedBlockCount <= 0;

    protected override void Activate_Internal(MyTimeSpan timeOfActivation)
    {
      base.Activate_Internal(timeOfActivation);
      if (this.GridId <= 0L)
      {
        if (string.IsNullOrEmpty(this.PrefabName))
          this.Fail();
        else
          this.SpawnPrefab(this.PrefabName);
      }
      else if (Sandbox.Game.Entities.MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById)
      {
        MyGps gps = this.PrepareGPS(entityById);
        foreach (long owner in this.Owners)
          MySession.Static.Gpss.SendAddGps(owner, ref gps, this.GridId);
        this.ScanForBlocksToRepair(entityById);
        this.SubscribeToBlocks(entityById);
        entityById.OnBlockRemoved += new Action<MySlimBlock>(this.BlockRemoved);
        if (!this.CanBeFinished)
          return;
        int num = (int) this.Finish();
      }
      else
        this.Fail();
    }

    private MyGps PrepareGPS(MyCubeGrid grid) => new MyGps()
    {
      DisplayName = MyTexts.GetString(MySpaceTexts.Contract_Repair_GpsName),
      Name = MyTexts.GetString(MySpaceTexts.Contract_Repair_GpsName),
      Description = MyTexts.GetString(MySpaceTexts.Contract_Repair_GpsDescription),
      Coords = this.GridPosition,
      ShowOnHud = true,
      DiscardAt = new TimeSpan?(),
      GPSColor = Color.DarkOrange,
      ContractId = this.Id
    };

    protected void SpawnPrefab(string name)
    {
      IMyFaction factionById = MySession.Static.Factions.TryGetFactionById(this.StartFaction);
      if (factionById == null)
      {
        MyLog.Default.Error("Contract - Repair: Starting faction is not in factions!!!\n Cannot spawn prefab.");
      }
      else
      {
        Vector3 v = Vector3.Normalize(MyUtils.GetRandomVector3());
        Vector3 perpendicularVector = Vector3.CalculatePerpendicularVector(v);
        MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties()
        {
          Position = this.GridPosition,
          Forward = v,
          Up = perpendicularVector,
          PrefabName = name,
          OwnerId = factionById.FounderId,
          Color = factionById.CustomColor,
          SpawningOptions = SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix,
          UpdateSync = true
        };
        MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, (Action) (() =>
        {
          if (spawnProperties.ResultList == null || spawnProperties.ResultList.Count == 0 || spawnProperties.ResultList.Count > 1)
            return;
          MyCubeGrid result = spawnProperties.ResultList[0];
          this.GridId = result.EntityId;
          MyGps gps = this.PrepareGPS(result);
          foreach (long owner in this.Owners)
            MySession.Static.Gpss.SendAddGps(owner, ref gps, this.GridId);
          this.ScanForBlocksToRepair(result);
          this.SubscribeToBlocks(result);
          result.OnBlockRemoved += new Action<MySlimBlock>(this.BlockRemoved);
          if (!this.CanBeFinished)
            return;
          int num = (int) this.Finish();
        }));
      }
    }

    private void BlockRemoved(MySlimBlock obj)
    {
      obj.UnsubscribeFromIsFunctionalChanged(new Action<MySlimBlock>(this.BlockFunctionalityChanged));
      if (this.BlocksToRepair.Contains(obj.Position))
        this.BlocksToRepair.Remove(obj.Position);
      if (this.State != MyContractStateEnum.Active)
        return;
      this.Fail();
    }

    private void ScanForBlocksToRepair(MyCubeGrid grid)
    {
      foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
      {
        if (!cubeBlock.ComponentStack.IsFunctional)
          this.BlocksToRepair.Add(cubeBlock.Position);
      }
      this.UnrepairedBlockCount = this.BlocksToRepair.Count;
    }

    private void ClearBlocksForRepair()
    {
      this.BlocksToRepair.Clear();
      this.UnrepairedBlockCount = 0;
    }

    private void SubscribeToBlocks(MyCubeGrid grid)
    {
      foreach (MySlimBlock cubeBlock in grid.CubeBlocks)
      {
        if (!cubeBlock.ComponentStack.IsFunctional)
        {
          this.BlocksToRepair.Add(cubeBlock.Position);
          cubeBlock.SubscribeForIsFunctionalChanged(new Action<MySlimBlock>(this.BlockFunctionalityChanged));
        }
      }
    }

    private void UnsubscribeFromBlocks(MyCubeGrid grid)
    {
      foreach (Vector3I pos in this.BlocksToRepair)
        grid.GetCubeBlock(pos)?.UnsubscribeFromIsFunctionalChanged(new Action<MySlimBlock>(this.BlockFunctionalityChanged));
    }

    private void BlockFunctionalityChanged(MySlimBlock block)
    {
      if (block.ComponentStack.IsFunctional)
        --this.UnrepairedBlockCount;
      else
        ++this.UnrepairedBlockCount;
      if (!this.CanBeFinished)
        return;
      int num = (int) this.Finish();
    }

    protected override void FailFor_Internal(long player, bool abandon = false)
    {
      base.FailFor_Internal(player, abandon);
      this.RemoveGpsForPlayer(player);
    }

    protected override void FinishFor_Internal(long player, int rewardeeCount)
    {
      base.FinishFor_Internal(player, rewardeeCount);
      this.RemoveGpsForPlayer(player);
    }

    protected override void CleanUp_Internal()
    {
      float num = 0.0f;
      if (Sandbox.Game.Entities.MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById)
      {
        entityById.OnBlockRemoved -= new Action<MySlimBlock>(this.BlockRemoved);
        this.UnsubscribeFromBlocks(entityById);
        this.ClearBlocksForRepair();
        if (this.State == MyContractStateEnum.Finished)
        {
          if (!this.KeepGridAtTheEnd)
          {
            this.CreateParticleEffectOnEntity("Warp", entityById.EntityId, true);
            num = 10f;
          }
        }
        else if (!this.KeepGridAtTheEnd)
        {
          this.CreateParticleEffectOnEntity("Explosion_Warhead_50", entityById.EntityId, false);
          num = 2f;
        }
        else
        {
          this.CreateParticleEffectOnEntity("", entityById.EntityId, false);
          num = 0.0f;
        }
      }
      this.m_disposeTime = num;
      this.m_isBeingDisposed = true;
      this.State = MyContractStateEnum.ToBeDisposed;
    }

    public override void Update(MyTimeSpan currentTime)
    {
      base.Update(currentTime);
      if (this.State != MyContractStateEnum.ToBeDisposed)
        return;
      bool flag = false;
      if (this.m_isBeingDisposed)
      {
        if (!this.DisposeTime.HasValue)
          this.DisposeTime = new MyTimeSpan?(currentTime + MyTimeSpan.FromSeconds((double) this.m_disposeTime));
        if (this.DisposeTime.Value <= currentTime)
          flag = true;
      }
      else
        flag = true;
      if (!flag)
        return;
      this.State = MyContractStateEnum.Disposed;
      if (this.KeepGridAtTheEnd || !(Sandbox.Game.Entities.MyEntities.GetEntityById(this.GridId) is MyCubeGrid entityById))
        return;
      entityById.DismountAllCockpits();
      entityById.Close();
    }

    public override MyDefinitionId? GetDefinitionId() => new MyDefinitionId?(new MyDefinitionId((MyObjectBuilderType) typeof (MyObjectBuilder_ContractTypeDefinition), "Repair"));

    private void RemoveGpsForPlayer(long identityId)
    {
      MyGps gpsByContractId = MySession.Static.Gpss.GetGpsByContractId(identityId, this.Id);
      if (gpsByContractId == null)
        return;
      MySession.Static.Gpss.SendDelete(identityId, gpsByContractId.Hash);
    }
  }
}
