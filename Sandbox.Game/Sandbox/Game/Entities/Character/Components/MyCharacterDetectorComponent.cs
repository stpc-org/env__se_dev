// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Character.Components.MyCharacterDetectorComponent
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Character.Components
{
  public abstract class MyCharacterDetectorComponent : MyCharacterComponent
  {
    private IMyEntity m_detectedEntity;
    protected IMyUseObject m_interactiveObject;
    protected static List<MyEntity> m_detectableEntities = new List<MyEntity>();
    protected MyHudNotification m_useObjectNotification;
    protected MyHudNotification m_showTerminalNotification;
    protected MyHudNotification m_openInventoryNotification;
    protected MyHudNotification m_buildPlannerNotification;
    protected bool m_usingContinuously;
    protected MyCharacterHitInfo CharHitInfo;

    public static event Action<IMyUseObject> OnInteractiveObjectChanged;

    public static event Action<IMyUseObject> OnInteractiveObjectUsed;

    public override void UpdateAfterSimulation10()
    {
      if (MySession.Static.ControlledEntity != this.Character)
        return;
      if (this.m_useObjectNotification != null && !this.m_usingContinuously)
        MyHud.Notifications.Add((MyHudNotificationBase) this.m_useObjectNotification);
      this.m_usingContinuously = false;
      if (!this.Character.IsSitting && !this.Character.IsDead)
      {
        MySandboxGame.Static.Invoke("MyCharacterDetectorComponent::DoDetection", (object) this, (Action<object>) (context =>
        {
          MyCharacterDetectorComponent detectorComponent = (MyCharacterDetectorComponent) context;
          MyCharacter character = detectorComponent.Character;
          if (character == null)
            return;
          detectorComponent.DoDetection(character.TargetFromCamera);
        }));
      }
      else
      {
        if (MySession.Static.ControlledEntity != this.Character)
          return;
        MyHud.SelectedObjectHighlight.RemoveHighlight();
      }
    }

    protected abstract void DoDetection(bool useHead);

    public virtual IMyUseObject UseObject
    {
      get => this.m_interactiveObject;
      set
      {
        if (value == this.m_interactiveObject)
          return;
        if (this.m_interactiveObject != null)
        {
          this.UseClose();
          this.m_interactiveObject.OnSelectionLost();
          this.InteractiveObjectRemoved();
        }
        this.m_interactiveObject = value;
        this.InteractiveObjectChanged();
      }
    }

    public IMyEntity DetectedEntity
    {
      protected set
      {
        if (this.m_detectedEntity != null)
          this.m_detectedEntity.OnMarkForClose -= new Action<IMyEntity>(this.OnDetectedEntityMarkForClose);
        this.m_detectedEntity = value;
        if (this.m_detectedEntity == null)
          return;
        this.m_detectedEntity.OnMarkForClose += new Action<IMyEntity>(this.OnDetectedEntityMarkForClose);
      }
      get => this.m_detectedEntity;
    }

    public Vector3D HitPosition { protected set; get; }

    public Vector3 HitNormal { protected set; get; }

    public uint ShapeKey { protected set; get; }

    public Vector3D StartPosition { protected set; get; }

    public MyStringHash HitMaterial { protected set; get; }

    public HkRigidBody HitBody { protected set; get; }

    public object HitTag { get; protected set; }

    protected virtual void OnDetectedEntityMarkForClose(IMyEntity obj)
    {
      this.DetectedEntity = (IMyEntity) null;
      if (this.UseObject == null)
        return;
      this.UseObject = (IMyUseObject) null;
      MyHud.SelectedObjectHighlight.RemoveHighlight();
    }

    protected void UseClose()
    {
      if (this.Character == null || this.UseObject == null || !this.UseObject.IsActionSupported(UseActionEnum.Close))
        return;
      this.UseObject.Use(UseActionEnum.Close, (IMyEntity) this.Character);
    }

    protected void InteractiveObjectRemoved()
    {
      if (this.Character == null)
        return;
      this.Character.RemoveNotification(ref this.m_useObjectNotification);
      this.Character.RemoveNotification(ref this.m_showTerminalNotification);
      this.Character.RemoveNotification(ref this.m_openInventoryNotification);
      this.Character.RemoveNotification(ref this.m_buildPlannerNotification);
    }

    protected void InteractiveObjectChanged()
    {
      if (MySession.Static.ControlledEntity != this.Character)
        return;
      if (this.UseObject != null)
      {
        this.GetNotification(this.UseObject, UseActionEnum.Manipulate, ref this.m_useObjectNotification);
        this.GetNotification(this.UseObject, UseActionEnum.OpenTerminal, ref this.m_showTerminalNotification);
        this.GetNotification(this.UseObject, UseActionEnum.OpenInventory, ref this.m_openInventoryNotification);
        this.GetNotification(this.UseObject, UseActionEnum.BuildPlanner, ref this.m_buildPlannerNotification);
        MyStringId myStringId1 = this.m_useObjectNotification != null ? this.m_useObjectNotification.Text : MySpaceTexts.Blank;
        MyStringId myStringId2 = this.m_showTerminalNotification != null ? this.m_showTerminalNotification.Text : MySpaceTexts.Blank;
        MyStringId myStringId3 = this.m_openInventoryNotification != null ? this.m_openInventoryNotification.Text : MySpaceTexts.Blank;
        if (myStringId1 != MySpaceTexts.Blank)
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_useObjectNotification);
        if (myStringId2 != MySpaceTexts.Blank && myStringId2 != myStringId1)
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_showTerminalNotification);
        if (myStringId3 != MySpaceTexts.Blank && myStringId3 != myStringId2 && myStringId3 != myStringId1)
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_openInventoryNotification);
        if (this.m_buildPlannerNotification != null)
          MyHud.Notifications.Add((MyHudNotificationBase) this.m_buildPlannerNotification);
      }
      if (MyCharacterDetectorComponent.OnInteractiveObjectChanged == null)
        return;
      MyCharacterDetectorComponent.OnInteractiveObjectChanged(this.UseObject);
    }

    public void RaiseObjectUsed()
    {
      if (MyCharacterDetectorComponent.OnInteractiveObjectUsed == null)
        return;
      MyCharacterDetectorComponent.OnInteractiveObjectUsed(this.UseObject);
    }

    private void GetNotification(
      IMyUseObject useObject,
      UseActionEnum actionType,
      ref MyHudNotification notification)
    {
      if ((useObject.SupportedActions & actionType) == UseActionEnum.None)
        return;
      MyActionDescription actionInfo = useObject.GetActionInfo(actionType);
      this.Character.RemoveNotification(ref notification);
      notification = new MyHudNotification(actionInfo.Text, 0, level: (actionInfo.IsTextControlHint ? MyNotificationLevel.Control : MyNotificationLevel.Normal));
      if (!MyDebugDrawSettings.DEBUG_DRAW_JOYSTICK_CONTROL_HINTS && (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed))
        notification.SetTextFormatArguments(actionInfo.FormatParams);
      else if (actionInfo.ShowForGamepad)
      {
        if (actionInfo.JoystickText.HasValue)
          notification.Text = actionInfo.JoystickText.Value;
        if (actionInfo.JoystickFormatParams == null)
          return;
        notification.SetTextFormatArguments(actionInfo.JoystickFormatParams);
      }
      else
      {
        notification.Text = MyStringId.NullOrEmpty;
        notification = (MyHudNotification) null;
      }
    }

    public void UseContinues()
    {
      MyHud.Notifications.Remove((MyHudNotificationBase) this.m_useObjectNotification);
      this.m_usingContinuously = true;
    }

    public override void OnCharacterDead()
    {
      this.UseObject = (IMyUseObject) null;
      base.OnCharacterDead();
    }

    public override void OnAddedToContainer()
    {
      base.OnAddedToContainer();
      this.Character.OnControlStateChanged += new MyCharacter.ControlStateChanged(this.CharacterOnOnControlStateChanged);
    }

    public override void OnBeforeRemovedFromContainer()
    {
      base.OnBeforeRemovedFromContainer();
      this.Character.OnControlStateChanged -= new MyCharacter.ControlStateChanged(this.CharacterOnOnControlStateChanged);
    }

    private void CharacterOnOnControlStateChanged(bool hasControl)
    {
      if (!hasControl)
        this.NeedsUpdateAfterSimulation10 = false;
      else
        this.NeedsUpdateAfterSimulation10 = !this.Character.ControllerInfo.IsRemotelyControlled();
    }

    public override void OnRemovedFromScene()
    {
      this.UseObject = (IMyUseObject) null;
      base.OnRemovedFromScene();
    }

    protected void GatherDetectorsInArea(Vector3D from)
    {
      BoundingSphereD sphere = new BoundingSphereD(from, (double) MyConstants.DEFAULT_INTERACTIVE_DISTANCE);
      MyGamePruningStructure.GetAllEntitiesInSphere(ref sphere, MyCharacterDetectorComponent.m_detectableEntities);
    }

    protected void EnableDetectorsInArea(Vector3D from)
    {
      this.GatherDetectorsInArea(from);
      for (int index = 0; index < MyCharacterDetectorComponent.m_detectableEntities.Count; ++index)
      {
        MyEntity detectableEntity = MyCharacterDetectorComponent.m_detectableEntities[index];
        if (detectableEntity is MyCompoundCubeBlock compoundCubeBlock)
        {
          foreach (MySlimBlock block in compoundCubeBlock.GetBlocks())
          {
            if (block.FatBlock != null)
              MyCharacterDetectorComponent.m_detectableEntities.Add((MyEntity) block.FatBlock);
          }
        }
        MyUseObjectsComponentBase component;
        if (detectableEntity.Components.TryGet<MyUseObjectsComponentBase>(out component) && component.DetectorPhysics != null)
        {
          component.PositionChanged(component.Container.Get<MyPositionComponentBase>());
          component.DetectorPhysics.Enabled = true;
        }
      }
    }

    protected void DisableDetectors()
    {
      foreach (MyEntity detectableEntity in MyCharacterDetectorComponent.m_detectableEntities)
      {
        MyUseObjectsComponentBase component;
        if (detectableEntity.Components.TryGet<MyUseObjectsComponentBase>(out component) && component.DetectorPhysics != null)
          component.DetectorPhysics.Enabled = false;
      }
      MyCharacterDetectorComponent.m_detectableEntities.Clear();
    }

    protected static void HandleInteractiveObject(IMyUseObject interactive)
    {
      if (MyFakes.ENABLE_USE_NEW_OBJECT_HIGHLIGHT)
      {
        MyHud.SelectedObjectHighlight.Color = MySector.EnvironmentDefinition.ContourHighlightColor;
        if (interactive.InstanceID != -1 || interactive is MyFloatingObject || interactive.Owner is MyInventoryBagEntity)
        {
          MyHud.SelectedObjectHighlight.HighlightAttribute = (string) null;
          MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.OutlineHighlight;
        }
        else if (interactive is MyCharacter myCharacter && myCharacter.IsDead)
        {
          MyHud.SelectedObjectHighlight.HighlightAttribute = (string) null;
          MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.OutlineHighlight;
        }
        else
        {
          bool flag = false;
          MyModelDummy dummy = interactive.Dummy;
          if (dummy != null && dummy.CustomData != null)
          {
            object obj;
            flag = dummy.CustomData.TryGetValue("highlight", out obj);
            string str1 = obj as string;
            if (flag && str1 != null)
            {
              MyHud.SelectedObjectHighlight.HighlightAttribute = str1;
              MyHud.SelectedObjectHighlight.HighlightStyle = !(interactive.Owner is MyTextPanel) ? MyHudObjectHighlightStyle.OutlineHighlight : MyHudObjectHighlightStyle.EdgeHighlight;
            }
            int num = dummy.CustomData.TryGetValue("highlighttype", out obj) ? 1 : 0;
            string str2 = obj as string;
            if (num != 0 && str2 != null)
              MyHud.SelectedObjectHighlight.HighlightStyle = !(str2 == "edge") ? MyHudObjectHighlightStyle.OutlineHighlight : MyHudObjectHighlightStyle.EdgeHighlight;
          }
          if (!flag)
          {
            MyHud.SelectedObjectHighlight.HighlightAttribute = (string) null;
            MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.DummyHighlight;
          }
        }
      }
      else
      {
        MyHud.SelectedObjectHighlight.HighlightAttribute = (string) null;
        MyHud.SelectedObjectHighlight.HighlightStyle = MyHudObjectHighlightStyle.DummyHighlight;
      }
      if (interactive.Owner is MyCubeBlock owner)
      {
        if (owner.HighlightMode == MyCubeBlockHighlightModes.AlwaysCanUse)
          MyHud.SelectedObjectHighlight.Color = MySector.EnvironmentDefinition.ContourHighlightColor;
        else if (owner.HighlightMode == MyCubeBlockHighlightModes.Default && owner.GetPlayerRelationToOwner() == MyRelationsBetweenPlayerAndBlock.Enemies || owner.HighlightMode == MyCubeBlockHighlightModes.AlwaysHostile)
          MyHud.SelectedObjectHighlight.Color = MySector.EnvironmentDefinition.ContourHighlightColorAccessDenied;
      }
      MyHud.SelectedObjectHighlight.Highlight(interactive);
    }
  }
}
