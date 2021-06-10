// Decompiled with JetBrains decompiler
// Type: Sandbox.Graphics.GUI.MyGuiSandbox
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Networking;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Input;
using VRage.Plugins;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
  public static class MyGuiSandbox
  {
    public static Regex urlRgx = new Regex("^(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?$");
    internal static IMyGuiSandbox Gui = (IMyGuiSandbox) new MyNullGui();
    private static Dictionary<Type, Type> m_createdScreenTypes = new Dictionary<Type, Type>();
    public static int TotalGamePlayTimeInMilliseconds;
    public static Action<object> GuiControlCreated;
    public static Action<object> GuiControlRemoved;
    private static Regex[] WWW_WHITELIST = new Regex[4]
    {
      new Regex("^(http[s]{0,1}://){0,1}[^/]*youtube.com/.*", RegexOptions.IgnoreCase),
      new Regex("^(http[s]{0,1}://){0,1}[^/]*youtu.be/.*", RegexOptions.IgnoreCase),
      new Regex("^(http[s]{0,1}://){0,1}[^/]*steamcommunity.com/.*", RegexOptions.IgnoreCase),
      new Regex("^(http[s]{0,1}://){0,1}[^/]*forum[s]{0,1}.keenswh.com/.*", RegexOptions.IgnoreCase)
    };
    private const int m_logLatency = 1800;
    private static int m_frame = 0;
    private static Stopwatch m_timer = new Stopwatch();
    private static double m_pastDrawTime;
    private static double m_pastUpdateTime;

    public static void SetMouseCursorVisibility(bool visible, bool changePosition = true) => MyGuiSandbox.Gui.SetMouseCursorVisibility(visible, changePosition);

    public static Vector2 MouseCursorPosition => MyGuiSandbox.Gui.MouseCursorPosition;

    public static Action<float, Vector2> DrawGameLogoHandler
    {
      get => MyGuiSandbox.Gui.DrawGameLogoHandler;
      set => MyGuiSandbox.Gui.DrawGameLogoHandler = value;
    }

    private static void AnselWarningMessage(bool pauseAllowed, bool spectatorEnabled) => MySandboxGame.Static.Invoke((Action) (() => MyGuiSandbox.AnselWarningMessageInternal(pauseAllowed, spectatorEnabled)), nameof (AnselWarningMessage));

    private static void AnselWarningMessageInternal(bool pauseAllowed, bool spectatorEnabled)
    {
      if (pauseAllowed && spectatorEnabled)
        return;
      StringBuilder messageText = new StringBuilder();
      if (!pauseAllowed)
      {
        messageText.Append((object) MyTexts.Get(MyCommonTexts.MessageBoxTextAnselCannotPauseOnlineGame));
        messageText.AppendLine("");
      }
      if (!spectatorEnabled)
      {
        messageText.Append((object) MyTexts.Get(MyCommonTexts.MessageBoxTextAnselSpectatorDisabled));
        messageText.AppendLine("");
      }
      messageText.Append((object) MyTexts.Get(MyCommonTexts.MessageBoxTextAnselTimeout));
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.NONE_TIMEOUT, messageText: messageText, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionWarning), timeoutInMiliseconds: 4000));
    }

    public static bool Ansel_IsSpectatorEnabled() => MyGuiScreenGamePlay.SpectatorEnabled;

    public static void LoadData(bool nullGui)
    {
      MyVRage.Platform.Ansel.WarningMessageDelegate += new Action<bool, bool>(MyGuiSandbox.AnselWarningMessage);
      MyVRage.Platform.Ansel.IsSpectatorEnabledDelegate += new Func<bool>(MyGuiSandbox.Ansel_IsSpectatorEnabled);
      if (!nullGui)
        MyGuiSandbox.Gui = (IMyGuiSandbox) new MyDX9Gui();
      MyGuiSandbox.Gui.LoadData();
    }

    public static void LoadContent() => MyGuiSandbox.Gui.LoadContent();

    public static bool IsUrlWhitelisted(string wwwLink)
    {
      foreach (Regex regex in MyGuiSandbox.WWW_WHITELIST)
      {
        if (regex.IsMatch(wwwLink))
          return true;
      }
      return false;
    }

    public static void OpenUrlWithFallback(
      string url,
      string urlFriendlyName,
      bool useWhitelist = false,
      Action<bool> onDone = null)
    {
      if (useWhitelist && !MyGuiSandbox.IsUrlWhitelisted(url))
      {
        MySandboxGame.Log.WriteLine("URL NOT ALLOWED: " + url);
        onDone.InvokeIfNotNull<bool>(false);
      }
      else
      {
        StringBuilder confirmMessageBrowser = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.MessageBoxTextOpenBrowser), (object) urlFriendlyName, (object) MySession.GameServiceName);
        MyGuiSandbox.OpenUrl(url, UrlOpenMode.SteamOrExternalWithConfirm, confirmMessageBrowser, onDone: onDone);
      }
    }

    public static bool IsUrlValid(string url) => MyGuiSandbox.urlRgx.IsMatch(url);

    private static bool OpenSteamOverlay(string url)
    {
      if (!MyGameService.IsOverlayBrowserAvailable)
        return false;
      MyGameService.OpenOverlayUrl(url);
      return true;
    }

    public static void OpenUrl(
      string url,
      UrlOpenMode openMode,
      StringBuilder confirmMessageBrowser = null,
      StringBuilder confirmCaptionBrowser = null,
      StringBuilder confirmMessageOverlay = null,
      StringBuilder confirmCaptionOverlay = null,
      Action<bool> onDone = null)
    {
      int num = (uint) (openMode & UrlOpenMode.SteamOverlay) > 0U ? 1 : 0;
      bool flag1 = (uint) (openMode & UrlOpenMode.ExternalBrowser) > 0U;
      bool flag2 = (uint) (openMode & UrlOpenMode.ConfirmExternal) > 0U;
      bool flag3 = false;
      if (num != 0)
      {
        if (flag2 && confirmMessageOverlay != null)
        {
          if (MyGameService.IsOverlayBrowserAvailable)
          {
            StringBuilder messageCaption = confirmCaptionOverlay ?? MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
            MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: confirmMessageOverlay, messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
            {
              if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
              {
                MyGuiSandbox.OpenSteamOverlay(url);
                onDone.InvokeIfNotNull<bool>(true);
              }
              else
                onDone.InvokeIfNotNull<bool>(false);
            }))));
            return;
          }
        }
        else
          flag3 = MyGuiSandbox.OpenSteamOverlay(url);
      }
      if (!flag3 & flag1)
      {
        if (flag2)
        {
          StringBuilder messageCaption = confirmCaptionBrowser ?? MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(buttonType: MyMessageBoxButtonsType.YES_NO, messageText: (confirmMessageBrowser ?? new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextOpenBrowser, (object) url)), messageCaption: messageCaption, callback: ((Action<MyGuiScreenMessageBox.ResultEnum>) (retval =>
          {
            if (retval == MyGuiScreenMessageBox.ResultEnum.YES)
              onDone.InvokeIfNotNull<bool>(MyGuiSandbox.OpenExternalBrowser(url));
            else
              onDone.InvokeIfNotNull<bool>(false);
          }))));
        }
        else
        {
          bool flag4 = MyGuiSandbox.OpenExternalBrowser(url);
          onDone.InvokeIfNotNull<bool>(flag4);
        }
      }
      else
        onDone.InvokeIfNotNull<bool>(true);
    }

    private static bool OpenExternalBrowser(string url)
    {
      if (MyVRage.Platform.System.OpenUrl(url))
        return true;
      StringBuilder messageText = new StringBuilder();
      messageText.AppendFormat(MyTexts.GetString(MyCommonTexts.TitleFailedToStartInternetBrowser), (object) url);
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.TitleFailedToStartInternetBrowser);
      MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(messageText: messageText, messageCaption: messageCaption));
      return false;
    }

    public static void UnloadContent() => MyGuiSandbox.Gui.UnloadContent();

    public static void SwitchDebugScreensEnabled() => MyGuiSandbox.Gui.SwitchDebugScreensEnabled();

    public static void ShowModErrors() => MyGuiSandbox.Gui.ShowModErrors();

    public static bool IsDebugScreenEnabled() => MyGuiSandbox.Gui.IsDebugScreenEnabled();

    public static MyGuiScreenBase CreateScreen(Type screenType, params object[] args) => Activator.CreateInstance(screenType, args) as MyGuiScreenBase;

    public static T CreateScreen<T>(params object[] args) where T : MyGuiScreenBase
    {
      Type createdType = (Type) null;
      if (!MyGuiSandbox.m_createdScreenTypes.TryGetValue(typeof (T), out createdType))
      {
        Type key = typeof (T);
        createdType = key;
        MyGuiSandbox.ChooseScreenType<T>(ref createdType, MyPlugins.GameAssembly);
        MyGuiSandbox.ChooseScreenType<T>(ref createdType, MyPlugins.SandboxAssembly);
        MyGuiSandbox.ChooseScreenType<T>(ref createdType, MyPlugins.UserAssemblies);
        MyGuiSandbox.m_createdScreenTypes[key] = createdType;
      }
      return Activator.CreateInstance(createdType, args) as T;
    }

    private static void ChooseScreenType<T>(ref Type createdType, Assembly[] assemblies) where T : MyGuiScreenBase
    {
      if (assemblies == null)
        return;
      foreach (Assembly assembly in assemblies)
        MyGuiSandbox.ChooseScreenType<T>(ref createdType, assembly);
    }

    private static void ChooseScreenType<T>(ref Type createdType, Assembly assembly) where T : MyGuiScreenBase
    {
      if (assembly == (Assembly) null)
        return;
      foreach (Type type in assembly.GetTypes())
      {
        if (typeof (T).IsAssignableFrom(type))
        {
          createdType = type;
          break;
        }
      }
    }

    public static void AddScreen(MyGuiScreenBase screen)
    {
      MyGuiSandbox.Gui.AddScreen(screen);
      if (MyGuiSandbox.GuiControlCreated != null)
        MyGuiSandbox.GuiControlCreated((object) screen);
      screen.Closed += (MyGuiScreenBase.ScreenHandler) ((x, isUnloading) =>
      {
        if (MyGuiSandbox.GuiControlRemoved == null)
          return;
        MyGuiSandbox.GuiControlRemoved((object) x);
      });
      if (MyAPIGateway.GuiControlCreated == null)
        return;
      MyAPIGateway.GuiControlCreated((object) screen);
    }

    public static void InsertScreen(MyGuiScreenBase screen, int index)
    {
      MyGuiSandbox.Gui.InsertScreen(screen, index);
      if (MyGuiSandbox.GuiControlCreated != null)
        MyGuiSandbox.GuiControlCreated((object) screen);
      screen.Closed += (MyGuiScreenBase.ScreenHandler) ((x, isUnloading) =>
      {
        if (MyGuiSandbox.GuiControlRemoved == null)
          return;
        MyGuiSandbox.GuiControlRemoved((object) x);
      });
      if (MyAPIGateway.GuiControlCreated == null)
        return;
      MyAPIGateway.GuiControlCreated((object) screen);
    }

    public static void RemoveScreen(MyGuiScreenBase screen)
    {
      MyGuiSandbox.Gui.RemoveScreen(screen);
      if (MyGuiSandbox.GuiControlRemoved == null)
        return;
      MyGuiSandbox.GuiControlRemoved((object) screen);
    }

    public static void HandleInput() => MyGuiSandbox.Gui.HandleInput();

    public static void HandleInputAfterSimulation() => MyGuiSandbox.Gui.HandleInputAfterSimulation();

    public static void Update(int totalTimeInMS)
    {
      MyGuiSandbox.m_timer.Restart();
      MyGuiSandbox.Gui.Update(totalTimeInMS);
      MyGuiSandbox.m_timer.Stop();
      MyGuiSandbox.m_pastUpdateTime += MyGuiSandbox.m_timer.Elapsed.TotalMilliseconds;
    }

    public static void Draw()
    {
      MyGuiSandbox.m_timer.Restart();
      MyGuiSandbox.Gui.Draw();
      MyGuiSandbox.m_timer.Stop();
      MyGuiSandbox.m_pastDrawTime += MyGuiSandbox.m_timer.Elapsed.TotalMilliseconds;
      if (++MyGuiSandbox.m_frame != 1800)
        return;
      MyGuiSandbox.m_frame = 0;
      MyLog.Default.WriteLine(string.Format("GUI Stats: Update {0}, Draw {1}", (object) (MyGuiSandbox.m_pastUpdateTime / 1800.0), (object) (MyGuiSandbox.m_pastDrawTime / 1800.0)));
      MyGuiSandbox.m_pastUpdateTime = MyGuiSandbox.m_pastDrawTime = 0.0;
    }

    public static void BackToIntroLogos(Action afterLogosAction) => MyGuiSandbox.Gui.BackToIntroLogos(afterLogosAction);

    public static void BackToMainMenu() => MyGuiSandbox.Gui.BackToMainMenu();

    public static float GetDefaultTextScaleWithLanguage() => MyGuiSandbox.Gui.GetDefaultTextScaleWithLanguage();

    public static void TakeScreenshot(
      int width,
      int height,
      string saveToPath = null,
      bool ignoreSprites = false,
      bool showNotification = true)
    {
      MyGuiSandbox.Gui.TakeScreenshot(width, height, saveToPath, ignoreSprites, showNotification);
    }

    public static MyGuiScreenMessageBox CreateMessageBox(
      MyMessageBoxStyleEnum styleEnum = MyMessageBoxStyleEnum.Error,
      MyMessageBoxButtonsType buttonType = MyMessageBoxButtonsType.OK,
      StringBuilder messageText = null,
      StringBuilder messageCaption = null,
      MyStringId? okButtonText = null,
      MyStringId? cancelButtonText = null,
      MyStringId? yesButtonText = null,
      MyStringId? noButtonText = null,
      Action<MyGuiScreenMessageBox.ResultEnum> callback = null,
      int timeoutInMiliseconds = 0,
      MyGuiScreenMessageBox.ResultEnum focusedResult = MyGuiScreenMessageBox.ResultEnum.YES,
      bool canHideOthers = true,
      Vector2? size = null,
      bool useOpacity = true,
      Vector2? position = null,
      bool focusable = true,
      bool canBeHidden = false,
      Action onClosing = null)
    {
      int num1 = (int) styleEnum;
      int num2 = (int) buttonType;
      StringBuilder messageText1 = messageText;
      StringBuilder messageCaption1 = messageCaption;
      MyStringId? nullable = okButtonText;
      MyStringId okButtonText1 = nullable ?? MyCommonTexts.Ok;
      nullable = cancelButtonText;
      MyStringId cancelButtonText1 = nullable ?? MyCommonTexts.Cancel;
      nullable = yesButtonText;
      MyStringId yesButtonText1 = nullable ?? MyCommonTexts.Yes;
      nullable = noButtonText;
      MyStringId noButtonText1 = nullable ?? MyCommonTexts.No;
      Action<MyGuiScreenMessageBox.ResultEnum> callback1 = callback;
      int timeoutInMiliseconds1 = timeoutInMiliseconds;
      int num3 = (int) focusedResult;
      int num4 = canHideOthers ? 1 : 0;
      Vector2? size1 = size;
      double num5 = useOpacity ? (double) MySandboxGame.Config.UIBkOpacity : 1.0;
      double num6 = useOpacity ? (double) MySandboxGame.Config.UIOpacity : 1.0;
      Vector2? position1 = position;
      int num7 = focusable ? 1 : 0;
      int num8 = canBeHidden ? 1 : 0;
      Action onClosing1 = onClosing;
      return new MyGuiScreenMessageBox((MyMessageBoxStyleEnum) num1, (MyMessageBoxButtonsType) num2, messageText1, messageCaption1, okButtonText1, cancelButtonText1, yesButtonText1, noButtonText1, callback1, timeoutInMiliseconds1, (MyGuiScreenMessageBox.ResultEnum) num3, num4 != 0, size1, (float) num5, (float) num6, position1, num7 != 0, num8 != 0, onClosing1);
    }

    public static void Show(StringBuilder text, MyStringId caption = default (MyStringId), MyMessageBoxStyleEnum type = MyMessageBoxStyleEnum.Error) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(type, messageText: text, messageCaption: MyTexts.Get(caption)));

    public static void Show(MyStringId text, MyStringId caption = default (MyStringId), MyMessageBoxStyleEnum type = MyMessageBoxStyleEnum.Error) => MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(type, messageText: MyTexts.Get(text), messageCaption: MyTexts.Get(caption)));

    public static void DrawGameLogo(float transitionAlpha, Vector2 position) => MyGuiSandbox.Gui.DrawGameLogo(transitionAlpha, position);

    public static void DrawBadge(
      string texture,
      float transitionAlpha,
      Vector2 position,
      Vector2 size)
    {
      MyGuiSandbox.Gui.DrawBadge(texture, transitionAlpha, position, size);
    }

    public static string GetKeyName(MyStringId control)
    {
      MyControl gameControl = MyInput.Static.GetGameControl(control);
      return gameControl != null ? gameControl.GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) : "";
    }
  }
}
