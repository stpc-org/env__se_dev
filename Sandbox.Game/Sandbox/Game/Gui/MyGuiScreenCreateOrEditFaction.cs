// Decompiled with JetBrains decompiler
// Type: Sandbox.Game.Gui.MyGuiScreenCreateOrEditFaction
// Assembly: Sandbox.Game, Version=0.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 343E1F2F-C9E5-4CAA-B3CC-F9D203DAE0A8
// Assembly location: D:\Files\library_development\lib_se\Sandbox.Game.dll

using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;

namespace Sandbox.Game.Gui
{
  public class MyGuiScreenCreateOrEditFaction : MyGuiScreenBase
  {
    protected MyGuiControlTextbox m_shortcut;
    protected MyGuiControlTextbox m_name;
    protected MyGuiControlMultilineEditableText m_desc;
    protected MyGuiControlMultilineEditableText m_privInfo;
    protected MyGuiControlImage m_factionIcon;
    protected MyGuiControlImageButton m_editFactionIconBtn;
    protected SerializableDefinitionId m_factionIconGroupId;
    protected int m_factionIconId;
    protected Vector3 m_factionColor;
    protected Vector3 m_factionIconColor;
    protected IMyFaction m_editFaction;

    public MyGuiScreenCreateOrEditFaction(ref IMyFaction editData)
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.5857143f, 0.639313f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = false;
      this.m_editFaction = editData;
      this.RecreateControls(true);
    }

    public MyGuiScreenCreateOrEditFaction()
      : base(new Vector2?(new Vector2(0.5f, 0.5f)), new Vector4?(MyGuiConstants.SCREEN_BACKGROUND_COLOR), new Vector2?(new Vector2(0.5857143f, 0.639313f)), backgroundTransition: MySandboxGame.Config.UIBkOpacity, guiTransition: MySandboxGame.Config.UIOpacity)
    {
      this.CanHideOthers = false;
      this.EnabledBackgroundFade = false;
    }

    public void Init(ref IMyFaction editData)
    {
      this.m_editFaction = editData;
      this.RecreateControls(true);
    }

    public override string GetFriendlyName() => nameof (MyGuiScreenCreateOrEditFaction);

    public override void RecreateControls(bool constructor) => base.RecreateControls(constructor);

    protected virtual void OnOkClick(MyGuiControlButton sender)
    {
      int num = this.m_editFaction != null ? (this.m_editFaction.IsLeader(MySession.Static.LocalPlayerId) ? 1 : 0) : 1;
      bool flag = MySession.Static.IsUserAdmin(Sync.MyId);
      if (num == 0 && !flag)
      {
        this.ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsMissingRights));
        this.CloseScreenNow();
      }
      else
      {
        this.m_shortcut.Text = this.m_shortcut.Text.Replace(" ", string.Empty);
        this.m_name.Text = this.m_name.Text.Trim();
        if (this.m_shortcut.Text.Length != 3 && this.m_shortcut.Enabled)
          this.ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsTag));
        else if (MySession.Static.Factions.FactionTagExists(this.m_shortcut.Text, this.m_editFaction))
          this.ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsTagAlreadyExists));
        else if (this.m_name.Text.Length < 4)
          this.ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsNameTooShort));
        else if (MySession.Static.Factions.FactionNameExists(this.m_name.Text, this.m_editFaction))
          this.ShowErrorBox(MyTexts.Get(MyCommonTexts.MessageBoxErrorFactionsNameAlreadyExists));
        else if (this.m_editFaction != null)
        {
          MySession.Static.Factions.EditFaction(this.m_editFaction.FactionId, this.m_shortcut.Text, this.m_name.Text, this.m_desc.Text.ToString(), this.m_privInfo.Text.ToString(), new SerializableDefinitionId?(this.m_factionIconGroupId), this.m_factionIconId, this.m_factionColor, this.m_factionIconColor);
          this.CloseScreenNow();
        }
        else
        {
          MyFactionCollection factions = MySession.Static.Factions;
          long localPlayerId = MySession.Static.LocalPlayerId;
          string text1 = this.m_shortcut.Text;
          string text2 = this.m_name.Text;
          string desc = this.m_desc.Text.ToString();
          string privateInfo = this.m_privInfo.Text.ToString();
          SerializableDefinitionId? nullable = new SerializableDefinitionId?(this.m_factionIconGroupId);
          int factionIconId1 = this.m_factionIconId;
          Vector3 factionColor = this.m_factionColor;
          Vector3 factionIconColor = this.m_factionIconColor;
          SerializableDefinitionId? factionIconGroupId = nullable;
          int factionIconId2 = factionIconId1;
          factions.CreateFaction(localPlayerId, text1, text2, desc, privateInfo, MyFactionTypes.PlayerMade, factionColor, factionIconColor, factionIconGroupId, factionIconId2);
          this.CloseScreenNow();
        }
      }
    }

    protected void OnCancelClick(MyGuiControlButton sender) => this.CloseScreenNow();

    protected void ShowErrorBox(StringBuilder text)
    {
      StringBuilder messageCaption = MyTexts.Get(MyCommonTexts.MessageBoxCaptionError);
      MyGuiScreenMessageBox messageBox = MyGuiSandbox.CreateMessageBox(messageText: text, messageCaption: messageCaption);
      messageBox.SkipTransition = true;
      messageBox.CloseBeforeCallback = true;
      messageBox.CanHideOthers = false;
      MyGuiSandbox.AddScreen((MyGuiScreenBase) messageBox);
    }
  }
}
