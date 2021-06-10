// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Entities.Blocks.MyLCDPanelsBlock
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_LCDPanelsBlock))]
  public class MyLCDPanelsBlock : MyFunctionalBlock, IMyTextSurfaceProvider, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner
  {
    private MyMultiTextPanelComponent m_multiPanel;
    private MyGuiScreenTextPanel m_textBox;
    private bool m_isTextPanelOpen;

    public MyLCDPanelsBlock()
    {
      this.CreateTerminalControls();
      this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);
    }

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyLCDPanelsBlock>())
        return;
      base.CreateTerminalControls();
      MyMultiTextPanelComponent.CreateTerminalControls<MyLCDPanelsBlock>();
    }

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      MyLCDPanelsBlockDefinition blockDefinition = this.BlockDefinition;
      MyResourceSinkComponent resourceSinkComponent = new MyResourceSinkComponent();
      resourceSinkComponent.Init(blockDefinition.ResourceSinkGroup, blockDefinition.RequiredPowerInput, (Func<float>) (() => !this.Enabled || !this.IsFunctional ? 0.0f : this.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId)));
      resourceSinkComponent.IsPoweredChanged += new Action(this.PowerReceiver_IsPoweredChanged);
      this.ResourceSink = resourceSinkComponent;
      base.Init(objectBuilder, cubeGrid);
      this.ResourceSink.Update();
      this.SlimBlock.ComponentStack.IsFunctionalChanged += new Action(this.ComponentStack_IsFunctionalChanged);
      MyObjectBuilder_LCDPanelsBlock builderLcdPanelsBlock = (MyObjectBuilder_LCDPanelsBlock) objectBuilder;
      if (blockDefinition.ScreenAreas == null || blockDefinition.ScreenAreas.Count <= 0)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
      this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, blockDefinition.ScreenAreas, builderLcdPanelsBlock.TextPanels);
      this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
    }

    protected override void Closing()
    {
      base.Closing();
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.SetRender((MyRenderComponentScreenAreas) null);
    }

    public override void OnModelChange()
    {
      base.OnModelChange();
      if (this.m_multiPanel != null)
        this.m_multiPanel.Reset();
      if (this.ResourceSink == null)
        return;
      this.UpdateScreen();
    }

    public MyLCDPanelsBlockDefinition BlockDefinition => (MyLCDPanelsBlockDefinition) base.BlockDefinition;

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_LCDPanelsBlock builderCubeBlock = (MyObjectBuilder_LCDPanelsBlock) base.GetObjectBuilderCubeBlock(copy);
      if (this.m_multiPanel != null)
        builderCubeBlock.TextPanels = this.m_multiPanel.Serialize();
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
    }

    private void PowerReceiver_IsPoweredChanged()
    {
      this.UpdateIsWorking();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override bool CheckIsWorking() => base.CheckIsWorking() && this.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId);

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.UpdateAfterSimulation(this.IsWorking);
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      this.UpdateScreen();
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.AddToScene();
    }

    private void UpdateScreen() => this.m_multiPanel?.UpdateScreen(this.IsWorking);

    private void ComponentStack_IsFunctionalChanged() => this.ResourceSink.Update();

    protected override void OnEnabledChanged()
    {
      this.ResourceSink.Update();
      base.OnEnabledChanged();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    int IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    IMyTextSurface IMyTextSurfaceProvider.GetSurface(int index) => this.m_multiPanel == null ? (IMyTextSurface) null : this.m_multiPanel.GetSurface(index);

    public override void UpdateVisual()
    {
      base.UpdateVisual();
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    protected override void OnStartWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected override void OnStopWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => MyMultiplayer.RaiseEvent<MyLCDPanelsBlock, int, int[]>(this, (Func<MyLCDPanelsBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    [Event(null, 222)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection) => this.m_multiPanel?.RemoveItems(panelIndex, selection);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => MyMultiplayer.RaiseEvent<MyLCDPanelsBlock, int, int[]>(this, (Func<MyLCDPanelsBlock, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    [Event(null, 233)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int panelIndex, int[] selection) => this.m_multiPanel?.SelectItems(panelIndex, selection);

    private void ChangeTextRequest(int panelIndex, string text) => MyMultiplayer.RaiseEvent<MyLCDPanelsBlock, int, string>(this, (Func<MyLCDPanelsBlock, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 244)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyLCDPanelsBlock, int, MySerializableSpriteCollection>(this, (Func<MyLCDPanelsBlock, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 258)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

    void IMyMultiTextPanelComponentOwner.SelectPanel(
      List<MyGuiControlListbox.Item> panelItems)
    {
      if (this.m_multiPanel != null)
        this.m_multiPanel.SelectPanel((int) panelItems[0].UserData);
      this.RaisePropertiesChanged();
    }

    MyMultiTextPanelComponent IMyMultiTextPanelComponentOwner.MultiTextPanel => this.m_multiPanel;

    public MyTextPanelComponent PanelComponent => this.m_multiPanel == null ? (MyTextPanelComponent) null : this.m_multiPanel.PanelComponent;

    private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
    {
      if (this.CubeGrid.IsPreview || !this.CubeGrid.SyncFlag)
      {
        this.PanelComponent.Text = description;
      }
      else
      {
        if (description.CompareTo(this.PanelComponent.Text) == 0)
          return;
        MyMultiplayer.RaiseEvent<MyLCDPanelsBlock, string, bool>(this, (Func<MyLCDPanelsBlock, Action<string, bool>>) (x => new Action<string, bool>(x.OnChangeDescription)), description.ToString(), isPublic);
      }
    }

    [Event(null, 301)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void OnChangeDescription(string description, bool isPublic)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Clear().Append(description);
      this.PanelComponent.Text = stringBuilder;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public void OpenWindow(bool isEditable, bool sync, bool isPublic)
    {
      if (sync)
      {
        this.SendChangeOpenMessage(true, isEditable, Sync.MyId, isPublic);
      }
      else
      {
        this.CreateTextBox(isEditable, new StringBuilder(this.PanelComponent.Text.ToString()), isPublic);
        MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
        MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = (MyGuiScreenBase) this.m_textBox);
      }
    }

    private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0, bool isPublic = false) => MyMultiplayer.RaiseEvent<MyLCDPanelsBlock, bool, bool, ulong, bool>(this, (Func<MyLCDPanelsBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenRequest)), isOpen, editable, user, isPublic);

    [Event(null, 328)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      if (((!Sync.IsServer ? 0 : (this.IsTextPanelOpen ? 1 : 0)) & (isOpen ? 1 : 0)) != 0)
        return;
      this.OnChangeOpen(isOpen, editable, user, isPublic);
      MyMultiplayer.RaiseEvent<MyLCDPanelsBlock, bool, bool, ulong, bool>(this, (Func<MyLCDPanelsBlock, Action<bool, bool, ulong, bool>>) (x => new Action<bool, bool, ulong, bool>(x.OnChangeOpenSuccess)), isOpen, editable, user, isPublic);
    }

    [Event(null, 339)]
    [Reliable]
    [Broadcast]
    private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic) => this.OnChangeOpen(isOpen, editable, user, isPublic);

    private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
    {
      this.IsTextPanelOpen = isOpen;
      if (((Sandbox.Engine.Platform.Game.IsDedicated ? 0 : ((long) user == (long) Sync.MyId ? 1 : 0)) & (isOpen ? 1 : 0)) == 0)
        return;
      this.OpenWindow(editable, false, isPublic);
    }

    private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
    {
      string displayNameText = this.DisplayNameText;
      string displayName = this.PanelComponent.DisplayName;
      string description1 = description.ToString();
      bool flag = isEditable;
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback = new Action<VRage.Game.ModAPI.ResultEnum>(this.OnClosedPanelTextBox);
      int num = flag ? 1 : 0;
      this.m_textBox = new MyGuiScreenTextPanel(displayNameText, "", displayName, description1, resultCallback, editable: (num != 0));
    }

    public void OnClosedPanelTextBox(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_textBox == null)
        return;
      if (this.m_textBox.Description.Text.Length > 100000)
      {
        Action<MyGuiScreenMessageBox.ResultEnum> callback = new Action<MyGuiScreenMessageBox.ResultEnum>(this.OnClosedPanelMessageBox);
        MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText), callback: callback));
      }
      else
        this.CloseWindow(true);
    }

    public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
    {
      if (result == MyGuiScreenMessageBox.ResultEnum.YES)
      {
        this.m_textBox.Description.Text.Remove(100000, this.m_textBox.Description.Text.Length - 100000);
        this.CloseWindow(true);
      }
      else
      {
        this.CreateTextBox(true, this.m_textBox.Description.Text, true);
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_textBox);
      }
    }

    private void CloseWindow(bool isPublic)
    {
      MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
      MyGuiScreenGamePlay.TmpGameplayScreenHolder = (MyGuiScreenBase) null;
      foreach (MySlimBlock cubeBlock in this.CubeGrid.CubeBlocks)
      {
        if (cubeBlock.FatBlock != null && cubeBlock.FatBlock.EntityId == this.EntityId)
        {
          this.SendChangeDescriptionMessage(this.m_textBox.Description.Text, isPublic);
          this.SendChangeOpenMessage(false);
          break;
        }
      }
    }

    public bool IsTextPanelOpen
    {
      get => this.m_isTextPanelOpen;
      set
      {
        if (this.m_isTextPanelOpen == value)
          return;
        this.m_isTextPanelOpen = value;
        this.RaisePropertiesChanged();
      }
    }

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyLCDPanelsBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLCDPanelsBlock @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSelectedImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyLCDPanelsBlock, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLCDPanelsBlock @this,
        in int panelIndex,
        in int[] selection,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnSelectImageRequest(panelIndex, selection);
      }
    }

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MyLCDPanelsBlock, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLCDPanelsBlock @this,
        in int panelIndex,
        in string text,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeTextRequest(panelIndex, text);
      }
    }

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyLCDPanelsBlock, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLCDPanelsBlock @this,
        in int panelIndex,
        in MySerializableSpriteCollection sprites,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnUpdateSpriteCollection(panelIndex, sprites);
      }
    }

    protected sealed class OnChangeDescription\u003C\u003ESystem_String\u0023System_Boolean : ICallSite<MyLCDPanelsBlock, string, bool, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLCDPanelsBlock @this,
        in string description,
        in bool isPublic,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeDescription(description, isPublic);
      }
    }

    protected sealed class OnChangeOpenRequest\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyLCDPanelsBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLCDPanelsBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenRequest(isOpen, editable, user, isPublic);
      }
    }

    protected sealed class OnChangeOpenSuccess\u003C\u003ESystem_Boolean\u0023System_Boolean\u0023System_UInt64\u0023System_Boolean : ICallSite<MyLCDPanelsBlock, bool, bool, ulong, bool, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyLCDPanelsBlock @this,
        in bool isOpen,
        in bool editable,
        in ulong user,
        in bool isPublic,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnChangeOpenSuccess(isOpen, editable, user, isPublic);
      }
    }

    private class Sandbox_Game_Entities_Blocks_MyLCDPanelsBlock\u003C\u003EActor : IActivator, IActivator<MyLCDPanelsBlock>
    {
      object IActivator.CreateInstance() => (object) new MyLCDPanelsBlock();

      MyLCDPanelsBlock IActivator<MyLCDPanelsBlock>.CreateInstance() => new MyLCDPanelsBlock();
    }
  }
}
