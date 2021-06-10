// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenDebugOfficial
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Utils;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Gui
{
  internal class MyGuiScreenDebugOfficial : MyGuiScreenDebugBase
  {
    private static readonly Vector2 SCREEN_SIZE = new Vector2(0.4f, 1.2f);
    private static readonly float HIDDEN_PART_RIGHT = 0.04f;

    public override string GetFriendlyName() => nameof (MyGuiScreenDebugOfficial);

    public MyGuiScreenDebugOfficial()
      : base(new Vector2(MyGuiManager.GetMaxMouseCoord().X - MyGuiScreenDebugOfficial.SCREEN_SIZE.X * 0.5f + MyGuiScreenDebugOfficial.HIDDEN_PART_RIGHT, 0.5f), new Vector2?(MyGuiScreenDebugOfficial.SCREEN_SIZE), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), false)
    {
      this.CanBeHidden = true;
      this.CanHideOthers = false;
      this.m_canCloseInCloseAllScreenCalls = true;
      this.m_canShareInput = true;
      this.m_isTopScreen = false;
      this.m_isTopMostScreen = false;
      this.RecreateControls(true);
    }

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      Vector2 vector2_1 = new Vector2(-0.05f, 0.0f);
      Vector2 vector2_2 = new Vector2(0.02f, 0.02f);
      Vector2 vector2_3 = new Vector2(0.008f, 0.005f);
      float num1 = 0.8f;
      float num2 = 0.02f;
      float usableWidth = (float) ((double) MyGuiScreenDebugOfficial.SCREEN_SIZE.X - (double) MyGuiScreenDebugOfficial.HIDDEN_PART_RIGHT - (double) vector2_2.X * 2.0);
      float y = (float) (((double) MyGuiScreenDebugOfficial.SCREEN_SIZE.Y - 1.0) / 2.0);
      this.m_currentPosition = -this.m_size.Value / 2f;
      this.m_currentPosition = this.m_currentPosition + vector2_2;
      this.m_currentPosition.Y += y;
      this.m_scale = num1;
      this.AddCaption(MyCommonTexts.ScreenDebugOfficial_Caption, new Vector4?(Color.White.ToVector4()), new Vector2?(vector2_2 + new Vector2(-MyGuiScreenDebugOfficial.HIDDEN_PART_RIGHT, y)));
      this.m_currentPosition.Y += MyGuiConstants.SCREEN_CAPTION_DELTA_Y * 2f;
      this.AddCheckBox(MyCommonTexts.ScreenDebugOfficial_EnableDebugDraw, (Func<bool>) (() => MyDebugDrawSettings.ENABLE_DEBUG_DRAW), (Action<bool>) (b => MyDebugDrawSettings.ENABLE_DEBUG_DRAW = b), color: new Vector4?(Color.White.ToVector4()), checkBoxOffset: new Vector2?(vector2_1));
      this.m_currentPosition.Y += num2;
      this.AddCheckBox(MyCommonTexts.ScreenDebugOfficial_ModelDummies, (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_MODEL_DUMMIES = b), color: new Vector4?(Color.White.ToVector4()), checkBoxOffset: new Vector2?(vector2_1));
      this.AddCheckBox(MyCommonTexts.ScreenDebugOfficial_MountPoints, (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS), (Action<bool>) (b => MyDebugDrawSettings.DEBUG_DRAW_MOUNT_POINTS = b), color: new Vector4?(Color.White.ToVector4()), checkBoxOffset: new Vector2?(vector2_1));
      this.AddCheckBox(MyCommonTexts.ScreenDebugOfficial_PhysicsPrimitives, (Func<bool>) (() => MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES), (Action<bool>) (b =>
      {
        MyDebugDrawSettings.DEBUG_DRAW_PHYSICS |= b;
        MyDebugDrawSettings.DEBUG_DRAW_PHYSICS_SHAPES = b;
      }), color: new Vector4?(Color.White.ToVector4()), checkBoxOffset: new Vector2?(vector2_1));
      this.m_currentPosition.Y += num2;
      this.CreateDebugButton(usableWidth, MyCommonTexts.ScreenDebugOfficial_ReloadTextures, new Action<MyGuiControlButton>(this.ReloadTextures));
      this.CreateDebugButton(usableWidth, MyCommonTexts.ScreenDebugOfficial_ReloadModels, new Action<MyGuiControlButton>(this.ReloadModels));
      this.CreateDebugButton(usableWidth, MyCommonTexts.ScreenDebugOfficial_SavePrefab, new Action<MyGuiControlButton>(this.SavePrefab), MyClipboardComponent.Static != null && MyClipboardComponent.Static.Clipboard.HasCopiedGrids(), new MyStringId?(MyCommonTexts.ToolTipSaveShip));
      this.AddSubcaption(MyTexts.GetString(MyCommonTexts.ScreenDebugOfficial_ErrorLogCaption), new Vector4?(Color.White.ToVector4()), new Vector2?(new Vector2(-MyGuiScreenDebugOfficial.HIDDEN_PART_RIGHT, 0.0f)));
      this.CreateDebugButton(usableWidth, MyCommonTexts.ScreenDebugOfficial_OpenErrorLog, new Action<MyGuiControlButton>(this.CreateErrorLogScreen));
      this.CreateDebugButton(usableWidth, MyCommonTexts.ScreenDebugOfficial_CopyErrorLogToClipboard, new Action<MyGuiControlButton>(this.CopyErrorLogToClipboard));
      this.m_currentPosition.Y += num2;
      Vector2 vector2_4 = MyGuiManager.GetMaxMouseCoord() / 2f - this.m_currentPosition;
      vector2_4.X = usableWidth;
      vector2_4.Y -= vector2_2.Y;
      this.m_currentPosition.X += vector2_3.X / 2f;
      MyGuiControlPanel myGuiControlPanel = new MyGuiControlPanel(new Vector2?(this.m_currentPosition - vector2_3), new Vector2?(vector2_4 + new Vector2(vector2_3.X, vector2_3.Y * 2f)), originAlign: MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
      myGuiControlPanel.BackgroundTexture = MyGuiConstants.TEXTURE_SCROLLABLE_LIST;
      this.Controls.Add((MyGuiControlBase) myGuiControlPanel);
      MyGuiControlMultilineText controlMultilineText = this.AddMultilineText(new Vector2?(vector2_4));
      if (MyDefinitionErrors.GetErrors().Count<MyDefinitionErrors.Error>() == 0)
      {
        controlMultilineText.AppendText(MyTexts.Get(MyCommonTexts.ScreenDebugOfficial_NoErrorText));
      }
      else
      {
        ListReader<MyDefinitionErrors.Error> errors = MyDefinitionErrors.GetErrors();
        Dictionary<string, Tuple<int, TErrorSeverity>> dictionary = new Dictionary<string, Tuple<int, TErrorSeverity>>();
        foreach (MyDefinitionErrors.Error error in errors)
        {
          string key = error.ModName ?? "Local Content";
          if (dictionary.ContainsKey(key))
          {
            if (dictionary[key].Item2 == error.Severity)
            {
              Tuple<int, TErrorSeverity> tuple = dictionary[key];
              dictionary[key] = new Tuple<int, TErrorSeverity>(tuple.Item1 + 1, tuple.Item2);
            }
          }
          else
            dictionary[key] = new Tuple<int, TErrorSeverity>(1, error.Severity);
        }
        List<Tuple<string, int, TErrorSeverity>> tupleList = new List<Tuple<string, int, TErrorSeverity>>();
        foreach (KeyValuePair<string, Tuple<int, TErrorSeverity>> keyValuePair in dictionary)
          tupleList.Add(new Tuple<string, int, TErrorSeverity>(keyValuePair.Key, keyValuePair.Value.Item1, keyValuePair.Value.Item2));
        Comparison<Tuple<string, int, TErrorSeverity>> comparison = (Comparison<Tuple<string, int, TErrorSeverity>>) ((e1, e2) => e2.Item3 - e1.Item3);
        tupleList.Sort(comparison);
        foreach (Tuple<string, int, TErrorSeverity> tuple in tupleList)
        {
          StringBuilder text = new StringBuilder();
          text.Append(tuple.Item1);
          text.Append(" [");
          if (tuple.Item3 == TErrorSeverity.Critical)
          {
            text.Append(MyDefinitionErrors.Error.GetSeverityName(tuple.Item3, false));
            text.Append("]");
          }
          else
          {
            text.Append(tuple.Item2.ToString());
            text.Append(" ");
            text.Append(MyDefinitionErrors.Error.GetSeverityName(tuple.Item3, tuple.Item2 != 1));
            text.Append("]");
          }
          controlMultilineText.AppendText(text, controlMultilineText.Font, controlMultilineText.TextScaleWithLanguage, MyDefinitionErrors.Error.GetSeverityColor(tuple.Item3).ToVector4());
          controlMultilineText.AppendLine();
        }
      }
    }

    private void CreateDebugButton(
      float usableWidth,
      MyStringId text,
      Action<MyGuiControlButton> onClick,
      bool enabled = true,
      MyStringId? tooltip = null)
    {
      MyGuiControlButton guiControlButton = this.AddButton(MyTexts.Get(text), onClick);
      guiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
      guiControlButton.TextScale = this.m_scale;
      guiControlButton.Size = new Vector2(usableWidth, guiControlButton.Size.Y);
      guiControlButton.Position = guiControlButton.Position + new Vector2((float) (-(double) MyGuiScreenDebugOfficial.HIDDEN_PART_RIGHT / 2.0), 0.0f);
      guiControlButton.Enabled = enabled;
      if (!tooltip.HasValue)
        return;
      guiControlButton.SetToolTip(tooltip.Value);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyInput.Static.IsNewKeyPressed(MyKeys.F11))
      {
        if (MySession.Static.IsServer && (MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId) || MySession.Static.IsUserAdmin(Sync.MyId)))
          MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenScriptingTools());
        this.CloseScreen();
      }
      if (!MyInput.Static.IsNewKeyPressed(MyKeys.F12) && !MyInput.Static.IsNewKeyPressed(MyKeys.F10))
        return;
      this.CloseScreen();
    }

    private void CreateErrorLogScreen(MyGuiControlButton obj) => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenDebugErrors());

    private void ReloadTextures(MyGuiControlButton obj)
    {
      MyRenderProxy.ReloadTextures();
      MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotificationDebug("Reloaded all textures in the game (modder only feature)", font: "Red"));
    }

    private void ReloadModels(MyGuiControlButton obj)
    {
      MyRenderProxy.ReloadModels();
      MyHud.Notifications.Add((MyHudNotificationBase) new MyHudNotificationDebug("Reloaded all models in the game (modder only feature)", font: "Red"));
    }

    private void OpenBotsScreen(MyGuiControlButton obj) => MyGuiSandbox.AddScreen((MyGuiScreenBase) new MyGuiScreenBotSettings());

    private void SavePrefab(MyGuiControlButton obj)
    {
      string name = MyUtils.StripInvalidChars(MyClipboardComponent.Static.Clipboard.CopiedGridsName);
      string path = Path.Combine(MyFileSystem.UserDataPath, "Export", name + ".sbc");
      int num = 1;
      try
      {
        while (MyFileSystem.FileExists(path))
        {
          path = Path.Combine(MyFileSystem.UserDataPath, "Export", name + "_" + (object) num + ".sbc");
          ++num;
        }
        MyClipboardComponent.Static.Clipboard.SaveClipboardAsPrefab(name, path);
      }
      catch (Exception ex)
      {
        MySandboxGame.Log.WriteLine(string.Format("Failed to write prefab at file {0}, message: {1}, stack:{2}", (object) path, (object) ex.Message, (object) ex.StackTrace));
      }
    }

    private void CopyErrorLogToClipboard(MyGuiControlButton obj)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (MyDefinitionErrors.GetErrors().Count<MyDefinitionErrors.Error>() == 0)
        stringBuilder.Append((object) MyTexts.Get(MyCommonTexts.ScreenDebugOfficial_NoErrorText));
      foreach (MyDefinitionErrors.Error error in MyDefinitionErrors.GetErrors())
      {
        stringBuilder.Append(error.ToString());
        stringBuilder.AppendLine();
      }
      MyVRage.Platform.System.Clipboard = stringBuilder.ToString();
    }
  }
}
