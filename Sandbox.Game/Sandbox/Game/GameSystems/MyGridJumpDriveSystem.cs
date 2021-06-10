// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GameSystems.MyGridJumpDriveSystem
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
  [StaticEventOwner]
  public class MyGridJumpDriveSystem : MyUpdateableGridSystem
  {
    public const float JUMP_DRIVE_DELAY = 10f;
    public const double MIN_JUMP_DISTANCE = 5000.0;
    private HashSet<MyJumpDrive> m_jumpDrives = new HashSet<MyJumpDrive>();
    private List<MyEntity> m_entitiesInRange = new List<MyEntity>();
    private List<MyObjectSeed> m_objectsInRange = new List<MyObjectSeed>();
    private List<BoundingBoxD> m_obstaclesInRange = new List<BoundingBoxD>();
    private List<MyCharacter> m_characters = new List<MyCharacter>();
    private Vector3D m_selectedDestination;
    private Vector3D m_jumpDirection;
    private Vector3D m_jumpDirectionNorm;
    private Vector3 m_effectOffset = Vector3.Zero;
    private bool m_isJumping;
    private float m_prevJumpTime;
    private bool m_jumped;
    private long m_userId;
    private float m_jumpTimeLeft;
    private bool m_playEffect;
    private Vector3D? m_savedJumpDirection;
    private float? m_savedRemainingJumpTime;
    private MySoundPair m_chargingSound = new MySoundPair("ShipJumpDriveCharging");
    private MySoundPair m_jumpInSound = new MySoundPair("ShipJumpDriveJumpIn");
    private MySoundPair m_jumpOutSound = new MySoundPair("ShipJumpDriveJumpOut");
    protected MyEntity3DSoundEmitter m_soundEmitter;
    private MyEntity3DSoundEmitter m_soundEmitterJumpIn;
    private MyParticleEffect m_effect;

    public MyGridJumpDriveSystem(MyCubeGrid grid)
      : base(grid)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
        return;
      this.m_soundEmitter = new MyEntity3DSoundEmitter((MyEntity) this.Grid);
      this.m_soundEmitterJumpIn = new MyEntity3DSoundEmitter((MyEntity) this.Grid);
    }

    public void Init(Vector3D? jumpDriveDirection, float? remainingTimeForJump)
    {
      this.m_savedJumpDirection = jumpDriveDirection;
      this.m_savedRemainingJumpTime = remainingTimeForJump;
      if (!jumpDriveDirection.HasValue)
        return;
      this.Schedule();
    }

    public Vector3D? GetJumpDriveDirection() => this.m_isJumping && !this.m_jumped ? new Vector3D?(this.m_jumpDirection) : new Vector3D?();

    internal float? GetRemainingJumpTime() => this.m_isJumping && !this.m_jumped ? new float?(this.m_jumpTimeLeft) : new float?();

    public void RegisterJumpDrive(MyJumpDrive jumpDrive) => this.m_jumpDrives.Add(jumpDrive);

    public void UnregisterJumpDrive(MyJumpDrive jumpDrive)
    {
      this.m_jumpDrives.Remove(jumpDrive);
      MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
    }

    protected override void Update()
    {
      if (this.m_savedJumpDirection.HasValue)
      {
        this.m_selectedDestination = this.m_savedJumpDirection.Value;
        this.m_isJumping = true;
        this.m_jumped = false;
        this.m_jumpTimeLeft = this.m_savedRemainingJumpTime.HasValue ? this.m_savedRemainingJumpTime.Value : 0.0f;
        this.m_savedJumpDirection = new Vector3D?();
        this.m_savedRemainingJumpTime = new float?();
      }
      this.UpdateJumpDriveSystem();
    }

    public double GetMaxJumpDistance(long userId)
    {
      double val1 = 0.0;
      double val2 = 0.0;
      double currentMass = (double) this.Grid.GetCurrentMass();
      foreach (MyJumpDrive jumpDrive in this.m_jumpDrives)
      {
        if (jumpDrive.CanJumpAndHasAccess(userId))
        {
          val1 += jumpDrive.BlockDefinition.MaxJumpDistance;
          val2 += jumpDrive.BlockDefinition.MaxJumpDistance * (jumpDrive.BlockDefinition.MaxJumpMass / currentMass);
        }
      }
      return Math.Min(val1, val2);
    }

    private void DepleteJumpDrives(double distance, long userId)
    {
      double currentMass = (double) this.Grid.GetCurrentMass();
      foreach (MyJumpDrive jumpDrive in this.m_jumpDrives)
      {
        if (jumpDrive.CanJumpAndHasAccess(userId))
        {
          jumpDrive.IsJumping = true;
          double num1 = jumpDrive.BlockDefinition.MaxJumpMass / currentMass;
          if (num1 > 1.0)
            num1 = 1.0;
          double num2 = jumpDrive.BlockDefinition.MaxJumpDistance * num1;
          if (num2 < distance)
          {
            distance -= num2;
            jumpDrive.SetStoredPower(0.0f);
          }
          else
          {
            double num3 = distance / num2;
            jumpDrive.SetStoredPower(1f - (float) num3);
            break;
          }
        }
      }
    }

    private bool IsJumpValid(long userId, out MyGridJumpDriveSystem.MyJumpFailReason reason)
    {
      reason = MyGridJumpDriveSystem.MyJumpFailReason.None;
      if (MyFakes.TESTING_JUMPDRIVE)
        return true;
      if (this.Grid.MarkedForClose)
      {
        reason = MyGridJumpDriveSystem.MyJumpFailReason.Other;
        return false;
      }
      if (!this.Grid.CanBeTeleported(this, out reason))
        return false;
      if (this.GetMaxJumpDistance(userId) >= 5000.0)
        return true;
      reason = MyGridJumpDriveSystem.MyJumpFailReason.ShortDistance;
      return false;
    }

    public void RequestAbort()
    {
      if (!this.m_isJumping || this.m_jumped)
        return;
      this.SendAbortJump();
    }

    public void RequestJump(string destinationName, Vector3D destination, long userId)
    {
      if (!Vector3.IsZero(MyGravityProviderSystem.CalculateNaturalGravityInPoint(this.Grid.WorldMatrix.Translation)))
      {
        MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationCannotJumpFromGravity, 1500);
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
      else if (!Vector3.IsZero(MyGravityProviderSystem.CalculateNaturalGravityInPoint(destination)))
      {
        MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationCannotJumpIntoGravity, 1500);
        MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
      }
      else
      {
        MyGridJumpDriveSystem.MyJumpFailReason reason;
        if (!this.IsJumpValid(userId, out reason))
          this.ShowNotification(reason);
        else if (MySession.Static.Settings.WorldSizeKm > 0 && destination.Length() > (double) (MySession.Static.Settings.WorldSizeKm * 500))
        {
          MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationCannotJumpOutsideWorld, 1500);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
        }
        else
        {
          this.m_selectedDestination = destination;
          double maxJumpDistance = this.GetMaxJumpDistance(userId);
          this.m_jumpDirection = destination - this.Grid.WorldMatrix.Translation;
          Vector3D.Normalize(ref this.m_jumpDirection, out this.m_jumpDirectionNorm);
          double distance = this.m_jumpDirection.Length();
          double actualDistance = distance;
          if (distance > maxJumpDistance)
          {
            double num = maxJumpDistance / distance;
            actualDistance = maxJumpDistance;
            this.m_jumpDirection *= num;
          }
          Vector3D vector3D1 = Vector3D.Normalize(destination - this.Grid.WorldMatrix.Translation);
          Vector3D translation1 = this.Grid.WorldMatrix.Translation;
          Vector3 vector3 = this.Grid.PositionComp.LocalAABB.Extents;
          Vector3D vector3D2 = (double) vector3.Max() * vector3D1;
          Vector3D linePointA = translation1 + vector3D2;
          LineD line = new LineD(linePointA, destination);
          MyIntersectionResultLineTriangleEx? intersectionWithLine = MyEntities.GetIntersectionWithLine(ref line, (MyEntity) this.Grid, (MyEntity) null, true, ignoreObjectsWithoutPhysics: false);
          Vector3D zero1 = Vector3D.Zero;
          Vector3D zero2 = Vector3D.Zero;
          if (intersectionWithLine.HasValue)
          {
            MyEntity entity = intersectionWithLine.Value.Entity as MyEntity;
            Vector3D translation2 = entity.WorldMatrix.Translation;
            Vector3D closestPointOnLine = MyUtils.GetClosestPointOnLine(ref linePointA, ref destination, ref translation2);
            if (intersectionWithLine.Value.Entity is MyPlanet)
            {
              MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationCannotJumpIntoGravity, 1500);
              MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
              return;
            }
            BoundingBox localAabb = entity.PositionComp.LocalAABB;
            vector3 = localAabb.Extents;
            float num1 = vector3.Length();
            Vector3D vector3D3 = closestPointOnLine;
            Vector3D vector3D4 = vector3D1;
            double num2 = (double) num1;
            localAabb = this.Grid.PositionComp.LocalAABB;
            vector3 = localAabb.HalfExtents;
            double num3 = (double) vector3.Length();
            double num4 = num2 + num3;
            Vector3D vector3D5 = vector3D4 * num4;
            destination = vector3D3 - vector3D5;
            this.m_selectedDestination = destination;
            this.m_jumpDirection = this.m_selectedDestination - linePointA;
            Vector3D.Normalize(ref this.m_jumpDirection, out this.m_jumpDirectionNorm);
            actualDistance = this.m_jumpDirection.Length();
          }
          if (actualDistance < 5000.0)
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: this.GetWarningText(actualDistance, intersectionWithLine.HasValue), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning)));
          else
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: this.GetConfirmationText(destinationName, distance, actualDistance, userId, intersectionWithLine.HasValue), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (result =>
            {
              reason = MyGridJumpDriveSystem.MyJumpFailReason.None;
              if (result == MyGuiScreenMessageBox.ResultEnum.YES && this.IsJumpValid(userId, out reason))
                this.RequestJump(this.m_selectedDestination, userId);
              else
                this.SendAbortJump();
            })), size: new Vector2?(new Vector2(0.839375f, 0.3675f))));
          if (!MyFakes.TESTING_JUMPDRIVE)
            return;
          this.m_jumpDirection *= 1000.0;
        }
      }
    }

    private void ShowNotification(MyGridJumpDriveSystem.MyJumpFailReason reason)
    {
      if (Sync.IsDedicated)
        return;
      switch (reason)
      {
        case MyGridJumpDriveSystem.MyJumpFailReason.Static:
          MyHudNotification myHudNotification1 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedStatic, 1500);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification1);
          break;
        case MyGridJumpDriveSystem.MyJumpFailReason.Locked:
          MyHudNotification myHudNotification2 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedLocked, 1500);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification2);
          break;
        case MyGridJumpDriveSystem.MyJumpFailReason.ShortDistance:
          MyHudNotification myHudNotification3 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedShortDistance, 1500);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification3);
          break;
        case MyGridJumpDriveSystem.MyJumpFailReason.AlreadyJumping:
          MyHudNotification myHudNotification4 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedAlreadyJumping, 1500);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification4);
          break;
        case MyGridJumpDriveSystem.MyJumpFailReason.NoLocation:
          MyHudNotification myHudNotification5 = new MyHudNotification(MySpaceTexts.NotificationJumpAbortedNoLocation, 1500);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification5);
          break;
        case MyGridJumpDriveSystem.MyJumpFailReason.Other:
          MyHudNotification myHudNotification6 = new MyHudNotification(MySpaceTexts.NotificationJumpAborted, 1500, "Red", level: MyNotificationLevel.Important);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification6);
          break;
      }
    }

    private StringBuilder GetConfirmationText(
      string name,
      double distance,
      double actualDistance,
      long userId,
      bool obstacleDetected)
    {
      int count = this.m_jumpDrives.Count;
      int num1 = this.m_jumpDrives.Count<MyJumpDrive>((Func<MyJumpDrive, bool>) (x => x.CanJumpAndHasAccess(userId)));
      distance /= 1000.0;
      actualDistance /= 1000.0;
      float num2 = (float) (actualDistance / distance);
      if ((double) num2 > 1.0)
        num2 = 1f;
      this.GetCharactersInBoundingBox(this.Grid.GetPhysicalGroupAABB(), this.m_characters);
      int num3 = 0;
      int num4 = 0;
      foreach (MyCharacter character in this.m_characters)
      {
        if (!character.IsDead)
        {
          ++num3;
          if (character.Parent != null)
            ++num4;
        }
      }
      this.m_characters.Clear();
      StringBuilder stringBuilder = new StringBuilder();
      string str = obstacleDetected ? MyTexts.Get(MySpaceTexts.Jump_Obstacle).ToString() : "";
      stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_Destination)).Append(name).Append("\n");
      stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_Distance)).Append(distance.ToString("N")).Append(" km\n");
      stringBuilder.Append(MyTexts.Get(MySpaceTexts.Jump_Achievable).ToString() + str + ": ").Append(num2.ToString("P")).Append(" (").Append(actualDistance.ToString("N")).Append(" km)\n");
      stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_Weight)).Append(MyHud.ShipInfo.Mass.ToString("N")).Append(" kg\n");
      stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_DriveCount)).Append(num1).Append("/").Append(count).Append("\n");
      stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_CrewCount)).Append(num4).Append("/").Append(num3).Append("\n");
      return stringBuilder;
    }

    private StringBuilder GetWarningText(double actualDistance, bool obstacleDetected)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (obstacleDetected)
        stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_ObstacleTruncation));
      stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_DistanceToDest)).Append(actualDistance.ToString("N")).Append(" m\n");
      stringBuilder.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Jump_MinDistance)).Append(5000.0.ToString("N")).Append(" m\n");
      return stringBuilder;
    }

    private void GetCharactersInBoundingBox(BoundingBoxD boundingBox, List<MyCharacter> characters)
    {
      MyGamePruningStructure.GetAllEntitiesInBox(ref boundingBox, this.m_entitiesInRange);
      foreach (MyEntity myEntity in this.m_entitiesInRange)
      {
        if (myEntity is MyCharacter myCharacter)
          characters.Add(myCharacter);
      }
      this.m_entitiesInRange.Clear();
    }

    private Vector3D? FindSuitableJumpLocation(Vector3D desiredLocation)
    {
      BoundingBoxD physicalGroupAabb = this.Grid.GetPhysicalGroupAABB();
      physicalGroupAabb.Inflate(1000.0);
      BoundingBoxD inflated = physicalGroupAabb.GetInflated(physicalGroupAabb.HalfExtents * 10.0);
      inflated.Translate(desiredLocation - inflated.Center);
      MyProceduralWorldGenerator.Static.OverlapAllPlanetSeedsInSphere(new BoundingSphereD(inflated.Center, inflated.HalfExtents.AbsMax()), this.m_objectsInRange);
      Vector3D vector3D1 = desiredLocation;
      foreach (MyObjectSeed myObjectSeed in this.m_objectsInRange)
      {
        if (myObjectSeed.BoundingVolume.Contains(vector3D1) != ContainmentType.Disjoint)
        {
          Vector3D vector3D2 = vector3D1 - myObjectSeed.BoundingVolume.Center;
          vector3D2.Normalize();
          Vector3D vector3D3 = vector3D2 * (myObjectSeed.BoundingVolume.HalfExtents * 1.5);
          vector3D1 = myObjectSeed.BoundingVolume.Center + vector3D3;
          break;
        }
      }
      this.m_objectsInRange.Clear();
      MyProceduralWorldGenerator.Static.OverlapAllAsteroidSeedsInSphere(new BoundingSphereD(inflated.Center, inflated.HalfExtents.AbsMax()), this.m_objectsInRange);
      foreach (MyObjectSeed myObjectSeed in this.m_objectsInRange)
        this.m_obstaclesInRange.Add(myObjectSeed.BoundingVolume);
      this.m_objectsInRange.Clear();
      MyProceduralWorldGenerator.Static.GetAllInSphere<MyStationCellGenerator>(new BoundingSphereD(inflated.Center, inflated.HalfExtents.AbsMax()), this.m_objectsInRange);
      foreach (MyObjectSeed myObjectSeed in this.m_objectsInRange)
      {
        if (myObjectSeed.UserData is MyStation userData)
        {
          BoundingBoxD boundingBoxD = new BoundingBoxD(userData.Position - (double) MyStation.SAFEZONE_SIZE, userData.Position + MyStation.SAFEZONE_SIZE);
          if (boundingBoxD.Contains(vector3D1) != ContainmentType.Disjoint)
            this.m_obstaclesInRange.Add(boundingBoxD);
        }
      }
      this.m_objectsInRange.Clear();
      MyGamePruningStructure.GetTopMostEntitiesInBox(ref inflated, this.m_entitiesInRange);
      foreach (MyEntity myEntity in this.m_entitiesInRange)
      {
        if (!(myEntity is MyPlanet))
          this.m_obstaclesInRange.Add(myEntity.PositionComp.WorldAABB.GetInflated(physicalGroupAabb.HalfExtents));
      }
      int num1 = 10;
      int num2 = 0;
      BoundingBoxD? nullable = new BoundingBoxD?();
      bool flag1 = false;
      while (num2 < num1)
      {
        ++num2;
        bool flag2 = false;
        foreach (BoundingBoxD box in this.m_obstaclesInRange)
        {
          switch (box.Contains(vector3D1))
          {
            case ContainmentType.Contains:
            case ContainmentType.Intersects:
              if (!nullable.HasValue)
                nullable = new BoundingBoxD?(box);
              BoundingBoxD boundingBoxD = nullable.Value;
              nullable = new BoundingBoxD?(boundingBoxD.Include(box));
              boundingBoxD = nullable.Value;
              nullable = new BoundingBoxD?(boundingBoxD.Inflate(1.0));
              vector3D1 = this.ClosestPointOnBounds(nullable.Value, vector3D1);
              flag2 = true;
              goto label_33;
            default:
              continue;
          }
        }
label_33:
        if (!flag2)
        {
          flag1 = true;
          break;
        }
      }
      this.m_obstaclesInRange.Clear();
      this.m_entitiesInRange.Clear();
      this.m_objectsInRange.Clear();
      return flag1 ? new Vector3D?(vector3D1) : new Vector3D?();
    }

    private Vector3D ClosestPointOnBounds(BoundingBoxD b, Vector3D p)
    {
      Vector3D vector3D = (p - b.Center) / b.HalfExtents;
      switch (vector3D.AbsMaxComponent())
      {
        case 0:
          p.X = vector3D.X <= 0.0 ? b.Min.X : b.Max.X;
          break;
        case 1:
          p.Y = vector3D.Y <= 0.0 ? b.Min.Y : b.Max.Y;
          break;
        case 2:
          p.Z = vector3D.Z <= 0.0 ? b.Min.Z : b.Max.Z;
          break;
      }
      return p;
    }

    private bool IsLocalCharacterAffectedByJump(bool forceRecompute = false)
    {
      if (MySession.Static.LocalCharacter == null || !(MySession.Static.ControlledEntity is MyShipController))
      {
        this.m_playEffect = false;
        MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
        return false;
      }
      if (this.m_playEffect && !forceRecompute)
        return true;
      this.GetCharactersInBoundingBox(this.Grid.GetPhysicalGroupAABB(), this.m_characters);
      foreach (MyCharacter character in this.m_characters)
      {
        if (character == MySession.Static.LocalCharacter && character.Parent != null)
        {
          this.m_characters.Clear();
          this.m_playEffect = true;
          return true;
        }
      }
      this.m_characters.Clear();
      this.m_playEffect = false;
      return false;
    }

    private void Jump(Vector3D jumpTarget, long userId)
    {
      double maxJumpDistance = this.GetMaxJumpDistance(userId);
      this.m_jumpDirection = jumpTarget - this.Grid.WorldMatrix.Translation;
      Vector3D.Normalize(ref this.m_jumpDirection, out this.m_jumpDirectionNorm);
      double num = this.m_jumpDirection.Length();
      if (num > maxJumpDistance)
        this.m_jumpDirection *= maxJumpDistance / num;
      this.m_selectedDestination = this.Grid.WorldMatrix.Translation + this.m_jumpDirection;
      this.m_isJumping = true;
      this.m_jumped = false;
      this.m_jumpTimeLeft = MyFakes.TESTING_JUMPDRIVE ? 1f : 10f;
      this.Grid.GridSystems.JumpSystem.m_jumpTimeLeft = this.m_jumpTimeLeft;
      this.m_soundEmitter?.PlaySound(this.m_chargingSound);
      this.m_prevJumpTime = 0.0f;
      this.m_userId = userId;
      this.Schedule();
    }

    private void UpdateJumpDriveSystem()
    {
      if (!this.m_isJumping)
        return;
      float jumpTimeLeft = this.m_jumpTimeLeft;
      if (this.m_effect == null)
        this.PlayParticleEffect();
      else
        this.UpdateParticleEffect();
      this.m_jumpTimeLeft -= 0.01666667f;
      if ((double) jumpTimeLeft > 0.400000005960464)
      {
        double num = Math.Round((double) jumpTimeLeft);
        if (num != (double) this.m_prevJumpTime && this.IsLocalCharacterAffectedByJump(true))
        {
          MyHudNotification myHudNotification = new MyHudNotification(MySpaceTexts.NotificationJumpWarmupTime, 500, priority: 3);
          myHudNotification.SetTextFormatArguments((object) num);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
        }
      }
      else if ((double) jumpTimeLeft > 0.0)
      {
        this.IsLocalCharacterAffectedByJump(true);
        if (this.m_soundEmitter != null && this.m_soundEmitter.SoundId != this.m_jumpOutSound.Arcade && this.m_soundEmitter.SoundId != this.m_jumpOutSound.Realistic)
          this.m_soundEmitter.PlaySound(this.m_jumpOutSound);
        this.UpdateJumpEffect(jumpTimeLeft / 0.4f);
        if ((double) jumpTimeLeft >= 0.300000011920929)
          ;
      }
      else if (!this.m_jumped)
      {
        if (Sync.IsServer)
        {
          Vector3D? suitableJumpLocation = this.FindSuitableJumpLocation(this.m_selectedDestination);
          double maxJumpDistance = this.GetMaxJumpDistance(this.m_userId);
          MyGridJumpDriveSystem.MyJumpFailReason reason = MyGridJumpDriveSystem.MyJumpFailReason.None;
          if (suitableJumpLocation.HasValue && this.m_jumpDirection.Length() <= maxJumpDistance && this.IsJumpValid(this.m_userId, out reason))
          {
            this.SendPerformJump(suitableJumpLocation.Value);
            this.PerformJump(suitableJumpLocation.Value);
          }
          else
            this.SendAbortJump();
        }
      }
      else if ((double) jumpTimeLeft > -0.600000023841858)
      {
        if (this.m_soundEmitterJumpIn != null && !this.m_soundEmitterJumpIn.IsPlaying)
          this.m_soundEmitterJumpIn.PlaySound(this.m_jumpInSound);
        this.UpdateJumpEffect(jumpTimeLeft / -0.6f);
      }
      else
        this.CleanupAfterJump();
      this.m_prevJumpTime = (float) Math.Round((double) jumpTimeLeft);
    }

    private void PlayParticleEffect()
    {
      if (this.m_effect != null)
        return;
      MatrixD fromDir = MatrixD.CreateFromDir(-this.m_jumpDirectionNorm);
      this.m_effectOffset = (Vector3) (this.m_jumpDirectionNorm * this.Grid.PositionComp.WorldAABB.HalfExtents.AbsMax() * 2.0);
      fromDir.Translation = this.Grid.PositionComp.WorldAABB.Center + this.m_effectOffset;
      MyParticlesManager.TryCreateParticleEffect("Warp", fromDir, out this.m_effect);
    }

    private void UpdateParticleEffect()
    {
      if (this.m_effect == null)
        return;
      Vector3D trans = this.Grid.PositionComp.WorldAABB.Center + this.m_effectOffset;
      this.m_effect.SetTranslation(ref trans);
    }

    private void StopParticleEffect()
    {
      if (this.m_effect == null)
        return;
      this.m_effect.StopEmitting(10f);
      this.m_effect = (MyParticleEffect) null;
    }

    private void PerformJump(Vector3D jumpTarget)
    {
      this.m_jumpDirection = jumpTarget - this.Grid.WorldMatrix.Translation;
      Vector3D.Normalize(ref this.m_jumpDirection, out this.m_jumpDirectionNorm);
      this.DepleteJumpDrives(this.m_jumpDirection.Length(), this.m_userId);
      bool flag = false;
      if (this.IsLocalCharacterAffectedByJump())
        flag = true;
      if (flag)
      {
        MyThirdPersonSpectator.Static.ResetViewerAngle(new Vector2?());
        MyThirdPersonSpectator.Static.ResetViewerDistance();
        MyThirdPersonSpectator.Static.RecalibrateCameraPosition();
      }
      this.m_jumped = true;
      MatrixD worldMatrix = this.Grid.WorldMatrix;
      worldMatrix.Translation = this.Grid.WorldMatrix.Translation + this.m_jumpDirection;
      this.Grid.Teleport(worldMatrix, (object) null, false);
      if (!flag)
        return;
      MyThirdPersonSpectator.Static.ResetViewerAngle(new Vector2?());
      MyThirdPersonSpectator.Static.ResetViewerDistance();
      MyThirdPersonSpectator.Static.RecalibrateCameraPosition();
    }

    public void AbortJump(MyGridJumpDriveSystem.MyJumpFailReason reason)
    {
      this.StopParticleEffect();
      this.m_soundEmitter?.StopSound(true);
      this.m_soundEmitterJumpIn?.StopSound(true);
      if (this.m_isJumping && this.IsLocalCharacterAffectedByJump())
        this.ShowNotification(reason);
      this.CleanupAfterJump();
    }

    private void CleanupAfterJump()
    {
      foreach (MyJumpDrive jumpDrive in this.m_jumpDrives)
        jumpDrive.IsJumping = false;
      if (this.IsLocalCharacterAffectedByJump())
        MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
      this.m_jumped = false;
      this.m_isJumping = false;
      this.m_effect = (MyParticleEffect) null;
      this.DeSchedule();
    }

    public void AfterGridClose()
    {
      if (this.m_isJumping)
      {
        this.m_soundEmitter?.StopSound(true);
        this.m_soundEmitterJumpIn?.StopSound(true);
        this.CleanupAfterJump();
      }
      if (this.m_soundEmitter != null)
      {
        this.m_soundEmitter.CleanEmitterMethods();
        this.m_soundEmitter = (MyEntity3DSoundEmitter) null;
      }
      if (this.m_soundEmitterJumpIn == null)
        return;
      this.m_soundEmitterJumpIn.CleanEmitterMethods();
      this.m_soundEmitterJumpIn = (MyEntity3DSoundEmitter) null;
    }

    private void UpdateJumpEffect(float t)
    {
      if (!this.m_playEffect)
        return;
      float radians = MathHelper.ToRadians(170f);
      MySector.MainCamera.FieldOfView = MathHelper.SmoothStep(MySandboxGame.Config.FieldOfView, radians, 1f - t);
    }

    public bool CheckReceivedCoordinates(ref Vector3D pos)
    {
      if ((double) this.m_jumpTimeLeft > 1.0 || Vector3D.DistanceSquared(this.Grid.PositionComp.GetPosition(), pos) <= 100000000.0 || !this.m_jumped)
        return true;
      MySandboxGame.Log.WriteLine(string.Format("Wrong position packet received, dist={0}, T={1})", (object) Vector3D.Distance(this.Grid.PositionComp.GetPosition(), pos), (object) this.m_jumpTimeLeft));
      return false;
    }

    private void OnRequestJumpFromClient(Vector3D jumpTarget, long userId)
    {
      MyGridJumpDriveSystem.MyJumpFailReason reason;
      if (!this.IsJumpValid(userId, out reason))
      {
        this.SendJumpFailure(reason);
      }
      else
      {
        this.m_jumpDirection = jumpTarget - this.Grid.WorldMatrix.Translation;
        Vector3D.Normalize(ref this.m_jumpDirection, out this.m_jumpDirectionNorm);
        double maxJumpDistance = this.GetMaxJumpDistance(userId);
        double num1 = (jumpTarget - this.Grid.WorldMatrix.Translation).Length();
        double num2 = num1;
        if (num1 > maxJumpDistance)
        {
          double num3 = maxJumpDistance / num1;
          num2 = maxJumpDistance;
          this.m_jumpDirection *= num3;
        }
        jumpTarget = this.Grid.WorldMatrix.Translation + this.m_jumpDirection;
        if (num2 < 4800.0)
        {
          this.SendJumpFailure(MyGridJumpDriveSystem.MyJumpFailReason.ShortDistance);
        }
        else
        {
          Vector3D? suitableJumpLocation = this.FindSuitableJumpLocation(jumpTarget);
          if (!suitableJumpLocation.HasValue)
            this.SendJumpFailure(MyGridJumpDriveSystem.MyJumpFailReason.NoLocation);
          else
            this.SendJumpSuccess(suitableJumpLocation.Value, userId);
        }
      }
    }

    private void RequestJump(Vector3D jumpTarget, long userId)
    {
      MyMultiplayer.RaiseStaticEvent<long, Vector3D, long>((Func<IMyEventOwner, Action<long, Vector3D, long>>) (s => new Action<long, Vector3D, long>(MyGridJumpDriveSystem.OnJumpRequested)), this.Grid.EntityId, jumpTarget, userId);
      if (MyVisualScriptLogicProvider.GridJumped == null)
        return;
      MyVisualScriptLogicProvider.GridJumped(userId, this.Grid.Name, this.Grid.EntityId);
    }

    [Event(null, 985)]
    [Reliable]
    [Server]
    private static void OnJumpRequested(long entityId, Vector3D jumpTarget, long userId)
    {
      MyCubeGrid entity;
      MyEntities.TryGetEntityById<MyCubeGrid>(entityId, out entity);
      entity?.GridSystems.JumpSystem.OnRequestJumpFromClient(jumpTarget, userId);
    }

    private void SendJumpSuccess(Vector3D jumpTarget, long userId) => MyMultiplayer.RaiseStaticEvent<long, Vector3D, long>((Func<IMyEventOwner, Action<long, Vector3D, long>>) (s => new Action<long, Vector3D, long>(MyGridJumpDriveSystem.OnJumpSuccess)), this.Grid.EntityId, jumpTarget, userId);

    [Event(null, 1002)]
    [Reliable]
    [ServerInvoked]
    [Broadcast]
    private static void OnJumpSuccess(long entityId, Vector3D jumpTarget, long userId)
    {
      MyCubeGrid entity;
      MyEntities.TryGetEntityById<MyCubeGrid>(entityId, out entity);
      entity?.GridSystems.JumpSystem.Jump(jumpTarget, userId);
    }

    private void SendJumpFailure(MyGridJumpDriveSystem.MyJumpFailReason reason) => MyMultiplayer.RaiseStaticEvent<long, MyGridJumpDriveSystem.MyJumpFailReason>((Func<IMyEventOwner, Action<long, MyGridJumpDriveSystem.MyJumpFailReason>>) (s => new Action<long, MyGridJumpDriveSystem.MyJumpFailReason>(MyGridJumpDriveSystem.OnJumpFailure)), this.Grid.EntityId, reason);

    [Event(null, 1019)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void OnJumpFailure(long entityId, MyGridJumpDriveSystem.MyJumpFailReason reason) => MyEntities.TryGetEntityById<MyCubeGrid>(entityId, out MyCubeGrid _);

    private void SendPerformJump(Vector3D jumpTarget) => MyMultiplayer.RaiseStaticEvent<long, Vector3D>((Func<IMyEventOwner, Action<long, Vector3D>>) (s => new Action<long, Vector3D>(MyGridJumpDriveSystem.OnPerformJump)), this.Grid.EntityId, jumpTarget);

    [Event(null, 1036)]
    [Reliable]
    [Broadcast]
    private static void OnPerformJump(long entityId, Vector3D jumpTarget)
    {
      MyCubeGrid entity;
      MyEntities.TryGetEntityById<MyCubeGrid>(entityId, out entity);
      entity?.GridSystems.JumpSystem.PerformJump(jumpTarget);
    }

    private void SendAbortJump() => MyMultiplayer.RaiseStaticEvent<long>((Func<IMyEventOwner, Action<long>>) (s => new Action<long>(MyGridJumpDriveSystem.OnAbortJump)), this.Grid.EntityId);

    [Event(null, 1052)]
    [Reliable]
    [Server]
    [Broadcast]
    private static void OnAbortJump(long entityId)
    {
      MyCubeGrid entity;
      MyEntities.TryGetEntityById<MyCubeGrid>(entityId, out entity);
      if (entity != null)
      {
        MyExternalReplicable byObject = MyExternalReplicable.FindByObject((object) entity);
        if (Sync.IsServer && !MyEventContext.Current.IsLocallyInvoked)
        {
          ValidationResult validationResult = ValidationResult.Passed;
          if (byObject != null)
            validationResult = byObject.HasRights(new EndpointId(MyEventContext.Current.Sender.Value), ValidationType.Controlled);
          if (validationResult != ValidationResult.Passed)
          {
            (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, validationResult.HasFlag((Enum) ValidationResult.Kick), (string) null, true);
            MyEventContext.ValidationFailed();
            return;
          }
        }
        entity.GridSystems.JumpSystem.AbortJump(MyGridJumpDriveSystem.MyJumpFailReason.None);
      }
      else
      {
        if (!Sync.IsServer || MyEventContext.Current.IsLocallyInvoked)
          return;
        (MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value, true, (string) null, true);
        MyEventContext.ValidationFailed();
      }
    }

    public bool IsJumping => this.m_isJumping;

    public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.BeforeSimulation;

    public override int UpdatePriority => 5;

    public enum MyJumpFailReason
    {
      None,
      Static,
      Locked,
      ShortDistance,
      AlreadyJumping,
      NoLocation,
      Other,
    }

    protected sealed class OnJumpRequested\u003C\u003ESystem_Int64\u0023VRageMath_Vector3D\u0023System_Int64 : ICallSite<IMyEventOwner, long, Vector3D, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3D jumpTarget,
        in long userId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGridJumpDriveSystem.OnJumpRequested(entityId, jumpTarget, userId);
      }
    }

    protected sealed class OnJumpSuccess\u003C\u003ESystem_Int64\u0023VRageMath_Vector3D\u0023System_Int64 : ICallSite<IMyEventOwner, long, Vector3D, long, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3D jumpTarget,
        in long userId,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGridJumpDriveSystem.OnJumpSuccess(entityId, jumpTarget, userId);
      }
    }

    protected sealed class OnJumpFailure\u003C\u003ESystem_Int64\u0023Sandbox_Game_GameSystems_MyGridJumpDriveSystem\u003C\u003EMyJumpFailReason : ICallSite<IMyEventOwner, long, MyGridJumpDriveSystem.MyJumpFailReason, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in MyGridJumpDriveSystem.MyJumpFailReason reason,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGridJumpDriveSystem.OnJumpFailure(entityId, reason);
      }
    }

    protected sealed class OnPerformJump\u003C\u003ESystem_Int64\u0023VRageMath_Vector3D : ICallSite<IMyEventOwner, long, Vector3D, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in Vector3D jumpTarget,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGridJumpDriveSystem.OnPerformJump(entityId, jumpTarget);
      }
    }

    protected sealed class OnAbortJump\u003C\u003ESystem_Int64 : ICallSite<IMyEventOwner, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in long entityId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGridJumpDriveSystem.OnAbortJump(entityId);
      }
    }
  }
}
