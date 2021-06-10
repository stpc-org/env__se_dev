// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.Entities.Blocks.MyJukebox
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.GUI.TextPanel;
using VRage.ModAPI;
using VRage.Network;
using VRage.Serialization;
using VRage.Sync;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
  [MyCubeBlockType(typeof (MyObjectBuilder_Jukebox))]
  public class MyJukebox : MySoundBlock, IMyTextSurfaceProvider, IMyTextPanelComponentOwner
  {
    private readonly VRage.Sync.Sync<bool, SyncDirection.BothWays> m_isJukeboxPlaying;
    private VRage.Sync.Sync<int, SyncDirection.BothWays> m_currentSound;
    private MyMultiTextPanelComponent m_multiPanel;
    private List<string> m_selectedSounds = new List<string>();
    private List<string> m_soundBlockListSelection = new List<string>();
    private List<string> m_jukeboxListSelection = new List<string>();
    private string m_localCueIdString;

    int IMyTextSurfaceProvider.SurfaceCount => this.m_multiPanel == null ? 0 : this.m_multiPanel.SurfaceCount;

    IMyTextSurface IMyTextSurfaceProvider.GetSurface(int index) => this.m_multiPanel == null ? (IMyTextSurface) null : this.m_multiPanel.GetSurface(index);

    public MyJukeboxDefinition BlockDefinition => (MyJukeboxDefinition) ((MyCubeBlock) this).BlockDefinition;

    public bool IsJukeboxPlaying => (bool) this.m_isJukeboxPlaying;

    public MyJukebox() => this.Render = (MyRenderComponentBase) new MyRenderComponentScreenAreas((MyEntity) this);

    public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
    {
      base.Init(objectBuilder, cubeGrid);
      MyObjectBuilder_Jukebox objectBuilderJukebox = objectBuilder as MyObjectBuilder_Jukebox;
      if (this.BlockDefinition.ScreenAreas != null && this.BlockDefinition.ScreenAreas.Count > 0)
      {
        this.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME;
        this.m_multiPanel = new MyMultiTextPanelComponent((MyTerminalBlock) this, this.BlockDefinition.ScreenAreas, objectBuilderJukebox.TextPanels);
        this.m_multiPanel.Init(new Action<int, int[]>(this.SendAddImagesToSelectionRequest), new Action<int, int[]>(this.SendRemoveSelectedImageRequest), new Action<int, string>(this.ChangeTextRequest), new Action<int, MySerializableSpriteCollection>(this.UpdateSpriteCollection));
      }
      if (objectBuilderJukebox.SelectedSounds != null)
        this.m_selectedSounds = objectBuilderJukebox.SelectedSounds.ToList<string>();
      this.m_isJukeboxPlaying.SetLocalValue(objectBuilderJukebox.IsJukeboxPlaying);
      this.m_currentSound.SetLocalValue(objectBuilderJukebox.CurrentSound);
      if ((bool) this.m_isJukeboxPlaying)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      this.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
    }

    public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(
      bool copy = false)
    {
      MyObjectBuilder_Jukebox builderCubeBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_Jukebox;
      if (this.m_multiPanel != null)
        builderCubeBlock.TextPanels = this.m_multiPanel.Serialize();
      builderCubeBlock.SelectedSounds = this.m_selectedSounds.ToList<string>();
      builderCubeBlock.IsJukeboxPlaying = (bool) this.m_isJukeboxPlaying;
      builderCubeBlock.CurrentSound = (int) this.m_currentSound;
      return (MyObjectBuilder_CubeBlock) builderCubeBlock;
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
      this.UpdateScreen();
    }

    public override void UpdateAfterSimulation10()
    {
      base.UpdateAfterSimulation10();
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated)
        return;
      if (this.m_multiPanel != null)
        this.m_multiPanel.UpdateAfterSimulation(this.IsWorking);
      if (!MyAudio.Static.CanPlay || this.m_soundEmitter.IsPlaying || (!this.IsJukeboxPlaying || !this.IsWorking) || !this.IsFunctional)
        return;
      this.PlayNextLocally();
    }

    public override void UpdateAfterSimulation100()
    {
      base.UpdateAfterSimulation100();
      if (Sandbox.Game.Multiplayer.Sync.IsDedicated || MySession.Static.LocalCharacter == null || Vector3D.DistanceSquared(this.PositionComp.WorldMatrixRef.Translation, MySession.Static.LocalCharacter.PositionComp.GetPosition()) >= (double) this.m_soundRadius.Value * (double) this.m_soundRadius.Value)
        return;
      this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
    }

    public override void OnAddedToScene(object source)
    {
      base.OnAddedToScene(source);
      if (this.m_multiPanel != null && this.m_multiPanel.SurfaceCount > 0)
        this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.AddToScene();
    }

    public override void UpdateOnceBeforeFrame()
    {
      base.UpdateOnceBeforeFrame();
      if (this.IsWorking && this.IsFunctional)
      {
        if ((bool) this.m_isJukeboxPlaying && !this.m_isPlaying)
          this.PlaySound(false);
      }
      else
        this.StopSound();
      this.UpdateScreen();
    }

    protected override void OnStartWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    protected override void OnStopWorking() => this.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;

    public void UpdateScreen() => this.m_multiPanel?.UpdateScreen(this.IsWorking);

    private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
    {
      if (!Sandbox.Game.Multiplayer.Sync.IsServer)
        return;
      MyMultiplayer.RaiseEvent<MyJukebox, int, MySerializableSpriteCollection>(this, (Func<MyJukebox, Action<int, MySerializableSpriteCollection>>) (x => new Action<int, MySerializableSpriteCollection>(x.OnUpdateSpriteCollection)), panelIndex, sprites);
    }

    [Event(null, 219)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    [DistanceRadius(32f)]
    private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites) => this.m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);

    private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection) => MyMultiplayer.RaiseEvent<MyJukebox, int, int[]>(this, (Func<MyJukebox, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnSelectImageRequest)), panelIndex, selection);

    private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection) => MyMultiplayer.RaiseEvent<MyJukebox, int, int[]>(this, (Func<MyJukebox, Action<int, int[]>>) (x => new Action<int, int[]>(x.OnRemoveSelectedImageRequest)), panelIndex, selection);

    [Event(null, 235)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.RemoveItems(panelIndex, selection);
    }

    [Event(null, 246)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnSelectImageRequest(int panelIndex, int[] selection)
    {
      if (this.m_multiPanel == null)
        return;
      this.m_multiPanel.SelectItems(panelIndex, selection);
    }

    public void OpenWindow(bool isEditable, bool sync, bool isPublic)
    {
    }

    public MyTextPanelComponent PanelComponent => this.m_multiPanel?.PanelComponent;

    public bool IsTextPanelOpen => false;

    protected override void CreateTerminalControls()
    {
      if (MyTerminalControlFactory.AreControlsCreated<MyJukebox>())
        return;
      base.CreateTerminalControls();
      MyTerminalControlFactory.AddControl<MyJukebox>((MyTerminalControl<MyJukebox>) new MyTerminalControlButton<MyJukebox>("SelectSounds", MySpaceTexts.BlockPropertyTitle_JukeboxScreenSelectSounds, MySpaceTexts.Blank, (Action<MyJukebox>) (x => x.SendAddSoundsToSelection())));
      MyTerminalControlListbox<MyJukebox> terminalControlListbox1 = new MyTerminalControlListbox<MyJukebox>("SelectedSoundsList", MySpaceTexts.BlockPropertyTitle_JukeboxScreenSelectedSounds, MySpaceTexts.Blank, true);
      terminalControlListbox1.ListContent = (MyTerminalControlListbox<MyJukebox>.ListContentDelegate) ((x, list1, list2, focusedItem) => x.FillSelectedSoundsContent(list1, list2));
      terminalControlListbox1.ItemDoubleClicked = (MyTerminalControlListbox<MyJukebox>.SelectItemDelegate) ((x, y) => x.SendRemoveSoundsFromSelection());
      terminalControlListbox1.ItemSelected += (MyTerminalControlListbox<MyJukebox>.SelectItemDelegate) ((x, y) => x.SelectJukeboxItem(y));
      MyTerminalControlFactory.AddControl<MyJukebox>((MyTerminalControl<MyJukebox>) terminalControlListbox1);
      MyTerminalControlFactory.AddControl<MyJukebox>((MyTerminalControl<MyJukebox>) new MyTerminalControlButton<MyJukebox>("RemoveSelectedSounds", MySpaceTexts.BlockPropertyTitle_JukeboxScreenRemoveSelectedSounds, MySpaceTexts.Blank, (Action<MyJukebox>) (x => x.SendRemoveSoundsFromSelection())));
      if (MyTerminalControlFactory.GetControls(typeof (MySoundBlock)).FirstOrDefault<ITerminalControl>((Func<ITerminalControl, bool>) (x => x.Id == "SoundsList")) is MyTerminalControlListbox<MySoundBlock> terminalControlListbox2)
      {
        ((IMyTerminalControlListbox) terminalControlListbox2).Multiselect = true;
        terminalControlListbox2.ItemDoubleClicked += (MyTerminalControlListbox<MySoundBlock>.SelectItemDelegate) ((x, y) =>
        {
          if (!(x is MyJukebox myJukebox))
            return;
          myJukebox.SendAddSoundsToSelection();
        });
      }
      if (MyTerminalControlFactory.GetControls(typeof (MySoundBlock)).FirstOrDefault<ITerminalControl>((Func<ITerminalControl, bool>) (x => x.Id == "PlaySound")) is MyTerminalControlButton<MySoundBlock> terminalControlButton)
        terminalControlButton.Visible = (Func<MySoundBlock, bool>) (x => !(x is MyJukebox));
      if (!(MyTerminalControlFactory.GetControls(typeof (MySoundBlock)).FirstOrDefault<ITerminalControl>((Func<ITerminalControl, bool>) (x => x.Id == "StopSound")) is MyTerminalControlButton<MySoundBlock> terminalControlButton))
        return;
      terminalControlButton.Visible = (Func<MySoundBlock, bool>) (x => !(x is MyJukebox));
    }

    public void PlayOrStop()
    {
      if ((bool) this.m_isJukeboxPlaying)
        this.RequestJukeboxStopSound();
      else
        this.PlayCurrentSound();
    }

    public void PlayNext()
    {
      this.m_currentSound.Value = (int) this.m_currentSound + 1;
      if ((bool) this.m_isJukeboxPlaying)
      {
        this.PlayCurrentSound();
      }
      else
      {
        this.m_localCueIdString = (string) null;
        this.UpdateCue();
      }
    }

    private void PlayNextLocally()
    {
      this.m_currentSound.SetLocalValue((int) this.m_currentSound + 1);
      this.UpdateCurrentSound();
      if ((int) this.m_currentSound < 0)
        return;
      this.m_localCueIdString = this.m_selectedSounds[(int) this.m_currentSound];
      this.PlaySingleSound(new MySoundPair(this.m_localCueIdString));
    }

    public void PlayPrevious()
    {
      this.m_currentSound.Value = (int) this.m_currentSound - 1;
      if ((bool) this.m_isJukeboxPlaying)
      {
        this.PlayCurrentSound();
      }
      else
      {
        this.UpdateCue();
        this.m_localCueIdString = (string) null;
      }
    }

    private void PlayCurrentSound()
    {
      this.UpdateCue();
      if (!string.IsNullOrEmpty(this.m_localCueIdString))
      {
        this.m_cueIdString.Value = this.m_localCueIdString;
        this.m_localCueIdString = (string) null;
      }
      if (string.IsNullOrEmpty(this.m_cueIdString.Value))
      {
        this.m_isJukeboxPlaying.Value = false;
        if (Sandbox.Game.Multiplayer.Sync.IsServer)
          this.RequestStopSound();
        else
          this.StopSound();
      }
      else
        this.RequestJukeboxPlaySound();
    }

    public void RequestJukeboxStopSound() => MyMultiplayer.RaiseEvent<MyJukebox>(this, (Func<MyJukebox, Action>) (x => new Action(x.StopJukeboxSound)));

    [Event(null, 409)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void StopJukeboxSound()
    {
      this.StopSoundInternal();
      this.m_isJukeboxPlaying.SetLocalValue(false);
      this.m_localCueIdString = (string) null;
    }

    public void RequestJukeboxPlaySound() => MyMultiplayer.RaiseEvent<MyJukebox, bool>(this, (Func<MyJukebox, Action<bool>>) (x => new Action<bool>(x.PlayJukeboxSound)), false);

    [Event(null, 422)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    public void PlayJukeboxSound(bool isLoopable)
    {
      this.PlaySoundInternal(isLoopable);
      this.m_isJukeboxPlaying.SetLocalValue(true);
    }

    private void UpdateCurrentSound()
    {
      if ((int) this.m_currentSound < 0)
        this.m_currentSound.SetLocalValue(this.m_selectedSounds.Count - 1);
      else if ((int) this.m_currentSound >= this.m_selectedSounds.Count)
        this.m_currentSound.SetLocalValue(0);
      if (this.m_selectedSounds.Count != 0)
        return;
      this.m_currentSound.SetLocalValue(-1);
    }

    private void UpdateCue()
    {
      this.UpdateCurrentSound();
      if ((int) this.m_currentSound >= 0)
        this.m_cueIdString.Value = this.m_selectedSounds[(int) this.m_currentSound];
      else
        this.m_cueIdString.Value = "";
    }

    protected override void SelectSound(List<MyGuiControlListbox.Item> cuesId, bool sync)
    {
      this.m_soundBlockListSelection.Clear();
      foreach (MyGuiControlListbox.Item obj in cuesId)
        this.m_soundBlockListSelection.Add(obj.UserData.ToString());
    }

    private void SendAddSoundsToSelection() => MyMultiplayer.RaiseEvent<MyJukebox, List<string>>(this, (Func<MyJukebox, Action<List<string>>>) (x => new Action<List<string>>(x.OnAddSoundsToSelectionRequest)), this.m_soundBlockListSelection);

    [Event(null, 467)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnAddSoundsToSelectionRequest(List<string> selection) => this.AddSoundsToSelection(selection);

    private void AddSoundsToSelection(List<string> selection)
    {
      foreach (string str in selection)
        this.m_selectedSounds.Add(str);
      this.m_currentSound.Value = this.m_selectedSounds.Count - 1;
      this.UpdateCue();
      this.RaisePropertiesChanged();
    }

    private void SendRemoveSoundsFromSelection() => MyMultiplayer.RaiseEvent<MyJukebox, List<string>>(this, (Func<MyJukebox, Action<List<string>>>) (x => new Action<List<string>>(x.OnRemoveSoundsFromSelectionRequest)), this.m_jukeboxListSelection.ToList<string>());

    [Event(null, 492)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnRemoveSoundsFromSelectionRequest(List<string> selection) => this.RemoveSoundsFromSelection(selection);

    private void RemoveSoundsFromSelection(List<string> selection)
    {
      foreach (string str in selection)
        this.m_selectedSounds.Remove(str);
      if (this.IsJukeboxPlaying)
        this.PlayCurrentSound();
      this.RaisePropertiesChanged();
    }

    private void SelectJukeboxItem(List<MyGuiControlListbox.Item> items)
    {
      this.m_jukeboxListSelection.Clear();
      foreach (MyGuiControlListbox.Item obj in items)
      {
        int userData = (int) obj.UserData;
        if (userData < this.m_selectedSounds.Count)
          this.m_jukeboxListSelection.Add(this.m_selectedSounds[userData]);
      }
    }

    public MySoundCategoryDefinition.SoundDescription GetCurrentSoundDescription()
    {
      if (!string.IsNullOrEmpty(this.m_localCueIdString))
        return this.GetSoundDescription(this.m_localCueIdString);
      return string.IsNullOrEmpty((string) this.m_cueIdString) ? (MySoundCategoryDefinition.SoundDescription) null : this.GetSoundDescription((string) this.m_cueIdString);
    }

    private MySoundCategoryDefinition.SoundDescription GetSoundDescription(
      string soundName)
    {
      ListReader<MySoundCategoryDefinition> categoryDefinitions = MyDefinitionManager.Static.GetSoundCategoryDefinitions();
      MySoundCategoryDefinition.SoundDescription soundDescription = (MySoundCategoryDefinition.SoundDescription) null;
      foreach (MySoundCategoryDefinition categoryDefinition in categoryDefinitions)
      {
        foreach (MySoundCategoryDefinition.SoundDescription sound in categoryDefinition.Sounds)
        {
          if (soundName == sound.SoundId)
          {
            soundDescription = sound;
            break;
          }
        }
        if (soundDescription != null)
          break;
      }
      return soundDescription;
    }

    private void FillSelectedSoundsContent(
      ICollection<MyGuiControlListbox.Item> listBoxContent,
      ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
    {
      int num = 0;
      foreach (string selectedSound in this.m_selectedSounds)
      {
        MySoundCategoryDefinition.SoundDescription soundDescription = this.GetSoundDescription(selectedSound);
        if (soundDescription != null)
          listBoxContent.Add(new MyGuiControlListbox.Item(new StringBuilder(soundDescription.SoundText), userData: ((object) num++)));
      }
    }

    public override MyCubeBlockHighlightModes HighlightMode => MyCubeBlockHighlightModes.AlwaysCanUse;

    private void ChangeTextRequest(int panelIndex, string text) => MyMultiplayer.RaiseEvent<MyJukebox, int, string>(this, (Func<MyJukebox, Action<int, string>>) (x => new Action<int, string>(x.OnChangeTextRequest)), panelIndex, text);

    [Event(null, 580)]
    [Reliable]
    [Server(ValidationType.Access | ValidationType.Ownership)]
    [Broadcast]
    private void OnChangeTextRequest(int panelIndex, [Nullable] string text) => this.m_multiPanel?.ChangeText(panelIndex, text);

    protected sealed class OnUpdateSpriteCollection\u003C\u003ESystem_Int32\u0023VRage_Game_GUI_TextPanel_MySerializableSpriteCollection : ICallSite<MyJukebox, int, MySerializableSpriteCollection, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyJukebox @this,
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

    protected sealed class OnRemoveSelectedImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyJukebox, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyJukebox @this,
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

    protected sealed class OnSelectImageRequest\u003C\u003ESystem_Int32\u0023System_Int32\u003C\u0023\u003E : ICallSite<MyJukebox, int, int[], DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyJukebox @this,
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

    protected sealed class StopJukeboxSound\u003C\u003E : ICallSite<MyJukebox, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyJukebox @this,
        in DBNull arg1,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.StopJukeboxSound();
      }
    }

    protected sealed class PlayJukeboxSound\u003C\u003ESystem_Boolean : ICallSite<MyJukebox, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyJukebox @this,
        in bool isLoopable,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.PlayJukeboxSound(isLoopable);
      }
    }

    protected sealed class OnAddSoundsToSelectionRequest\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_String\u003E : ICallSite<MyJukebox, List<string>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyJukebox @this,
        in List<string> selection,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnAddSoundsToSelectionRequest(selection);
      }
    }

    protected sealed class OnRemoveSoundsFromSelectionRequest\u003C\u003ESystem_Collections_Generic_List`1\u003CSystem_String\u003E : ICallSite<MyJukebox, List<string>, DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyJukebox @this,
        in List<string> selection,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        @this.OnRemoveSoundsFromSelectionRequest(selection);
      }
    }

    protected sealed class OnChangeTextRequest\u003C\u003ESystem_Int32\u0023System_String : ICallSite<MyJukebox, int, string, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in MyJukebox @this,
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

    protected class m_isJukeboxPlaying\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<bool, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<bool, SyncDirection.BothWays>(obj1, obj2));
        ((MyJukebox) obj0).m_isJukeboxPlaying = (VRage.Sync.Sync<bool, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }

    protected class m_currentSound\u003C\u003ESyncComposer : ISyncComposer
    {
      public virtual ISyncType Compose([In] object obj0, [In] int obj1, [In] ISerializerInfo obj2)
      {
        VRage.Sync.Sync<int, SyncDirection.BothWays> sync;
        ISyncType syncType = (ISyncType) (sync = new VRage.Sync.Sync<int, SyncDirection.BothWays>(obj1, obj2));
        ((MyJukebox) obj0).m_currentSound = (VRage.Sync.Sync<int, SyncDirection.BothWays>) syncType;
        return (ISyncType) sync;
      }
    }
  }
}
