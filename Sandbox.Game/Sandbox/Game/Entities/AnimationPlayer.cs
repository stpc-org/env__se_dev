// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.AnimationPlayer
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using System.Collections.Generic;
using System.ComponentModel;
using VRage.Game.Models;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Game.Entities
{
  internal class AnimationPlayer
  {
    public static bool ENABLE_ANIMATION_CACHE = false;
    public static bool ENABLE_ANIMATION_LODS = true;
    private static Dictionary<int, AnimationPlayer> CachedAnimationPlayers = new Dictionary<int, AnimationPlayer>();
    private float m_position;
    private float m_duration;
    private AnimationPlayer.BoneInfo[] m_boneInfos;
    private Dictionary<float, List<AnimationPlayer.BoneInfo>> m_boneLODs = new Dictionary<float, List<AnimationPlayer.BoneInfo>>();
    private int m_boneCount;
    private MySkinnedEntity m_skinnedEntity;
    private MyFrameOption m_frameOption = MyFrameOption.PlayOnce;
    private float m_weight = 1f;
    private float m_timeScale = 1f;
    private bool m_initialized;
    private int m_currentLODIndex;
    private List<AnimationPlayer.BoneInfo> m_currentLOD;
    private int m_hash;
    public string AnimationMwmPathDebug;
    public string AnimationNameDebug;
    private bool t = true;

    private static int GetAnimationPlayerHash(AnimationPlayer player)
    {
      float num = 10f;
      return player.Name.GetHashCode() * 397 ^ MyUtils.GetHash((double) player.m_skinnedEntity.Model.UniqueId) * 397 ^ (int) ((double) player.m_position.GetHashCode() * (double) num) * 397 ^ player.m_currentLODIndex.GetHashCode();
    }

    public MyStringId Name { get; private set; }

    public void Advance(float value)
    {
      if (this.m_frameOption != MyFrameOption.JustFirstFrame)
      {
        this.Position += value * this.m_timeScale;
        if (this.m_frameOption != MyFrameOption.StayOnLastFrame || (double) this.Position <= (double) this.m_duration)
          return;
        this.Position = this.m_duration;
      }
      else
        this.Position = 0.0f;
    }

    [Browsable(false)]
    public float Position
    {
      get => this.m_position;
      set
      {
        float num = value;
        if ((double) num > (double) this.m_duration)
        {
          if (this.Looping)
            num -= this.m_duration;
          else
            value = this.m_duration;
        }
        this.m_position = num;
      }
    }

    public float Weight
    {
      get => this.m_weight;
      set => this.m_weight = value;
    }

    public float TimeScale
    {
      get => this.m_timeScale;
      set => this.m_timeScale = value;
    }

    public void UpdateBones(float distance)
    {
      if (!AnimationPlayer.ENABLE_ANIMATION_LODS)
      {
        for (int index = 0; index < this.m_boneCount; ++index)
          this.m_boneInfos[index].SetPosition(this.m_position);
      }
      else
      {
        this.m_currentLODIndex = -1;
        this.m_currentLOD = (List<AnimationPlayer.BoneInfo>) null;
        int num = 0;
        List<AnimationPlayer.BoneInfo> boneInfoList = (List<AnimationPlayer.BoneInfo>) null;
        foreach (KeyValuePair<float, List<AnimationPlayer.BoneInfo>> boneLoD in this.m_boneLODs)
        {
          if ((double) distance > (double) boneLoD.Key)
          {
            boneInfoList = boneLoD.Value;
            this.m_currentLODIndex = num;
            this.m_currentLOD = boneInfoList;
            ++num;
          }
          else
            break;
        }
        if (boneInfoList == null)
          return;
        AnimationPlayer player;
        if (AnimationPlayer.CachedAnimationPlayers.TryGetValue(this.m_hash, out player) && player == this)
          AnimationPlayer.CachedAnimationPlayers.Remove(this.m_hash);
        this.m_hash = AnimationPlayer.GetAnimationPlayerHash(this);
        if (AnimationPlayer.CachedAnimationPlayers.TryGetValue(this.m_hash, out player) && this.m_hash != AnimationPlayer.GetAnimationPlayerHash(player))
        {
          AnimationPlayer.CachedAnimationPlayers.Remove(this.m_hash);
          player = (AnimationPlayer) null;
        }
        if (player != null)
        {
          for (int index = 0; index < boneInfoList.Count; ++index)
          {
            boneInfoList[index].Translation = player.m_currentLOD[index].Translation;
            boneInfoList[index].Rotation = player.m_currentLOD[index].Rotation;
            boneInfoList[index].AssignToCharacterBone();
          }
        }
        else
        {
          if (boneInfoList.Count <= 0)
            return;
          if (AnimationPlayer.ENABLE_ANIMATION_CACHE)
            AnimationPlayer.CachedAnimationPlayers[this.m_hash] = this;
          foreach (AnimationPlayer.BoneInfo boneInfo in boneInfoList)
            boneInfo.SetPosition(this.m_position);
        }
      }
    }

    public bool Looping => this.m_frameOption == MyFrameOption.Loop;

    public bool AtEnd => (double) this.Position >= (double) this.m_duration && this.m_frameOption != MyFrameOption.StayOnLastFrame;

    public void Initialize(AnimationPlayer player)
    {
      if (this.m_hash != 0)
      {
        AnimationPlayer.CachedAnimationPlayers.Remove(this.m_hash);
        this.m_hash = 0;
      }
      this.Name = player.Name;
      this.m_duration = player.m_duration;
      this.m_skinnedEntity = player.m_skinnedEntity;
      this.m_weight = player.Weight;
      this.m_timeScale = player.m_timeScale;
      this.m_frameOption = player.m_frameOption;
      foreach (List<AnimationPlayer.BoneInfo> boneInfoList in this.m_boneLODs.Values)
        boneInfoList.Clear();
      this.m_boneCount = player.m_boneCount;
      if (this.m_boneInfos == null || this.m_boneInfos.Length < this.m_boneCount)
        this.m_boneInfos = new AnimationPlayer.BoneInfo[this.m_boneCount];
      this.Position = player.Position;
      for (int index = 0; index < this.m_boneCount; ++index)
      {
        AnimationPlayer.BoneInfo boneInfo1 = this.m_boneInfos[index];
        if (boneInfo1 == null)
        {
          boneInfo1 = new AnimationPlayer.BoneInfo();
          this.m_boneInfos[index] = boneInfo1;
        }
        boneInfo1.ClipBone = player.m_boneInfos[index].ClipBone;
        boneInfo1.Player = this;
        boneInfo1.SetModel(this.m_skinnedEntity);
        boneInfo1.CurrentKeyframe = player.m_boneInfos[index].CurrentKeyframe;
        boneInfo1.SetPosition(this.Position);
        if (player.m_boneLODs != null && boneInfo1.ModelBone != null && AnimationPlayer.ENABLE_ANIMATION_LODS)
        {
          foreach (KeyValuePair<float, List<AnimationPlayer.BoneInfo>> boneLoD in player.m_boneLODs)
          {
            List<AnimationPlayer.BoneInfo> boneInfoList;
            if (!this.m_boneLODs.TryGetValue(boneLoD.Key, out boneInfoList))
            {
              boneInfoList = new List<AnimationPlayer.BoneInfo>();
              this.m_boneLODs.Add(boneLoD.Key, boneInfoList);
            }
            foreach (AnimationPlayer.BoneInfo boneInfo2 in boneLoD.Value)
            {
              if (boneInfo2.ModelBone != null && boneInfo1.ModelBone.Name == boneInfo2.ModelBone.Name)
              {
                boneInfoList.Add(boneInfo1);
                break;
              }
            }
          }
        }
        int num = MyFakes.ENABLE_BONES_AND_ANIMATIONS_DEBUG ? 1 : 0;
      }
      this.m_initialized = true;
    }

    public void Initialize(
      MyModel animationModel,
      string playerName,
      int clipIndex,
      MySkinnedEntity skinnedEntity,
      float weight,
      float timeScale,
      MyFrameOption frameOption,
      string[] explicitBones = null,
      Dictionary<float, string[]> boneLODs = null)
    {
      if (this.m_hash != 0)
      {
        AnimationPlayer.CachedAnimationPlayers.Remove(this.m_hash);
        this.m_hash = 0;
      }
      MyAnimationClip clip = animationModel.Animations.Clips[clipIndex];
      this.Name = MyStringId.GetOrCompute(animationModel.AssetName + " : " + playerName);
      this.m_duration = (float) clip.Duration;
      this.m_skinnedEntity = skinnedEntity;
      this.m_weight = weight;
      this.m_timeScale = timeScale;
      this.m_frameOption = frameOption;
      foreach (List<AnimationPlayer.BoneInfo> boneInfoList in this.m_boneLODs.Values)
        boneInfoList.Clear();
      List<AnimationPlayer.BoneInfo> boneInfoList1;
      if (!this.m_boneLODs.TryGetValue(0.0f, out boneInfoList1))
      {
        boneInfoList1 = new List<AnimationPlayer.BoneInfo>();
        this.m_boneLODs.Add(0.0f, boneInfoList1);
      }
      int length = explicitBones == null ? clip.Bones.Count : explicitBones.Length;
      if (this.m_boneInfos == null || this.m_boneInfos.Length < length)
        this.m_boneInfos = new AnimationPlayer.BoneInfo[length];
      int index1 = 0;
      for (int index2 = 0; index2 < length; ++index2)
      {
        MyAnimationClip.Bone bone = explicitBones == null ? clip.Bones[index2] : this.FindBone(clip.Bones, explicitBones[index2]);
        if (bone != null && bone.Keyframes.Count != 0)
        {
          AnimationPlayer.BoneInfo boneInfo = this.m_boneInfos[index1];
          if (this.m_boneInfos[index1] == null)
          {
            boneInfo = new AnimationPlayer.BoneInfo(bone, this);
          }
          else
          {
            boneInfo.Clear();
            boneInfo.Init(bone, this);
          }
          this.m_boneInfos[index1] = boneInfo;
          this.m_boneInfos[index1].SetModel(skinnedEntity);
          if (boneInfo.ModelBone != null)
          {
            boneInfoList1.Add(boneInfo);
            if (boneLODs != null)
            {
              foreach (KeyValuePair<float, string[]> boneLoD in boneLODs)
              {
                List<AnimationPlayer.BoneInfo> boneInfoList2;
                if (!this.m_boneLODs.TryGetValue(boneLoD.Key, out boneInfoList2))
                {
                  boneInfoList2 = new List<AnimationPlayer.BoneInfo>();
                  this.m_boneLODs.Add(boneLoD.Key, boneInfoList2);
                }
                foreach (string str in boneLoD.Value)
                {
                  if (boneInfo.ModelBone.Name == str)
                  {
                    boneInfoList2.Add(boneInfo);
                    break;
                  }
                }
              }
            }
          }
          ++index1;
        }
      }
      this.m_boneCount = index1;
      this.Position = 0.0f;
      this.m_initialized = true;
    }

    public void Done()
    {
      this.m_initialized = false;
      AnimationPlayer.CachedAnimationPlayers.Remove(this.m_hash);
    }

    public bool IsInitialized => this.m_initialized;

    private MyAnimationClip.Bone FindBone(
      List<MyAnimationClip.Bone> bones,
      string name)
    {
      foreach (MyAnimationClip.Bone bone in bones)
      {
        if (bone.Name == name)
          return bone;
      }
      return (MyAnimationClip.Bone) null;
    }

    private class BoneInfo
    {
      private int m_currentKeyframe;
      private bool m_isConst;
      private MyCharacterBone m_assignedBone;
      public Quaternion Rotation;
      public Vector3 Translation;
      public AnimationPlayer Player;
      public MyAnimationClip.Keyframe Keyframe1;
      public MyAnimationClip.Keyframe Keyframe2;
      private MyAnimationClip.Bone m_clipBone;

      public int CurrentKeyframe
      {
        get => this.m_currentKeyframe;
        set
        {
          this.m_currentKeyframe = value;
          this.SetKeyframes();
        }
      }

      public MyAnimationClip.Bone ClipBone
      {
        get => this.m_clipBone;
        set => this.m_clipBone = value;
      }

      public MyCharacterBone ModelBone => this.m_assignedBone;

      public BoneInfo()
      {
      }

      public BoneInfo(MyAnimationClip.Bone bone, AnimationPlayer player) => this.Init(bone, player);

      public void Init(MyAnimationClip.Bone bone, AnimationPlayer player)
      {
        this.ClipBone = bone;
        this.Player = player;
        this.SetKeyframes();
        this.SetPosition(0.0f);
        this.m_isConst = this.ClipBone.Keyframes.Count == 1;
      }

      public void Clear()
      {
        this.m_currentKeyframe = 0;
        this.m_isConst = false;
        this.m_assignedBone = (MyCharacterBone) null;
        this.Rotation = new Quaternion();
        this.Translation = Vector3.Zero;
        this.Player = (AnimationPlayer) null;
        this.Keyframe1 = (MyAnimationClip.Keyframe) null;
        this.Keyframe2 = (MyAnimationClip.Keyframe) null;
        this.m_clipBone = (MyAnimationClip.Bone) null;
      }

      public void SetPosition(float position)
      {
        if (this.ClipBone == null)
          return;
        List<MyAnimationClip.Keyframe> keyframes = this.ClipBone.Keyframes;
        if (keyframes == null || this.Keyframe1 == null || (this.Keyframe2 == null || keyframes.Count == 0))
          return;
        if (!this.m_isConst)
        {
          while ((double) position < this.Keyframe1.Time && this.m_currentKeyframe > 0)
          {
            --this.m_currentKeyframe;
            this.SetKeyframes();
          }
          while ((double) position >= this.Keyframe2.Time && this.m_currentKeyframe < this.ClipBone.Keyframes.Count - 2)
          {
            ++this.m_currentKeyframe;
            this.SetKeyframes();
          }
          if (this.Keyframe1 == this.Keyframe2)
          {
            this.Rotation = this.Keyframe1.Rotation;
            this.Translation = this.Keyframe1.Translation;
          }
          else
          {
            float amount = MathHelper.Clamp((float) (((double) position - this.Keyframe1.Time) * this.Keyframe2.InvTimeDiff), 0.0f, 1f);
            Quaternion.Slerp(ref this.Keyframe1.Rotation, ref this.Keyframe2.Rotation, amount, out this.Rotation);
            Vector3.Lerp(ref this.Keyframe1.Translation, ref this.Keyframe2.Translation, amount, out this.Translation);
          }
        }
        this.AssignToCharacterBone();
      }

      public void AssignToCharacterBone()
      {
        if (this.m_assignedBone == null)
          return;
        Quaternion rotation1 = this.Rotation;
        Quaternion rotation2 = this.Rotation * this.Player.m_skinnedEntity.GetAdditionalRotation(this.m_assignedBone.Name);
        this.m_assignedBone.SetCompleteTransform(ref this.Translation, ref rotation2, this.Player.Weight);
      }

      private void SetKeyframes()
      {
        if (this.ClipBone == null)
          return;
        if (this.ClipBone.Keyframes.Count > 0)
        {
          this.Keyframe1 = this.ClipBone.Keyframes[this.m_currentKeyframe];
          if (this.m_currentKeyframe == this.ClipBone.Keyframes.Count - 1)
            this.Keyframe2 = this.Keyframe1;
          else
            this.Keyframe2 = this.ClipBone.Keyframes[this.m_currentKeyframe + 1];
        }
        else
        {
          this.Keyframe1 = (MyAnimationClip.Keyframe) null;
          this.Keyframe2 = (MyAnimationClip.Keyframe) null;
        }
      }

      public void SetModel(MySkinnedEntity skinnedEntity)
      {
        if (this.ClipBone == null)
          return;
        this.m_assignedBone = skinnedEntity.AnimationController.FindBone(this.ClipBone.Name, out int _);
      }
    }
  }
}
