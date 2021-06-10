// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenEditor
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using VRage;
using VRage.Compression;
using VRage.FileSystem;
using VRage.Game;
using VRage.Input;
using VRage.Network;
using VRage.Scripting;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
  [StaticEventOwner]
  public class MyGuiScreenEditor : MyGuiScreenBase
  {
    private const string CODE_WRAPPER_BEFORE = "using System;\nusing System.Collections.Generic;\nusing VRageMath;\nusing VRage.Game;\nusing System.Text;\nusing Sandbox.ModAPI.Interfaces;\nusing Sandbox.ModAPI.Ingame;\nusing Sandbox.Game.EntityComponents;\nusing VRage.Game.Components;\nusing VRage.Collections;\nusing VRage.Game.ObjectBuilders.Definitions;\nusing VRage.Game.ModAPI.Ingame;\nusing SpaceEngineers.Game.ModAPI.Ingame;\npublic class Program: MyGridProgram\n{\n";
    private const string CODE_WRAPPER_AFTER = "\n}";
    private Action<VRage.Game.ModAPI.ResultEnum> m_resultCallback;
    private Action m_saveCodeCallback;
    private string m_description = "";
    private VRage.Game.ModAPI.ResultEnum m_screenResult = VRage.Game.ModAPI.ResultEnum.CANCEL;
    public const int MAX_NUMBER_CHARACTERS = 100000;
    private List<string> m_compilerErrors = new List<string>();
    private MyGuiControlMultilineText m_descriptionBox;
    private MyGuiControlCompositePanel m_descriptionBackgroundPanel;
    private MyGuiControlButton m_okButton;
    private MyGuiControlButton m_openWorkshopButton;
    private MyGuiControlButton m_checkCodeButton;
    private MyGuiControlButton m_help;
    private MyGuiControlLabel m_lineCounter;
    private MyGuiControlLabel m_TextTooLongMessage;
    private MyGuiControlLabel m_LetterCounter;
    private MyGuiControlMultilineEditableText m_editorWindow;
    private MyGuiScreenProgress m_progress;

    public MyGuiControlMultilineText Description => this.m_descriptionBox;

    public MyGuiScreenEditor(
      string description,
      Action<VRage.Game.ModAPI.ResultEnum> resultCallback,
      Action saveCodeCallback)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(1f, 0.9f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.m_description = description;
      this.m_saveCodeCallback = saveCodeCallback;
      this.m_resultCallback = resultCallback;
      this.CanBeHidden = true;
      this.CanHideOthers = true;
      this.m_closeOnEsc = true;
      this.EnabledBackgroundFade = true;
      this.CloseButtonEnabled = true;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenEditor);

    public override void RecreateControls(bool constructor)
    {
      base.RecreateControls(constructor);
      this.AddCaption(MySpaceTexts.ProgrammableBlock_CodeEditor_Title, captionOffset: new Vector2?(new Vector2(0.0f, 3f / 1000f)));
      MyGuiControlSeparatorList controlSeparatorList = new MyGuiControlSeparatorList();
      controlSeparatorList.AddHorizontal(new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.904999971389771 / 2.0), (float) ((double) this.m_size.Value.Y / 2.0 - 0.0750000029802322)), this.m_size.Value.X * 0.905f);
      Vector2 start = new Vector2(0.0f, 0.0f) - new Vector2((float) ((double) this.m_size.Value.X * 0.904999971389771 / 2.0), (float) (-(double) this.m_size.Value.Y / 2.0 + 0.123000003397465));
      controlSeparatorList.AddHorizontal(start, this.m_size.Value.X * 0.905f);
      this.Controls.Add((MyGuiControlBase) controlSeparatorList);
      Vector2? position1 = new Vector2?(new Vector2(-0.184f, 0.378f));
      Vector2? size1 = new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE);
      Vector4? colorMask1 = new Vector4?();
      StringBuilder stringBuilder1 = MyTexts.Get(MyCommonTexts.Ok);
      Action<MyGuiControlButton> action = new Action<MyGuiControlButton>(this.OkButtonClicked);
      string toolTip1 = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_CodeEditor_SaveExit_Tooltip);
      StringBuilder text1 = stringBuilder1;
      Action<MyGuiControlButton> onButtonClick1 = action;
      int? buttonIndex1 = new int?();
      this.m_okButton = new MyGuiControlButton(position1, size: size1, colorMask: colorMask1, toolTip: toolTip1, text: text1, onButtonClick: onButtonClick1, buttonIndex: buttonIndex1);
      this.Controls.Add((MyGuiControlBase) this.m_okButton);
      Vector2? position2 = new Vector2?(new Vector2(-1f / 1000f, 0.378f));
      Vector2? size2 = new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE);
      Vector4? colorMask2 = new Vector4?();
      StringBuilder stringBuilder2 = MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_CheckCode);
      string toolTip2 = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CheckCode_Tooltip);
      StringBuilder text2 = stringBuilder2;
      Action<MyGuiControlButton> onButtonClick2 = new Action<MyGuiControlButton>(this.CheckCodeButtonClicked);
      int? buttonIndex2 = new int?();
      this.m_checkCodeButton = new MyGuiControlButton(position2, size: size2, colorMask: colorMask2, toolTip: toolTip2, text: text2, onButtonClick: onButtonClick2, buttonIndex: buttonIndex2);
      this.Controls.Add((MyGuiControlBase) this.m_checkCodeButton);
      this.m_help = new MyGuiControlButton(new Vector2?(new Vector2(0.182f, 0.378f)), size: new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE), text: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_Help), onButtonClick: new Action<MyGuiControlButton>(this.HelpButtonClicked));
      this.m_help.SetToolTip(MySpaceTexts.ProgrammableBlock_Editor_HelpTooltip);
      this.Controls.Add((MyGuiControlBase) this.m_help);
      Vector2? position3 = new Vector2?(new Vector2(0.365f, 0.378f));
      Vector2? size3 = new Vector2?(MyGuiConstants.BACK_BUTTON_SIZE);
      Vector4? colorMask3 = new Vector4?();
      StringBuilder stringBuilder3 = MyTexts.Get(MyCommonTexts.ProgrammableBlock_Editor_BrowseScripts);
      string toolTip3 = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_BrowseWorkshop_Tooltip);
      StringBuilder text3 = stringBuilder3;
      Action<MyGuiControlButton> onButtonClick3 = new Action<MyGuiControlButton>(this.OpenWorkshopButtonClicked);
      int? buttonIndex3 = new int?();
      this.m_openWorkshopButton = new MyGuiControlButton(position3, size: size3, colorMask: colorMask3, toolTip: toolTip3, text: text3, onButtonClick: onButtonClick3, buttonIndex: buttonIndex3);
      this.Controls.Add((MyGuiControlBase) this.m_openWorkshopButton);
      this.m_descriptionBackgroundPanel = new MyGuiControlCompositePanel();
      this.m_descriptionBackgroundPanel.BackgroundTexture = MyGuiConstants.TEXTURE_RECTANGLE_DARK_BORDER;
      this.m_descriptionBackgroundPanel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_descriptionBackgroundPanel.Position = new Vector2(-0.451f, -0.356f);
      this.m_descriptionBackgroundPanel.Size = new Vector2(0.902f, 0.664f);
      this.Controls.Add((MyGuiControlBase) this.m_descriptionBackgroundPanel);
      this.m_descriptionBox = this.AddMultilineText(new Vector2?(new Vector2(0.5f, 0.44f)), new Vector2?(new Vector2(-0.446f, -0.356f)));
      this.m_descriptionBox.TextPadding = new MyGuiBorderThickness(0.012f, 0.0f, 0.0f, 0.0f);
      this.m_descriptionBox.TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_descriptionBox.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
      this.m_descriptionBox.Text = new StringBuilder(this.m_description);
      this.m_descriptionBox.Position = Vector2.Zero;
      this.m_descriptionBox.Size = this.m_descriptionBackgroundPanel.Size - new Vector2(0.0f, 0.03f);
      this.m_descriptionBox.Position = new Vector2(0.0f, -0.024f);
      this.m_lineCounter = new MyGuiControlLabel(new Vector2?(new Vector2(-0.45f, 0.357f)), text: string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), (object) 1, (object) this.m_editorWindow.GetTotalNumLines()), font: "White");
      this.Elements.Add((MyGuiControlBase) this.m_lineCounter);
      this.m_LetterCounter = new MyGuiControlLabel(new Vector2?(new Vector2(-0.45f, -0.397f)), font: "White");
      this.Elements.Add((MyGuiControlBase) this.m_LetterCounter);
      this.m_TextTooLongMessage = new MyGuiControlLabel(new Vector2?(new Vector2(-0.34f, -0.4f)), font: "Red");
      this.Elements.Add((MyGuiControlBase) this.m_TextTooLongMessage);
      Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
      MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2?(new Vector2(this.m_TextTooLongMessage.PositionX, this.m_lineCounter.PositionY)));
      myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
      this.Controls.Add((MyGuiControlBase) myGuiControlLabel);
      this.GamepadHelpTextId = new MyStringId?(MySpaceTexts.PbEditor_Help_Screen);
      this.FocusedControl = (MyGuiControlBase) this.m_descriptionBox;
      if (MyVRage.Platform.ImeProcessor == null)
        return;
      MyVRage.Platform.ImeProcessor.RegisterActiveScreen((IVRageGuiScreen) this);
    }

    protected MyGuiControlMultilineText AddMultilineText(
      Vector2? size = null,
      Vector2? offset = null,
      float textScale = 1f,
      bool selectable = false,
      MyGuiDrawAlignEnum textAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
      MyGuiDrawAlignEnum textBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
    {
      Vector2 vector2 = size ?? this.Size ?? new Vector2(1.2f, 0.5f);
      MyGuiControlMultilineEditableText multilineEditableText = new MyGuiControlMultilineEditableText(new Vector2?(vector2 / 2f + (offset ?? Vector2.Zero)), new Vector2?(vector2), new Vector4?(Color.White.ToVector4()), "White", textAlign: textAlign, textBoxAlign: textBoxAlign);
      multilineEditableText.IgnoreOffensiveText = true;
      if (MyPlatformGameSettings.IsMultilineEditableByGamepad)
        multilineEditableText.GamepadHelpTextId = MyCommonTexts.Gamepad_Help_MultiLineTextbox;
      this.m_editorWindow = multilineEditableText;
      this.Controls.Add((MyGuiControlBase) multilineEditableText);
      return (MyGuiControlMultilineText) multilineEditableText;
    }

    public bool TextTooLong() => this.m_editorWindow.Text.Length > 100000;

    public override bool CloseScreen(bool isUnloading = false)
    {
      this.CallResultCallback(this.m_screenResult);
      return base.CloseScreen(isUnloading);
    }

    public void SetDescription(string desc)
    {
      this.m_description = desc;
      this.m_descriptionBox.Clear();
      this.m_descriptionBox.Text = new StringBuilder(this.m_description);
    }

    public void AppendTextToDescription(string text, Vector4 color, string font = "White", float scale = 1f)
    {
      this.m_description += text;
      this.m_descriptionBox.AppendText(text, font, scale, color);
    }

    public void AppendTextToDescription(string text, string font = "White", float scale = 1f)
    {
      this.m_description += text;
      this.m_descriptionBox.AppendText(text, font, scale, Vector4.One);
    }

    private void HelpButtonClicked(MyGuiControlButton button) => MyGuiSandbox.OpenUrlWithFallback(MySteamConstants.URL_BROWSE_WORKSHOP_INGAMESCRIPTS_HELP, "Steam Workshop");

    private void OkButtonClicked(MyGuiControlButton button)
    {
      this.m_screenResult = VRage.Game.ModAPI.ResultEnum.OK;
      this.CloseScreen(false);
    }

    private void OpenWorkshopButtonClicked(MyGuiControlButton button)
    {
      this.m_openWorkshopButton.Enabled = false;
      this.m_checkCodeButton.Enabled = false;
      this.m_editorWindow.Enabled = false;
      this.m_okButton.Enabled = false;
      this.HideScreen();
      MyBlueprintUtils.OpenScriptScreen(new Action<string>(this.ScriptSelected), new Func<string>(this.GetCode), new Action(this.WorkshopWindowClosed));
    }

    private void OpenWorkshopButtonClicked() => this.OpenWorkshopButtonClicked((MyGuiControlButton) null);

    private string GetCode() => this.m_descriptionBox.Text.ToString();

    private void WorkshopWindowClosed()
    {
      if (MyVRage.Platform.ImeProcessor != null)
        MyVRage.Platform.ImeProcessor.RegisterActiveScreen((IVRageGuiScreen) this);
      this.UnhideScreen();
      this.FocusedControl = (MyGuiControlBase) this.m_descriptionBox;
      this.m_openWorkshopButton.Enabled = true;
      this.m_checkCodeButton.Enabled = true;
      this.m_editorWindow.Enabled = true;
      this.m_okButton.Enabled = true;
    }

    private void ScriptSelected(string scriptPath)
    {
      string input = (string) null;
      string extension = Path.GetExtension(scriptPath);
      if (extension == ".cs" && File.Exists(scriptPath))
        input = File.ReadAllText(scriptPath);
      else if (extension == ".bin")
      {
        foreach (string file in MyFileSystem.GetFiles(scriptPath, ".cs", MySearchOption.AllDirectories))
        {
          if (MyFileSystem.FileExists(file))
          {
            using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(file)))
              input = streamReader.ReadToEnd();
          }
        }
      }
      else if (MyFileSystem.IsDirectory(scriptPath))
      {
        foreach (string file in MyFileSystem.GetFiles(scriptPath, "*.cs", MySearchOption.AllDirectories))
        {
          if (MyFileSystem.FileExists(file))
          {
            using (StreamReader streamReader = new StreamReader(MyFileSystem.OpenRead(file)))
            {
              input = streamReader.ReadToEnd();
              break;
            }
          }
        }
      }
      else if (File.Exists(scriptPath))
      {
        try
        {
          using (MyZipArchive myZipArchive = MyZipArchive.OpenOnFile(scriptPath))
          {
            foreach (ZipArchiveEntry file in myZipArchive.Files)
            {
              if (Path.GetExtension(file.Name).ToLower() == ".cs")
              {
                using (StreamReader streamReader = new StreamReader(file.Open()))
                {
                  input = streamReader.ReadToEnd();
                  break;
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          MyLog.Default.WriteLine(ex);
        }
      }
      if (input == null)
        return;
      this.SetDescription(Regex.Replace(input, "\r\n", " \n"));
      this.m_lineCounter.Text = string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), (object) this.m_editorWindow.GetCurrentCarriageLine(), (object) this.m_editorWindow.GetTotalNumLines());
      this.m_openWorkshopButton.Enabled = true;
      this.m_checkCodeButton.Enabled = true;
      this.m_editorWindow.Enabled = true;
      this.m_okButton.Enabled = true;
    }

    private void CheckCodeButtonClicked(MyGuiControlButton button)
    {
      string str = this.Description.Text.ToString();
      this.m_compilerErrors.Clear();
      if (MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
      {
        this.ProcessCompilationResults(MyGuiScreenEditor.CompileProgram(str, this.m_compilerErrors));
      }
      else
      {
        MyMultiplayer.RaiseStaticEvent<byte[]>((Func<IMyEventOwner, Action<byte[]>>) (x => new Action<byte[]>(MyGuiScreenEditor.CompileProgramServer)), StringCompressor.CompressString(str));
        this.m_progress = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_CheckingCode));
        MyScreenManager.AddScreen((MyGuiScreenBase) this.m_progress);
      }
    }

    private void ProcessCompilationResults(bool success)
    {
      if (success)
      {
        if (this.m_compilerErrors.Count > 0)
        {
          StringBuilder stringBuilder = new StringBuilder();
          foreach (string compilerError in this.m_compilerErrors)
          {
            stringBuilder.Append(compilerError);
            stringBuilder.Append('\n');
          }
          MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenEditorError(stringBuilder.ToString()));
        }
        else
        {
          StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.ProgrammableBlock_CodeEditor_Title);
          MyGuiSandbox.AddScreen((MyGuiScreenBase) MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_Editor_CompilationOk), messageCaption: messageCaption));
        }
      }
      else
        MyScreenManager.AddScreen((MyGuiScreenBase) new MyGuiScreenEditorError(string.Join("\n", (IEnumerable<string>) this.m_compilerErrors)));
      if (MyVRage.Platform.ImeProcessor != null)
        MyVRage.Platform.ImeProcessor.RegisterActiveScreen((IVRageGuiScreen) this);
      this.FocusedControl = (MyGuiControlBase) this.m_descriptionBox;
    }

    public static bool CompileProgram(string program, List<string> errors)
    {
      if (!MyVRage.Platform.Scripting.IsRuntimeCompilationSupported)
      {
        errors.Add(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_NotSupported));
        return false;
      }
      if (string.IsNullOrEmpty(program))
        return false;
      List<Message> diagnostics;
      Assembly result = MyVRage.Platform.Scripting.CompileIngameScriptAsync(Path.Combine(MyFileSystem.UserDataPath, "EditorCode.dll"), program, out diagnostics, "PB Code editor", "Program", typeof (MyGridProgram).Name).Result;
      errors.Clear();
      errors.AddRange(diagnostics.OrderByDescending<Message, int>((Func<Message, int>) (m => !m.IsError ? 0 : 1)).Select<Message, string>((Func<Message, string>) (m => m.Text)));
      return result != (Assembly) null;
    }

    [Event(null, 432)]
    [Reliable]
    [Server]
    private static void CompileProgramServer(byte[] program)
    {
      List<string> errors = new List<string>();
      MyMultiplayer.RaiseStaticEvent<bool, List<string>>((Func<IMyEventOwner, Action<bool, List<string>>>) (x => new Action<bool, List<string>>(MyGuiScreenEditor.ReportCompilationResults)), MyGuiScreenEditor.CompileProgram(StringCompressor.DecompressString(program), errors), errors, MyEventContext.Current.Sender);
    }

    [Event(null, 441)]
    [Reliable]
    [Client]
    private static void ReportCompilationResults(bool success, List<string> messages)
    {
      MyGuiScreenEditor myGuiScreenEditor = MyScreenManager.Screens.OfType<MyGuiScreenEditor>().FirstOrDefault<MyGuiScreenEditor>();
      if (myGuiScreenEditor == null)
        return;
      myGuiScreenEditor.m_compilerErrors.AddRange((IEnumerable<string>) messages);
      myGuiScreenEditor.ProcessCompilationResults(success);
      MyScreenManager.RemoveScreen((MyGuiScreenBase) myGuiScreenEditor.m_progress);
      myGuiScreenEditor.m_progress = (MyGuiScreenProgress) null;
    }

    private string FormatError(string error)
    {
      try
      {
        char[] chArray = new char[4]{ ':', ')', '(', ',' };
        string[] strArray = error.Split(chArray);
        if (strArray.Length <= 2)
          return error;
        int num = Convert.ToInt32(strArray[2]) - this.m_editorWindow.MeasureNumLines("using System;\nusing System.Collections.Generic;\nusing VRageMath;\nusing VRage.Game;\nusing System.Text;\nusing Sandbox.ModAPI.Interfaces;\nusing Sandbox.ModAPI.Ingame;\nusing Sandbox.Game.EntityComponents;\nusing VRage.Game.Components;\nusing VRage.Collections;\nusing VRage.Game.ObjectBuilders.Definitions;\nusing VRage.Game.ModAPI.Ingame;\nusing SpaceEngineers.Game.ModAPI.Ingame;\npublic class Program: MyGridProgram\n{\n");
        string str = strArray[6];
        for (int index = 7; index < strArray.Length; ++index)
        {
          if (!string.IsNullOrWhiteSpace(strArray[index]))
            str = str + "," + strArray[index];
        }
        return string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CompilationFailedErrorFormat), (object) num, (object) str);
      }
      catch (Exception ex)
      {
      }
      return error;
    }

    public override bool Update(bool hasFocus)
    {
      if (hasFocus && this.m_editorWindow.CarriageMoved())
        this.m_lineCounter.Text = string.Format(MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_LineNo), (object) this.m_editorWindow.GetCurrentCarriageLine(), (object) this.m_editorWindow.GetTotalNumLines());
      if (hasFocus)
      {
        this.m_LetterCounter.Text = MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_CharacterLimit) + " " + string.Format("{0} / {1}", (object) this.m_editorWindow.Text.Length, (object) 100000);
        this.m_LetterCounter.Font = !this.TextTooLong() ? "White" : "Red";
        this.m_TextTooLongMessage.Text = this.TextTooLong() ? MyTexts.GetString(MySpaceTexts.ProgrammableBlock_Editor_TextTooLong) : "";
      }
      this.m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_checkCodeButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_help.Visible = !MyInput.Static.IsJoystickLastUsed;
      this.m_openWorkshopButton.Visible = !MyInput.Static.IsJoystickLastUsed;
      return base.Update(hasFocus);
    }

    public override void HandleInput(bool receivedFocusInThisUpdate)
    {
      base.HandleInput(receivedFocusInThisUpdate);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
        this.OkButtonClicked((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
        this.CheckCodeButtonClicked((MyGuiControlButton) null);
      if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
        this.HelpButtonClicked((MyGuiControlButton) null);
      if (!MyControllerHelper.GetControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON).IsNewReleased())
        return;
      this.OpenWorkshopButtonClicked((MyGuiControlButton) null);
    }

    protected override void Canceling()
    {
      base.Canceling();
      this.m_screenResult = VRage.Game.ModAPI.ResultEnum.CANCEL;
    }

    protected void CallResultCallback(VRage.Game.ModAPI.ResultEnum result)
    {
      if (this.m_resultCallback == null)
        return;
      this.m_resultCallback(result);
    }

    protected sealed class CompileProgramServer\u003C\u003ESystem_Byte\u003C\u0023\u003E : ICallSite<IMyEventOwner, byte[], DBNull, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in byte[] program,
        in DBNull arg2,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenEditor.CompileProgramServer(program);
      }
    }

    protected sealed class ReportCompilationResults\u003C\u003ESystem_Boolean\u0023System_Collections_Generic_List`1\u003CSystem_String\u003E : ICallSite<IMyEventOwner, bool, List<string>, DBNull, DBNull, DBNull, DBNull>
    {
      public virtual void Invoke(
        in IMyEventOwner _param1,
        in bool success,
        in List<string> messages,
        in DBNull arg3,
        in DBNull arg4,
        in DBNull arg5,
        in DBNull arg6)
      {
        MyGuiScreenEditor.ReportCompilationResults(success, messages);
      }
    }
  }
}
