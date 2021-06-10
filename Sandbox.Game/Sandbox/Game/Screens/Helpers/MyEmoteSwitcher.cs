// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyEmoteSwitcher
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Definitions.Animation;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Game.Screens.Helpers
{
  [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
  public class MyEmoteSwitcher : MySessionComponentBase
  {
    private static readonly int PAGE_SIZE = 4;
    private List<MyEmoteSwitcher.MyPrioritizedDefinition> m_animations = new List<MyEmoteSwitcher.MyPrioritizedDefinition>();
    private int m_currentPage;
    private bool m_isActive;
    public static Vector4 DISABLED_COLOR = new Vector4(0.6f, 0.6f, 0.6f, 0.6f);

    public bool IsActive
    {
      get => this.m_isActive;
      private set
      {
        if (this.m_isActive == value)
          return;
        this.m_isActive = value;
        Action activeStateChanged = this.OnActiveStateChanged;
        if (activeStateChanged == null)
          return;
        activeStateChanged();
      }
    }

    public int AnimationCount { get; private set; }

    public int AnimationPageCount { get; private set; }

    public int CurrentPage
    {
      get => this.m_currentPage;
      private set
      {
        if (this.m_currentPage == value)
          return;
        this.m_currentPage = value >= 0 ? (value >= this.AnimationPageCount ? (this.AnimationPageCount < 0 ? 0 : this.AnimationPageCount - 1) : value) : 0;
        Action onPageChanged = this.OnPageChanged;
        if (onPageChanged == null)
          return;
        onPageChanged();
      }
    }

    public event Action OnActiveStateChanged;

    public event Action OnPageChanged;

    public MyEmoteSwitcher() => this.InitializeAnimationList();

    public override void HandleInput()
    {
      if (MyScreenManager.GetScreenWithFocus() != MyGuiScreenGamePlay.Static)
        return;
      IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
      MyStringId context = controlledEntity != null ? controlledEntity.AuxiliaryContext : MyStringId.NullOrEmpty;
      if (!MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER, MyControlStateType.PRESSED))
      {
        this.IsActive = false;
      }
      else
      {
        this.IsActive = true;
        if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER_LEFT))
          this.PreviousPage();
        if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SWITCHER_RIGHT))
          this.NextPage();
        if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_1))
          this.ActivateEmote(0);
        if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_2))
          this.ActivateEmote(1);
        if (MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_3))
          this.ActivateEmote(2);
        if (!MyControllerHelper.IsControl(context, MyControlsSpace.EMOTE_SELECT_4))
          return;
        this.ActivateEmote(3);
      }
    }

    private void InitializeAnimationList()
    {
      this.m_animations.Clear();
      foreach (MyAnimationDefinition animationDefinition in MyDefinitionManager.Static.GetAnimationDefinitions())
      {
        if (animationDefinition.Public)
          this.m_animations.Add(new MyEmoteSwitcher.MyPrioritizedDefinition()
          {
            Definition = (MyDefinitionBase) animationDefinition,
            Priority = animationDefinition.Priority
          });
      }
      foreach (MyEmoteDefinition definition in MyDefinitionManager.Static.GetDefinitions<MyEmoteDefinition>())
      {
        if (definition.Public)
          this.m_animations.Add(new MyEmoteSwitcher.MyPrioritizedDefinition()
          {
            Definition = (MyDefinitionBase) definition,
            Priority = definition.Priority
          });
      }
      this.m_animations.Sort((IComparer<MyEmoteSwitcher.MyPrioritizedDefinition>) MyEmoteSwitcher.MyPrioritizedComparer.Static);
      this.AnimationCount = this.m_animations.Count;
      this.AnimationPageCount = this.AnimationCount % MyEmoteSwitcher.PAGE_SIZE == 0 ? this.AnimationCount / MyEmoteSwitcher.PAGE_SIZE : this.AnimationCount / MyEmoteSwitcher.PAGE_SIZE + 1;
      this.CurrentPage = 0;
      MyRenderProxy.PreloadTextures(this.m_animations.SelectMany<MyEmoteSwitcher.MyPrioritizedDefinition, string>((Func<MyEmoteSwitcher.MyPrioritizedDefinition, IEnumerable<string>>) (x => (IEnumerable<string>) x.Definition.Icons)), TextureType.GUI);
    }

    public string GetIconUp() => this.GetIcon(0);

    public string GetIconLeft() => this.GetIcon(1);

    public string GetIconRight() => this.GetIcon(2);

    public string GetIconDown() => this.GetIcon(3);

    public string GetIcon(int id) => this.GetIconLinear(this.LinearizeIndex(id));

    private void NextPage() => ++this.CurrentPage;

    private void PreviousPage() => --this.CurrentPage;

    public string GetIconLinear(int linearIndex)
    {
      if (linearIndex < 0 || linearIndex >= this.AnimationCount)
        return string.Empty;
      return this.m_animations[linearIndex].Definition is MyAnimationDefinition definition ? (definition.Icons.Length == 0 ? string.Empty : definition.Icons[0]) : (this.m_animations[linearIndex].Definition is MyEmoteDefinition definition && definition.Icons.Length != 0 ? definition.Icons[0] : string.Empty);
    }

    private void ActivateEmote(int id) => this.ActivateEmoteLinear(this.LinearizeIndex(id));

    private void ActivateEmoteLinear(int linearIndex)
    {
      if (linearIndex < 0 || linearIndex >= this.AnimationCount || !this.HasNecessaryDLCsLinear(linearIndex, out string _))
        return;
      MySession.Static.ControlledEntity.SwitchToWeapon((MyToolbarItemWeapon) null);
      if (this.m_animations[linearIndex].Definition is MyAnimationDefinition definition)
      {
        MyAnimationActivator.Activate(definition);
      }
      else
      {
        if (!(this.m_animations[linearIndex].Definition is MyEmoteDefinition definition))
          return;
        MyAnimationActivator.Activate(definition);
      }
    }

    private int LinearizeIndex(int id) => this.m_currentPage * MyEmoteSwitcher.PAGE_SIZE + id;

    private bool HasNecessaryDLCs(int index, out string icon) => this.HasNecessaryDLCsLinear(this.LinearizeIndex(index), out icon);

    private bool HasNecessaryDLCsLinear(int linearIndex, out string icon)
    {
      if (linearIndex >= 0 && linearIndex < this.AnimationCount)
        return this.HasNecessaryDLCs(this.m_animations[linearIndex].Definition, out icon);
      icon = string.Empty;
      return false;
    }

    private bool HasNecessaryDLCs(MyDefinitionBase definition, out string icon)
    {
      if (definition.DLCs != null && definition.DLCs.Length != 0)
      {
        MyDLCs.MyDLC missingDefinitionDlc = MySession.Static.GetComponent<MySessionComponentDLC>().GetFirstMissingDefinitionDLC(definition, Sync.MyId);
        if (missingDefinitionDlc != null)
        {
          icon = missingDefinitionDlc.Icon;
          return false;
        }
        MyDLCs.MyDLC dlc;
        icon = !MyDLCs.TryGetDLC(definition.DLCs[0], out dlc) ? string.Empty : dlc.Icon;
        return true;
      }
      icon = string.Empty;
      return true;
    }

    public Vector4 GetIconMask(int i) => this.HasNecessaryDLCs(i, out string _) ? Vector4.One : MyEmoteSwitcher.DISABLED_COLOR;

    internal Vector4 GetIconUpMask() => this.GetIconMask(0);

    internal Vector4 GetIconLeftMask() => this.GetIconMask(1);

    internal Vector4 GetIconRightMask() => this.GetIconMask(2);

    internal Vector4 GetIconDownMask() => this.GetIconMask(3);

    public string GetSubIcon(int i)
    {
      string icon;
      this.HasNecessaryDLCs(i, out icon);
      return icon;
    }

    internal string GetSubIconUp() => this.GetSubIcon(0);

    internal string GetSubIconLeft() => this.GetSubIcon(1);

    internal string GetSubIconRight() => this.GetSubIcon(2);

    internal string GetSubIconDown() => this.GetSubIcon(3);

    private struct MyPrioritizedDefinition
    {
      public int Priority;
      public MyDefinitionBase Definition;
    }

    private class MyPrioritizedComparer : IComparer<MyEmoteSwitcher.MyPrioritizedDefinition>
    {
      public static MyEmoteSwitcher.MyPrioritizedComparer Static = new MyEmoteSwitcher.MyPrioritizedComparer();

      private MyPrioritizedComparer()
      {
      }

      public int Compare(
        MyEmoteSwitcher.MyPrioritizedDefinition x,
        MyEmoteSwitcher.MyPrioritizedDefinition y)
      {
        return y.Priority.CompareTo(x.Priority);
      }
    }
  }
}
