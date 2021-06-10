// Decompiled with JetBrains decompiler
// Type: SpaceEngineers.Game.GUI.MyGuiScreenMvvmBase
// Assembly: SpaceEngineers.Game, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB5833CD-B906-484A-B0B7-76E25B7A0109
// Assembly location: D:\Files\library_development\lib_se\SpaceEngineers.Game.dll

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Debug;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using ParallelTasks;
using Sandbox;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using VRage.Audio;
using VRage.Profiler;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace SpaceEngineers.Game.GUI
{
  public abstract class MyGuiScreenMvvmBase : MyGuiScreenBase
  {
    private DebugViewModel m_debug;
    private int m_elapsedTime;
    private int m_previousTime;
    private bool m_layoutUpdated;
    private bool m_enableAsyncDraw;
    private MyRenderMessageDrawCommands m_drawAsyncMessages;
    private Task? m_drawTask;
    protected UIRoot m_view;
    protected MyViewModelBase m_viewModel;

    public MyGuiScreenMvvmBase(MyViewModelBase viewModel)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)))
    {
      this.EnabledBackgroundFade = true;
      this.m_closeOnEsc = false;
      this.m_drawEvenWithoutFocus = true;
      this.CanHideOthers = true;
      this.CanBeHidden = true;
      this.m_viewModel = viewModel;
      Rectangle safeGuiRectangle = MyGuiManager.GetSafeGuiRectangle();
      viewModel.MaxWidth = (float) safeGuiRectangle.Width * (1f / UIElement.DpiScaleX);
      if (MySession.Static == null)
        return;
      MySession.Static.LocalCharacter.CharacterDied += new Action<MyCharacter>(this.OnCharacterDied);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.RecreateControls(false);
    }

    public abstract UIRoot CreateView();

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.m_view = this.CreateView();
      if (this.m_view == null)
        throw new NullReferenceException("View is empty");
      RelayCommand relayCommand = new RelayCommand(new Action<object>(this.OnExitScreen), new Predicate<object>(this.CanExit));
      this.m_view.InputBindings.Add((InputBinding) new GamepadBinding((ICommand) relayCommand, new GamepadGesture(GamepadInput.BButton)));
      this.m_view.InputBindings.Add((InputBinding) new KeyBinding((ICommand) relayCommand, new KeyGesture(KeyCode.Escape)));
      this.m_viewModel.BackgroundOverlay = new ColorW(1f, 1f, 1f, MySandboxGame.Config.UIBkOpacity);
      ImageManager.Instance.LoadImages((object) null);
      SoundSourceCollection sourceCollection1 = new SoundSourceCollection();
      sourceCollection1.Add(new SoundSource()
      {
        SoundType = SoundType.ButtonsClick,
        SoundAsset = GuiSounds.MouseClick.ToString()
      });
      sourceCollection1.Add(new SoundSource()
      {
        SoundType = SoundType.ButtonsHover,
        SoundAsset = GuiSounds.MouseOver.ToString()
      });
      sourceCollection1.Add(new SoundSource()
      {
        SoundType = SoundType.CheckBoxHover,
        SoundAsset = GuiSounds.MouseOver.ToString()
      });
      sourceCollection1.Add(new SoundSource()
      {
        SoundType = SoundType.TabControlSelect,
        SoundAsset = GuiSounds.MouseClick.ToString()
      });
      sourceCollection1.Add(new SoundSource()
      {
        SoundType = SoundType.TabControlMove,
        SoundAsset = GuiSounds.MouseClick.ToString()
      });
      sourceCollection1.Add(new SoundSource()
      {
        SoundType = SoundType.ListBoxSelect,
        SoundAsset = GuiSounds.MouseClick.ToString()
      });
      sourceCollection1.Add(new SoundSource()
      {
        SoundType = SoundType.FocusChanged,
        SoundAsset = GuiSounds.MouseOver.ToString()
      });
      SoundSourceCollection sourceCollection2 = sourceCollection1;
      SoundManager.SoundsProperty.DefaultMetadata.DefaultValue = (object) sourceCollection2;
      SoundManager.Instance.AddSound(GuiSounds.MouseClick.ToString());
      SoundManager.Instance.AddSound(GuiSounds.MouseOver.ToString());
      SoundManager.Instance.AddSound(GuiSounds.Item.ToString());
      SoundManager.Instance.LoadSounds((object) null);
      this.m_view.DataContext = (object) this.m_viewModel;
      Parallel.Start((Action) (() => this.m_view.UpdateLayout(0.0)), (Action) (() =>
      {
        this.m_layoutUpdated = true;
        this.m_viewModel.InitializeData();
      }));
    }

    protected virtual bool CanExit(object parameter) => true;

    private void OnExitScreen(object obj) => this.Canceling();

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      if (this.m_layoutUpdated)
      {
        if (InputManager.Current.FocusedElement is TabControl)
        {
          InputManager.Current.NavigateTabNext(false);
          this.m_view.ShowGamepadHelp(InputManager.Current.FocusedElement);
        }
        this.m_view.UpdateInput((double) this.m_elapsedTime);
      }
      base.HandleInput(receivedFocusInThisUpdate);
    }

    private void OnCharacterDied(MyCharacter character) => this.CloseScreen(false);

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.m_viewModel.OnScreenClosing();
      this.m_layoutUpdated = false;
      VisualTreeHelper.Instance.ClearParentCache();
      if (MySession.Static != null)
        MySession.Static.LocalCharacter.CharacterDied -= new Action<MyCharacter>(this.OnCharacterDied);
      Action action = new Action(this.m_viewModel.OnScreenClosed);
      int num = base.CloseScreen(isUnloading) ? 1 : 0;
      action();
      return num != 0;
    }

    public override bool Update(bool hasFocus)
    {
      Engine.Instance.Update();
      this.m_viewModel.Update();
      return base.Update(hasFocus);
    }

    public override bool Draw()
    {
      if (!base.Draw() || !this.m_layoutUpdated)
        return false;
      this.m_elapsedTime = MySandboxGame.TotalTimeInMilliseconds - this.m_previousTime;
      bool disposeAfterDraw = true;
      this.m_view.UpdateLayout((double) this.m_elapsedTime);
      if (!this.m_enableAsyncDraw)
      {
        this.m_view.Draw((double) this.m_elapsedTime);
      }
      else
      {
        if (this.m_drawAsyncMessages == null)
          this.DrawAsync();
        ref Task? local = ref this.m_drawTask;
        if (local.HasValue)
          local.GetValueOrDefault().WaitOrExecute();
        MyRenderProxy.ExecuteCommands(this.m_drawAsyncMessages, disposeAfterDraw);
        this.m_drawTask = new Task?();
        if (disposeAfterDraw)
          this.m_drawTask = new Task?(Parallel.Start(new Action(this.DrawAsync), Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.GUI, "EKScreensDraw"), WorkPriority.VeryHigh));
      }
      this.m_previousTime = MySandboxGame.TotalTimeInMilliseconds;
      return true;
    }

    private void DrawAsync()
    {
      MyRenderProxy.BeginRecordingDeferredMessages();
      this.m_view.Draw((double) this.m_elapsedTime);
      this.m_drawAsyncMessages = MyRenderProxy.FinishRecordingDeferredMessages();
    }
  }
}
