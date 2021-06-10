// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyHudNotifications
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage.Game;
using VRage.Generics;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyHudNotifications
  {
    public const int MAX_PRIORITY = 5;
    private Predicate<MyHudNotifications.NotificationDrawData> m_disappearedPredicate;
    private Dictionary<int, List<MyHudNotifications.NotificationDrawData>> m_notificationsByPriority;
    private List<StringBuilder> m_texts;
    private readonly List<MyHudNotifications.NotificationDrawData> m_toDraw = new List<MyHudNotifications.NotificationDrawData>(8);
    private static MyObjectsPool<StringBuilder> m_textsPool;
    private HashSet<MyHudNotificationBase> m_toRemove = new HashSet<MyHudNotificationBase>();
    private HashSet<MyHudNotificationBase> m_toAdd = new HashSet<MyHudNotificationBase>();
    private object m_lockObject = new object();
    private MyHudNotificationBase[] m_singletons;
    public Vector2 Position;
    private int m_visibleCount;

    public event Action<MyNotificationSingletons> OnNotificationAdded;

    public MyHudNotificationBase Add(
      MyNotificationSingletons singleNotification)
    {
      this.Add(this.m_singletons[(int) singleNotification]);
      if (this.OnNotificationAdded != null)
        this.OnNotificationAdded(singleNotification);
      return this.m_singletons[(int) singleNotification];
    }

    public void Remove(MyNotificationSingletons singleNotification) => this.Remove(this.m_singletons[(int) singleNotification]);

    public MyHudNotificationBase Get(
      MyNotificationSingletons singleNotification)
    {
      return this.m_singletons[(int) singleNotification];
    }

    public MyHudNotifications()
    {
      this.Position = MyNotificationConstants.DEFAULT_NOTIFICATION_MESSAGE_NORMALIZED_POSITION;
      this.m_disappearedPredicate = (Predicate<MyHudNotifications.NotificationDrawData>) (x => !x.Notification.Alive);
      this.m_notificationsByPriority = new Dictionary<int, List<MyHudNotifications.NotificationDrawData>>();
      this.m_texts = new List<StringBuilder>(8);
      MyHudNotifications.m_textsPool = new MyObjectsPool<StringBuilder>(80);
      this.m_singletons = new MyHudNotificationBase[Enum.GetValues(typeof (MyNotificationSingletons)).Length];
      this.Register(MyNotificationSingletons.GameOverload, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationMemoryOverload, font: "Red", priority: 2));
      this.Register(MyNotificationSingletons.SuitEnergyLow, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationSuitEnergyLow, font: "Red", priority: 2));
      this.Register(MyNotificationSingletons.SuitEnergyCritical, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationSuitEnergyCritical, font: "Red", priority: 2));
      this.Register(MyNotificationSingletons.IncompleteGrid, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationIncompleteGrid, font: "Red", priority: 2));
      this.Register(MyNotificationSingletons.DisabledWeaponsAndTools, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationToolDisabled, 0, "Red"));
      this.Register(MyNotificationSingletons.WeaponDisabledInWorldSettings, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationWeaponDisabledInSettings, font: "Red"));
      this.Register(MyNotificationSingletons.MultiplayerDisabled, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationMultiplayerDisabled, font: "Red", priority: 5));
      this.Register(MyNotificationSingletons.MissingComponent, (MyHudNotificationBase) new MyHudMissingComponentNotification(MyCommonTexts.NotificationMissingComponentToPlaceBlockFormat, priority: 1));
      this.Register(MyNotificationSingletons.WorldLoaded, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.WorldLoaded));
      this.Register(MyNotificationSingletons.ObstructingBlockDuringMerge, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationObstructingBlockDuringMerge, font: "Red"));
      this.Register(MyNotificationSingletons.HideHints, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationHideHintsInGameOptions, 0, priority: 2, level: MyNotificationLevel.Control));
      this.Register(MyNotificationSingletons.HelpHint, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationNeedShowHelpScreen, 0, priority: 1, level: MyNotificationLevel.Control));
      this.Register(MyNotificationSingletons.ScreenHint, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationScreenFormat, 0, level: MyNotificationLevel.Control));
      this.Register(MyNotificationSingletons.RespawnShipWarning, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationRespawnShipDelete, 10000, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.BuildingOnRespawnShipWarning, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationBuildingOnRespawnShip, 10000, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PlayerDemotedNone, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPlayerDemoted_None, 10000, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PlayerDemotedScripter, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPlayerDemoted_Scripter, 10000, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PlayerDemotedModerator, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPlayerDemoted_Moderator, 10000, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PlayerDemotedSpaceMaster, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPlayerDemoted_SpaceMaster, 10000, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PlayerPromotedScripter, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPlayerPromoted_Scripter, 10000, level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PlayerPromotedModerator, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPlayerPromoted_Moderator, 10000, level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PlayerPromotedSpaceMaster, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPlayerPromoted_SpaceMaster, 10000, level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PlayerPromotedAdmin, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPlayerPromoted_Admin, 10000, level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.CopySucceeded, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationCopySucceeded, 1300, level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.CopyFailed, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationCopyFailed, 1300, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.PasteFailed, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationPasteFailed, 1300, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.CutPermissionFailed, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationCutPermissionFailed, 1300, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.DeletePermissionFailed, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationDeletePermissionFailed, 1300, "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.ClientCannotSave, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationClientCannotSave, font: "Red"));
      this.Register(MyNotificationSingletons.CannotSave, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationSavingDisabled, font: "Red"));
      this.Register(MyNotificationSingletons.WheelNotPlaced, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationWheelNotPlaced, font: "Red"));
      this.Register(MyNotificationSingletons.CopyPasteBlockNotAvailable, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationCopyPasteBlockNotAvailable, font: "Red"));
      this.Register(MyNotificationSingletons.CopyPasteFloatingObjectNotAvailable, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationCopyPasteFloatingObjectNotAvailable, font: "Red"));
      this.Register(MyNotificationSingletons.CopyPasteAsteoridObstructed, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationCopyPasteAsteroidObstructed, font: "Red"));
      this.Register(MyNotificationSingletons.TextPanelReadOnly, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.NotificationTextPanelReadOnly, font: "Red"));
      this.Register(MyNotificationSingletons.AccessDenied, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.AccessDenied, font: "Red"));
      this.Register(MyNotificationSingletons.AdminMenuNotAvailable, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.AdminMenuNotAvailable, 10000, "Red", priority: 2, level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.HeadNotPlaced, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.Notification_PistonHeadNotPlaced, font: "Red"));
      this.Register(MyNotificationSingletons.HeadAlreadyExists, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.Notification_PistonHeadAlreadyExists, font: "Red"));
      MyHudNotification myHudNotification1 = new MyHudNotification(MySpaceTexts.NotificationLimitsGridSize, 5000, "Red");
      myHudNotification1.SetTextFormatArguments((object) MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
      this.Register(MyNotificationSingletons.LimitsGridSize, (MyHudNotificationBase) myHudNotification1);
      MyHudNotification myHudNotification2 = new MyHudNotification(MySpaceTexts.NotificationLimitsPerBlockType, 5000, "Red");
      myHudNotification2.SetTextFormatArguments((object) MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
      this.Register(MyNotificationSingletons.LimitsPerBlockType, (MyHudNotificationBase) myHudNotification2);
      MyHudNotification myHudNotification3 = new MyHudNotification(MySpaceTexts.NotificationLimitsPlayer, 5000, "Red");
      myHudNotification3.SetTextFormatArguments((object) MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
      this.Register(MyNotificationSingletons.LimitsPlayer, (MyHudNotificationBase) myHudNotification3);
      MyHudNotification myHudNotification4 = new MyHudNotification(MySpaceTexts.NotificationLimitsPCU, 5000, "Red");
      myHudNotification4.SetTextFormatArguments((object) MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
      this.Register(MyNotificationSingletons.LimitsPCU, (MyHudNotificationBase) myHudNotification4);
      MyHudNotification myHudNotification5 = new MyHudNotification(MySpaceTexts.NotificationLimitsNoFaction, 5000, "Red");
      myHudNotification5.SetTextFormatArguments((object) MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard));
      this.Register(MyNotificationSingletons.LimitsNoFaction, (MyHudNotificationBase) myHudNotification5);
      this.Register(MyNotificationSingletons.GridReachedPhysicalLimit, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationGridReachedPhysicalLimit, font: "Red"));
      this.Register(MyNotificationSingletons.BlockNotResearched, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationBlockNotResearched, font: "Red"));
      this.Register(MyNotificationSingletons.ManipulatingDoorFailed, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.Notification_CannotManipulateDoor, font: "Red", level: MyNotificationLevel.Important));
      this.Register(MyNotificationSingletons.BlueprintScriptsRemoved, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.Notification_BlueprintScriptRemoved, font: "Red"));
      this.Register(MyNotificationSingletons.ConnectionProblem, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.PerformanceWarningHeading_Connection, 0, "Red"));
      this.Register(MyNotificationSingletons.MissingDLC, (MyHudNotificationBase) new MyHudNotification(MyCommonTexts.RequiresDlc, font: "Red"));
      this.Register(MyNotificationSingletons.BuildPlannerEmpty, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationBuildPlannerEmpty));
      this.Register(MyNotificationSingletons.WithdrawSuccessful, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationWithdrawSuccessful));
      this.Register(MyNotificationSingletons.WithdrawFailed, (MyHudNotificationBase) new MyHudNotification(MyStringId.GetOrCompute("{0}")));
      this.Register(MyNotificationSingletons.DepositSuccessful, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationDepositSuccessful));
      this.Register(MyNotificationSingletons.DepositFailed, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationDepositFailed));
      this.Register(MyNotificationSingletons.PutToProductionSuccessful, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPutToProductionSuccessful));
      this.Register(MyNotificationSingletons.PutToProductionFailed, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationPutToProductionFailed));
      this.Register(MyNotificationSingletons.BuildPlannerCapacityReached, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.BuildPlannerCapacityReached));
      this.Register(MyNotificationSingletons.BuildPlannerComponentsAdded, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.BuildPlannerComponentsAdded));
      this.Register(MyNotificationSingletons.DamageTurnedOff, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationDamageTurnedOff));
      this.Register(MyNotificationSingletons.GridIsImmune, (MyHudNotificationBase) new MyHudNotification(MySpaceTexts.NotificationGridIsImmune));
      this.FormatNotifications(MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed && MyFakes.ENABLE_CONTROLLER_HINTS);
      MyInput.Static.JoystickConnected += new Action<bool>(this.Static_JoystickConnected);
    }

    private void Static_JoystickConnected(bool value) => this.FormatNotifications(value && MyFakes.ENABLE_CONTROLLER_HINTS);

    public void Add(MyHudNotificationBase notification)
    {
      lock (this.m_lockObject)
      {
        List<MyHudNotifications.NotificationDrawData> notificationGroup = this.GetNotificationGroup(notification.Priority);
        if (notificationGroup.All<MyHudNotifications.NotificationDrawData>((Func<MyHudNotifications.NotificationDrawData, bool>) (x => x.Notification != notification)))
        {
          notification.BeforeAdd();
          notificationGroup.Add(new MyHudNotifications.NotificationDrawData(notification));
        }
        notification.ResetAliveTime();
      }
    }

    public void Remove(MyHudNotificationBase notification)
    {
      if (notification == null)
        return;
      lock (this.m_lockObject)
        this.GetNotificationGroup(notification.Priority).RemoveAll((Predicate<MyHudNotifications.NotificationDrawData>) (x =>
        {
          if (x.Notification != notification)
            return false;
          x.Notification.BeforeRemove();
          x.Clear();
          return true;
        }));
    }

    public void Update(MyHudNotificationBase notification)
    {
      if (notification == null)
        return;
      lock (this.m_lockObject)
        this.GetNotificationGroup(notification.Priority).FindLast((Predicate<MyHudNotifications.NotificationDrawData>) (x =>
        {
          if (x.Notification != notification)
            return false;
          x.Update(notification);
          return true;
        }));
    }

    public void Clear()
    {
      MyInput.Static.JoystickConnected -= new Action<bool>(this.Static_JoystickConnected);
      lock (this.m_lockObject)
      {
        foreach (KeyValuePair<int, List<MyHudNotifications.NotificationDrawData>> keyValuePair in this.m_notificationsByPriority)
          keyValuePair.Value.Clear();
      }
    }

    public void Update() => this.ProcessBeforeDraw(out this.m_visibleCount);

    public void Draw()
    {
      this.DrawFog();
      this.DrawNotifications(this.m_visibleCount);
    }

    public void ReloadTexts()
    {
      this.FormatNotifications(MyInput.Static.IsJoystickConnected() && MyInput.Static.IsJoystickLastUsed && MyFakes.ENABLE_CONTROLLER_HINTS);
      lock (this.m_lockObject)
      {
        foreach (KeyValuePair<int, List<MyHudNotifications.NotificationDrawData>> keyValuePair in this.m_notificationsByPriority)
        {
          foreach (MyHudNotifications.NotificationDrawData notificationDrawData in keyValuePair.Value)
          {
            notificationDrawData.Notification.SetTextDirty();
            notificationDrawData.Clear();
            notificationDrawData.PrepareElements();
          }
        }
      }
    }

    public void UpdateBeforeSimulation()
    {
      lock (this.m_lockObject)
      {
        foreach (KeyValuePair<int, List<MyHudNotifications.NotificationDrawData>> keyValuePair in this.m_notificationsByPriority)
        {
          foreach (MyHudNotifications.NotificationDrawData notificationDrawData in keyValuePair.Value)
            notificationDrawData.Notification.AddAliveTime(16);
        }
        foreach (KeyValuePair<int, List<MyHudNotifications.NotificationDrawData>> keyValuePair in this.m_notificationsByPriority)
        {
          foreach (MyHudNotifications.NotificationDrawData notificationDrawData in keyValuePair.Value)
          {
            if (this.m_disappearedPredicate(notificationDrawData))
            {
              notificationDrawData.Notification.BeforeRemove();
              notificationDrawData.Clear();
            }
          }
          keyValuePair.Value.RemoveAll(this.m_disappearedPredicate);
        }
      }
    }

    public void Register(MyNotificationSingletons singleton, MyHudNotificationBase notification) => this.m_singletons[(int) singleton] = notification;

    public static MyHudNotification CreateControlNotification(
      MyStringId textId,
      params object[] args)
    {
      MyHudNotification myHudNotification = new MyHudNotification(textId, 0, level: MyNotificationLevel.Control);
      myHudNotification.SetTextFormatArguments(args);
      return myHudNotification;
    }

    private void ProcessBeforeDraw(out int visibleCount)
    {
      this.m_toDraw.Clear();
      visibleCount = 0;
      lock (this.m_lockObject)
      {
        for (int key = 5; key >= 0; --key)
        {
          List<MyHudNotifications.NotificationDrawData> notificationDrawDataList;
          this.m_notificationsByPriority.TryGetValue(key, out notificationDrawDataList);
          if (notificationDrawDataList != null)
          {
            foreach (MyHudNotifications.NotificationDrawData notificationDrawData in notificationDrawDataList)
            {
              if (this.IsDrawn(notificationDrawData.Notification))
              {
                this.m_toDraw.Add(notificationDrawData);
                ++visibleCount;
                if (visibleCount == 8)
                  return;
              }
            }
          }
        }
      }
    }

    private void DrawFog()
    {
      Vector2 position = this.Position;
      for (int index = 0; index < this.m_toDraw.Count; ++index)
      {
        MyHudNotifications.NotificationDrawData notificationDrawData = this.m_toDraw[index];
        if (!notificationDrawData.IsClear && notificationDrawData.HasFog)
        {
          Vector2 textSize = notificationDrawData.TextSize;
          MyGuiTextShadows.DrawShadow(ref position, ref textSize);
          position.Y += textSize.Y;
        }
      }
    }

    private void DrawNotifications(int visibleCount)
    {
      lock (this.m_lockObject)
      {
        Vector2 position = this.Position;
        int num = Math.Max(visibleCount, this.m_toDraw.Count);
        for (int index = 0; index < num; ++index)
        {
          MyHudNotifications.NotificationDrawData notificationDrawData = this.m_toDraw[index];
          if (!notificationDrawData.IsClear)
          {
            position.X = this.Position.X - notificationDrawData.TextSize.X / 2f;
            if (notificationDrawData.Elements != null)
            {
              bool flag = false;
              foreach (MyHudNotifications.NotificationDrawData.Element element in notificationDrawData.Elements)
              {
                Color color = flag ? Color.Yellow : Color.White;
                if (element.Text != null)
                  MyGuiManager.DrawString(notificationDrawData.Notification.Font, element.Text.ToString(), position, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.2f, new Color?(color), useFullClientArea: MyVideoSettingsManager.IsTripleHead());
                position.X += element.Size.X;
                flag = !flag;
              }
            }
            else
              MyGuiManager.DrawString(notificationDrawData.Notification.Font, notificationDrawData.Text.ToString(), position, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.2f, new Color?(Color.White), useFullClientArea: MyVideoSettingsManager.IsTripleHead());
            position.Y += notificationDrawData.TextSize.Y;
          }
        }
      }
    }

    private List<MyHudNotifications.NotificationDrawData> GetNotificationGroup(
      int priority)
    {
      List<MyHudNotifications.NotificationDrawData> notificationDrawDataList;
      if (!this.m_notificationsByPriority.TryGetValue(priority, out notificationDrawDataList))
      {
        notificationDrawDataList = new List<MyHudNotifications.NotificationDrawData>();
        this.m_notificationsByPriority[priority] = notificationDrawDataList;
      }
      return notificationDrawDataList;
    }

    private bool IsDrawn(MyHudNotificationBase notification)
    {
      bool flag = notification.Alive;
      if (notification.IsControlsHint)
        flag = flag && MySandboxGame.Config.ControlsHints;
      if (MyHud.MinimalHud && !MyHud.CutsceneHud && notification.Level != MyNotificationLevel.Important)
        flag = false;
      if (MyHud.CutsceneHud && notification.Level == MyNotificationLevel.Control)
        flag = false;
      return flag;
    }

    private void SetNotificationTextAndArgs(
      MyNotificationSingletons type,
      MyStringId textId,
      params object[] args)
    {
      MyHudNotification myHudNotification = this.Get(type) as MyHudNotification;
      myHudNotification.Text = textId;
      myHudNotification.SetTextFormatArguments(args);
      this.Add((MyHudNotificationBase) myHudNotification);
    }

    private void FormatNotifications(bool forJoystick)
    {
      if (forJoystick)
      {
        MyStringId cxBase = MySpaceBindingCreator.CX_BASE;
        MyStringId cxCharacter = MySpaceBindingCreator.CX_CHARACTER;
        MyStringId controlMenu = MyControlsSpace.CONTROL_MENU;
        MyControllerHelper.GetCodeForControl(cxBase, controlMenu);
        MyControllerHelper.GetCodeForControl(cxCharacter, MyControlsSpace.TOOLBAR_NEXT_ITEM);
        MyControllerHelper.GetCodeForControl(cxCharacter, MyControlsSpace.TOOLBAR_PREV_ITEM);
      }
      else
      {
        MyInput.Static.GetGameControl(MyControlsSpace.TOGGLE_HUD);
        MyInput.Static.GetGameControl(MyControlsSpace.SLOT1);
        MyInput.Static.GetGameControl(MyControlsSpace.SLOT2);
        MyInput.Static.GetGameControl(MyControlsSpace.SLOT3);
        MyInput.Static.GetGameControl(MyControlsSpace.BUILD_SCREEN);
        MyInput.Static.GetGameControl(MyControlsSpace.HELP_SCREEN);
        MyInput.Static.GetGameControl(MyControlsSpace.SWITCH_COMPOUND);
      }
    }

    private class NotificationDrawData
    {
      public MyHudNotificationBase Notification;
      public Vector2 TextSize;
      public StringBuilder Text;
      public MyHudNotifications.NotificationDrawData.Element[] Elements;
      private string[] m_separators = new string[2]
      {
        "[",
        "]"
      };

      public bool HasFog => this.Notification.HasFog;

      public bool IsClear => this.Text == null;

      public NotificationDrawData(MyHudNotificationBase notification)
      {
        this.Notification = notification;
        this.PrepareElements();
      }

      public void PrepareElements()
      {
        if (MyHudNotifications.m_textsPool == null)
          MyHudNotifications.m_textsPool = new MyObjectsPool<StringBuilder>(80);
        if (this.Notification == null)
          return;
        string text = this.Notification.GetText();
        StringBuilder stringBuilder1 = MyHudNotifications.m_textsPool.Allocate();
        if (stringBuilder1 == null)
          return;
        this.Text = stringBuilder1.Clear().Append(text).UpdateControlsFromNotificationFriendly();
        if (this.Text == null || this.Notification.Font == null || string.IsNullOrEmpty(text))
          return;
        this.TextSize = MyGuiManager.MeasureString(this.Notification.Font, this.Text, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.2f);
        string[] strArray = text.Split(this.m_separators, StringSplitOptions.None);
        if (strArray == null || strArray.Length == 1)
          return;
        this.Elements = new MyHudNotifications.NotificationDrawData.Element[strArray.Length];
        for (int index = 0; index < strArray.Length; ++index)
        {
          StringBuilder stringBuilder2 = MyHudNotifications.m_textsPool.Allocate();
          if (stringBuilder2 == null)
            break;
          this.Elements[index].Text = stringBuilder2.Clear().Append(strArray[index]).UpdateControlsFromNotificationFriendly();
          this.Elements[index].Size = MyGuiManager.MeasureString(this.Notification.Font, this.Elements[index].Text, MyGuiSandbox.GetDefaultTextScaleWithLanguage() * 1.2f);
        }
      }

      public void Clear()
      {
        if (MyHudNotifications.m_textsPool != null)
        {
          if (this.Elements != null)
          {
            for (int index = 0; index < this.Elements.Length; ++index)
              MyHudNotifications.m_textsPool.Deallocate(this.Elements[index].Text);
            this.Elements = (MyHudNotifications.NotificationDrawData.Element[]) null;
          }
          MyHudNotifications.m_textsPool.Deallocate(this.Text);
        }
        else
          MyHudNotifications.m_textsPool = new MyObjectsPool<StringBuilder>(80);
        this.Text = (StringBuilder) null;
      }

      internal void Update(MyHudNotificationBase notification)
      {
        this.Clear();
        this.Notification = notification;
        this.PrepareElements();
      }

      public struct Element
      {
        public StringBuilder Text;
        public Vector2 Size;
      }
    }

    public class ControlsHelper
    {
      private MyControl[] m_controls;

      public ControlsHelper(params MyControl[] controls) => this.m_controls = controls;

      public override string ToString() => string.Join(", ", ((IEnumerable<MyControl>) this.m_controls).Select<MyControl, string>((Func<MyControl, string>) (s => s.ButtonNamesIgnoreSecondary)).Where<string>((Func<string, bool>) (s => !string.IsNullOrEmpty(s))));
    }
  }
}
