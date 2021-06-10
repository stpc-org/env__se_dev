// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.AI.Actions.MyHumanoidBotActions
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.AI.Logic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.AI;
using VRage.Library.Utils;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.AI.Actions
{
  public abstract class MyHumanoidBotActions : MyAgentActions
  {
    private MyTimeSpan m_reservationTimeOut;
    private const int RESERVATION_WAIT_TIMEOUT_SECONDS = 3;

    protected MyHumanoidBot Bot => base.Bot as MyHumanoidBot;

    protected MyHumanoidBotActions(MyAgentBot humanoidBot)
      : base(humanoidBot)
    {
    }

    [MyBehaviorTreeAction("PlaySound", ReturnsRunning = false)]
    protected MyBehaviorTreeState PlaySound([BTParam] string soundName)
    {
      this.Bot.HumanoidEntity.SoundComp.StartSecondarySound(soundName, true);
      return MyBehaviorTreeState.SUCCESS;
    }

    [MyBehaviorTreeAction("EquipItem", ReturnsRunning = false)]
    protected MyBehaviorTreeState EquipItem([BTParam] string itemName)
    {
      if (string.IsNullOrEmpty(itemName))
        return MyBehaviorTreeState.FAILURE;
      MyCharacter humanoidEntity = this.Bot.HumanoidEntity;
      if (humanoidEntity.CurrentWeapon != null && humanoidEntity.CurrentWeapon.DefinitionId.SubtypeName == itemName)
        return MyBehaviorTreeState.SUCCESS;
      MyObjectBuilder_PhysicalGunObject newObject = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_PhysicalGunObject>(itemName);
      MyDefinitionId id = newObject.GetId();
      if (!MyEntityExtensions.GetInventory(humanoidEntity).ContainItems((MyFixedPoint) 1, (MyObjectBuilder_PhysicalObject) newObject) && humanoidEntity.WeaponTakesBuilderFromInventory(new MyDefinitionId?(id)))
        return MyBehaviorTreeState.FAILURE;
      humanoidEntity.SwitchToWeapon(id);
      return MyBehaviorTreeState.SUCCESS;
    }

    private void ReservationHandler(
      ref MyAiTargetManager.ReservedEntityData reservedEntity,
      bool success)
    {
      if (this.Bot?.HumanoidLogic == null || this.Bot.Player == null || this.Bot.Player.Id.SerialId != reservedEntity.ReserverId.SerialId)
        return;
      MyHumanoidBotLogic humanoidLogic = this.Bot.HumanoidLogic;
      humanoidLogic.ReservationStatus = MyReservationStatus.FAILURE;
      if (!success || reservedEntity.EntityId != humanoidLogic.ReservationEntityData.EntityId || reservedEntity.Type == MyReservedEntityType.ENVIRONMENT_ITEM && reservedEntity.LocalId != humanoidLogic.ReservationEntityData.LocalId || reservedEntity.Type == MyReservedEntityType.VOXEL && reservedEntity.GridPos != humanoidLogic.ReservationEntityData.GridPos)
        return;
      humanoidLogic.ReservationStatus = MyReservationStatus.SUCCESS;
    }

    private void AreaReservationHandler(
      ref MyAiTargetManager.ReservedAreaData reservedArea,
      bool success)
    {
      if (this.Bot?.HumanoidLogic == null || this.Bot.Player == null || this.Bot.Player.Id.SerialId != reservedArea.ReserverId.SerialId)
        return;
      MyHumanoidBotLogic humanoidLogic = this.Bot.HumanoidLogic;
      humanoidLogic.ReservationStatus = MyReservationStatus.FAILURE;
      if (!success || !(reservedArea.WorldPosition == humanoidLogic.ReservationAreaData.WorldPosition) || (double) reservedArea.Radius != (double) humanoidLogic.ReservationAreaData.Radius)
        return;
      humanoidLogic.ReservationStatus = MyReservationStatus.SUCCESS;
    }

    [MyBehaviorTreeAction("TryReserveEntity")]
    protected MyBehaviorTreeState TryReserveEntity(
      [BTIn] ref MyBBMemoryTarget inTarget,
      [BTParam] int timeMs)
    {
      if (this.Bot?.Player == null)
        return MyBehaviorTreeState.FAILURE;
      MyHumanoidBotLogic humanoidLogic = this.Bot.HumanoidLogic;
      MyBBMemoryTarget myBbMemoryTarget = inTarget;
      if ((myBbMemoryTarget != null ? (myBbMemoryTarget.EntityId.HasValue ? 1 : 0) : 0) != 0 && inTarget.TargetType != MyAiTargetEnum.POSITION && inTarget.TargetType != MyAiTargetEnum.NO_TARGET)
      {
        switch (humanoidLogic.ReservationStatus)
        {
          case MyReservationStatus.NONE:
            switch (inTarget.TargetType)
            {
              case MyAiTargetEnum.GRID:
              case MyAiTargetEnum.CUBE:
              case MyAiTargetEnum.CHARACTER:
              case MyAiTargetEnum.ENTITY:
                humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
                humanoidLogic.ReservationEntityData = new MyAiTargetManager.ReservedEntityData()
                {
                  Type = MyReservedEntityType.ENTITY,
                  EntityId = inTarget.EntityId.Value,
                  ReservationTimer = (long) timeMs,
                  ReserverId = new MyPlayer.PlayerId(this.Bot.Player.Id.SteamId, this.Bot.Player.Id.SerialId)
                };
                MyAiTargetManager.OnReservationResult += new MyAiTargetManager.ReservationHandler(this.ReservationHandler);
                MyAiTargetManager.Static.RequestEntityReservation(humanoidLogic.ReservationEntityData.EntityId, humanoidLogic.ReservationEntityData.ReservationTimer, this.Bot.Player.Id.SerialId);
                break;
              case MyAiTargetEnum.ENVIRONMENT_ITEM:
                humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
                humanoidLogic.ReservationEntityData = new MyAiTargetManager.ReservedEntityData()
                {
                  Type = MyReservedEntityType.ENVIRONMENT_ITEM,
                  EntityId = inTarget.EntityId.Value,
                  LocalId = inTarget.TreeId.Value,
                  ReservationTimer = (long) timeMs,
                  ReserverId = new MyPlayer.PlayerId(this.Bot.Player.Id.SteamId, this.Bot.Player.Id.SerialId)
                };
                MyAiTargetManager.OnReservationResult += new MyAiTargetManager.ReservationHandler(this.ReservationHandler);
                MyAiTargetManager.Static.RequestEnvironmentItemReservation(humanoidLogic.ReservationEntityData.EntityId, humanoidLogic.ReservationEntityData.LocalId, humanoidLogic.ReservationEntityData.ReservationTimer, this.Bot.Player.Id.SerialId);
                break;
              case MyAiTargetEnum.VOXEL:
                humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
                humanoidLogic.ReservationEntityData = new MyAiTargetManager.ReservedEntityData()
                {
                  Type = MyReservedEntityType.VOXEL,
                  EntityId = inTarget.EntityId.Value,
                  GridPos = inTarget.VoxelPosition,
                  ReservationTimer = (long) timeMs,
                  ReserverId = new MyPlayer.PlayerId(this.Bot.Player.Id.SteamId, this.Bot.Player.Id.SerialId)
                };
                MyAiTargetManager.OnReservationResult += new MyAiTargetManager.ReservationHandler(this.ReservationHandler);
                MyAiTargetManager.Static.RequestVoxelPositionReservation(humanoidLogic.ReservationEntityData.EntityId, humanoidLogic.ReservationEntityData.GridPos, humanoidLogic.ReservationEntityData.ReservationTimer, this.Bot.Player.Id.SerialId);
                break;
              default:
                humanoidLogic.ReservationStatus = MyReservationStatus.FAILURE;
                break;
            }
            this.m_reservationTimeOut = MySandboxGame.Static.TotalTime + MyTimeSpan.FromSeconds(3.0);
            break;
          case MyReservationStatus.WAITING:
            if (this.m_reservationTimeOut < MySandboxGame.Static.TotalTime)
            {
              humanoidLogic.ReservationStatus = MyReservationStatus.FAILURE;
              break;
            }
            break;
        }
      }
      switch (humanoidLogic.ReservationStatus)
      {
        case MyReservationStatus.WAITING:
          return MyBehaviorTreeState.RUNNING;
        case MyReservationStatus.SUCCESS:
          return MyBehaviorTreeState.SUCCESS;
        default:
          return MyBehaviorTreeState.FAILURE;
      }
    }

    [MyBehaviorTreeAction("TryReserveEntity", MyBehaviorTreeActionType.POST)]
    protected void Post_TryReserveEntity()
    {
      if (this.Bot?.HumanoidLogic == null)
        return;
      MyHumanoidBotLogic humanoidLogic = this.Bot.HumanoidLogic;
      if (humanoidLogic.ReservationStatus != MyReservationStatus.NONE)
        MyAiTargetManager.OnReservationResult -= new MyAiTargetManager.ReservationHandler(this.ReservationHandler);
      humanoidLogic.ReservationStatus = MyReservationStatus.NONE;
    }

    [MyBehaviorTreeAction("TryReserveArea")]
    protected MyBehaviorTreeState TryReserveAreaAroundEntity(
      [BTParam] string areaName,
      [BTParam] float radius,
      [BTParam] int timeMs)
    {
      MyHumanoidBotLogic humanoidLogic = this.Bot.HumanoidLogic;
      MyBehaviorTreeState behaviorTreeState = MyBehaviorTreeState.FAILURE;
      if (humanoidLogic != null)
      {
        switch (humanoidLogic.ReservationStatus)
        {
          case MyReservationStatus.NONE:
            humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
            MyHumanoidBotLogic humanoidBotLogic = humanoidLogic;
            MyAiTargetManager.ReservedAreaData reservedAreaData1 = new MyAiTargetManager.ReservedAreaData();
            ref MyAiTargetManager.ReservedAreaData local = ref reservedAreaData1;
            MatrixD worldMatrix = this.Bot.HumanoidEntity.WorldMatrix;
            Vector3D translation1 = worldMatrix.Translation;
            local.WorldPosition = translation1;
            reservedAreaData1.Radius = radius;
            reservedAreaData1.ReservationTimer = MyTimeSpan.FromMilliseconds((double) timeMs);
            reservedAreaData1.ReserverId = new MyPlayer.PlayerId(this.Bot.Player.Id.SteamId, this.Bot.Player.Id.SerialId);
            MyAiTargetManager.ReservedAreaData reservedAreaData2 = reservedAreaData1;
            humanoidBotLogic.ReservationAreaData = reservedAreaData2;
            MyAiTargetManager.OnAreaReservationResult += new MyAiTargetManager.AreaReservationHandler(this.AreaReservationHandler);
            MyAiTargetManager myAiTargetManager = MyAiTargetManager.Static;
            string reservationName = areaName;
            worldMatrix = this.Bot.HumanoidEntity.WorldMatrix;
            Vector3D translation2 = worldMatrix.Translation;
            double num = (double) radius;
            long reservationTimeMs = (long) timeMs;
            int serialId = this.Bot.Player.Id.SerialId;
            myAiTargetManager.RequestAreaReservation(reservationName, translation2, (float) num, reservationTimeMs, serialId);
            this.m_reservationTimeOut = MySandboxGame.Static.TotalTime + MyTimeSpan.FromSeconds(3.0);
            humanoidLogic.ReservationStatus = MyReservationStatus.WAITING;
            behaviorTreeState = MyBehaviorTreeState.RUNNING;
            break;
          case MyReservationStatus.WAITING:
            behaviorTreeState = !(this.m_reservationTimeOut < MySandboxGame.Static.TotalTime) ? MyBehaviorTreeState.RUNNING : MyBehaviorTreeState.FAILURE;
            break;
          case MyReservationStatus.SUCCESS:
            behaviorTreeState = MyBehaviorTreeState.SUCCESS;
            break;
          case MyReservationStatus.FAILURE:
            behaviorTreeState = MyBehaviorTreeState.FAILURE;
            break;
        }
      }
      return behaviorTreeState;
    }

    [MyBehaviorTreeAction("TryReserveArea", MyBehaviorTreeActionType.POST)]
    protected void Post_TryReserveArea()
    {
      if (this.Bot.HumanoidLogic == null)
        return;
      MyHumanoidBotLogic humanoidLogic = this.Bot.HumanoidLogic;
      if (humanoidLogic.ReservationStatus != MyReservationStatus.NONE)
        MyAiTargetManager.OnAreaReservationResult -= new MyAiTargetManager.AreaReservationHandler(this.AreaReservationHandler);
      humanoidLogic.ReservationStatus = MyReservationStatus.NONE;
    }

    [MyBehaviorTreeAction("IsInReservedArea", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsInReservedArea([BTParam] string areaName) => MyAiTargetManager.Static.IsInReservedArea(areaName, this.Bot.HumanoidEntity.WorldMatrix.Translation) ? MyBehaviorTreeState.SUCCESS : MyBehaviorTreeState.FAILURE;

    [MyBehaviorTreeAction("IsNotInReservedArea", ReturnsRunning = false)]
    protected MyBehaviorTreeState IsNotInReservedArea([BTParam] string areaName) => MyAiTargetManager.Static.IsInReservedArea(areaName, this.Bot.HumanoidEntity.WorldMatrix.Translation) ? MyBehaviorTreeState.FAILURE : MyBehaviorTreeState.SUCCESS;
  }
}
