// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.SessionComponents.MySessionComponentReplay
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Entities;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ObjectBuilders.Components;
using VRage.ObjectBuilder;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 2000, typeof (MyObjectBuilder_SessionComponentReplay), null, false)]
  public class MySessionComponentReplay : MySessionComponentBase
  {
    public static MySessionComponentReplay Static;
    private static Dictionary<long, Dictionary<int, PerFrameData>> m_recordedEntities = new Dictionary<long, Dictionary<int, PerFrameData>>();
    private ulong m_replayingStart = ulong.MaxValue;
    private ulong m_recordingStart = ulong.MaxValue;

    public bool HasRecordedData => MySessionComponentReplay.m_recordedEntities.Count > 0;

    public MySessionComponentReplay() => MySessionComponentReplay.Static = this;

    public void DeleteRecordings() => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings_Confirm), messageCaption: MyTexts.Get(MySpaceTexts.ScreenDebugAdminMenu_ReplayTool_DeleteRecordings), callback: new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnDeleteRecordingsClicked)));

    private void OnDeleteRecordingsClicked(MyGuiScreenMessageBox.ResultEnum callbackReturn)
    {
      if (callbackReturn != MyGuiScreenMessageBox.ResultEnum.YES)
        return;
      MySessionComponentReplay.m_recordedEntities.Clear();
    }

    public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
    {
      base.Init(sessionComponent);
      MyObjectBuilder_SessionComponentReplay sessionComponentReplay = sessionComponent as MyObjectBuilder_SessionComponentReplay;
      if (sessionComponentReplay.EntityReplayData == null)
        return;
      foreach (PerEntityData perEntityData in (List<PerEntityData>) sessionComponentReplay.EntityReplayData)
      {
        Dictionary<int, PerFrameData> dictionary = new Dictionary<int, PerFrameData>((IDictionary<int, PerFrameData>) perEntityData.Data.Dictionary);
        if (!MySessionComponentReplay.m_recordedEntities.ContainsKey(perEntityData.EntityId))
          MySessionComponentReplay.m_recordedEntities.Add(perEntityData.EntityId, dictionary);
      }
    }

    public override MyObjectBuilder_SessionComponent GetObjectBuilder()
    {
      MyObjectBuilder_SessionComponentReplay sessionComponentReplay = new MyObjectBuilder_SessionComponentReplay();
      if (MySessionComponentReplay.m_recordedEntities.Count > 0)
      {
        sessionComponentReplay.EntityReplayData = new MySerializableList<PerEntityData>();
        foreach (KeyValuePair<long, Dictionary<int, PerFrameData>> recordedEntity in MySessionComponentReplay.m_recordedEntities)
        {
          PerEntityData perEntityData = new PerEntityData();
          perEntityData.EntityId = recordedEntity.Key;
          perEntityData.Data = new SerializableDictionary<int, PerFrameData>();
          foreach (KeyValuePair<int, PerFrameData> keyValuePair in recordedEntity.Value)
            perEntityData.Data[keyValuePair.Key] = keyValuePair.Value;
          sessionComponentReplay.EntityReplayData.Add(perEntityData);
        }
      }
      return (MyObjectBuilder_SessionComponent) sessionComponentReplay;
    }

    public bool IsRecording => this.m_recordingStart != ulong.MaxValue;

    public bool IsReplaying => this.m_replayingStart != ulong.MaxValue;

    public void StartRecording()
    {
      MySessionComponentReplay.m_recordedEntities.Remove(MySession.Static.ControlledEntity.Entity.GetTopMostParent((Type) null).EntityId);
      this.m_recordingStart = MySandboxGame.Static.SimulationFrameCounter;
    }

    public void StopRecording() => this.m_recordingStart = ulong.MaxValue;

    public void StartReplay()
    {
      this.m_replayingStart = MySandboxGame.Static.SimulationFrameCounter;
      foreach (long key in MySessionComponentReplay.m_recordedEntities.Keys)
      {
        MyCubeGrid entity;
        if (MyEntities.TryGetEntityById<MyCubeGrid>(key, out entity))
          entity.StartReplay();
      }
    }

    public void StopReplay()
    {
      this.m_replayingStart = ulong.MaxValue;
      foreach (long key in MySessionComponentReplay.m_recordedEntities.Keys)
      {
        MyCubeGrid entity;
        if (MyEntities.TryGetEntityById<MyCubeGrid>(key, out entity))
          entity.StopReplay();
      }
    }

    public bool IsEntityBeingRecorded(long entityId) => this.IsRecording && MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity != null && MySession.Static.ControlledEntity.Entity.GetTopMostParent((Type) null).EntityId == entityId;

    public bool IsEntityBeingReplayed(long entityId) => this.IsReplaying && MySessionComponentReplay.m_recordedEntities.ContainsKey(entityId) && !this.IsEntityBeingRecorded(entityId);

    public bool IsEntityBeingReplayed(long entityId, out PerFrameData perFrameData)
    {
      Dictionary<int, PerFrameData> dictionary;
      if (this.IsReplaying && this.IsEntityBeingReplayed(entityId) && MySessionComponentReplay.m_recordedEntities.TryGetValue(entityId, out dictionary))
      {
        int key = (int) ((long) MySandboxGame.Static.SimulationFrameCounter - (long) this.m_replayingStart);
        if (dictionary.TryGetValue(key, out perFrameData))
          return true;
      }
      perFrameData = new PerFrameData();
      return false;
    }

    public bool HasAnyData => MySessionComponentReplay.m_recordedEntities.Count > 0;

    public bool HasEntityReplayData(long entityId) => MySessionComponentReplay.m_recordedEntities.ContainsKey(entityId);

    public void ProvideEntityRecordData(long entityId, PerFrameData data)
    {
      Dictionary<int, PerFrameData> dictionary;
      if (!MySessionComponentReplay.m_recordedEntities.TryGetValue(entityId, out dictionary))
      {
        dictionary = new Dictionary<int, PerFrameData>();
        MySessionComponentReplay.m_recordedEntities[entityId] = dictionary;
      }
      int key = (int) ((long) MySandboxGame.Static.SimulationFrameCounter - (long) this.m_recordingStart);
      PerFrameData perFrameData;
      if (dictionary.TryGetValue(key, out perFrameData))
      {
        if (data.MovementData.HasValue)
          perFrameData.MovementData = data.MovementData;
        if (data.SwitchWeaponData.HasValue)
        {
          SerializableDefinitionId? weaponDefinition = data.SwitchWeaponData.Value.WeaponDefinition;
          if (weaponDefinition.HasValue && !MyObjectBuilderType.IsValidTypeName(weaponDefinition?.TypeIdString))
            data.SwitchWeaponData = new SwitchWeaponData?(new SwitchWeaponData());
          perFrameData.SwitchWeaponData = data.SwitchWeaponData;
        }
        if (data.ShootData.HasValue)
          perFrameData.ShootData = data.ShootData;
        if (data.AnimationData.HasValue)
        {
          if (perFrameData.AnimationData.HasValue)
          {
            AnimationData animationData = new AnimationData()
            {
              Animation = perFrameData.AnimationData.Value.Animation,
              Animation2 = data.AnimationData.Value.Animation
            };
            perFrameData.AnimationData = new AnimationData?(animationData);
          }
          else
            perFrameData.AnimationData = data.AnimationData;
        }
        if (data.ControlSwitchesData.HasValue)
          perFrameData.ControlSwitchesData = data.ControlSwitchesData;
        if (data.UseData.HasValue)
          perFrameData.UseData = data.UseData;
      }
      else
        perFrameData = data;
      dictionary[key] = perFrameData;
    }

    protected override void UnloadData()
    {
      base.UnloadData();
      MySessionComponentReplay.Static = (MySessionComponentReplay) null;
    }

    public delegate void ActionRef<T>(ref T item);
  }
}
