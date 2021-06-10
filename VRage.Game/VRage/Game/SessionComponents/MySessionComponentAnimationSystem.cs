// Decompiled with JetBrains decompiler
// Type: VRage.Game.SessionComponents.MySessionComponentAnimationSystem
// Assembly: VRage.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4A8C954-DA6F-40CE-8710-DE433EC3E0F2
// Assembly location: D:\Files\library_development\lib_se\VRage.Game.dll

using System;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.Game.Components;
using VRage.Game.Debugging;
using VRage.Game.Definitions;
using VRage.Game.Definitions.Animation;
using VRage.Game.Entity;
using VRage.Game.Models;
using VRage.Game.ObjectBuilders;
using VRage.Generics;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageRender.Animations;

namespace VRage.Game.SessionComponents
{
  [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, 0)]
  public class MySessionComponentAnimationSystem : MySessionComponentBase
  {
    public static MySessionComponentAnimationSystem Static;
    private readonly HashSet<MyAnimationControllerComponent> m_skinnedEntityComponents = new HashSet<MyAnimationControllerComponent>();
    private readonly HashSet<MyAnimationControllerComponent> m_skinnedEntityComponentsToAdd = new HashSet<MyAnimationControllerComponent>();
    private readonly HashSet<MyAnimationControllerComponent> m_skinnedEntityComponentsToRemove = new HashSet<MyAnimationControllerComponent>();
    private readonly FastResourceLock m_lock = new FastResourceLock();
    private int m_debuggingSendNameCounter;
    private const int m_debuggingSendNameCounterMax = 60;
    private string m_debuggingLastNameSent;
    private readonly List<MyStateMachineNode> m_debuggingAnimControllerCurrentNodes = new List<MyStateMachineNode>();
    private readonly List<int[]> m_debuggingAnimControllerTreePath = new List<int[]>();
    private List<MyAnimationControllerComponent> m_updatedAnimationControllers = new List<MyAnimationControllerComponent>();
    private List<MyAnimationControllerComponent> m_updatedControllersSwap = new List<MyAnimationControllerComponent>();
    public MyEntity EntitySelectedForDebug;

    public IEnumerable<MyAnimationControllerComponent> RegisteredAnimationComponents => (IEnumerable<MyAnimationControllerComponent>) this.m_skinnedEntityComponents;

    public override void LoadData()
    {
      this.EntitySelectedForDebug = (MyEntity) null;
      this.m_skinnedEntityComponents.Clear();
      this.m_skinnedEntityComponentsToAdd.Clear();
      this.m_skinnedEntityComponentsToRemove.Clear();
      MySessionComponentAnimationSystem.Static = this;
      if (!MySessionComponentExtDebug.Static.IsHandlerRegistered(new MySessionComponentExtDebug.ReceivedMsgHandler(this.LiveDebugging_ReceivedMessageHandler)))
        MySessionComponentExtDebug.Static.ReceivedMsg += new MySessionComponentExtDebug.ReceivedMsgHandler(this.LiveDebugging_ReceivedMessageHandler);
      MyAnimationTreeNodeDynamicTrack.OnAction += new Func<MyStringId, MyAnimationTreeNodeDynamicTrack.DynamicTrackData>(this.OnDynamicTrackAction);
    }

    protected override void UnloadData()
    {
      this.EntitySelectedForDebug = (MyEntity) null;
      this.m_skinnedEntityComponents.Clear();
      this.m_skinnedEntityComponentsToAdd.Clear();
      this.m_skinnedEntityComponentsToRemove.Clear();
      MyAnimationTreeNodeDynamicTrack.OnAction -= new Func<MyStringId, MyAnimationTreeNodeDynamicTrack.DynamicTrackData>(this.OnDynamicTrackAction);
      MySessionComponentAnimationSystem.Static = (MySessionComponentAnimationSystem) null;
    }

    public override void UpdateAfterSimulation()
    {
      base.UpdateAfterSimulation();
      using (this.m_lock.AcquireExclusiveUsing())
      {
        foreach (MyAnimationControllerComponent controllerComponent in this.m_skinnedEntityComponentsToRemove)
        {
          if (this.m_skinnedEntityComponents.Remove(controllerComponent))
            this.m_skinnedEntityComponentsToAdd.Remove(controllerComponent);
        }
        this.m_skinnedEntityComponentsToRemove.Clear();
        foreach (MyAnimationControllerComponent controllerComponent in this.m_skinnedEntityComponentsToAdd)
          this.m_skinnedEntityComponents.Add(controllerComponent);
        this.m_skinnedEntityComponentsToAdd.Clear();
      }
      this.LiveDebugging();
    }

    private void PostProcessAnimations()
    {
      using (this.m_lock.AcquireExclusiveUsing())
        MyUtils.Swap<List<MyAnimationControllerComponent>>(ref this.m_updatedControllersSwap, ref this.m_updatedAnimationControllers);
      foreach (MyAnimationControllerComponent controllerComponent in this.m_updatedControllersSwap)
        controllerComponent.FinishUpdate();
      this.m_updatedControllersSwap.Clear();
    }

    internal void RegisterEntityComponent(MyAnimationControllerComponent entityComponent)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_skinnedEntityComponentsToAdd.Add(entityComponent);
    }

    internal void UnregisterEntityComponent(MyAnimationControllerComponent entityComponent)
    {
      using (this.m_lock.AcquireExclusiveUsing())
        this.m_skinnedEntityComponentsToRemove.Add(entityComponent);
    }

    private void LiveDebugging()
    {
      if (this.Session == null || MySessionComponentExtDebug.Static == null)
        return;
      MyEntity myEntity = this.EntitySelectedForDebug ?? (this.Session.ControlledObject != null ? this.Session.ControlledObject.Entity as MyEntity : (MyEntity) null);
      if (myEntity == null)
        return;
      MyAnimationControllerComponent controllerComponent = myEntity.Components.Get<MyAnimationControllerComponent>();
      if (controllerComponent == null || controllerComponent.SourceId.TypeId.IsNull)
        return;
      --this.m_debuggingSendNameCounter;
      if (controllerComponent.SourceId.SubtypeName != this.m_debuggingLastNameSent)
        this.m_debuggingSendNameCounter = 0;
      if (this.m_debuggingSendNameCounter <= 0)
      {
        MyDefinitionId sourceId = controllerComponent.SourceId;
        this.LiveDebugging_SendControllerNameToEditor(sourceId.SubtypeName);
        this.m_debuggingSendNameCounter = 60;
        sourceId = controllerComponent.SourceId;
        this.m_debuggingLastNameSent = sourceId.SubtypeName;
      }
      this.LiveDebugging_SendAnimationStateChangesToEditor(controllerComponent.Controller);
    }

    private void LiveDebugging_SendControllerNameToEditor(string subtypeName)
    {
      ACConnectToEditorMsg msg = new ACConnectToEditorMsg()
      {
        ACName = subtypeName
      };
      MySessionComponentExtDebug.Static.SendMessageToClients<ACConnectToEditorMsg>(msg);
    }

    private void LiveDebugging_SendAnimationStateChangesToEditor(
      MyAnimationController animController)
    {
      if (animController == null)
        return;
      int layerCount = animController.GetLayerCount();
      if (layerCount != this.m_debuggingAnimControllerCurrentNodes.Count)
      {
        this.m_debuggingAnimControllerCurrentNodes.Clear();
        for (int index = 0; index < layerCount; ++index)
          this.m_debuggingAnimControllerCurrentNodes.Add((MyStateMachineNode) null);
        this.m_debuggingAnimControllerTreePath.Clear();
        for (int index = 0; index < layerCount; ++index)
          this.m_debuggingAnimControllerTreePath.Add(new int[animController.GetLayerByIndex(index).VisitedTreeNodesPath.Length]);
      }
      for (int index = 0; index < layerCount; ++index)
      {
        int[] visitedTreeNodesPath = animController.GetLayerByIndex(index).VisitedTreeNodesPath;
        if (animController.GetLayerByIndex(index).CurrentNode != this.m_debuggingAnimControllerCurrentNodes[index] || !MySessionComponentAnimationSystem.LiveDebugging_CompareAnimTreePathSeqs(visitedTreeNodesPath, this.m_debuggingAnimControllerTreePath[index]))
        {
          Array.Copy((Array) visitedTreeNodesPath, (Array) this.m_debuggingAnimControllerTreePath[index], visitedTreeNodesPath.Length);
          this.m_debuggingAnimControllerCurrentNodes[index] = animController.GetLayerByIndex(index).CurrentNode;
          if (this.m_debuggingAnimControllerCurrentNodes[index] != null)
          {
            ACSendStateToEditorMsg msg = ACSendStateToEditorMsg.Create(this.m_debuggingAnimControllerCurrentNodes[index].Name, this.m_debuggingAnimControllerTreePath[index]);
            MySessionComponentExtDebug.Static.SendMessageToClients<ACSendStateToEditorMsg>(msg);
          }
        }
      }
    }

    private static bool LiveDebugging_CompareAnimTreePathSeqs(int[] seq1, int[] seq2)
    {
      if (seq1 == null || seq2 == null || seq1.Length != seq2.Length)
        return false;
      for (int index = 0; index < seq1.Length; ++index)
      {
        if (seq1[index] != seq2[index])
          return false;
        if (seq1[index] == 0 && seq2[index] == 0)
          return true;
      }
      return true;
    }

    private void LiveDebugging_ReceivedMessageHandler(
      MyExternalDebugStructures.CommonMsgHeader messageHeader,
      byte[] messageData)
    {
      ACReloadInGameMsg outMsg;
      if (!MyExternalDebugStructures.ReadMessageFromPtr<ACReloadInGameMsg>(ref messageHeader, messageData, out outMsg))
        return;
      try
      {
        string acContentAddress = outMsg.ACContentAddress;
        string acAddress = outMsg.ACAddress;
        string acName = outMsg.ACName;
        MyObjectBuilder_Definitions objectBuilder;
        if (!MyObjectBuilderSerializer.DeserializeXML<MyObjectBuilder_Definitions>(acAddress, out objectBuilder) || objectBuilder.Definitions == null || objectBuilder.Definitions.Length == 0)
          return;
        MyObjectBuilder_DefinitionBase definition1 = objectBuilder.Definitions[0];
        MyModContext modContext = new MyModContext();
        modContext.Init("AnimationControllerDefinition", acAddress, acContentAddress);
        MyAnimationControllerDefinition controllerDefinition = new MyAnimationControllerDefinition();
        controllerDefinition.Init(definition1, modContext);
        MyStringHash orCompute = MyStringHash.GetOrCompute(acName);
        MyAnimationControllerDefinition definition2 = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationControllerDefinition>(orCompute);
        MyDefinitionPostprocessor postProcessor = MyDefinitionManagerBase.GetPostProcessor(typeof (MyObjectBuilder_AnimationControllerDefinition));
        if (postProcessor != null)
        {
          MyDefinitionPostprocessor.Bundle currentDefinitions = new MyDefinitionPostprocessor.Bundle()
          {
            Context = MyModContext.BaseGame,
            Definitions = new Dictionary<MyStringHash, MyDefinitionBase>()
            {
              {
                orCompute,
                (MyDefinitionBase) definition2
              }
            },
            Set = new MyDefinitionSet()
          };
          currentDefinitions.Set.AddDefinition((MyDefinitionBase) definition2);
          MyDefinitionPostprocessor.Bundle bundle = new MyDefinitionPostprocessor.Bundle()
          {
            Context = modContext,
            Definitions = new Dictionary<MyStringHash, MyDefinitionBase>()
            {
              {
                orCompute,
                (MyDefinitionBase) controllerDefinition
              }
            },
            Set = new MyDefinitionSet()
          };
          bundle.Set.AddDefinition((MyDefinitionBase) controllerDefinition);
          postProcessor.AfterLoaded(ref bundle);
          postProcessor.OverrideBy(ref currentDefinitions, ref bundle);
        }
        foreach (MyAnimationControllerComponent skinnedEntityComponent in this.m_skinnedEntityComponents)
        {
          if (skinnedEntityComponent != null && skinnedEntityComponent.SourceId.SubtypeName == acName)
          {
            skinnedEntityComponent.Clear();
            skinnedEntityComponent.InitFromDefinition(definition2, true);
            if (skinnedEntityComponent.ReloadBonesNeeded != null)
              skinnedEntityComponent.ReloadBonesNeeded();
          }
        }
      }
      catch (Exception ex)
      {
        MyLog.Default.WriteLine(ex);
      }
    }

    public void ReloadMwmTracks()
    {
      foreach (MyAnimationControllerComponent skinnedEntityComponent in this.m_skinnedEntityComponents)
      {
        MyAnimationControllerDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationControllerDefinition>(MyStringHash.GetOrCompute(skinnedEntityComponent.SourceId.SubtypeName));
        if (definition != null)
        {
          skinnedEntityComponent.Clear();
          skinnedEntityComponent.InitFromDefinition(definition, true);
          if (skinnedEntityComponent.ReloadBonesNeeded != null)
            skinnedEntityComponent.ReloadBonesNeeded();
        }
      }
    }

    private MyAnimationTreeNodeDynamicTrack.DynamicTrackData OnDynamicTrackAction(
      MyStringId action)
    {
      MyAnimationTreeNodeDynamicTrack.DynamicTrackData dynamicTrackData = new MyAnimationTreeNodeDynamicTrack.DynamicTrackData();
      MyAnimationDefinition definition = MyDefinitionManagerBase.Static.GetDefinition<MyAnimationDefinition>(action.ToString());
      if (definition == null)
        return dynamicTrackData;
      string animationModel = definition.AnimationModel;
      if (string.IsNullOrEmpty(definition.AnimationModel))
        return dynamicTrackData;
      if (!MyFileSystem.FileExists(Path.IsPathRooted(animationModel) ? animationModel : Path.Combine(MyFileSystem.ContentPath, animationModel)))
      {
        definition.Status = MyAnimationDefinition.AnimationStatus.Failed;
        return dynamicTrackData;
      }
      MyModel onlyAnimationData = MyModels.GetModelOnlyAnimationData(animationModel);
      if (onlyAnimationData != null && onlyAnimationData.Animations == null || onlyAnimationData.Animations.Clips.Count == 0)
        return dynamicTrackData;
      dynamicTrackData.Clip = onlyAnimationData.Animations.Clips[definition.ClipIndex];
      dynamicTrackData.Loop = definition.Loop;
      return dynamicTrackData;
    }

    public override Type[] Dependencies => new Type[1]
    {
      typeof (MySessionComponentExtDebug)
    };
  }
}
