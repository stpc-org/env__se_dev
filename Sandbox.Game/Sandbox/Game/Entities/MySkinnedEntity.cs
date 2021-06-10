// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MySkinnedEntity
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Library.Utils;
using VRage.Network;
using VRageMath;
using VRageRender;
using VRageRender.Animations;
using VRageRender.Import;
using VRageRender.Messages;

namespace Sandbox.Game.Entities
{
  public class MySkinnedEntity : MyEntity, IMySkinnedEntity, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity
  {
    public bool UseNewAnimationSystem;
    private const int MAX_BONE_DECALS_COUNT = 10;
    private MyAnimationControllerComponent m_compAnimationController;
    private Dictionary<int, List<uint>> m_boneDecals = new Dictionary<int, List<uint>>();
    protected ulong m_actualUpdateFrame;
    protected ulong m_actualDrawFrame;
    protected Dictionary<string, Quaternion> m_additionalRotations = new Dictionary<string, Quaternion>();
    private Dictionary<string, MyAnimationPlayerBlendPair> m_animationPlayers = new Dictionary<string, MyAnimationPlayerBlendPair>();
    private Queue<MyAnimationCommand> m_commandQueue = new Queue<MyAnimationCommand>();
    private BoundingBoxD m_actualWorldAABB;
    private BoundingBoxD m_aabb;
    private List<MyAnimationSetData> m_continuingAnimSets = new List<MyAnimationSetData>();

    public MyAnimationControllerComponent AnimationController => this.m_compAnimationController;

    public Matrix[] BoneAbsoluteTransforms => this.m_compAnimationController.BoneAbsoluteTransforms;

    public Matrix[] BoneRelativeTransforms => this.m_compAnimationController.BoneRelativeTransforms;

    public List<MyBoneDecalUpdate> DecalBoneUpdates { get; private set; }

    internal ulong ActualUpdateFrame => this.m_actualUpdateFrame;

    public MySkinnedEntity()
    {
      this.Render = (MyRenderComponentBase) new MyRenderComponentSkinnedEntity();
      this.Render.EnableColorMaskHsv = true;
      this.Render.NeedsDraw = true;
      this.Render.CastShadows = true;
      this.Render.NeedsResolveCastShadow = false;
      this.Render.SkipIfTooSmall = false;
      MyEntityTerrainHeightProviderComponent component = new MyEntityTerrainHeightProviderComponent();
      this.Components.Add<MyEntityTerrainHeightProviderComponent>(component);
      this.m_compAnimationController = new MyAnimationControllerComponent((MyEntity) this, new Action(this.ObtainBones), (IMyTerrainHeightProvider) component);
      this.Components.Add<MyAnimationControllerComponent>(this.m_compAnimationController);
      this.DecalBoneUpdates = new List<MyBoneDecalUpdate>();
    }

    public override void Init(
      StringBuilder displayName,
      string model,
      MyEntity parentObject,
      float? scale,
      string modelCollision = null)
    {
      base.Init(displayName, model, parentObject, scale, modelCollision);
      this.InitBones();
    }

    protected void InitBones()
    {
      this.ObtainBones();
      this.m_animationPlayers.Clear();
      this.AddAnimationPlayer("", (string[]) null);
    }

    public void SetBoneLODs(Dictionary<float, string[]> boneLODs)
    {
      foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in this.m_animationPlayers)
        animationPlayer.Value.SetBoneLODs(boneLODs);
    }

    public virtual void UpdateControl(float distance)
    {
    }

    public virtual void UpdateAnimation(float distance)
    {
      this.m_compAnimationController.CameraDistance = distance;
      if ((!MyPerGameSettings.AnimateOnlyVisibleCharacters || Sandbox.Engine.Platform.Game.IsDedicated || this.Render != null && this.Render.RenderObjectIDs.Length != 0 && (MyRenderProxy.VisibleObjectsRead != null && MyRenderProxy.VisibleObjectsRead.Contains(this.Render.RenderObjectIDs[0]))) && (double) distance < (double) MyFakes.ANIMATION_UPDATE_DISTANCE)
      {
        if (this.UseNewAnimationSystem)
        {
          this.UpdateRenderObject();
        }
        else
        {
          this.UpdateContinuingSets();
          int num1 = this.AdvanceAnimation() ? 1 : 0;
          bool flag = this.ProcessCommands();
          this.UpdateAnimationState();
          int num2 = flag ? 1 : 0;
          if ((num1 | num2) != 0)
          {
            this.CalculateTransforms(distance);
            this.UpdateRenderObject();
          }
        }
      }
      this.UpdateBoneDecals();
    }

    private void UpdateContinuingSets()
    {
      foreach (MyAnimationSetData continuingAnimSet in this.m_continuingAnimSets)
        this.PlayAnimationSet(continuingAnimSet);
    }

    private void UpdateBones(float distance)
    {
      foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in this.m_animationPlayers)
        animationPlayer.Value.UpdateBones(distance);
    }

    private bool AdvanceAnimation()
    {
      bool flag = false;
      foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in this.m_animationPlayers)
        flag = animationPlayer.Value.Advance() | flag;
      return flag;
    }

    private void UpdateAnimationState()
    {
      foreach (KeyValuePair<string, MyAnimationPlayerBlendPair> animationPlayer in this.m_animationPlayers)
        animationPlayer.Value.UpdateAnimationState();
    }

    public virtual void ObtainBones()
    {
      MyCharacterBone[] characterBones = new MyCharacterBone[this.Model.Bones.Length];
      Matrix[] matrixArray1 = new Matrix[this.Model.Bones.Length];
      Matrix[] matrixArray2 = new Matrix[this.Model.Bones.Length];
      for (int index = 0; index < this.Model.Bones.Length; ++index)
      {
        MyModelBone bone = this.Model.Bones[index];
        Matrix transform = bone.Transform;
        MyCharacterBone parent = bone.Parent != -1 ? characterBones[bone.Parent] : (MyCharacterBone) null;
        MyCharacterBone myCharacterBone = new MyCharacterBone(bone.Name, parent, transform, index, matrixArray1, matrixArray2);
        characterBones[index] = myCharacterBone;
      }
      this.m_compAnimationController.SetCharacterBones(characterBones, matrixArray1, matrixArray2);
    }

    public Quaternion GetAdditionalRotation(string bone)
    {
      Quaternion identity = Quaternion.Identity;
      return string.IsNullOrEmpty(bone) || this.m_additionalRotations.TryGetValue(bone, out identity) ? identity : Quaternion.Identity;
    }

    internal void AddAnimationPlayer(string name, string[] bones) => this.m_animationPlayers.Add(name, new MyAnimationPlayerBlendPair(this, bones, (Dictionary<float, string[]>) null, name));

    internal bool TryGetAnimationPlayer(string name, out MyAnimationPlayerBlendPair player)
    {
      if (name == null)
        name = "";
      if (name == "Body")
        name = "";
      return this.m_animationPlayers.TryGetValue(name, out player);
    }

    internal DictionaryReader<string, MyAnimationPlayerBlendPair> GetAllAnimationPlayers() => (DictionaryReader<string, MyAnimationPlayerBlendPair>) this.m_animationPlayers;

    private void PlayAnimationSet(MyAnimationSetData animationSetData)
    {
      if ((double) MyRandom.Instance.NextFloat(0.0f, 1f) >= (double) animationSetData.AnimationSet.Probability)
        return;
      float num1 = ((IEnumerable<AnimationItem>) animationSetData.AnimationSet.AnimationItems).Sum<AnimationItem>((Func<AnimationItem, float>) (x => x.Ratio));
      if ((double) num1 <= 0.0)
        return;
      float num2 = MyRandom.Instance.NextFloat(0.0f, 1f);
      float num3 = 0.0f;
      foreach (AnimationItem animationItem in animationSetData.AnimationSet.AnimationItems)
      {
        num3 += animationItem.Ratio / num1;
        if ((double) num2 < (double) num3)
        {
          MyAnimationCommand command = new MyAnimationCommand()
          {
            AnimationSubtypeName = animationItem.Animation,
            PlaybackCommand = MyPlaybackCommand.Play,
            Area = animationSetData.Area,
            BlendTime = animationSetData.BlendTime,
            TimeScale = 1f,
            KeepContinuingAnimations = true
          };
          this.ProcessCommand(ref command);
          break;
        }
      }
    }

    internal void PlayersPlay(
      string bonesArea,
      MyAnimationDefinition animDefinition,
      bool firstPerson,
      MyFrameOption frameOption,
      float blendTime,
      float timeScale)
    {
      string[] strArray = bonesArea.Split(' ');
      if (animDefinition.AnimationSets != null)
      {
        foreach (AnimationSet animationSet in animDefinition.AnimationSets)
        {
          MyAnimationSetData animationSetData = new MyAnimationSetData()
          {
            BlendTime = blendTime,
            Area = bonesArea,
            AnimationSet = animationSet
          };
          if (animationSet.Continuous)
            this.m_continuingAnimSets.Add(animationSetData);
          else
            this.PlayAnimationSet(animationSetData);
        }
      }
      else
      {
        foreach (string playerName in strArray)
          this.PlayerPlay(playerName, animDefinition, firstPerson, frameOption, blendTime, timeScale);
      }
    }

    internal void PlayerPlay(
      string playerName,
      MyAnimationDefinition animDefinition,
      bool firstPerson,
      MyFrameOption frameOption,
      float blendTime,
      float timeScale)
    {
      MyAnimationPlayerBlendPair player;
      if (!this.TryGetAnimationPlayer(playerName, out player))
        return;
      player.Play(animDefinition, firstPerson, frameOption, blendTime, timeScale);
    }

    internal void PlayerStop(string playerName, float blendTime)
    {
      MyAnimationPlayerBlendPair player;
      if (!this.TryGetAnimationPlayer(playerName, out player))
        return;
      player.Stop(blendTime);
    }

    protected virtual void CalculateTransforms(float distance)
    {
      if (!this.UseNewAnimationSystem)
        this.UpdateBones(distance);
      this.AnimationController.UpdateTransformations();
    }

    [Obsolete]
    protected bool TryGetAnimationDefinition(
      string animationSubtypeName,
      out MyAnimationDefinition animDefinition)
    {
      if (animationSubtypeName == null)
      {
        animDefinition = (MyAnimationDefinition) null;
        return false;
      }
      animDefinition = MyDefinitionManager.Static.TryGetAnimationDefinition(animationSubtypeName);
      if (animDefinition != null)
        return true;
      string path = Path.Combine(MyFileSystem.ContentPath, animationSubtypeName);
      if (MyFileSystem.FileExists(path))
      {
        animDefinition = new MyAnimationDefinition()
        {
          AnimationModel = path,
          ClipIndex = 0
        };
        return true;
      }
      animDefinition = (MyAnimationDefinition) null;
      return false;
    }

    protected bool ProcessCommands()
    {
      if (this.m_commandQueue.Count <= 0)
        return false;
      MyAnimationCommand command = this.m_commandQueue.Dequeue();
      this.ProcessCommand(ref command);
      return true;
    }

    protected void AddBoneDecal(uint decalId, int boneIndex)
    {
      List<uint> uintList;
      if (!this.m_boneDecals.TryGetValue(boneIndex, out uintList))
      {
        uintList = new List<uint>(10);
        this.m_boneDecals.Add(boneIndex, uintList);
      }
      if (uintList.Count == uintList.Capacity)
      {
        MyDecals.RemoveDecal(uintList[0]);
        uintList.RemoveAt(0);
      }
      uintList.Add(decalId);
    }

    private void UpdateBoneDecals()
    {
      this.DecalBoneUpdates.Clear();
      foreach (KeyValuePair<int, List<uint>> boneDecal in this.m_boneDecals)
      {
        foreach (uint num in boneDecal.Value)
          this.DecalBoneUpdates.Add(new MyBoneDecalUpdate()
          {
            BoneID = boneDecal.Key,
            DecalID = num
          });
      }
    }

    protected void FlushAnimationQueue()
    {
      while (this.m_commandQueue.Count > 0)
        this.ProcessCommands();
    }

    private void ProcessCommand(ref MyAnimationCommand command)
    {
      if (command.PlaybackCommand == MyPlaybackCommand.Play)
      {
        MyAnimationDefinition animDefinition;
        if (!this.TryGetAnimationDefinition(command.AnimationSubtypeName, out animDefinition))
          return;
        string bonesArea = animDefinition.InfluenceArea;
        MyFrameOption frameOption = command.FrameOption;
        if (frameOption == MyFrameOption.Default)
          frameOption = animDefinition.Loop ? MyFrameOption.Loop : MyFrameOption.PlayOnce;
        bool useFirstPersonVersion = false;
        this.OnAnimationPlay(animDefinition, command, ref bonesArea, ref frameOption, ref useFirstPersonVersion);
        if (!string.IsNullOrEmpty(command.Area))
          bonesArea = command.Area;
        if (bonesArea == null)
          bonesArea = "";
        if (!command.KeepContinuingAnimations)
          this.m_continuingAnimSets.Clear();
        if (this.UseNewAnimationSystem)
          return;
        this.PlayersPlay(bonesArea, animDefinition, useFirstPersonVersion, frameOption, command.BlendTime, command.TimeScale);
      }
      else
      {
        if (command.PlaybackCommand != MyPlaybackCommand.Stop)
          return;
        string[] strArray = (command.Area == null ? "" : command.Area).Split(' ');
        if (this.UseNewAnimationSystem)
          return;
        foreach (string playerName in strArray)
          this.PlayerStop(playerName, command.BlendTime);
      }
    }

    public virtual void AddCommand(MyAnimationCommand command, bool sync = false) => this.m_commandQueue.Enqueue(command);

    protected virtual void OnAnimationPlay(
      MyAnimationDefinition animDefinition,
      MyAnimationCommand command,
      ref string bonesArea,
      ref MyFrameOption frameOption,
      ref bool useFirstPersonVersion)
    {
    }

    protected void UpdateRenderObject()
    {
    }

    public virtual void UpdateBeforeSimulationParallel()
    {
    }

    public virtual void UpdateAfterSimulationParallel()
    {
      MyAnimationControllerComponent animationController = this.AnimationController;
      float cameraDistance = animationController.CameraDistance;
      this.UpdateControl(animationController.CameraDistance);
      animationController.Update();
      animationController.FinishUpdate();
      this.UpdateAnimation(animationController.CameraDistance);
      this.CalculateTransforms(cameraDistance);
    }

    public MyParallelUpdateFlags UpdateFlags => this.NeedsUpdate.GetParallel() | MyParallelUpdateFlags.EACH_FRAME_PARALLEL;

    private class Sandbox_Game_Entities_MySkinnedEntity\u003C\u003EActor : IActivator, IActivator<MySkinnedEntity>
    {
      object IActivator.CreateInstance() => (object) new MySkinnedEntity();

      MySkinnedEntity IActivator<MySkinnedEntity>.CreateInstance() => new MySkinnedEntity();
    }
  }
}
