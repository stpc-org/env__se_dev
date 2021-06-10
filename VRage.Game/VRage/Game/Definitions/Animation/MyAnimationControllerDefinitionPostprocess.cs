// Decompiled with JetBrains decompiler
// Type: VRage.Game.Definitions.Animation.MyAnimationControllerDefinitionPostprocess
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.Definitions.Animation
{
  internal class MyAnimationControllerDefinitionPostprocess : MyDefinitionPostprocessor
  {
    public override void AfterLoaded(ref MyDefinitionPostprocessor.Bundle definitions)
    {
      foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition in definitions.Definitions)
      {
        if (definition.Value is MyAnimationControllerDefinition controllerDefinition && controllerDefinition.StateMachines != null && (!definition.Value.Context.IsBaseGame && definition.Value.Context != null) && definition.Value.Context.ModPath != null)
        {
          foreach (MyObjectBuilder_AnimationSM stateMachine in controllerDefinition.StateMachines)
          {
            foreach (MyObjectBuilder_AnimationSMNode node in stateMachine.Nodes)
            {
              if (node.AnimationTree != null && node.AnimationTree.Child != null)
                this.ResolveMwmPaths(definition.Value.Context, node.AnimationTree.Child);
            }
          }
        }
      }
    }

    private void ResolveMwmPaths(
      MyModContext modContext,
      MyObjectBuilder_AnimationTreeNode objBuilderNode)
    {
      if (objBuilderNode is MyObjectBuilder_AnimationTreeNodeTrack animationTreeNodeTrack && animationTreeNodeTrack.PathToModel != null)
      {
        string path = Path.Combine(modContext.ModPath, animationTreeNodeTrack.PathToModel);
        if (MyFileSystem.FileExists(path))
          animationTreeNodeTrack.PathToModel = path;
      }
      if (objBuilderNode is MyObjectBuilder_AnimationTreeNodeMix1D animationTreeNodeMix1D && animationTreeNodeMix1D.Children != null)
      {
        foreach (MyParameterAnimTreeNodeMapping child in animationTreeNodeMix1D.Children)
        {
          if (child.Node != null)
            this.ResolveMwmPaths(modContext, child.Node);
        }
      }
      if (!(objBuilderNode is MyObjectBuilder_AnimationTreeNodeAdd animationTreeNodeAdd))
        return;
      if (animationTreeNodeAdd.BaseNode.Node != null)
        this.ResolveMwmPaths(modContext, animationTreeNodeAdd.BaseNode.Node);
      if (animationTreeNodeAdd.AddNode.Node == null)
        return;
      this.ResolveMwmPaths(modContext, animationTreeNodeAdd.AddNode.Node);
    }

    public override void AfterPostprocess(
      MyDefinitionSet set,
      Dictionary<MyStringHash, MyDefinitionBase> definitions)
    {
    }

    public override void OverrideBy(
      ref MyDefinitionPostprocessor.Bundle currentDefinitions,
      ref MyDefinitionPostprocessor.Bundle overrideBySet)
    {
      foreach (KeyValuePair<MyStringHash, MyDefinitionBase> definition1 in overrideBySet.Definitions)
      {
        MyAnimationControllerDefinition controllerDefinition = definition1.Value as MyAnimationControllerDefinition;
        if (definition1.Value.Enabled && controllerDefinition != null)
        {
          bool flag1 = true;
          if (currentDefinitions.Definitions.ContainsKey(definition1.Key) && currentDefinitions.Definitions[definition1.Key] is MyAnimationControllerDefinition definition)
          {
            foreach (MyObjectBuilder_AnimationSM stateMachine1 in controllerDefinition.StateMachines)
            {
              bool flag2 = false;
              foreach (MyObjectBuilder_AnimationSM stateMachine2 in definition.StateMachines)
              {
                if (stateMachine1.Name == stateMachine2.Name)
                {
                  stateMachine2.Nodes = stateMachine1.Nodes;
                  stateMachine2.Transitions = stateMachine1.Transitions;
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
                definition.StateMachines.Add(stateMachine1);
            }
            foreach (MyObjectBuilder_AnimationLayer layer1 in controllerDefinition.Layers)
            {
              bool flag2 = false;
              foreach (MyObjectBuilder_AnimationLayer layer2 in definition.Layers)
              {
                if (layer1.Name == layer2.Name)
                {
                  layer2.Name = layer1.Name;
                  layer2.BoneMask = layer1.BoneMask;
                  layer2.InitialSMNode = layer1.InitialSMNode;
                  layer2.StateMachine = layer1.StateMachine;
                  layer2.Mode = layer1.Mode;
                  flag2 = true;
                }
              }
              if (!flag2)
                definition.Layers.Add(layer1);
            }
            flag1 = false;
          }
          if (flag1)
            currentDefinitions.Definitions[definition1.Key] = definition1.Value;
        }
      }
    }
  }
}
