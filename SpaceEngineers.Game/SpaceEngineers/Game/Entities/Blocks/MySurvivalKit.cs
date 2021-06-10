// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MySurvivalKit
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using SpaceEngineers.Game.EntityComponents.GameLogic;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Network;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_SurvivalKit))]
  public class MySurvivalKit : MyAssembler, IMyLifeSupportingBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, IMyRechargeSocketOwner, IMySpawnBlock
  {
    private readonly List<MyTextPanelComponent> m_panels = new List<MyTextPanelComponent>();
    private MyLifeSupportingComponent m_lifeSupportingComponent;

    public MySurvivalKit()
    {
      this.SpawnName = new StringBuilder();
      this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);
    }

    public MySurvivalKitDefinition BlockDefinition => (MySurvivalKitDefinition) base.BlockDefinition;

    public override bool SupportsAdvancedFunctions => false;

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MySurvivalKit>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlTextbox<MySurvivalKit> terminalControlTextbox = new MyTerminalControlTextbox<MySurvivalKit>("SpawnName", MySpaceTexts.SurvivalKit_SpawnNameLabel, MySpaceTexts.SurvivalKit_SpawnNameToolTip);
      terminalControlTextbox.Getter = (MyTerminalControlTextbox<MySurvivalKit>.GetterDelegate) (x => x.SpawnName);
      terminalControlTextbox.Setter = (MyTerminalControlTextbox<MySurvivalKit>.SetterDelegate) ((x, v) => x.SetSpawnName(v));
      terminalControlTextbox.SupportsMultipleBlocks = false;
      MyTerminalControlFactory.AddControl<MySurvivalKit>((MyTerminalControl<MySurvivalKit>) terminalControlTextbox);
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_SurvivalKit builderCubeBlock = (MyObjectBuilder_SurvivalKit) base.GetObjectBuilderCubeBlock(copy);
      builderCubeBlock.SpawnName = this.SpawnName.ToString();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      MyObjectBuilder_SurvivalKit builderSurvivalKit = objectBuilder as MyObjectBuilder_SurvivalKit;
      this.SpawnName.Clear();
      if (builderSurvivalKit.SpawnName != null)
        this.SpawnName.Append(builderSurvivalKit.SpawnName);
      this.m_lifeSupportingComponent = new MyLifeSupportingComponent((MyEntity) this, new MySoundPair(this.BlockDefinition.ProgressSound));
      this.Components.Add<MyLifeSupportingComponent>(this.m_lifeSupportingComponent);
      if (this.CubeGrid.CreatePhysics)
        this.Components.Add<MyEntityRespawnComponentBase>((MyEntityRespawnComponentBase) new MyRespawnComponent());
      this.ResourceSink.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      List<ScreenArea> screenAreas = this.BlockDefinition.ScreenAreas;
      if (screenAreas == null || screenAreas.Count <= 0)
        return;
      for (int index = 0; index < screenAreas.Count; ++index)
      {
        MyTextPanelComponent textPanelComponent = new MyTextPanelComponent(index, (MyTerminalBlock) this, screenAreas[index].Name, screenAreas[index].DisplayName, screenAreas[index].TextureResolution);
        this.m_panels.Add(textPanelComponent);
        this.SyncType.Append((object) textPanelComponent);
        textPanelComponent.Init();
      }
    }

    private void PowerReceiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      this.m_lifeSupportingComponent.Update10();
    }

    public override void UpdateSoundEmitters()
    {
      base.UpdateSoundEmitters();
      this.m_lifeSupportingComponent.UpdateSoundEmitters();
    }

    MyRechargeSocket IMyRechargeSocketOwner.RechargeSocket => this.m_lifeSupportingComponent.RechargeSocket;

    bool IMyLifeSupportingBlock.RefuelAllowed => true;

    bool IMyLifeSupportingBlock.HealingAllowed => true;

    MyLifeSupportingBlockType IMyLifeSupportingBlock.BlockType => MyLifeSupportingBlockType.SurvivalKit;

    void IMyLifeSupportingBlock.ShowTerminal(MyCharacter user) => MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, (MyEntity) this);

    void IMyLifeSupportingBlock.BroadcastSupportRequest(MyCharacter user) => MyMultiplayer.RaiseEvent<MySurvivalKit, long>(this, (Func<MySurvivalKit, Action<long>>) (x => new Action<long>(x.RequestSupport)), user.EntityId);

    [Event(null, 163)]
    [Reliable]
    [Server(ValidationType.Access)]
    [Broadcast]
    private void RequestSupport(long userId)
    {
      if (!this.GetUserRelationToOwner(MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value, 0)).IsFriendly() && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
        return;
      MyCharacter entity;
      Sandbox.Game.Entities.MyEntities.TryGetEntityById<MyCharacter>(userId, out entity);
      if (entity == null)
        return;
      this.m_lifeSupportingComponent.ProvideSupport(entity);
    }

    public override int GUIPriority => 600;

    public override bool AllowSelfPulling() => true;

    public StringBuilder SpawnName { get; private set; }

    string IMySpawnBlock.SpawnName => this.SpawnName.ToString();

    private void SetSpawnName(StringBuilder text)
    {
      if (!this.SpawnName.CompareUpdate(text))
        return;
      MyMultiplayer.RaiseEvent<MySurvivalKit, string>(this, (Func<MySurvivalKit, Action<string>>) (x => new Action<string>(x.SetSpawnTextEvent)), text.ToString());
    }

    [Event(null, 206)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [BroadcastExcept]
    protected void SetSpawnTextEvent(string text) => this.SpawnName.CompareUpdate(text);

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateScreen();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_panels.Count > 0)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      foreach (MyTextPanelComponent panel in this.m_panels)
      {
        panel.SetRender((MyRenderComponentScreenAreas) this.Render);
        ((MyRenderComponentScreenAreas) this.Render).AddScreenArea(this.Render.RenderObjectIDs, panel.Name);
      }
    }

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public void UpdateScreen()
    {
      if (!this.CheckIsWorking())
      {
        for (int index = 0; index < this.m_panels.Count; ++index)
          ((MyRenderComponentScreenAreas) this.Render).ChangeTexture(index, this.m_panels[index].GetPathForID("Offline"));
      }
      else
      {
        for (int area = 0; area < this.m_panels.Count; ++area)
          ((MyRenderComponentScreenAreas) this.Render).ChangeTexture(area, (string) null);
      }
    }

    public override float GetEfficiencyMultiplierForBlueprint(
      MyBlueprintDefinitionBase targetBlueprint)
    {
      foreach (MyBlueprintDefinitionBase.Item prerequisite in targetBlueprint.Prerequisites)
      {
        if (prerequisite.Id.TypeId == typeof (MyObjectBuilder_Ore))
          return 1f;
      }
      return base.GetEfficiencyMultiplierForBlueprint(targetBlueprint);
    }

    protected override void OnStartWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected override void OnStopWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected sealed class RequestSupport\u003C\u003ESystem_Int64 : ICallSite<MySurvivalKit, long, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySurvivalKit @this,
        in long userId,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.RequestSupport(userId);
      }
    }

    protected sealed class SetSpawnTextEvent\u003C\u003ESystem_String : ICallSite<MySurvivalKit, string, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MySurvivalKit @this,
        in string text,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.SetSpawnTextEvent(text);
      }
    }
  }
}
