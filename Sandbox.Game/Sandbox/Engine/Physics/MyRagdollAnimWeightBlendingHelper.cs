// Decompiled with JetBrains decompiler
// Type: Sandbox.Engine.Physics.MyRagdollAnimWeightBlendingHelper
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;
using VRageRender.Animations;

namespace Sandbox.Engine.Physics
{
  public class MyRagdollAnimWeightBlendingHelper
  {
    private const string RAGDOLL_WEIGHT_VARIABLE_PREFIX = "rd_weight_";
    private const string RAGDOLL_BLEND_TIME_VARIABLE_PREFIX = "rd_blend_time_";
    private const string RAGDOLL_DEFAULT_BLEND_TIME_VARIABLE_NAME = "rd_default_blend_time";
    private const float DEFAULT_BLEND_TIME = 2.5f;
    private static readonly MyGameTimer TIMER = new MyGameTimer();
    private MyRagdollAnimWeightBlendingHelper.BoneData[] m_boneIndexToData;
    private float m_defaultBlendTime = 0.8f;
    private MyStringId m_defautlBlendTimeId;

    public bool Initialized { get; private set; }

    public bool Init(MyCharacterBone[] bones, MyAnimationController controller)
    {
      List<MyAnimationStateMachine> source = new List<MyAnimationStateMachine>(controller.GetLayerCount());
      for (int index = 0; index < controller.GetLayerCount(); ++index)
      {
        MyAnimationStateMachine layerByIndex = controller.GetLayerByIndex(index);
        if (layerByIndex.BoneMask == null)
          return false;
        source.Add(layerByIndex);
      }
      this.m_boneIndexToData = new MyRagdollAnimWeightBlendingHelper.BoneData[bones.Length];
      foreach (MyCharacterBone bone1 in bones)
      {
        MyCharacterBone bone = bone1;
        this.m_boneIndexToData[bone.Index] = new MyRagdollAnimWeightBlendingHelper.BoneData()
        {
          WeightId = MyStringId.GetOrCompute("rd_weight_" + bone.Name),
          BlendTimeId = MyStringId.GetOrCompute("rd_blend_time_" + bone.Name),
          BlendTimeMs = -1.0,
          StartingWeight = 0.0f,
          TargetWeight = 0.0f,
          PrevWeight = 0.0f,
          Layers = source.Where<MyAnimationStateMachine>((Func<MyAnimationStateMachine, bool>) (layer => layer.BoneMask[bone.Index])).Select<MyAnimationStateMachine, MyRagdollAnimWeightBlendingHelper.LayerData>((Func<MyAnimationStateMachine, MyRagdollAnimWeightBlendingHelper.LayerData>) (layer => new MyRagdollAnimWeightBlendingHelper.LayerData()
          {
            LayerId = MyStringId.GetOrCompute("rd_weight_" + layer.Name),
            LayerBlendTimeId = MyStringId.GetOrCompute("rd_blend_time_" + layer.Name)
          })).ToArray<MyRagdollAnimWeightBlendingHelper.LayerData>()
        };
      }
      this.m_defautlBlendTimeId = MyStringId.GetOrCompute("rd_default_blend_time");
      this.Initialized = true;
      return true;
    }

    public void Prepare(IMyVariableStorage<float> controllerVariables)
    {
      if (controllerVariables.GetValue(this.m_defautlBlendTimeId, out this.m_defaultBlendTime))
        return;
      this.m_defaultBlendTime = 2.5f;
    }

    public void BlendWeight(
      ref float weight,
      MyCharacterBone bone,
      MyAnimationVariableStorage controllerVariables)
    {
      if (this.m_boneIndexToData.Length <= bone.Index)
        return;
      ref MyRagdollAnimWeightBlendingHelper.BoneData local = ref this.m_boneIndexToData[bone.Index];
      float num1;
      if (!controllerVariables.GetValue(local.WeightId, out num1) || (double) num1 < 0.0)
        num1 = -1f;
      float num2;
      if (!controllerVariables.GetValue(local.BlendTimeId, out num2) || (double) num2 < 0.0)
        num2 = -1f;
      if ((double) num1 < 0.0 || (double) num2 < 0.0)
      {
        float val1_1 = float.MaxValue;
        float val1_2 = float.MaxValue;
        foreach (MyRagdollAnimWeightBlendingHelper.LayerData layer in local.Layers)
        {
          float val2_1;
          if (controllerVariables.GetValue(layer.LayerId, out val2_1))
            val1_1 = Math.Min(val1_1, val2_1);
          float val2_2;
          if (controllerVariables.GetValue(layer.LayerBlendTimeId, out val2_2))
            val1_2 = Math.Min(val1_2, val2_2);
        }
        if ((double) num1 < 0.0)
        {
          if ((double) val1_1 == 3.40282346638529E+38)
            return;
          num1 = val1_1;
        }
        if ((double) num2 < 0.0)
          num2 = (double) val1_2 == 3.40282346638529E+38 ? this.m_defaultBlendTime : val1_2;
      }
      double totalMilliseconds = MyRagdollAnimWeightBlendingHelper.TIMER.ElapsedTimeSpan.TotalMilliseconds;
      local.BlendTimeMs = (double) num2 * 1000.0;
      if ((double) num1 != (double) local.TargetWeight)
      {
        local.StartedMs = totalMilliseconds;
        local.StartingWeight = (double) local.PrevWeight == -1.0 ? weight : local.PrevWeight;
        local.TargetWeight = num1;
      }
      double amount = MathHelper.Clamp((totalMilliseconds - local.StartedMs) / local.BlendTimeMs, 0.0, 1.0);
      weight = (float) MathHelper.Lerp((double) local.StartingWeight, (double) local.TargetWeight, amount);
      local.PrevWeight = weight;
    }

    public void ResetWeights()
    {
      if (this.m_boneIndexToData == null)
        return;
      for (int index = 0; index < this.m_boneIndexToData.Length; ++index)
      {
        this.m_boneIndexToData[index].PrevWeight = 0.0f;
        this.m_boneIndexToData[index].TargetWeight = 0.0f;
      }
    }

    private struct BoneData
    {
      public MyStringId WeightId;
      public MyStringId BlendTimeId;
      public double BlendTimeMs;
      public double StartedMs;
      public float StartingWeight;
      public float TargetWeight;
      public float PrevWeight;
      public MyRagdollAnimWeightBlendingHelper.LayerData[] Layers;
    }

    private struct LayerData
    {
      public MyStringId LayerId;
      public MyStringId LayerBlendTimeId;
    }
  }
}
