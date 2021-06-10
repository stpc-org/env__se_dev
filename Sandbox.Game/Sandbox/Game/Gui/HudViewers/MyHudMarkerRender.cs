// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.GUI.HudViewers.MyHudMarkerRender
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Generics;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI.HudViewers
{
  public class MyHudMarkerRender : MyHudMarkerRenderBase
  {
    private static float m_friendAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;
    private static bool m_disableFading = false;
    private bool m_disableFadingToggle;
    private static MyHudNotification m_signalModeNotification = (MyHudNotification) null;
    private ConcurrentDictionary<MyEntity, MyHudMarkerRender.MyPlayerIndicator> m_playerIndicatorsDict = new ConcurrentDictionary<MyEntity, MyHudMarkerRender.MyPlayerIndicator>();
    private static float m_ownerAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;
    private static float m_enemyAntennaRange = MyPerGameSettings.MaxAntennaDrawDistance;
    private static MatrixD m_distanceMeasuringMatrix;
    private MyDynamicObjectPool<MyHudMarkerRender.PointOfInterest> m_pointOfInterestPool = new MyDynamicObjectPool<MyHudMarkerRender.PointOfInterest>(32);
    private List<MyHudMarkerRender.PointOfInterest> m_pointsOfInterest = new List<MyHudMarkerRender.PointOfInterest>();

    public static MyHudMarkerRender.SignalMode SignalDisplayMode { get; private set; }

    public override void Update()
    {
      IMyControllableEntity controlledEntity1 = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity1 != null ? controlledEntity1.AuxiliaryContext : MyStringId.NullOrEmpty;
      MyHudMarkerRender.m_disableFading = MyControllerHelper.IsControl(MyControllerHelper.CX_BASE, MyControlsSpace.LOOKAROUND, MyControlStateType.PRESSED);
      IMyControllableEntity controlledEntity2 = MySession.Static.ControlledEntity;
      MyStringId toggleSignals = MyControlsSpace.TOGGLE_SIGNALS;
      if (MyControllerHelper.IsControl(context, toggleSignals) && !MyInput.Static.IsAnyCtrlKeyPressed() && MyScreenManager.FocusedControl == null)
        MyHudMarkerRender.ChangeSignalMode();
      MatrixD? controlledEntityMatrix = MyHudMarkerRender.ControlledEntityMatrix;
      if (!controlledEntityMatrix.HasValue || !MySession.Static.CameraOnCharacter && MySession.Static.IsCameraUserControlledSpectator())
      {
        MyHudMarkerRender.m_distanceMeasuringMatrix = MyHudMarkerRender.CameraMatrix;
      }
      else
      {
        MatrixD? localCharacterMatrix = MyHudMarkerRender.LocalCharacterMatrix;
        if (MySession.Static.CameraOnCharacter && localCharacterMatrix.HasValue)
          MyHudMarkerRender.m_distanceMeasuringMatrix = localCharacterMatrix.Value;
        else
          MyHudMarkerRender.m_distanceMeasuringMatrix = controlledEntityMatrix.Value;
      }
    }

    public static void ChangeSignalMode()
    {
      if (MyHud.IsHudMinimal || MyHud.MinimalHud)
        return;
      MySandboxGame.Static.Invoke((Action) (() => MyGuiAudio.PlaySound(MyGuiSounds.HudClick)), "Play Sound");
      ++MyHudMarkerRender.SignalDisplayMode;
      if (MyHudMarkerRender.SignalDisplayMode >= MyHudMarkerRender.SignalMode.MaxSignalModes)
        MyHudMarkerRender.SignalDisplayMode = MyHudMarkerRender.SignalMode.DefaultMode;
      if (MyHudMarkerRender.m_signalModeNotification != null)
      {
        MyHud.Notifications.Remove((MyHudNotificationBase) MyHudMarkerRender.m_signalModeNotification);
        MyHudMarkerRender.m_signalModeNotification = (MyHudNotification) null;
      }
      switch (MyHudMarkerRender.SignalDisplayMode)
      {
        case MyHudMarkerRender.SignalMode.DefaultMode:
          MyHudMarkerRender.m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_DefaultMode, 1000);
          break;
        case MyHudMarkerRender.SignalMode.FullDisplay:
          MyHudMarkerRender.m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_FullDisplay, 1000);
          break;
        case MyHudMarkerRender.SignalMode.NoNames:
          MyHudMarkerRender.m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_NoNames, 1000);
          break;
        case MyHudMarkerRender.SignalMode.Off:
          MyHudMarkerRender.m_signalModeNotification = new MyHudNotification(MyCommonTexts.SignalMode_Switch_Off, 1000);
          break;
      }
      if (MyHudMarkerRender.m_signalModeNotification == null)
        return;
      MyHud.Notifications.Add((MyHudNotificationBase) MyHudMarkerRender.m_signalModeNotification);
    }

    public static float FriendAntennaRange
    {
      get => MyHudMarkerRender.NormalizeLog(MyHudMarkerRender.m_friendAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
      set => MyHudMarkerRender.m_friendAntennaRange = MyHudMarkerRender.Denormalize(value);
    }

    public static float OwnerAntennaRange
    {
      get => MyHudMarkerRender.NormalizeLog(MyHudMarkerRender.m_ownerAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
      set => MyHudMarkerRender.m_ownerAntennaRange = MyHudMarkerRender.Denormalize(value);
    }

    public static float EnemyAntennaRange
    {
      get => MyHudMarkerRender.NormalizeLog(MyHudMarkerRender.m_enemyAntennaRange, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);
      set => MyHudMarkerRender.m_enemyAntennaRange = MyHudMarkerRender.Denormalize(value);
    }

    public MyHudMarkerRender(MyGuiScreenHudBase hudScreen)
      : base(hudScreen)
    {
    }

    public override void DrawLocationMarkers(MyHudLocationMarkers locationMarkers)
    {
      if (MySession.Static == null || MySession.Static.LocalHumanPlayer == null || MySession.Static.LocalHumanPlayer.Identity == null)
        return;
      locationMarkers.ProcessChanges();
      float num1 = MyHudMarkerRender.m_ownerAntennaRange * MyHudMarkerRender.m_ownerAntennaRange;
      float num2 = MyHudMarkerRender.m_friendAntennaRange * MyHudMarkerRender.m_friendAntennaRange;
      float num3 = MyHudMarkerRender.m_enemyAntennaRange * MyHudMarkerRender.m_enemyAntennaRange;
      MatrixD matrixD = MyHudMarkerRender.GetDistanceMeasuringMatrix();
      foreach (MyHudEntityParams markerEntity in (IEnumerable<MyHudEntityParams>) locationMarkers.MarkerEntities)
      {
        if (markerEntity.ShouldDraw == null || markerEntity.ShouldDraw())
        {
          double num4 = (markerEntity.Position - matrixD.Translation).LengthSquared();
          MyRelationsBetweenPlayerAndBlock relationPlayerBlock = MyIDModule.GetRelationPlayerBlock(markerEntity.Owner, MySession.Static.LocalHumanPlayer.Identity.IdentityId, markerEntity.Share);
          switch (relationPlayerBlock)
          {
            case MyRelationsBetweenPlayerAndBlock.NoOwnership:
            case MyRelationsBetweenPlayerAndBlock.FactionShare:
            case MyRelationsBetweenPlayerAndBlock.Friends:
              if (num4 <= (double) num2)
                break;
              continue;
            case MyRelationsBetweenPlayerAndBlock.Owner:
              if (num4 <= (double) num1)
                break;
              continue;
            case MyRelationsBetweenPlayerAndBlock.Neutral:
            case MyRelationsBetweenPlayerAndBlock.Enemies:
              if (num4 > (double) num3)
                continue;
              break;
          }
          if (markerEntity.Entity is MyEntity entity)
            this.AddEntity(entity, relationPlayerBlock, markerEntity.Text, MyHudMarkerRender.IsScenarioObjective(entity));
          else
            this.AddProxyEntity(markerEntity.Position, relationPlayerBlock, markerEntity.Text);
        }
      }
      this.m_hudScreen?.DrawTexts();
    }

    private static bool IsScenarioObjective(MyEntity entity) => entity != null && entity.Name != null && (entity.Name.Length >= 13 && entity.Name.Substring(0, 13).Equals("MissionStart_"));

    private static MatrixD? ControlledEntityMatrix => MySession.Static.ControlledEntity != null ? new MatrixD?(MySession.Static.ControlledEntity.Entity.PositionComp.WorldMatrixRef) : new MatrixD?();

    private static MatrixD? LocalCharacterMatrix => MySession.Static.LocalCharacter != null ? new MatrixD?(MySession.Static.LocalCharacter.WorldMatrix) : new MatrixD?();

    private static MatrixD CameraMatrix => MySector.MainCamera.WorldMatrix;

    public static ref readonly MatrixD GetDistanceMeasuringMatrix() => ref MyHudMarkerRender.m_distanceMeasuringMatrix;

    public void AddPlayerIndicator(
      MyEntity target,
      MyRelationsBetweenPlayers relation,
      bool isAlwaysVisible)
    {
      MyHudMarkerRender.MyPlayerIndicator myPlayerIndicator1;
      this.m_playerIndicatorsDict.TryGetValue(target, out myPlayerIndicator1);
      if (myPlayerIndicator1 == null)
      {
        MyHudMarkerRender.MyPlayerIndicator myPlayerIndicator2 = new MyHudMarkerRender.MyPlayerIndicator(target, relation, isAlwaysVisible);
        this.m_playerIndicatorsDict[target] = myPlayerIndicator2;
      }
      else
        myPlayerIndicator1.ResetFadeout(relation);
    }

    public void AddPOI(
      Vector3D worldPosition,
      StringBuilder name,
      MyRelationsBetweenPlayerAndBlock relationship)
    {
      if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.Off)
        return;
      MyHudMarkerRender.PointOfInterest pointOfInterest = this.m_pointOfInterestPool.Allocate();
      this.m_pointsOfInterest.Add(pointOfInterest);
      pointOfInterest.Reset();
      pointOfInterest.SetState(worldPosition, MyHudMarkerRender.PointOfInterest.PointOfInterestType.GPS, relationship);
      pointOfInterest.SetText(name);
    }

    public void AddEntity(
      MyEntity entity,
      MyRelationsBetweenPlayerAndBlock relationship,
      StringBuilder entityName,
      bool IsScenarioMarker = false)
    {
      if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.Off || entity == null)
        return;
      Vector3D position = entity.PositionComp.GetPosition();
      MyHudMarkerRender.PointOfInterest.PointOfInterestType type = MyHudMarkerRender.PointOfInterest.PointOfInterestType.UnknownEntity;
      switch (entity)
      {
        case MyCharacter _:
          if (entity == MySession.Static.LocalCharacter)
            return;
          type = MyHudMarkerRender.PointOfInterest.PointOfInterestType.Character;
          position += entity.WorldMatrix.Up * 1.29999995231628;
          break;
        case MyCubeBlock myCubeBlock when myCubeBlock.CubeGrid != null:
          type = myCubeBlock.CubeGrid.GridSizeEnum != MyCubeSize.Small ? (myCubeBlock.CubeGrid.IsStatic ? MyHudMarkerRender.PointOfInterest.PointOfInterestType.StaticEntity : MyHudMarkerRender.PointOfInterest.PointOfInterestType.LargeEntity) : MyHudMarkerRender.PointOfInterest.PointOfInterestType.SmallEntity;
          break;
      }
      MyHudMarkerRender.PointOfInterest pointOfInterest = this.m_pointOfInterestPool.Allocate();
      this.m_pointsOfInterest.Add(pointOfInterest);
      pointOfInterest.Reset();
      if (IsScenarioMarker)
        type = MyHudMarkerRender.PointOfInterest.PointOfInterestType.Scenario;
      pointOfInterest.SetState(position, type, relationship);
      pointOfInterest.SetEntity(entity);
      pointOfInterest.SetText(entityName);
    }

    public void AddGPS(MyGps gps)
    {
      if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.Off)
        return;
      MyHudMarkerRender.PointOfInterest pointOfInterest = this.m_pointOfInterestPool.Allocate();
      this.m_pointsOfInterest.Add(pointOfInterest);
      pointOfInterest.Reset();
      pointOfInterest.Color = gps.GPSColor;
      pointOfInterest.SetState(gps.Coords, gps.ContractId != 0L ? MyHudMarkerRender.PointOfInterest.PointOfInterestType.ContractGPS : (gps.IsObjective ? MyHudMarkerRender.PointOfInterest.PointOfInterestType.Objective : MyHudMarkerRender.PointOfInterest.PointOfInterestType.GPS), MyRelationsBetweenPlayerAndBlock.Owner);
      if (string.IsNullOrEmpty(gps.DisplayName))
        pointOfInterest.SetText(gps.Name);
      else
        pointOfInterest.SetText(gps.DisplayName);
      pointOfInterest.AlwaysVisible = gps.AlwaysVisible;
      pointOfInterest.ContainerRemainingTime = gps.ContainerRemainingTime;
    }

    public void AddButtonMarker(Vector3D worldPosition, string name)
    {
      MyHudMarkerRender.PointOfInterest pointOfInterest = this.m_pointOfInterestPool.Allocate();
      pointOfInterest.Reset();
      pointOfInterest.AlwaysVisible = true;
      pointOfInterest.SetState(worldPosition, MyHudMarkerRender.PointOfInterest.PointOfInterestType.ButtonMarker, MyRelationsBetweenPlayerAndBlock.Owner);
      pointOfInterest.SetText(name);
      this.m_pointsOfInterest.Add(pointOfInterest);
    }

    public void AddOre(Vector3D worldPosition, string name)
    {
      if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.Off)
        return;
      MyHudMarkerRender.PointOfInterest pointOfInterest = this.m_pointOfInterestPool.Allocate();
      this.m_pointsOfInterest.Add(pointOfInterest);
      pointOfInterest.Reset();
      pointOfInterest.SetState(worldPosition, MyHudMarkerRender.PointOfInterest.PointOfInterestType.Ore, MyRelationsBetweenPlayerAndBlock.NoOwnership);
      pointOfInterest.SetText(name);
    }

    public void AddTarget(Vector3D worldPosition)
    {
      if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.Off)
        return;
      MyHudMarkerRender.PointOfInterest pointOfInterest = this.m_pointOfInterestPool.Allocate();
      this.m_pointsOfInterest.Add(pointOfInterest);
      pointOfInterest.Reset();
      pointOfInterest.SetState(worldPosition, MyHudMarkerRender.PointOfInterest.PointOfInterestType.Target, MyRelationsBetweenPlayerAndBlock.Enemies);
    }

    public void AddHacking(Vector3D worldPosition, StringBuilder name)
    {
      if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.Off)
        return;
      MyHudMarkerRender.PointOfInterest pointOfInterest = this.m_pointOfInterestPool.Allocate();
      this.m_pointsOfInterest.Add(pointOfInterest);
      pointOfInterest.Reset();
      pointOfInterest.SetState(worldPosition, MyHudMarkerRender.PointOfInterest.PointOfInterestType.Hack, MyRelationsBetweenPlayerAndBlock.Owner);
      pointOfInterest.SetText(name);
    }

    public void AddProxyEntity(
      Vector3D worldPosition,
      MyRelationsBetweenPlayerAndBlock relationship,
      StringBuilder name)
    {
      if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.Off)
        return;
      MyHudMarkerRender.PointOfInterest pointOfInterest = this.m_pointOfInterestPool.Allocate();
      this.m_pointsOfInterest.Add(pointOfInterest);
      pointOfInterest.Reset();
      pointOfInterest.SetState(worldPosition, MyHudMarkerRender.PointOfInterest.PointOfInterestType.UnknownEntity, relationship);
      pointOfInterest.SetText(name);
    }

    public static void AppendDistance(StringBuilder stringBuilder, double distance)
    {
      if (stringBuilder == null)
        return;
      distance = Math.Abs(distance);
      if (distance > 9.460730473E+15)
      {
        stringBuilder.AppendDecimal(Math.Round(distance / 9.460730473E+15, 2), 2);
        stringBuilder.Append("ly");
      }
      else if (distance > 299792458.000137)
      {
        stringBuilder.AppendDecimal(Math.Round(distance / 299792458.000137, 2), 2);
        stringBuilder.Append("ls");
      }
      else if (distance > 1000.0)
      {
        if (distance > 1000000.0)
          stringBuilder.AppendDecimal(Math.Round(distance / 1000.0, 2), 1);
        else
          stringBuilder.AppendDecimal(Math.Round(distance / 1000.0, 2), 2);
        stringBuilder.Append("km");
      }
      else
      {
        stringBuilder.AppendDecimal(Math.Round(distance, 2), 1);
        stringBuilder.Append("m");
      }
    }

    public static bool TryComputeScreenPoint(
      Vector3D worldPosition,
      out Vector2 projectedPoint2D,
      out bool isBehind)
    {
      Vector3D position = Vector3D.Transform(worldPosition, MySector.MainCamera.ViewMatrix);
      Vector4D vector4D = Vector4D.Transform(position, MySector.MainCamera.ProjectionMatrix);
      if (position.Z > 0.0)
      {
        vector4D.X *= -1.0;
        vector4D.Y *= -1.0;
      }
      if (vector4D.W == 0.0)
      {
        projectedPoint2D = Vector2.Zero;
        isBehind = false;
        return false;
      }
      projectedPoint2D = new Vector2((float) (vector4D.X / vector4D.W / 2.0) + 0.5f, (float) (-vector4D.Y / vector4D.W / 2.0 + 0.5));
      if (MyVideoSettingsManager.IsTripleHead())
        projectedPoint2D.X = (float) (((double) projectedPoint2D.X - 0.333333343267441) / 0.333333343267441);
      Vector3D vector2 = worldPosition - MyHudMarkerRender.CameraMatrix.Translation;
      vector2.Normalize();
      double num = Vector3D.Dot((Vector3D) MySector.MainCamera.ForwardVector, vector2);
      isBehind = num < 0.0;
      return true;
    }

    public override void Draw()
    {
      Vector3D position = MySector.MainCamera.Position;
      List<MyHudMarkerRender.PointOfInterest> pointOfInterestList = new List<MyHudMarkerRender.PointOfInterest>();
      if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.FullDisplay)
      {
        pointOfInterestList.AddRange((IEnumerable<MyHudMarkerRender.PointOfInterest>) this.m_pointsOfInterest);
      }
      else
      {
        for (int index1 = 0; index1 < this.m_pointsOfInterest.Count; ++index1)
        {
          MyHudMarkerRender.PointOfInterest poi1 = this.m_pointsOfInterest[index1];
          MyHudMarkerRender.PointOfInterest pointOfInterest = (MyHudMarkerRender.PointOfInterest) null;
          if (poi1.AlwaysVisible)
          {
            pointOfInterestList.Add(poi1);
          }
          else
          {
            if (poi1.AllowsCluster)
            {
              int index2 = index1 + 1;
              while (index2 < this.m_pointsOfInterest.Count)
              {
                MyHudMarkerRender.PointOfInterest poi2 = this.m_pointsOfInterest[index2];
                if (poi2 == poi1)
                  ++index2;
                else if (!poi2.AllowsCluster)
                  ++index2;
                else if (poi1.IsPOINearby(poi2, position))
                {
                  if (pointOfInterest == null)
                  {
                    pointOfInterest = this.m_pointOfInterestPool.Allocate();
                    pointOfInterest.Reset();
                    pointOfInterest.SetState(Vector3D.Zero, MyHudMarkerRender.PointOfInterest.PointOfInterestType.Group, MyRelationsBetweenPlayerAndBlock.NoOwnership);
                    pointOfInterest.AddPOI(poi1);
                  }
                  pointOfInterest.AddPOI(poi2);
                  this.m_pointsOfInterest.RemoveAt(index2);
                }
                else
                  ++index2;
              }
            }
            else if (poi1.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Target && (position - poi1.WorldPosition).Length() > 2000.0)
              continue;
            if (pointOfInterest != null)
              pointOfInterestList.Add(pointOfInterest);
            else
              pointOfInterestList.Add(poi1);
          }
        }
      }
      pointOfInterestList.Sort((Comparison<MyHudMarkerRender.PointOfInterest>) ((a, b) => b.DistanceToCam.CompareTo(a.DistanceToCam)));
      List<Vector2D> vector2DList = new List<Vector2D>(pointOfInterestList.Count);
      List<Vector2> vector2List = new List<Vector2>(pointOfInterestList.Count);
      List<bool> boolList = new List<bool>(pointOfInterestList.Count);
      if (!MyHudMarkerRender.m_disableFading && MyHudMarkerRender.SignalDisplayMode != MyHudMarkerRender.SignalMode.FullDisplay)
      {
        for (int index1 = pointOfInterestList.Count - 1; index1 >= 0; --index1)
        {
          Vector3D worldPosition = pointOfInterestList[index1].WorldPosition;
          Vector3D screen = MySector.MainCamera.WorldToScreen(ref worldPosition);
          Vector2D vector2D = new Vector2D(screen.X, screen.Y);
          bool flag = Vector3D.Dot(pointOfInterestList[index1].WorldPosition - MyHudMarkerRender.CameraMatrix.Translation, MyHudMarkerRender.CameraMatrix.Forward) < 0.0;
          float num1 = float.MaxValue;
          for (int index2 = 0; index2 < vector2DList.Count; ++index2)
          {
            if (flag == boolList[index2])
            {
              float num2 = (float) (vector2DList[index2] - vector2D).LengthSquared();
              if ((double) num2 < (double) num1)
                num1 = num2;
            }
          }
          float x;
          float y;
          if ((double) num1 > 0.0219999998807907)
          {
            x = 1f;
            y = 1f;
          }
          else if ((double) num1 > 11.0 / 1000.0)
          {
            x = (float) (81.8099975585938 * (double) num1 - 0.800000011920929);
            y = (float) (90.0 * (double) num1 - 0.980000019073486);
          }
          else
          {
            x = 0.1f;
            y = 0.01f;
          }
          vector2DList.Add(vector2D);
          vector2List.Add(new Vector2(x, y));
          boolList.Add(flag);
        }
      }
      if (MyHudMarkerRender.m_disableFading || MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.FullDisplay)
      {
        for (int index = 0; index < pointOfInterestList.Count; ++index)
        {
          int count = pointOfInterestList.Count;
          pointOfInterestList[index].Draw(this, scale: (pointOfInterestList[index].POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Objective ? 2f : 1f), drawBox: (pointOfInterestList[index].POIType != MyHudMarkerRender.PointOfInterest.PointOfInterestType.Objective));
        }
      }
      else
      {
        for (int index1 = 0; index1 < pointOfInterestList.Count; ++index1)
        {
          int index2 = pointOfInterestList.Count - index1 - 1;
          pointOfInterestList[index1].Draw(this, vector2List[index2].X, vector2List[index2].Y, pointOfInterestList[index1].POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Objective ? 2f : 1f, pointOfInterestList[index1].POIType != MyHudMarkerRender.PointOfInterest.PointOfInterestType.Objective);
        }
      }
      foreach (MyHudMarkerRender.PointOfInterest pointOfInterest in this.m_pointsOfInterest)
      {
        pointOfInterest.Reset();
        this.m_pointOfInterestPool.Deallocate(pointOfInterest);
      }
      MyHudMarkerRender.MyPlayerIndicator myPlayerIndicator1 = (MyHudMarkerRender.MyPlayerIndicator) null;
      foreach (MyHudMarkerRender.MyPlayerIndicator myPlayerIndicator2 in (IEnumerable<MyHudMarkerRender.MyPlayerIndicator>) this.m_playerIndicatorsDict.Values)
      {
        if (MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.Off)
        {
          if (!myPlayerIndicator2.IsAlwaysVisible)
            myPlayerIndicator1 = myPlayerIndicator2;
        }
        else if (!myPlayerIndicator2.Draw())
          myPlayerIndicator1 = myPlayerIndicator2;
      }
      if (myPlayerIndicator1 != null)
        this.m_playerIndicatorsDict.Remove<MyEntity, MyHudMarkerRender.MyPlayerIndicator>(myPlayerIndicator1.TargetEntity);
      this.m_pointsOfInterest.Clear();
    }

    public static float Normalize(float value) => MyHudMarkerRender.NormalizeLog(value, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);

    public static float Denormalize(float value) => MyHudMarkerRender.DenormalizeLog(value, 0.1f, MyPerGameSettings.MaxAntennaDrawDistance);

    private static float NormalizeLog(float f, float min, float max) => MathHelper.Clamp(MathHelper.InterpLogInv(f, min, max), 0.0f, 1f);

    private static float DenormalizeLog(float f, float min, float max) => MathHelper.Clamp(MathHelper.InterpLog(f, min, max), min, max);

    public enum SignalMode
    {
      DefaultMode,
      FullDisplay,
      NoNames,
      Off,
      MaxSignalModes,
    }

    private class PointOfInterest
    {
      public const double ClusterAngle = 10.0;
      public const int MaxTextLength = 64;
      public const double ClusterNearDistance = 3500.0;
      public const double ClusterScaleDistance = 20000.0;
      public const double MinimumTargetRange = 2000.0;
      public const double OreDistance = 200.0;
      private const double AngleConversion = 0.00872664625997165;
      public static readonly Color DefaultColor = new Color(117, 201, 241);
      public Color Color = MyHudMarkerRender.PointOfInterest.DefaultColor;
      public List<MyHudMarkerRender.PointOfInterest> m_group = new List<MyHudMarkerRender.PointOfInterest>(10);
      private bool m_alwaysVisible;

      public Vector3D WorldPosition { get; private set; }

      public MyHudMarkerRender.PointOfInterest.PointOfInterestType POIType { get; private set; }

      public MyRelationsBetweenPlayerAndBlock Relationship { get; private set; }

      public MyEntity Entity { get; private set; }

      public StringBuilder Text { get; private set; }

      public double Distance { get; private set; }

      public double DistanceToCam { get; private set; }

      public string ContainerRemainingTime { get; set; }

      public bool AlwaysVisible
      {
        get => this.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Ore && this.Distance < 200.0 || this.m_alwaysVisible;
        set => this.m_alwaysVisible = value;
      }

      public bool AllowsCluster => !this.AlwaysVisible && this.POIType != MyHudMarkerRender.PointOfInterest.PointOfInterestType.Target && (this.POIType != MyHudMarkerRender.PointOfInterest.PointOfInterestType.Ore || this.Distance >= 200.0);

      public PointOfInterest()
      {
        this.WorldPosition = Vector3D.Zero;
        this.POIType = MyHudMarkerRender.PointOfInterest.PointOfInterestType.Unknown;
        this.Relationship = MyRelationsBetweenPlayerAndBlock.Owner;
        this.Text = new StringBuilder(64, 64);
      }

      public override string ToString() => this.POIType.ToString() + ": " + (object) this.Text + " (" + (object) this.Distance + ")";

      public void Reset()
      {
        this.WorldPosition = Vector3D.Zero;
        this.POIType = MyHudMarkerRender.PointOfInterest.PointOfInterestType.Unknown;
        this.Relationship = MyRelationsBetweenPlayerAndBlock.Owner;
        this.Entity = (MyEntity) null;
        this.Text.Clear();
        this.m_group.Clear();
        this.Distance = 0.0;
        this.DistanceToCam = 0.0;
        this.AlwaysVisible = false;
        this.ContainerRemainingTime = (string) null;
        this.Color = MyHudMarkerRender.PointOfInterest.DefaultColor;
      }

      public void SetState(
        Vector3D position,
        MyHudMarkerRender.PointOfInterest.PointOfInterestType type,
        MyRelationsBetweenPlayerAndBlock relationship)
      {
        this.WorldPosition = position;
        this.POIType = type;
        this.Relationship = relationship;
        this.Distance = (position - MyHudMarkerRender.GetDistanceMeasuringMatrix().Translation).Length();
        this.DistanceToCam = (this.WorldPosition - MyHudMarkerRender.CameraMatrix.Translation).Length();
      }

      public void SetEntity(MyEntity entity) => this.Entity = entity;

      public void SetText(StringBuilder text)
      {
        this.Text.Clear();
        if (text == null)
          return;
        this.Text.AppendSubstring(text, 0, Math.Min(text.Length, 64));
      }

      public void SetText(string text)
      {
        this.Text.Clear();
        if (string.IsNullOrWhiteSpace(text))
          return;
        this.Text.Append(text, 0, Math.Min(text.Length, 64));
      }

      public bool AddPOI(MyHudMarkerRender.PointOfInterest poi)
      {
        if (this.POIType != MyHudMarkerRender.PointOfInterest.PointOfInterestType.Group)
          return false;
        Vector3D vector3D = this.WorldPosition * (double) this.m_group.Count;
        this.m_group.Add(poi);
        this.Text.Clear();
        this.Text.Append(this.m_group.Count);
        switch (this.GetGroupRelation())
        {
          case MyRelationsBetweenPlayerAndBlock.NoOwnership:
            this.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Mixed));
            break;
          case MyRelationsBetweenPlayerAndBlock.Owner:
            this.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Own));
            break;
          case MyRelationsBetweenPlayerAndBlock.FactionShare:
            this.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Friendly));
            break;
          case MyRelationsBetweenPlayerAndBlock.Neutral:
            this.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Neutral));
            break;
          case MyRelationsBetweenPlayerAndBlock.Enemies:
            this.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Signal_Enemy));
            break;
        }
        this.WorldPosition = (vector3D + poi.WorldPosition) / (double) this.m_group.Count;
        this.Distance = (this.WorldPosition - MyHudMarkerRender.GetDistanceMeasuringMatrix().Translation).Length();
        this.DistanceToCam = (this.WorldPosition - MyHudMarkerRender.CameraMatrix.Translation).Length();
        if (poi.Relationship > this.Relationship)
          this.Relationship = poi.Relationship;
        return true;
      }

      public bool IsPOINearby(
        MyHudMarkerRender.PointOfInterest poi,
        Vector3D cameraPosition,
        double angle = 10.0)
      {
        Vector3D vector3D = 0.5 * (this.WorldPosition - poi.WorldPosition);
        double num1 = vector3D.LengthSquared();
        double num2 = (cameraPosition - (poi.WorldPosition + vector3D)).Length();
        double num3 = Math.Sin(angle * (Math.PI / 360.0)) * num2;
        double num4 = num3 * num3;
        return num1 <= num4;
      }

      public void GetColorAndFontForRelationship(
        MyRelationsBetweenPlayerAndBlock relationship,
        out Color color,
        out Color fontColor,
        out string font)
      {
        color = Color.White;
        fontColor = Color.White;
        font = "White";
        switch (relationship)
        {
          case MyRelationsBetweenPlayerAndBlock.Owner:
            color = new Color(117, 201, 241);
            fontColor = new Color(117, 201, 241);
            font = "Blue";
            break;
          case MyRelationsBetweenPlayerAndBlock.FactionShare:
          case MyRelationsBetweenPlayerAndBlock.Friends:
            color = new Color(101, 178, 90);
            font = "Green";
            break;
          case MyRelationsBetweenPlayerAndBlock.Enemies:
            color = new Color(227, 62, 63);
            font = "Red";
            break;
        }
      }

      public void GetPOIColorAndFontInformation(
        out Color poiColor,
        out Color fontColor,
        out string font,
        string text = null)
      {
        poiColor = Color.White;
        fontColor = Color.White;
        font = "White";
        switch (this.POIType)
        {
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Unknown:
            poiColor = Color.White;
            font = "White";
            fontColor = Color.White;
            break;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Group:
            bool flag = true;
            MyHudMarkerRender.PointOfInterest.PointOfInterestType pointOfInterestType = MyHudMarkerRender.PointOfInterest.PointOfInterestType.Unknown;
            if (text != null && text.Contains(MyTexts.Get(MySpaceTexts.Signal_Own).ToString()))
            {
              poiColor = this.Color;
              fontColor = this.Color;
              font = "Blue";
              break;
            }
            if (this.m_group.Count > 0)
            {
              this.m_group[0].GetPOIColorAndFontInformation(out poiColor, out fontColor, out font);
              pointOfInterestType = this.m_group[0].POIType;
            }
            for (int index = 1; index < this.m_group.Count; ++index)
            {
              if (this.m_group[index].POIType != pointOfInterestType)
              {
                flag = false;
                break;
              }
            }
            if (flag)
              break;
            this.GetColorAndFontForRelationship(this.GetGroupRelation(), out poiColor, out fontColor, out font);
            break;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Ore:
            poiColor = Color.Khaki;
            font = "White";
            fontColor = Color.Khaki;
            break;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.GPS:
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.ContractGPS:
            poiColor = this.Color;
            fontColor = this.Color;
            font = "Blue";
            break;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Objective:
            poiColor = this.Color * 1.3f;
            fontColor = this.Color * 1.3f;
            font = "Blue";
            break;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Scenario:
            poiColor = Color.DarkOrange;
            fontColor = Color.DarkOrange;
            font = "White";
            break;
          default:
            this.GetColorAndFontForRelationship(this.Relationship, out poiColor, out fontColor, out font);
            break;
        }
      }

      private MyRelationsBetweenPlayerAndBlock GetGroupRelation()
      {
        if (this.m_group == null || this.m_group.Count == 0)
          return MyRelationsBetweenPlayerAndBlock.NoOwnership;
        MyRelationsBetweenPlayerAndBlock betweenPlayerAndBlock = this.m_group[0].Relationship;
        for (int index = 1; index < this.m_group.Count; ++index)
        {
          if (this.m_group[index].Relationship != betweenPlayerAndBlock)
          {
            if (betweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.Owner && this.m_group[index].Relationship == MyRelationsBetweenPlayerAndBlock.FactionShare)
            {
              betweenPlayerAndBlock = MyRelationsBetweenPlayerAndBlock.FactionShare;
            }
            else
            {
              if (betweenPlayerAndBlock != MyRelationsBetweenPlayerAndBlock.FactionShare || this.m_group[index].Relationship != MyRelationsBetweenPlayerAndBlock.Owner)
                return MyRelationsBetweenPlayerAndBlock.NoOwnership;
              betweenPlayerAndBlock = MyRelationsBetweenPlayerAndBlock.FactionShare;
            }
          }
        }
        return betweenPlayerAndBlock == MyRelationsBetweenPlayerAndBlock.NoOwnership ? MyRelationsBetweenPlayerAndBlock.Neutral : betweenPlayerAndBlock;
      }

      public void Draw(
        MyHudMarkerRender renderer,
        float alphaMultiplierMarker = 1f,
        float alphaMultiplierText = 1f,
        float scale = 1f,
        bool drawBox = true)
      {
        Vector2 projectedPoint2D = Vector2.Zero;
        bool isBehind = false;
        if (!MyHudMarkerRender.TryComputeScreenPoint(this.WorldPosition, out projectedPoint2D, out isBehind))
          return;
        Vector2 vector2_1 = new Vector2((float) MyGuiManager.GetSafeFullscreenRectangle().Width, (float) MyGuiManager.GetSafeFullscreenRectangle().Height);
        Vector2 hudSize = MyGuiManager.GetHudSize();
        Vector2 hudSizeHalf = MyGuiManager.GetHudSizeHalf();
        float uiScale = MyGuiManager.UIScale;
        float num1 = vector2_1.Y / 1080f;
        Vector2 vector2_2 = projectedPoint2D * hudSize;
        Color white1 = Color.White;
        Color fontColor = Color.White;
        string font = "White";
        this.GetPOIColorAndFontInformation(out white1, out fontColor, out font, this.Text.ToString());
        Vector2 upVector = vector2_2 - hudSizeHalf;
        Vector3D vector3D = Vector3D.Transform(this.WorldPosition, MySector.MainCamera.ViewMatrix);
        Vector2 vector2_3 = hudSize * (1f - uiScale) / 2f + 0.04f;
        if ((double) vector2_2.X < (double) vector2_3.X || (double) vector2_2.X > (double) hudSize.X - (double) vector2_3.X || ((double) vector2_2.Y < (double) vector2_3.Y || (double) vector2_2.Y > (double) hudSize.Y - (double) vector2_3.Y) || vector3D.Z > 0.0)
        {
          if (this.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Target)
            return;
          Vector2 vector2_4 = Vector2.Normalize(upVector);
          Vector2 position = hudSizeHalf + hudSizeHalf * vector2_4 * 0.77f * uiScale;
          upVector = position - hudSizeHalf;
          if ((double) upVector.LengthSquared() > 9.99999943962493E-11)
            upVector.Normalize();
          else
            upVector = new Vector2(1f, 0.0f);
          float num2 = 0.0053336f / num1 / num1;
          renderer.AddTexturedQuad(MyHudTexturesEnum.DirectionIndicator, position, upVector, white1, num2, num2);
          vector2_2 = position - upVector * 0.006667f * 2f;
        }
        else
        {
          float num2 = scale * 0.006667f / num1 / num1;
          if (this.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Target)
          {
            renderer.AddTexturedQuad(MyHudTexturesEnum.TargetTurret, vector2_2, -Vector2.UnitY, Color.White, num2, num2);
            return;
          }
          if (drawBox)
            renderer.AddTexturedQuad(MyHudTexturesEnum.Target_neutral, vector2_2, -Vector2.UnitY, white1, num2, num2);
        }
        float num3 = 0.03f;
        float num4 = 0.07f;
        float num5 = 0.15f;
        float num6 = upVector.Length();
        float val;
        float num7;
        int num8;
        if ((double) num6 <= (double) num3)
        {
          val = 1f;
          num7 = 1f;
          num8 = 0;
        }
        else if ((double) num6 > (double) num3 && (double) num6 < (double) num4)
        {
          float num2 = num5 - num3;
          float num9 = (float) (1.0 - ((double) num6 - (double) num3) / (double) num2);
          val = num9 * num9;
          float num10 = num4 - num3;
          float num11 = (float) (1.0 - ((double) num6 - (double) num3) / (double) num10);
          num7 = num11 * num11;
          num8 = 1;
        }
        else if ((double) num6 >= (double) num4 && (double) num6 < (double) num5)
        {
          float num2 = num5 - num3;
          float num9 = (float) (1.0 - ((double) num6 - (double) num3) / (double) num2);
          val = num9 * num9;
          float num10 = num5 - num4;
          float num11 = (float) (1.0 - ((double) num6 - (double) num4) / (double) num10);
          num7 = num11 * num11;
          num8 = 2;
        }
        else
        {
          val = 0.0f;
          num7 = 0.0f;
          num8 = 2;
        }
        float num12 = MathHelper.Clamp((float) (((double) num6 - 0.200000002980232) / 0.5), 0.0f, 1f);
        float num13 = MyMath.Clamp(val, 0.0f, 1f);
        if (MyHudMarkerRender.m_disableFading || MyHudMarkerRender.SignalDisplayMode == MyHudMarkerRender.SignalMode.FullDisplay || this.AlwaysVisible)
        {
          num13 = 1f;
          num7 = 1f;
          num12 = 1f;
          num8 = 0;
        }
        Vector2 vector2_5 = new Vector2(0.0f, (float) ((double) scale * (double) num1 * 24.0) / (float) MyGuiManager.GetFullscreenRectangle().Width);
        if ((MyHudMarkerRender.SignalDisplayMode != MyHudMarkerRender.SignalMode.NoNames || this.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.ButtonMarker || (MyHudMarkerRender.m_disableFading || this.AlwaysVisible)) && ((double) num13 > 1.40129846432482E-45 && this.Text.Length > 0))
        {
          MyHudText myHudText = renderer.m_hudScreen.AllocateText();
          if (myHudText != null)
          {
            fontColor.A = (byte) ((double) byte.MaxValue * (double) alphaMultiplierText * (double) num13);
            myHudText.Start(font, vector2_2 - vector2_5, fontColor, 0.7f / num1, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
            myHudText.Append(this.Text);
          }
        }
        if (this.POIType != MyHudMarkerRender.PointOfInterest.PointOfInterestType.Group)
        {
          byte a = white1.A;
          white1.A = (byte) ((double) byte.MaxValue * (double) alphaMultiplierMarker * (double) num12);
          MyHudMarkerRender.PointOfInterest.DrawIcon(renderer, this.POIType, this.Relationship, vector2_2, white1, scale);
          white1.A = a;
          MyHudText myHudText1 = renderer.m_hudScreen.AllocateText();
          if (myHudText1 != null)
          {
            StringBuilder stringBuilder = new StringBuilder();
            MyHudMarkerRender.AppendDistance(stringBuilder, this.Distance);
            fontColor.A = (byte) ((double) alphaMultiplierText * (double) byte.MaxValue);
            myHudText1.Start(font, vector2_2 + vector2_5 * (float) (0.699999988079071 + 0.300000011920929 * (double) num13), fontColor, (float) (0.5 + 0.200000002980232 * (double) num13) / num1, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
            myHudText1.Append(stringBuilder);
          }
          if (string.IsNullOrEmpty(this.ContainerRemainingTime))
            return;
          MyHudText myHudText2 = renderer.m_hudScreen.AllocateText();
          myHudText2.Start(font, vector2_2 + vector2_5 * (float) (1.60000002384186 + 0.300000011920929 * (double) num13), fontColor, (float) (0.5 + 0.200000002980232 * (double) num13) / num1, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          myHudText2.Append(this.ContainerRemainingTime);
        }
        else
        {
          Dictionary<MyRelationsBetweenPlayerAndBlock, List<MyHudMarkerRender.PointOfInterest>> significantGroupPoIs = this.GetSignificantGroupPOIs();
          Vector2[] vector2Array1 = new Vector2[5]
          {
            new Vector2(-6f, -4f),
            new Vector2(6f, -4f),
            new Vector2(-6f, 4f),
            new Vector2(6f, 4f),
            new Vector2(0.0f, 12f)
          };
          Vector2[] vector2Array2 = new Vector2[5]
          {
            new Vector2(16f, -4f),
            new Vector2(16f, 4f),
            new Vector2(16f, 12f),
            new Vector2(16f, 20f),
            new Vector2(16f, 28f)
          };
          for (int index = 0; index < vector2Array1.Length; ++index)
          {
            float num2 = num8 < 2 ? 1f : num7;
            float y1 = vector2Array1[index].Y;
            vector2Array1[index].X = (vector2Array1[index].X + 22f * num2) / (float) MyGuiManager.GetFullscreenRectangle().Width;
            vector2Array1[index].Y = y1 / 1080f / num1;
            if (MyVideoSettingsManager.IsTripleHead())
              vector2Array1[index].X /= 0.33f;
            if ((double) vector2Array1[index].Y <= 1.40129846432482E-45)
              vector2Array1[index].Y = y1 / 1080f;
            float y2 = vector2Array2[index].Y;
            vector2Array2[index].X = vector2Array2[index].X / (float) MyGuiManager.GetFullscreenRectangle().Width / num1;
            vector2Array2[index].Y = y2 / 1080f / num1;
            if (MyVideoSettingsManager.IsTripleHead())
              vector2Array2[index].X /= 0.33f;
            if ((double) vector2Array2[index].Y <= 1.40129846432482E-45)
              vector2Array2[index].Y = y2 / 1080f;
          }
          int index1 = 0;
          if (significantGroupPoIs.Count > 1)
          {
            MyRelationsBetweenPlayerAndBlock[] betweenPlayerAndBlockArray = new MyRelationsBetweenPlayerAndBlock[4]
            {
              MyRelationsBetweenPlayerAndBlock.Owner,
              MyRelationsBetweenPlayerAndBlock.FactionShare,
              MyRelationsBetweenPlayerAndBlock.Neutral,
              MyRelationsBetweenPlayerAndBlock.Enemies
            };
            foreach (MyRelationsBetweenPlayerAndBlock betweenPlayerAndBlock in betweenPlayerAndBlockArray)
            {
              if (significantGroupPoIs.ContainsKey(betweenPlayerAndBlock))
              {
                List<MyHudMarkerRender.PointOfInterest> pointOfInterestList = significantGroupPoIs[betweenPlayerAndBlock];
                if (pointOfInterestList.Count != 0)
                {
                  MyHudMarkerRender.PointOfInterest poi = pointOfInterestList[0];
                  if (poi != null)
                  {
                    if (poi.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.ContractGPS)
                      poi.GetPOIColorAndFontInformation(out white1, out fontColor, out font);
                    else
                      this.GetColorAndFontForRelationship(betweenPlayerAndBlock, out white1, out fontColor, out font);
                    float amount = num8 == 0 ? 1f : num7;
                    if (num8 >= 2)
                      amount = 0.0f;
                    Vector2 vector2_4 = Vector2.Lerp(vector2Array1[index1], vector2Array2[index1], amount);
                    string iconForRelationship = MyHudMarkerRender.PointOfInterest.GetIconForRelationship(betweenPlayerAndBlock);
                    white1.A = (byte) ((double) alphaMultiplierMarker * (double) white1.A);
                    MyHudMarkerRender.PointOfInterest.DrawIcon(renderer, iconForRelationship, vector2_2 + vector2_4, white1, 0.75f / num1);
                    if (this.IsPoiAtHighAlert(poi))
                    {
                      Color white2 = Color.White;
                      white2.A = (byte) ((double) alphaMultiplierMarker * (double) byte.MaxValue);
                      MyHudMarkerRender.PointOfInterest.DrawIcon(renderer, "Textures\\HUD\\marker_alert.dds", vector2_2 + vector2_4, white2, 0.75f / num1);
                    }
                    if ((MyHudMarkerRender.SignalDisplayMode != MyHudMarkerRender.SignalMode.NoNames || MyHudMarkerRender.m_disableFading || this.AlwaysVisible) && poi.Text.Length > 0)
                    {
                      MyHudText myHudText = renderer.m_hudScreen.AllocateText();
                      if (myHudText != null)
                      {
                        float num2 = 1f;
                        if (num8 == 1)
                          num2 = num7;
                        else if (num8 > 1)
                          num2 = 0.0f;
                        fontColor.A = (byte) ((double) byte.MaxValue * (double) alphaMultiplierText * (double) num2);
                        Vector2 vector2_6 = new Vector2(8f / (float) MyGuiManager.GetFullscreenRectangle().Width, 0.0f);
                        vector2_6.X /= num1;
                        myHudText.Start(font, vector2_2 + vector2_4 + vector2_6, fontColor, 0.55f / num1, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
                        myHudText.Append(poi.Text);
                      }
                    }
                    ++index1;
                  }
                }
              }
            }
          }
          else
          {
            using (Dictionary<MyRelationsBetweenPlayerAndBlock, List<MyHudMarkerRender.PointOfInterest>>.Enumerator enumerator = significantGroupPoIs.GetEnumerator())
            {
label_86:
              while (enumerator.MoveNext())
              {
                KeyValuePair<MyRelationsBetweenPlayerAndBlock, List<MyHudMarkerRender.PointOfInterest>> current = enumerator.Current;
                MyRelationsBetweenPlayerAndBlock key = current.Key;
                if (significantGroupPoIs.ContainsKey(key))
                {
                  List<MyHudMarkerRender.PointOfInterest> pointOfInterestList = current.Value;
                  int index2 = 0;
                  while (true)
                  {
                    if (index2 < 4 && index2 < pointOfInterestList.Count)
                    {
                      MyHudMarkerRender.PointOfInterest poi = pointOfInterestList[index2];
                      if (poi != null)
                      {
                        if (poi.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Scenario || poi.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.ContractGPS || poi.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Ore)
                        {
                          poi.GetPOIColorAndFontInformation(out white1, out fontColor, out font);
                        }
                        else
                        {
                          this.GetColorAndFontForRelationship(key, out white1, out fontColor, out font);
                          fontColor = poi.Color;
                        }
                        float amount = num8 == 0 ? 1f : num7;
                        if (num8 >= 2)
                          amount = 0.0f;
                        Vector2 vector2_4 = Vector2.Lerp(vector2Array1[index1], vector2Array2[index1], amount);
                        string centerIconSprite = poi.POIType != MyHudMarkerRender.PointOfInterest.PointOfInterestType.Scenario ? MyHudMarkerRender.PointOfInterest.GetIconForRelationship(key) : "Textures\\HUD\\marker_scenario.dds";
                        white1.A = (byte) ((double) alphaMultiplierMarker * (double) white1.A);
                        MyHudMarkerRender.PointOfInterest.DrawIcon(renderer, centerIconSprite, vector2_2 + vector2_4, white1, 0.75f / num1);
                        if (this.ShouldDrawHighAlertMark(poi))
                        {
                          Color white2 = Color.White;
                          white2.A = (byte) ((double) alphaMultiplierMarker * (double) byte.MaxValue);
                          MyHudMarkerRender.PointOfInterest.DrawIcon(renderer, "Textures\\HUD\\marker_alert.dds", vector2_2 + vector2_4, white2, 0.75f / num1);
                        }
                        if ((MyHudMarkerRender.SignalDisplayMode != MyHudMarkerRender.SignalMode.NoNames || MyHudMarkerRender.m_disableFading || this.AlwaysVisible) && poi.Text.Length > 0)
                        {
                          MyHudText myHudText = renderer.m_hudScreen.AllocateText();
                          if (myHudText != null)
                          {
                            float num2 = 1f;
                            if (num8 == 1)
                              num2 = num7;
                            else if (num8 > 1)
                              num2 = 0.0f;
                            fontColor.A = (byte) ((double) byte.MaxValue * (double) alphaMultiplierText * (double) num2);
                            Vector2 vector2_6 = new Vector2(8f / (float) MyGuiManager.GetFullscreenRectangle().Width, 0.0f);
                            vector2_6.X /= num1;
                            myHudText.Start(font, vector2_2 + vector2_4 + vector2_6, fontColor, 0.55f / num1, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
                            myHudText.Append(poi.Text);
                          }
                        }
                        ++index1;
                      }
                      ++index2;
                    }
                    else
                      goto label_86;
                  }
                }
              }
            }
          }
          this.GetPOIColorAndFontInformation(out white1, out fontColor, out font);
          float amount1 = num8 == 0 ? 1f : num7;
          if (num8 >= 2)
            amount1 = 0.0f;
          Vector2 vector2_7 = Vector2.Lerp(vector2Array1[4], vector2Array2[index1], amount1);
          Vector2 vector2_8 = Vector2.Lerp(Vector2.Zero, new Vector2(0.02222222f / num1, 1f / 270f / num1), amount1);
          MyHudText myHudText1 = renderer.m_hudScreen.AllocateText();
          if (myHudText1 == null)
            return;
          StringBuilder stringBuilder = new StringBuilder();
          MyHudMarkerRender.AppendDistance(stringBuilder, this.Distance);
          myHudText1.Start(font, vector2_2 + vector2_7 + vector2_8, this.Color, (float) (0.5 + 0.200000002980232 * (double) num13) / num1, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
          myHudText1.Append(stringBuilder);
        }
      }

      private Dictionary<MyRelationsBetweenPlayerAndBlock, List<MyHudMarkerRender.PointOfInterest>> GetSignificantGroupPOIs()
      {
        Dictionary<MyRelationsBetweenPlayerAndBlock, List<MyHudMarkerRender.PointOfInterest>> dictionary = new Dictionary<MyRelationsBetweenPlayerAndBlock, List<MyHudMarkerRender.PointOfInterest>>();
        if (this.m_group == null || this.m_group.Count == 0)
          return dictionary;
        bool flag = true;
        MyRelationsBetweenPlayerAndBlock relationship = this.m_group[0].Relationship;
        for (int index = 1; index < this.m_group.Count; ++index)
        {
          if (this.m_group[index].Relationship != relationship)
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          this.m_group.Sort(new Comparison<MyHudMarkerRender.PointOfInterest>(this.ComparePointOfInterest));
          dictionary[relationship] = new List<MyHudMarkerRender.PointOfInterest>();
          for (int index = this.m_group.Count - 1; index >= 0; --index)
          {
            dictionary[relationship].Add(this.m_group[index]);
            if (dictionary[relationship].Count >= 4)
              break;
          }
        }
        else
        {
          for (int index = 0; index < this.m_group.Count; ++index)
          {
            MyHudMarkerRender.PointOfInterest poiA = this.m_group[index];
            MyRelationsBetweenPlayerAndBlock key = poiA.Relationship;
            if (key == MyRelationsBetweenPlayerAndBlock.NoOwnership)
              key = MyRelationsBetweenPlayerAndBlock.Neutral;
            if (dictionary.ContainsKey(key))
            {
              if (this.ComparePointOfInterest(poiA, dictionary[key][0]) > 0)
              {
                dictionary[key].Clear();
                dictionary[key].Add(poiA);
              }
            }
            else
            {
              dictionary[key] = new List<MyHudMarkerRender.PointOfInterest>();
              dictionary[key].Add(poiA);
            }
          }
        }
        return dictionary;
      }

      private bool IsRelationHostile(
        MyRelationsBetweenPlayerAndBlock relationshipA,
        MyRelationsBetweenPlayerAndBlock relationshipB)
      {
        if (relationshipA == MyRelationsBetweenPlayerAndBlock.Owner || relationshipA == MyRelationsBetweenPlayerAndBlock.FactionShare)
          return relationshipB == MyRelationsBetweenPlayerAndBlock.Enemies;
        return (relationshipB == MyRelationsBetweenPlayerAndBlock.Owner || relationshipB == MyRelationsBetweenPlayerAndBlock.FactionShare) && relationshipA == MyRelationsBetweenPlayerAndBlock.Enemies;
      }

      private bool IsPoiAtHighAlert(MyHudMarkerRender.PointOfInterest poi)
      {
        if (poi.Relationship == MyRelationsBetweenPlayerAndBlock.Neutral)
          return false;
        if (poi.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.Scenario)
          return true;
        foreach (MyHudMarkerRender.PointOfInterest pointOfInterest in this.m_group)
        {
          if (this.IsRelationHostile(poi.Relationship, pointOfInterest.Relationship) && (double) ((Vector3) (pointOfInterest.WorldPosition - poi.WorldPosition)).LengthSquared() < 1000000.0)
            return true;
        }
        return false;
      }

      private bool ShouldDrawHighAlertMark(MyHudMarkerRender.PointOfInterest poi) => poi.POIType != MyHudMarkerRender.PointOfInterest.PointOfInterestType.Scenario && this.IsPoiAtHighAlert(poi);

      private bool IsGrid() => this.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.SmallEntity || this.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.LargeEntity || this.POIType == MyHudMarkerRender.PointOfInterest.PointOfInterestType.StaticEntity;

      private static void DrawIcon(
        MyHudMarkerRender renderer,
        MyHudMarkerRender.PointOfInterest.PointOfInterestType poiType,
        MyRelationsBetweenPlayerAndBlock relationship,
        Vector2 screenPosition,
        Color markerColor,
        float sizeScale = 1f)
      {
        string empty = string.Empty;
        Vector2 vector2_1 = new Vector2(12f, 12f);
        MyHudTexturesEnum texture;
        switch (poiType)
        {
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Unknown:
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.UnknownEntity:
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Character:
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.SmallEntity:
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.LargeEntity:
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.StaticEntity:
            string iconForRelationship = MyHudMarkerRender.PointOfInterest.GetIconForRelationship(relationship);
            MyHudMarkerRender.PointOfInterest.DrawIcon(renderer, iconForRelationship, screenPosition, markerColor, sizeScale);
            return;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Target:
            texture = MyHudTexturesEnum.TargetTurret;
            break;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Group:
            return;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Ore:
            texture = MyHudTexturesEnum.HudOre;
            markerColor = Color.Khaki;
            break;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Hack:
            texture = MyHudTexturesEnum.hit_confirmation;
            break;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.GPS:
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Objective:
            string centerIconSprite1 = "Textures\\HUD\\marker_gps.dds";
            MyHudMarkerRender.PointOfInterest.DrawIcon(renderer, centerIconSprite1, screenPosition, markerColor, sizeScale);
            return;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.ButtonMarker:
            return;
          case MyHudMarkerRender.PointOfInterest.PointOfInterestType.Scenario:
            string centerIconSprite2 = "Textures\\HUD\\marker_scenario.dds";
            MyHudMarkerRender.PointOfInterest.DrawIcon(renderer, centerIconSprite2, screenPosition, markerColor, sizeScale);
            return;
          default:
            return;
        }
        if (!string.IsNullOrWhiteSpace(empty))
        {
          Vector2 vector2_2 = vector2_1 * sizeScale;
          renderer.AddTexturedQuad(empty, screenPosition, -Vector2.UnitY, markerColor, vector2_2.X, vector2_2.Y);
        }
        else
        {
          float num = 0.0053336f * sizeScale;
          renderer.AddTexturedQuad(texture, screenPosition, -Vector2.UnitY, markerColor, num, num);
        }
      }

      public static string GetIconForRelationship(MyRelationsBetweenPlayerAndBlock relationship)
      {
        string str = string.Empty;
        switch (relationship)
        {
          case MyRelationsBetweenPlayerAndBlock.NoOwnership:
          case MyRelationsBetweenPlayerAndBlock.Neutral:
            str = "Textures\\HUD\\marker_neutral.dds";
            break;
          case MyRelationsBetweenPlayerAndBlock.Owner:
            str = "Textures\\HUD\\marker_self.dds";
            break;
          case MyRelationsBetweenPlayerAndBlock.FactionShare:
          case MyRelationsBetweenPlayerAndBlock.Friends:
            str = "Textures\\HUD\\marker_friendly.dds";
            break;
          case MyRelationsBetweenPlayerAndBlock.Enemies:
            str = "Textures\\HUD\\marker_enemy.dds";
            break;
        }
        return str;
      }

      private static void DrawIcon(
        MyHudMarkerRender renderer,
        string centerIconSprite,
        Vector2 screenPosition,
        Color markerColor,
        float sizeScale = 1f)
      {
        Vector2 vector2 = new Vector2(8f, 8f) * sizeScale;
        renderer.AddTexturedQuad(centerIconSprite, screenPosition, -Vector2.UnitY, markerColor, vector2.X, vector2.Y);
      }

      private int ComparePointOfInterest(
        MyHudMarkerRender.PointOfInterest poiA,
        MyHudMarkerRender.PointOfInterest poiB)
      {
        int num1 = this.IsPoiAtHighAlert(poiA).CompareTo(this.IsPoiAtHighAlert(poiB));
        if (num1 != 0)
          return num1;
        if (poiA.POIType >= MyHudMarkerRender.PointOfInterest.PointOfInterestType.UnknownEntity && poiB.POIType >= MyHudMarkerRender.PointOfInterest.PointOfInterestType.UnknownEntity)
        {
          int num2 = poiA.POIType.CompareTo((object) poiB.POIType);
          if (num2 != 0)
            return num2;
        }
        if (poiA.IsGrid() && poiB.IsGrid())
        {
          MyCubeBlock entity1 = poiA.Entity as MyCubeBlock;
          MyCubeBlock entity2 = poiB.Entity as MyCubeBlock;
          if (entity1 != null && entity2 != null)
          {
            int num2 = entity1.CubeGrid.BlocksCount.CompareTo(entity2.CubeGrid.BlocksCount);
            if (num2 != 0)
              return num2;
          }
        }
        return poiB.Distance.CompareTo(poiA.Distance);
      }

      public enum PointOfInterestState
      {
        NonDirectional,
        Directional,
      }

      public enum PointOfInterestType
      {
        Unknown,
        Target,
        Group,
        Ore,
        Hack,
        UnknownEntity,
        Character,
        SmallEntity,
        LargeEntity,
        StaticEntity,
        GPS,
        ButtonMarker,
        Objective,
        Scenario,
        ContractGPS,
      }
    }

    private class MyPlayerIndicator
    {
      private MyHudMarkerRender.MyPlayerIndicator.MyPlayerIndicatorStatus m_status;
      private Color m_relationIndicatorColor_original = Color.White;
      private Color m_relationIndicatorColor_toDraw = Color.White;
      private MyRelationsBetweenPlayers m_targetRelation = MyRelationsBetweenPlayers.Neutral;

      public MyEntity TargetEntity { get; }

      public bool IsAlwaysVisible { get; set; }

      public MyPlayerIndicator(
        MyEntity targetEntity,
        MyRelationsBetweenPlayers relation,
        bool isAlwaysVisible)
      {
        this.TargetEntity = targetEntity;
        this.m_targetRelation = relation;
        this.m_relationIndicatorColor_original = MyHudMarkerRender.MyPlayerIndicator.GetRelationIndicatorColor(relation);
        this.m_relationIndicatorColor_toDraw = MyHudMarkerRender.MyPlayerIndicator.GetRelationIndicatorColor(relation);
        this.m_status = MyHudMarkerRender.MyPlayerIndicator.MyPlayerIndicatorStatus.Visible;
        this.IsAlwaysVisible = isAlwaysVisible;
      }

      private static Color GetRelationIndicatorColor(MyRelationsBetweenPlayers relation)
      {
        switch (relation)
        {
          case MyRelationsBetweenPlayers.Self:
            return new Color(61, 180, (int) byte.MaxValue, (int) byte.MaxValue);
          case MyRelationsBetweenPlayers.Allies:
            return new Color(106, 248, 0, (int) byte.MaxValue);
          case MyRelationsBetweenPlayers.Neutral:
            return new Color((int) byte.MaxValue, 210, 0, (int) byte.MaxValue);
          default:
            return new Color((int) byte.MaxValue, 70, 61, (int) byte.MaxValue);
        }
      }

      private byte FadeColorChannel(byte original, byte current, float secToFade = 2f)
      {
        float num1 = (float) original / (secToFade * 60f);
        float num2 = (float) current - num1;
        if ((double) num2 < 0.0)
          num2 = 0.0f;
        return (byte) num2;
      }

      public void ResetFadeout(MyRelationsBetweenPlayers relation)
      {
        this.m_status = MyHudMarkerRender.MyPlayerIndicator.MyPlayerIndicatorStatus.Visible;
        if (this.m_targetRelation != relation)
        {
          this.m_targetRelation = relation;
          this.m_relationIndicatorColor_original = MyHudMarkerRender.MyPlayerIndicator.GetRelationIndicatorColor(relation);
        }
        this.m_relationIndicatorColor_toDraw.A = this.m_relationIndicatorColor_original.A;
        this.m_relationIndicatorColor_toDraw.R = this.m_relationIndicatorColor_original.R;
        this.m_relationIndicatorColor_toDraw.G = this.m_relationIndicatorColor_original.G;
        this.m_relationIndicatorColor_toDraw.B = this.m_relationIndicatorColor_original.B;
      }

      public bool Draw()
      {
        if (MyHud.HudState == 0 || MyGuiScreenHudSpace.Static.State != MyGuiScreenState.OPENED)
          return true;
        if (!(this.TargetEntity is MyCharacter targetEntity) || targetEntity.IsDead)
          return false;
        if (targetEntity.RadioBroadcaster != null && targetEntity.RadioBroadcaster.Enabled)
          return true;
        float num1 = 0.02f;
        float num2 = -0.02f;
        float scale = 1f;
        Vector2 projectedPoint2D;
        bool isBehind;
        if (!MyHudMarkerRender.TryComputeScreenPoint(this.TargetEntity.PositionComp.GetPosition() + ((double) this.TargetEntity.PositionComp.LocalAABB.Height + (double) num1) * this.TargetEntity.PositionComp.WorldMatrixRef.Up, out projectedPoint2D, out isBehind))
          return false;
        if (!isBehind)
        {
          Vector2 normalizedGuiPosition = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref projectedPoint2D);
          normalizedGuiPosition.Y += num2;
          string font = "Blue";
          string text = targetEntity.CustomNameWithFaction.ToString();
          if (text == "")
            text = targetEntity.UpdateCustomNameWithFaction().ToString();
          Vector2 textSize = MyGuiManager.MeasureString(font, text, MyGuiSandbox.GetDefaultTextScaleWithLanguage());
          MyGuiTextShadows.DrawShadow(ref normalizedGuiPosition, ref textSize, fogAlphaMultiplier: ((float) this.m_relationIndicatorColor_toDraw.A / (float) byte.MaxValue), ignoreBounds: true);
          MyGuiManager.DrawString(font, text, normalizedGuiPosition, scale, new Color?(this.m_relationIndicatorColor_toDraw), MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, ignoreBounds: true);
        }
        if (this.m_status == MyHudMarkerRender.MyPlayerIndicator.MyPlayerIndicatorStatus.Fading)
        {
          if (this.m_relationIndicatorColor_toDraw.A <= (byte) 0)
            return false;
          float secToFade = 2f;
          this.m_relationIndicatorColor_toDraw.A = this.FadeColorChannel(this.m_relationIndicatorColor_original.A, this.m_relationIndicatorColor_toDraw.A, secToFade);
          this.m_relationIndicatorColor_toDraw.R = this.FadeColorChannel(this.m_relationIndicatorColor_original.R, this.m_relationIndicatorColor_toDraw.R, secToFade);
          this.m_relationIndicatorColor_toDraw.G = this.FadeColorChannel(this.m_relationIndicatorColor_original.G, this.m_relationIndicatorColor_toDraw.G, secToFade);
          this.m_relationIndicatorColor_toDraw.B = this.FadeColorChannel(this.m_relationIndicatorColor_original.B, this.m_relationIndicatorColor_toDraw.B, secToFade);
        }
        this.m_status = MyHudMarkerRender.MyPlayerIndicator.MyPlayerIndicatorStatus.Fading;
        if (this.IsAlwaysVisible)
          this.ResetFadeout(MyRelationsBetweenPlayers.Allies);
        return true;
      }

      private enum MyPlayerIndicatorStatus
      {
        Visible,
        Fading,
      }
    }
  }
}
