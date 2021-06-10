// Decompiled with JetBrains decompiler
// Type: VRage.Game.Components.MyAnimationControllerComponentLoadFromDef
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VRage.Game.Definitions.Animation;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Game.ObjectBuilders.Animation;
using VRage.Generics;
using VRage.Generics.StateMachine;
using VRage.Library.Utils;
using VRage.Utils;
using VRageRender.Animations;

namespace VRage.Game.Components
{
  public static class MyAnimationControllerComponentLoadFromDef
  {
    private static readonly char[] m_boneListSeparators = new char[1]
    {
      ' '
    };

    public static bool InitFromDefinition(
      this MyAnimationControllerComponent thisController,
      MyAnimationControllerDefinition animControllerDefinition,
      bool forceReloadMwm = false)
    {
      bool flag = true;
      thisController.Clear();
      thisController.SourceId = animControllerDefinition.Id;
      foreach (MyObjectBuilder_AnimationLayer layer1 in animControllerDefinition.Layers)
      {
        MyAnimationStateMachine layer2 = thisController.Controller.CreateLayer(layer1.Name);
        if (layer2 != null)
        {
          switch (layer1.Mode)
          {
            case MyObjectBuilder_AnimationLayer.MyLayerMode.Replace:
              layer2.Mode = MyAnimationStateMachine.MyBlendingMode.Replace;
              break;
            case MyObjectBuilder_AnimationLayer.MyLayerMode.Add:
              layer2.Mode = MyAnimationStateMachine.MyBlendingMode.Add;
              break;
            default:
              layer2.Mode = MyAnimationStateMachine.MyBlendingMode.Replace;
              break;
          }
          if (layer1.BoneMask != null)
          {
            foreach (string str in layer1.BoneMask.Split(MyAnimationControllerComponentLoadFromDef.m_boneListSeparators))
              layer2.BoneMaskStrIds.Add(MyStringId.GetOrCompute(str));
          }
          else
            layer2.BoneMaskStrIds.Clear();
          layer2.BoneMask = (bool[]) null;
          MyAnimationControllerComponentLoadFromDef.MyAnimationVirtualNodes virtualNodes = new MyAnimationControllerComponentLoadFromDef.MyAnimationVirtualNodes();
          flag = MyAnimationControllerComponentLoadFromDef.InitLayerNodes(layer2, layer1.StateMachine, animControllerDefinition, thisController.Controller, layer2.Name + "/", virtualNodes, forceReloadMwm) & flag;
          layer2.SetState(layer2.Name + "/" + layer1.InitialSMNode);
          if (layer2.ActiveCursors.Count > 0 && layer2.ActiveCursors[0].Node != null && layer2.ActiveCursors[0].Node is MyAnimationStateMachineNode node)
          {
            foreach (MyAnimationStateMachineNode.VarAssignmentData variableAssignment in node.VariableAssignments)
              thisController.Controller.Variables.SetValue(variableAssignment.VariableId, variableAssignment.Value);
          }
          layer2.SortTransitions();
        }
      }
      foreach (MyObjectBuilder_AnimationFootIkChain footIkChain in animControllerDefinition.FootIkChains)
        thisController.InverseKinematics.RegisterFootBone(footIkChain.FootBone, footIkChain.ChainLength, footIkChain.AlignBoneWithTerrain);
      foreach (string ikIgnoredBone in animControllerDefinition.IkIgnoredBones)
        thisController.InverseKinematics.RegisterIgnoredBone(ikIgnoredBone);
      if (flag)
        thisController.MarkAsValid();
      return flag;
    }

    private static bool InitLayerNodes(
      MyAnimationStateMachine layer,
      string stateMachineName,
      MyAnimationControllerDefinition animControllerDefinition,
      MyAnimationController animationController,
      string currentNodeNamePrefix,
      MyAnimationControllerComponentLoadFromDef.MyAnimationVirtualNodes virtualNodes,
      bool forceReloadMwm)
    {
      MyObjectBuilder_AnimationSM builderAnimationSm = animControllerDefinition.StateMachines.FirstOrDefault<MyObjectBuilder_AnimationSM>((Func<MyObjectBuilder_AnimationSM, bool>) (x => x.Name == stateMachineName));
      if (builderAnimationSm == null)
        return false;
      bool flag = true;
      if (builderAnimationSm.Nodes != null)
      {
        foreach (MyObjectBuilder_AnimationSMNode node in builderAnimationSm.Nodes)
        {
          string str = currentNodeNamePrefix + node.Name;
          if (node.StateMachineName != null)
          {
            if (!MyAnimationControllerComponentLoadFromDef.InitLayerNodes(layer, node.StateMachineName, animControllerDefinition, animationController, str + "/", virtualNodes, forceReloadMwm))
              flag = false;
          }
          else
          {
            MyAnimationStateMachineNode stateMachineNode = new MyAnimationStateMachineNode(str);
            if (node.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.PassThrough || node.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.Any || node.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.AnyExceptTarget)
              stateMachineNode.PassThrough = true;
            else
              stateMachineNode.PassThrough = false;
            if (node.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.Any || node.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.AnyExceptTarget)
              virtualNodes.NodesAny.Add(str, new MyAnimationControllerComponentLoadFromDef.MyAnimationVirtualNodeData()
              {
                AnyNodePrefix = currentNodeNamePrefix,
                ExceptTarget = node.Type == MyObjectBuilder_AnimationSMNode.MySMNodeType.AnyExceptTarget
              });
            layer.AddNode((MyStateMachineNode) stateMachineNode);
            if (node.AnimationTree != null)
            {
              node.Name.Contains("State");
              MyAnimationTreeNode animationTreeNode = MyAnimationControllerComponentLoadFromDef.InitNodeAnimationTree(animationController, node.AnimationTree.Child, forceReloadMwm);
              stateMachineNode.RootAnimationNode = animationTreeNode;
            }
            else
              stateMachineNode.RootAnimationNode = (MyAnimationTreeNode) new MyAnimationTreeNodeDummy();
            if (node.Variables != null)
              stateMachineNode.VariableAssignments = new List<MyAnimationStateMachineNode.VarAssignmentData>(node.Variables.Select<MyObjectBuilder_AnimationSMVariable, MyAnimationStateMachineNode.VarAssignmentData>((Func<MyObjectBuilder_AnimationSMVariable, MyAnimationStateMachineNode.VarAssignmentData>) (builder => new MyAnimationStateMachineNode.VarAssignmentData()
              {
                VariableId = MyStringId.GetOrCompute(builder.Name),
                Value = builder.Value
              })));
          }
        }
      }
      if (builderAnimationSm.Transitions != null)
      {
        foreach (MyObjectBuilder_AnimationSMTransition transition in builderAnimationSm.Transitions)
        {
          string str = currentNodeNamePrefix + transition.From;
          string absoluteNameNodeTo = currentNodeNamePrefix + transition.To;
          MyAnimationControllerComponentLoadFromDef.MyAnimationVirtualNodeData animationVirtualNodeData;
          if (virtualNodes.NodesAny.TryGetValue(str, out animationVirtualNodeData))
          {
            foreach (KeyValuePair<string, MyStateMachineNode> allNode in layer.AllNodes)
            {
              if (allNode.Key.StartsWith(animationVirtualNodeData.AnyNodePrefix) && allNode.Key != str && (!animationVirtualNodeData.ExceptTarget || absoluteNameNodeTo != allNode.Key))
                MyAnimationControllerComponentLoadFromDef.CreateTransition(layer, animationController, allNode.Key, absoluteNameNodeTo, transition);
            }
          }
          MyAnimationControllerComponentLoadFromDef.CreateTransition(layer, animationController, str, absoluteNameNodeTo, transition);
        }
      }
      return flag;
    }

    private static void CreateTransition(
      MyAnimationStateMachine layer,
      MyAnimationController animationController,
      string absoluteNameNodeFrom,
      string absoluteNameNodeTo,
      MyObjectBuilder_AnimationSMTransition objBuilderTransition)
    {
      int index = 0;
      do
      {
        if (layer.AddTransition(absoluteNameNodeFrom, absoluteNameNodeTo, (MyStateMachineTransition) new MyAnimationStateMachineTransition()) is MyAnimationStateMachineTransition machineTransition)
        {
          machineTransition.Name = MyStringId.GetOrCompute(objBuilderTransition.Name != null ? objBuilderTransition.Name.ToLower() : (string) null);
          machineTransition.TransitionTimeInSec = objBuilderTransition.TimeInSec;
          machineTransition.Sync = objBuilderTransition.Sync;
          machineTransition.Curve = objBuilderTransition.Curve;
          machineTransition.Priority = objBuilderTransition.Priority;
          if (objBuilderTransition.Conditions != null && objBuilderTransition.Conditions[index] != null)
          {
            foreach (MyObjectBuilder_AnimationSMCondition condition in objBuilderTransition.Conditions[index].Conditions)
            {
              MyCondition<float> oneCondition = MyAnimationControllerComponentLoadFromDef.ParseOneCondition(animationController, condition);
              if (oneCondition != null)
                machineTransition.Conditions.Add((IMyCondition) oneCondition);
            }
          }
        }
        ++index;
      }
      while (objBuilderTransition.Conditions != null && index < objBuilderTransition.Conditions.Length);
    }

    private static MyCondition<float> ParseOneCondition(
      MyAnimationController animationController,
      MyObjectBuilder_AnimationSMCondition objBuilderCondition)
    {
      objBuilderCondition.ValueLeft = objBuilderCondition.ValueLeft != null ? objBuilderCondition.ValueLeft.ToLower() : "0";
      objBuilderCondition.ValueRight = objBuilderCondition.ValueRight != null ? objBuilderCondition.ValueRight.ToLower() : "0";
      double result1;
      MyCondition<float> myCondition;
      if (double.TryParse(objBuilderCondition.ValueLeft, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
      {
        double result2;
        if (double.TryParse(objBuilderCondition.ValueRight, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
        {
          myCondition = new MyCondition<float>((IMyVariableStorage<float>) animationController.Variables, MyAnimationControllerComponentLoadFromDef.ConvertOperation(objBuilderCondition.Operation), (float) result1, (float) result2);
        }
        else
        {
          myCondition = new MyCondition<float>((IMyVariableStorage<float>) animationController.Variables, MyAnimationControllerComponentLoadFromDef.ConvertOperation(objBuilderCondition.Operation), (float) result1, objBuilderCondition.ValueRight);
          MyStringId orCompute = MyStringId.GetOrCompute(objBuilderCondition.ValueRight);
          if (!animationController.Variables.AllVariables.ContainsKey(orCompute))
            animationController.Variables.SetValue(orCompute, 0.0f);
        }
      }
      else
      {
        double result2;
        if (double.TryParse(objBuilderCondition.ValueRight, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
        {
          myCondition = new MyCondition<float>((IMyVariableStorage<float>) animationController.Variables, MyAnimationControllerComponentLoadFromDef.ConvertOperation(objBuilderCondition.Operation), objBuilderCondition.ValueLeft, (float) result2);
          MyStringId orCompute = MyStringId.GetOrCompute(objBuilderCondition.ValueLeft);
          if (!animationController.Variables.AllVariables.ContainsKey(orCompute))
            animationController.Variables.SetValue(orCompute, 0.0f);
        }
        else
        {
          myCondition = new MyCondition<float>((IMyVariableStorage<float>) animationController.Variables, MyAnimationControllerComponentLoadFromDef.ConvertOperation(objBuilderCondition.Operation), objBuilderCondition.ValueLeft, objBuilderCondition.ValueRight);
          MyStringId orCompute1 = MyStringId.GetOrCompute(objBuilderCondition.ValueLeft);
          MyStringId orCompute2 = MyStringId.GetOrCompute(objBuilderCondition.ValueRight);
          if (!animationController.Variables.AllVariables.ContainsKey(orCompute1))
            animationController.Variables.SetValue(orCompute1, 0.0f);
          if (!animationController.Variables.AllVariables.ContainsKey(orCompute2))
            animationController.Variables.SetValue(orCompute2, 0.0f);
        }
      }
      return myCondition;
    }

    private static MyAnimationTreeNode InitNodeAnimationTree(
      MyAnimationController controller,
      MyObjectBuilder_AnimationTreeNode objBuilderNode,
      bool forceReloadMwm)
    {
      if (objBuilderNode is MyObjectBuilder_AnimationTreeNodeDynamicTrack nodeDynamicTrack)
      {
        MyAnimationTreeNodeDynamicTrack nodeDynamicTrack = new MyAnimationTreeNodeDynamicTrack();
        nodeDynamicTrack.Loop = nodeDynamicTrack.Loop;
        nodeDynamicTrack.Speed = nodeDynamicTrack.Speed;
        nodeDynamicTrack.DefaultAnimation = MyStringId.GetOrCompute(nodeDynamicTrack.DefaultAnimation);
        nodeDynamicTrack.Interpolate = nodeDynamicTrack.Interpolate;
        nodeDynamicTrack.SynchronizeWithLayer = nodeDynamicTrack.SynchronizeWithLayer;
        nodeDynamicTrack.Key = nodeDynamicTrack.Key;
        if (!string.IsNullOrEmpty(nodeDynamicTrack.Key))
          controller.RegisterKeyedTrack((MyAnimationTreeNode) nodeDynamicTrack);
        return (MyAnimationTreeNode) nodeDynamicTrack;
      }
      MyObjectBuilder_AnimationTreeNodeTrack objBuilderNodeTrack = objBuilderNode as MyObjectBuilder_AnimationTreeNodeTrack;
      if (objBuilderNodeTrack != null)
      {
        MyAnimationTreeNodeTrack animationTreeNodeTrack = new MyAnimationTreeNodeTrack();
        MyModel myModel = objBuilderNodeTrack.PathToModel != null ? MyModels.GetModelOnlyAnimationData(objBuilderNodeTrack.PathToModel, forceReloadMwm) : (MyModel) null;
        if (myModel != null && myModel.Animations != null && (myModel.Animations.Clips != null && myModel.Animations.Clips.Count > 0))
        {
          MyAnimationClip animationClip = myModel.Animations.Clips.FirstOrDefault<MyAnimationClip>((Func<MyAnimationClip, bool>) (clipItem => clipItem.Name == objBuilderNodeTrack.AnimationName)) ?? myModel.Animations.Clips[0];
          animationTreeNodeTrack.SetClip(animationClip);
          animationTreeNodeTrack.Loop = objBuilderNodeTrack.Loop;
          animationTreeNodeTrack.Speed = objBuilderNodeTrack.Speed;
          animationTreeNodeTrack.Interpolate = objBuilderNodeTrack.Interpolate;
          animationTreeNodeTrack.SynchronizeWithLayer = objBuilderNodeTrack.SynchronizeWithLayer;
          animationTreeNodeTrack.Key = objBuilderNodeTrack.Key;
          for (int index = 0; index < objBuilderNodeTrack.EventNames.Count; ++index)
            animationTreeNodeTrack.AddEvent(objBuilderNodeTrack.EventNames[index], objBuilderNodeTrack.EventTimes[index]);
          if (!string.IsNullOrEmpty(animationTreeNodeTrack.Key))
            controller.RegisterKeyedTrack((MyAnimationTreeNode) animationTreeNodeTrack);
        }
        else if (objBuilderNodeTrack.PathToModel != null)
          MyLog.Default.Log(MyLogSeverity.Error, "Cannot load MWM track {0}.", (object) objBuilderNodeTrack.PathToModel);
        return (MyAnimationTreeNode) animationTreeNodeTrack;
      }
      if (objBuilderNode is MyObjectBuilder_AnimationTreeNodeMix1D animationTreeNodeMix1D)
      {
        MyAnimationTreeNodeMix1D animationTreeNodeMix1D1 = new MyAnimationTreeNodeMix1D();
        if (animationTreeNodeMix1D.Children != null)
        {
          foreach (MyParameterAnimTreeNodeMapping child in animationTreeNodeMix1D.Children)
          {
            MyAnimationTreeNodeMix1D.MyParameterNodeMapping parameterNodeMapping = new MyAnimationTreeNodeMix1D.MyParameterNodeMapping()
            {
              ParamValueBinding = child.Param,
              Child = MyAnimationControllerComponentLoadFromDef.InitNodeAnimationTree(controller, child.Node, forceReloadMwm)
            };
            animationTreeNodeMix1D1.ChildMappings.Add(parameterNodeMapping);
          }
          animationTreeNodeMix1D1.ChildMappings.Sort((Comparison<MyAnimationTreeNodeMix1D.MyParameterNodeMapping>) ((x, y) => x.ParamValueBinding.CompareTo(y.ParamValueBinding)));
        }
        animationTreeNodeMix1D1.ParameterName = MyStringId.GetOrCompute(animationTreeNodeMix1D.ParameterName);
        animationTreeNodeMix1D1.Circular = animationTreeNodeMix1D.Circular;
        animationTreeNodeMix1D1.Sensitivity = animationTreeNodeMix1D.Sensitivity;
        MyAnimationTreeNodeMix1D animationTreeNodeMix1D2 = animationTreeNodeMix1D1;
        float? maxChange = animationTreeNodeMix1D.MaxChange;
        double num = maxChange.HasValue ? (double) maxChange.GetValueOrDefault() : double.PositiveInfinity;
        animationTreeNodeMix1D2.MaxChange = (float) num;
        if ((double) animationTreeNodeMix1D1.MaxChange <= 0.0)
          animationTreeNodeMix1D1.MaxChange = float.PositiveInfinity;
        return (MyAnimationTreeNode) animationTreeNodeMix1D1;
      }
      MyObjectBuilder_AnimationTreeNodeAdd animationTreeNodeAdd = objBuilderNode as MyObjectBuilder_AnimationTreeNodeAdd;
      return (MyAnimationTreeNode) null;
    }

    private static MyCondition<float>.MyOperation ConvertOperation(
      MyObjectBuilder_AnimationSMCondition.MyOperationType operation)
    {
      switch (operation)
      {
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.AlwaysFalse:
          return MyCondition<float>.MyOperation.AlwaysFalse;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.AlwaysTrue:
          return MyCondition<float>.MyOperation.AlwaysTrue;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.NotEqual:
          return MyCondition<float>.MyOperation.NotEqual;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.Less:
          return MyCondition<float>.MyOperation.Less;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.LessOrEqual:
          return MyCondition<float>.MyOperation.LessOrEqual;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.Equal:
          return MyCondition<float>.MyOperation.Equal;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.GreaterOrEqual:
          return MyCondition<float>.MyOperation.GreaterOrEqual;
        case MyObjectBuilder_AnimationSMCondition.MyOperationType.Greater:
          return MyCondition<float>.MyOperation.Greater;
        default:
          return MyCondition<float>.MyOperation.AlwaysFalse;
      }
    }

    private struct MyAnimationVirtualNodeData
    {
      public bool ExceptTarget;
      public string AnyNodePrefix;
    }

    private class MyAnimationVirtualNodes
    {
      public readonly Dictionary<string, MyAnimationControllerComponentLoadFromDef.MyAnimationVirtualNodeData> NodesAny = new Dictionary<string, MyAnimationControllerComponentLoadFromDef.MyAnimationVirtualNodeData>();
    }
  }
}
