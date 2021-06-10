// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.MyAnimationPlayerBlendPair
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.Definitions.Animation;
using VRage.Game.Models;
using VRageMath;

namespace Sandbox.Game.Entities
{
  internal class MyAnimationPlayerBlendPair
  {
    public AnimationPlayer BlendPlayer = new AnimationPlayer();
    public AnimationPlayer ActualPlayer = new AnimationPlayer();
    private MyAnimationPlayerBlendPair.AnimationBlendState m_state;
    public float m_currentBlendTime;
    public float m_totalBlendTime;
    private string[] m_bones;
    private MySkinnedEntity m_skinnedEntity;
    private string m_name;
    private Dictionary<float, string[]> m_boneLODs;

    public MyAnimationPlayerBlendPair(
      MySkinnedEntity skinnedEntity,
      string[] bones,
      Dictionary<float, string[]> boneLODs,
      string name)
    {
      this.m_bones = bones;
      this.m_skinnedEntity = skinnedEntity;
      this.m_boneLODs = boneLODs;
      this.m_name = name;
    }

    public void UpdateBones(float distance)
    {
      if (this.m_state == MyAnimationPlayerBlendPair.AnimationBlendState.Stopped)
        return;
      if (this.BlendPlayer.IsInitialized)
        this.BlendPlayer.UpdateBones(distance);
      if (!this.ActualPlayer.IsInitialized)
        return;
      this.ActualPlayer.UpdateBones(distance);
    }

    public bool Advance()
    {
      if (this.m_state == MyAnimationPlayerBlendPair.AnimationBlendState.Stopped)
        return false;
      float num = 0.01666667f;
      this.m_currentBlendTime += num * this.ActualPlayer.TimeScale;
      this.ActualPlayer.Advance(num);
      if (!this.ActualPlayer.Looping && this.ActualPlayer.AtEnd && this.m_state == MyAnimationPlayerBlendPair.AnimationBlendState.Playing)
        this.Stop(this.m_totalBlendTime);
      return true;
    }

    public void UpdateAnimationState()
    {
      float num = 0.0f;
      if (this.ActualPlayer.IsInitialized && (double) this.m_currentBlendTime > 0.0)
      {
        num = 1f;
        if ((double) this.m_totalBlendTime > 0.0)
          num = MathHelper.Clamp(this.m_currentBlendTime / this.m_totalBlendTime, 0.0f, 1f);
      }
      if (!this.ActualPlayer.IsInitialized)
        return;
      if (this.m_state == MyAnimationPlayerBlendPair.AnimationBlendState.BlendOut)
      {
        this.ActualPlayer.Weight = 1f - num;
        if ((double) num == 1.0)
        {
          this.ActualPlayer.Done();
          this.m_state = MyAnimationPlayerBlendPair.AnimationBlendState.Stopped;
        }
      }
      if (this.m_state != MyAnimationPlayerBlendPair.AnimationBlendState.BlendIn)
        return;
      if ((double) this.m_totalBlendTime == 0.0)
        num = 1f;
      this.ActualPlayer.Weight = num;
      if (this.BlendPlayer.IsInitialized)
        this.BlendPlayer.Weight = 1f;
      if ((double) num != 1.0)
        return;
      this.m_state = MyAnimationPlayerBlendPair.AnimationBlendState.Playing;
      this.BlendPlayer.Done();
    }

    public void Play(
      MyAnimationDefinition animationDefinition,
      bool firstPerson,
      MyFrameOption frameOption,
      float blendTime,
      float timeScale)
    {
      string str = !firstPerson || string.IsNullOrEmpty(animationDefinition.AnimationModelFPS) ? animationDefinition.AnimationModel : animationDefinition.AnimationModelFPS;
      if (string.IsNullOrEmpty(animationDefinition.AnimationModel))
        return;
      if (animationDefinition.Status == MyAnimationDefinition.AnimationStatus.Unchecked && !MyFileSystem.FileExists(Path.IsPathRooted(str) ? str : Path.Combine(MyFileSystem.ContentPath, str)))
      {
        animationDefinition.Status = MyAnimationDefinition.AnimationStatus.Failed;
      }
      else
      {
        animationDefinition.Status = MyAnimationDefinition.AnimationStatus.OK;
        MyModel onlyAnimationData = MyModels.GetModelOnlyAnimationData(str);
        if (onlyAnimationData != null && onlyAnimationData.Animations == null || (onlyAnimationData.Animations.Clips.Count == 0 || onlyAnimationData.Animations.Clips.Count <= animationDefinition.ClipIndex))
          return;
        if (this.ActualPlayer.IsInitialized)
          this.BlendPlayer.Initialize(this.ActualPlayer);
        this.ActualPlayer.Initialize(onlyAnimationData, this.m_name, animationDefinition.ClipIndex, this.m_skinnedEntity, 1f, timeScale, frameOption, this.m_bones, this.m_boneLODs);
        this.ActualPlayer.AnimationMwmPathDebug = str;
        this.ActualPlayer.AnimationNameDebug = animationDefinition.Id.SubtypeName;
        this.m_state = MyAnimationPlayerBlendPair.AnimationBlendState.BlendIn;
        this.m_currentBlendTime = 0.0f;
        this.m_totalBlendTime = blendTime;
      }
    }

    public void Stop(float blendTime)
    {
      if (this.m_state == MyAnimationPlayerBlendPair.AnimationBlendState.Stopped)
        return;
      this.BlendPlayer.Done();
      this.m_state = MyAnimationPlayerBlendPair.AnimationBlendState.BlendOut;
      this.m_currentBlendTime = 0.0f;
      this.m_totalBlendTime = blendTime;
    }

    public MyAnimationPlayerBlendPair.AnimationBlendState GetState() => this.m_state;

    public void SetBoneLODs(Dictionary<float, string[]> boneLODs) => this.m_boneLODs = boneLODs;

    public enum AnimationBlendState
    {
      Stopped,
      BlendIn,
      Playing,
      BlendOut,
    }
  }
}
