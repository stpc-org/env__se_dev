// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyTargetDummyBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Havok;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using SpaceEngineers.ObjectBuilders.ObjectBuilders;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_TargetDummyBlock))]
  [MyTerminalInterface(new System.Type[] {typeof (Sandbox.ModAPI.IMyTargetDummyBlock), typeof (Sandbox.ModAPI.Ingame.IMyTargetDummyBlock)})]
  public class MyTargetDummyBlock : MyFunctionalBlock, Sandbox.ModAPI.IMyTargetDummyBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, IMyInventoryOwner, IMyConveyorEndpointBlock
  {
    private static readonly int TOOLBAR_ITEM_COUNT = 2;
    private static readonly float PULL_AMOUNT_CONSTANT = 4f;
    protected readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_useConveyorSystem;
    private readonly VRage.Sync.Sync<float, SyncDirection.BothWays> m_restorationDelay;
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_enableRestoration;
    private bool m_canSyncState = true;
    private List<MyEntitySubpart> m_subparts = new List<MyEntitySubpart>();
    private List<string> m_subpartNames = new List<string>();
    private Dictionary<long, MyTargetDummyBlock.MySubpartState> m_subpartStates = new Dictionary<long, MyTargetDummyBlock.MySubpartState>();
    private MyTimeSpan m_restorationTime;
    private bool m_areSubpartsInitialized;
    private bool m_isDestroyed = true;
    private MyMultilineConveyorEndpoint m_endpoint;
    public bool? LoadIsDestroyed;
    public int LoadTimeToRestore;
    private List<MyObjectBuilder_TargetDummyBlock.MySubpartSavedState> LoadSubpartState;
    private List<ToolbarItem> m_items;
    private static List<MyToolbar> m_openedToolbars = new List<MyToolbar>();
    private static bool m_shouldSetOtherToolbars;
    private bool m_syncing;
    private float m_currentFillThreshold;

    public bool CanSyncState => this.m_canSyncState;

    public float MinRestorationDelay => this.Definition.MinRegenerationTimeInS;

    public float MaxRestorationDelay => this.Definition.MaxRegenerationTimeInS;

    public MyTargetDummyBlockDefinition Definition => this.BlockDefinition as MyTargetDummyBlockDefinition;

    public MyToolbar Toolbar { get; set; }

    public float RestorationDelay
    {
      get => (float) this.m_restorationDelay;
      protected set
      {
        if ((double) (float) this.m_restorationDelay == (double) value)
          return;
        this.m_restorationDelay.Value = value;
      }
    }

    public bool EnableRestoration
    {
      get => (bool) this.m_enableRestoration;
      set
      {
        if ((bool) this.m_enableRestoration == value)
          return;
        this.m_enableRestoration.Value = value;
      }
    }

    private bool IsDestroyed
    {
      get => this.m_isDestroyed;
      set
      {
        if (this.m_isDestroyed == value)
          return;
        this.m_isDestroyed = value;
        if (this.m_isDestroyed)
        {
          this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
          this.m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((double) (float) this.m_restorationDelay);
          this.ActivateActionDestruction();
        }
        else
        {
          if (this.GetInventoryBase(0) is MyInventory inventoryBase && (double) this.m_currentFillThreshold >= (double) inventoryBase.VolumeFillFactor)
            return;
          this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
        }
      }
    }

    public IMyConveyorEndpoint ConveyorEndpoint => (IMyConveyorEndpoint) this.m_endpoint;

    bool IMyInventoryOwner.UseConveyorSystem
    {
      get => (bool) this.m_useConveyorSystem;
      set => this.m_useConveyorSystem.Value = value;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyTargetDummyBlockDefinition definition = this.Definition;
      this.m_endpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);
      this.m_useConveyorSystem.SetLocalValue(true);
      this.m_items = new List<ToolbarItem>(MyTargetDummyBlock.TOOLBAR_ITEM_COUNT);
      for (int index = 0; index < MyTargetDummyBlock.TOOLBAR_ITEM_COUNT; ++index)
        this.m_items.Add(new ToolbarItem() { EntityID = 0L });
      this.Toolbar = new MyToolbar(MyToolbarType.ButtonPanel, MyTargetDummyBlock.TOOLBAR_ITEM_COUNT, 1);
      this.Toolbar.DrawNumbers = false;
      if (objectBuilder is MyObjectBuilder_TargetDummyBlock targetDummyBlock)
      {
        this.m_useConveyorSystem.SetLocalValue(targetDummyBlock.UseConveyorSystem);
        this.m_restorationDelay.SetLocalValue(targetDummyBlock.RestorationDelay);
        this.m_enableRestoration.SetLocalValue(targetDummyBlock.EnableRestoration);
        if (targetDummyBlock.IsDestroyed.HasValue)
        {
          this.LoadIsDestroyed = targetDummyBlock.IsDestroyed;
          this.LoadTimeToRestore = targetDummyBlock.TimeToRestore;
          this.LoadSubpartState = targetDummyBlock.SubpartState;
        }
        this.Toolbar.Init(targetDummyBlock.Toolbar, (MyEntity) this);
        for (int index = 0; index < 2; ++index)
        {
          MyToolbarItem itemAtIndex = this.Toolbar.GetItemAtIndex(index);
          if (itemAtIndex != null)
          {
            this.m_items.RemoveAt(index);
            this.m_items.Insert(index, ToolbarItem.FromItem(itemAtIndex));
          }
        }
      }
      double cubeSize = (double) MyDefinitionManager.Static.GetCubeSize(definition.CubeSize);
      float inventoryMaxVolume = definition.InventoryMaxVolume;
      Vector3 inventorySize = definition.InventorySize;
      MyInventory myInventory = MyEntityExtensions.GetInventory(this);
      if (myInventory == null)
      {
        myInventory = new MyInventory(inventoryMaxVolume, inventorySize, MyInventoryFlags.CanSend);
        this.Components.Add<MyInventoryBase>((MyInventoryBase) myInventory);
        myInventory.Constraint = definition.InventoryConstraint;
      }
      myInventory.ContentsChanged += new Action<MyInventoryBase>(this.ContentChangedCallback);
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
      this.m_enableRestoration.ValueChanged += new Action<SyncBase>(this.EnableRestorationChangedCallback);
      this.m_restorationDelay.ValueChanged += new Action<SyncBase>(this.RestorationDelayChangedCallback);
      this.Toolbar.ItemChanged += new Action<MyToolbar, MyToolbar.IndexArgs, bool>(this.Toolbar_ItemChanged);
      this.CreateTerminalControls();
      if (this.LoadSubpartState != null)
        return;
      this.LoadSubpartState = new List<MyObjectBuilder_TargetDummyBlock.MySubpartSavedState>();
    }

    public override void UpdateBeforeSimulation()
    {
      base.UpdateBeforeSimulation();
      if (this.CubeGrid.Physics == null)
        return;
      Vector3 linearVelocity = this.CubeGrid.Physics.LinearVelocity;
      Vector3 angularVelocity = this.CubeGrid.Physics.AngularVelocity;
      Vector3D centerOfMassWorld = this.CubeGrid.Physics.CenterOfMassWorld;
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        if (subpart.Physics != null && subpart.Physics.Enabled)
        {
          Vector3 vector3_1 = Vector3.Cross(this.CubeGrid.Physics.AngularVelocity, (Vector3) (subpart.Physics.CenterOfMassWorld - centerOfMassWorld));
          Vector3 vector3_2 = linearVelocity + vector3_1;
          if (subpart.Physics.LinearVelocity != vector3_2)
            subpart.Physics.LinearVelocity = vector3_2;
          if (subpart.Physics.AngularVelocity != angularVelocity)
            subpart.Physics.AngularVelocity = angularVelocity;
        }
      }
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyTargetDummyBlock>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlFactory.AddControl<MyTargetDummyBlock>((MyTerminalControl<MyTargetDummyBlock>) new MyTerminalControlButton<MyTargetDummyBlock>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, (Action<MyTargetDummyBlock>) (self =>
      {
        MyTargetDummyBlock.m_openedToolbars.Add(self.Toolbar);
        if (MyGuiScreenToolbarConfigBase.Static != null)
          return;
        MyTargetDummyBlock.m_shouldSetOtherToolbars = true;
        MyToolbarComponent.CurrentToolbar = self.Toolbar;
        MyGuiScreenBase screen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, (object) 0, (object) self, null);
        MyToolbarComponent.AutoUpdate = false;
        screen.Closed += (MyGuiScreenBase.ScreenHandler) ((source, isUnloading) =>
        {
          MyToolbarComponent.AutoUpdate = true;
          MyTargetDummyBlock.m_openedToolbars.Clear();
        });
        MyGuiSandbox.AddScreen(screen);
      })));
      MyTerminalControlSlider<MyTargetDummyBlock> slider = new MyTerminalControlSlider<MyTargetDummyBlock>("Delay", MySpaceTexts.BlockPropertyTitle_RegenerationDelay, MySpaceTexts.BlockPropertyDescription_RegenerationDelay);
      slider.SetLimits((MyTerminalValueControl<MyTargetDummyBlock, float>.GetterDelegate) (block => this.MinRestorationDelay), (MyTerminalValueControl<MyTargetDummyBlock, float>.GetterDelegate) (block => this.MaxRestorationDelay));
      slider.DefaultValue = new float?(5f);
      slider.Getter = (MyTerminalValueControl<MyTargetDummyBlock, float>.GetterDelegate) (x => x.RestorationDelay);
      slider.Setter = (MyTerminalValueControl<MyTargetDummyBlock, float>.SetterDelegate) ((x, v) => x.RestorationDelay = v);
      slider.Writer = (MyTerminalControl<MyTargetDummyBlock>.WriterDelegate) ((x, result) => result.AppendFormatedDecimal("", x.RestorationDelay, 0, " s"));
      slider.EnableActions<MyTargetDummyBlock>();
      MyTerminalControlFactory.AddControl<MyTargetDummyBlock>((MyTerminalControl<MyTargetDummyBlock>) slider);
      MyTerminalControlOnOffSwitch<MyTargetDummyBlock> controlOnOffSwitch = new MyTerminalControlOnOffSwitch<MyTargetDummyBlock>("Enable Restoration", MySpaceTexts.BlockPropertyTitle_EnableRegeneration, MySpaceTexts.BlockPropertyDescription_EnableRegeneration);
      controlOnOffSwitch.Getter = (MyTerminalValueControl<MyTargetDummyBlock, bool>.GetterDelegate) (x => x.EnableRestoration);
      controlOnOffSwitch.Setter = (MyTerminalValueControl<MyTargetDummyBlock, bool>.SetterDelegate) ((x, v) => x.EnableRestoration = v);
      MyTerminalControlFactory.AddControl<MyTargetDummyBlock>((MyTerminalControl<MyTargetDummyBlock>) controlOnOffSwitch);
    }

    private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
    {
      if (this.m_syncing)
        return;
      ToolbarItem toolbarItem1 = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
      ToolbarItem toolbarItem2 = this.m_items[index.ItemIndex];
      if (toolbarItem1.EntityID == 0L && toolbarItem2.EntityID == 0L || toolbarItem1.EntityID != 0L && toolbarItem2.EntityID != 0L && toolbarItem1.Equals((object) toolbarItem2))
        return;
      this.m_items.RemoveAt(index.ItemIndex);
      this.m_items.Insert(index.ItemIndex, toolbarItem1);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTargetDummyBlock, ToolbarItem, int>(this, (Func<MyTargetDummyBlock, Action<ToolbarItem, int>>) (x => new Action<ToolbarItem, int>(x.SendToolbarItemChanged)), toolbarItem1, index.ItemIndex);
      if (!MyTargetDummyBlock.m_shouldSetOtherToolbars)
        return;
      MyTargetDummyBlock.m_shouldSetOtherToolbars = false;
      foreach (MyToolbar openedToolbar in MyTargetDummyBlock.m_openedToolbars)
      {
        if (openedToolbar != self)
          openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
      }
      MyTargetDummyBlock.m_shouldSetOtherToolbars = true;
    }

    [Event(null, 386)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
    {
      this.m_syncing = true;
      MyToolbarItem myToolbarItem = (MyToolbarItem) null;
      if (sentItem.EntityID != 0L)
        myToolbarItem = ToolbarItem.ToItem(sentItem);
      this.Toolbar.SetItemAtIndex(index, myToolbarItem);
      this.m_syncing = false;
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      this.InitializationUpdate();
    }

    private void InitializationUpdate()
    {
      if (this.IsWorking)
        this.InitializeSubparts();
      else
        this.UninitializeSubparts();
    }

    private void ActivateActionDestruction()
    {
      if (!this.IsFunctional)
        return;
      this.Toolbar.UpdateItem(1);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.Toolbar.ActivateItemAtSlot(1, playActivationSound: false);
    }

    private void ActivateActionHit()
    {
      if (!this.IsFunctional)
        return;
      this.Toolbar.UpdateItem(0);
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.Toolbar.ActivateItemAtSlot(0, playActivationSound: false);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_TargetDummyBlock builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_TargetDummyBlock;
      builderCubeBlock.RestorationDelay = this.m_restorationDelay.Value;
      builderCubeBlock.EnableRestoration = this.m_enableRestoration.Value;
      builderCubeBlock.UseConveyorSystem = this.m_useConveyorSystem.Value;
      builderCubeBlock.SubpartState = this.LoadSubpartState;
      if (this.m_areSubpartsInitialized)
      {
        if (this.IsDestroyed)
        {
          builderCubeBlock.IsDestroyed = new bool?(true);
          builderCubeBlock.TimeToRestore = (int) (this.m_restorationTime - MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds)).Seconds;
        }
        else
        {
          builderCubeBlock.IsDestroyed = new bool?(false);
          builderCubeBlock.SubpartState = new List<MyObjectBuilder_TargetDummyBlock.MySubpartSavedState>();
          foreach (KeyValuePair<long, MyTargetDummyBlock.MySubpartState> subpartState in this.m_subpartStates)
            builderCubeBlock.SubpartState.Add(new MyObjectBuilder_TargetDummyBlock.MySubpartSavedState()
            {
              SubpartName = subpartState.Value.Name,
              SubpartHealth = subpartState.Value.IntegrityCurrent
            });
        }
      }
      builderCubeBlock.Toolbar = this.Toolbar.GetObjectBuilder();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void EnableRestorationChangedCallback(SyncBase obj)
    {
      if (!(bool) this.m_enableRestoration || !this.m_isDestroyed)
        return;
      this.m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((double) (float) this.m_restorationDelay);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    private void RestorationDelayChangedCallback(SyncBase obj)
    {
      if (!this.m_isDestroyed)
        return;
      this.m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((double) (float) this.m_restorationDelay);
    }

    private void ContentChangedCallback(MyInventoryBase obj)
    {
      if (!this.m_isDestroyed)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (!this.IsWorking)
        return;
      this.InitializeSubparts();
      if (!this.LoadIsDestroyed.HasValue)
        return;
      if (this.LoadIsDestroyed.Value)
      {
        this.m_isDestroyed = true;
        this.m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((double) this.LoadTimeToRestore);
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
        foreach (MyEntitySubpart subpart in this.m_subparts)
          this.HideSubpart(subpart);
      }
      else
      {
        this.RestoreAllSubparts();
        foreach (MyObjectBuilder_TargetDummyBlock.MySubpartSavedState subpartSavedState in this.LoadSubpartState)
        {
          foreach (KeyValuePair<long, MyTargetDummyBlock.MySubpartState> subpartState in this.m_subpartStates)
          {
            if (subpartState.Value.Name == subpartSavedState.SubpartName)
            {
              subpartState.Value.IntegrityCurrent = subpartSavedState.SubpartHealth;
              break;
            }
          }
        }
        if (!Sandbox.Game.Multiplayer.Sync.IsServer)
          this.ResetAllSubparts();
      }
      this.LoadIsDestroyed = new bool?();
      this.LoadTimeToRestore = 0;
      this.LoadSubpartState = (List<MyObjectBuilder_TargetDummyBlock.MySubpartSavedState>) null;
    }

    public override void RefreshModels(string modelPath, string modelCollisionPath)
    {
      base.RefreshModels(modelPath, modelCollisionPath);
      if (!this.m_areSubpartsInitialized)
        return;
      this.ReinitializeSubpartsOnModelChange();
    }

    private void ReinitializeSubpartsOnModelChange()
    {
      Dictionary<string, float> dictionary = new Dictionary<string, float>();
      foreach (KeyValuePair<long, MyTargetDummyBlock.MySubpartState> subpartState in this.m_subpartStates)
        dictionary.Add(subpartState.Value.Name, subpartState.Value.IntegrityCurrent);
      bool isDestroyed = this.m_isDestroyed;
      MyTimeSpan restorationTime = this.m_restorationTime;
      this.UninitializeSubparts();
      this.InitializeSubparts();
      this.DisableStateSyncing();
      foreach (KeyValuePair<long, MyTargetDummyBlock.MySubpartState> subpartState in this.m_subpartStates)
      {
        float num;
        if (dictionary.TryGetValue(subpartState.Value.Name, out num))
          subpartState.Value.IntegrityCurrent = num;
      }
      this.EnableStateSyncing();
      this.m_isDestroyed = isDestroyed;
      this.m_restorationTime = restorationTime;
      this.ResetAllSubparts();
    }

    private void InitializeSubparts()
    {
      if (this.m_areSubpartsInitialized)
        return;
      this.m_areSubpartsInitialized = true;
      this.GatherSubparts((MyEntity) this);
      this.BuildSubpartStates();
      foreach (MyEntitySubpart subpart in this.m_subparts)
      {
        this.InitializeSubpart((MyEntity) subpart);
        if (subpart.Physics != null && !subpart.Physics.Enabled)
          subpart.Physics.Activate();
      }
      this.m_isDestroyed = true;
      bool flag = true;
      if (MySession.Static.CreativeMode && this.m_restorationTime <= MyTimeSpan.Zero)
      {
        this.m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds);
        flag = false;
      }
      else
        this.m_restorationTime = MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds) + MyTimeSpan.FromSeconds((double) (float) this.m_restorationDelay);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
      if (!flag)
        return;
      foreach (MyEntitySubpart subpart in this.m_subparts)
        this.HideSubpart(subpart);
    }

    private void UninitializeSubparts()
    {
      if (!this.m_areSubpartsInitialized)
        return;
      this.m_areSubpartsInitialized = false;
      this.m_subparts.Clear();
      this.m_subpartNames.Clear();
      this.m_subpartStates.Clear();
    }

    private void GatherSubparts(MyEntity ent)
    {
      Dictionary<string, MyEntitySubpart>.Enumerator enumerator = ent.Subparts.GetEnumerator();
      while (enumerator.MoveNext())
      {
        KeyValuePair<string, MyEntitySubpart> current = enumerator.Current;
        this.m_subparts.Add(current.Value);
        this.m_subpartNames.Add(current.Key);
        this.GatherSubparts((MyEntity) current.Value);
      }
    }

    private void BuildSubpartStates()
    {
      MyTargetDummyBlockDefinition definition = this.Definition;
      if (definition == null)
        return;
      for (int index = 0; index < this.m_subparts.Count; ++index)
      {
        MyEntitySubpart subpart = this.m_subparts[index];
        string subpartName = this.m_subpartNames[index];
        float num = 1f;
        bool flag = false;
        if (definition.SubpartDefinitions.ContainsKey(subpartName))
        {
          MyTargetDummyBlockDefinition.MyDummySubpartDescription subpartDefinition = definition.SubpartDefinitions[subpartName];
          num = subpartDefinition.Health;
          flag = subpartDefinition.IsCritical;
        }
        MyTargetDummyBlock.MySubpartState mySubpartState = new MyTargetDummyBlock.MySubpartState()
        {
          Subpart = subpart,
          Block = this,
          Name = subpartName,
          IsCritical = flag,
          IntegrityCurrent = 0.0f,
          IntegrityMax = num
        };
        this.m_subpartStates.Add(subpart.EntityId, mySubpartState);
      }
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      this.CheckInventoryFill();
      if (!this.m_isDestroyed)
        return;
      if (!(bool) this.m_enableRestoration)
      {
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
      }
      else
      {
        if (!(this.m_restorationTime < MyTimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds)) || !this.IsFunctional)
          return;
        if (MySession.Static.CreativeMode)
        {
          this.RestoreAllSubparts();
        }
        else
        {
          MyTargetDummyBlockDefinition definition = this.Definition;
          MyInventoryBase inventoryBase = this.GetInventoryBase(0);
          if (inventoryBase.GetItemAmount(definition.ConstructionItem).ToIntSafe() < definition.ConstructionItemAmount)
            return;
          MyFixedPoint constructionItemAmount = (MyFixedPoint) definition.ConstructionItemAmount;
          inventoryBase.RemoveItemsOfType(constructionItemAmount, definition.ConstructionItem);
          this.RestoreAllSubparts();
        }
      }
    }

    private void CheckInventoryFill()
    {
      MyInventory inventoryBase = this.GetInventoryBase(0) as MyInventory;
      MyTargetDummyBlockDefinition definition = this.Definition;
      if ((double) inventoryBase.VolumeFillFactor <= (double) definition.MinFillFactor)
        this.m_currentFillThreshold = definition.MaxFillFactor;
      if ((double) this.m_currentFillThreshold > (double) inventoryBase.VolumeFillFactor)
        this.CubeGrid.GridSystems.ConveyorSystem.PullItem(definition.ConstructionItem, new MyFixedPoint?((MyFixedPoint) (MyTargetDummyBlock.PULL_AMOUNT_CONSTANT * (float) definition.ConstructionItemAmount)), (IMyConveyorEndpointBlock) this, inventoryBase, false, false);
      else if (!this.m_isDestroyed)
        this.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
      if ((double) inventoryBase.VolumeFillFactor < (double) definition.MaxFillFactor)
        return;
      this.m_currentFillThreshold = definition.MinFillFactor;
    }

    private void InitializeSubpart(MyEntity subpart)
    {
      bool flag = !Sandbox.Game.Multiplayer.Sync.IsServer;
      if (subpart.Physics == null && subpart.ModelCollision.HavokCollisionShapes != null && subpart.ModelCollision.HavokCollisionShapes.Length != 0)
      {
        HkShape havokCollisionShape = subpart.ModelCollision.HavokCollisionShapes[0];
        MyPhysicsBody myPhysicsBody = new MyPhysicsBody((VRage.ModAPI.IMyEntity) subpart, RigidBodyFlag.RBF_KINEMATIC | RigidBodyFlag.RBF_UNLOCKED_SPEEDS);
        subpart.Physics = (MyPhysicsComponentBase) myPhysicsBody;
        Vector3 zero = Vector3.Zero;
        HkMassProperties volumeMassProperties = HkInertiaTensorComputer.ComputeBoxVolumeMassProperties(subpart.PositionComp.LocalAABB.HalfExtents, 0.0f);
        subpart.Physics.IsPhantom = false;
        volumeMassProperties.Volume = subpart.PositionComp.LocalAABB.Volume();
        subpart.GetPhysicsBody().CreateFromCollisionObject(havokCollisionShape, zero, subpart.WorldMatrix, new HkMassProperties?(volumeMassProperties), 6);
        ((MyPhysicsBody) subpart.Physics).IsSubpart = true;
      }
      if (subpart.Physics == null)
        return;
      if (!flag)
        subpart.Physics.Enabled = true;
      else
        subpart.Physics.Enabled = true;
    }

    public override bool ReceivedDamage(
      float damage,
      MyStringHash damageType,
      long attackerId,
      long realHitEntityId)
    {
      bool flag = base.ReceivedDamage(damage, damageType, attackerId, realHitEntityId);
      if (this.m_subpartStates.ContainsKey(realHitEntityId))
      {
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
        {
          this.ActivateActionHit();
          this.m_subpartStates[realHitEntityId].Damage(damage);
        }
        flag = false;
      }
      return flag;
    }

    private void SubpartDestroyed(MyEntitySubpart subpart)
    {
      this.HideSubpart(subpart);
      if (this.IsDestroyed)
        return;
      bool flag = false;
      foreach (KeyValuePair<long, MyTargetDummyBlock.MySubpartState> subpartState in this.m_subpartStates)
      {
        if ((double) subpartState.Value.IntegrityCurrent > 0.0)
        {
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      this.IsDestroyed = true;
    }

    private void SubpartRestored(MyEntitySubpart subpart)
    {
      int num = subpart.Render.Visible ? 1 : 0;
      subpart.Render.Visible = true;
      if (subpart.Physics != null)
        subpart.Physics.Enabled = true;
      if (num != 0)
        return;
      MyTargetDummyBlockDefinition definition = this.Definition;
      MatrixD effectMatrix = (MatrixD) ref subpart.PositionComp.LocalMatrixRef;
      Vector3D position = subpart.PositionComp.GetPosition();
      MyParticleEffect effect;
      MyParticlesManager.TryCreateParticleEffect(definition.RegenerationEffectName, ref effectMatrix, ref position, this.Render.GetRenderObjectID(), out effect);
      if (effect != null)
        effect.UserBirthMultiplier = definition.RegenerationEffectMultiplier;
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.PlaySound(definition.RegenerationSound, force3D: new bool?(true));
    }

    private void HideSubpart(MyEntitySubpart subpart)
    {
      int num = !subpart.Render.Visible ? 1 : 0;
      subpart.Render.Visible = false;
      if (subpart.Physics != null)
        subpart.Physics.Enabled = false;
      if (num != 0)
        return;
      MyTargetDummyBlockDefinition definition = this.Definition;
      MatrixD effectMatrix = (MatrixD) ref subpart.PositionComp.LocalMatrixRef;
      Vector3D position = subpart.PositionComp.GetPosition();
      MyParticleEffect effect;
      MyParticlesManager.TryCreateParticleEffect(definition.DestructionEffectName, ref effectMatrix, ref position, this.Render.GetRenderObjectID(), out effect);
      if (effect != null)
        effect.UserBirthMultiplier = definition.DestructionEffectMultiplier;
      if (this.m_soundEmitter == null)
        return;
      this.m_soundEmitter.PlaySound(definition.DestructionSound, force3D: new bool?(true));
    }

    private void RestoreAllSubparts()
    {
      this.DisableStateSyncing();
      foreach (KeyValuePair<long, MyTargetDummyBlock.MySubpartState> subpartState in this.m_subpartStates)
        subpartState.Value.IntegrityCurrent = subpartState.Value.IntegrityMax;
      this.EnableStateSyncing();
      if (Sandbox.Game.Multiplayer.Sync.IsServer)
        this.SynchronizeSubparts();
      this.IsDestroyed = false;
    }

    private void DestroyAllSubparts()
    {
      if (this.IsDestroyed)
        return;
      this.IsDestroyed = true;
      this.DisableStateSyncing();
      foreach (KeyValuePair<long, MyTargetDummyBlock.MySubpartState> subpartState in this.m_subpartStates)
        subpartState.Value.IntegrityCurrent = 0.0f;
      this.EnableStateSyncing();
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      this.SynchronizeSubparts();
    }

    private void EnableStateSyncing() => this.m_canSyncState = true;

    private void DisableStateSyncing() => this.m_canSyncState = false;

    private void ResetAllSubparts()
    {
      foreach (KeyValuePair<long, MyTargetDummyBlock.MySubpartState> subpartState in this.m_subpartStates)
      {
        bool flag = (double) subpartState.Value.IntegrityCurrent > 0.0;
        MyEntitySubpart subpart = subpartState.Value.Subpart;
        subpart.Render.Visible = flag;
        if (subpart.Physics != null)
          subpart.Physics.Enabled = flag;
      }
    }

    private void SynchronizeSubparts()
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      List<float> floatList = new List<float>();
      foreach (MyEntitySubpart subpart in this.m_subparts)
        floatList.Add(this.m_subpartStates[subpart.EntityId].IntegrityCurrent);
      Sandbox.Engine.Multiplayer.MyMultiplayer.RaiseEvent<MyTargetDummyBlock, List<float>>(this, (Func<MyTargetDummyBlock, Action<List<float>>>) (x => new Action<List<float>>(x.SendStates)), floatList);
    }

    [Event(null, 940)]
    [Reliable]
    [Broadcast]
    private void SendStates(List<float> subpartHealths)
    {
      for (int index = 0; index < this.m_subparts.Count; ++index)
      {
        if ((double) subpartHealths[index] > 0.0 && !this.m_subparts[index].Render.Visible)
          this.SubpartRestored(this.m_subparts[index]);
        else if ((double) subpartHealths[index] <= 0.0 && this.m_subparts[index].Render.Visible)
          this.HideSubpart(this.m_subparts[index]);
      }
    }

    VRage.Game.ModAPI.Ingame.IMyInventory IMyInventoryOwner.GetInventory(
      int index)
    {
      return (VRage.Game.ModAPI.Ingame.IMyInventory) MyEntityExtensions.GetInventory(this, index);
    }

    public void InitializeConveyorEndpoint() => this.m_endpoint = new MyMultilineConveyorEndpoint((MyCubeBlock) this);

    public bool AllowSelfPulling() => false;

    public PullInformation GetPullInformation() => new PullInformation()
    {
      Inventory = MyEntityExtensions.GetInventory(this),
      OwnerID = this.OwnerId,
      Constraint = new MyInventoryConstraint("Empty constraint")
    };

    public PullInformation GetPushInformation() => (PullInformation) null;

    [SpecialName]
    int IMyInventoryOwner.get_InventoryCount() => this.InventoryCount;

    [SpecialName]
    bool IMyInventoryOwner.get_HasInventory() => this.HasInventory;

    private class MySubpartState
    {
      public string Name;
      public bool IsCritical;
      public MyEntitySubpart Subpart;
      public MyTargetDummyBlock Block;
      private float m_integrityCurrent;
      public float IntegrityMax = 1f;

      public float IntegrityCurrent
      {
        get => this.m_integrityCurrent;
        set
        {
          float num = MathHelper.Clamp(value, 0.0f, this.IntegrityMax);
          if ((double) num == (double) this.m_integrityCurrent)
            return;
          float integrityCurrent = this.m_integrityCurrent;
          this.m_integrityCurrent = num;
          if (!Sandbox.Game.Multiplayer.Sync.IsServer)
            return;
          if ((double) integrityCurrent > 0.0 && (double) num <= 0.0)
          {
            if (this.IsCritical)
            {
              this.Block.SubpartDestroyed(this.Subpart);
              this.Block.DestroyAllSubparts();
            }
            else
              this.Block.SubpartDestroyed(this.Subpart);
            if (!this.Block.CanSyncState)
              return;
            this.Block.SynchronizeSubparts();
          }
          else
          {
            if ((double) integrityCurrent > 0.0 || (double) num <= 0.0)
              return;
            this.Block.SubpartRestored(this.Subpart);
            if (!this.Block.CanSyncState)
              return;
            this.Block.SynchronizeSubparts();
          }
        }
      }

      public void Damage(float damage) => this.IntegrityCurrent -= damage;
    }

    protected sealed class SendToolbarItemChanged\u003C\u003ESandbox_Game_Entities_Blocks_ToolbarItem\u0023System_Int32 : ICallSite<MyTargetDummyBlock, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTargetDummyBlock @this,
        in ToolbarItem sentItem,
        in int index,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SendToolbarItemChanged(sentItem, index);
      }
    }

    protected sealed class SendStates\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_Single\u003E : ICallSite<MyTargetDummyBlock, List<float>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyTargetDummyBlock @this,
        in List<float> subpartHealths,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SendStates(subpartHealths);
      }
    }

    protected class m_useConveyorSystem\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyTargetDummyBlock) obj0).m_useConveyorSystem = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_restorationDelay\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<float, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<float, SyncDirection.BothWays>(obj1, obj2));
        ((MyTargetDummyBlock) obj0).m_restorationDelay = (VRage.Sync.Sync<float, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_enableRestoration\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyTargetDummyBlock) obj0).m_enableRestoration = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyTargetDummyBlock\u003C\u003EActor : IActivator, IActivator<MyTargetDummyBlock>
    {
      object IActivator.CreateInstance() => (object) new MyTargetDummyBlock();

      MyTargetDummyBlock IActivator<MyTargetDummyBlock>.CreateInstance() => new MyTargetDummyBlock();
    }
  }
}
