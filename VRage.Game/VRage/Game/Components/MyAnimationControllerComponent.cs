// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyAnimationControllerComponent
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Game.Entity;
using VRage.Game.SessionComponents;
using VRage.Generics;
using VRage.Network;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace VRage.Game.Components
{
  public class MyAnimationControllerComponent : MyEntityComponentBase
  {
    private readonly MyAnimationController m_controller = new MyAnimationController();
    private List<MyAnimationClip.BoneState> m_lastBoneResult;
    private MyAnimationControllerComponent.BoneDataPack m_boneData;
    private bool m_componentValid;
    public readonly Action ReloadBonesNeeded;
    public float CameraDistance;
    private MyEntity m_entity;
    public float OtherLayersAnimationSpeed = 1f;
    public float MainLayerAnimationSpeed = 1f;

    public event Action<MyStringId> ActionTriggered;

    public override string ComponentTypeDebugString => "AnimationControllerComp";

    public override void OnAddedToContainer()
    {
      int num = this.m_entity.InScene ? 1 : 0;
      MySessionComponentAnimationSystem.Static.RegisterEntityComponent(this);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      int num = this.m_entity.InScene ? 1 : 0;
      MySessionComponentAnimationSystem.Static.UnregisterEntityComponent(this);
    }

    public List<MyAnimationTreeNode> GetKeyedAnimationTracks(string key) => this.m_controller.GetKeyedTracks(key);

    public void MarkAsValid()
    {
      int num = this.m_entity.InScene ? 1 : 0;
      this.m_componentValid = true;
    }

    private void MarkAsInvalid() => this.m_componentValid = false;

    public MyAnimationController Controller => this.m_controller;

    public IMyVariableStorage<float> Variables => (IMyVariableStorage<float>) this.m_controller.Variables;

    public MyCharacterBone[] CharacterBones => this.m_boneData.Bones;

    public MyAnimationInverseKinematics InverseKinematics => this.m_controller.InverseKinematics;

    private MyAnimationControllerComponent()
    {
    }

    public MyAnimationControllerComponent(
      MyEntity entity,
      Action obtainBones,
      IMyTerrainHeightProvider heightProvider)
    {
      this.m_entity = entity;
      this.ReloadBonesNeeded = obtainBones;
      this.m_controller.InverseKinematics.TerrainHeightProvider = heightProvider;
    }

    public void AttachAnimationEventCallback(Action<List<string>> action) => this.m_controller.OnAnimationEventTriggered += action;

    public void DetachAnimationEventCallback(Action<List<string>> action) => this.m_controller.OnAnimationEventTriggered -= action;

    public MyCharacterBone[] CharacterBonesSorted => this.m_boneData.SortedBones;

    public Matrix[] BoneRelativeTransforms => this.m_boneData.RelativeTransforms;

    public Matrix[] BoneAbsoluteTransforms => this.m_boneData.AbsoluteTransforms;

    public List<MyAnimationClip.BoneState> LastRawBoneResult => this.m_lastBoneResult;

    public MyDefinitionId SourceId { get; set; }

    public List<MyStringId> LastFrameActions
    {
      get
      {
        int num = this.m_entity.InScene ? 1 : 0;
        return (List<MyStringId>) null;
      }
    }

    public void SetCharacterBones(
      MyCharacterBone[] characterBones,
      Matrix[] relativeTransforms,
      Matrix[] absoluteTransforms)
    {
      int num = this.m_entity.InScene ? 1 : 0;
      this.m_controller.ResultBonesPool.Reset(characterBones);
      this.m_boneData = new MyAnimationControllerComponent.BoneDataPack(characterBones, relativeTransforms, absoluteTransforms);
    }

    public bool Update()
    {
      if ((double) this.CameraDistance > 200.0 || !this.m_componentValid || (!(this.Entity is IMySkinnedEntity) || !this.Entity.InScene))
        return false;
      MyAnimationUpdateData animationUpdateData = new MyAnimationUpdateData();
      animationUpdateData.DeltaTimeInSeconds = 0.0166666675359011 * (double) this.OtherLayersAnimationSpeed;
      animationUpdateData.DeltaTimeInSecondsForMainLayer = 0.0166666675359011 * (double) this.MainLayerAnimationSpeed;
      animationUpdateData.CharacterBones = this.m_boneData.Bones;
      animationUpdateData.Controller = (MyAnimationController) null;
      animationUpdateData.BonesResult = (List<MyAnimationClip.BoneState>) null;
      lock (this.m_controller)
        this.m_controller.Update(ref animationUpdateData);
      this.m_lastBoneResult = animationUpdateData.BonesResult;
      List<MyAnimationClip.BoneState> lastBoneResult = this.m_lastBoneResult;
      return true;
    }

    public void FinishUpdate()
    {
      if (this.m_lastBoneResult == null || this.m_lastBoneResult.Count != this.m_boneData.Bones.Length)
        return;
      for (int index = 0; index < this.m_lastBoneResult.Count; ++index)
        this.m_boneData.Bones[index].SetCompleteTransform(ref this.m_lastBoneResult[index].Translation, ref this.m_lastBoneResult[index].Rotation);
    }

    public void UpdateTransformations()
    {
      if (this.m_boneData.Bones == null)
        return;
      MyCharacterBone.ComputeAbsoluteTransforms(this.m_boneData.SortedBones);
    }

    public void UpdateInverseKinematics() => this.m_controller.UpdateInverseKinematics(this.m_boneData.Bones);

    [Conditional("DEBUG")]
    private void CheckAccess()
    {
      int num = this.m_entity.InScene ? 1 : 0;
      if (!Monitor.TryEnter((object) this.m_boneData.Bones))
        return;
      Monitor.Exit((object) this.m_boneData.Bones);
    }

    public MyCharacterBone FindBone(string name, out int index)
    {
      int num = this.m_entity.InScene ? 1 : 0;
      if (name != null)
      {
        MyCharacterBone[] bones = this.m_boneData.Bones;
        for (int index1 = 0; index1 < bones.Length; ++index1)
        {
          if (bones[index1].Name == name)
          {
            index = index1;
            return bones[index1];
          }
        }
      }
      index = -1;
      return (MyCharacterBone) null;
    }

    public void TriggerAction(MyStringId actionName, string[] layers = null)
    {
      int num = this.m_entity.InScene ? 1 : 0;
      if (!this.m_componentValid)
        return;
      if (layers != null)
        this.m_controller.TriggerAction(actionName, layers);
      else
        this.m_controller.TriggerAction(actionName);
      if (this.ActionTriggered == null)
        return;
      this.ActionTriggered(actionName);
    }

    public void Clear()
    {
      int num = this.m_entity.InScene ? 1 : 0;
      this.MarkAsInvalid();
      this.m_controller.InverseKinematics.Clear();
      this.m_controller.DeleteAllLayers();
      this.m_controller.Variables.Clear();
      this.m_controller.ResultBonesPool.Reset((MyCharacterBone[]) null);
    }

    private class CopyOnWriteStorage<T> : IMyVariableStorage<T>, IEnumerable<KeyValuePair<MyStringId, T>>, IEnumerable
    {
      public IMyVariableStorage<T> Read { get; }

      public IMyVariableStorage<T> Write { get; }

      public CopyOnWriteStorage(IMyVariableStorage<T> read, IMyVariableStorage<T> write)
      {
        this.Read = read;
        this.Write = write;
      }

      public void SetValue(MyStringId key, T newValue) => this.Write.SetValue(key, newValue);

      public bool GetValue(MyStringId key, out T value) => this.Write.GetValue(key, out value) || this.Read.GetValue(key, out value);

      public IEnumerator<KeyValuePair<MyStringId, T>> GetEnumerator()
      {
        foreach (KeyValuePair<MyStringId, T> keyValuePair in (IEnumerable<KeyValuePair<MyStringId, T>>) this.Write)
          yield return keyValuePair;
        foreach (KeyValuePair<MyStringId, T> keyValuePair in (IEnumerable<KeyValuePair<MyStringId, T>>) this.Read)
        {
          if (!this.Write.GetValue(keyValuePair.Key, out T _))
            yield return keyValuePair;
        }
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
    }

    private readonly struct BoneDataPack
    {
      public readonly MyCharacterBone[] Bones;
      public readonly MyCharacterBone[] SortedBones;
      public readonly Matrix[] RelativeTransforms;
      public readonly Matrix[] AbsoluteTransforms;

      public int Length => this.Bones.Length;

      public BoneDataPack(
        in MyAnimationControllerComponent.BoneDataPack pack)
      {
        this.Bones = new MyCharacterBone[pack.Length];
        this.SortedBones = new MyCharacterBone[pack.Length];
        this.RelativeTransforms = new Matrix[pack.Length];
        Array.Copy((Array) pack.RelativeTransforms, (Array) this.RelativeTransforms, pack.Length);
        this.AbsoluteTransforms = new Matrix[pack.Length];
        Array.Copy((Array) pack.AbsoluteTransforms, (Array) this.AbsoluteTransforms, pack.Length);
        Dictionary<MyCharacterBone, int> index = new Dictionary<MyCharacterBone, int>();
        for (int index1 = 0; index1 < pack.Length; ++index1)
          index[pack.SortedBones[index1]] = index1;
        MyCharacterBone[] sortedBones = pack.SortedBones;
        MyCharacterBone[] copies = this.SortedBones;
        for (int index1 = 0; index1 < pack.Length; ++index1)
          copies[index1] = new MyCharacterBone(sortedBones[index1].Name, Get(sortedBones[index1].Parent), sortedBones[index1].BindTransform, sortedBones[index1].Index, this.RelativeTransforms, this.AbsoluteTransforms);
        Array.Copy((Array) this.SortedBones, (Array) this.Bones, this.Bones.Length);
        Array.Sort<MyCharacterBone>(this.Bones, (Comparison<MyCharacterBone>) ((x, y) => x.Index.CompareTo(y.Index)));

        MyCharacterBone Get(MyCharacterBone sourceBone)
        {
          int index;
          return sourceBone != null && index.TryGetValue(sourceBone, out index) ? copies[index] : (MyCharacterBone) null;
        }
      }

      public BoneDataPack(
        MyCharacterBone[] bones,
        Matrix[] relativeTransforms,
        Matrix[] absoluteTransforms)
      {
        this.Bones = bones;
        this.RelativeTransforms = relativeTransforms;
        this.AbsoluteTransforms = absoluteTransforms;
        this.SortedBones = new MyCharacterBone[bones.Length];
        Array.Copy((Array) bones, (Array) this.SortedBones, this.SortedBones.Length);
        MyAnimationControllerComponent.BoneDataPack.SortBones(this.SortedBones);
      }

      private static void SortBones(MyCharacterBone[] bones) => Array.Sort<MyCharacterBone>(bones, (Comparison<MyCharacterBone>) ((x, y) => x.Depth.CompareTo(y.Depth)));

      public void CopyMatricesTo(
        in MyAnimationControllerComponent.BoneDataPack pack)
      {
        Array.Copy((Array) this.RelativeTransforms, (Array) pack.RelativeTransforms, pack.Length);
        Array.Copy((Array) this.AbsoluteTransforms, (Array) pack.AbsoluteTransforms, pack.Length);
      }

      public void CopyTransformsTo(
        in MyAnimationControllerComponent.BoneDataPack pack)
      {
        for (int index = 0; index < this.Bones.Length; ++index)
        {
          pack.Bones[index].Rotation = this.Bones[index].Rotation;
          pack.Bones[index].Translation = this.Bones[index].Translation;
          pack.Bones[index].ComputeAbsoluteTransform(false);
        }
      }
    }

    private class VRage_Game_Components_MyAnimationControllerComponent\u003C\u003EActor : IActivator, IActivator<MyAnimationControllerComponent>
    {
      object IActivator.CreateInstance() => (object) new MyAnimationControllerComponent();

      MyAnimationControllerComponent IActivator<MyAnimationControllerComponent>.CreateInstance() => new MyAnimationControllerComponent();
    }
  }
}
