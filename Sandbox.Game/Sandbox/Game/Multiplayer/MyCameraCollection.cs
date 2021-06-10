// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Multiplayer.MyCameraCollection
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Multiplayer
{
  [StaticEventOwner]
  internal class MyCameraCollection
  {
    private Dictionary<MyPlayer.PlayerId, Dictionary<long, MyEntityCameraSettings>> m_entityCameraSettings = new Dictionary<MyPlayer.PlayerId, Dictionary<long, MyEntityCameraSettings>>();
    private List<long> m_entitiesToRemove = new List<long>();
    private Dictionary<MyPlayer.PlayerId, MyEntityCameraSettings> m_lastCharacterSettings = new Dictionary<MyPlayer.PlayerId, MyEntityCameraSettings>();

    public void RequestSaveEntityCameraSettings(
      MyPlayer.PlayerId pid,
      long entityId,
      bool isFirstPerson,
      double distance,
      float headAngleX,
      float headAngleY,
      bool isLocalCharacter)
    {
      Vector2 vector2 = new Vector2(headAngleX, headAngleY);
      MyMultiplayer.RaiseStaticEvent<MyPlayer.PlayerId, long, bool, double, Vector2, bool>((Func<IMyEventOwner, Action<MyPlayer.PlayerId, long, bool, double, Vector2, bool>>) (x => new Action<MyPlayer.PlayerId, long, bool, double, Vector2, bool>(MyCameraCollection.OnSaveEntityCameraSettings)), pid, entityId, isFirstPerson, distance, vector2, isLocalCharacter);
    }

    [Event(null, 44)]
    [Reliable]
    [Server]
    private static void OnSaveEntityCameraSettings(
      MyPlayer.PlayerId playerId,
      long entityId,
      bool isFirstPerson,
      double distance,
      Vector2 headAngle,
      bool isLocalCharacter)
    {
      MySession.Static.Cameras.AddCameraData(new MyPlayer.PlayerId(playerId.SteamId, playerId.SerialId), entityId, isFirstPerson, distance, headAngle, isLocalCharacter);
    }

    public bool ContainsPlayer(MyPlayer.PlayerId pid) => this.m_entityCameraSettings.ContainsKey(pid);

    private void AddCameraData(
      MyPlayer.PlayerId pid,
      long entityId,
      bool isFirstPerson,
      double distance,
      Vector2 headAngle,
      bool isLocalCharacter)
    {
      MyEntityCameraSettings cameraSettings = (MyEntityCameraSettings) null;
      if (this.TryGetCameraSettings(pid, entityId, isLocalCharacter, out cameraSettings))
      {
        cameraSettings.IsFirstPerson = isFirstPerson;
        if (!isFirstPerson)
        {
          cameraSettings.Distance = distance;
          cameraSettings.HeadAngle = new Vector2?(headAngle);
        }
        if (!isLocalCharacter)
          return;
        this.m_lastCharacterSettings[pid] = cameraSettings;
      }
      else
      {
        MyEntityCameraSettings data = new MyEntityCameraSettings()
        {
          Distance = distance,
          IsFirstPerson = isFirstPerson,
          HeadAngle = new Vector2?(headAngle)
        };
        this.AddCameraData(pid, entityId, isLocalCharacter, data);
      }
    }

    private void AddCameraData(
      MyPlayer.PlayerId pid,
      long entityId,
      bool isLocalCharacter,
      MyEntityCameraSettings data)
    {
      if (!this.ContainsPlayer(pid))
        this.m_entityCameraSettings[pid] = new Dictionary<long, MyEntityCameraSettings>();
      if (this.m_entityCameraSettings[pid].ContainsKey(entityId))
        this.m_entityCameraSettings[pid][entityId] = data;
      else
        this.m_entityCameraSettings[pid].Add(entityId, data);
      if (!isLocalCharacter)
        return;
      this.m_lastCharacterSettings[pid] = data;
    }

    public bool TryGetCameraSettings(
      MyPlayer.PlayerId pid,
      long entityId,
      bool isLocalCharacter,
      out MyEntityCameraSettings cameraSettings)
    {
      if (Sandbox.Engine.Platform.Game.IsDedicated)
      {
        MySession.Static.Players.GetPlayerById(pid);
      }
      else
      {
        MyPlayer localHumanPlayer = MySession.Static.LocalHumanPlayer;
      }
      if (this.ContainsPlayer(pid))
      {
        if (this.m_entityCameraSettings[pid].ContainsKey(entityId))
          return this.m_entityCameraSettings[pid].TryGetValue(entityId, out cameraSettings);
        if (isLocalCharacter && this.m_lastCharacterSettings.ContainsKey(pid))
        {
          cameraSettings = this.m_lastCharacterSettings[pid];
          this.m_entityCameraSettings[pid][entityId] = cameraSettings;
          return true;
        }
      }
      cameraSettings = (MyEntityCameraSettings) null;
      return false;
    }

    public void LoadCameraCollection(MyObjectBuilder_Checkpoint checkpoint)
    {
      this.m_entityCameraSettings = new Dictionary<MyPlayer.PlayerId, Dictionary<long, MyEntityCameraSettings>>();
      SerializableDictionary<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> allPlayersData = checkpoint.AllPlayersData;
      if (allPlayersData == null)
        return;
      foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair in allPlayersData.Dictionary)
      {
        MyPlayer.PlayerId key = new MyPlayer.PlayerId(keyValuePair.Key.GetClientId(), keyValuePair.Key.SerialId);
        this.m_entityCameraSettings[key] = new Dictionary<long, MyEntityCameraSettings>();
        foreach (CameraControllerSettings controllerSettings in keyValuePair.Value.EntityCameraData)
        {
          MyEntityCameraSettings entityCameraSettings1 = new MyEntityCameraSettings();
          entityCameraSettings1.Distance = controllerSettings.Distance;
          SerializableVector2? headAngle = controllerSettings.HeadAngle;
          entityCameraSettings1.HeadAngle = headAngle.HasValue ? new Vector2?((Vector2) headAngle.GetValueOrDefault()) : new Vector2?();
          entityCameraSettings1.IsFirstPerson = controllerSettings.IsFirstPerson;
          MyEntityCameraSettings entityCameraSettings2 = entityCameraSettings1;
          this.m_entityCameraSettings[key][controllerSettings.EntityId] = entityCameraSettings2;
        }
      }
    }

    public void SaveCameraCollection(MyObjectBuilder_Checkpoint checkpoint)
    {
      if (checkpoint.AllPlayersData == null)
        return;
      foreach (KeyValuePair<MyObjectBuilder_Checkpoint.PlayerId, MyObjectBuilder_Player> keyValuePair1 in checkpoint.AllPlayersData.Dictionary)
      {
        MyPlayer.PlayerId key1 = new MyPlayer.PlayerId(keyValuePair1.Key.GetClientId(), keyValuePair1.Key.SerialId);
        keyValuePair1.Value.EntityCameraData = new List<CameraControllerSettings>();
        if (this.m_entityCameraSettings.ContainsKey(key1))
        {
          this.m_entitiesToRemove.Clear();
          foreach (KeyValuePair<long, MyEntityCameraSettings> keyValuePair2 in this.m_entityCameraSettings[key1])
          {
            if (MyEntities.EntityExists(keyValuePair2.Key))
            {
              CameraControllerSettings controllerSettings1 = new CameraControllerSettings();
              controllerSettings1.Distance = keyValuePair2.Value.Distance;
              controllerSettings1.IsFirstPerson = keyValuePair2.Value.IsFirstPerson;
              Vector2? headAngle = keyValuePair2.Value.HeadAngle;
              controllerSettings1.HeadAngle = headAngle.HasValue ? new SerializableVector2?((SerializableVector2) headAngle.GetValueOrDefault()) : new SerializableVector2?();
              controllerSettings1.EntityId = keyValuePair2.Key;
              CameraControllerSettings controllerSettings2 = controllerSettings1;
              keyValuePair1.Value.EntityCameraData.Add(controllerSettings2);
            }
            else
              this.m_entitiesToRemove.Add(keyValuePair2.Key);
          }
          foreach (long key2 in this.m_entitiesToRemove)
            this.m_entityCameraSettings[key1].Remove(key2);
        }
      }
    }

    public void SaveEntityCameraSettings(
      MyPlayer.PlayerId pid,
      long entityId,
      bool isFirstPerson,
      double distance,
      bool isLocalCharacter,
      float headAngleX,
      float headAngleY,
      bool sync = true)
    {
      if (!Sync.IsServer & sync)
        this.RequestSaveEntityCameraSettings(pid, entityId, isFirstPerson, distance, headAngleX, headAngleY, isLocalCharacter);
      Vector2 headAngle = new Vector2(headAngleX, headAngleY);
      this.AddCameraData(pid, entityId, isFirstPerson, distance, headAngle, isLocalCharacter);
    }

    protected sealed class OnSaveEntityCameraSettings\u003C\u003ESandbox_Game_World_MyPlayer\u003C\u003EPlayerId\u0023System_Int64\u0023System_Boolean\u0023System_Double\u0023VRageMath_Vector2\u0023System_Boolean : ICallSite<IMyEventOwner, MyPlayer.PlayerId, long, bool, double, Vector2, bool>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in MyPlayer.PlayerId playerId,
        in long entityId,
        in bool isFirstPerson,
        in double distance,
        in Vector2 headAngle,
        in bool isLocalCharacter)
      {
        MyCameraCollection.OnSaveEntityCameraSettings(playerId, entityId, isFirstPerson, distance, headAngle, isLocalCharacter);
      }
    }
  }
}
