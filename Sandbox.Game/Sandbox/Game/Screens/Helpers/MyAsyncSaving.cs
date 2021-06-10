// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Screens.Helpers.MyAsyncSaving
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VRage;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Helpers
{
  public static class MyAsyncSaving
  {
    private static Action m_callbackOnFinished;
    private static int m_inProgressCount;
    private static bool m_screenshotTaken;
    private static bool m_saveErrorIsShown;

    public static bool InProgress => MyAsyncSaving.m_inProgressCount > 0;

    private static void PushInProgress() => ++MyAsyncSaving.m_inProgressCount;

    private static void PopInProgress() => --MyAsyncSaving.m_inProgressCount;

    public static void Start(Action callbackOnFinished = null, string customName = null)
    {
      MyAsyncSaving.PushInProgress();
      MyAsyncSaving.m_callbackOnFinished = callbackOnFinished;
      MySessionSnapshot snapshot;
      MyAsyncSaving.OnSnapshotDone(MySession.Static.Save(out snapshot, customName), snapshot);
    }

    public static void DelayedSaveAfterLoad(string saveName) => MySession.Static.AddUpdateCallback(new MyUpdateCallback((Func<bool>) (() =>
    {
      MyGuiScreenGamePlay firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>();
      if (firstScreenOfType == null || firstScreenOfType.State != MyGuiScreenState.OPENED)
        return false;
      if (MyHud.ScreenEffects.IsBlackscreenFadeInProgress())
        MyHud.ScreenEffects.OnBlackscreenFadeFinishedCallback += (Action) (() => MyAsyncSaving.Start(customName: saveName));
      else
        MyAsyncSaving.Start(customName: saveName);
      return true;
    })));

    private static void OnSnapshotDone(bool snapshotSuccess, MySessionSnapshot snapshot)
    {
      if (snapshotSuccess)
      {
        Func<bool> screenshotTaken = (Func<bool>) null;
        string str = (string) null;
        if (!Sandbox.Engine.Platform.Game.IsDedicated && !MySandboxGame.Config.SyncRendering)
        {
          str = MySession.Static.ThumbPath;
          try
          {
            if (File.Exists(str))
              File.Delete(str);
            MyAsyncSaving.m_screenshotTaken = false;
            MySandboxGame.Static.OnScreenshotTaken += new EventHandler(MyAsyncSaving.OnScreenshotTaken);
            MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), str, false, true, false);
            screenshotTaken = (Func<bool>) (() => MyAsyncSaving.m_screenshotTaken);
          }
          catch (Exception ex)
          {
            MySandboxGame.Log.WriteLine("Could not take session thumb screenshot. Exception:");
            MySandboxGame.Log.WriteLine(ex);
          }
        }
        snapshot.SaveParallel(screenshotTaken, str, (Action) (() => MyAsyncSaving.SaveFinished(snapshot)));
      }
      else
      {
        MyLog.Default.WriteLine("OnSnapshotDone: failed to save the world " + Environment.StackTrace);
        if (!Sandbox.Engine.Platform.Game.IsDedicated)
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.WorldNotSaved), (object) MySession.Static.Name), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
        MyAsyncSaving.PopInProgress();
      }
      if (MyAsyncSaving.m_callbackOnFinished != null)
        MyAsyncSaving.m_callbackOnFinished();
      MyAsyncSaving.m_callbackOnFinished = (Action) null;
    }

    private static void OnScreenshotTaken(object sender, System.EventArgs e)
    {
      MySandboxGame.Static.OnScreenshotTaken -= new EventHandler(MyAsyncSaving.OnScreenshotTaken);
      MyAsyncSaving.m_screenshotTaken = true;
    }

    private static void SaveFinished(MySessionSnapshot snapshot)
    {
      if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
      {
        if (snapshot.SavingSuccess)
        {
          IEnumerable<MyCharacter> savedCharacters = MySession.Static.SavedCharacters;
          if (savedCharacters != null)
          {
            foreach (MyCharacter character in savedCharacters)
            {
              if (character.Definition.UsableByPlayer)
                MyLocalCache.SaveInventoryConfig(character);
            }
          }
          MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.WorldSaved);
          myHudNotification.SetTextFormatArguments((object) MySession.Static.Name);
          MyHud.Notifications.Add((MyHudNotificationBase) myHudNotification);
        }
        else
        {
          MyLog.Default.WriteLine("SaveFinished: failed to save the world " + Environment.StackTrace);
          if (!MyAsyncSaving.m_saveErrorIsShown)
          {
            MyAsyncSaving.m_saveErrorIsShown = true;
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: new StringBuilder().AppendFormat(MyTexts.GetString(!snapshot.TooLongPath ? MyCloudHelper.GetErrorMessage(snapshot.CloudResult, new MyStringId?(MyCommonTexts.WorldNotSaved)) : MyCloudHelper.GetErrorMessage(snapshot.CloudResult, new MyStringId?(MyCommonTexts.WorldNotSavedPathTooLong))), (object) MySession.Static.Name), messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), onClosing: ((Action) (() => MyAsyncSaving.m_saveErrorIsShown = false))));
          }
        }
      }
      MyAsyncSaving.PopInProgress();
    }
  }
}
